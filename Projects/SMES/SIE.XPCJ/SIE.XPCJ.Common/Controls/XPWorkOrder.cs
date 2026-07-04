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
    public partial class XPWorkOrder : UserControl
    {
        public BorderStyle ATextBoxBorderStyle
        {
            get => this.textBox1.BorderStyle;
            set
            {
                this.textBox1.BorderStyle = value;
                this.textBox2.BorderStyle = value;
                this.textBox3.BorderStyle = value;
                this.textBox4.BorderStyle = value;
            }
        }

        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                this.textBox1.BackColor = value;
                this.textBox2.BackColor = value;
                this.textBox3.BackColor = value;
                this.textBox4.BackColor = value;
            }
        }

        private int _AColumn = 3;
        public int AColumn
        {
            get => _AColumn;
            set
            {
                _AColumn = value;
                this.textBox3.Visible = value > 1;
                this.label3.Visible = this.textBox1.Visible;
                this.textBox4.Visible = value > 2;
                this.label4.Visible = this.textBox4.Visible;
            }
        }

        public string ALabel1Text
        {
            get => this.label1.Text;
            set => this.label1.Text = value;
        }

        public string ATextBox1Text
        {
            get => this.textBox1.AText;
            set => this.textBox1.AText = value;
        }

        public Font ATextBox1Font
        {
            get => this.textBox1.Font;
            set => this.textBox1.Font = value;
        }

        public string ALabel2Text
        {
            get => this.label2.Text;
            set => this.label2.Text = value;
        }

        public string ATextBox2Text
        {
            get => this.textBox2.AText;
            set => this.textBox2.AText = value;
        }

        public Font ATextBox2Font
        {
            get => this.textBox2.Font;
            set => this.textBox2.Font = value;
        }

        public string ALabel3Text
        {
            get => this.label3.Text;
            set => this.label3.Text = value;
        }

        public string ATextBox3Text
        {
            get => this.textBox3.AText;
            set => this.textBox3.AText = value;
        }

        public Font ATextBox3Font
        {
            get => this.textBox3.Font;
            set => this.textBox3.Font = value;
        }

        public string ALabel4Text
        {
            get => this.label4.Text;
            set => this.label4.Text = value;
        }

        public string ATextBox4Text
        {
            get => this.textBox4.AText;
            set => this.textBox4.AText = value;
        }

        public Font ATextBox4Font
        {
            get => this.textBox4.Font;
            set => this.textBox4.Font = value;
        }

        public XPWorkOrder()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// 下拉显示完整信息
        /// </summary>
        public event EventHandler AMoreInfoClick;


        public bool AExistMoreInfo
        {
            get 
            { 
                return this.buttonMoreOrderInfo.Visible;
            }
            set 
            { 
                this.buttonMoreOrderInfo.Visible = value;
                row1.ColumnStyles[3].Width = value ? 50 : 0;
            }
        }

        private void buttonMoreOrderInfo_Click(object sender, EventArgs e)
        {
            AMoreInfoClick?.Invoke(sender, e);
        }
    }
}
