using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPScanBarcode : UserControl
    {
        private Font tipsDefautFont;

        private Font tipsChangeFont;


        /// <summary>
        /// 提示信息超出控件后字体缩放字体大小
        /// </summary>
        [Category("Design"), Description("提示信息超出控件后字体缩放字体的大小,默认18")]
        public int TipsOverFontSize
        {
            get;
            set;
        } = 18;


        #region 左边Switch属性
        public int ALeftSwitchWidth
        {
            get { return this.xpSwitchLeft.Width; }
            set { this.xpSwitchLeft.Width = value; }
        }
        public bool ALeftSwitchVisible
        {
            get { return this.xpSwitchLeft.Visible; }
            set { this.xpSwitchLeft.Visible = value; }
        }
        public string ALeftSwitchLeftText
        {
            get { return this.xpSwitchLeft.ALeftText; }
            set { this.xpSwitchLeft.ALeftText = value; }
        }

        public string ALeftSwitchMiddleText
        {
            get { return this.xpSwitchLeft.AMiddleText; }
            set { this.xpSwitchLeft.AMiddleText = value; }
        }

        public string ALeftSwitchRightText
        {
            get { return this.xpSwitchLeft.ARightText; }
            set { this.xpSwitchLeft.ARightText = value; }
        }
        public bool ALeftSwitchChecked
        {
            get { return this.xpSwitchLeft.AChecked; }
            set { this.xpSwitchLeft.AChecked = value; }
        }
        public int ALeftSwitchCheckedIndex
        {
            get { return this.xpSwitchLeft.ACheckedIndex; }
            set { this.xpSwitchLeft.ACheckedIndex = value; }
        }
        public event EventHandler ALeftSwitchCheckedChanged
        {
            add
            {
                this.xpSwitchLeft.ACheckedChanged += value;
            }
            remove { this.xpSwitchLeft.ACheckedChanged -= value; }
        }
        #endregion

        #region 右边Switch属性
        public int ARightSwitchWidth
        {
            get { return this.xpSwitchRight.Width; }
            set { this.xpSwitchRight.Width = value; }
        }
        public bool ARightSwitchVisible
        {
            get { return this.xpSwitchRight.Visible; }
            set { this.xpSwitchRight.Visible = value; }
        }
        public string ARightSwitchLeftText
        {
            get { return this.xpSwitchRight.ALeftText; }
            set { this.xpSwitchRight.ALeftText = value; }
        }
        public string ARightSwitchMiddleText
        {
            get { return this.xpSwitchRight.AMiddleText; }
            set { this.xpSwitchRight.AMiddleText = value; }
        }
        public string ARightSwitchRightText
        {
            get { return this.xpSwitchRight.ARightText; }
            set { this.xpSwitchRight.ARightText = value; }
        }
        public bool ARightSwitchChecked
        {
            get { return this.xpSwitchRight.AChecked; }
            set { this.xpSwitchRight.AChecked = value; }
        }
        public int ARightSwitchCheckedIndex
        {
            get { return this.xpSwitchRight.ACheckedIndex; }
            set { this.xpSwitchRight.ACheckedIndex = value; }
        }
        public event EventHandler ARightSwitchCheckedChanged
        {
            add { this.xpSwitchRight.ACheckedChanged += value; }
            remove { this.xpSwitchRight.ACheckedChanged -= value; }
        }
        #endregion

        #region Tips
        public string ATips
        {
            get { return this.lbTips.Text; }
            set
            {
                this.lbTips.Text = value;
                this.lbTips.Font = IsTipsOverControl(value) ? tipsChangeFont : tipsDefautFont;
            }
        }

        public Color ATipsColor
        {
            get { return this.lbTips.ForeColor; }
            set { this.lbTips.ForeColor = value; }
        }

        public Font ATipsFont
        {
            get { return this.lbTips.Font; }
            set { this.lbTips.Font = value; }
        }
        #endregion


        /// <summary>
        /// 条码(已经移除开始和结尾的换行符)
        /// </summary>
        public string ABarcode
        {
            get { return this.watermarkTextBox1.Text.TrimStart('\r').TrimStart('\n').TrimEnd('\n').TrimEnd('\r'); }
            set { this.watermarkTextBox1.Text = value; }
        }

        /// <summary>
        /// 条码变更事件
        /// </summary>
        public event EventHandler ABarcodeChanged;

        private void buttonScan_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Focus();
            ABarcodeChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// 回车事件时触发扫码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watermarkTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ABarcodeChanged?.Invoke(sender, e);
            }
        }

        public XPScanBarcode()
        {
            InitializeComponent();
            DoubleBuffered = true;
            tipsDefautFont = this.lbTips.Font;
            tipsChangeFont = new Font(tipsDefautFont.FontFamily, TipsOverFontSize, tipsDefautFont.Style);
        }

        /// <summary>
        /// 重置扫描窗体
        /// </summary>
        public void ResetBarcode()
        {
            this.watermarkTextBox1.Text = "";
            this.watermarkTextBox1.Focus();
        }

        /// <summary>
        /// 判断提示信息是否超过控件
        /// </summary>
        /// <returns></returns>
        private bool IsTipsOverControl(string text)
        {
            if (!this.IsDisposed)
            {
                var sizeF = MeasureString(text, tipsDefautFont);
                return sizeF.Width > this.lbTips.Width;
            }
            return false;

        }

        /// <summary>
        /// 计算字符串长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        private SizeF MeasureString(string text, Font font)
        {
            using (Graphics graphics = this.CreateGraphics())
            {
                return graphics.MeasureString(text, font);
            }
        }


        /// <summary>
        /// 设置条码并触发条码变更事件
        /// </summary>
        /// <param name="barcode"></param>
        public void SetBarcode(string barcode)
        {
            this.watermarkTextBox1.Text = barcode;
            ABarcodeChanged?.Invoke(null, null);
        }

        private void watermarkTextBox1_Enter(object sender, EventArgs e)
        {
            //this.panelTextBox.BackColor = System.Drawing.Color.LightBlue;
            //this.watermarkTextBox1.BackColor = this.panelTextBox.BackColor;
        }

        private void watermarkTextBox1_Leave(object sender, EventArgs e)
        {
            //this.panelTextBox.BackColor = System.Drawing.Color.White;
            //this.watermarkTextBox1.BackColor = this.panelTextBox.BackColor;
        }

        /// <summary>
        /// 设置提示
        /// </summary>
        /// <param name="tips"></param>
        /// <param name="oprateState">操作成功</param>
        public void ShowTips(string tips, bool isReplaceN = true)
        {
            this.ATips = isReplaceN ? tips.Replace("\r\n", "").Replace("\n", "") : tips;
            this.ATipsColor = Color.FromArgb(0, 203, 106);
        }

        public void ShowError(string tips, bool isReplaceN = true)
        {
            this.ATips = isReplaceN ? tips.Replace("\r\n", "").Replace("\n", "") : tips;
            this.ATipsColor = Color.FromArgb(255, 0, 0);
        }

        public void ShowWaring(string tips, bool isReplaceN = true)
        {
            this.ATips = isReplaceN ? tips.Replace("\r\n", "").Replace("\n", "") : tips;
            this.ATipsColor = Color.FromArgb(255, 0, 0);
        }

        public void FocusTextBox()
        {
            this.watermarkTextBox1.Focus();
        }
    }
}
