using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSGeo.GDAL;
using System.Drawing.Imaging;

namespace Test
{
    public partial class Image_Registration : Form
    {
        bool startEdit = false;
        bool finishEdit= true;
        bool editIndexPix1 = true;
        bool editIndexPix2 = false;
        //创建BaseOperate对象
        RSImage Image_Reg = new Test.RSImage();
        List<int> bandList = new List<int>();
        int index = 1;
        Dataset Standard_Image;
        Dataset Registration_Image;
        string saveFileName;
        RSImage SImage = new RSImage();
        RSImage RImage = new RSImage();
        List<dataList> registrationData = new List<dataList>();
        dataList temp;
        public Image_Registration G { get; set; }
        Bitmap reBitmap, stBitmap;
        public Image_Registration()
        {
            InitializeComponent();
        }
        Registration_statistics tempReg;
        private void 查看数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Registration_statistics Re = new Registration_statistics(registrationData,G);
            tempReg = Re;
            Re.Show();
        }
        
        private void 读取待配准影像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            string o_FileName = RSImage.ReadFileName();          
            if (o_FileName != null)
            {
                Registration_Image = OSGeo.GDAL.Gdal.Open(o_FileName, Access.GA_ReadOnly);
                RImage.bandListIni();
                RImage.ChooseBandList(Registration_Image, RImage);
                 reBitmap= RImage.GetImage(Registration_Image, pictureBox2);
                pictureBox2.Image = RImage.GetImage(Registration_Image, pictureBox2);
            }                    
        }

        private void 读取标准影像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            string sd_FileName = RSImage.ReadFileName();
            if (sd_FileName != null)
            {
                 Standard_Image = Gdal.Open(sd_FileName, Access.GA_ReadOnly);
                SImage.bandListIni();
                SImage.ChooseBandList(Standard_Image, SImage);
                stBitmap = SImage.GetImage(Standard_Image, pictureBox1);
                pictureBox1.Image = SImage.GetImage(Standard_Image, pictureBox1);
            }      
        }

        private void 开始编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startEdit = true;
            finishEdit = false;
            pictureBox1.Cursor = Cursors.Cross;
            pictureBox2.Cursor= Cursors.Cross;
            toolStripStatusLabel2.Text = "开始编辑";           
        }

        private void 停止编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startEdit = false;
            finishEdit = true;
            pictureBox1.Cursor = Cursors.Default;
            pictureBox2.Cursor = Cursors.Default;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (startEdit&&editIndexPix1)
            {
                toolStripStatusLabel2.Text = "标准影像坐标：X：" + e.X.ToString() + ";Y:" + e.Y + ";";
            }
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (startEdit&&editIndexPix2)
            {
                toolStripStatusLabel2.Text = "待配准影像坐标：X：" + e.X.ToString() + ";Y:" + e.Y + ";";
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {          
            if (startEdit&&editIndexPix1)
            {
                temp = new dataList();
                temp.Base_X = e.X;
                temp.Base_Y= e.Y;
                temp.No = index++;
                toolStripStatusLabel2.Text = "当前输入坐标：X:" + e.X.ToString() + ";Y:" + e.Y + ";";
                DrawArrow(pictureBox1, e, Color.Green,temp.No);
                editIndexPix1 = false;
                editIndexPix2 = true;
            }
            
        }
        private void DrawArrow(PictureBox pic, MouseEventArgs e,Color cor,int index)
        {
            Bitmap image = (Bitmap)(pic.Image);
            Graphics g = pic.CreateGraphics();//在picture上绘制
            g = Graphics.FromImage(image);
            Pen drawPen = new Pen(cor);
            //绘制竖线
            Point Px1 = new Point(e.X, e.Y + 5);
            Point Px2 = new Point(e.X, e.Y - 5);
            g.DrawLine(drawPen, Px1, Px2);
            //绘制横线
            Point Py1 = new Point(e.X + 5, e.Y);
            Point Py2 = new Point(e.X - 5, e.Y);
            g.DrawLine(drawPen, Py1, Py2);
            //绘制文字
            Font ft = new Font("宋体", 10);
            Point Ite = new Point(e.X + 5, e.Y + 5);
            g.DrawString("P" + index.ToString(), ft, Brushes.Green, Ite);
            g.Dispose();
            pic.Image = image;
        }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (startEdit&&editIndexPix2)
            {
                temp.Warp_Y = e.Y;
                temp.Warp_X = e.X;
                registrationData.Add(temp);                  
                toolStripStatusLabel2.Text = "当前输入坐标：X：" + e.X.ToString() + ";Y:" + e.Y + ";";
                DrawArrow(pictureBox2, e, Color.Red, temp.No);
                editIndexPix1 = true;
                editIndexPix2 = false;
                if (temp.No >= 3)
                {
                    double[,] X_x = TransformFuction(registrationData);
                    for (int i = 0; i < registrationData.Count; i++)
                    {
                        registrationData[i].Predict_X= (X_x[0, 0] +X_x[1, 0] * registrationData[i].Warp_X +X_x[2, 0] * registrationData[i].Warp_Y);
                        registrationData[i].Predict_Y = (X_x[0, 1] + X_x[1, 1] * registrationData[i].Warp_X + X_x[2, 1] * registrationData[i].Warp_Y);
                    }
                    if (tempReg != null)
                    {
                        tempReg.displayData(registrationData);
                    }
                }
            }
        }
        #region
        private double [,] TransformFuction(List<dataList> List)
        {
            //一次多项式
            double[,] delta = new double[3, 2];
            double[,] pLArr = new double[List.Count, 3];
            for (int i = 0; i < List.Count; i++)
            {            
                pLArr[i, 0] = 1;
                pLArr[i, 1] = List[i].Base_X;
                pLArr[i, 2] = List[i].Base_Y;
            }
            double[,] rLArr = new double[List.Count,2];
                for (int i = 0; i < List.Count; i++)
                {
                rLArr[i, 0] = List[i].Warp_X;
                rLArr[i, 1] = List[i].Warp_Y;
                }
     
            Matrix A = new Matrix(List.Count, 3);
            A.Detail = pLArr;
            Matrix L = new Matrix(List.Count,2);
            L.Detail = rLArr;
            Matrix Res = new Matrix(3,2);
            Res = MatrixOpera.MatrixMulti(MatrixOpera.MatrixMulti(MatrixOpera.MatrixInvByCom(MatrixOpera.MatrixMulti(MatrixOpera.MatrixTrans(A), A)), MatrixOpera.MatrixTrans(A)), L);
            delta = Res.Detail;
            //确定输出范围
            return delta;
        }
        private double[,] AffineTrans(double [,]delta,int [,]dispose)
        {
            double [,]Res = new  double[4,2];
            for (int i = 0; i < 4; i++)
            {
                Res[i, 0] =(delta[0, 0] + delta[1, 0] * dispose[i, 1] + delta[2, 0] * dispose[i, 0]);
                Res[i, 1] =(delta[0, 1] + delta[1, 1] * dispose[i, 1] + delta[2, 1] * dispose[i, 0]);
            }
            return Res;
        }
        #endregion
        public void changeDataset(int row)
        {
            registrationData.Remove(registrationData[row]);
            //重置序号
            for (int i = 0; i < registrationData.Count; i++)
            {
                registrationData[i].No = i + 1;
            }
            //更新列表
            if (tempReg != null)
            {
                tempReg.displayData(registrationData);
            }
            //重绘标志
            Bitmap temp_re = reBitmap;
            Bitmap temp_st = stBitmap;
            for (int i = 0; i < registrationData.Count; i++)
            {
                Draw(Convert.ToInt32(registrationData[i].Base_X),Convert.ToInt32(registrationData[i].Base_Y),registrationData[i].No,temp_st,pictureBox1);
                Draw(Convert.ToInt32(registrationData[i].Warp_X), Convert.ToInt32(registrationData[i].Warp_Y), registrationData[i].No,temp_re, pictureBox2);
            }
            pictureBox1.Image = temp_st;
            pictureBox2.Image = temp_re;
        }
        public void Draw(int X,int Y,int No,Bitmap bitm,PictureBox pic)
        {          
                Bitmap image = (Bitmap)(bitm);
                Graphics g = pic.CreateGraphics();//在picture上绘制
                g = Graphics.FromImage(image);
                Pen drawPen = new Pen(Color.Blue);
                //绘制竖线
                Point Px1 = new Point(X, Y + 5);
                Point Px2 = new Point(X, Y - 5);
                g.DrawLine(drawPen, Px1, Px2);
                //绘制横线
                Point Py1 = new Point(X + 5, Y);
                Point Py2 = new Point(X - 5, Y);
                g.DrawLine(drawPen, Py1, Py2);
                //绘制文字
                Font ft = new Font("宋体", 10);
                Point Ite = new Point(X + 5, Y + 5);
                g.DrawString("P" +No.ToString() , ft, Brushes.Green, Ite);
                g.Dispose();
        }
        private void 几何精校正ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileName != null)
            {
                toolStripStatusLabel2.Text = "开始校正";
                progressBar1.Value = 0;
                double[,] X_x = TransformFuction(registrationData);//X->x
                                                                   //转换
                List<dataList> invData = registrationData;
                for (int i = 0; i < registrationData.Count; i++)
                {
                    invData[i].Base_X = registrationData[i].Warp_X;
                    invData[i].Base_Y = registrationData[i].Warp_Y;
                    invData[i].Warp_X = registrationData[i].Base_X;
                    invData[i].Warp_Y = registrationData[i].Base_Y;
                }
                double[,] x_X = TransformFuction(invData);//x->X
                int res_Width = Registration_Image.RasterXSize;
                int res_Height = Registration_Image.RasterYSize;
                int[,] range = new int[4, 2];
                range[0, 0] = 0; range[0, 1] = 0;
                range[1, 0] = res_Height; range[1, 1] = 0;
                range[2, 0] = 0; range[2, 1] = res_Width;
                range[3, 0] = res_Height; range[3, 1] = res_Width;
                toolStripStatusLabel2.Text = "范围计算完成！";
                progressBar1.Value = 15;

                CoarseCor cor = new CoarseCor();
                double[] MaxMin =cor.get_MaxMinRange(AffineTrans(x_X, range));

                res_Width = Convert.ToInt32(MaxMin[0] - MaxMin[1]) + 1;
                res_Height = Convert.ToInt32(MaxMin[2] - MaxMin[3]) + 1;
                double LeftTop_x = MaxMin[1];
                double LeftTop_y = MaxMin[3];
                Driver dri = Gdal.GetDriverByName("GTiff");
                toolStripStatusLabel2.Text = "创建影像";
                progressBar1.Value = 40;
                Dataset output = dri.Create(saveFileName, res_Width, res_Height, Registration_Image.RasterCount, DataType.GDT_UInt16, null);
                double[] fInTransForm1 = new double[6];
                Standard_Image.GetGeoTransform(fInTransForm1);
                output.SetGeoTransform(fInTransForm1);
                output.SetProjection(Standard_Image.GetProjection());

                for (int i = 1; i < output.RasterCount + 1; i++)
                {
                    double temp_X = LeftTop_x;
                    double temp_Y = LeftTop_y;
                    int[] buffer = new int[res_Width * res_Height];
                    int[] oriBuffer = new int[Registration_Image.RasterXSize * Registration_Image.RasterYSize];
                    Registration_Image.GetRasterBand(i).ReadRaster(0, 0, Registration_Image.RasterXSize, Registration_Image.RasterYSize, oriBuffer, Registration_Image.RasterXSize, Registration_Image.RasterYSize, 0, 0);
                    for (int row = 0; row < res_Height; row++)
                    {
                        for (int col = 0; col < res_Width; col++)
                        {
                            int o_col = Convert.ToInt32(X_x[0, 0] + X_x[1, 0] * temp_X + X_x[2, 0] * temp_Y);
                            int o_row = Convert.ToInt32(X_x[0, 1] + X_x[1, 1] * temp_X + X_x[2, 1] * temp_Y);
                            if ((o_col >=0) && o_col < Registration_Image.RasterXSize && (o_row >= 0) && o_row < Registration_Image.RasterYSize)
                            {
                                buffer[row * res_Width + col] = oriBuffer[o_row * Registration_Image.RasterXSize + o_col];
                            }
                            else
                            {
                                buffer[row * res_Width + col] = 0;
                            }
                            temp_X++;
                        }
                        temp_X = LeftTop_x;
                        temp_Y++;
                    }
                    output.GetRasterBand(i).WriteRaster(0, 0, output.RasterXSize, output.RasterYSize, buffer, output.RasterXSize, output.RasterYSize, 0, 0);
                    progressBar1.Value = 40 + i * 15;
                }
                progressBar1.Value = 100;
                MessageBox.Show("校正成功！", "提示", MessageBoxButtons.OK);
                Image_Reg.bandListIni();
                Image_Reg.ChooseBandList(output, Image_Reg);
                pictureBox2.Image = Image_Reg.GetImage(output, pictureBox2);
                output.Dispose();
            }
            else { MessageBox.Show("请输入存储路径！","提示",MessageBoxButtons.OK); }
        }

        private void 开始编辑ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
          
        }


        private void Image_Registration_Load(object sender, EventArgs e)
        {

        }

        private void 读取文件存储路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = RSImage.ShowSaveFileDialog();
            if (file != null)
            {
                saveFileName = file;
            }
        }
    }
}
