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
using WeifenLuo.WinFormsUI.Docking;
using static Test.ImageShow;

namespace Test.Supervised_Classification
{
    public partial class SupervisedClassFrm:DockContent
    {
        public SupervisedClassFrm(DockPanel panel)
        {
            InitializeComponent();
            MainPanel = panel;
        }
        public SupervisedClassFrm myfrm { get; set; }
        DockPanel MainPanel;
        RSImage SupervisedImage = new RSImage();
        string disposeFileName,strImageList,saveFileName,colorFileName;
         bool setIni=false;
        Dataset disposeData,outputData,colorData;
        List<ROI> all_ROI = new List<ROI>();
        ROI currentROI;
        Set_ROI set;
        Point[] SetCord;
        Bitmap currentBitmap;
        List<int[]> buffer = new List<int[]>();
        int index = 0;
        int index_No = 0;
        //存储所有分类
        List<List<ROI>> allClass = new List<List<ROI>>();
        //存储类聚中心
        List<Clusters> cluster = new List<Clusters>();
        string nodeText;
        K_Means.K_Means KM = new K_Means.K_Means();
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void SupervisedClassFrm_Load(object sender, EventArgs e)
        {
            strImageList = "ClassifiedImageList";
            treeView1.Nodes.Add(strImageList);
        }

        private void 读取分类文件ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string file = RSImage.ReadFileName();
            if (file != null)
            {
                disposeFileName = file;
                //给treeview添加文件结点                  
                TreeNode treeNo = SupervisedImage.getNodeByName(treeView1.Nodes, strImageList);//查找根结点
                treeNo.Nodes.Add(disposeFileName);//加入子节点
            }
        }

        private void 添加样本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Set_ROI setFrm = new Set_ROI();
            set = setFrm;
            set.SucFrm = myfrm;
            setFrm.Show(MainPanel);
            setFrm.DockTo(MainPanel,DockStyle.Right);
            setIni = true;
            nodeText = treeView1.SelectedNode.Text;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (setIni)
            {
                if (set.editStart)
                {
                    pictureBox1.Cursor = Cursors.Cross;
                }
                else { pictureBox1.Cursor = Cursors.Default; }
            }
        }
           

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (setIni)
            {
                if (set.editStart)
                {
                    ROI newROI = new ROI();
                    currentROI = newROI;
                    currentROI.No = ++index;
                    SetCord = new Point[2];
                    Point left = new Point();
                    left.X = e.X;
                    left.Y = e.Y;
                    SetCord[0] = left;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (setIni)
            {
                if (set.editStart)
                {
                    //SetCord = new Point[2];
                    Point right = new Point();
                    right.X = e.X;
                    right.Y = e.Y;
                    SetCord[1] = right;
                    currentROI.rectangleBit = SetCord;
                    DrawROI(pictureBox1,SetCord,currentBitmap);
                    all_ROI.Add(get_ImageROI(currentROI,pictureBox1,disposeData));
                    set.updataROIData(all_ROI);
                }
            }
        }

        private void SupervisedClassFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (disposeData != null)
            {
                disposeData.Dispose();
            }
              
        }

        public void DrawROI(PictureBox pic,Point[] Cord,Bitmap image)
        {
            Graphics g = pic.CreateGraphics();//在picture上绘制
            g = Graphics.FromImage(image);
            SolidBrush brush = new SolidBrush(set.get_Color);
            Size s = new Size(Cord[1].X - Cord[0].X, Cord[1].Y - Cord[0].Y);
            Rectangle rec = new Rectangle(Cord[0], s);
            g.FillRectangle(brush, rec);
            pic.Image = image;
            g.Dispose();
        }
        private void 读取存储路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = RSImage.ShowSaveFileDialog();
            if (file != null)
            {
                try
                {
                    saveFileName = file;
                    colorFileName = file.Remove(file.Length - 4, 4) + "_SuperviedColor.tif";
                }
                catch { }
            }
        }

        private void 开始执行分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            //获取每个像素点在每个波段的值
            //List<List<int>> ImgTotalVector = new List<List<int>>();
            //分别计算每个像元至每个类聚中心的距离     
            if (saveFileName != null && saveFileName != "")
            {
                toolStripProgressBar1.Value = 5;
                for (int rowNum = 0; rowNum < disposeData.RasterYSize; rowNum++)
                {
                    for (int colNum = 0; colNum < disposeData.RasterXSize; colNum++)
                    {
                        List<int> vocter = new List<int>();
                        for (int num = 0; num < disposeData.RasterCount; num++)
                        {
                            vocter.Add(buffer[num][rowNum * disposeData.RasterXSize + colNum]);
                        }
                        //ImgTotalVector.Add(vocter);
                        DIS[] dispose = new DIS[cluster.Count];
                        for (int num = 0; num < cluster.Count; num++)
                        {
                            dispose[num].classNo = cluster[num].No;
                            dispose[num].distance = KM.CalculateDis(cluster[num], vocter);
                        }
                        //赋值
                        Sample sample = new Sample();
                        sample.row = rowNum;
                        sample.col = colNum;
                        sample.imgVector = vocter;
                        //添加
                        int index = KM.SortDis(dispose) - 1;
                        cluster[index].allSamples.Add(sample);
                        //vocter.Clear();
                    }
                }
                toolStripProgressBar1.Value = 35;
                cluster = KM.ItreativeCal(cluster, 12, 3, disposeData.RasterCount, SupervisedImage);
                toolStripProgressBar1.Value = 75;
                Driver dri = Gdal.GetDriverByName("GTiff");
                outputData = dri.Create(saveFileName, disposeData.RasterXSize, disposeData.RasterYSize, 1, DataType.GDT_UInt16, null);
                colorData = dri.Create(colorFileName, disposeData.RasterXSize, disposeData.RasterYSize, 3, DataType.GDT_Byte, null);
                K_MeansFrm kFrm = new K_MeansFrm();
                kFrm.previewImage(cluster, outputData, colorData);
                toolStripProgressBar1.Value = 95;
                //加入结点信息
                TreeNode node = SupervisedImage.getNodeByName(treeView1.Nodes, "ClassifiedImageList");
                node.Nodes.Add(colorFileName);
                node.Nodes.Add(saveFileName);
                //
                toolStripProgressBar1.Value = 100;
                MessageBox.Show("分类成功！", "提示", MessageBoxButtons.OK);
                //
                colorData.Dispose();
                outputData.Dispose();
            }
            else { MessageBox.Show("请选择存储路径！","提示",MessageBoxButtons.OK); }
        }
        public void reDrawRegion(int indexRow)
        {
            index--;
            //删除数据
            all_ROI.Remove(all_ROI[indexRow]);
            //重新排序
            for (int i = 0; i < all_ROI.Count; i++)
            {
                all_ROI[i].No = i + 1;
            }
            //更新数据
            set.updataROIData(all_ROI);
            //重绘影像
            Bitmap newBitmap = SupervisedImage.GetImage(disposeData,pictureBox1);
            Graphics g = pictureBox1.CreateGraphics();//在picture上绘制
            g = Graphics.FromImage(newBitmap);
            SolidBrush brush = new SolidBrush(set.get_Color);
            for (int i = 0; i < all_ROI.Count; i++)
            {
                Size s = new Size(all_ROI[i].rectangleBit[1].X - all_ROI[i].rectangleBit[0].X, all_ROI[i].rectangleBit[1].Y - all_ROI[i].rectangleBit[0].Y);
                Rectangle rec = new Rectangle(all_ROI[i].rectangleBit[0], s);
                g.FillRectangle(brush, rec);
            }
            pictureBox1.Image = newBitmap;
        }
        public ROI get_ImageROI(ROI roi,PictureBox pic,Dataset data)
        {
           Point[] rectData = new Point[roi.rectangleBit.Length];
            roi.rectangleData = rectData;
            for (int i = 0; i < roi.rectangleBit.Length; i++)
            {
                roi.rectangleData[i].X =Convert.ToInt32((Convert.ToDouble(roi.rectangleBit[i].X))/ Convert.ToDouble(pic.Image.Width)*disposeData.RasterXSize);
                roi.rectangleData[i].Y = Convert.ToInt32((Convert.ToDouble(roi.rectangleBit[i].Y)) / Convert.ToDouble(pic.Image.Height) * disposeData.RasterYSize);
            }
            int pixCount = 0;
            for (int i = roi.rectangleData[0].Y; i < roi.rectangleData[1].Y+1; i++)
            {
                for (int j = roi.rectangleData[0].X; j < roi.rectangleData[1].X + 1; j++)
                {
                    Sample sam = new K_Means.Sample();
                    List<int> vector = new List<int>();
                    for (int num = 0; num < buffer.Count; num++)
                    {
                        vector.Add(buffer[num][i*data.RasterXSize+j]);
                    }
                    sam.imgVector= vector;
                    sam.row = i;
                    sam.col = j;
                    roi.contentSample.Add(sam);
                    pixCount++;
                }
            }
            roi.Num = pixCount;
            return roi;

        }
        private void 显示影像ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            TreeNode Node = this.treeView1.SelectedNode;
            string file = Node.Text;
            if (file != null && file != "ClassifiedImageList" && file.Remove(file.Length - 2, 2) != "Class")
            {
                Dataset temp = Gdal.Open(file,Access.GA_ReadOnly);
                disposeData = temp;
                SupervisedImage.bandListIni();
                SupervisedImage.ChooseBandList(temp,SupervisedImage);
                pictureBox1.Image = SupervisedImage.GetImage(temp,pictureBox1);
               currentBitmap= SupervisedImage.GetImage(temp, pictureBox1);
              //  temp.Dispose();
            }
            Gdal.AllRegister();
            buffer.Clear();       
            for (int i = 0; i < disposeData.RasterCount; i++)
            {
                int[] buffe_temp = new int[disposeData.RasterXSize * disposeData.RasterYSize];
                disposeData.GetRasterBand(i + 1).ReadRaster(0,0,disposeData.RasterXSize,disposeData.RasterYSize,buffe_temp,disposeData.RasterXSize,disposeData.RasterYSize,0,0);
                buffer.Add(buffe_temp);
            }
        }
        public void ClassifyData()
        {
            double[] temp = new double[buffer.Count];
            int allCount = 0;
            for (int num= 0; num < all_ROI.Count; num++)
            {              
                for (int i = 0; i < all_ROI[num].contentSample.Count; i++)
                {
                    for (int j = 0; j < buffer.Count; j++)
                    {
                        temp[j] += all_ROI[num].contentSample[i].imgVector[j];
                       
                    }
                    allCount++;
                }
            }
            List<double> center = new List<double>();
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] /= allCount;
                center.Add(temp[i]);
            }          
            List<Sample> sample = new List<K_Means.Sample>();
            Clusters newClu = new Clusters(++index_No,set.get_Color,sample,center);
            allClass.Add(all_ROI);
            cluster.Add(newClu);
            //加入子节点
            TreeNode Node = SupervisedImage.getNodeByName((SupervisedImage.getNodeByName(treeView1.Nodes, "ClassifiedImageList").Nodes), nodeText);
            Node.Nodes.Add("Class_"+set.get_Name);
            //清除当前ROI
            all_ROI.Clear();
            index = 0;
        }
    }
}
