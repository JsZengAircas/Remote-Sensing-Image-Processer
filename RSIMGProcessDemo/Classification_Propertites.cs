using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.K_Means;

namespace Test
{
    public partial class Classification_Propertites : Form
    {
        List<Clusters> currentCluster = new List<Clusters>();
        RSImage classProperty;
        public Classification_Propertites(List<Clusters> cluster,RSImage ima)
        {
            InitializeComponent();
            currentCluster = cluster;
            classProperty = ima;
        }

        private void Classification_Propertites_Load(object sender, EventArgs e)
        {
            if (currentCluster!=null)
            {
                LoadCLusterInfo(currentCluster, comboBox1, comboBox2, richTextBox1);
            }
        }
        public void LoadCLusterInfo(List<Clusters> cluster,ComboBox remove,ComboBox colorBox,RichTextBox Info)
        {
            string Infomation = "类聚基本信息" + System.Environment.NewLine;
            Infomation += "-------------------------" + System.Environment.NewLine;
            for (int i = 0; i < cluster.Count; i++)
            {
                comboBox1.Items.Add("Class_" + (currentCluster[i].No).ToString());
                comboBox2.Items.Add("Class_" + (currentCluster[i].No).ToString());
                Infomation += "-------------------------" + System.Environment.NewLine;
                Infomation +="1.类别："+(cluster[i].No).ToString()+ System.Environment.NewLine;
                Infomation+="2.类聚中心："+System.Environment.NewLine;
                for (int num = 0; num < cluster[i].clusterCenter.Count; num++)
                {
                    if (num == cluster[i].clusterCenter.Count-1)
                    {
                        Infomation += cluster[i].clusterCenter[num].ToString() + ";";
                    }
                    else {
                        Infomation += cluster[i].clusterCenter[num].ToString() + ",";
                    }            
                }
                Infomation +=  System.Environment.NewLine+"3.样本总数："+cluster[i].allSamples.Count.ToString();
                string color = cluster[i].colorType.ToString();
                color = color.Remove(0, 5);
                Infomation += System.Environment.NewLine + "4.表示颜色：" +color;
            }
            richTextBox1.Text = Infomation;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int removeClusterIndex = Convert.ToInt32(comboBox1.Text.Remove(0, comboBox1.Text.Length - 1));
            if (MessageBox.Show("确定要删除该类聚中心吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                currentCluster.Remove(currentCluster[removeClusterIndex-1]);
                classProperty.get_Cluster = currentCluster;
                LoadCLusterInfo(currentCluster, comboBox1, comboBox2, richTextBox1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog chooseColor = new ColorDialog();  
            if (chooseColor.ShowDialog()==DialogResult.OK)
            {
                int changeColorClusterIndex = Convert.ToInt32(comboBox2.Text.Remove(0, comboBox2.Text.Length - 1));
                currentCluster[changeColorClusterIndex-1].colorType = chooseColor.Color;
                button2.BackColor = currentCluster[changeColorClusterIndex-1].colorType;
                LoadCLusterInfo(currentCluster, comboBox1, comboBox2, richTextBox1);
            }
        }

        private void Classification_Propertites_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            int changeColorClusterIndex = Convert.ToInt32(comboBox2.Text.Remove(0, comboBox1.Text.Length - 1));
            button2.BackColor = currentCluster[changeColorClusterIndex].colorType;
        }
    }
}
