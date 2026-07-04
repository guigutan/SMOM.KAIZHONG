
namespace SIE.XPCJ.Common.Controls
{
    partial class XPSwitch
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
            this.btnLeft = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnRight = new SIE.XPCJ.Common.Controls.XPButton();
            this.btnMiddle = new SIE.XPCJ.Common.Controls.XPButton();
            this.SuspendLayout();
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(35)))), ((int)(((byte)(54)))));
            this.btnLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLeft.ForeColor = System.Drawing.Color.White;
            this.btnLeft.IsPrivilegeAllow = true;
            this.btnLeft.Location = new System.Drawing.Point(0, 0);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.PrivilegeName = null;
            this.btnLeft.Size = new System.Drawing.Size(100, 62);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.Text = "左";
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnRight.IsPrivilegeAllow = true;
            this.btnRight.Location = new System.Drawing.Point(202, 0);
            this.btnRight.Name = "btnRight";
            this.btnRight.PrivilegeName = null;
            this.btnRight.Size = new System.Drawing.Size(100, 62);
            this.btnRight.TabIndex = 0;
            this.btnRight.Text = "右";
            this.btnRight.UseVisualStyleBackColor = false;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnMiddle
            // 
            this.btnMiddle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMiddle.FlatAppearance.BorderSize = 0;
            this.btnMiddle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMiddle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMiddle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.btnMiddle.IsPrivilegeAllow = true;
            this.btnMiddle.Location = new System.Drawing.Point(100, 0);
            this.btnMiddle.Name = "btnMiddle";
            this.btnMiddle.PrivilegeName = null;
            this.btnMiddle.Size = new System.Drawing.Size(102, 62);
            this.btnMiddle.TabIndex = 2;
            this.btnMiddle.Text = "中间";
            this.btnMiddle.UseVisualStyleBackColor = false;
            this.btnMiddle.Click += new System.EventHandler(this.btnMiddle_Click);
            // 
            // XPSwitch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnMiddle);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Name = "XPSwitch";
            this.Size = new System.Drawing.Size(302, 62);
            this.SizeChanged += new System.EventHandler(this.XPSwitch_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        public SIE.XPCJ.Common.Controls.XPButton btnRight;
        public SIE.XPCJ.Common.Controls.XPButton btnLeft;
        public XPButton btnMiddle;
    }
}
