using SIE.XPCJ.Models.WIP;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.Controls
{
    public partial class XPTasksList : BaseUserControl
    {

        /// <summary>
        /// 报工任务数据源
        /// </summary>
        private BindingList<ReportTask> ReportTasksDataSource { get; set; }
        public XPTasksList()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="reportTasksDataSource"></param>
        public void SetData(BindingList<ReportTask> reportTasksDataSource)
        {
            this.panel1.Controls.Clear();
            this.ReportTasksDataSource = reportTasksDataSource;
            if (ReportTasksDataSource != null)
            {
                foreach (var task in ReportTasksDataSource)
                {
                    var ctr = new XPTaskListCard();
                    ctr.RepportTask = task;
                    ctr.Dock = DockStyle.Top;
                    this.panel1.Controls.Add(ctr);
                }

            }

        }
    }
}
