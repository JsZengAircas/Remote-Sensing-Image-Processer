using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OSGeo;
using OSGeo.GDAL;
using Ogr = OSGeo.OGR.Ogr;
using System.Reflection;

namespace Test
{
    public partial class ImageShow : DockContent
    {
        public ImageShow()
        {
            InitializeComponent();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(ImageZoom_MouseWheel);
        }
        public  static string filePath;
        private Dataset CurrentDataSet;
        private Bitmap CurrentImage;
        Point mouseDownPoint = new Point();
        bool isMove;
        RSImage operate = new RSImage();
        /*
        public string FilePath
        {
            set { filePath = value; }
            get { return filePath; }
        }*/
        public static bool FormExt=true;

        public delegate void ShowImaInfoEventHandler(object sender, ShowEventArgs e);//委托

        public event ShowImaInfoEventHandler ShowImaInfo;//定义一个事件,用于打开图像信息窗体

        public event ShowImaInfoEventHandler ShowHisto;//用于打开直方图窗体

        private void ImageShow_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //必须选中当前结点
            if (this.treeview.SelectedNode == null)
                return;

            TreeNode pNode = this.treeview.SelectedNode;
            
            if (pNode.Level != 1) return;
            
            OSGeo.GDAL.Dataset dataSet =OSGeo.GDAL.Gdal.Open(filePath, OSGeo.GDAL.Access.GA_ReadOnly);//读取文件
            CurrentDataSet = dataSet;
            List<int> curBandList = new List<int>();//波段选取     
            BandChooseFrm BandFrm = new BandChooseFrm(MainFrm.currentDataset,operate);
            if (dataSet.RasterCount != 3&& dataSet.RasterCount !=1)
            {
                if ((MessageBox.Show("影像波段大于3，默认RGB显示？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes))
                {
                    curBandList.Add(3);
                    curBandList.Add(2);
                    curBandList.Add(1);
                    operate.get_BandList = curBandList;
                }
                else
                {
                    if ((MessageBox.Show("请选择全色显示还是选择波段（Yes：选择波段，No：全色显示）？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes))
                    {
                        BandFrm.Show();                
                    }
                    else
                    {
                        curBandList.Add(1);
                        operate.get_BandList = curBandList;
                    }
                }
            }
            else {
                if (dataSet.RasterCount == 1)
                {
                    curBandList.Add(1);
                    operate.get_BandList = curBandList;
                }
                else {
                    curBandList.Add(3);
                    curBandList.Add(2);
                    curBandList.Add(1);
                    operate.get_BandList = curBandList;
                }               
            }
            this.pictureBox1.Image= operate.GetImage(dataSet, this.pictureBox1);
            CurrentImage = operate.GetImage(dataSet, this.pictureBox1);

        }

        private void ImageShow_Load(object sender, EventArgs e)
        {
             this.toolStripStatusLabel4.Text = "当前时间: " + DateTime.Now.ToString()+"|";
        }

        private void treeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //如果不是右键或者处在一级菜单，无操作
            if (e.Button != MouseButtons.Right) return;

            if (e.Node.Level != 1)//重新建立一个文件夹
            {

            }

            Point ClickPoint = new Point(e.X, e.Y);//获取鼠标的当前位置

            TreeNode CurrentNode = treeview.GetNodeAt(ClickPoint);

            CurrentNode.ContextMenuStrip = contextMenuStrip1;//打开菜单

           treeview.SelectedNode = CurrentNode;//选中这个节点

          filePath = CurrentNode.Text;//获取当前结点的文件位置
            
        }

        private void ImageInfo_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ShowImaInfo != null)//事件不为空
                ShowImaInfo(this, new ShowEventArgs(filePath));
        }

        private void ImageShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出当前界面吗？", "提示信息",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                FormExt = true;
            }
            else {
                e.Cancel=true;
            }
        }

        public void Histogram_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowHisto != null)//事件不为空
                ShowHisto(this, new ShowEventArgs(filePath));
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
         
        }

        private void removeLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //必须选中当前结点
            if (this.treeview.SelectedNode == null)
                return;

            TreeNode pNode = this.treeview.SelectedNode;

            if (pNode.Level != 1) return;
            //删除影像
            if (this.treeview.SelectedNode.Text == MainFrm.CurentImage)
            {
                pictureBox1.Image = null;
                pictureBox1.Refresh();
            }
            //删除
            pNode.Remove();

        }
        private void ImageZoom_MouseWheel(object sender, MouseEventArgs e)
        {
            double scale = 1;
            if (pictureBox1.Height > 0)
            {
                scale = (double)pictureBox1.Width / (double)pictureBox1.Height;
            }
            pictureBox1.Width += (int)(e.Delta * scale);
            pictureBox1.Height += e.Delta;
        }

        private void changeRGBBandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BandChooseFrm BandFrm = new BandChooseFrm(MainFrm.currentDataset,operate);
            if (BandFrm.ShowDialog()== DialogResult.Yes)
            {
                pictureBox1.Image = operate.GetImage(MainFrm.currentDataset,pictureBox1);
            }    
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentDataSet != null&&CurrentImage!=null)
            {
                if (e.X < CurrentImage.Width && e.Y < CurrentImage.Height)
                {
                    int col = Convert.ToInt32((e.X / (double)CurrentImage.Width) * CurrentDataSet.RasterXSize);
                    int row = Convert.ToInt32((e.Y / (double)CurrentImage.Height) * CurrentDataSet.RasterYSize);
                    //计算显示图像左上角空间坐标
                    double[] fTransForm = new double[6];                  
                    CurrentDataSet.GetGeoTransform(fTransForm);
                   // string prj=  CurrentDataSet.GetProjection();
                    //计算鼠标空间坐标
                    double CoordX = fTransForm[0] + row * fTransForm[1];
                    double CoordY = fTransForm[3] + col * fTransForm[5];
                    string strText = "当前坐标  X: ";
                    //strText = strText + string.Format("{0}", e.Location.X.ToString("#######.##"));
                    strText = strText + CoordX.ToString("F2");
                    strText = strText + "    Y: ";
                    //strText = strText + string.Format("{0}", e.Location.Y.ToString("#######.##"));
                    strText = strText + CoordY.ToString("F2");
                    toolStripStatusLabel3.Text = "|"+strText+"|";              
                }
                if (isMove)
                {
                    int x, y;   //新的pictureBox1.Location(x,y)
                    int moveX, moveY; //X方向，Y方向移动大小。
                    moveX = Cursor.Position.X - mouseDownPoint.X;
                    moveY = Cursor.Position.Y - mouseDownPoint.Y;
                    x = pictureBox1.Location.X + moveX;
                    y = pictureBox1.Location.Y + moveY;
                    pictureBox1.Location = new Point(x, y);
                    mouseDownPoint.X = Cursor.Position.X;
                    mouseDownPoint.Y = Cursor.Position.Y;
                }
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null)
                return;          
                mouseDownPoint.X = Cursor.Position.X; //记录鼠标左键按下时位置
                mouseDownPoint.Y = Cursor.Position.Y;
                isMove = true;
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null)
                return;          
                isMove = false;
        }
    }

    public class ShowEventArgs : EventArgs
    {
        private string filename;

        // Constructors
        public ShowEventArgs(string filename)
        {
            this.filename = filename;
        }

        // Location property
        public string Filename
        {
            get { return filename; }
        }

    }
    
}
