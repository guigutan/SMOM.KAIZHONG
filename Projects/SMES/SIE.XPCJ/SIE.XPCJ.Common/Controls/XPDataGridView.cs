using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public class XPDataGridView : DataGridView
    {
        public XPDataGridView()
        {
            DoubleBuffered = true;
            this.AllowUserToResizeRows = false;
            this.AllowUserToAddRows = false;
            this.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.EnableHeadersVisualStyles = false;
            this.GridColor = this.BackColor;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        }
    }
}
