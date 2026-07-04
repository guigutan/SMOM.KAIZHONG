using System;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormNumberInput : XPFormBaseDialog
    {
        public decimal QtyResult { get; set; }

        private bool disablePoint = false;
        public XPFormNumberInput(decimal qty)
        {
            InitializeComponent();
            QtyResult = qty;
            this.watermarkTextBox1.Text = QtyResult.ToString();
            SetCursor();
        }

        public static DialogResult ShowInput(string title, bool isHidePoint, decimal qty, out decimal newQty)
        {
            newQty = -1;
            XPFormNumberInput comQtyInputForm = new XPFormNumberInput(qty);
            if (isHidePoint)
                comQtyInputForm.HiddenPoint();
            comQtyInputForm.SetTitle(title);
            var reslut = comQtyInputForm.ShowDialog();
            if (reslut == DialogResult.OK)
            {
                newQty = comQtyInputForm.QtyResult;
            }

            return reslut;
        }

        public static DialogResult ShowInput(decimal qty, out decimal newQty)
        {
            newQty = -1;
            XPFormNumberInput comQtyInputForm = new XPFormNumberInput(qty);
            var reslut = comQtyInputForm.ShowDialog();
            if (reslut == DialogResult.OK)
            {
                newQty = comQtyInputForm.QtyResult;
            }

            return reslut;
        }

        /// <summary>
        /// 隐藏小数点
        /// </summary>
        public void HiddenPoint()
        {
            xpButton11.Visible = false;
            xpButton13.Visible = false;
            disablePoint = true;//禁用输入小数点
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="strTitle"></param>
        public void SetTitle(string strTitle = "输入数量")
        {
            this.label1.Text = strTitle;
        }

        private void xpButton13_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += ".";
            SetCursor();
        }

        private void xpButton14_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text = "";
            SetCursor();
        }

        private void xpButton1_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "1";
            SetCursor();
        }

        private void xpButton2_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "2";
            SetCursor();
        }

        private void xpButton3_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "3";
            SetCursor();
        }

        private void xpButton4_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "4";
            SetCursor();
        }

        private void xpButton6_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "5";
            SetCursor();
        }

        private void xpButton7_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "6";
            SetCursor();
        }

        private void xpButton9_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "7";
            SetCursor();
        }

        private void xpButton8_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "8";
            SetCursor();
        }

        private void xpButton5_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "9";
            SetCursor();
        }

        private void xpButton12_Click(object sender, EventArgs e)
        {
            this.watermarkTextBox1.Text += "0";
            SetCursor();

        }

        /// <summary>
        /// 设置光标
        /// </summary>
        private void SetCursor()
        {
            if (this.watermarkTextBox1.Text.Length > 0)
            {
                this.watermarkTextBox1.Focus();
                this.watermarkTextBox1.SelectionStart = this.watermarkTextBox1.Text.Length <= 0 ? 0 : this.watermarkTextBox1.Text.Length;
            }
        }

        private void xpButton10_Click(object sender, EventArgs e)
        {
            if (this.watermarkTextBox1.Text.Length > 0)
            {
                this.watermarkTextBox1.Text = this.watermarkTextBox1.Text.Remove(this.watermarkTextBox1.Text.Length - 1, 1);
                this.watermarkTextBox1.Focus();
                this.watermarkTextBox1.SelectionStart = this.watermarkTextBox1.Text.Length <= 0 ? 0 : this.watermarkTextBox1.Text.Length;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Comfrim();
        }

        /// <summary>
        /// 确认
        /// </summary>
        private void Comfrim()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            QtyResult = this.watermarkTextBox1.Text.Length > 0 ? decimal.Parse(this.watermarkTextBox1.Text) : 0;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许输入数字、Backspace、Delete和箭头键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46)
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)13)//回车
            {
                //Comfrim();
                e.Handled = true;
            }

            // 允许小数点
            if (e.KeyChar == '.')
            {
                if (disablePoint)//禁用小数点
                {
                    e.Handled = true;
                    return;
                }
                // 确保textbox中没有小数点
                if (watermarkTextBox1.Text.Contains(".") || watermarkTextBox1.Text.Length == 0)
                {
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// 增加该事件是由于避免触发扫描框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void watermarkTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//回车
            {
                Comfrim();
                e.Handled = true;
            }
        }
    }
}
