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
using Test.Supervised_Classification;

namespace Test
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }
        string strImageList = "RS_ImageList";
        //string strImagePath = "";
        private ImageShow imaShow;
        imageInfo info = new imageInfo();//地图信息显示窗口
        HistogramForm histogram = new HistogramForm();//直方图窗口
        private bool firstOpen = true;//用于判断是否是第一次打开窗体
        public static List<string> fileNameList = new List<string>();//存储所有文件名字
        public static string CurentImage;
        public static OSGeo.GDAL.Dataset currentDataset;
        int zx = 0;
        //打开地图
        private void openDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            if (ImageShow.FormExt)
            {
                imaShow = new ImageShow();//实例化窗体
                TreeNode Root = new TreeNode();
                imaShow.treeview.Nodes.Add(Root);
                Root.Text = "RS_ImageList";
                ImageShow.FormExt = false;
                //初始化方法
                SetupShowImaInfoEvents(imaShow);
                SetupShowHistoEvents(imaShow);
            }
            string file  = RSImage.ReadFileName();
            if (file!=null)
            {
                string fileName = file;
                RSImage op = new Test.RSImage();
                //给treelist分配结点.结点名称为文件名
                OSGeo.GDAL.Dataset dataset = OSGeo.GDAL.Gdal.Open(fileName, OSGeo.GDAL.Access.GA_ReadOnly);
                currentDataset = dataset;

                TreeNode treeNo = op.getNodeByName(imaShow.treeview.Nodes, strImageList);//查找根结点
                                                                                         //给根节点下的第一个子节点录入信息
                treeNo.Name = fileName;
                CurentImage = fileName;//获取当前窗口显示的影像
                fileNameList.Add(treeNo.Name);//存储文件名字
                treeNo.Nodes.Add(fileName);//加入子节点
                                           //遍历整棵树
                treeNo = op.getNodeByName(treeNo.Nodes, fileName);
                //初始化函数调用语句
                SharpMap.GdalConfiguration.ConfigureGdal();
                SharpMap.GdalConfiguration.ConfigureOgr();

                //读取影像信息

                if (dataset != null)
                {
                    for (int i = 0; i < dataset.RasterCount; i++)
                    {
                        string name = "band_" + (i + 1).ToString();//录入波段信息
                        treeNo.Nodes.Add(name);
                    }
                }
                //获取影像的高度宽度

                //检查当前窗口显示情况，是否关闭
                if (firstOpen || (imaShow != null))
                {
                    ShowMap(true);
                }
                firstOpen = false;
            }
        }
        //地图显示函数
        private void ShowMap(bool show)
        {

            if (show)
            {

                imaShow.Show(this.dockPanel1);

                imaShow.DockTo(this.dockPanel1, DockStyle.Fill);

            }
            else
            {
                imaShow.Hide();
            }
        }


       //按钮信息提示
        private void toolStripButton1_MouseEnter(object sender, EventArgs e)
        {
            toolStripButton1.Text = "图像信息";
        }
        //快捷菜单图像信息显示
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            imageInfo frm = new Test.imageInfo();
            //窗体显示
            frm.Show(this.dockPanel1);
            //悬浮窗体停靠右边
            frm.DockTo(this.dockPanel1, DockStyle.Right);
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            
        }
        #region
        //显示Imageinfo
        /// <summary> 

        /// 将方法绑定至委托

        /// </summary> 

        ///将RS_ImageInfo绑定至ShowImaInfo事件。“+=”

        ///格式：class.event+=(new) class.delegate(pama);

        ///
        private void SetupShowImaInfoEvents(ImageShow show)
        {
            show.ShowImaInfo += new ImageShow.ShowImaInfoEventHandler(this.RS_ImageInfo);
        }
        private void RS_ImageInfo(object sender,ShowEventArgs e)
        {
            if (!imageInfo.ImaInfoSta)
            {
                imageInfo newImageinfo = new imageInfo();
                ShowImaInfo(true, newImageinfo);
            }
            else
            {
                ShowImaInfo(true,info);//调用窗体事件
            }

        }
        private void ShowImaInfo(bool show,imageInfo info)
        {
            if (show)
            {
                info.Show(this.dockPanel1);
                info.DockTo(this.dockPanel1,DockStyle.Right);
            }
        }

        //一致
        #endregion
        #region
        private void SetupShowHistoEvents(ImageShow show)
        {
            show.ShowHisto += new ImageShow.ShowImaInfoEventHandler(this.RS_ImageHisto);
        }
        private void RS_ImageHisto(object sender, ShowEventArgs e)
        {
            if (!HistogramForm.HisogramState)
            { 
                HistogramForm New_histogram = new HistogramForm();//直方图窗口
                HistogramForm.HisogramState = true;
                GetIniHist(New_histogram, e.Filename);

            }
            else { GetIniHist(histogram, e.Filename); }   
        }
        public void GetIniHist(HistogramForm histogram,string FileName)
        {
            histogram.Filename = FileName;
            Gdal.AllRegister();

            OSGeo.GDAL.Dataset dataset = Gdal.Open(FileName, OSGeo.GDAL.Access.GA_ReadOnly);//读取文件

            if (dataset != null)
            {
                histogram.comboBox1.Items.Clear();//清除combox
                for (int i = 0; i < dataset.RasterCount; i++)//输入波段信息
                {
                    string name = "Band_" + (i + 1).ToString();
                    histogram.comboBox1.Items.Add(name);
                }
            }
            histogram.textBox1.Text = FileName;
            ShowImageHisto(true,histogram);//调用窗体事件

        }
        private void ShowImageHisto(bool show,HistogramForm hist)
        {
            if (show||HistogramForm.HisogramState)
            {
                hist.Show(this.dockPanel1);
                hist.DockTo(this.dockPanel1, DockStyle.Right);
            }
        }
        #endregion
        private void radiationCorrectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void radiometricCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadiationCorr corr = new RadiationCorr();
            corr.ShowDialog();
        }

        private void radiometricCalibrationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }

        private void lineScratchToolStripMenuItem_Click(object sender, EventArgs e)
        {

           

        }

        private void radiometricCalibrationToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }

        private void histogramEqualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurentImage != null)
            {
                OSGeo.GDAL.Dataset ds = Gdal.Open(CurentImage, OSGeo.GDAL.Access.GA_ReadOnly);
                RSImage op = new RSImage();
                string saveFileName = CurentImage.Remove(CurentImage.Length-4,4)+ "_Equalization.tif";
                Driver dri = Gdal.GetDriverByName("GTiff");
                Dataset output = dri.Create(saveFileName,ds.RasterXSize,ds.RasterYSize,ds.RasterCount,DataType.GDT_Byte,null);               
                Radiation_Enhancement.Equalization(ds, CurentImage,output);
                op.bandListIni();
                op.ChooseBandList(output, op);
                //更新图像
                //   imaShow.pictureBox1.Image = null;
                imaShow.pictureBox1.Image = op.GetImage(output, imaShow.pictureBox1);
                MessageBox.Show("操作成功!", "提示", MessageBoxButtons.OK);
                TreeNode treeNo = op.getNodeByName(imaShow.treeview.Nodes, strImageList);//查找根结点
                treeNo.Nodes.Add(saveFileName);
                output.Dispose();
                ds.Dispose();
            }
        }

        private void geometricCoarseCorrectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CoarseCor Co = new CoarseCor();
            Co.ShowDialog();
        }

        private void manualImageRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image_Registration IR = new Image_Registration();
            IR.G = IR;
            IR.Show();
        }

        private void broveyFusionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image_Fusion imageFusion = new Image_Fusion(1);
            imageFusion.Show();
        }

        private void iHS变换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image_Fusion imageFusion = new Image_Fusion(2);
            imageFusion.Show();
        }

        private void histogramMtchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Histogram_MatchingFrm His_MaFrm = new Test.Histogram_MatchingFrm();
            His_MaFrm.Show();
        }

        private void otsu阈值分割ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image_Segmentation ImaSeg = new Image_Segmentation(1);
            ImaSeg.Text = "otsu阈值分割";
            ImaSeg.Show();
        }

        private void iterativeThresholdSegmentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image_Segmentation ImaSeg = new Image_Segmentation(2);
            ImaSeg.Text = "迭代阈值分割";
            ImaSeg.Show();
        }

        private void unsuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void changeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Segement_Application apply = new Test.Segement_Application();
            apply.Show();
        }

        private void kMeansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            K_MeansFrm classFrm = new Test.K_MeansFrm();
            classFrm.Show();
        }

        private void iosDataClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISODataFrm ISOFrm = new Test.ISODataFrm();
            ISOFrm.ShowDialog();
        }

        private void 影像直方图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!HistogramForm.HisogramState)
            {
                HistogramForm New_histogram = new HistogramForm();//直方图窗口
                HistogramForm.HisogramState = true;
                GetIniHist(New_histogram, CurentImage);
            }
            else { GetIniHist(histogram, CurentImage); }
        }

        private void 辐射定标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadiationCorr corr = new RadiationCorr();
            corr.ShowDialog();
        }

        private void applicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Driver dri = Gdal.GetDriverByName("GTiff");
            string saveAs = RSImage.ShowSaveFileDialog();
            Dataset saveAsDataset = dri.CreateCopy(saveAs,currentDataset,1,null,null,null);
            MessageBox.Show("另存成功！", "提示", MessageBoxButtons.OK);
            saveAsDataset.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void saveDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存成功！","提示",MessageBoxButtons.OK);
        }

        private void supervisedClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Supervised_Classification.SupervisedClassFrm superFrm = new Supervised_Classification.SupervisedClassFrm(this.dockPanel1);
            superFrm.Show(this.dockPanel1);
            superFrm.DockTo(this.dockPanel1,DockStyle.Fill);
            superFrm.myfrm = superFrm;
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void 线性拉伸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Linear(0,CurentImage);
        }
        public void Linear(int Type,string ImageFile)
        {
            if (CurentImage != null)
            {
                OSGeo.GDAL.Dataset ds = Gdal.Open(ImageFile, OSGeo.GDAL.Access.GA_ReadOnly);
                RSImage op = new RSImage();
                string saveFileName;
                if (Type == 0)
                {
                     saveFileName = CurentImage.Remove(CurentImage.Length - 4, 4) + "_Linear2.tif";
                }
                else {  saveFileName = CurentImage.Remove(CurentImage.Length - 4, 4) + "_Linear5.tif"; }
                Driver dri = Gdal.GetDriverByName("GTiff");
                Dataset output = dri.Create(saveFileName, ds.RasterXSize, ds.RasterYSize, ds.RasterCount, DataType.GDT_Byte, null);                
                Radiation_Enhancement.LineStretch(ds,output, CurentImage,Type);//获取           
                imaShow.pictureBox1.Image = null;
                op.bandListIni();
                op.ChooseBandList(output, op);
                imaShow.pictureBox1.Image = op.GetImage(output, imaShow.pictureBox1);
                TreeNode treeNo = op.getNodeByName(imaShow.treeview.Nodes, strImageList);//查找根结点
                treeNo.Nodes.Add(saveFileName);
                MessageBox.Show("操作成功！!", "提示", MessageBoxButtons.OK);
                ds.Dispose();
                output.Dispose();
            }
            else
            {
                MessageBox.Show("请打开图像！");
                return;
            }
        }
        private void 线性拉伸ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Linear(1, CurentImage);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (imaShow.pictureBox1.Image == null)
                return;
            if (zx > 15)
                return;
            int x = imaShow.pictureBox1.Width / 2;
            int y = imaShow.pictureBox1.Height / 2;
            int ow = imaShow.pictureBox1.Width;
            int oh = imaShow.pictureBox1.Height;
            int VX, VY;     //因缩放产生的位移矢量

            imaShow.pictureBox1.Width += 50;
            imaShow.pictureBox1.Height += 50;

            VX = (int)((double)x * (ow - imaShow.pictureBox1.Width) / ow);
            VY = (int)((double)y * (oh - imaShow.pictureBox1.Height) / oh);
            imaShow.pictureBox1.Location = new Point(imaShow.pictureBox1.Location.X + VX, imaShow.pictureBox1.Location.Y + VY);
            zx++;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (imaShow.pictureBox1.Image == null)
                return;
            if (zx <=0)
                return;
            int x = imaShow.pictureBox1.Width / 2;
            int y = imaShow.pictureBox1.Height / 2;
            int ow = imaShow.pictureBox1.Width;
            int oh = imaShow.pictureBox1.Height;
            int VX, VY;     //因缩放产生的位移矢量

            imaShow.pictureBox1.Width -= 50;
            imaShow.pictureBox1.Height -= 50;

            VX = (int)((double)x * (ow - imaShow.pictureBox1.Width) / ow);
            VY = (int)((double)y * (oh - imaShow.pictureBox1.Height) / oh);
            imaShow.pictureBox1.Location = new Point(imaShow.pictureBox1.Location.X + VX, imaShow.pictureBox1.Location.Y + VY);
            zx--;
        }
    }
}
