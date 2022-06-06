using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gdal = OSGeo.GDAL.Gdal;
using Ogr = OSGeo.OGR.Ogr;
using OSGeo;
using System.Drawing.Drawing2D;
using WeifenLuo.WinFormsUI.Docking;
using OSGeo.GDAL;

namespace Test
{
    public partial class RadiationCorr : Form
    {
        public RadiationCorr()
        {
            InitializeComponent();
        }
        //用于存储更改的参数
        public List<double> gainValue = new List<double>();
        public List<double> offsetValue = new List<double>();
        
        private void RadiationCorr_Load(object sender, EventArgs e)
        {
            if (MainFrm.fileNameList.Count == 0)
            {
                MessageBox.Show("请打开相应文件", "提示",MessageBoxButtons.OK);
                okButton.Enabled = false;
                cancelButton.Enabled = false;
            }
            else {
                for (int i = 0; i < MainFrm.fileNameList.Count; i++)
                {
                    List<string> FileItem = new List<string>();
                    FileItem = MainFrm.fileNameList;
                    FilecomboBox.Items.Add(FileItem[i]);
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FileName = FilecomboBox.Text;
            Gdal.AllRegister();

            OSGeo.GDAL.Dataset dataset = Gdal.Open(FileName, OSGeo.GDAL.Access.GA_ReadOnly);//读取文件

            if (dataset != null)
            {
                gainComboBox.Items.Clear();//清除combox
                offsetComboBox.Items.Clear();
                for (int i = 0; i < dataset.RasterCount; i++)//输入波段信息
                {
                    string name = "Band_" + (i + 1).ToString();
                    gainComboBox.Items.Add(name);
                    offsetComboBox.Items.Add(name);
                }
            }
           //初始化
            gainValue = InitialList(dataset.RasterCount, 1);
            offsetValue = InitialList(dataset.RasterCount,0.5);
            SetTextValues(gainRichTextBox, offsetRichTextBox, FileName);
        }
        public void SetTextValues(RichTextBox Gain,RichTextBox Offset,string FileName)
        {
            Gdal.AllRegister();

            OSGeo.GDAL.Dataset dataset = Gdal.Open(FileName, OSGeo.GDAL.Access.GA_ReadOnly);//读取文件
            //清除初始内容
            Gain.Clear();
            Offset.Clear();

            if (dataset != null)
            {
               //comboBox1.Items.Clear();//清除combox
                for (int i = 0; i < dataset.RasterCount; i++)//输入波段信息
                {
                    string name = "Band_" + (i + 1).ToString();
                    Gain.Text = Gain.Text + name + "  " + gainValue[i].ToString("F4") + System.Environment.NewLine;
                    Offset.Text = Offset.Text + name + "  " + offsetValue[i].ToString("F4") + System.Environment.NewLine;
                }
            }
        }
        //初始化列表
        public List<double> InitialList(int length,double iniSet)
        {
            List<double> temp = new List<double>();
            for (int i = 0; i < length; i++)
                temp.Add(iniSet);
            return temp;

        }
        //更改列表
        public List<double> ChangeList(List<double> Value,string combtext,string text )
        {

            List<double> Item = new List<double>();
            Item = Value;
            int n = Convert.ToInt32(combtext.Remove(0,5));//获取最后一个字符的索引
            Value[n - 1] = Convert.ToDouble(text);
            return Value;
        }
        //辐射定标
        private void okButton_Click(object sender, EventArgs e)
        {
            if (saveFileTextBox.Text != "")
            {
                //新建空白栅格文件
                //注册
                Gdal.AllRegister();
                string inputFileName = FilecomboBox.Text;
                string outputFileName = saveFileTextBox.Text;
                //读取辐射定标文件
                OSGeo.GDAL.Dataset inputDataset = Gdal.Open(inputFileName, Access.GA_ReadOnly);
                //按行进行计算
                //新建输出影像文件，以第一个输入文件为基准，生成空白栅格图像
                //默认格式
                OSGeo.GDAL.Driver pCreateDriver=Gdal.GetDriverByName("GTiff"); ;
                string fileSuff = getFileSuffix(outputFileName);

                //按照要求选择不一样的文件格式
                switch (fileSuff)
                {
                    case "tif": {                            
                            pCreateDriver.Register();
                            break; }
                    case "img": {
                            pCreateDriver = Gdal.GetDriverByName("HFA");
                            pCreateDriver.Register();
                            break;
                        }
                    case "jpg": {
                            pCreateDriver = Gdal.GetDriverByName("jpeg");
                            pCreateDriver.Register();
                            break;
                        }
                    case "png":
                        {
                            pCreateDriver = Gdal.GetDriverByName("png");
                            pCreateDriver.Register();
                            break;
                        }
                    default : { break; }
                }
                

                //获取图像的大小
                int InWidth = inputDataset.RasterXSize;
                int InHeight = inputDataset.RasterYSize;

                //获取通道
                Band inputBand = inputDataset.GetRasterBand(1);

                //创建输出dataset
                OSGeo.GDAL.Dataset outputDataSet = pCreateDriver.Create(outputFileName, inputDataset.RasterXSize, inputDataset.RasterYSize, inputDataset.RasterCount, DataType.GDT_UInt16, null);

                int[] bufferArr = new int[InWidth];
                int bandCount = inputDataset.RasterCount;
                for (int i = 1; i < bandCount + 1; i++)
                {
                    for (int row = 0; row < InHeight; row++)
                    {
                        //读入栅格数据
                        inputDataset.GetRasterBand(i).ReadRaster(0, row, InWidth, 1, bufferArr, InWidth, 1, 0, 0);
                        //定标计算
                        for (int n = 0; n < InWidth; n++)
                            bufferArr[n] = (int)(bufferArr[n] * gainValue[i-1] + offsetValue[i-1]);
                        outputDataSet.GetRasterBand(i).WriteRaster(0, row, InWidth, 1, bufferArr, InWidth, 1, 0, 0);
                    }
                }
                MessageBox.Show("Succeed！", "Message", MessageBoxButtons.OK);
                outputDataSet.Dispose();
                this.Close();
            }
            else {
                MessageBox.Show("请输入存储路径！", "提示", MessageBoxButtons.YesNo);
            }

        }
       
       
        //回车键修改值
        private void gainTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyValue==13)
            {
                gainValue = ChangeList(gainValue, gainComboBox.Text, gainTextBox1.Text);
                
                SetTextValues(gainRichTextBox, offsetRichTextBox, FilecomboBox.Text);
            }           
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                offsetValue = ChangeList(offsetValue, offsetComboBox.Text,textBox2.Text);

                SetTextValues(gainRichTextBox, offsetRichTextBox, FilecomboBox.Text);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("取消当前操作？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
            else {

            }
         
        }

        private void btSaveFile_Click(object sender, EventArgs e)
        {
                saveFileTextBox.Text =RSImage.ShowSaveFileDialog();
        }

        //选择保存路径
      
        public string getFileSuffix(string fullFilePath)
        {
            string fileSuffix = "";
            int n = fullFilePath.LastIndexOf(".");
            if (n >= 0)
            {
                fileSuffix = fullFilePath.Remove(0, n + 1);
            }
            else
            {
                fileSuffix = fullFilePath;
            }
            return fileSuffix;
        }

    }
}
