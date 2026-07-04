using SIE.Core.WorkOrders;
using SIE.MES.TaskManagement.SchedulingInfReports;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIE.Web.MES.TaskManagement.SchedulingInfReports
{
    /// <summary>
    /// 排程状态查询表-Web查询视图
    /// </summary>
    public class SchedulingInfReportCriteriaViewConfig: WebViewConfig<SIE.MES.TaskManagement.SchedulingInfReports.SchedulingInfReportCriteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {            
            using (View.OrderProperties())
            {
                View.Property(p => p.IsImport).Show();
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ProcessCode).Show();
                View.Property(p => p.ProcessName).Show();
                View.Property(p => p.Mrp).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.UpdateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
                View.Property(p => p.ImportTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
                View.Property(p => p.IsSchedulingInfReturn).Show();
                //View.Property(p => p.IsCheck).Show();取消
                View.Property(p => p.IsGenerateTask).Show(); 
            }
        }
    }
}
