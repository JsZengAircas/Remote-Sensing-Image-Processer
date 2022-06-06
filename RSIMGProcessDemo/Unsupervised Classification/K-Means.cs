using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
using System.Drawing;

namespace Test.K_Means
{
    //用于排序
    public struct DIS { public int classNo; public double distance; }
    public class K_Means
    {      
        public static List<Color> getColor()
        {
            List<Color> output = new List<Color>();
            output.Add(Color.Red);
            output.Add(Color.Yellow);
            output.Add(Color.Green);
            output.Add(Color.Blue);
            output.Add(Color.GreenYellow);
            output.Add(Color.HotPink);
            output.Add(Color.DarkOrange);
            output.Add(Color.Gray);
            return output;
        }
        public List<Clusters> IniCluster(Dataset ds, int clusterNum,RSImage ima,int BandNumber)
        {
            List<Clusters> Result = new List<Clusters>();
            //存储所有样本      
            int Width = ds.RasterXSize;
            int Height = ds.RasterYSize;
            ima.get_Height = Height;
            ima.get_Width = Width;
            int colorCount = 0;          
                //随机数工具
                Random rndTool = new Random();               
                //随机初始化
                for (int num = 0; num < clusterNum; num++)
                {
                    int row = 0, col = 0, count = 0;
                    //初始化聚类中心
                    while (true)
                    {
                        if (count == 10)
                        {
                            row = row / 10;
                            col = col / 10;
                            break;
                        }
                        row += rndTool.Next(0, Height);
                        col += rndTool.Next(0, Width);
                        count++;
                    }
                    List<double> center = new List<double>();
                    for (int index = 0; index < BandNumber; index++)
                    {
                        center.Add(ima.get_Buffer[index][row * Height + col]);
                    }
                    if (colorCount < 8)
                    {
                        List<Sample> all = new List<Sample>();
                        Clusters cluster = new Clusters(num + 1, getColor()[colorCount++], all, center);
                        Result.Add(cluster);
                    }
                    //  center.Clear();                     
                }
            
            //获取每个像素点在每个波段的值
            List<List<int>> ImgTotalVector = new List<List<int>>();
            //分别计算每个像元至每个类聚中心的距离          
            for (int rowNum = 0; rowNum < Height; rowNum++)
            {
                for (int colNum = 0; colNum < Width; colNum++)
                {
                    List<int> vocter = new List<int>();
                    for (int num = 0; num < BandNumber; num++)
                    {
                        vocter.Add(ima.get_Buffer[num][rowNum*Width+colNum]);
                    }
                    ImgTotalVector.Add(vocter);
                    DIS[] dispose = new DIS[clusterNum];
                    for (int num = 0; num < clusterNum; num++)
                    {
                        dispose[num].classNo = Result[num].No;
                        dispose[num].distance = CalculateDis(Result[num],vocter);            
                    }
                    //赋值
                    Sample sample = new Sample();
                    sample.row = rowNum;
                    sample.col = colNum;
                    sample.imgVector = vocter;
                    //添加
                    int index = SortDis(dispose) - 1;
                    Result[index].allSamples.Add(sample);
                    //vocter.Clear();
                }
            }
            ima.get_TotalVector = ImgTotalVector;
            return Result; 
        }
        public double CalculateDis(Clusters cluster,List<int> sample)
        {
            double result = 0;
            for (int i = 0; i < sample.Count; i++)
            {
                result += Math.Pow((cluster.clusterCenter[i]-sample[i]),2);
            }
            return Math.Sqrt(result);
        }
        public int SortDis(DIS[] data)
        {
            double temp = data[0].distance;
            int index=1;
            for (int i = 0; i < data.Length; i++)
            {
                if (temp > data[i].distance)
                {
                    index = data[i].classNo;
                    temp = data[i].distance;
                }
            }
            return index;
        }
        public List<List<double>> ReCalCenter(List<Clusters> cluster,int bandNum)
        {
            List<List<double>> result = new List<List<double>>();
            for (int num = 0; num < cluster.Count; num++)
            {
                double[] Sum = new double[bandNum];
                if (cluster[num].allSamples.Count == 0)
                {
                    for (int j = 0; j < bandNum; j++)
                    {
                        Sum[j] = cluster[num].clusterCenter[j];
                    }
                }
                else
                {
                    for (int i = 0; i < cluster[num].allSamples.Count; i++)
                    {
                        for (int j = 0; j < bandNum; j++)
                        {
                            Sum[j] += cluster[num].allSamples[i].imgVector[j];
                        }
                    }
                }
                List<double> newCenter = new List<double>();
                // cluster[num].clusterCenter.Clear();
                for (int sumCount = 0; sumCount < bandNum; sumCount++)
                {
                    if (cluster[num].allSamples.Count != 0)
                    {
                        Sum[sumCount] = Sum[sumCount] / cluster[num].allSamples.Count;
                        newCenter.Add(Sum[sumCount]);
                    }
                    else {
                        newCenter.Add(Sum[sumCount]);
                    }  
                }
                result.Add(newCenter);
            }
            return result;
        }
        public List<Clusters> ItreativeCal(List<Clusters> cluster,int MaxItre,double threshold,int bandNum,RSImage ima)
        {
            int ItreateCount = 0;
            List<List<double>> newCenter = ReCalCenter(cluster, bandNum);
            List<Clusters> finalResult = new List<Clusters>();
            while (ItreateCount < MaxItre)
            {
                List<double> changeList = new List<double>();
                for (int i = 0; i < cluster.Count; i++)
                {
                    double temp = 0;
                    for (int j = 0; j < bandNum; j++)
                    {
                        if (cluster[i].clusterCenter[j] <= 1e-7)
                        {
                            temp = Math.Abs(cluster[i].clusterCenter[j] - newCenter[i][j]) / 0.0001;
                        }
                        else {
                            temp = Math.Abs(cluster[i].clusterCenter[j] - newCenter[i][j])/ cluster[i].clusterCenter[j];
                        }
                        changeList.Add(temp);
                    }                 
                }
                double maxChange = changeList[0];
                for (int i = 1; i < changeList.Count; i++)
                {
                    if (changeList[i] > maxChange)
                    {
                        maxChange = changeList[i];
                    }
                }
                //更换聚类中心
                for (int i = 0; i < cluster.Count; i++)
                {
                    //cluster[i].clusterCenter.Clear();
                    cluster[i].clusterCenter=(newCenter[i]);
                }
                //判断是否需要进行迭代
                if (maxChange < threshold)
                {
                    break;
                }
                //清除样本
                for (int num = 0; num < cluster.Count; num++)
                {
                    cluster[num].allSamples.Clear();
                }
                //重新计算各像元属于的类别
                for (int rowNum = 0; rowNum <ima.get_Height ; rowNum++)
                {
                    for (int colNum = 0; colNum < ima.get_Width; colNum++)
                    {
                        DIS[] dispose = new DIS[cluster.Count];
                        for (int num = 0; num < cluster.Count; num++)
                        {
                            dispose[num].classNo = cluster[num].No;
                            dispose[num].distance = CalculateDis(cluster[num], ima.get_TotalVector[ima.get_Width*rowNum+colNum]);
                        }
                        //赋值
                        Sample sample = new Sample();
                        sample.row = rowNum;
                        sample.col = colNum;
                        sample.imgVector = ima.get_TotalVector[ima.get_Width * rowNum + colNum];
                        //添加
                        cluster[SortDis(dispose)-1].allSamples.Add(sample);
                    }
                }
                ItreateCount++;
            }
            return cluster;
        }

    }
}
