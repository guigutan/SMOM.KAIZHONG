using SIE.Domain;
using SIE.MES.TaskManagement.Reports;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录审核查询视图
    /// </summary>
    public class ReportRecordExamineCriteriaViewConfig : WebViewConfig<ReportRecordExamineCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.Wo).HasLabel("关联工单").Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.Resource).Show().UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(WipResource.CodeProperty.Name);
                    p.SearchFieldList.Add(WipResource.NameProperty.Name);
                });
                //View.Property(p => p.WorkShop).UseDataSource((source, pagingInfo, keyword) =>
                //{
                //    var workshop = RT.Service.Resolve<EnterpriseController>().GetWorkShops(pagingInfo, keyword, null);
                //    if (workshop == null || workshop.Count <= 0)
                //    {
                //        return new EntityList<Enterprise>();
                //    }
                //    workshop.ForEach(p => p.TreePId = null);
                //    return workshop;
                //}).Show();
                View.Property(p => p.WorkShopCode).Show();
                View.Property(p => p.InspectionStatus).Show();
                View.Property(p => p.InspectionResult).Show();
                View.Property(p => p.ExamineState).Show();
                View.Property(p => p.Process).Show();
                View.Property(p => p.WipBatchNos).Show();
                View.Property(p => p.Licha).Show();
                View.Property(p => p.SourceType).Show();
                View.Property(p => p.ReportTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
            }
        }
    }
}
