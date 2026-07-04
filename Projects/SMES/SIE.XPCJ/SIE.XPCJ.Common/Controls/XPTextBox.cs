using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPTextBox : UserControl
    {
        private int PreFontHeight = 0;
        public XPTextBox()
        {
            InitializeComponent();
            DoubleBuffered = true;
            PreFontHeight = this.textBox1.Height;
        }

        public  string AText
        {
            get => this.textBox1.Text;
            set => this.textBox1.Text = value;
        }

        private void XPTextBox_Resize(object sender, EventArgs e)
        {
            this.textBox1.Location = new Point(4, (this.Height - this.textBox1.Height) / 2);
        }

        public override Font Font
        {
            get => this.textBox1.Font;
            set
            {
                this.textBox1.Font = value;
                int y = (PreFontHeight - this.textBox1.Height) / 2;
                PreFontHeight = this.textBox1.Height;
                this.textBox1.Location = new Point(4, this.textBox1.Location.Y + y);
            }
        }

        public override Color BackColor { 
            get => base.BackColor; 
            set
            {
                base.BackColor = value;
                this.textBox1.BackColor = value;
            }
        }

        public override Color ForeColor 
        { 
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                this.textBox1.ForeColor = value;
            }
        }

        public bool ReadOnly
        {
            get { return this.textBox1.ReadOnly; }
            set 
            {
                this.textBox1.ReadOnly = value;
            }
        }
    }
}
