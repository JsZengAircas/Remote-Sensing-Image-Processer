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
    public partial class Segement_Application : Form
    {
        public Segement_Application()
        {
            InitializeComponent();
        }
        RSImage apply_Image = new RSImage();
        Dataset beforData, afterData,output;
        private void btSaveFile_Click(object sender, EventArgs e)
        {
            textBox1.Text = RSImage.ReadFileName();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = RSImage.ReadFileName();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = RSImage.ShowSaveFileDialog();
        }

        private void Segement_Application_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null)
            {
               Gdal.AllRegister();
                beforData = Gdal.Open(textBox1.Text, Access.GA_ReadOnly);
               // apply_Image.ChooseBandList(beforData, apply_Image);
                for (int i = 0; i < beforData.RasterCount; i++)
                {
                    BandComboBox.Items.Add("Band_" + (i+1).ToString());
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int bandNum = Convert.ToInt32(HistogramForm.GetBandNum(BandComboBox.Text));
                afterData = Gdal.Open(textBox2.Text, Access.GA_ReadOnly);
                string saveFile = textBox3.Text;
                Driver dri = Gdal.GetDriverByName("GTiff");
                output = dri.Create(saveFile, beforData.RasterXSize, beforData.RasterYSize, 1, DataType.GDT_Byte, null);

                double[] fInTransForm1 = new double[6];
                beforData.GetGeoTransform(fInTransForm1);
                output.SetGeoTransform(fInTransForm1);
                output.SetProjection(beforData.GetProjection());

                int[] beforBuffer = new int[beforData.RasterXSize * beforData.RasterYSize];
                beforData.GetRasterBand(bandNum).ReadRaster(0, 0, beforData.RasterXSize, beforData.RasterYSize, beforBuffer, beforData.RasterXSize, beforData.RasterYSize, 0, 0);
                int[] afterBuffer = new int[afterData.RasterXSize * afterData.RasterYSize];
                afterData.GetRasterBand(bandNum).ReadRaster(0, 0, afterData.RasterXSize, afterData.RasterYSize, afterBuffer, afterData.RasterXSize, afterData.RasterYSize, 0, 0);
                for (int row = 0; row < beforData.RasterYSize; row++)
                {
                    for (int col = 0; col < beforData.RasterXSize; col++)
                    {
                        beforBuffer[row * beforData.RasterXSize + col] = Math.Abs(beforBuffer[row * beforData.RasterXSize + col] - afterBuffer[row * beforData.RasterXSize + col]);
                    }
                }
                output.GetRasterBand(1).WriteRaster(0, 0, output.RasterXSize, output.RasterYSize, beforBuffer, output.RasterXSize, output.RasterYSize, 0, 0);
                Image_Segmentation IS = new Image_Segmentation(1);
                IS.OstuSegmentation(output, output, 1);
                apply_Image.bandListIni();
                apply_Image.ChooseBandList(output, apply_Image);
                pictureBox1.Image = apply_Image.GetImage(output, pictureBox1);
                output.Dispose();
                MessageBox.Show("Success!", "提示", MessageBoxButtons.OK);
            }
            catch { }
        }
    }
}
