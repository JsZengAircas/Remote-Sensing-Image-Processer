using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSGeo.GDAL;

namespace Test
{
    public partial class Image_Segmentation : Form
    {
        public Image_Segmentation(int caseType)
        {
            InitializeComponent();
            Type=caseType;
        }
        private  RSImage currentImage=new RSImage();
        private Dataset currentData;
        int Type;
        int fileCount_O = 1;
        int fileCount_I = 1;
        private void Image_Segmentation_Load(object sender, EventArgs e)
        {
            if (MainFrm.fileNameList.Count == 0)
            {
                srcComboBox.Text = "请点击按钮输入处理文件";
            }
            else
            {
                for (int i = 0; i < MainFrm.fileNameList.Count; i++)
                {
                    List<string> FileItem = new List<string>();
                    FileItem = MainFrm.fileNameList;
                    srcComboBox.Items.Add(FileItem[i]);
                }
            }
        }

        private void btSaveFile_Click(object sender, EventArgs e)
        {
            string file = RSImage.ReadFileName();
            if (file != null)
            {
                srcComboBox.Text =file;
            }       
        }

        private void srcComboBox_TextChanged(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            if (srcComboBox.Text != null && srcComboBox.Text != "请点击按钮输入处理文件")
            {
                Dataset srcData = Gdal.Open(srcComboBox.Text,Access.GA_ReadOnly);
                currentData = srcData;
                RSImage srcImage = new RSImage();
                srcImage.get_Width = 1;
                currentImage = srcImage;
                //srcImage.ChooseBandList(srcData, srcImage);
                for (int i = 0; i < srcData.RasterCount; i++)
                {
                    BandComboBox.Items.Add("Band_" +(i+1).ToString());
                }
            }
        }
        public void OstuSegmentation(Dataset src, Dataset output,int bandNum)
        {
            Gdal.AllRegister();
            
            OSGeo.GDAL.Band band = src.GetRasterBand(bandNum);//获取通道
            //设定参数,stdDev标准方差
            double outMin, outMax, outMean, outStdDev;
            band.GetStatistics(0, 1, out outMin, out outMax, out outMean, out outStdDev);//采集信息
            int[] histData = new int[(int)outMax];//直方图统计
            band.GetHistogram(outMin - 1.5, outMax + 1.5, (int)outMax, histData, 0, 1, null, null);
            int T = 0; //Otsu算法阈值 
            double varValue = 0; //类间方差中间值保存
            double w0 = 0; //前景像素点数所占比例  
            double w1 = 0; //背景像素点数所占比例  
            double u0 = 0; //前景平均灰度  
            double u1 = 0; //背景平均灰度
            double totalNum =src.RasterYSize*src.RasterXSize; //像素总数
            int minpos=0, maxpos=0;
            for (int i = 0; i < histData.Length; i++)
            {
                if (histData[i] != 0)
                {
                    minpos = i;
                    break;
                }
            }
            for (int i = histData.Length-1; i > 0; i--)
            {
                if (histData[i] != 0)
                {
                    maxpos = i;
                    break;
                }
            }
            for (int i = minpos; i <= maxpos; i++)
            {
                //每次遍历之前初始化各变量  
                w1 = 0; u1 = 0; w0 = 0; u0 = 0;
                //***********背景各分量值计算**************************  
                for (int j = 0; j <= i; j++) //背景部分各值计算  
                {
                    w1 += histData[j];   //背景部分像素点总数  
                    u1 += j * histData[j]; //背景部分像素总灰度和  
                }
                if (w1 == 0) //背景部分像素点数为0时退出  
                {
                    break;
                }
                u1 = u1 / w1; //背景像素平均灰度  
                w1 = w1 / totalNum; // 背景部分像素点数所占比例
               //***********背景各分量值计算**************************  

                //***********前景各分量值计算**************************  
                for (int k = i + 1; k < histData.Length; k++)
                {
                    w0 += histData[k];  //前景部分像素点总数  
                    u0 += k *histData[k]; //前景部分像素总灰度和  
                }
                if (w0 == 0) //前景部分像素点数为0时退出  
                {
                    break;
                }
                u0 = u0 / w0; //前景像素平均灰度  
                w0 = w0 / totalNum; // 前景部分像素点数所占比例  
             //***********前景各分量值计算**************************  

              //***********类间方差计算******************************  
                double varValueI = w0 * w1 * (u1 - u0) * (u1 - u0); //当前类间方差计算  
                if (varValue < varValueI)
                {
                    varValue = varValueI;
                    T = i;
                }
            }
            int[] buffer_Pix = new int[src.RasterXSize*src.RasterYSize];
            src.GetRasterBand(bandNum).ReadRaster(0,0,src.RasterXSize,src.RasterYSize,buffer_Pix, src.RasterXSize, src.RasterYSize,0,0);
            for (int row = 0; row < output.RasterYSize; row++)
            {
                for (int col = 0; col < output.RasterXSize; col++)
                {
                    if (buffer_Pix[row * src.RasterXSize + col] > T)
                    {
                        buffer_Pix[row * src.RasterXSize + col] = 255;
                    }
                    else {
                        buffer_Pix[row * src.RasterXSize + col] = 1;
                    }
                }
            }
            output.GetRasterBand(1).WriteRaster(0, 0, output.RasterXSize, output.RasterYSize, buffer_Pix, output.RasterXSize, output.RasterYSize, 0, 0);
            //output.Dispose();
        }
        public void IterationThreshold(Dataset src, Dataset output, int bandNum)
        {
            Gdal.AllRegister();           
            OSGeo.GDAL.Band band = src.GetRasterBand(bandNum);//获取通道
            //设定参数,stdDev标准方差
            double outMin, outMax, outMean, outStdDev;
            band.GetStatistics(0, 1, out outMin, out outMax, out outMean, out outStdDev);//采集信息
            int[] hisData = new int[(int)outMax];//直方图统计
            band.GetHistogram(outMin - 1.5, outMax + 1.5, (int)outMax, hisData, 0, 1, null, null);
            int T0 = 0;
            for (int i = 0; i < hisData.Length; i++)
            {
                T0 += i * hisData[i];
            }
            //所有灰度的均值
            T0 /= src.RasterXSize*src.RasterYSize;

            int T1 = 0, T2 = 0;
            int num1 = 0, num2 = 0;
            int T = 0;
            while (true)
            {
                for (int i = 0; i < T0 + 1; i++)
                {
                    T1 += i * hisData[i];
                    num1 += hisData[i];
                }
                if (num1 == 0)
                    continue;
                for (int i = T0 + 1; i < hisData.Length; i++)
                {
                    T2 += i * hisData[i];
                    num2 += hisData[i];
                }
                if (num2 == 0)
                    continue;

                T = (T1 / num1 + T2 / num2) / 2;

                if ((T - T0)<0.0000001)
                    break;
                else
                    T0 = T;
            }
            int[] buffer_Pix = new int[src.RasterXSize * src.RasterYSize];
            src.GetRasterBand(bandNum).ReadRaster(0, 0, src.RasterXSize, src.RasterYSize, buffer_Pix, src.RasterXSize, src.RasterYSize, 0, 0);
            for (int row = 0; row < output.RasterYSize; row++)
            {
                for (int col = 0; col < output.RasterXSize; col++)
                {
                    if (buffer_Pix[row * src.RasterXSize + col] > T0)
                    {
                        buffer_Pix[row * src.RasterXSize + col] = 255;
                    }
                    else
                    {
                        buffer_Pix[row * src.RasterXSize + col] = 1;
                    }
                }
            }
            output.GetRasterBand(1).WriteRaster(0, 0, output.RasterXSize, output.RasterYSize, buffer_Pix, output.RasterXSize, output.RasterYSize, 0, 0);
           // output.Dispose();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            string file = BandComboBox.Text;
            int bandNum = Convert.ToInt32(HistogramForm.GetBandNum(file));
            currentImage.bandListIni();
            currentImage.get_BandList.Add(1);
            Dataset re;
            if (Type == 1)
            {
                string saveFileName = srcComboBox.Text.Remove(srcComboBox.Text.Length - 4, 4) + "_Ostu" + (fileCount_O++).ToString() + ".tif";
                Driver dri = Gdal.GetDriverByName("Gtiff");
                re = dri.Create(saveFileName, currentData.RasterXSize, currentData.RasterYSize, 1, DataType.GDT_Byte, null);
                OstuSegmentation(currentData, re, bandNum);
            }
            else {
                string saveFileName = srcComboBox.Text.Remove(srcComboBox.Text.Length - 4, 4) + "_Iteration" + (fileCount_I++).ToString() + ".tif";
                Driver dri = Gdal.GetDriverByName("Gtiff");
                re = dri.Create(saveFileName, currentData.RasterXSize, currentData.RasterYSize, 1, DataType.GDT_Byte, null);
                IterationThreshold(currentData, re, bandNum);
            }
            pictureBox1.Image= currentImage.GetImage(re, pictureBox1);
            re.Dispose();
            MessageBox.Show("成功！","提示",MessageBoxButtons.OK);
            
        }
    }
}
