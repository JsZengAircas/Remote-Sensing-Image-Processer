using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Space_ResectionCal
{
    public class BaseCal
    {
        public string s = "";
        public DATA Rotation_Ma_Cal(double phi, double omega, double kappa)
        {
            DATA dd = new DATA();
            //计算各元素
            double a1 = Math.Cos(phi) * Math.Cos(kappa) - Math.Sin(phi) * Math.Sin(omega) * Math.Sin(kappa);
            dd.Rotation_Ma[0,0]= a1;
            double a2=-Math.Cos(phi) * Math.Sin(kappa)- Math.Sin(phi) * Math.Sin(omega) * Math.Cos(kappa);
            dd.Rotation_Ma[0, 1] = a2;
            double a3 = -Math.Sin(phi) * Math.Cos(omega);
            dd.Rotation_Ma[0, 2] = a3;

            double b1 = Math.Cos(omega) * Math.Sin(kappa);
            dd.Rotation_Ma[1, 0] = b1;
            double b2 = Math.Cos(omega) * Math.Cos(kappa);
            dd.Rotation_Ma[1, 1] = b2;
            double b3 = -Math.Sin(omega);
            dd.Rotation_Ma[1, 2] = b3;

            double c1 = Math.Sin(phi) * Math.Cos(kappa) + Math.Cos(phi) * Math.Sin(omega) * Math.Sin(kappa);
            dd.Rotation_Ma[2, 0] = c1;
            double c2 = -Math.Sin(phi) * Math.Sin(kappa) + Math.Cos(phi) * Math.Sin(omega) * Math.Cos(kappa);
            dd.Rotation_Ma[2, 1] = c2;
            double c3 = Math.Cos(phi) * Math.Cos(omega);
            dd.Rotation_Ma[2, 2] = c3;
           
            return dd;

        }
       public DATA Cal_origal(DATA a)
        {
            DATA OX = new DATA();
            double Xso = 0, Yso = 0, Zso = 0;
            DATA o = new DATA();
            int count = a.Ground_Cordinate_x.Count;           
            for (int i = 0; i < count; i++)
            {

                Xso = Xso + a.Ground_Cordinate[0][i];
                Yso = Yso + a.Ground_Cordinate[1][i];
                Zso = Zso + a.Ground_Cordinate[2][i];
            }
            Xso = Xso / count;
            Yso = Yso / count;
            Zso = o.GetScale * o.GetIntElem_f + Zso / count;
            OX.Orignal_o.Add(Xso);
            OX.Orignal_o.Add(Yso);
            OX.Orignal_o.Add(Zso);
           
            return OX;
        }
        public DATA Appro_Data(DATA EQU,DATA OX,DATA GC,DATA DP)
        {

            DATA o = new DATA();
            int co = GC.Ground_Cordinate_x.Count();

            for (int i = 0; i <co ; i++)
            {
                double Lx, Ly,a,b,c;
                a = EQU.Rotation_Ma[0, 0] * (GC.Ground_Cordinate[0][i] - OX.Orignal_o[0]) + EQU.Rotation_Ma[1, 0] * (GC.Ground_Cordinate[1][i] - OX.Orignal_o[1]) + EQU.Rotation_Ma[2, 0] * (GC.Ground_Cordinate[2][i] - OX.Orignal_o[2]);
                b = EQU.Rotation_Ma[0, 1] * (GC.Ground_Cordinate[0][i] - OX.Orignal_o[0]) + EQU.Rotation_Ma[1, 1] * (GC.Ground_Cordinate[1][i] - OX.Orignal_o[1]) + EQU.Rotation_Ma[2, 1] * (GC.Ground_Cordinate[2][i] - OX.Orignal_o[2]);
                c = EQU.Rotation_Ma[0, 2] * (GC.Ground_Cordinate[0][i] - OX.Orignal_o[0]) + EQU.Rotation_Ma[1, 2] * (GC.Ground_Cordinate[1][i] - OX.Orignal_o[1]) + EQU.Rotation_Ma[2, 2] * (GC.Ground_Cordinate[2][i] - OX.Orignal_o[2]);
                Lx = DP.Pixel_Cordinate[0][i] + o.GetIntElem_f * (a) / c;
                Ly = DP.Pixel_Cordinate[1][i] + o.GetIntElem_f * (b) / c;
                
                o.AL.Add (Lx);
                o.AL.Add(Ly);
            }
            return o;

        }
        public DATA AccuracyCal(DATA Internment, Matrix Re,DATA L)
        {
            DATA XGM = new DATA();
            Matrix MulMa = new Matrix(Internment.InternElement_o.Count,1);
            Matrix Intern = new Matrix(Internment.InternElement_o.Count, 6);
            Matrix Re_x = new Matrix(6,1);
            Matrix Lt = new Matrix(Internment.InternElement_o.Count, 1);
            double[,] d = new double[Internment.InternElement_o.Count, 6];
            double[,] k = new double[6,1];
            double[,] l = new double[Internment.InternElement_o.Count, 1];
            for (int i = 0; i < Internment.InternElement_o.Count; i++)
                for (int j = 0; j < 6; j++)
                {
                    d[i, j] = Internment.InternElement[j][i];

                }
            Intern.Detail = d;
           
                Lt.Detail = l;//L矩阵赋值
                k = Re.Detail;
            Re_x.Detail = k;//x矩阵赋值

            MulMa = MatrixOpera.MatrixMulti(Intern, Re_x);//计算
            Matrix V_Ma = new Matrix(Internment.InternElement_o.Count, 1);
            V_Ma = MatrixOpera.MatrixSub(MulMa, Lt);

            Matrix Trans_V_Ma = new Matrix(6,Internment.InternElement_o.Count);
            Trans_V_Ma = MatrixOpera.MatrixTrans(V_Ma);

            Matrix Xg = new Matrix(1, 1);
            Xg = MatrixOpera.MatrixMulti(Trans_V_Ma, V_Ma);//计算

            int count = 0;
            count = Internment.InternElement_o.Count - 6;

            double[,] mo = new double[1, 1];
            mo = Xg.Detail;
            mo[0, 0] = (mo[0, 0] /count);
            s = s + Convert.ToString(mo[0, 0]) + "//";
            //////////////////////////////////////////////////////计算At*A-1
            Matrix tans_m = new Matrix(Internment.InternElement_o.Count,6);
            tans_m = MatrixOpera.MatrixTrans(Intern);
            Matrix mul= new Matrix(6, 6);
            mul = MatrixOpera.MatrixMulti(tans_m, Intern);
            Matrix Dmul = new Matrix(6, 6);
            Dmul = MatrixOpera.MatrixInvByCom(mul);
            double[,] RE = new double[6, 6];
            RE = Dmul.Detail;
            for (int i = 0; i < 6; i++)
                    XGM.Accuracy.Add(RE[i, i]*mo[0,0]);
            return XGM;

                    











        }

        
        
    }
   
}
