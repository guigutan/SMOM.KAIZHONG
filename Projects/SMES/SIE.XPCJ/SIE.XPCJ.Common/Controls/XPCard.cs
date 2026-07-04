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
    public partial class XPCard : BaseUserControl
    {
        public XPCard()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public object Data { get; set; }

        public virtual void BindData(object obj)
        {
            this.Data = obj;
        }
    }
}
