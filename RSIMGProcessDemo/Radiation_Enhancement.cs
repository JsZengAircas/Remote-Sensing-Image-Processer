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
using Gdal = OSGeo.GDAL.Gdal;
using Ogr = OSGeo.OGR.Ogr;
using System.Drawing.Imaging;
using OSGeo.GDAL;

namespace Test
{
    class Radiation_Enhancement
    {
        public static void LineStretch(Dataset ds,Dataset output,string File,int type)
        {
                //获取图像的大小
                int InWidth = ds.RasterXSize;
                int InHeight = ds.RasterYSize;
                //定义数组
                int[] bufferPix = new int[InWidth*InHeight];
                int bandCount = ds.RasterCount;
                for (int i = 1; i < bandCount + 1; i++)
                {
                    //获取最大最小值
                    OSGeo.GDAL.Band band = ds.GetRasterBand(i);//获取通道
                    double outMin, outMax, outMean, outStdDev;
                    band.GetStatistics(0, 1, out outMin, out outMax, out outMean, out outStdDev);//采集信息
                    int[] histData = new int[(int)outMax + 1];//直方图统计
                   band.GetHistogram(outMin - 0.5, outMax + 0.5, (int)outMax + 1, histData, 0, 0, null, null);
                    int []maxMin =getLinearPara(histData, InWidth * InHeight,type);
                   ds.GetRasterBand(i).ReadRaster(0, 0, InWidth, InHeight, bufferPix, InWidth, InHeight, 0, 0);
               
                    for (int row = 0; row < InHeight; row++)
                    {
                        for (int col = 0; col < InWidth; col++)
                        {
                            bufferPix[row * InWidth + col] = Convert.ToInt32((255 / (maxMin[1] - maxMin[0])) * (bufferPix[row * InWidth + col] - maxMin[0]));
                        }                     
                    }
                    //读入栅格数据
                    output.GetRasterBand(i).WriteRaster(0, 0, InWidth, InHeight, bufferPix, InWidth, InHeight, 0, 0);
                }
          
        }
        //直方图均衡化
        public static void Equalization(Dataset ds,string File,Dataset output)
        {       
                int InWidth = ds.RasterXSize;
                int InHeight = ds.RasterYSize;
                //定义数组
                int[] bufferPix = new int[InWidth*InHeight];
                int bandCount = ds.RasterCount;            
                for (int i = 1; i < bandCount+1; i++)
                {
                    OSGeo.GDAL.Band band = ds.GetRasterBand(i);//获取通道
                                                               //设定参数,stdDev标准方差
                    double outMin, outMax, outMean, outStdDev;
                    band.GetStatistics(0, 1, out outMin, out outMax, out outMean, out outStdDev);//采集信息                  
                    int[] histData = new int[(int)outMax+1];//直方图统计
                    //获取图像的统计直方图
                    band.GetHistogram(outMin - 0.5, outMax + 0.5, (int)outMax+1, histData, 0, 0, null, null);
                    double[] differ = FormerOp(histData, InWidth * InHeight);//获取概率统计直方图
                    ds.GetRasterBand(i).ReadRaster(0, 0, InWidth, InHeight, bufferPix, InWidth, InHeight , 0, 0);
                    //创建新的图像
                    //写入信息
                    for (int row = 0; row < InHeight; row++)
                    {
                        for (int col = 0; col < InWidth; col++)
                        {
                            bufferPix[row * InWidth + col] = Convert.ToInt32(outMin+(outMax-outMin) * 
                                Convert.ToInt32(differ[bufferPix[row * InWidth + col]]));
                        }
                    }
                    output.GetRasterBand(i).WriteRaster(0, 0, InWidth, InHeight, bufferPix, InWidth, InHeight, 0, 0);
                }            

        }
        //拉伸参数计算
        public static int[] getLinearPara(int []his,double total,int Type)
        {
            double radio = 0;
            if (Type == 0)
            {
                radio = 0.02;
            }
            else { radio = 0.05; }
            int[] Result = new int[2];
            double[] Differ = new double[his.Length];
            for (int i = 0; i < his.Length; i++)
            {
                if (i != 0)
                {
                    Differ[i] = Differ[i - 1] + his[i] / total;
                  
                }
                else
                {
                    Differ[0] = his[0] / total;
                }
            }
            bool min=true, max=true;
            for (int i = 0; i < Differ.Length; i++)
            {
                if (min && Differ[i] > radio)
                {
                    Result[0] = i;
                    min = false;
                }
                if (max && Differ[i] > 1-radio)
                {
                    Result[1] = i;
                    max = false;
                }
            }
            return Result;
        }
        public static double[] FormerOp(int []Buffer,double Total)
        {
            double[] Differ = new double[Buffer.Length];
            int t = 0;
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (i != 0)
                {
                    Differ[i] = Differ[i - 1] + Buffer[i] / Total;
                    t += Buffer[i];
                }
                else
                {
                    Differ[i] = Buffer[i] / Total;
                    t = Buffer[0];
                }
            }
            return Differ;
        }
        //直方图匹配
        public static int[] HistMatching(int[] src,int[] matching,double srcTotal,double matchingTotal)
        {
            double[] src_Differ = new double[src.Length];
            double[] matching_Differ = new double[matching.Length];
            for (int i = 0; i < src.Length; i++)
            {
                if (i != 0)
                {
                    src_Differ[i] = src_Differ[i - 1] + src[i] / srcTotal;
                }
                else
                {
                    src_Differ[i] = src[i] / srcTotal;
                }
            }

            for (int i = 0; i < matching.Length; i++)
            {
                if (i != 0)
                {
                    matching_Differ[i] = matching_Differ[i - 1] + matching[i] / matchingTotal;
                }
                else
                {
                    matching_Differ[i] = matching[i] / matchingTotal;
                }
            }
            return( ImageFusion.HistogramMatching(src_Differ, matching_Differ));
        }
        //对于一般数字图像的直方图匹配
        #region
        /*
        /// <summary>
        /// 直方图匹配
        /// </summary>
        /// <param name="srcBmp">原始图像</param>
        /// <param name="matchingBmp">匹配图像</param>
        /// <param name="dstBmp">处理后图像</param>
        /// <returns>处理成功 true 失败 false</returns>
        public static bool HistogramMatching(Bitmap srcBmp, Bitmap matchingBmp, out Bitmap dstBmp)
        {
            if (srcBmp == null || matchingBmp == null)
            {
                dstBmp = null;
                return false;
            }
            dstBmp = new Bitmap(srcBmp);
            Bitmap tempSrcBmp = new Bitmap(srcBmp);
            Bitmap tempMatchingBmp = new Bitmap(matchingBmp);
            double[] srcCpR = null;
            double[] srcCpG = null;
            double[] srcCpB = null;
            double[] matchCpB = null;
            double[] matchCpG = null;
            double[] matchCpR = null;
            //分别计算两幅图像的累计概率分布
            getCumulativeProbabilityRGB(tempSrcBmp, out srcCpR, out srcCpG, out srcCpB);
            getCumulativeProbabilityRGB(tempMatchingBmp, out matchCpR, out matchCpG, out matchCpB);

            double diffAR = 0, diffBR = 0, diffAG = 0, diffBG = 0, diffAB = 0, diffBB = 0;
            byte kR = 0, kG = 0, kB = 0;
            //逆映射函数
            byte[] mapPixelR = new byte[256];
            byte[] mapPixelG = new byte[256];
            byte[] mapPixelB = new byte[256];
            //分别计算RGB三个分量的逆映射函数
            //R
            for (int i = 0; i < 256; i++)
            {
                diffBR = 1;
                for (int j = kR; j < 256; j++)
                {
                    //找到两个累计分布函数中最相似的位置
                    diffAR = Math.Abs(srcCpR[i] - matchCpR[j]);
                    if (diffAR - diffBR < 1.0E-08)
                    {//当两概率之差小于0.000000001时可近似认为相等
                        diffBR = diffAR;
                        //记录下此时的灰度级
                        kR = (byte)j;
                    }
                    else
                    {
                        kR = (byte)Math.Abs(j - 1);
                        break;
                    }
                }
                if (kR == 255)
                {
                    for (int l = i; l < 256; l++)
                    {
                        mapPixelR[l] = kR;
                    }
                    break;
                }
                mapPixelR[i] = kR;
            }
            //G
            for (int i = 0; i < 256; i++)
            {
                diffBG = 1;
                for (int j = kG; j < 256; j++)
                {
                    diffAG = Math.Abs(srcCpG[i] - matchCpG[j]);
                    if (diffAG - diffBG < 1.0E-08)
                    {
                        diffBG = diffAG;
                        kG = (byte)j;
                    }
                    else
                    {
                        kG = (byte)Math.Abs(j - 1);
                        break;
                    }
                }
                if (kG == 255)
                {
                    for (int l = i; l < 256; l++)
                    {
                        mapPixelG[l] = kG;
                    }
                    break;
                }
                mapPixelG[i] = kG;
            }
            //B
            for (int i = 0; i < 256; i++)
            {
                diffBB = 1;
                for (int j = kB; j < 256; j++)
                {
                    diffAB = Math.Abs(srcCpB[i] - matchCpB[j]);
                    if (diffAB - diffBB < 1.0E-08)
                    {
                        diffBB = diffAB;
                        kB = (byte)j;
                    }
                    else
                    {
                        kB = (byte)Math.Abs(j - 1);
                        break;
                    }
                }
                if (kB == 255)
                {
                    for (int l = i; l < 256; l++)
                    {
                        mapPixelB[l] = kB;
                    }
                    break;
                }
                mapPixelB[i] = kB;
            }
            //映射变换
            BitmapData bmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = null;
                for (int i = 0; i < dstBmp.Height; i++)
                {
                    ptr = (byte*)bmpData.Scan0 + i * bmpData.Stride;
                    for (int j = 0; j < dstBmp.Width; j++)
                    {
                        ptr[j * 3 + 2] = mapPixelR[ptr[j * 3 + 2]];
                        ptr[j * 3 + 1] = mapPixelG[ptr[j * 3 + 1]];
                        ptr[j * 3] = mapPixelB[ptr[j * 3]];
                    }
                }
            }
            dstBmp.UnlockBits(bmpData);
            return true;
        }

        /// <summary>
        /// 计算各个图像分量的累计概率分布
        /// </summary>
        /// <param name="srcBmp">原始图像</param>
        /// <param name="cpR">R分量累计概率分布</param>
        /// <param name="cpG">G分量累计概率分布</param>
        /// <param name="cpB">B分量累计概率分布</param>
        private static void getCumulativeProbabilityRGB(Bitmap srcBmp, out double[] cpR, out double[] cpG, out double[] cpB)
        {
            if (srcBmp == null)
            {
                cpB = cpG = cpR = null;
                return;
            }
            cpR = new double[256];
            cpG = new double[256];
            cpB = new double[256];
            int[] hR = null;
            int[] hG = null;
            int[] hB = null;
            double[] tempR = new double[256];
            double[] tempG = new double[256];
            double[] tempB = new double[256];
            getHistogramRGB(srcBmp, out hR, out hG, out hB);
            int totalPxl = srcBmp.Width * srcBmp.Height;
            for (int i = 0; i < 256; i++)
            {
                if (i != 0)
                {
                    tempR[i] = tempR[i - 1] + hR[i];
                    tempG[i] = tempG[i - 1] + hG[i];
                    tempB[i] = tempB[i - 1] + hB[i];
                }
                else
                {
                    tempR[0] = hR[0];
                    tempG[0] = hG[0];
                    tempB[0] = hB[0];
                }
                cpR[i] = (tempR[i] / totalPxl);
                cpG[i] = (tempG[i] / totalPxl);
                cpB[i] = (tempB[i] / totalPxl);
            }
        }

        /// <summary>
        /// 获取图像三个分量的直方图数据
        /// </summary>
        /// <param name="srcBmp">图像</param>
        /// <param name="hR">R分量直方图数据</param>
        /// <param name="hG">G分量直方图数据</param>
        /// <param name="hB">B分量直方图数据</param>
        public static void getHistogramRGB(Bitmap srcBmp, out int[] hR, out int[] hG, out int[] hB)
        {
            if (srcBmp == null)
            {
                hR = hB = hG = null;
                return;
            }
            hR = new int[256];
            hB = new int[256];
            hG = new int[256];
            BitmapData bmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = null;
                for (int i = 0; i < srcBmp.Height; i++)
                {
                    ptr = (byte*)bmpData.Scan0 + i * bmpData.Stride;
                    for (int j = 0; j < srcBmp.Width; j++)
                    {
                        hB[ptr[j * 3]]++;
                        hG[ptr[j * 3 + 1]]++;
                        hR[ptr[j * 3 + 2]]++;
                    }
                }
            }
            srcBmp.UnlockBits(bmpData);
            return;
        }
        */
        #endregion
    }
}
