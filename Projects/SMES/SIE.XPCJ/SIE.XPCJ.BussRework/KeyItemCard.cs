using SIE.XPCJ.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRework
{
    public partial class KeyItemCard : XPCJ.Common.Controls.XPCard
    {
        public KeyItemCard()
        {
            InitializeComponent();
        }

        public override void BindData(object obj)
        {
            base.BindData(obj);
            KeyItemDataObj itm = obj as KeyItemDataObj;
            if (itm == null)
            {
                this.panelItem1.Visible = false;
                this.panelItem2.Visible = false;
                this.panelItem3.Visible = false;
                return;
            }

            if (itm.Item1 != null)
            {
                this.checkBox1.Visible = itm.IsShowCheckBox;
                this.checkBox1.Checked = itm.Item1.IsUnbound;
                this.checkBox1.Text = itm.Item1.ItemCode; // itm.Item1.Barcode;
                this.labelType1.Text = itm.Item1.SourceType.ToLabel();
                this.labelCode1.Text = itm.Item1.SourceCode;
                this.labelCount1.Text = itm.Item1.Qty.ToString("0.00");
            }
            else
            {
                this.panelItem1.Visible = false;
            }

            if (itm.Item2 != null)
            {
                this.checkBox2.Visible = itm.IsShowCheckBox;
                this.checkBox2.Checked = itm.Item2.IsUnbound;
                this.checkBox2.Text = itm.Item2.ItemCode; // itm.Item2.Barcode;
                this.labelType2.Text = itm.Item2.SourceType.ToLabel();
                this.labelCode2.Text = itm.Item2.SourceCode;
                this.labelCount2.Text = itm.Item2.Qty.ToString("0.00");
            }
            else
            {
                this.panelItem2.Visible = false;
            }

            if (itm.Item3 != null)
            {
                this.checkBox3.Visible = itm.IsShowCheckBox;
                this.checkBox3.Checked = itm.Item3.IsUnbound;
                this.checkBox3.Text = itm.Item3.ItemCode; // itm.Item3.Barcode;
                this.labelType3.Text = itm.Item3.SourceType.ToLabel();
                this.labelCode3.Text = itm.Item3.SourceCode;
                this.labelCount3.Text = itm.Item3.Qty.ToString("0.00");
            }
            else
            {
                this.panelItem3.Visible = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Data == null)
                return;

            KeyItemDataObj itm = this.Data as KeyItemDataObj;
            if(itm.Item1 != null)
                itm.Item1.IsUnbound = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Data == null)
                return;

            KeyItemDataObj itm = this.Data as KeyItemDataObj;
            if (itm.Item2 != null)
                itm.Item2.IsUnbound = this.checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Data == null)
                return;

            KeyItemDataObj itm = this.Data as KeyItemDataObj;
            if (itm.Item3 != null)
                itm.Item3.IsUnbound = this.checkBox3.Checked;
        }
    }
}
