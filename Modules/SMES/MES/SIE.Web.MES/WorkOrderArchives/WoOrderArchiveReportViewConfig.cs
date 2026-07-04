using SIE.MES.WorkOrderArchives;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案报工记录视图配置
    /// </summary>
    public class WoOrderArchiveReportViewConfig : WebViewConfig<WoOrderArchiveReportViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.TaskNo).ShowInList(width: 150);
                View.Property(p => p.TaskState).ShowInList(width: 150);
                View.Property(p => p.DispatchQty).ShowInList(width: 150);
                View.Property(p => p.ReportQty)
                    .UseDisplayEditor(p => p.ColumnXType = "ReportDispatchTaskDisplay").HasLabel("任务进度").ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.Charger).ShowInList(width: 150);
                //View.Property(p => p.TaskTime).ShowInList(width: 150);
                View.Property(p => p.RecordReportQty).ShowInList(width: 150);
                View.Property(p => p.RecordOkQty).ShowInList(width: 150);
                View.Property(p => p.RecordNgQty).ShowInList(width: 150);
                View.Property(p => p.Hour).ShowInList(width: 150);
                View.Property(p => p.Station).ShowInList(width: 150);
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.ReportTime).ShowInList(width: 150);
                View.Property(p => p.Defects).ShowInList(width: 150);
                View.Property(p => p.SpecificationCode).ShowInList(width: 150);
                View.Property(p => p.SpecificationName).ShowInList(width: 150);
                View.Property(p => p.IsVirtualPart).ShowInList(width: 150);
                View.Property(p => p.VirtualPartCode).ShowInList(width: 150);
                View.Property(p => p.VirtualPartName).ShowInList(width: 150);
                View.Property(p => p.ReportMode).ShowInList(width: 150);
            }
        }
    }
}
