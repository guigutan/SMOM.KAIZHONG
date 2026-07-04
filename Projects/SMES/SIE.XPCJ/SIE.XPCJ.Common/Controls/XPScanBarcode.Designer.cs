
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    partial class XPScanBarcode
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
            this.panelTextBox = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lbTips = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPadding = new System.Windows.Forms.Panel();
            this.watermarkTextBox1 = new SIE.XPCJ.Common.Controls.XPWatermarkTextBox();
            this.xpSwitchRight = new SIE.XPCJ.Common.Controls.XPSwitch();
            this.xpSwitchLeft = new SIE.XPCJ.Common.Controls.XPSwitch();
            this.panelTextBox.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTextBox
            // 
            this.panelTextBox.Controls.Add(this.watermarkTextBox1);
            this.panelTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTextBox.Location = new System.Drawing.Point(162, 0);
            this.panelTextBox.Name = "panelTextBox";
            this.panelTextBox.Size = new System.Drawing.Size(422, 50);
            this.panelTextBox.TabIndex = 21;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.panelTextBox);
            this.panelTop.Controls.Add(this.xpSwitchRight);
            this.panelTop.Controls.Add(this.xpSwitchLeft);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(774, 50);
            this.panelTop.TabIndex = 22;
            // 
            // lbTips
            // 
            this.lbTips.BackColor = System.Drawing.Color.LightGray;
            this.lbTips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTips.Font = new System.Drawing.Font("宋体", 25F);
            this.lbTips.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(203)))), ((int)(((byte)(106)))));
            this.lbTips.Location = new System.Drawing.Point(4, 54);
            this.lbTips.Name = "lbTips";
            this.lbTips.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lbTips.Size = new System.Drawing.Size(774, 60);
            this.lbTips.TabIndex = 23;
            this.lbTips.Text = "请扫描条码";
            this.lbTips.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(4, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 4);
            this.panel1.TabIndex = 24;
            // 
            // panelPadding
            // 
            this.panelPadding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPadding.Location = new System.Drawing.Point(778, 54);
            this.panelPadding.Name = "panelPadding";
            this.panelPadding.Size = new System.Drawing.Size(4, 64);
            this.panelPadding.TabIndex = 25;
            // 
            // watermarkTextBox1
            // 
            this.watermarkTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.watermarkTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.watermarkTextBox1.Font = new System.Drawing.Font("宋体", 25F);
            this.watermarkTextBox1.Location = new System.Drawing.Point(10, 8);
            this.watermarkTextBox1.Name = "watermarkTextBox1";
            this.watermarkTextBox1.Size = new System.Drawing.Size(441, 39);
            this.watermarkTextBox1.TabIndex = 17;
            this.watermarkTextBox1.WaterText = "请扫描条码";
            this.watermarkTextBox1.WordWrap = false;
            this.watermarkTextBox1.Enter += new System.EventHandler(this.watermarkTextBox1_Enter);
            this.watermarkTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.watermarkTextBox1_KeyUp);
            this.watermarkTextBox1.Leave += new System.EventHandler(this.watermarkTextBox1_Leave);
            // 
            // xpSwitchRight
            // 
            this.xpSwitchRight.AChecked = false;
            this.xpSwitchRight.ACheckedIndex = 0;
            this.xpSwitchRight.ALeftText = "合格";
            this.xpSwitchRight.AMiddleText = "中间";
            this.xpSwitchRight.ARightText = "不合格";
            this.xpSwitchRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.xpSwitchRight.Location = new System.Drawing.Point(584, 0);
            this.xpSwitchRight.Name = "xpSwitchRight";
            this.xpSwitchRight.Size = new System.Drawing.Size(190, 50);
            this.xpSwitchRight.TabIndex = 19;
            // 
            // xpSwitchLeft
            // 
            this.xpSwitchLeft.AChecked = false;
            this.xpSwitchLeft.ACheckedIndex = 0;
            this.xpSwitchLeft.ALeftText = "上料";
            this.xpSwitchLeft.AMiddleText = "中间";
            this.xpSwitchLeft.ARightText = "采集";
            this.xpSwitchLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.xpSwitchLeft.Location = new System.Drawing.Point(0, 0);
            this.xpSwitchLeft.Name = "xpSwitchLeft";
            this.xpSwitchLeft.Size = new System.Drawing.Size(162, 50);
            this.xpSwitchLeft.TabIndex = 20;
            this.xpSwitchLeft.Visible = false;
            // 
            // XPScanBarcode
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.panelPadding);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbTips);
            this.Controls.Add(this.panelTop);
            this.Name = "XPScanBarcode";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(782, 118);
            this.panelTextBox.ResumeLayout(false);
            this.panelTextBox.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelTextBox;
        private SIE.XPCJ.Common.Controls.XPWatermarkTextBox watermarkTextBox1;
        private Panel panelTop;
        private XPSwitch xpSwitchRight;
        private XPSwitch xpSwitchLeft;
        private Label lbTips;
        private Panel panel1;
        private Panel panelPadding;
    }
}
