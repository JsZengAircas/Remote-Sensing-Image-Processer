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
    public partial class BandChooseFrm : Form
    {
        private Dataset nowData;
        public   List<int> band = new List<int>();
        private RSImage nowImage;
        public BandChooseFrm(Dataset data,RSImage ima)
        {
            InitializeComponent();
            nowData = data;
            nowImage = ima;
        }

        private void BandChooseFrm_Load(object sender, EventArgs e)
        {
            if (nowData != null)
            {
                comboBox_R.Items.Clear();//清除combox
                comboBox_G.Items.Clear();//清除combox
                comboBox_B.Items.Clear();//清除combox
                for (int i = 0; i < nowData.RasterCount; i++)//输入波段信息
                {
                    string name = "Band_" + (i + 1).ToString();
                    comboBox_R.Items.Add(name);
                    comboBox_G.Items.Add(name);
                    comboBox_B.Items.Add(name);                  
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            band.Clear();     
            band.Add(Convert.ToInt32(comboBox_R.Text.Remove(0, comboBox_R.Text.LastIndexOf("_")+1)));
            band.Add(Convert.ToInt32(comboBox_G.Text.Remove(0, comboBox_G.Text.LastIndexOf("_") + 1)));
            band.Add(Convert.ToInt32(comboBox_B.Text.Remove(0, comboBox_B.Text.LastIndexOf("_") + 1)));
            Image_Fusion.bandList_Msi= band;
            nowImage.bandListIni();
            nowImage.get_BandList = band;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
