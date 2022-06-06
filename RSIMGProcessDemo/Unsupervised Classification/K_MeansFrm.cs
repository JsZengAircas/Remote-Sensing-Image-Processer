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
using Test.K_Means;

namespace Test
{
    public partial class K_MeansFrm : Form
    {
        public K_MeansFrm()
        {
            InitializeComponent();
          
        }
      
        RSImage K_Image = new RSImage();
        public int ClassNum { get; set; }
        public double changeThreshold { get; set; }
       public int maxIteration { get; set; }
        Dataset srcData;//待处理影像
        string saveFileName,colorFile,srcFile;
        Dataset outputData,colorData;
     //   List<Clusters> currentCluster = new List<Clusters>();
      
        private void Classification_Load(object sender, EventArgs e)
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
            this.Size = new Size(370, 570);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fileComboBox.Text = RSImage.ReadFileName();//读取文件
            if (fileComboBox.Text != null && fileComboBox.Text != "请输入文件")
            {
                Gdal.AllRegister();
                srcFile = fileComboBox.Text;
                srcData = Gdal.Open(fileComboBox.Text, Access.GA_ReadOnly);
                // K_Image.ChooseBandList(srcData, K_Image);
                bandComboBox.Items.Add("默认处理所有波段");
                for (int i = 0; i < srcData.RasterCount; i++)
                {
                    bandComboBox.Items.Add("Band_" + (i + 1).ToString());
                }
            }
        }

        private void fileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 5;
            List<int> band = new List<int>();
            if (bandComboBox.Text == "默认处理所有波段")
            {
                for (int i = 1; i < srcData.RasterCount + 1; i++)
                {
                    band.Add(i);
                }
            }
            else {
                band.Add(Convert.ToInt32(HistogramForm.GetBandNum(bandComboBox.Text)));
            }
            Gdal.AllRegister();
            ClassNum = Convert.ToInt32(classNumBox.Text);//获取分类数目
            changeThreshold= Convert.ToDouble(rangeBox.Text)/100;//阈值变化范围
            maxIteration = Convert.ToInt32(Iteration_NumBox.Text);//最大迭代次数       
            List<int[]> bufferArr = new List<int[]>();
            for (int i = 0; i < band.Count; i++)
            {
                int[] buffer = new int[srcData.RasterXSize * srcData.RasterYSize];
                srcData.GetRasterBand(band[i]).ReadRaster(0,0, srcData.RasterXSize ,srcData.RasterYSize,buffer, srcData.RasterXSize, srcData.RasterYSize, 0,0);
                bufferArr.Add(buffer);
            }
            K_Image.get_Buffer = bufferArr;
            progressBar1.Value = 10;
            K_Means.K_Means K = new K_Means.K_Means();
            //初始化类聚中心
            List<Clusters> test= K.IniCluster(srcData, ClassNum, K_Image,band.Count);
            progressBar1.Value = 60;
            test = K.ItreativeCal(test,maxIteration,changeThreshold,band.Count,K_Image);
            progressBar1.Value = 80;
            K_Image.get_Cluster = test;
            previewImage(K_Image.get_Cluster, outputData,colorData);
            K_Image.bandListIni();
            K_Image.ChooseBandList(colorData, K_Image);
            pictureBox1.Image = K_Image.GetImage(colorData,pictureBox1);
            progressBar1.Value = 90;
            //给treeview添加文件结点
            string strImageList = "ClassifiedImageList";
            treeView1.Nodes.Add(strImageList);
            TreeNode treeNo = K_Image.getNodeByName(treeView1.Nodes, strImageList);//查找根结点
            treeNo.Nodes.Add(colorFile);//加入子节点
            treeNo.Nodes.Add(saveFileName);//加入子节点
            treeNo.Nodes.Add(srcFile);//加入子节点
            treeNo = K_Image.getNodeByName(treeNo.Nodes, colorFile);
            for (int i = 0; i < K_Image.get_Cluster.Count; i++)
            {
                string name = "Class_" + (K_Image.get_Cluster[i].No).ToString();//录入波段信息
                treeNo.Nodes.Add(name);
            }
            progressBar1.Value = 100;
            MessageBox.Show("分类成功！", "提示", MessageBoxButtons.OK);
        }
        public void previewImage(List<Clusters> cluster,Dataset outputData,Dataset coData)
        {
            int[] byte_R = new int[coData.RasterXSize*coData.RasterYSize];
            int[] byte_G = new int[coData.RasterXSize * coData.RasterYSize];
            int[] byte_B = new int[coData.RasterXSize * coData.RasterYSize];
            int[] DN = new int[coData.RasterXSize * coData.RasterYSize];
            for (int num = 0; num < cluster.Count; num++)
            {
                for (int iCount = 0; iCount < cluster[num].allSamples.Count; iCount++)
                {
                    byte_R[cluster[num].allSamples[iCount].row * coData.RasterXSize + cluster[num].allSamples[iCount].col] = cluster[num].colorType.R;
                    byte_G[cluster[num].allSamples[iCount].row * coData.RasterXSize + cluster[num].allSamples[iCount].col] = cluster[num].colorType.G;
                    byte_B[cluster[num].allSamples[iCount].row * coData.RasterXSize + cluster[num].allSamples[iCount].col] = cluster[num].colorType.B;
                    DN[cluster[num].allSamples[iCount].row * coData.RasterXSize + cluster[num].allSamples[iCount].col] = cluster[num].No;
                }
            }
            coData.GetRasterBand(1).WriteRaster(0,0, coData.RasterXSize,coData.RasterYSize,byte_R, coData.RasterXSize, coData.RasterYSize,0,0);
            coData.GetRasterBand(2).WriteRaster(0, 0, coData.RasterXSize, coData.RasterYSize, byte_G, coData.RasterXSize, coData.RasterYSize, 0, 0);
            coData.GetRasterBand(3).WriteRaster(0, 0, coData.RasterXSize, coData.RasterYSize, byte_B, coData.RasterXSize, coData.RasterYSize, 0, 0);
            outputData.GetRasterBand(1).WriteRaster(0, 0, outputData.RasterXSize, outputData.RasterYSize, DN, outputData.RasterXSize, outputData.RasterYSize, 0, 0);
        }
        private void btSaveFile_Click(object sender, EventArgs e)
        {
            saveBox.Text = RSImage.ShowSaveFileDialog();
            if (saveBox.Text != null)
            {
                saveFileName = saveBox.Text;
                colorFile = saveFileName.Remove(saveFileName.Length - 4, 4) + "_ColorType_K_Means.tif";
                Driver Dri = Gdal.GetDriverByName("GTiff");
                outputData = Dri.Create(saveFileName,srcData.RasterXSize,srcData.RasterYSize,1,DataType.GDT_UInt32,null);
                colorData = Dri.Create(colorFile, srcData.RasterXSize, srcData.RasterYSize, 3, DataType.GDT_Byte, null);
                double[] fInTransForm1 = new double[6];
                srcData.GetGeoTransform(fInTransForm1);
                outputData.SetGeoTransform(fInTransForm1);
                outputData.SetProjection(srcData.GetProjection());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 更改类别属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            TreeNode pNode = this.treeView1.SelectedNode;
            if (pNode.Text == colorFile)
            {
                Classification_Propertites property = new Classification_Propertites(K_Image.get_Cluster, K_Image);
                if (property.ShowDialog() == DialogResult.Yes)
                {
                    TreeNode treeNo = K_Image.getNodeByName(treeView1.Nodes, "ClassifiedImageList");//查找根结点
                    treeNo = K_Image.getNodeByName(treeNo.Nodes, colorFile);
                    treeNo.Nodes.Clear();
                    for (int i = 0; i < K_Image.get_Cluster.Count; i++)
                    {
                        string name = "Class_" + (K_Image.get_Cluster[i].No).ToString();//录入波段信息
                        treeNo.Nodes.Add(name);
                    }
                    previewImage(K_Image.get_Cluster, outputData,colorData);
                    K_Image.bandListIni();
                    K_Image.ChooseBandList(colorData, K_Image);
                    pictureBox1.Image = K_Image.GetImage(colorData, pictureBox1);

                }
            }
        }

        private void 显示影像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //必须选中当前结点
            if (this.treeView1.SelectedNode == null)
                return;
            TreeNode pNode = this.treeView1.SelectedNode;
            Dataset data=null;
            if (pNode.Text == srcFile)
            {
                data = srcData;
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
            im.ChooseBandList(data,im);
            pictureBox1.Image = im.GetImage(data,pictureBox1);
        }

        private void Classification_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (outputData != null)
            {
                outputData.Dispose();
            }       
        }
    }
}
