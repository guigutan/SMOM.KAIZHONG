using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.ComboBox;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPTabControlHeader : UserControl
    {
        private int selectedIndex = 0;

        private List<SIE.XPCJ.Common.Controls.XPButton> xpButtons;
        private List<Label> labels;
        public XPTabControlHeader()
        {
            InitializeComponent();
            DoubleBuffered = true;
            xpButtons = new List<SIE.XPCJ.Common.Controls.XPButton>() {
            this.xpButton1,
            this.xpButton2,
            this.xpButton3,
            this.xpButton4,
            this.xpButton5,
            this.xpButton6,
            this.xpButton7,
            this.xpButton8,
            this.xpButton9,
            };
            labels = new List<Label>() {
              this.label1,
              this.label2,
              this.label3,
              this.label4,
              this.label5,
              this.label6,
              this.label7,
              this.label8,
              this.label9,
            };
        }

        private TabControl _ATabControl = null;
        public TabControl ATabControl 
        {
            get { return this._ATabControl; }
            set
            {
                _ATabControl = value;
                value.ItemSize = new Size(100, this.Height - 4);
            }
        }

        public string AButton1Text
        {
            get => this.xpButton1.Text;
            set
            {
                this.xpButton1.Text = value;
                this.xpButton1.Visible = !string.IsNullOrEmpty(value);
            }
        }
        public string AButton2Text
        {
            get => this.xpButton2.Text;
            set
            {
                this.xpButton2.Text = value;
                this.xpButton2.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string AButton3Text
        {
            get => this.xpButton3.Text;
            set
            {
                this.xpButton3.Text = value;
                this.xpButton3.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string AButton4Text
        {
            get => this.xpButton4.Text;
            set
            {
                this.xpButton4.Text = value;
                this.xpButton4.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string AButton5Text
        {
            get => this.xpButton5.Text;
            set
            {
                this.xpButton5.Text = value;
                this.xpButton5.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string AButton6Text
        {
            get => this.xpButton6.Text;
            set
            {
                this.xpButton6.Text = value;
                this.xpButton6.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string AButton7Text
        {
            get => this.xpButton7.Text;
            set
            {
                this.xpButton7.Text = value;
                this.xpButton7.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string AButton8Text
        {
            get => this.xpButton8.Text;
            set
            {
                this.xpButton8.Text = value;
                this.xpButton8.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string AButton9Text
        {
            get => this.xpButton9.Text;
            set
            {
                this.xpButton9.Text = value;
                this.xpButton9.Visible = !string.IsNullOrEmpty(value);
            }
        }


        /// <summary>
        /// 按钮选中切换事件
        /// </summary>
        public event EventHandler ASelectIndexChanged;

        public int ASelectedIndex
        {
            get { return selectedIndex; }
            set 
            {
                if (value < 0)
                    return;
                if (value >= xpButtons.Count)
                    return;

                if (string.IsNullOrEmpty(xpButtons[value].Text))
                    return;

                selectedIndex = value;
                this.Selected(selectedIndex);
            }
        }

        private void xpButton_Click(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                // 在设计时执行的代码
                this.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                // 在运行时执行的代码
                // ...
            }
            this.selectedIndex = Convert.ToInt32((sender as XPButton).Tag);
            this.Selected(selectedIndex);
        }

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="xPButton"></param>
        /// <param name="label"></param>
        private void Selected(int index)
        {
            for (int i = 0; i < xpButtons.Count; i++)
            {
                if (i == index)
                {
                    xpButtons[i].Font = new System.Drawing.Font(this.xpButton1.Font, System.Drawing.FontStyle.Bold);
                    xpButtons[i].ForeColor = Color.FromArgb(173, 5, 61);
                    labels[i].Visible = true;
                }
                else
                {
                    xpButtons[i].Font = new System.Drawing.Font(this.xpButton1.Font, System.Drawing.FontStyle.Regular);
                    xpButtons[i].ForeColor = Color.FromArgb(102, 102, 102);
                    labels[i].Visible = false;
                }
            }

            if(this.ATabControl != null && this.ATabControl.TabPages.Count > index)
                this.ATabControl.SelectedIndex = index;

            ASelectIndexChanged?.Invoke(this, null);
        }

        private void XPTabControlHeader_Load(object sender, EventArgs e)
        {
            if (!DesignMode && this.ATabControl != null)
                this.ATabControl.SendToBack();
        }
    }

}
