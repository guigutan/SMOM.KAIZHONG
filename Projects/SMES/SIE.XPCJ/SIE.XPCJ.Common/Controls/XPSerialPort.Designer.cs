
namespace SIE.XPCJ.Common.Controls
{
    partial class XPSerialPort
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.xpButton1 = new SIE.XPCJ.Common.Controls.XPButton();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(248, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // xpButton1
            // 
            this.xpButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(35)))), ((int)(((byte)(54)))));
            this.xpButton1.FlatAppearance.BorderSize = 0;
            this.xpButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton1.ForeColor = System.Drawing.Color.White;
            this.xpButton1.IsPrivilegeAllow = true;
            this.xpButton1.Location = new System.Drawing.Point(491, 1);
            this.xpButton1.Name = "xpButton1";
            this.xpButton1.PrivilegeName = null;
            this.xpButton1.Size = new System.Drawing.Size(75, 23);
            this.xpButton1.TabIndex = 2;
            this.xpButton1.Text = "删除";
            this.xpButton1.UseVisualStyleBackColor = false;
            this.xpButton1.Click += new System.EventHandler(this.xpButton1_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200",
            "128000",
            "256000"});
            this.comboBox2.Location = new System.Drawing.Point(254, 3);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(231, 20);
            this.comboBox2.TabIndex = 3;
            // 
            // SerialPortCtr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.xpButton1);
            this.Controls.Add(this.comboBox1);
            this.Name = "SerialPortCtr";
            this.Size = new System.Drawing.Size(600, 28);
            this.Load += new System.EventHandler(this.SerialPortCtr_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private SIE.XPCJ.Common.Controls.XPButton xpButton1;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}
