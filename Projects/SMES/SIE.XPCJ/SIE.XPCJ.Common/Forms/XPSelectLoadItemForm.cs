using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPSelectLoadItemForm : SIE.XPCJ.Common.Forms.XPFormBaseDialog
    {
        public XPSelectLoadItemForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 上料条码信息
        /// </summary>
        public List<LoadItemBarcodeInfo> loadItemBarcodeInfos { get; set; }

        public LoadItemBarcodeInfo CurrentLoadItemBarcodeInfo { get; set; }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selectedItemId = (double)this.dataGridView1.SelectedRows[0].Cells[0].Value;
            var index = loadItemBarcodeInfos.FindIndex(m => m.Id == selectedItemId);
            if (index >= 0)
            {
                CurrentLoadItemBarcodeInfo = loadItemBarcodeInfos[index];
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SelectLoadItemForm_Load(object sender, EventArgs e)
        {
            if (loadItemBarcodeInfos != null)
            {
                this.dataGridView1.DataSource = loadItemBarcodeInfos;
            }

        }
    }
}
