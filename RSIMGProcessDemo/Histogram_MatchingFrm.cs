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
    public partial class Histogram_MatchingFrm : Form
    {
        public Histogram_MatchingFrm()
        {
            InitializeComponent();
        }
        RSImage srcImage = new RSImage();
        RSImage matchingImage = new RSImage();
        RSImage dstImage = new RSImage();
        Dataset srcDataSet; Dataset matchingDataSet; Dataset dstDataSet;
        String srcFile, matchingFile, dstFile;
        private void Histogram_MatchingFrm_Load(object sender, EventArgs e)
        {
            if (MainFrm.fileNameList.Count == 0)
            {
                MessageBox.Show("请在主界面输入文件！", "错误");
                ok_Button.Enabled = false;
                cancel_Button.Enabled = false;
            }
            else
            {
                for (int i = 0; i < MainFrm.fileNameList.Count; i++)
                {
                    List<string> FileItem = new List<string>();
                    FileItem = MainFrm.fileNameList;
                    oFilecomboBox.Items.Add(FileItem[i]);
                    mFilecomboBox.Items.Add(FileItem[i]);
                }
            }
        }

        private void mFilecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            matchingFile = mFilecomboBox.Text;
            if (matchingFile != null)
            {
                matchingDataSet = Gdal.Open(matchingFile, Access.GA_ReadOnly);
                matchingImage.ChooseBandList(matchingDataSet, matchingImage);
                // matchingImage.GetImage(matchingDataSet, pictureBox1);
            }
        }

        private void oFilecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            srcFile = oFilecomboBox.SelectedItem.ToString();
            if (srcFile != null)
            {
                srcDataSet = Gdal.Open(srcFile, Access.GA_Update);
                srcImage.ChooseBandList(srcDataSet, srcImage);
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Driver dri = Gdal.GetDriverByName("GTiff");
            
            dstFile = srcFile.Remove(srcFile.Length-4,4)+"_HisMatching.tif";
            dstDataSet= dri.CreateCopy(dstFile,srcDataSet,1,null,null,null);
            double src_Total = srcDataSet.RasterXSize * srcDataSet.RasterYSize;
            double matching_Total = matchingDataSet.RasterYSize * matchingDataSet.RasterXSize;
            for (int i = 1; i < srcDataSet.RasterCount + 1; i++)
            {
                double outMin, outMax, outMean, outStdDev;
                Band band = srcDataSet.GetRasterBand(i);
                band.GetStatistics(0, 1, out outMin, out outMax, out outMean, out outStdDev);
                int[] histData = new int[Convert.ToInt32(outMax) + 1];//统计频数
                band.GetHistogram(outMin - 1.5, outMax + 1.5, Convert.ToInt32(outMax) + 1, histData, 0, 0, null, null);
                //获取全色影像的灰度频数
                double outMin_Matching, outMax_Matching, outMean_Matching, outStdDev_Matching;
                Band band_Pan = matchingDataSet.GetRasterBand(1);
                band_Pan.GetStatistics(0, 1, out outMin_Matching, out outMax_Matching, out outMean_Matching, out outStdDev_Matching);
                int[] histData_Matching = new int[Convert.ToInt32(outMax_Matching) + 1];//统计频数
                band_Pan.GetHistogram(outMin_Matching - 1.5, outMax_Matching + 1.5, Convert.ToInt32(outMax_Matching) + 1, histData_Matching, 0, 0, null, null);
                int[] lut = Radiation_Enhancement.HistMatching(histData,histData_Matching,src_Total,matching_Total);
                int[] buffer_I = new int[dstDataSet.RasterXSize * dstDataSet.RasterYSize];
                dstDataSet.GetRasterBand(1).ReadRaster(0, 0, dstDataSet .RasterXSize, dstDataSet .RasterYSize, buffer_I, dstDataSet .RasterXSize, dstDataSet .RasterYSize, 0, 0);
                for (int row = 0; row < dstDataSet.RasterYSize; row++)
                {
                    for (int col = 0; col <dstDataSet.RasterXSize; col++)
                    {
                        buffer_I[row * dstDataSet .RasterXSize + col] = lut[buffer_I[row * dstDataSet.RasterXSize + col]];
                    }
                }
               dstDataSet .GetRasterBand(i).WriteRaster(0, 0, dstDataSet .RasterXSize, dstDataSet .RasterYSize, buffer_I,dstDataSet .RasterXSize, dstDataSet.RasterYSize, 0, 0);
            }       
            List<int> bandList = new List<int>();
            bandList.Add(3);
            bandList.Add(2);
            bandList.Add(1);
            dstImage.get_BandList = bandList;
            pictureBox1.Image = dstImage.GetImage(dstDataSet,pictureBox1);
            dstDataSet.Dispose();
        }
    }
}
