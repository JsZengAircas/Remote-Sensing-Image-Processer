using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Registration_statistics : Form
    {
        public Registration_statistics(List<dataList> IniList,Image_Registration IR)
        {
            InitializeComponent();
            Data = IniList;
            tempIR = IR;
        }
        public List<dataList> Data = new List<dataList>();
        int currentRow=-1;
        bool delete;
        Image_Registration tempIR;
        private void Registration_statistics_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                dataGridView1.Rows.Add(Data[i].No, Data[i].Base_X, Data[i].Base_Y, Data[i].Warp_X, Data[i].Warp_Y, Data[i].Predict_X, Data[i].Predict_Y);
            }
        }
        public void displayData(List<dataList> data)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                dataGridView1.Rows.Add(data[i].No, data[i].Base_X, data[i].Base_Y, data[i].Warp_X, data[i].Warp_Y, data[i].Predict_X, data[i].Predict_Y);
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

        private void 删除数据列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentRow != -1&&delete)
            {
                tempIR.changeDataset(currentRow);
            }
        }
    }
}
