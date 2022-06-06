using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.K_Means;

namespace Test.Supervised_Classification
{
   public class ROI
    {
        public int No;
        public string Name;
        public Point[] rectangleBit = new Point[2];//Point[0]为左上角点，Point[2]右下角，bitmap
        public Point[] rectangleData = new Point[2];//Point[0]为左上角点，Point[2]右下角,生成的影像
        public Color colorType;
        public List<Sample> contentSample = new List<Sample>();//存储相应的像元
        public int Num;
        public List<double> averageList = new List<double>();
    }
}
