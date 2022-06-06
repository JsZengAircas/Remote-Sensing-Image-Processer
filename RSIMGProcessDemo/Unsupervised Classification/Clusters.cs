using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.K_Means
{
    public struct Sample
    {
        //获取位置
        public int row;
        public int col;
        //获取特征向量
        public List<int> imgVector;
    }
    public class Clusters
    {   
        public int No { get; set; }
        //获取颜色类别
        public Color colorType { get; set; }
        //单个特征向量
      
        //所有的特征向量
        public List<Sample> allSamples = new List<Sample>();
        //获取聚类中心
        public List<double> clusterCenter = new List<double>();
        //初始化
        public Clusters(int no, Color color,List<Sample> all,List<double> center)
        {
            colorType = color;
            allSamples = all;
            clusterCenter = center;
            No = no;
        }
    }
}
