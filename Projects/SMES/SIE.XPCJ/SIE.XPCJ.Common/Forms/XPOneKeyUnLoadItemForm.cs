using SIE.XPCJ.Models.WIP.Entity;
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
    public partial class OneKeyUnLoadItemForm : Common.Forms.FormBase
    {
        private List<LoadItemViewModel> UnloadDataSoure = new List<LoadItemViewModel>();

        public List<double> SelectIds = new List<double>();
        public OneKeyUnLoadItemForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="unloadDataSoure"></param>
        public void SetDataSource(List<LoadItemViewModel> unloadDataSoure)
        {
            this.UnloadDataSoure = unloadDataSoure;
            this.dataGridView1.DataSource = this.UnloadDataSoure;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                var loadId = double.Parse(row.Cells[0].Value.ToString());
                SelectIds.Add(loadId);
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridView1.Rows)
            {
                dr.Selected = true;
            }
        }
    }
}
