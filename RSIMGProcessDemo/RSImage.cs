using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.OGR;
using OSGeo.OSR;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using OSGeo.GDAL;
using Test.K_Means;

namespace Test
{
    public class dataList
    {
        public int No;
        public double Base_X;
        public double Base_Y;
        public double Warp_X;
        public double Warp_Y;
        public double Predict_X;
        public double Predict_Y;
    }
    public  class RSImage
    {
        private bool calMat = false;
        private int imageHeight;
        private int imageWidth;
        private double sensorWidth;
        private double sensorHeight;
        private  List<int> bandList = new List<int>();
        public double avgHeight;
        private double[] externalEle;
        private double[] internalEle;
        private double[,] RoMatlab;
        //文件存储路径
        private string saveFileIndex;
        //空间分辨率
        private double Resolution;
        //平均高程设置
        public double evelation=50;
        private Rectangle __ImageRect;
        /// <summary>
        /// 图像的高度
        /// </summary>
        public int get_Width {
            set { imageWidth = value; }
            get { return imageWidth; }
        }
        public int get_Height
        {
            set { imageHeight = value; }
            get { return imageHeight; }
        }
        public double get_SensorHeight
        {
            set { sensorHeight = value; }
            get { return sensorHeight; }
        }
        public double get_SensorWidth 
        {
            set { sensorWidth = value; }
            get { return sensorWidth; }
        }
        public double[] get_ExternalEle
        {
            set { externalEle = value; }
            get { return externalEle; }
        }
        public double[] get_internalEle
        {
            set { internalEle = value; }
            get { return internalEle; }
        }
        public string  get_FileIndex
        {
            set { saveFileIndex = value; }
            get { return saveFileIndex; }
        }
        public double get_Resolution_Ratio
        {
            set { Resolution = value; }
            get { return Resolution; }
        }
        public double[,] get_RoMatlab {
            set { RoMatlab = value; }
            get { return RoMatlab; }

        }
        public List<int> get_BandList
        {
            set {
                bandList = value;
            }
            get {
                return bandList;
            }
        }
        public  void  bandListIni()
        {
            bandList.Clear();
        }
        public Rectangle ImageRect
        {
            get { return __ImageRect; }
            set { __ImageRect = value; }
        }
        //存储影像
        public List<int[]> get_Buffer{
            get;set;
            }
        //
        public List<List<int>> get_TotalVector
        {
            get; set;
        }
        //获取影像的类聚中心
        public List<Clusters> get_Cluster
        {
            get; set;
        }
        /// <summary> 
        /// GDAL栅格转换为位图 
        /// </summary> 
        /// <param name="ds">GDAL Dataset</param> 
        /// <param name="showRect">显示区域</param> 
        /// <param name="bandList">需要显示的波段列表</param> 
        /// <returns>返回Bitmap对象</returns> 
        public Bitmap GetImage(OSGeo.GDAL.Dataset ds, PictureBox showRect)
        {
            int imgWidth = ds.RasterXSize;   //影像宽 
            int imgHeight = ds.RasterYSize;  //影像高 
            float ImgRatio = imgWidth / (float)imgHeight;  //影像宽高比 
            //获取显示控件大小 
            int BoxWidth = showRect.Width;
            int BoxHeight = showRect.Height;
            float BoxRatio = BoxWidth / (float)BoxHeight;  //显示控件宽高比 
            //计算实际显示区域大小，防止影像畸变显示 
            int BufferWidth, BufferHeight;
            if (BoxRatio >= ImgRatio)
            {
                BufferHeight = BoxHeight;
                BufferWidth = (int)(BoxHeight * ImgRatio);
            }
            else
            {
                BufferWidth = BoxWidth;
                BufferHeight = (int)(BoxWidth / ImgRatio);
            }
            //构建RGB位图 
            Bitmap bitmap = new Bitmap(BufferWidth, BufferHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            if (bandList.Count == 3)     //RGB显示 
            {
                int[] r = new int[BufferWidth * BufferHeight];
                Band band1 = ds.GetRasterBand(bandList[2]);
                band1.ReadRaster(0, 0, imgWidth, imgHeight, r, BufferWidth, BufferHeight, 0, 0);  //读取图像到内存 
                //为了显示好看，进行最大最小值拉伸显示 
                double[] maxandmin1 = { 0, 0 };
                band1.ComputeRasterMinMax(maxandmin1, 0);
                int[] g = new int[BufferWidth * BufferHeight];

                Band band2 = ds.GetRasterBand(bandList[1]);
                band2.ReadRaster(0, 0, imgWidth, imgHeight, g, BufferWidth, BufferHeight, 0, 0);
                double[] maxandmin2 = { 0, 0 };
                band2.ComputeRasterMinMax(maxandmin2, 0);
                int[] b = new int[BufferWidth * BufferHeight];

                Band band3 = ds.GetRasterBand(bandList[0]);
                band3.ReadRaster(0, 0, imgWidth, imgHeight, b, BufferWidth, BufferHeight, 0, 0);
                double[] maxandmin3 = { 0, 0 };
                band3.ComputeRasterMinMax(maxandmin3, 0);

                int i, j;
                for (i = 0; i < BufferWidth; i++)
                {
                    for (j = 0; j < BufferHeight; j++)
                    {
                        Color newColor;
                        int rVal, gVal, bVal;
                        if ((maxandmin1[1] - maxandmin1[0]) == 0)
                        {
                            rVal = 0;
                        }
                        else {
                             rVal = Convert.ToInt32(r[i + j * BufferWidth]);
                            rVal = (int)((rVal - maxandmin1[0]) / (maxandmin1[1] - maxandmin1[0]) * 255);                        
                        }
                        if ((maxandmin2[1] - maxandmin2[0]) == 0)
                        {
                            gVal = 0;
                        }
                        else {
                            gVal = Convert.ToInt32(g[i + j * BufferWidth]);
                            gVal = (int)((gVal - maxandmin2[0]) / (maxandmin2[1] - maxandmin2[0]) * 255);
                        }
                        if ((maxandmin3[1] - maxandmin3[0]) == 0)
                        {
                            bVal = 0;                        
                        }
                        else {
                            bVal = Convert.ToInt32(b[i + j * BufferWidth]);
                            bVal = (int)((bVal - maxandmin3[0]) / (maxandmin3[1] - maxandmin3[0]) * 255);
                        }
                        newColor = Color.FromArgb(rVal, gVal, bVal);
                        bitmap.SetPixel(i, j, newColor);
                    }
                }
            }
            else               //灰度显示 
            {
                int[] r = new int[BufferWidth * BufferHeight];
                Band band1 = ds.GetRasterBand(bandList[0]);
                band1.ReadRaster(0, 0, imgWidth, imgHeight, r, BufferWidth, BufferHeight, 0, 0);
                double[] maxandmin1 = { 0, 0 };
                band1.ComputeRasterMinMax(maxandmin1, 0);
                double z = maxandmin1[0];
                double t = maxandmin1[1];
                int i, j;
                for (i = 0; i < BufferWidth; i++)
                {
                    for (j = 0; j < BufferHeight; j++)
                    {
                        Color newColor;
                        if ((maxandmin1[1] - maxandmin1[0]) == 0)
                        {
                            newColor = Color.FromArgb(0, 0, 0);
                        }
                        else
                        {
                            int rVal = Convert.ToInt32(r[i + j * BufferWidth]);
                            rVal = (int)((rVal - maxandmin1[0]) / (maxandmin1[1] - maxandmin1[0]) * 255);
                             newColor = Color.FromArgb(rVal, rVal, rVal);
                        }
                        bitmap.SetPixel(i, j, newColor);
                    }
                }
            }
            return bitmap;
        }
        //按名字查找ListTree中的结点
        public TreeNode getNodeByName(TreeNodeCollection pTreeNode, string strNodeName)
        {
            //遍历每个结点
            foreach (TreeNode node in pTreeNode)
            {
                if (node.Text == strNodeName)//查找成功则返回
                {
                    return node;
                }
            }
            return null;//查找失败返回空
        }
        //获取图像信息并且生成文本信息
        public string getImageInfo(RichTextBox richTextBox)
        {
            //ImageShow image = new ImageShow();
            OSGeo.GDAL.Dataset dataset = OSGeo.GDAL.Gdal.Open(ImageShow.filePath, Access.GA_ReadOnly);
            string dispa = "======================================================";
            string En = System.Environment.NewLine;
            string Info = "ImageInformation" + En;
            Info = Info + dispa + System.Environment.NewLine;
            Info = Info + "Description:" + dataset.GetDriver().GetDescription() + En; //获取图片基本信息
            Info = Info + "Width:" + Convert.ToString(dataset.RasterXSize) + "cm;" + "Height:" + Convert.ToString(dataset.RasterYSize) + "cm" + En;//影像的长宽
            Info = Info + "BandCount:" + Convert.ToString(dataset.RasterCount) + En;//波段信息
            Info = Info + dataset.GetProjectionRef() + En;//投影信息
            return Info;
        }
      
        ///</summary>
        ///<parma name="row_I">行号
        ///<parma name=col_J>列号
        ///<returns double xy[]>返回xy值
        public int[] Pixel_TransferTo_RowCol(double x, double y)
        {
            int[] Res = new int[2];
            double det = ((Convert.ToDouble(sensorWidth)) / imageWidth) * 0.001;//每个像元的大小
            Res[1] =Convert.ToInt32((x/det)+(imageWidth)/2);  //列号
            Res[0] =Convert.ToInt32( (imageHeight)/2-(y/det));//行号
            //确定仿射关系
            return Res;
        }
        /// <summary>
        /// 计算旋转矩阵
        /// </summary>
        /// <param name="phi"></param>
        /// <param name="omega"></param>
        /// <param name="kappa"></param>
        /// <returns></returns>
        public double[,] Rotation_Ma_Cal(double al, double om, double ka)
        {
            double Llithita = 0;//参考的航向角度

            if (ka >= 0.0 && ka < 45.0) Llithita = 0.0;
            if (ka >= 45.0 && ka < 135.0) Llithita = Math.PI / 2.0;
            if (ka >= 135.0 && ka < 225.0) Llithita = Math.PI;
            if (ka >= 225.0 && ka <= 315.0) Llithita = 1.5 * Math.PI;
            if (ka >= 315.0 && ka < 360.0) Llithita = 2.0 * Math.PI;

            double kappa = Llithita - (ka * (Math.PI) / 180.0);//根据参考航向计算偏航 改成了右手系
            double omega = om * (Math.PI) / 180.0;
            double phi = al * (Math.PI) / 180.0;
            double[,] Rotation_Ma = new double[3, 3];
            //计算各元素
            double a1 = Math.Cos(phi) * Math.Cos(kappa) - Math.Sin(phi) * Math.Sin(omega) * Math.Sin(kappa);Rotation_Ma[0, 0] = a1;
            double a2 = -Math.Cos(phi) * Math.Sin(kappa) - Math.Sin(phi) * Math.Sin(omega) * Math.Cos(kappa);Rotation_Ma[0, 1] = a2;
            double a3 = -Math.Sin(phi) * Math.Cos(omega);Rotation_Ma[0, 2] = a3;

            double b1 = Math.Cos(omega) * Math.Sin(kappa);Rotation_Ma[1, 0] = b1;
            double b2 = Math.Cos(omega) * Math.Cos(kappa); Rotation_Ma[1, 1] = b2;
            double b3 = -Math.Sin(omega);Rotation_Ma[1, 2] = b3;

            double c1 = Math.Sin(phi) * Math.Cos(kappa) + Math.Cos(phi) * Math.Sin(omega) * Math.Sin(kappa);Rotation_Ma[2, 0] = c1;
            double c2 = -Math.Sin(phi) * Math.Sin(kappa) + Math.Cos(phi) * Math.Sin(omega) * Math.Cos(kappa);Rotation_Ma[2, 1] = c2;
            double c3 = Math.Cos(phi) * Math.Cos(omega);Rotation_Ma[2, 2] = c3;
            get_RoMatlab = Rotation_Ma;
            return Rotation_Ma;

        }
        /// <summary>
        /// 经纬度坐标转UTM坐标
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public double[] project(double lon, double lat) {
            double[] Res = new double[2];
            const double a54 = 6378245;  //椭球长半轴
            const double b54 = 6356863.01877;//椭球短半轴
            double e2 = (a54 * a54 - b54 * b54) / (a54 * a54);  //地球椭球偏心率
            double e1 = Math.Sqrt((a54 * a54 - b54 * b54) / (b54 * b54));
            double pi = Math.Atan(1.0) * 4.0;
            int zoneNum = (int)(Math.Floor(lon * 30.0 / (Math.PI)) + 1.0);//先化度,再分6度带.
            double centralL = zoneNum * 6 - 3.0;
            double FalseEast = zoneNum * 1.0e6 + 500000;
            double B, L, sx, N, ita2, m, t, d;
            double A0, B0, C0, D0,E0;
            double e4, e6, e8, t2, t4, ita4;
            double gama;
            d = a54 * (1 - e2);
            B = lat;
            L = lon * 180 / (pi);
            ita2 = e1 * e1 * Math.Cos(B) * Math.Cos(B);
            t = Math.Tan(B);
            t2 = t * t;
            t4 = t2 * t2;
            ita4 = ita2 * ita2;
            N = a54 / (Math.Sqrt(1 - e2)) / Math.Sqrt(1 + ita2);
            m = Math.Cos(B) * (pi) / 180.0 * (L - centralL);

            e4 = e2 * e2;
            e6 = e4 * e2;
            e8 = e6 * e2;
            A0 = d * (1 + 0.750 * e2 + 45.0 / 64.0 * e4 + 175.0 / 256.0 * e6 + 11025.0 / 16384.0 * e8);
            B0 = A0 - d;
            C0 = d * (15.0 / 32.0 * e4 + 175.0 / 368.0 * e6 + 3675.0 / 8192.0 * e8);
            D0 = d * (35.0 / 96.0 * e6 + 735.0 / 2048.0 * e8);
            E0= d * (315.0 / 1024.0 * e8);
            sx = A0 * B - (B0 + C0 * Math.Sin(B) * Math.Sin(B) + D0 * Math.Sin(B) * Math.Sin(B) * Math.Sin(B) * Math.Sin(B) + E0 * Math.Sin(B) * Math.Sin(B) * Math.Sin(B) * Math.Sin(B) * Math.Sin(B) * Math.Sin(B)) * Math.Sin(B) * Math.Cos(B);
            Res[0] = sx + N * t * (0.50 * Math.Pow(m, 2.0) + 1.0 / 24.0 * (5.0 - t2 + 9.0 * ita2 + 4.0 * ita4) * Math.Pow(m, 4.0) + (61.0 - 58.0 * t2 + t4) * Math.Pow(m, 6.0) / 720.0);
            Res[1] = N * (m + (1.0 - t2 + ita2) * Math.Pow(m, 3.0) / 6.0 + (5.0 - 18.0 * t2 + t4 + 14.0 * ita2 - 58.0 * ita2 * t2) * Math.Pow(m, 5.0) / 120.0);
            Res[1] = Res[1] + FalseEast;
            double namada;
            namada = ((L - centralL) * 3600.0) / 206265.0;
            gama = namada * Math.Sin(B) + namada * namada * namada * Math.Sin(B) * Math.Cos(B) * Math.Cos(B) * (1.0 + 3.0 * ita2) / 3.0;
            gama = gama * 180.0 / (pi);
            return Res;
        }
        public double[] lonLai_Convert_UTM(double lon,double lat)
        {
           const double kD2R = Math.PI/ 180;
           double   ZoneNumber =Math.Floor((lon - 1.5) / 3.0)+ 1;
           double L0 = ZoneNumber * 3.0;

           const double  a = 6378137.0;const double   F = 298.257223563;const double   f = 1 / F;
           const double    b = a * (1 - f);const double   ee = (a * a - b * b) / (a * a);const double e2 = (a * a - b * b) / (b * b);
           double  n = (a - b) / (a + b); double n2 = (n * n);double  n3 = (n2 * n); double n4 = (n2 * n2);double n5 = (n4 * n);
           double  al = (a + b) * (1 + n2 / 4 + n4 / 64) / 2.0;double bt = -3 * n / 2 + 9 * n3 / 16 - 3 * n5 / 32.0;
           double  gm = 15 * n2 / 16 - 15 * n4 / 32;double  dt = -35 * n3 / 48 + 105 * n5 / 256;
           double ep = 315 * n4 / 512;double   B = lat * kD2R;double   L = lon * kD2R;L0 = L0 * kD2R;
           double l = L - L0; double cl = (Math.Cos(B) * l); double cl2 = (cl * cl); double cl3 = (cl2 * cl); double cl4 = (cl2 * cl2); double cl5 = (cl4 * cl); 
           double cl6 = (cl5 * cl); double cl7 = (cl6 * cl); double cl8 = (cl4 * cl4);
           double lB = al * (B + bt * Math.Sin(2 * B) + gm * Math.Sin(4 * B) + dt * Math.Sin(6 * B) + ep * Math.Sin(8 * B));
           double t = Math.Tan(B); double t2 = (t * t);double t4 = (t2 * t2); double t6 = (t4 * t2);
           double   Nn = a / Math.Sqrt(1 - ee * Math.Sin(B) * Math.Sin (B));
           double yt = e2 * Math.Cos(B) * Math.Cos(B);
           double  N = lB;
           N = N + t * Nn * cl2 / 2;
           N = N + t * Nn * cl4 * (5 - t2 + 9 * yt + 4 * yt * yt) / 24;
           N = N + t * Nn * cl6 * (61 - 58 * t2 + t4 + 270 * yt - 330 * t2 * yt) / 720;
           N = N + t * Nn * cl8 * (1385 - 3111 * t2 + 543 * t4 - t6) / 40320;
           double  E = Nn * cl;
           E = E + Nn * cl3 * (1 - t2 + yt) / 6;E = E + Nn * cl5 * (5 - 18 * t2 + t4 + 14 * yt - 58 * t2 * yt) / 120;
           E = E + Nn * cl7 * (61 - 479 * t2 + 179 * t4 - t6) / 5040;E = E + 500000;
           N = 0.9996 * N;E = 0.9996 * (E - 500000.0) + 500000.0;
           double[] Res = new double[2];
           Res[0] = N;Res[1] = E;
           return Res;
        }
        public double[] pixelToGeoCor(double imx,double imy)
        {
            
            double[] Geo = new double[2];
            double[,] Ro = new double[3, 3];
            double x= imx * sensorHeight - internalEle[0];
            double y= internalEle[1] - imy * sensorHeight;
            Ro = Rotation_Ma_Cal(get_ExternalEle[3], get_ExternalEle[4], get_ExternalEle[5]);
            Geo[0] = externalEle[0] + (evelation - externalEle[2]) * (Ro[0, 0] * x + Ro[0, 1] * y - Ro[0, 2] * internalEle[2])/ (Ro[2, 0] * x + Ro[2, 1] * y - Ro[2, 2] * internalEle[2]);
            Geo[1]=externalEle[1]+(evelation-externalEle[2]) * (Ro[1, 0] * x + Ro[1, 1] * y - Ro[1, 2] * internalEle[2]) / (Ro[2, 0] * x + Ro[2, 1] * y - Ro[2, 2] * internalEle[2]);
            return Geo;
        }
        public double[] ImgxyToGcpXYByColline(double imx,double imy,double imz)
        {
            double f, x, y;
            double a1, a2, a3, b1, b2, b3, c1, c2, c3, om, al, ka;
            double Xs, Ys, Zs;
            double Xt, Yt, Zt, Dx, Dy, Dz;
            double dtemp1, dtemp2;


            /** 地面坐标- 右手系

                    Y 北向
                    /\
                    |
                    |
                    |
                    |
------------O------------------->X东向
                    |
                    |
                    |
                    |
                    */


            Xs = externalEle[0];//东向   2013-10-31 采用了右手系的地面坐标和图像坐标
            Ys = externalEle[1];//北向
            Zs = externalEle[2];

            al = externalEle[3];//姿态 传进来为度  侧滚
            om = externalEle[4];//俯仰
            ka = externalEle[5];//航向 0-360的左手系


            double Llithita=0;//参考的航向角度

            if (ka >= 0.0 && ka < 45.0) Llithita = 0.0;
            if (ka >= 45.0 && ka < 135.0) Llithita = Math.PI / 2.0;
            if (ka >= 135.0 && ka < 225.0) Llithita = Math.PI;
            if (ka >= 225.0 && ka <= 315.0) Llithita = 1.5 * Math.PI;
            if (ka >= 315.0 && ka < 360.0) Llithita = 2.0 * Math.PI;

            ka = Llithita - (ka * (Math.PI) / 180.0);//根据参考航向计算偏航 改成了右手系

            om = om * (Math.PI) / 180.0;
            al = al * (Math.PI) / 180.0;

           
            x = imx * sensorHeight - internalEle[0];//x:计算中心化的列向像坐标 	右手系
            y = internalEle[1] - imy * sensorHeight;//y:计算中心化的航向像坐标 	



            f = internalEle[2];//焦距
            Zt = imz;//地面平均高度


            a1 = Math.Cos(al) * Math.Cos(ka) - Math.Sin(al) * Math.Sin(om) * Math.Sin(ka);
            a2 = -Math.Cos(al) * Math.Sin(ka) - Math.Sin(al) * Math.Sin(om) * Math.Cos(ka);

            a3 = -Math.Sin(al) * Math.Cos(om);

            b1 = Math.Cos(om) * Math.Sin(ka);
            b2 = Math.Cos(om) * Math.Cos(ka);

            b3 = -Math.Sin(om);

            c1 = Math.Sin(al) * Math.Cos(ka) + Math.Cos(al) * Math.Sin(om) * Math.Sin(ka);
            c2 = -Math.Sin(al) * Math.Sin(ka) + Math.Cos(al) * Math.Sin(om) * Math.Cos(ka);


            c3 = Math.Cos(al) * Math.Cos(om);

            Dz = Zt - Zs;
            dtemp1 = (a1 * x + a2 * y - a3 * f);
            dtemp2 = c1 * x + c2 * y - c3 * f;
            Dx = Dz * dtemp1 / dtemp2;//东向/旁向 增量 
            dtemp1 = (b1 * x + b2 * y - b3 * f);
            Dy = Dz * dtemp1 / dtemp2;//北行/航向 增量
                                  

            Xt = Xs + Math.Cos(Llithita) * Dx + Math.Sin(Llithita) * Dy;//东向/旁向 
            Yt = Ys - Math.Sin(Llithita) * Dx + Math.Cos(Llithita) * Dy;//北行/航向 2013-11-31 确认这种方法较为科学

            double[] ResXYZ = new double[2];
            ResXYZ[0] = Xt;//东向
            ResXYZ[1] = Yt;//北向
            return ResXYZ;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcfname">low-res 1-band Geotiff filename (i.e. NIR, Red, Green, Blue)</param>
        /// <param name="dstfname"> high-res (pan) Geotiff filename</param>
        /// <returns></returns>
        public Dataset Resample(string srcfname,string dstfname)
        {
            Gdal.AllRegister();
            Dataset srcDataset;
           string srcProjection;

            /* open up low-resolution Geotiff file dataset */
            srcDataset = Gdal.Open(srcfname, Access.GA_ReadOnly);
            if (srcDataset == null)
            {
                return null;
            }
            /* read projection from low-res. Geotiff. Also read its
           * data-type. For LANDSAT8 its unsigned int16.
           */
            srcProjection = srcDataset.GetProjectionRef();
            DataType sourceDataType;
           sourceDataType= srcDataset.GetRasterBand(1).DataType;

            String outfname;
            int len = (srcfname).Length;
            String extension=("_resampled.tif");
            outfname = (srcfname).Substring(0, len - 4) + extension;

            /* open the destination (high-res) file (i.e. panchromatic image file) ,
            * read following parameters:
            *  (1) ncols : output resampled number of columns (samples)
            *  (2) nrows : output resampled number of rows (lines)
            *  (3) output geotransform for resampled low-res geotiff : array of 6 DOUBLE elements
            *  (4) output projection string
            */
            Dataset dstDataset;
            int dstnrows, dstncols;
            double []dstGeotransform=new double[6];
            string dstProjection;

            dstDataset = Gdal.Open(dstfname, Access.GA_ReadOnly);
            dstncols = dstDataset.RasterXSize;
            dstnrows = dstDataset.RasterYSize;
            dstDataset.GetGeoTransform(dstGeotransform);
            dstProjection = dstDataset.GetProjectionRef();

            /* create geotransform object ... pass in following parameters:
             * (1) srcDataset : open GDAL dataset from low-res file
               * (2) srcProjection : projection from low-res file
              * (3) destination projection : projection of panchromatic (high-res.)
              *     among other arguments.
             */
           Gdal.ReprojectImage(srcDataset, dstDataset, srcfname, dstfname, ResampleAlg.GRA_Bilinear, 1, 0, null, null);
            return srcDataset;
        
        }
        public static string ReadFileName()
        {
            //创建打开文件类的对象
            OpenFileDialog openFile = new OpenFileDialog();
            //设置路径
            //“标签|*.jpg;*.png;*.gif”，注意：只是在筛选器中多添加了几个后缀，不同后缀之间使用分号隔开
            //filter属性设置如下：“标签1|*.jpg|标签2|.png|标签3|.gif”。注意：不同的筛选器之间使用“|”分隔即可。
            openFile.Filter = "RSImage|*.tiff;*.img;*.tif;|regular digital image|*.jpg;*.png;*.bmp*";
            if (openFile.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
               
            return openFile.FileName;
        }
        public static string ShowSaveFileDialog()
        {
            string localFilePath = "";
            //string localFilePath, fileNameExt, newFileName, FilePath; 
            SaveFileDialog sfd = new SaveFileDialog();
            //设置文件类型 
            sfd.Filter = ".tiff file|*.tiff;|.img file|*.img;|.jpg file|*.jpg;|.png file|*.png;|.bmp file|*.bmp*";

            //设置默认文件类型显示顺序 
            sfd.FilterIndex = 1;

            //保存对话框是否记忆上次打开的目录 
            sfd.RestoreDirectory = true;

            //点了保存按钮进入 
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                localFilePath = sfd.FileName.ToString(); //获得文件路径


            }
            return localFilePath;
        }
        public void ChooseBandList(Dataset Item,RSImage ima)
        {
            if (Item.RasterCount != 3)
            {
                if (Item.RasterCount == 1)
                {
                    ima.get_BandList.Add(1);//全色显示
                }
                else
                {
                    if (MessageBox.Show("影像波段大于3，请选择相应波段操作!", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        BandChooseFrm bandfrm = new BandChooseFrm(Item, ima);
                        if (bandfrm.ShowDialog() == DialogResult.Yes)
                        {

                        }
                        // pix_Pre.Image = fusionImage.GetImage(data_Msi, this.pix_Pre);
                        //Msi_State = true;                                        
                    }
                }
            }
            else
            {
               ima.get_BandList.Add(3);
                ima.get_BandList.Add(2);
                ima.get_BandList.Add(1);
                // fusionImage.get_BandList = bandList_Msi;
            }
        }
     }
}
