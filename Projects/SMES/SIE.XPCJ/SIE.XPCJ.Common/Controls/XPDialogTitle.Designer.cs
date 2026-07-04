
namespace SIE.XPCJ.Common.Controls
{
    partial class XPDialogTitle
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
            this.buttonOK = new SIE.XPCJ.Common.Controls.XPButton();
            this.buttonCancel = new SIE.XPCJ.Common.Controls.XPButton();
            this.labelTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(41)))), ((int)(((byte)(60)))));
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("宋体", 12F);
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.IsPrivilegeAllow = true;
            this.buttonOK.Location = new System.Drawing.Point(451, 0);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.PrivilegeName = null;
            this.buttonOK.Size = new System.Drawing.Size(91, 57);
            this.buttonOK.TabIndex = 23;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("宋体", 12F);
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.IsPrivilegeAllow = true;
            this.buttonCancel.Location = new System.Drawing.Point(542, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.PrivilegeName = null;
            this.buttonCancel.Size = new System.Drawing.Size(91, 57);
            this.buttonCancel.TabIndex = 22;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.LightGray;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.labelTitle.Size = new System.Drawing.Size(633, 57);
            this.labelTitle.TabIndex = 21;
            this.labelTitle.Text = "输入数量";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // XPDialogTitle
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelTitle);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "XPDialogTitle";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.Size = new System.Drawing.Size(633, 58);
            this.ResumeLayout(false);

        }

        #endregion

        private XPButton buttonOK;
        private XPButton buttonCancel;
        private System.Windows.Forms.Label labelTitle;
    }
}
