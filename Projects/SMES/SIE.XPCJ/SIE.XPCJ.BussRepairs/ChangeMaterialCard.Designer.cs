
namespace SIE.XPCJ.BussRepairs
{
    partial class ChangeMaterialCard
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.xpTextBoxItemQty = new SIE.XPCJ.Common.Controls.XPTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.xpTextBoxCanQty = new SIE.XPCJ.Common.Controls.XPTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.xpButtonEditQty = new SIE.XPCJ.Common.Controls.XPButton();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(646, 60);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.xpButtonEditQty, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(646, 60);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.xpTextBoxItemQty);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 33);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(287, 24);
            this.panel3.TabIndex = 2;
            // 
            // xpTextBoxItemQty
            // 
            this.xpTextBoxItemQty.AText = "1";
            this.xpTextBoxItemQty.BackColor = System.Drawing.Color.White;
            this.xpTextBoxItemQty.Dock = System.Windows.Forms.DockStyle.Right;
            this.xpTextBoxItemQty.Location = new System.Drawing.Point(163, 0);
            this.xpTextBoxItemQty.Name = "xpTextBoxItemQty";
            this.xpTextBoxItemQty.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.xpTextBoxItemQty.ReadOnly = true;
            this.xpTextBoxItemQty.Size = new System.Drawing.Size(124, 24);
            this.xpTextBoxItemQty.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.label6.Location = new System.Drawing.Point(5, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "换料数量:";
            // 
            // panel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(552, 24);
            this.panel2.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.BackColor = System.Drawing.Color.White;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold);
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.checkBox1.Location = new System.Drawing.Point(0, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.checkBox1.Size = new System.Drawing.Size(598, 28);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "MU20411110000001";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.xpTextBoxCanQty);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(296, 33);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(287, 24);
            this.panel4.TabIndex = 1;
            // 
            // xpTextBoxCanQty
            // 
            this.xpTextBoxCanQty.AText = "1";
            this.xpTextBoxCanQty.BackColor = System.Drawing.Color.White;
            this.xpTextBoxCanQty.Dock = System.Windows.Forms.DockStyle.Right;
            this.xpTextBoxCanQty.Location = new System.Drawing.Point(163, 0);
            this.xpTextBoxCanQty.Name = "xpTextBoxCanQty";
            this.xpTextBoxCanQty.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.xpTextBoxCanQty.ReadOnly = true;
            this.xpTextBoxCanQty.Size = new System.Drawing.Size(124, 24);
            this.xpTextBoxCanQty.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "可用数量:";
            // 
            // xpButtonEditQty
            // 
            this.xpButtonEditQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(113)))), ((int)(((byte)(119)))));
            this.xpButtonEditQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpButtonEditQty.FlatAppearance.BorderSize = 0;
            this.xpButtonEditQty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButtonEditQty.ForeColor = System.Drawing.Color.White;
            this.xpButtonEditQty.ForeColorDisable = System.Drawing.Color.Silver;
            this.xpButtonEditQty.ForeColorEnable = System.Drawing.Color.White;
            this.xpButtonEditQty.IsPrivilegeAllow = true;
            this.xpButtonEditQty.Location = new System.Drawing.Point(589, 3);
            this.xpButtonEditQty.Name = "xpButtonEditQty";
            this.xpButtonEditQty.PrivilegeName = null;
            this.tableLayoutPanel1.SetRowSpan(this.xpButtonEditQty, 2);
            this.xpButtonEditQty.Size = new System.Drawing.Size(54, 54);
            this.xpButtonEditQty.TabIndex = 3;
            this.xpButtonEditQty.Text = "修改";
            this.xpButtonEditQty.UseVisualStyleBackColor = false;
            this.xpButtonEditQty.Click += new System.EventHandler(this.xpButtonEditQty_Click);
            // 
            // ChangeMaterialCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.panel1);
            this.Name = "ChangeMaterialCard";
            this.Size = new System.Drawing.Size(646, 60);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private Common.Controls.XPTextBox xpTextBoxItemQty;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private Common.Controls.XPTextBox xpTextBoxCanQty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox1;
        private Common.Controls.XPButton xpButtonEditQty;
    }
}
