using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Imaging;
using AForge.Math;

namespace Test
{
    public partial class HistogramForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public HistogramForm()
        {
            InitializeComponent();
            geoDataSet = null;//初始化变量
        }
        private OSGeo.GDAL.Dataset geoDataSet;	//定义一个数据集
        private string strFileName;             //存储图像的信息
        List<Color> colorList = new List<Color>();//存储颜色信息
        int colorCount = 0;//
        private ImageStatistics ImageState;     //用于获取文件的属性
        //private AForge.Math.Histogram activeHistogram = null;

        public static bool HisogramState=true ;
        public double  MaxLab, MinLab;
        //建立属性
        public OSGeo.GDAL.Dataset GeoDataset
        {
            set
            {
                geoDataSet = value;
            }
            get { return geoDataSet; }
        }
        public string Filename
        {
            set
            {
                strFileName = value;
            }
            get { return strFileName; }
        }

        private void HistogramForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = strFileName;        
            colorList.Add(Color.Red);
            colorList.Add(Color.Yellow);
            colorList.Add(Color.Green);
            colorList.Add(Color.Blue);
            colorList.Add(Color.GreenYellow);
            colorList.Add(Color.HotPink);
            colorList.Add(Color.DarkOrange);
            colorList.Add(Color.Gray);
        }
        //直方图绘制
        //1.获取combox通道
        public static string GetBandNum(string bandname)
        {
            string name = "";
            int n = bandname.LastIndexOf("_");//获取最后一个字符的索引
            if (n >= 0)
            {
                name = bandname.Remove(0, n + 1);//赋值
            }
            else
            {
                name = bandname;
            }
            return name;
        }
        //3.选择通道完成绘制直方图
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Item = comboBox1.SelectedItem.ToString();

            int nBandnum = Convert.ToInt32(GetBandNum(Item));//获取波段数

            OSGeo.GDAL.Gdal.AllRegister();
            OSGeo.GDAL.Dataset dataset = OSGeo.GDAL.Gdal.Open(strFileName, OSGeo.GDAL.Access.GA_ReadOnly);//获取文件

           OSGeo.GDAL.Band band = dataset.GetRasterBand(nBandnum);//获取通道
            
            //设定参数,stdDev标准方差
            double outMin, outMax, outMean, outStdDev;
            band.GetStatistics(0, 1, out outMin, out outMax, out outMean, out outStdDev);//采集信息
            int[] hisData_Trans = new int[Convert.ToInt32(outMax+1)];//线性拉伸后的直方图
            MaxLab = outMax;MinLab = outMin;
            band.GetHistogram(outMin - 1.5, outMax + 1.5, Convert.ToInt32(outMax + 1), hisData_Trans, 0, 1, null, null);
            //绘制直方图
            chart1.Series.Clear();
            var series = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Pixels",
                Color = colorList[(colorCount++)%8],
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };
            this.chart1.Series.Add(series);
            double picCount = 0;
            for (int i = 0; i < hisData_Trans.Length; i++)
            {
                series.Points.AddXY(i, hisData_Trans[i]);
                picCount += hisData_Trans[i];
            }
            chart1.Invalidate();
            MeanLabel.Text ="平均值:"+ outMean.ToString("F2");//保留两位小数
            MaxLabel.Text = "最大值:"+outMax.ToString();
            MinLabel.Text ="最小值:"+ outMin.ToString();
            StdDevLabel.Text ="标准差:"+ outStdDev.ToString("F2");
            PixLabel.Text = "像素数:" + picCount.ToString();
        }
        /*
        public void drawhistogram(int[] data, double Max, double  Min)
        {
            //获取控件的大小
            int PanHeight, PanWidth;
            PanHeight = pictureBox1.Height;
            PanWidth = pictureBox1.Width;
            //创建画布
            Bitmap bitmap = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            Graphics g = pictureBox1.CreateGraphics();//在picture上绘制
            g = Graphics.FromImage(bitmap);
            pictureBox1.Image = null;
            //Brush brush = new Brush(Color.Gray);
            Pen drawpen = new Pen(Color.Black, 2);
            //定义坐标系

            //绘制x轴
            Point Px1 = new Point(4,PanHeight-4);
            Point Px2 = new Point(4,0);
            g.DrawLine(drawpen, Px1, Px2);
            //绘制箭头
            Point xArrowLeft = new Point(0,4);
            Point xArrowright = new Point(8,4);
            g.DrawLine(drawpen, Px2, xArrowLeft);
            g.DrawLine(drawpen, Px2, xArrowright);
            //绘制y坐标
            Point Py1 = new Point( 4,PanHeight-4);
            Point Py2 = new Point(PanWidth-4, PanHeight-4);
            g.DrawLine(drawpen, Py1, Py2);
            //绘制箭头
            Point yArrowUp = new Point(PanWidth-8, PanHeight-8);
            Point yArrowLow = new Point(PanWidth-8, PanHeight);
            g.DrawLine(drawpen, Py2, yArrowUp);
            g.DrawLine(drawpen, Py2, yArrowLow);
            //绘制直方图
            float MaxData = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (MaxData < data[i])
                { MaxData = data[i]; }
            }

            //绘制比例格
            int MaxSca = Convert.ToInt32(MaxData / (100f))+1;
            Font ft = new Font("宋体", 8);
            for (int i = 0; i < 4; i++)
            {
                Point Ite = new Point(4, (PanHeight / 4) * i);
                Point Itf = new Point(8, (PanHeight / 4) * i);
                g.DrawLine(drawpen, Ite, Itf);
                g.DrawString(Convert.ToString(MaxSca * 100*(4-i) / 4), ft, Brushes.Black, Ite);
            }
           
                //Point Ites = new Point(4, PanHeight-4);
                Point Itfs = new Point(PanWidth-30, PanHeight-15);
                Font fts = new Font("宋体", 8);
                g.DrawString(MaxLab.ToString(), fts, Brushes.Black, Itfs);
            

            float newWid = PanWidth - 14;
            float newHei = (PanHeight - 4);
            float Scalew = newWid/ 256;
            float Scaleh = newHei/ MaxData;
            
            for (int i = 0; i < data.Length; i++)
            {            
                g.FillRectangle(Brushes.Blue,4+i*Scalew,(PanHeight - 4- data[i] * Scaleh), Scalew, data[i] * Scaleh);
            }
            
            pictureBox1.Image = bitmap;


        }
        */
        

        private void HistogramForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HisogramState = false;
        }

        private void MeanLabel_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
           
        }
    }
}
