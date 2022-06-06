namespace Test
{
    partial class Histogram_MatchingFrm
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
            this.label1 = new System.Windows.Forms.Label();
            this.oFilecomboBox = new System.Windows.Forms.ComboBox();
            this.mFilecomboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "源影像：";
            // 
            // oFilecomboBox
            // 
            this.oFilecomboBox.FormattingEnabled = true;
            this.oFilecomboBox.Location = new System.Drawing.Point(122, 22);
            this.oFilecomboBox.Name = "oFilecomboBox";
            this.oFilecomboBox.Size = new System.Drawing.Size(215, 26);
            this.oFilecomboBox.TabIndex = 1;
            this.oFilecomboBox.SelectedIndexChanged += new System.EventHandler(this.oFilecomboBox_SelectedIndexChanged);
            // 
            // mFilecomboBox
            // 
            this.mFilecomboBox.FormattingEnabled = true;
            this.mFilecomboBox.Location = new System.Drawing.Point(122, 63);
            this.mFilecomboBox.Name = "mFilecomboBox";
            this.mFilecomboBox.Size = new System.Drawing.Size(215, 26);
            this.mFilecomboBox.TabIndex = 3;
            this.mFilecomboBox.SelectedIndexChanged += new System.EventHandler(this.mFilecomboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "匹配影像：";
            // 
            // ok_Button
            // 
            this.ok_Button.Location = new System.Drawing.Point(48, 446);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(96, 32);
            this.ok_Button.TabIndex = 10;
            this.ok_Button.Text = "开始匹配";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Location = new System.Drawing.Point(226, 446);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(96, 32);
            this.cancel_Button.TabIndex = 11;
            this.cancel_Button.Text = "取消";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(15, 165);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(338, 250);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 13;
            this.label3.Text = "结果预览：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(341, 18);
            this.label4.TabIndex = 31;
            this.label4.Text = "-------------------------------------";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 425);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(341, 18);
            this.label5.TabIndex = 32;
            this.label5.Text = "-------------------------------------";
            // 
            // Histogram_MatchingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 490);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.mFilecomboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.oFilecomboBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Histogram_MatchingFrm";
            this.Text = "Histogram_MatchingFrm";
            this.Load += new System.EventHandler(this.Histogram_MatchingFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox oFilecomboBox;
        private System.Windows.Forms.ComboBox mFilecomboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}