using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.K_Means;
using Test.Unsupervised_Classification;

namespace Test
{
    public partial class ISODataFrm : Form
    {
        public ISODataFrm()
        {
            InitializeComponent();
        }
        ISOData ISO = new ISOData();
        RSImage ISO_Image = new RSImage();
        string fileName,colorFile;
        string saveFileName;
        Dataset outputData,colorData,oriData;
        private void ISODataFrm_Load(object sender, EventArgs e)
        {
            if (MainFrm.fileNameList.Count == 0)
            {
                fileComboBox.Text = "请输入文件";
            }
            else
            {
                for (int i = 0; i < MainFrm.fileNameList.Count; i++)
                {
                    List<string> FileItem = new List<string>();
                    FileItem = MainFrm.fileNameList;
                    fileComboBox.Items.Add(FileItem[i]);
                    fileComboBox.Items.Add(FileItem[i]);
                }
            }
            this.Size =new Size(370, 570);
        }
        K_MeansFrm Kfrm = new K_MeansFrm();
        private void button1_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            ISO.classNumMin = Convert.ToInt32(numericUpDown1.Value);
            ISO.classNumMax = Convert.ToInt32(numericUpDown2.Value);
            ISO.maxItreNum = Convert.ToDouble(numericUpDown3.Value);
            ISO.minPixInClass = Convert.ToInt32(numericUpDown4.Value);
            ISO.maxStdvInClass = Convert.ToInt32(textBox2.Text);
            ISO.minDisInClass = Convert.ToInt32(textBox3.Text);
            ISO.changeThreshold = Convert.ToDouble(textBox1.Text);
            oriData = Gdal.Open(fileName, Access.GA_ReadOnly);
            int bandNum = oriData.RasterCount;
            progressBar1.Value = 5;
            List<int[]> bufferArr = new List<int[]>();
            for (int i = 0; i < bandNum; i++)
            {
                int[] buffer = new int[oriData.RasterXSize *  oriData.RasterYSize];
                 oriData.GetRasterBand(i + 1).ReadRaster(0, 0,  oriData.RasterXSize,  oriData.RasterYSize, buffer,  oriData.RasterXSize,  oriData.RasterYSize, 0, 0);
                bufferArr.Add(buffer);
            }
            ISO_Image.get_Buffer = bufferArr;
            progressBar1.Value = 20;
            ISO_Image.get_Cluster = ISO.CalCLusters_ISOData(oriData, bandNum, ISO_Image);
            progressBar1.Value = 80;
            if (ISO_Image.get_Cluster.Count < 8)
            {
                for (int i = 0; i < ISO_Image.get_Cluster.Count; i++)
                {
                    ISO_Image.get_Cluster[i].colorType = K_Means.K_Means.getColor()[i];
                }
            }
            Driver dri = Gdal.GetDriverByName("GTiff");
            outputData = dri.Create(saveFileName,  oriData.RasterXSize,  oriData.RasterYSize, 1,  DataType.GDT_UInt32, null);
            colorData = dri.Create(colorFile,  oriData.RasterXSize,  oriData.RasterYSize, 3, DataType.GDT_Byte, null);
             Kfrm.previewImage(ISO_Image.get_Cluster, outputData,colorData);
            ISO_Image.bandListIni();
            ISO_Image.ChooseBandList(colorData, ISO_Image);
            pictureBox1.Image = ISO_Image.GetImage(colorData, pictureBox1);
            progressBar1.Value = 90;
            //给treeview添加文件结点
            string strImageList = "ClassifiedImageList";
            treeView1.Nodes.Add(strImageList);
            TreeNode treeNo = ISO_Image.getNodeByName(treeView1.Nodes, strImageList);//查找根结点
            treeNo.Nodes.Add(fileName);//加入子节点
            treeNo.Nodes.Add(saveFileName);//加入子节点
            treeNo.Nodes.Add(colorFile);//加入子节点
            treeNo = ISO_Image.getNodeByName(treeNo.Nodes, colorFile);
            for (int i = 0; i < ISO_Image.get_Cluster.Count; i++)
            {
                string name = "Class_" + (ISO_Image.get_Cluster[i].No).ToString();//录入波段信息
                treeNo.Nodes.Add(name);
            }
            progressBar1.Value = 100;
            MessageBox.Show("分类成功！", "提示", MessageBoxButtons.OK);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fileName = RSImage.ReadFileName();
            if (fileName != null)
            {
                fileComboBox.Text = fileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
          string  File = RSImage.ShowSaveFileDialog();
            if (File != null)
            {
                textBox4.Text = File;
                saveFileName = File;
                colorFile = File.Remove(File.Length - 4, 4) + "_ColorType_ISO.tif";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 显示影像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //必须选中当前结点
            if (this.treeView1.SelectedNode == null)
                return;
            TreeNode pNode = this.treeView1.SelectedNode;
            Dataset data = null;
            if (pNode.Text == fileName)
            {
                data = oriData;
            }
            if (pNode.Text == colorFile)
            {
                data = colorData;
            }
            if (pNode.Text == saveFileName)
            {
                data = outputData;
            }
            if (data == null)
            {
                return;
            }
            RSImage im = new RSImage();
            im.bandListIni();
            im.ChooseBandList(data, im);
            pictureBox1.Image = im.GetImage(data, pictureBox1);
        }

        private void 更改聚类信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            TreeNode pNode = this.treeView1.SelectedNode;
            if (pNode.Text == colorFile)
            {
                Classification_Propertites property = new Classification_Propertites(ISO_Image.get_Cluster, ISO_Image);
                if (property.ShowDialog() == DialogResult.Yes)
                {
                    TreeNode treeNo = ISO_Image.getNodeByName(treeView1.Nodes, "ClassifiedImageList");//查找根结点
                    treeNo = ISO_Image.getNodeByName(treeNo.Nodes, colorFile);
                    treeNo.Nodes.Clear();
                    for (int i = 0; i < ISO_Image.get_Cluster.Count; i++)
                    {
                        string name = "Class_" + (ISO_Image.get_Cluster[i].No).ToString();//录入波段信息
                        treeNo.Nodes.Add(name);
                    }
                    Kfrm.previewImage(ISO_Image.get_Cluster, outputData, colorData);
                    ISO_Image.bandListIni();
                    ISO_Image.ChooseBandList(colorData, ISO_Image);
                    pictureBox1.Image = ISO_Image.GetImage(colorData, pictureBox1);
                }
            }
        }

        private void ISODataFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (outputData != null)
            {
                outputData.Dispose();
            }
        }
    }
}
