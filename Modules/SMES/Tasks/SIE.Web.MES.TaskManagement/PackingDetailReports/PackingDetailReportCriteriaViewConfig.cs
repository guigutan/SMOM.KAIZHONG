using SIE.MES.TaskManagement.PackingDetailReports;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.PackingDetailReports
{
    /// <summary>
    /// 包装QC确认明细报表 视图配置
    /// </summary>
    public class PackingDetailReportCriteriaViewConfig : WebViewConfig<PackingDetailReportCriteria>
    {
        /// <summary>
        /// 包装QC确认明细报表 视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).Show();
                View.Property(p => p.Confirm).Show();
                View.Property(p => p.PackIdent).Show();
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ProductLabel).Show();
                View.Property(p => p.BatchLabel).Show();
                View.Property(p => p.ReportsType).Show();
                View.Property(p => p.ResourceId).Show().UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(WipResource.CodeProperty.Name);
                    p.SearchFieldList.Add(WipResource.NameProperty.Name);
                });               
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
            }
        }

    }
}
