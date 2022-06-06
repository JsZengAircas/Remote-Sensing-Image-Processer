using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
namespace Test
{
   public class ImageFusion
    {
       public static  List<int[]> IHS = new List<int[]>();
        #region
        /// <summary>
        /// RGB空间换至IHS空间
        /// </summary>
        /// <param name="msi_Data">多光谱影像数据源</param>
        /// <param name="ima">读取用户选定的波段</param>
        /// <returns></returns>
        public static Dataset Msi_RGB_To_IHS(Dataset msi_Data)
        {
            Gdal.AllRegister();         
            double[,] Matrix_A = new double[3, 3];
            int Count = 1;
            Matrix_A[0, 0] = Matrix_A[0, 1] = Matrix_A[0, 2]=(double) (1 / 3.0);
            Matrix_A[1, 0] = Matrix_A[1, 1] = (-Math.Sqrt(2))/ 6; Matrix_A[1, 2] = (2 * Math.Sqrt(2)) / 6;
            Matrix_A[2, 0] = 1 / Math.Sqrt(2); Matrix_A[2, 1] =-(1 / Math.Sqrt(2)); Matrix_A[2, 2] = 0;
            int[] R = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            int[] G = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            int[] B = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
           msi_Data.GetRasterBand(Count++).ReadRaster(0,0, msi_Data.RasterXSize , msi_Data.RasterYSize,R, msi_Data.RasterXSize, msi_Data.RasterYSize,0,0);
            msi_Data.GetRasterBand(Count++).ReadRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, G, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
            msi_Data.GetRasterBand(Count++).ReadRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, B, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
            int []I=new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            int[] H = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            int[] S = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];

            int[] v1 = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            int[] v2 = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];

            for (int row = 0; row < msi_Data.RasterYSize; row++)
            {
                for (int col = 0; col < msi_Data.RasterXSize; col++)
                {
                    Matrix rgb_Ma = new Matrix(3, 1);
                    Matrix ihs_Ma = new Matrix(3, 1);
                    Matrix A = new Matrix(3, 3);
                    A.Detail = Matrix_A;
                    rgb_Ma.Detail[0, 0] = R[row * msi_Data.RasterXSize + col];
                    rgb_Ma.Detail[1, 0] = G[row * msi_Data.RasterXSize + col];
                    rgb_Ma.Detail[2, 0] = B[row * msi_Data.RasterXSize + col];
                    ihs_Ma = MatrixOpera.MatrixMulti(A, rgb_Ma);
                    I[row * msi_Data.RasterXSize + col] =Convert.ToInt32(ihs_Ma.Detail[0, 0]);
                    v1[row * msi_Data.RasterXSize + col]= Convert.ToInt32(ihs_Ma.Detail[1, 0]);
                    v2[row * msi_Data.RasterXSize + col] = Convert.ToInt32(ihs_Ma.Detail[2, 0]);
                }
            }
            IHS.Add(I);
            IHS.Add(v1);
            IHS.Add(v2);
            int index = 1;
            msi_Data.GetRasterBand(index).WriteRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, I, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
           // msi_Data.GetRasterBand(ima.get_BandList[index++]).WriteRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, H, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
            //msi_Data.GetRasterBand(ima.get_BandList[index++]).WriteRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, S, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
            return msi_Data;
        }
        /// <summary>
        /// IHS逆变换
        /// </summary>
        /// <param name="IHS"></param>
        /// <param name="msi_Data"></param>
        /// <returns></returns>
        public static Dataset IHS_To_RGB(Dataset msi_Data)
        {
            Gdal.AllRegister();
            double[,] Matrix_A = new double[3, 3];
            Matrix_A[0, 0] = 1; Matrix_A[0, 1] = -1 / Math.Sqrt(2); Matrix_A[0, 2] = 1 / Math.Sqrt(2);
            Matrix_A[1, 0] = 1 ; Matrix_A[1, 1] = -1 / Math.Sqrt(2); Matrix_A[1, 2] = -1 / Math.Sqrt(2);
            Matrix_A[2, 0] = 1; Matrix_A[2, 1] = Math.Sqrt(2); Matrix_A[2, 2] = 0;
            int[] R = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            int[] G = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            int[] B = new int[msi_Data.RasterXSize * msi_Data.RasterYSize];
            for (int row = 0; row < msi_Data.RasterYSize; row++)
            {
                for (int col = 0; col < msi_Data.RasterXSize; col++)
                {
                    Matrix rgb_Ma = new Matrix(3, 1);
                    Matrix ihs_Ma = new Matrix(3, 1);
                    Matrix A = new Matrix(3, 3);
                    A.Detail = Matrix_A;
                    ihs_Ma.Detail[0, 0] = IHS[0][row * msi_Data.RasterXSize + col];
                    ihs_Ma.Detail[1, 0] = IHS[1][row * msi_Data.RasterXSize + col];
                    ihs_Ma.Detail[2, 0] = IHS[2][row * msi_Data.RasterXSize + col];
                    ihs_Ma = MatrixOpera.MatrixMulti(A, ihs_Ma);
                    R[row * msi_Data.RasterXSize + col] = Convert.ToInt32(ihs_Ma.Detail[0, 0]);                 
                    G[row * msi_Data.RasterXSize + col] = Convert.ToInt32(ihs_Ma.Detail[1, 0]);
                    B[row * msi_Data.RasterXSize + col] = Convert.ToInt32(ihs_Ma.Detail[2, 0]);
                }
            }
            msi_Data.GetRasterBand(1).WriteRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, R, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
            msi_Data.GetRasterBand(2).WriteRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, G, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
            msi_Data.GetRasterBand(3).WriteRaster(0, 0, msi_Data.RasterXSize, msi_Data.RasterYSize, B, msi_Data.RasterXSize, msi_Data.RasterYSize, 0, 0);
            return msi_Data;
        }
        public static Dataset msi_Resample(Dataset msi_Data,int radio,RSImage ima,Dataset resample)
        {
            Gdal.AllRegister();
            int re_Inwidth = msi_Data.RasterXSize * radio;
            int re_Inheight = msi_Data.RasterYSize * radio;
            int rowCount = 0;
            int colCount = 0;
            for (int i = 0; i < resample.RasterCount; i++)
            {
                rowCount = 0;
                int[] buffer = new int[re_Inheight*re_Inwidth];
                int[] ori_Buffer = new int[msi_Data.RasterXSize*msi_Data.RasterYSize];
                msi_Data.GetRasterBand(ima.get_BandList[i]).ReadRaster(0,0,msi_Data.RasterXSize,msi_Data.RasterYSize,ori_Buffer, msi_Data.RasterXSize, msi_Data.RasterYSize,0,0);
                for (int row = 0; row < resample.RasterXSize; row++)
                {
                    if (row != 0 && (row % (radio) == 0)) rowCount++;
                    colCount = 0;
                    for (int col = 0; col < resample.RasterYSize; col++)
                    {
                        if (col != 0 && (col % (radio) == 0))
                        {
                            colCount++;
                        }
                        buffer[row * re_Inwidth + col] = ori_Buffer[rowCount * msi_Data.RasterXSize + colCount];                     
                    }
                }
                resample.GetRasterBand(i + 1).WriteRaster(0, 0, re_Inwidth, re_Inheight, buffer, re_Inwidth, re_Inheight, 0, 0);
                
            }
            return resample;
        }

        /// <summary>
        /// 累积概率表匹配映射。
        /// </summary>
        /// <param name="InputCumuProba">输入的累积概率表。</param>
        /// <param name="MatchingCumuProba">要匹配到的累积概率表。</param>
        /// <returns>匹配结果。长度与输入的累积概率表相同，内容为输入概率表索引映射到要匹配的概率表的索引。</returns>
        public static int[] HistogramMatching(double[] InputCumuProba, double[] MatchingCumuProba)
        {
            int[] lut = new int[InputCumuProba.Length];
            for (int i = 0; i < InputCumuProba.Length; i++)
            {
                double[] minus = new double[MatchingCumuProba.Length];
                //计算匹配到的概率表各项与输入概率表一项的差。
                for (int j = 0; j < MatchingCumuProba.Length; j++)
                {
                    minus[j] = Math.Abs(InputCumuProba[i] - MatchingCumuProba[j]);
                }
                double min = 32767;
                int minIndex = 0;
                //计算差值最小的作为对应项。
                for (int j = 0; j < MatchingCumuProba.Length; j++)
                {
                    if (minus[j] < min)
                    {
                        min = minus[j];
                        minIndex = j;
                    }
                }
                lut[i] = minIndex;
            }
            return lut;
        }


        #endregion
    }
}
