using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public class XPListBoxCtr : ListBox
    {
        public XPListBoxCtr()
        {
            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.ItemHeight = 40; // 设置每个项的高度
            this.MeasureItem += new MeasureItemEventHandler(listBox1_MeasureItem);
            this.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
            DoubleBuffered = true;
        }
        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 40; // 确保项的高度与设置的一致
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (this.Items.Count <=0)
            {
                return;
            }


            using (Brush brush = new SolidBrush(Color.FromArgb(238, 238, 238)))
            {
                using (Brush selectBrush = new SolidBrush(Color.FromArgb(176, 41, 60)))
                {
                    // 设置选中项的背景色
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        e.Graphics.FillRectangle(selectBrush, e.Bounds); // 选中项的背景色
                        e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, Brushes.White, new Point(e.Bounds.X, e.Bounds.Y + 10)); // 设置文字的颜色
                    }
                    else
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds); // 其他项的背景色
                        e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, Brushes.Black, new Point(e.Bounds.X, e.Bounds.Y + 10));// 设置文字的颜色
                    }
                }
            }
            e.Graphics.DrawLine(Pens.White, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            // 如果需要，还可以自定义其他样式，例如边框等
        }
    }
}
