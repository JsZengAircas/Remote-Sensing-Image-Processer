using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.K_Means;

namespace Test.Unsupervised_Classification
{
  public  class ISOData
    {
     //   public struct DIS { public int classNo; public double distance; }
        /// <summary>
        /// 颜色选择器
        /// </summary>
        /// <returns></returns>
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
        #region
        /// <summary>
        /// 定义相关指标
        /// </summary类别数目、阈值范围、最大迭代次数、聚类最小像素数、类间最大方差、类间最小聚类/>
        private int _minClass,_maxClass, _minPix, _maxStd, _minDis;private double _classThres, _maxItre;
        public int classNumMax { set { _maxClass = value; } get { return _maxClass; } }
        public int classNumMin { set { _minClass = value; } get { return _minClass; } }
        public double changeThreshold { set { _classThres = value; } get { return _classThres; } }
        public double  maxItreNum { set { _maxItre = value; } get { return _maxItre; } }
        public int minPixInClass { set { _minPix = value; } get { return _minPix; } }
        public int maxStdvInClass { set { _maxStd = value; } get { return _maxStd; } }
        public int minDisInClass { set { _minDis = value; } get { return _minDis; } }
        #endregion
        K_Means.K_Means KM = new K_Means.K_Means();
        public List<Clusters> CalCLusters_ISOData(Dataset oriData,int bandNum,RSImage ima)
        {        
            //首先随意设定几个类聚中心，进行初始化
            Random rndTool = new Random();
            int clusterNum = rndTool.Next(_minClass, _maxClass);
            List<Clusters> IniCluster =KM.IniCluster(oriData,clusterNum,ima,bandNum);
            // 去除少于要求的类聚中心
            for (int i = 0; i < IniCluster.Count; i++)
            {
                if (IniCluster[i].allSamples.Count < _minPix)
                {
                    IniCluster.Remove(IniCluster[i]);
                }
            }
            List<Clusters> _Cluster = IniCluster;
            int ItreCount = 0;
            while (ItreCount < _maxItre)
            {
                //更改聚类中心
            List<List<double>> newCenter = KM.ReCalCenter(_Cluster, bandNum);      
            for (int i = 0; i < IniCluster.Count; i++)
            {
                _Cluster[i].clusterCenter = newCenter[i];
            }
            //计算每个像元至各类聚中心的平均距离
            List<double> centerToPixDis = CalDisCenterToPix(_Cluster);
                //计算平均距离
                double totalDis = 0;
                for (int i = 0; i < centerToPixDis.Count; i++)
                {
                    totalDis += centerToPixDis[i];
                }
                totalDis /= centerToPixDis.Count;
            //判断是否为最后一次迭代

                //迭代次数
                if (ItreCount % 2 == 0 && _Cluster.Count > 2 * _maxClass && ItreCount == _maxItre - 1)
                {
                    int Num = _Cluster.Count;
                    List<double[]> centerDis = new List<double[]>();
                    //分别计算各类聚中心的距离
                    for (int i = 0; i < _Cluster.Count; i++)
                    {
                        for (int j = 0; j < _Cluster.Count; j++)
                        {
                            double tempDis = CalDistance(_Cluster[i].clusterCenter, _Cluster[j].clusterCenter);
                            if (tempDis < _minDis)
                            {
                                double[] per = new double[3];
                                per[0] = tempDis;
                                per[1] = i;
                                per[2] = j;
                                centerDis.Add(per);
                            }
                        }
                    }
                    //选出最小的那个
                    double[] Combine = new double[3];
                    if (centerDis.Count != 0)
                    {
                        Combine = centerDis[0];
                        for (int i = 1; i < centerDis.Count; i++)
                        {
                            if (Combine[0] > centerDis[i][0])
                            {
                                Combine = centerDis[i];
                            }
                        }
                    }
                    //开始合并
                    int oneIndex = Convert.ToInt32(Combine[1]);
                    int twoIndex = Convert.ToInt32(Combine[2]);
                    Clusters newCluster = _Cluster[oneIndex];
                    for (int i = 0; i < newCluster.clusterCenter.Count; i++)
                    {
                        newCluster.clusterCenter[i] = (_Cluster[oneIndex].clusterCenter[i] * _Cluster[oneIndex].allSamples.Count + _Cluster[twoIndex].clusterCenter[i] * _Cluster[twoIndex].allSamples.Count)
                            / (_Cluster[twoIndex].allSamples.Count * _Cluster[oneIndex].allSamples.Count);
                    }
                    _Cluster.Remove(_Cluster[oneIndex]);
                    _Cluster.Remove(_Cluster[twoIndex]);
                    _Cluster.Add(newCluster);
                    for (int i = 0; i < _Cluster.Count; i++)
                    {
                        _Cluster[i].No = i+1;
                    }
                    if (ItreCount == _maxItre - 1)
                    {
                        break;
                    }
                }
                else
                {
                    //分裂
                    //（1）计算每个类聚中心的标准差
                    List<List<double>> clusterStdv = CalPerClusterSdtv(_Cluster, bandNum);
                    //(2)选择其中最大的一个
                    //  用于记录需要处理的聚类中心
                    List<double[]> markIndex = new List<double[]>();
                    //找出要分裂的聚类
                    for (int i = 0; i < clusterStdv.Count; i++)
                    {
                        double temp = clusterStdv[i][0];
                        int No = 0;
                        for (int j = 1; j < clusterStdv[i].Count; j++)
                        {
                            if (temp < clusterStdv[i][j])
                            {
                                temp = clusterStdv[i][j];
                                No = j;
                            }
                        }
                        if ((temp > _maxStd) && (_Cluster[i].allSamples.Count > (2 * (minPixInClass + 1))&&totalDis<centerToPixDis[i]) || (_Cluster.Count < _minClass))
                        {
                            double[] buf = new double[3];
                            buf[0] = i;
                            buf[1] = No;
                            buf[2] = temp;
                            markIndex.Add(buf);
                        }
                    }
                    if (markIndex.Count != 0)
                    {
                        for (int i = 0; i < markIndex.Count; i++)
                        {
                            int index = Convert.ToInt32(markIndex[i][0]);
                            Clusters One = _Cluster[index];
                            Clusters Two = _Cluster[index];
                            One.clusterCenter[Convert.ToInt32(markIndex[i][1])] += 0.5 * markIndex[i][2];
                            Two.clusterCenter[Convert.ToInt32(markIndex[i][1])] -= 0.5 * markIndex[i][2];
                            _Cluster.Remove(_Cluster[index]);
                            _Cluster.Add(One);
                            _Cluster.Add(Two);
                        }
                    }
                    for (int i = 0; i < _Cluster.Count; i++)
                    {
                        _Cluster[0].No = i+1;
                    }
                }
                for (int i = 0; i < _Cluster.Count; i++)
                {
                    _Cluster[i].No = i + 1;
                }
                //重新计算各像元属于的类别
                for (int i = 0; i < _Cluster.Count; i++)
                {
                    _Cluster[i].allSamples.Clear();
                }
                for (int rowNum = 0; rowNum < ima.get_Height; rowNum++)
                {
                    for (int colNum = 0; colNum < ima.get_Width; colNum++)
                    {
                       DIS[] dispose = new DIS[_Cluster.Count];
                        for (int num = 0; num < _Cluster.Count; num++)
                        {
                            dispose[num].classNo = _Cluster[num].No;
                            dispose[num].distance =KM. CalculateDis(_Cluster[num], ima.get_TotalVector[ima.get_Width * rowNum + colNum]);
                        }
                        //赋值
                        Sample sample = new Sample();
                        sample.row = rowNum;
                        sample.col = colNum;
                        sample.imgVector = ima.get_TotalVector[ima.get_Width * rowNum + colNum];
                        //添加
                        _Cluster[KM.SortDis(dispose)-1].allSamples.Add(sample);
                    }
                }
                ItreCount++;
            }
            for (int i = 0; i < _Cluster.Count; i++)
            {
                _Cluster[i].No = i + 1;
            }
            return _Cluster;
        }
        /// <summary>
        ///   //计算每个像元至类聚中心的距离
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public List<double> CalDisCenterToPix(List<Clusters> cluster)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < cluster.Count; i++)
            {
                double tempDis = 0;
                for (int j = 0; j < cluster[i].allSamples.Count; j++)
                {
                    double  tempPerDis = 0;
                    for (int band = 0; band < cluster[i].allSamples[j].imgVector.Count; band++)
                    {
                        tempPerDis += Math.Pow(Math.Abs(cluster[i].allSamples[j].imgVector[band]-cluster[i].clusterCenter[band]),2);
                    }
                    tempPerDis =Math.Sqrt(tempPerDis)/ cluster[i].clusterCenter.Count;
                    tempDis += tempPerDis;
                }
                tempDis /= cluster[i].allSamples.Count;
                result.Add(tempDis);
            }
            return result;
        }
        /// <summary>
        /// 返回计算全部模式样本和其对应聚类中心的总平均距离
        /// </summary>
        /// <param name="perDis"></param>
        /// <returns></returns>
        public double CalAverageDis(List<double> perDis)
        {
            double result = 0;
            for (int i = 0; i < perDis.Count; i++)
            {
                result += perDis[i];
            }
            return result / perDis.Count;
        }
        /// <summary>
        /// 计算每个类聚中心标准差
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="bandNum"></param>
        /// <returns></returns>
        public  List<List<double>> CalPerClusterSdtv(List<Clusters> cluster,int bandNum)
        {
            List<List<double>> result = new List<List<double>>();
            for (int i = 0; i < cluster.Count; i++)
            {
                List<double> perBandStdv = new List<double>();
                double[] eachBand = new double[bandNum];
                for (int j = 0; j < cluster[i].allSamples.Count; j++)
                {                 
                    for (int band = 0; band < bandNum; band++)
                    {
                        eachBand[band] += Math.Pow((cluster[i].allSamples[j].imgVector[band] - cluster[i].clusterCenter[band]), 2);
                    }
                }
                for (int no = 0; no < eachBand.Length; no++)
                {
                    eachBand[no] =Math.Sqrt(eachBand[no])/ cluster[i].allSamples.Count;
                    perBandStdv.Add(eachBand[no]);
                }
                result.Add(perBandStdv);
            }
            return result;
        }
        public double CalDistance(List<double> clu1, List<double> clu2)
        {
            double result=0;
            double[] buffer = new double[clu1.Count];
            for (int i = 0; i < clu1.Count; i++)
            {
                buffer[i] =Math.Sqrt( Math.Pow(clu1[i]-clu2[i],2));
            }
            for (int i = 0; i < clu1.Count; i++)
            {
                result += buffer[i];
            }
            return Math.Sqrt(result) / clu1.Count;
        }
        
    }
}
