using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Models.WIP.Entity;
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
    public partial class UnloadItemsCtr : BaseUserControl
    {
        public UnloadItem unloadItem { get; set; }
        public UnloadItemsCtr()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        private void UnloadItemsCtr_Load(object sender, EventArgs e)
        {
            if (unloadItem != null)
            {
                this.label2.Text = unloadItem.SourceCode;
                this.label13.Text = unloadItem.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                this.label4.Text = unloadItem.ItemCode;
                this.label5.Text = unloadItem.ItemName;
                this.label7.Text =unloadItem.IsNg ?  "不良下料".L10N():"正常下料".L10N();
                this.label7.ForeColor = unloadItem.IsNg ? Color.FromArgb(209, 66, 87) : Color.FromArgb(0, 204, 153);


                this.label9.Text = unloadItem.SourceType.ToLabel();
                this.label11.Text = unloadItem.Qty.ToString();
            }
        }
    }
}
