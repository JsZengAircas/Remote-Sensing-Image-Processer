using System;
using System.Windows.Forms;
using Gdal = OSGeo.GDAL.Gdal;
using OSGeo.GDAL;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication1
{

    class ReadImage
    {
        string strNodeName = "OpenImage";
        public static string fileName;
        #region 打开影像
        public Bitmap GetImage(OSGeo.GDAL.Dataset ds, Rectangle showRect, int[] bandlist)
        {
            int imgWidth = ds.RasterXSize;   //影像宽
            int imgHeight = ds.RasterYSize;  //影像高

            float ImgRatio = imgWidth / (float)imgHeight;  //影像宽高比

            //获取显示控件大小
            int BoxWidth = showRect.Width;
            int BoxHeight = showRect.Height;

            float BoxRatio = imgWidth / (float)imgHeight;  //显示控件宽高比

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

            //构建位图
            Bitmap bitmap = new Bitmap(BufferWidth, BufferHeight,
                                     System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            if (bandlist.Length == 3)     //RGB显示
            {
                int[] r = new int[BufferWidth * BufferHeight];
                Band band1 = ds.GetRasterBand(bandlist[0]);
                band1.ReadRaster(0, 0, imgWidth, imgHeight, r, BufferWidth, BufferHeight, 0, 0);  //读取图像到内存

                //为了显示好看，进行最大最小值拉伸显示
                double[] maxandmin1 = { 0, 0 };
                band1.ComputeRasterMinMax(maxandmin1, 0);

                int[] g = new int[BufferWidth * BufferHeight];
                Band band2 = ds.GetRasterBand(bandlist[1]);
                band2.ReadRaster(0, 0, imgWidth, imgHeight, g, BufferWidth, BufferHeight, 0, 0);

                double[] maxandmin2 = { 0, 0 };
                band2.ComputeRasterMinMax(maxandmin2, 0);

                int[] b = new int[BufferWidth * BufferHeight];
                Band band3 = ds.GetRasterBand(bandlist[2]);
                band3.ReadRaster(0, 0, imgWidth, imgHeight, b, BufferWidth, BufferHeight, 0, 0);

                double[] maxandmin3 = { 0, 0 };
                band3.ComputeRasterMinMax(maxandmin3, 0);

                int i, j;
                for (i = 0; i < BufferWidth; i++)
                {
                    for (j = 0; j < BufferHeight; j++)
                    {
                        int rVal = Convert.ToInt32(r[i + j * BufferWidth]);
                        rVal = (int)((rVal - maxandmin1[0]) / (maxandmin1[1] - maxandmin1[0]) * 255);

                        int gVal = Convert.ToInt32(g[i + j * BufferWidth]);
                        gVal = (int)((gVal - maxandmin2[0]) / (maxandmin2[1] - maxandmin2[0]) * 255);

                        int bVal = Convert.ToInt32(b[i + j * BufferWidth]);
                        bVal = (int)((bVal - maxandmin3[0]) / (maxandmin3[1] - maxandmin3[0]) * 255);

                        Color newColor = Color.FromArgb(rVal, gVal, bVal);
                        bitmap.SetPixel(i, j, newColor);
                    }
                }
            }
            else               //灰度显示
            {
                int[] r = new int[BufferWidth * BufferHeight];
                Band band1 = ds.GetRasterBand(bandlist[0]);
                band1.ReadRaster(0, 0, imgWidth, imgHeight, r, BufferWidth, BufferHeight, 0, 0);

                double[] maxandmin1 = { 0, 0 };
                band1.ComputeRasterMinMax(maxandmin1, 0);

                int i, j;
                for (i = 0; i < BufferWidth; i++)
                {
                    for (j = 0; j < BufferHeight; j++)
                    {
                        int rVal = Convert.ToInt32(r[i + j * BufferWidth]);
                        rVal = (int)((rVal - maxandmin1[0]) / (maxandmin1[1] - maxandmin1[0]) * 255);

                        Color newColor = Color.FromArgb(rVal, rVal, rVal);
                        bitmap.SetPixel(i, j, newColor);
                    }
                }
            }

            return bitmap;
        }
        #endregion
        #region 创建树
        public OSGeo.GDAL.Dataset Read(System.Windows.Forms.TreeView TreeView1)
        {
            string filename = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Tiff文件|*.tif|Erdas img文件|*.img|Bmp文件|*.bmp|jpeg文件|*.jpg|所有文件|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filename = dlg.FileName;
                fileName = filename;
            }

            if (filename == "")
            {
                MessageBox.Show("影像路径不能为空");
                return null;

            }
            TreeNode pNode = TreeViewUse.u_GetNodebyName(TreeView1.Nodes, strNodeName);
            //找到同名的根节点
            pNode.Nodes.Add(dlg.FileName);
            //在根节点下添加文件名
            pNode = TreeViewUse.u_GetNodebyName(pNode.Nodes, dlg.FileName);
            //再找到此节点下的的结点
            OSGeo.GDAL.Gdal.AllRegister();
            OSGeo.GDAL.Dataset ds = Gdal.Open(filename, OSGeo.GDAL.Access.GA_ReadOnly);
            if (ds == null)
            {
                MessageBox.Show("影像打开失败");
                return null;
            }

            if (ds != null)
            {
                for (int i = 0; i < ds.RasterCount; i++)
                {
                    string name = "band_" + (i + 1).ToString();
                    pNode.Nodes.Add(name);
                }
            }




            return ds;
        }
        #endregion
        #region 打开图像在pan（picturebox）里
        public OSGeo.GDAL.Dataset ImageShow(PictureBox pic, System.Windows.Forms.TreeView TreeView)
        {
            OSGeo.GDAL.Dataset ds = Read(TreeView);
            Rectangle pictureRect = new Rectangle();
            pictureRect.X = 0;
            pictureRect.Y = 0;
            pictureRect.Width = pic.Width;
            pictureRect.Height = pic.Height;


            int[] disband = { 3, 2, 1 };

            Bitmap bitmap = GetImage(ds, pictureRect, disband);   //遥感影像构建位图
            pic.Image = bitmap;                   //将位图传递给PictureBox控件进行显示

            return ds;
        }
        #endregion
        #region 画统计直方图
        public void Statichistogram(OSGeo.GDAL.Dataset ds, int BanNum, PictureBox pan,out int nBuckets,out int[] panHistogram)
        {

            Band band = ds.GetRasterBand(BanNum + 1);
            double dfmin; double dfmax; double pdfMean; double pdfStdDev;
            band.GetStatistics(1, 1, out dfmin, out dfmax, out pdfMean, out pdfStdDev);
            nBuckets = 255;//表示直方图统计的份数
            panHistogram = new int[255];//存储直方图的数组
            int include_out_of_range = 0;//如果这个参数设置为TRUE，那么图像中的像元值小于最小值的像元值将被统计到直方图数组中的第一个里面去，图像中的像元值大于最大值的像元会被统计到直方图数组中的最后一个里面去。如果设置为FALSE，那么图像中的像元值小于最小值的像元不进行统计，同样，像元值超过最大值的像元值也不进行统计。
            int approx_ok = 1;
            Gdal.GDALProgressFuncDelegate callback = null;
            string callback_data = null;
            band.GetHistogram(dfmin, dfmax, nBuckets, panHistogram, include_out_of_range, approx_ok, callback, callback_data);
        }
        public void Drawhistogram(int[] panHistogram, int nBuckets,PictureBox pan)

        { Bitmap bitmap = new Bitmap(pan.Width, pan.Height);
            Graphics g = pan.CreateGraphics();//在picture上绘制
            g = Graphics.FromImage(bitmap);
            //开始绘制直方图
            int temp = 0;//存储灰度频率最大的位
            double maxPixel = panHistogram[0];//存储灰度频率最大的像素数
            for (int i = 0; i < nBuckets; i++)
            {
                if (panHistogram[i] > maxPixel)
                {
                    maxPixel = panHistogram[i];
                    temp = i;
                }
            }

            float move = 20f;
            float LenX = -(pan.Width - 2 * move);
            float LenY = -(pan.Height - 2 * move);
            Pen p = new Pen(Color.Black, 1);
            for (int i = 0; i < nBuckets; i++)
            {
                g.DrawLine(p, Convert.ToInt64(0.5 * i), move - LenY, Convert.ToInt64(0.5 * i), Convert.ToInt64(LenY / maxPixel * panHistogram[i] + move) - LenY);
            }
            Draw.DrawXY(pan, bitmap);
            Draw.DrawYLine(pan, Convert.ToInt64(maxPixel), 6, bitmap);
            Draw.DrawXLine(pan, nBuckets, 6, bitmap);
            pan.Image = bitmap;
            g.Dispose();
        }
        #endregion

    }
}
