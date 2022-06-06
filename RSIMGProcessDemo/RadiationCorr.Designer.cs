namespace Test
{
    partial class RadiationCorr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadiationCorr));
            this.label1 = new System.Windows.Forms.Label();
            this.FilecomboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gainRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gainComboBox = new System.Windows.Forms.ComboBox();
            this.gainTextBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.offsetComboBox = new System.Windows.Forms.ComboBox();
            this.offsetRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.saveFileTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.btSaveFile = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择文件：";
            // 
            // FilecomboBox
            // 
            this.FilecomboBox.FormattingEnabled = true;
            this.FilecomboBox.Location = new System.Drawing.Point(12, 30);
            this.FilecomboBox.Name = "FilecomboBox";
            this.FilecomboBox.Size = new System.Drawing.Size(398, 26);
            this.FilecomboBox.TabIndex = 1;
            this.FilecomboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "设置参数（Gain）：";
            // 
            // gainRichTextBox
            // 
            this.gainRichTextBox.Enabled = false;
            this.gainRichTextBox.Location = new System.Drawing.Point(12, 102);
            this.gainRichTextBox.Name = "gainRichTextBox";
            this.gainRichTextBox.Size = new System.Drawing.Size(395, 76);
            this.gainRichTextBox.TabIndex = 3;
            this.gainRichTextBox.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(341, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "选择波段更改：         |回车完成修改|";
            // 
            // gainComboBox
            // 
            this.gainComboBox.FormattingEnabled = true;
            this.gainComboBox.Location = new System.Drawing.Point(29, 224);
            this.gainComboBox.Name = "gainComboBox";
            this.gainComboBox.Size = new System.Drawing.Size(125, 26);
            this.gainComboBox.TabIndex = 5;
            // 
            // gainTextBox1
            // 
            this.gainTextBox1.Location = new System.Drawing.Point(230, 224);
            this.gainTextBox1.Name = "gainTextBox1";
            this.gainTextBox1.Size = new System.Drawing.Size(116, 28);
            this.gainTextBox1.TabIndex = 6;
            this.gainTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gainTextBox1_KeyUp);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(230, 424);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(116, 28);
            this.textBox2.TabIndex = 11;
            this.textBox2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyUp);
            // 
            // offsetComboBox
            // 
            this.offsetComboBox.FormattingEnabled = true;
            this.offsetComboBox.Location = new System.Drawing.Point(29, 424);
            this.offsetComboBox.Name = "offsetComboBox";
            this.offsetComboBox.Size = new System.Drawing.Size(125, 26);
            this.offsetComboBox.TabIndex = 10;
            // 
            // offsetRichTextBox
            // 
            this.offsetRichTextBox.Enabled = false;
            this.offsetRichTextBox.Location = new System.Drawing.Point(12, 302);
            this.offsetRichTextBox.Name = "offsetRichTextBox";
            this.offsetRichTextBox.Size = new System.Drawing.Size(395, 76);
            this.offsetRichTextBox.TabIndex = 8;
            this.offsetRichTextBox.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "修改参数(Offset):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 477);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 18);
            this.label6.TabIndex = 12;
            this.label6.Text = "输入存储路径:";
            // 
            // saveFileTextBox
            // 
            this.saveFileTextBox.Location = new System.Drawing.Point(12, 508);
            this.saveFileTextBox.Name = "saveFileTextBox";
            this.saveFileTextBox.Size = new System.Drawing.Size(364, 28);
            this.saveFileTextBox.TabIndex = 13;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(67, 564);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(115, 30);
            this.okButton.TabIndex = 16;
            this.okButton.Text = "确认";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(241, 564);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(115, 30);
            this.cancelButton.TabIndex = 17;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // btSaveFile
            // 
            this.btSaveFile.ForeColor = System.Drawing.SystemColors.Control;
            this.btSaveFile.Image = ((System.Drawing.Image)(resources.GetObject("btSaveFile.Image")));
            this.btSaveFile.Location = new System.Drawing.Point(383, 502);
            this.btSaveFile.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveFile.Name = "btSaveFile";
            this.btSaveFile.Size = new System.Drawing.Size(38, 36);
            this.btSaveFile.TabIndex = 20;
            this.btSaveFile.UseVisualStyleBackColor = true;
            this.btSaveFile.Click += new System.EventHandler(this.btSaveFile_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 390);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(341, 18);
            this.label8.TabIndex = 22;
            this.label8.Text = "选择波段更改：         |回车完成修改|";
            // 
            // RadiationCorr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 610);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btSaveFile);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.saveFileTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.offsetComboBox);
            this.Controls.Add(this.offsetRichTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gainTextBox1);
            this.Controls.Add(this.gainComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gainRichTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FilecomboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RadiationCorr";
            this.Text = "RadiationCorr";
            this.Load += new System.EventHandler(this.RadiationCorr_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox FilecomboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox gainRichTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox gainComboBox;
        private System.Windows.Forms.TextBox gainTextBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox offsetComboBox;
        private System.Windows.Forms.RichTextBox offsetRichTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox saveFileTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button btSaveFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label8;
    }
}