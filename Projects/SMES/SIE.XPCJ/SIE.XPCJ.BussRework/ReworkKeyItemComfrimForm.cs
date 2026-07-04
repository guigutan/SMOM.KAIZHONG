using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP;
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
    public partial class ReworkKeyItemComfrimForm : XPCJ.Common.Forms.XPFormBaseDialog
    {
        /// <summary>
        /// 是否选择了置换后下料
        /// </summary>
        public bool SelectedBlankingWay { get; set; }

        public ReplaceItemHandleMethod m_ReplaceItemHandleMethod { get; set; }

        /// <summary>
        /// 选中的下料目标线边仓
        /// </summary>
        public XPLinesideWarehouse SelectedWarehouse { get; set; }

        List<XPLinesideWarehouse> listWarehouse;
        public ReworkKeyItemComfrimForm(ReplaceItemHandleMethod replaceItemHandleMethod, List<XPLinesideWarehouse> listWarehouse)
        {
            InitializeComponent();
            this.listWarehouse = listWarehouse;

            this.comboBox1.Items.Add("");
            foreach (XPLinesideWarehouse w in this.listWarehouse)
            {
                this.comboBox1.Items.Add($"{w.Name}-{w.StorageLocationName}");
            }

            this.SelectedBlankingWay = replaceItemHandleMethod != ReplaceItemHandleMethod.Scrap;
            this.xpSwitch1.ACheckedIndex = ((int)replaceItemHandleMethod) / 10 - 1;
            this.xpSwitch1.AChecked = this.SelectedBlankingWay;
            this.panelWarehouse.Visible = this.xpSwitch1.AChecked;
        }

        private void xpDialogTitle1_AOkClick(object sender, EventArgs e)
        {
            this.SelectedWarehouse = null;
            this.m_ReplaceItemHandleMethod = (ReplaceItemHandleMethod)((this.xpSwitch1.ACheckedIndex + 1) * 10);
            this.SelectedBlankingWay = m_ReplaceItemHandleMethod != ReplaceItemHandleMethod.Scrap;
            if (this.SelectedBlankingWay)
            {
                if (this.comboBox1.SelectedIndex <= 0)
                {
                    MessageBox.Show("请选择线边仓".L10N());
                    return;
                }
                this.SelectedWarehouse = this.listWarehouse[this.comboBox1.SelectedIndex -1];
            }
            this.DialogResult = DialogResult.OK;
        }

        private void xpSwitch1_ACheckedChanged(object sender, EventArgs e)
        {
            this.panelWarehouse.Visible = this.xpSwitch1.ACheckedIndex!=0;
        }
    }
}
