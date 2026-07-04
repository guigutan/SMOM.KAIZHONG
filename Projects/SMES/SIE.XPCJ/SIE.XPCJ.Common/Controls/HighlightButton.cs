using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Common.Controls
{
    public class HighlightButton : XPButton
    {

        public bool IsHighlight = false;
        public double DataId { get; set; }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            SetHighLight();
        }

        public void SetHighLight()
        {

            // 设置按钮边框颜色
            this.FlatAppearance.BorderSize = 1;
            this.FlatAppearance.BorderColor = !IsHighlight ? Color.FromArgb(39, 131, 254) : Color.White;
            this.ForeColor = !IsHighlight ? Color.FromArgb(39, 131, 254) : Color.FromArgb(51, 51, 51);
            IsHighlight = !IsHighlight;
        }
    }
}
