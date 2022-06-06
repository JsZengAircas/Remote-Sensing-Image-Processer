namespace Test
{
    partial class HistogramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.BandLable = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MeanLabel = new System.Windows.Forms.Label();
            this.MaxLabel = new System.Windows.Forms.Label();
            this.MinLabel = new System.Windows.Forms.Label();
            this.PixLabel = new System.Windows.Forms.Label();
            this.StdDevLabel = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(177, 56);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(201, 26);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // BandLable
            // 
            this.BandLable.AutoSize = true;
            this.BandLable.Location = new System.Drawing.Point(79, 59);
            this.BandLable.Name = "BandLable";
            this.BandLable.Size = new System.Drawing.Size(98, 18);
            this.BandLable.TabIndex = 9;
            this.BandLable.Text = "波    段：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 10;
            this.label1.Text = "文件名称：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(178, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 28);
            this.textBox1.TabIndex = 11;
            // 
            // MeanLabel
            // 
            this.MeanLabel.AutoSize = true;
            this.MeanLabel.Location = new System.Drawing.Point(280, 435);
            this.MeanLabel.Name = "MeanLabel";
            this.MeanLabel.Size = new System.Drawing.Size(71, 18);
            this.MeanLabel.TabIndex = 0;
            this.MeanLabel.Text = "平均值:";
            // 
            // MaxLabel
            // 
            this.MaxLabel.AutoSize = true;
            this.MaxLabel.Location = new System.Drawing.Point(33, 435);
            this.MaxLabel.Name = "MaxLabel";
            this.MaxLabel.Size = new System.Drawing.Size(71, 18);
            this.MaxLabel.TabIndex = 4;
            this.MaxLabel.Text = "最大值:";
            // 
            // MinLabel
            // 
            this.MinLabel.AutoSize = true;
            this.MinLabel.Location = new System.Drawing.Point(33, 474);
            this.MinLabel.Name = "MinLabel";
            this.MinLabel.Size = new System.Drawing.Size(71, 18);
            this.MinLabel.TabIndex = 5;
            this.MinLabel.Text = "最小值:";
            // 
            // PixLabel
            // 
            this.PixLabel.AutoSize = true;
            this.PixLabel.Location = new System.Drawing.Point(280, 474);
            this.PixLabel.Name = "PixLabel";
            this.PixLabel.Size = new System.Drawing.Size(71, 18);
            this.PixLabel.TabIndex = 6;
            this.PixLabel.Text = "像素数:";
            // 
            // StdDevLabel
            // 
            this.StdDevLabel.AutoSize = true;
            this.StdDevLabel.Location = new System.Drawing.Point(33, 508);
            this.StdDevLabel.Name = "StdDevLabel";
            this.StdDevLabel.Size = new System.Drawing.Size(71, 18);
            this.StdDevLabel.TabIndex = 12;
            this.StdDevLabel.Text = "标准差:";
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(5, 145);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(443, 238);
            this.chart1.TabIndex = 13;
            this.chart1.Text = "chart1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 397);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(440, 18);
            this.label2.TabIndex = 14;
            this.label2.Text = "---波段信息-------------------------------------";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(449, 18);
            this.label3.TabIndex = 15;
            this.label3.Text = "---影像直方图------------------------------------";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 542);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(440, 18);
            this.label4.TabIndex = 16;
            this.label4.Text = "------------------------------------------------";
            // 
            // HistogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(453, 569);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.StdDevLabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BandLable);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.PixLabel);
            this.Controls.Add(this.MinLabel);
            this.Controls.Add(this.MaxLabel);
            this.Controls.Add(this.MeanLabel);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistogramForm";
            this.Text = "Histogram";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HistogramForm_FormClosed);
            this.Load += new System.EventHandler(this.HistogramForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label BandLable;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label MeanLabel;
        private System.Windows.Forms.Label MaxLabel;
        private System.Windows.Forms.Label MinLabel;
        private System.Windows.Forms.Label PixLabel;
        private System.Windows.Forms.Label StdDevLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}