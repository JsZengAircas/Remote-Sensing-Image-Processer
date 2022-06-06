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

namespace Test.Supervised_Classification
{
    public partial class Set_ROI : DockContent
    {
        public Set_ROI()
        {
            InitializeComponent();
        }
        private string name;
        private Color color;
        bool edit = false;
        public string get_Name { get { return name; } set {; } }
        public Color get_Color { get { return color; } set {; } }
        public bool editStart { set {edit =value; } get { return edit; } }
        public SupervisedClassFrm SucFrm { set; get; }
        int currentRow=-1;
        bool delete;
        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog choose = new ColorDialog();
            choose.ShowDialog();
            if (choose.Color != null)
            {
                color = choose.Color;
                textBox2.Text=Convert.ToString(color.R);
                textBox3.Text = Convert.ToString(color.G);
                textBox4.Text = Convert.ToString(color.B);
                button1.BackColor = color;
                button1.ForeColor = color;
            }
        }
        private void Set_ROI_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (color != null)
            {
                name = textBox1.Text;
                edit = true;
            }
            else { MessageBox.Show("请输入类别颜色", "提示", MessageBoxButtons.OK); }
        }
        public void updataROIData(List<ROI> roi)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < roi.Count; i++)
            {
                dataGridView1.Rows.Add(roi[i].No,roi[i].Num,roi[i].rectangleBit[0].X, roi[i].rectangleBit[0].Y, roi[i].rectangleBit[1].X, roi[i].rectangleBit[1].Y);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            edit = false;
            if (textBox1.Text != null)
            {
                name = textBox1.Text;
                SucFrm.ClassifyData();
                // this.Close();
                textBox1.Text = null;
                dataGridView1.Rows.Clear();
                textBox2.Text = null;
                textBox3.Text = null;
                textBox4.Text = null;
            }
            else { MessageBox.Show("请输入类别名称", "提示", MessageBoxButtons.OK); }           
        }

        private void 删除样区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentRow != -1 && delete)
            {
                SucFrm.reDrawRegion(currentRow);
               
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currentRow = e.RowIndex;
                delete = true;
            }
        }
    }
}
