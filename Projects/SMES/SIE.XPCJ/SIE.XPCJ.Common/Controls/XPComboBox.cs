using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public class XPComboBox : ComboBox
    {
        public XPComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 24;
            DoubleBuffered = true;
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (e.Index >= 0)
            {
                Brush backgroundBrush = Brushes.White;
                Brush foregroundBrush = Brushes.Black;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    backgroundBrush = SystemBrushes.Highlight;
                    foregroundBrush = SystemBrushes.HighlightText;
                }
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                string itemText = GetItemText(Items[e.Index]);
                e.Graphics.DrawString(itemText, e.Font, foregroundBrush, e.Bounds.X + 2, e.Bounds.Y + 2);
            }
        }

    }
}
