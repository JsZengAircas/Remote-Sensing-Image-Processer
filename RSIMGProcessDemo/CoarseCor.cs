using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSGeo.OGR;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Test
{
    public partial class CoarseCor : Form
    {
        public CoarseCor()
        {
            InitializeComponent();
        }
        RSImage imageParma = new RSImage();
        public double evelation = 45;
        private void CoarseCor_Load(object sender, EventArgs e)
        {
            statusStrip1.Text = "就绪";
        }
        double[] fInTransForm1 = new double[6];
        private void button1_Click(object sender, EventArgs e)
        {
            if (MainFrm.CurentImage != null)
            {
                //获取传感器的基本参数
                OSGeo.GDAL.Dataset dataset = OSGeo.GDAL.Gdal.Open(MainFrm.CurentImage, OSGeo.GDAL.Access.GA_ReadOnly);

                imageParma.get_Height = dataset.RasterYSize;
                imageParma.get_Width = dataset.RasterXSize;

                imageParma.get_SensorWidth = Convert.ToDouble(tBox_SensorWidth.Text)* Math.Pow(10,-6);
                imageParma.get_SensorHeight = Convert.ToDouble(tBox_SensorHeight.Text) * Math.Pow(10, -6);
                //获取像点的经纬坐标

                //外方位元素
                double[] externalElement = new double[6];
                // longitude 
                externalElement[0] = Convert.ToDouble(textBox1.Text);
                // latitude
                externalElement[1] = Convert.ToDouble(textBox2.Text);
                // elevation
                externalElement[2] = Convert.ToDouble(textBox3.Text);
                // α横滚
                externalElement[3] = Convert.ToDouble(textBox6.Text);
                //β俯仰
                externalElement[4] = Convert.ToDouble(textBox5.Text);
                // κ航向
                externalElement[5] =  Convert.ToDouble(textBox4.Text);

                //获取UTM坐标
                double[] utm = imageParma.lonLai_Convert_UTM(externalElement[0], externalElement[1]);
                //double[] XYZ = imageParma.project(externalElement[0]* (Math.PI / 180.0), externalElement[1]*(Math.PI / 180.0));

                //externalElement[0] = XYZ[1];//东向
                //externalElement[1] = XYZ[0];//北向
                externalElement[1] = utm[0];
                externalElement[0] = utm[1];
                //传值
                imageParma.get_ExternalEle = externalElement;

                //内方位元素
                double[] internalEle = new double[3];
                //x
                internalEle[0] = Convert.ToDouble(textBox7.Text) * 0.001;
                //y
                internalEle[1] = Convert.ToDouble(textBox8.Text) * 0.001;
                //f
                internalEle[2] = Convert.ToDouble(textBox9.Text) * 0.001;

                imageParma.get_internalEle = internalEle;
                imageParma.get_Resolution_Ratio = Convert.ToDouble(textBox11.Text);

                imageParma.avgHeight = Convert.ToDouble(textBox12.Text);


                double[,] res = CalRange();
                double[] _res = get_MaxMinRange(res);

                int[] kk = get_Input_RowCol(_res);

                CoarseCal(kk, _res,res);
            }
            else { MessageBox.Show("请输入影像","提示",MessageBoxButtons.OK); }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageParma.get_FileIndex = RSImage.ShowSaveFileDialog();
            textBox10.Text = imageParma.get_FileIndex;
        }
        public double[,] CalRange()
        {
            double[,] MaxMin = new double[4, 2];
            MaxMin[0, 0] = 0;
            MaxMin[0, 1] = 0;
            MaxMin[1, 0] = Convert.ToDouble((imageParma.get_Width));
            MaxMin[1, 1] = 0;
            MaxMin[2, 0] = 0;
            MaxMin[2, 1] = Convert.ToDouble(((imageParma.get_Height ))) ;
            MaxMin[3, 0] = Convert.ToDouble((imageParma.get_Width)) ;
            MaxMin[3, 1] = Convert.ToDouble(((imageParma.get_Height)));

            for (int i = 0; i < 4; i++)
            {
                //double[] temp = imageParma.ImgxyToGcpXYByColline(MaxMin[i, 0], MaxMin[i, 1],50);
                double[] temp = imageParma.pixelToGeoCor(MaxMin[i, 0], MaxMin[i, 1]);
                MaxMin[i, 0] = temp[0];
                MaxMin[i, 1] = temp[1];
            }
            return MaxMin;
        }
        /// <summary>
        /// 获取边界点的最大最小值
        /// </summary>
        /// <param name="inputRange"></param>
        /// <returns></returns>
        public double[] get_MaxMinRange(double[,] inputRange)
        {
            double[] res = new double[4];
            double Max_x = 0;
            double Min_x = inputRange[0, 0];
            double Max_y = 0;
            double Min_y = inputRange[0, 1];
            for (int i = 0; i < 4; i++)
            {
                if (inputRange[i, 0] > Max_x)
                {
                    Max_x = inputRange[i, 0];
                }
                if (inputRange[i, 0] < Min_x)
                {
                    Min_x = inputRange[i, 0];
                }
            }
            for (int i = 0; i < 4; i++)
            {

                if (inputRange[i, 1] > Max_y)
                {
                    Max_y = inputRange[i, 1];

                }
                if (inputRange[i, 1] < Min_y)
                {
                    Min_y = inputRange[i, 1];

                }
            }
            res[0] = Max_x;
            res[1] = Min_x;
            res[2] = Max_y;
            res[3] = Min_y;

            fInTransForm1[0] = Max_x;
            fInTransForm1[3] = Min_y;
            return res;
        }

        public int[] get_Input_RowCol(double[] input)
        {
            int[] res = new int[2];
            res[1] = Convert.ToInt32((input[0] - input[1]) / imageParma.get_Resolution_Ratio);//列号
            res[0] = Convert.ToInt32((input[2] - input[3]) / imageParma.get_Resolution_Ratio);//行号
            return res;
        }
        public int[] CorTransPixel(double X, double Y)
        {
            int[] RowCol = new int[2];
            double[,] a = imageParma.get_RoMatlab;
            double delt_X = X - imageParma.get_ExternalEle[0];
            double delt_Y = Y - imageParma.get_ExternalEle[1];
            double delt_Z = imageParma.evelation - imageParma.get_ExternalEle[2];
            double x_Pixel = -imageParma.get_internalEle[2] * (a[0, 0] * delt_X + a[1, 0] * delt_Y + a[2, 0] * delt_Z) / (a[0,2] * delt_X + a[1, 2] * delt_Y + a[2, 2] * delt_Z);
            double y_Pixel = -imageParma.get_internalEle[2] * (a[0, 1] * delt_X + a[1, 1] * delt_Y + a[2, 1] * delt_Z) / (a[0, 2] * delt_X + a[1, 2] * delt_Y + a[2, 2] * delt_Z);
            RowCol = imageParma.Pixel_TransferTo_RowCol(x_Pixel, y_Pixel);
            return RowCol;
        }

        public void CoarseCal(int[] RowCol, double[] MaxMin,double[,] range)
        {
            statusStrip1.Text = "处理进程：";
            OSGeo.GDAL.Driver Dri = OSGeo.GDAL.Gdal.GetDriverByName("GTiff");
            //读取原图像
            OSGeo.GDAL.Dataset orignalDataset = OSGeo.GDAL.Gdal.Open(MainFrm.CurentImage, OSGeo.GDAL.Access.GA_ReadOnly);
            int band = orignalDataset.RasterCount;
            OSGeo.GDAL.Dataset outputDataSet = Dri.Create(textBox10.Text, RowCol[1], RowCol[0], band, OSGeo.GDAL.DataType.GDT_UInt32, null);
            //设置左上点坐标
            int outWidth = outputDataSet.RasterXSize;int outHeight = outputDataSet.RasterYSize;
            int InWidth = orignalDataset.RasterXSize;int InHeight = orignalDataset.RasterYSize;progressBar1.Value = 0;
            int step = 100 / orignalDataset.RasterCount;int[] orignal_Buffer = new int[InWidth * InHeight];//先读取每个波段的像元            
            for (int i = 1; i < orignalDataset.RasterCount + 1; i++)
            {
                progressBar1.Value = step * (i);
                int[] res_Buffer = new int[outHeight * outWidth];
                
                orignalDataset.GetRasterBand(i).ReadRaster(0, 0, InWidth, InHeight, orignal_Buffer, InWidth, InHeight, 0, 0);

                  for (int row = 0; row < outputDataSet.RasterYSize; row++)
                  {                
                      for (int col = 0; col < outputDataSet.RasterXSize; col++)
                      {
                          //首先挨个读取粗校正的每个像元的真实坐标，计算出他们在像平面坐标系中的坐标x,y
                          //再反算至行列坐标
                          double X_Cor = (MaxMin[1]) +imageParma.get_Resolution_Ratio * (row);
                          double Y_Cor = (MaxMin[2]) - imageParma.get_Resolution_Ratio * col;
                          double   x = -imageParma.get_internalEle[2] * (imageParma.get_RoMatlab[0, 0] * (X_Cor - imageParma.get_ExternalEle[0]) + imageParma.get_RoMatlab[1, 0] 
                            * (Y_Cor- imageParma.get_ExternalEle[1]) + imageParma.get_RoMatlab[2, 0] * (evelation - imageParma.get_ExternalEle[2]))
                            / (imageParma.get_RoMatlab[0, 2] * (X_Cor - imageParma.get_ExternalEle[0]) + imageParma.get_RoMatlab[1, 2] * (Y_Cor- imageParma.get_ExternalEle[1]) 
                            + imageParma.get_RoMatlab[2, 2] * (evelation - imageParma.get_ExternalEle[2]));
                          double   y = -imageParma.get_internalEle[2] * (imageParma.get_RoMatlab[0, 1] * (X_Cor - imageParma.get_ExternalEle[0]) + imageParma.get_RoMatlab[1, 1] 
                            * (Y_Cor- imageParma.get_ExternalEle[1]) + imageParma.get_RoMatlab[2, 1] * (evelation - imageParma.get_ExternalEle[2])) 
                            / (imageParma.get_RoMatlab[0, 2] * (X_Cor - imageParma.get_ExternalEle[0]) + imageParma.get_RoMatlab[1, 2] * (Y_Cor- imageParma.get_ExternalEle[1]) 
                            + imageParma.get_RoMatlab[2, 2] * (evelation - imageParma.get_ExternalEle[2]));                     
                          int  newR=(int)((x+imageParma.get_internalEle[0])/imageParma.get_SensorHeight);int  newC= (int)((imageParma.get_internalEle[1]-y) / imageParma.get_SensorHeight);
                        if (newR < imageParma.get_Height && newC < imageParma.get_Width && (newR >=0) && (newC >= 0))
                          {res_Buffer[row*outWidth+col] = orignal_Buffer[newR* InWidth + newC];}
                          else {res_Buffer[row * outWidth + col] = 0;}
                      }              
                  }
                outputDataSet.GetRasterBand(i).WriteRaster(0, 0, outputDataSet.RasterXSize, outputDataSet.RasterYSize, res_Buffer, outputDataSet.RasterXSize, outputDataSet.RasterYSize, 0, 0);
            }

            progressBar1.Value = 100; outputDataSet.Dispose();MessageBox.Show("Success!", "提示", MessageBoxButtons.OK);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
           
        }
        
    }
}
