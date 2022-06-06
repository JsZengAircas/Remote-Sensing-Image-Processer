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
    public partial class Image_Fusion : Form
    {
        private int processType;
        RSImage fusionImage = new RSImage();
        //融合波段
        public static  List<int> bandList_Msi = new List<int>();
        List<int> bandList_Pan = new List<int>();
        bool Msi_State = false;
        //创建两个dataset去存储两个影像
        Dataset data_Msi;
        Dataset data_Pan;
        public Image_Fusion(int type )
        {
            InitializeComponent();
            processType = type;
        }
        private struct ByteArr
        {
          public  int[] arr;
          public  int bandNo;
        }
        private void Image_Fusion_Load(object sender, EventArgs e)
        {
            switch (processType)
            {
                case 1:
                    Type_label.Text = "Brovey融合";
                    break;
                case 2:
                    Type_label.Text = "IHS融合";
                    break;

            }
            label_StateBar.Text = "就绪";
        }

        private void btn_msi_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            textBox_msi.Text = RSImage.ReadFileName();
            precheckedListBox.Items.Add(textBox_msi.Text);
            data_Msi = Gdal.Open(textBox_msi.Text, Access.GA_ReadOnly);
            fusionImage.ChooseBandList(data_Msi,fusionImage);
            label_StateBar.Text = "读取完毕！";
        }

        private void btn_pan_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            textBox_pan.Text = RSImage.ReadFileName();
            precheckedListBox.Items.Add(textBox_pan.Text);
            data_Pan = Gdal.Open(textBox_pan.Text, Access.GA_ReadOnly);
            bandList_Pan.Add(1);
            label_StateBar.Text = "读取完毕！";
            //  fusionImage.get_BandList = bandList_Pan;
        }

        private void btSaveFile_Click(object sender, EventArgs e)
        {
            textBox_saveFile.Text = RSImage.ShowSaveFileDialog();
           // precheckedListBox.Items.Add(textBox_saveFile.Text);
        }

        private void precheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            pix_Pre.Image = null;
            if (precheckedListBox.SelectedItem.ToString() == textBox_msi.Text)
            {
                fusionImage.get_BandList = bandList_Msi;
                pix_Pre.Image = fusionImage.GetImage(data_Msi, this.pix_Pre);
                label_StateBar.Text = "影像显示成功！";
            }
            else
            {
                fusionImage.get_BandList = bandList_Pan;
                pix_Pre.Image = fusionImage.GetImage(data_Pan, this.pix_Pre);
                label_StateBar.Text = "影像显示成功！";
            }           
                      
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
               Gdal.AllRegister();
               progressBar1.Value = 0;
                label_StateBar.Text = "开始融合：";
                if (Msi_State)
                {
                    bandList_Msi = fusionImage.get_BandList;
                }
                Dataset data_Msi = Gdal.Open(textBox_msi.Text, Access.GA_ReadOnly);
                Dataset data_Pan = Gdal.Open(textBox_pan.Text, Access.GA_ReadOnly);
                int bandCount = bandList_Msi.Count;

                //新建融合文件
                Driver Dri = Gdal.GetDriverByName("GTiff");
                Dataset outPut = Dri.Create(textBox_saveFile.Text, data_Pan.RasterXSize, data_Pan.RasterYSize, bandCount, DataType.GDT_UInt16, null);
            //给定地理坐标
            double[] fInTransForm1 = new double[6];
            data_Msi.GetGeoTransform(fInTransForm1);
            outPut.SetGeoTransform(fInTransForm1);
            outPut.SetProjection(outPut.GetProjection());

            progressBar1.Value = 15;
               label_StateBar.Text = "初始化成功！";
            int Width_msi = data_Msi.RasterXSize;
                int Height_msi = data_Msi.RasterYSize;
            if (processType == 1)
            {
                List<ByteArr> k = new List<ByteArr>();
                int co = 0;
                for (int i = 1; i < data_Msi.RasterCount; i++)
                {
                    int[] buffer_msi = new int[Width_msi * Height_msi];
                    data_Msi.GetRasterBand(bandList_Msi[co]).ReadRaster(0, 0, Width_msi, Height_msi, buffer_msi, Width_msi, Height_msi, 0, 0);
                    ByteArr Barr;
                    Barr.arr = buffer_msi;
                    Barr.bandNo = i;
                    k.Add(Barr);
                    co++;
                }
                int Width_pan = data_Pan.RasterXSize;
                int Height_pan = data_Pan.RasterYSize;
                int[] buffer_pan = new int[Width_pan * Height_pan];
                data_Pan.GetRasterBand(1).ReadRaster(0, 0, Width_pan, Height_pan, buffer_pan, Width_pan, Height_pan, 0, 0);
                int rowCount = 0, colCount = 0;
                int[] ko = new int[10];
                int index = 0;
                progressBar1.Value = 30;
                for (int i = 0; i < outPut.RasterCount; i++)
                {
                    rowCount = 0;
                    for (int row = 0; row < outPut.RasterXSize; row++)
                    {
                        if (row != 0 && (row % (Width_pan / Width_msi) == 0)) rowCount++;
                        colCount = 0;
                        for (int col = 0; col < outPut.RasterYSize; col++)
                        {
                            if (col != 0 && (col % (Width_pan / Width_msi) == 0))
                            {
                                colCount++;
                            }
                            int R = k[0].arr[rowCount * Width_msi + colCount];
                            int G = k[1].arr[rowCount * Width_msi + colCount];
                            int B = k[2].arr[rowCount * Width_msi + colCount];
                            int nowBandValue = k[i].arr[rowCount * Width_msi + colCount];
                            buffer_pan[Width_pan * row + col] = ((buffer_pan[Width_pan * row + col]) * nowBandValue) / (R + G + B);
                   
                        }

                    }
                    outPut.GetRasterBand(i + 1).WriteRaster(0, 0, Width_pan, Height_pan, buffer_pan, Width_pan, Height_pan, 0, 0);
                    index++;
                    progressBar1.Value += 15;
                }
                progressBar1.Value = 100;
                label_StateBar.Text = "融合完毕！";
                outPut.Dispose();
            }
            if (processType == 2)
            {
                outPut = ImageFusion.msi_Resample(data_Msi,data_Pan.RasterXSize/data_Msi.RasterXSize,fusionImage,outPut);
                progressBar1.Value = 30;
                label_StateBar.Text = "重采样已完成！";
                outPut = ImageFusion.Msi_RGB_To_IHS(outPut);
                progressBar1.Value = 45;
                label_StateBar.Text = "RGB已转换至IHS空间！";
                double outMin, outMax, outMean, outStdDev;
                Band band = outPut.GetRasterBand(1);
                band.GetStatistics(0, 1, out outMin, out outMax, out outMean, out outStdDev);
                int[] histData = new int[Convert.ToInt32(outMax)+1];//统计频数
                band.GetHistogram(outMin - 1.5, outMax + 1.5, Convert.ToInt32(outMax) + 1, histData, 0, 1, null, null);
                //获取全色影像的灰度频数
                double outMin_Pan, outMax_Pan, outMean_Pan, outStdDev_Pan;
                Band band_Pan = data_Pan.GetRasterBand(1);
                band_Pan.GetStatistics(0, 1, out outMin_Pan, out outMax_Pan, out outMean_Pan, out outStdDev_Pan);
                int[] histData_Pan = new int[Convert.ToInt32(outMax_Pan) + 1];//统计频数
                band_Pan.GetHistogram(outMin_Pan - 1.5, outMax_Pan + 1.5, Convert.ToInt32(outMax_Pan) + 1, histData_Pan, 0, 1, null, null);
                double total = outPut.RasterXSize * outPut.RasterYSize;
                progressBar1.Value = 75;
                label_StateBar.Text = "进行直方图匹配！";
                int[] lut = Radiation_Enhancement.HistMatching(histData,histData_Pan,total,total);
                int[] buffer_I = new int[outPut.RasterXSize * outPut.RasterYSize];
                outPut.GetRasterBand(1).ReadRaster(0,0, outPut.RasterXSize , outPut.RasterYSize,buffer_I, outPut.RasterXSize, outPut.RasterYSize,0,0);
                for (int i = 0; i < outPut.RasterYSize; i++)
                {
                    for (int j = 0; j < outPut.RasterXSize; j++)
                    {
                        buffer_I[i * outPut.RasterXSize + j] = lut[buffer_I[i * outPut.RasterXSize + j]];
                    }
                }
                outPut.GetRasterBand(1).WriteRaster(0, 0, outPut.RasterXSize, outPut.RasterYSize, buffer_I, outPut.RasterXSize, outPut.RasterYSize, 0, 0);
                progressBar1.Value = 90;
                label_StateBar.Text = "IHS空间转回RGB空间！";
                ImageFusion.IHS_To_RGB(outPut);
                outPut.Dispose();
                progressBar1.Value = 90;
                label_StateBar.Text = "结束！";
                progressBar1.Value = 100;
                MessageBox.Show("融合成功！","提示",MessageBoxButtons.OK);
            }
        }

        private void btn__Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
