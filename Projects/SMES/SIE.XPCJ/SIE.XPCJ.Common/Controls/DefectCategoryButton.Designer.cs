
namespace SIE.XPCJ.Common.Controls
{
    partial class DefectCategoryButton
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
            this.xpButton1 = new SIE.XPCJ.Common.Controls.XPButton();
            this.SuspendLayout();
            // 
            // xpButton1
            // 
            this.xpButton1.AutoEllipsis = true;
            this.xpButton1.BackColor = System.Drawing.Color.White;
            this.xpButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpButton1.FlatAppearance.BorderSize = 0;
            this.xpButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.xpButton1.Location = new System.Drawing.Point(0, 0);
            this.xpButton1.Name = "xpButton1";
            this.xpButton1.Size = new System.Drawing.Size(87, 30);
            this.xpButton1.TabIndex = 0;
            this.xpButton1.Text = "缺陷啊缺陷啊";
            this.xpButton1.UseVisualStyleBackColor = false;
            this.xpButton1.Click += new System.EventHandler(this.DefectCategoryButton_Click);
            // 
            // DefectCategoryButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xpButton1);
            this.Name = "DefectCategoryButton";
            this.Size = new System.Drawing.Size(87, 30);
            this.Click += new System.EventHandler(this.DefectCategoryButton_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private SIE.XPCJ.Common.Controls.XPButton xpButton1;
    }
}
