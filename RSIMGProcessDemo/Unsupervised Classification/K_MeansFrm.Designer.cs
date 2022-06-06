namespace Test
{
    partial class K_MeansFrm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(K_MeansFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fileComboBox = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.bandComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.classNumBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.显示影像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更改类别属性ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btSaveFile = new System.Windows.Forms.Button();
            this.saveBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Iteration_NumBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rangeBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.fileComboBox);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.bandComboBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.classNumBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(618, 799);
            this.panel1.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(566, 18);
            this.label12.TabIndex = 58;
            this.label12.Text = "———————————————————————————————";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(105, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 18);
            this.label1.TabIndex = 34;
            this.label1.Text = "选择待处理文件：";
            // 
            // fileComboBox
            // 
            this.fileComboBox.FormattingEnabled = true;
            this.fileComboBox.Location = new System.Drawing.Point(273, 32);
            this.fileComboBox.Name = "fileComboBox";
            this.fileComboBox.Size = new System.Drawing.Size(203, 26);
            this.fileComboBox.TabIndex = 35;
            this.fileComboBox.SelectedIndexChanged += new System.EventHandler(this.fileComboBox_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.SystemColors.Control;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(483, 28);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(29, 32);
            this.button3.TabIndex = 54;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // bandComboBox
            // 
            this.bandComboBox.FormattingEnabled = true;
            this.bandComboBox.Location = new System.Drawing.Point(273, 72);
            this.bandComboBox.Name = "bandComboBox";
            this.bandComboBox.Size = new System.Drawing.Size(203, 26);
            this.bandComboBox.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 18);
            this.label2.TabIndex = 36;
            this.label2.Text = "选择待处理波段：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(105, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 18);
            this.label3.TabIndex = 38;
            this.label3.Text = "输入类别数目：";
            // 
            // classNumBox
            // 
            this.classNumBox.Location = new System.Drawing.Point(273, 109);
            this.classNumBox.Name = "classNumBox";
            this.classNumBox.Size = new System.Drawing.Size(203, 28);
            this.classNumBox.TabIndex = 39;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 772);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(566, 18);
            this.label11.TabIndex = 57;
            this.label11.Text = "———————————————————————————————";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 281);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(566, 18);
            this.label10.TabIndex = 55;
            this.label10.Text = "———————————————————————————————";
            // 
            // 显示影像ToolStripMenuItem
            // 
            this.显示影像ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("显示影像ToolStripMenuItem.Image")));
            this.显示影像ToolStripMenuItem.Name = "显示影像ToolStripMenuItem";
            this.显示影像ToolStripMenuItem.Size = new System.Drawing.Size(196, 30);
            this.显示影像ToolStripMenuItem.Text = "显示影像";
            this.显示影像ToolStripMenuItem.Click += new System.EventHandler(this.显示影像ToolStripMenuItem_Click);
            // 
            // 更改类别属性ToolStripMenuItem
            // 
            this.更改类别属性ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("更改类别属性ToolStripMenuItem.Image")));
            this.更改类别属性ToolStripMenuItem.Name = "更改类别属性ToolStripMenuItem";
            this.更改类别属性ToolStripMenuItem.Size = new System.Drawing.Size(196, 30);
            this.更改类别属性ToolStripMenuItem.Text = "更改类别属性";
            this.更改类别属性ToolStripMenuItem.Click += new System.EventHandler(this.更改类别属性ToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.更改类别属性ToolStripMenuItem,
            this.显示影像ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 64);
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(149, 326);
            this.treeView1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.treeView1);
            this.panel2.Location = new System.Drawing.Point(429, 327);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(149, 326);
            this.panel2.TabIndex = 53;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(387, 745);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 18);
            this.label8.TabIndex = 52;
            this.label8.Text = "进度：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(455, 746);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(131, 17);
            this.progressBar1.TabIndex = 51;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(403, 677);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 35);
            this.button2.TabIndex = 50;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(108, 677);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 35);
            this.button1.TabIndex = 49;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 656);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(566, 18);
            this.label9.TabIndex = 56;
            this.label9.Text = "———————————————————————————————";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 306);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 48;
            this.label7.Text = "--预览--";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 327);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(411, 326);
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            // 
            // btSaveFile
            // 
            this.btSaveFile.ForeColor = System.Drawing.SystemColors.Control;
            this.btSaveFile.Image = ((System.Drawing.Image)(resources.GetObject("btSaveFile.Image")));
            this.btSaveFile.Location = new System.Drawing.Point(483, 245);
            this.btSaveFile.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveFile.Name = "btSaveFile";
            this.btSaveFile.Size = new System.Drawing.Size(29, 32);
            this.btSaveFile.TabIndex = 46;
            this.btSaveFile.UseVisualStyleBackColor = true;
            this.btSaveFile.Click += new System.EventHandler(this.btSaveFile_Click);
            // 
            // saveBox
            // 
            this.saveBox.Location = new System.Drawing.Point(281, 249);
            this.saveBox.Name = "saveBox";
            this.saveBox.Size = new System.Drawing.Size(195, 28);
            this.saveBox.TabIndex = 45;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(105, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 18);
            this.label6.TabIndex = 44;
            this.label6.Text = "获取存储文件路径：";
            // 
            // Iteration_NumBox
            // 
            this.Iteration_NumBox.Location = new System.Drawing.Point(281, 198);
            this.Iteration_NumBox.Name = "Iteration_NumBox";
            this.Iteration_NumBox.Size = new System.Drawing.Size(195, 28);
            this.Iteration_NumBox.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(105, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 18);
            this.label5.TabIndex = 42;
            this.label5.Text = "输入最大迭代次数：";
            // 
            // rangeBox
            // 
            this.rangeBox.Location = new System.Drawing.Point(328, 155);
            this.rangeBox.Name = "rangeBox";
            this.rangeBox.Size = new System.Drawing.Size(148, 28);
            this.rangeBox.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(105, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(233, 18);
            this.label4.TabIndex = 40;
            this.label4.Text = "输入类聚中心变化阈值(%)：";
            // 
            // K_MeansFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(618, 799);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btSaveFile);
            this.Controls.Add(this.saveBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Iteration_NumBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rangeBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(620, 831);
            this.Name = "K_MeansFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "K-Means分类";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Classification_FormClosed);
            this.Load += new System.EventHandler(this.Classification_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem 显示影像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更改类别属性ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btSaveFile;
        private System.Windows.Forms.TextBox saveBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Iteration_NumBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox rangeBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox classNumBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox bandComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox fileComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
    }
}