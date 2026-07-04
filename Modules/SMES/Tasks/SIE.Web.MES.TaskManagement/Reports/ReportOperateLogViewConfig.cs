using SIE.MES.TaskManagement.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportOperateLogViewConfig : WebViewConfig<ReportOperateLog>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.StartTime).UseListSetting(p => p.HelpInfo = "任务单开工，或在执行过程恢复时写入").Readonly().Show();
            View.Property(p => p.StartOpter).Readonly().Show();
            View.Property(p => p.EndTime).UseListSetting(p => p.HelpInfo = "任务单在执行过程暂停时写入").Readonly().Show();
            View.Property(p => p.EndOpter).Readonly().Show();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

        }
    }
}
