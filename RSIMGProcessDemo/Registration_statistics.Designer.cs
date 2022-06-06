namespace Test
{
    partial class Registration_statistics
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Col_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除数据列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_No,
            this.BX,
            this.BY,
            this.WX,
            this.WY,
            this.PX,
            this.PY});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.Size = new System.Drawing.Size(740, 267);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            // 
            // Col_No
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Col_No.DefaultCellStyle = dataGridViewCellStyle8;
            this.Col_No.HeaderText = "No";
            this.Col_No.Name = "Col_No";
            this.Col_No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BX
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.BX.DefaultCellStyle = dataGridViewCellStyle9;
            this.BX.HeaderText = "Base X";
            this.BX.Name = "BX";
            this.BX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BY
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.BY.DefaultCellStyle = dataGridViewCellStyle10;
            this.BY.HeaderText = "Base Y";
            this.BY.Name = "BY";
            this.BY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // WX
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.WX.DefaultCellStyle = dataGridViewCellStyle11;
            this.WX.HeaderText = "Warp X";
            this.WX.Name = "WX";
            this.WX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // WY
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.WY.DefaultCellStyle = dataGridViewCellStyle12;
            this.WY.HeaderText = "Warp Y";
            this.WY.Name = "WY";
            this.WY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PX
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PX.DefaultCellStyle = dataGridViewCellStyle13;
            this.PX.HeaderText = "Predict X";
            this.PX.Name = "PX";
            this.PX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PY
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PY.DefaultCellStyle = dataGridViewCellStyle14;
            this.PY.HeaderText = "Predict Y";
            this.PY.Name = "PY";
            this.PY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除数据列ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 65);
            // 
            // 删除数据列ToolStripMenuItem
            // 
            this.删除数据列ToolStripMenuItem.Name = "删除数据列ToolStripMenuItem";
            this.删除数据列ToolStripMenuItem.Size = new System.Drawing.Size(198, 28);
            this.删除数据列ToolStripMenuItem.Text = "删除数据列";
            this.删除数据列ToolStripMenuItem.Click += new System.EventHandler(this.删除数据列ToolStripMenuItem_Click);
            // 
            // Registration_statistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 267);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Registration_statistics";
            this.Text = "Registration_statistics";
            this.Load += new System.EventHandler(this.Registration_statistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn BX;
        private System.Windows.Forms.DataGridViewTextBoxColumn BY;
        private System.Windows.Forms.DataGridViewTextBoxColumn WX;
        private System.Windows.Forms.DataGridViewTextBoxColumn WY;
        private System.Windows.Forms.DataGridViewTextBoxColumn PX;
        private System.Windows.Forms.DataGridViewTextBoxColumn PY;
        private System.Windows.Forms.ToolStripMenuItem 删除数据列ToolStripMenuItem;
    }
}