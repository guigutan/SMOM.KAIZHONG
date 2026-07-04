using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Forms
{
    public partial class XPFormSwitchWorkOrder : XPFormBaseDialog
    {

        /// <summary>
        /// 工作单元
        /// </summary>
        public Workcell Workcell { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// 当前选择工单
        /// </summary>
        public WorkOrder CurrentWo { get; set; }
        public XPFormSwitchWorkOrder()
        {
            InitializeComponent();
            this.dataGridView1.DataSourceChanged += DataGridView1_DataSourceChanged;
            this.dataGridView1.AllowUserToResizeColumns = true;
            this.dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        }

        private void DataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(238, 238, 238);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SwitchWorkOrderForm_Load(object sender, EventArgs e)
        {
            if (this.Workcell != null)
            {
                var woList = WipService.GetWorkOrdertInfos(new Models.WIP.WorkOrderQueryInfo()
                {
                    Keyword = "",
                    ResourceId = this.Workcell.ResourceId,
                    StateList = new System.Collections.Generic.List<int>() { 0, 1 }
                });
                this.dataGridView1.DataSource = woList;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.Workcell == null)
            {
                MessageBox.Show("请先选择工作单元".L10N());
                return;
            }

            var woList = WipService.GetWorkOrdertInfos(new Models.WIP.WorkOrderQueryInfo()
            {
                Keyword = this.watermarkTextBox1.Text,
                ResourceId = this.Workcell.ResourceId,
                StateList = new System.Collections.Generic.List<int>() { 0, 1 }
            });
            this.dataGridView1.DataSource = woList;
        }

        /// <summary>
        /// 双击代替确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SetSwitchWorkOrder();
        }

        /// <summary>
        /// 设置切换工单
        /// </summary>
        private void SetSwitchWorkOrder()
        {
            WorkOrder = null;
            if (this.dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择一行工单进行切换".L10N());
                return;
            }
            //获取选中行
            var rowWoId = this.dataGridView1.SelectedRows[0].Cells["Id"].Value;
            if (rowWoId == null)
            {
                MessageBox.Show("请选择一行工单进行切换".L10N());
                return;
            }

            double woId = double.Parse(rowWoId.ToString());
            if (woId <= 0)
            {
                MessageBox.Show("请选择一行工单进行切换".L10N());
                return;
            }
            if (CurrentWo != null && CurrentWo.Id == woId)
            {
                MessageBox.Show("您选择的工单和当前在制工单相同，无需切换".L10N());
                return;
            }
            WorkOrder = WipService.ChangeWipResourceWorkOrder(woId, Workcell);
            if (WorkOrder != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetSwitchWorkOrder();
        }

        private void watermarkTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchText();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void SearchText()
        {
            if (this.Workcell == null)
            {
                MessageBox.Show("请先选择工作单元".L10N());
                return;
            }

            var woList = WipService.GetWorkOrdertInfos(new Models.WIP.WorkOrderQueryInfo()
            {
                Keyword = this.watermarkTextBox1.Text,
                ResourceId = this.Workcell.ResourceId,
                StateList = new System.Collections.Generic.List<int>() { 0, 1 }
            });
            this.dataGridView1.DataSource = woList;
        }
    }
}
