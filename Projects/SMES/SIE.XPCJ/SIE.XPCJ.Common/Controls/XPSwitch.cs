using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPSwitch : UserControl
    {
        public string ALeftText
        {
            get { return this.btnLeft.Text; }
            set { this.btnLeft.Text = value; }
        }

        public string AMiddleText
        {
            get { return this.btnMiddle.Text; }
            set 
            {
                this.btnMiddle.Text = value;
                if (string.IsNullOrEmpty(value))
                {
                    this.btnLeft.Width = this.Width / 2;
                    this.btnRight.Width = this.Width - this.btnLeft.Width;
                }
            }
        }

        public string ARightText
        {
            get { return this.btnRight.Text; }
            set { this.btnRight.Text = value; }
        }

        /// <summary>
        /// 设置按钮样式，右边按钮选中为选中状态
        /// </summary>
        /// <param name="isChecked"></param>
        private void SetCheckedStytle(int checkedIndex)
        {
            if (checkedIndex == 2)
            {
                this.btnRight.BackColor = Color.FromArgb(169, 35, 54);
                this.btnRight.ForeColor = Color.White;
                //this.btnRight.FlatStyle = FlatStyle.Flat;

                this.btnMiddle.BackColor = Color.FromArgb(242, 242, 242);
                this.btnMiddle.ForeColor = Color.FromArgb(153, 153, 153);
                //this.btnLeft.FlatStyle = FlatStyle.Flat;

                this.btnLeft.BackColor = Color.FromArgb(242, 242, 242);
                this.btnLeft.ForeColor = Color.FromArgb(153, 153, 153);
                //this.btnLeft.FlatStyle = FlatStyle.Standard;
            }
            else if (checkedIndex == 0)
            {
                this.btnLeft.BackColor = Color.FromArgb(169, 35, 54);
                this.btnLeft.ForeColor = Color.White;
                //this.btnLeft.FlatStyle = FlatStyle.Flat;

                this.btnMiddle.BackColor = Color.FromArgb(242, 242, 242);
                this.btnMiddle.ForeColor = Color.FromArgb(153, 153, 153);
                //this.btnLeft.FlatStyle = FlatStyle.Flat;

                this.btnRight.BackColor = Color.FromArgb(242, 242, 242);
                this.btnRight.ForeColor = Color.FromArgb(153, 153, 153);
                //this.btnRight.FlatStyle = FlatStyle.Standard;
            }
            else if (checkedIndex == 1)
            {
                this.btnLeft.BackColor = Color.FromArgb(242, 242, 242);
                this.btnLeft.ForeColor = Color.FromArgb(153, 153, 153);
                //this.btnLeft.FlatStyle = FlatStyle.Flat;

                this.btnMiddle.BackColor = Color.FromArgb(169, 35, 54);
                this.btnMiddle.ForeColor = Color.White;
                //this.btnLeft.FlatStyle = FlatStyle.Flat;

                this.btnRight.BackColor = Color.FromArgb(242, 242, 242);
                this.btnRight.ForeColor = Color.FromArgb(153, 153, 153);
                //this.btnRight.FlatStyle = FlatStyle.Standard;
            }
        }

        private int _ACheckedIndex = 0;
        public int ACheckedIndex
        {
            get { return _ACheckedIndex; }
            set 
            {
                if (_ACheckedIndex == value)
                    return;

                _AChecked = value == 2;
                _ACheckedIndex = value;
                SetCheckedStytle(value);
            }
        }


        private bool _AChecked = false;
        /// <summary>
        /// 只有2个按钮的时候, 右边按钮选中为选中状态
        /// </summary>
        public bool AChecked
        {
            get
            {
                return _AChecked;
            }
            set 
            {
                if (_AChecked == value)
                    return;

                _AChecked = value;
                _ACheckedIndex = value ? 2 : 0;
                SetCheckedStytle(_ACheckedIndex);
            }
        }


        public XPSwitch()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            bool isChanged = _ACheckedIndex != 0;
            if (isChanged)
            {
                ACheckedIndex = 0;
                ACheckedChanged?.Invoke(this, e);
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            bool isChanged = _ACheckedIndex != 2;
            if (isChanged)
            {
                ACheckedIndex = 2;
                ACheckedChanged?.Invoke(this, e);
            }
        }

        private void btnMiddle_Click(object sender, EventArgs e)
        {
            bool isChanged = _ACheckedIndex != 1;
            if (isChanged)
            {
                ACheckedIndex = 1;
                ACheckedChanged?.Invoke(this, e);
            }
        }


        /// <summary>
        /// 按钮选中切换事件
        /// </summary>
        public event EventHandler ACheckedChanged;

        private void XPSwitch_SizeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.btnMiddle.Text))
            {
                this.btnLeft.Width = this.Width / 2;
                this.btnRight.Width = this.Width - this.btnLeft.Width;
            }
            else
            {
                this.btnLeft.Width = this.Width / 3;
                this.btnRight.Width = this.btnLeft.Width;
            }
        }
    }
}
