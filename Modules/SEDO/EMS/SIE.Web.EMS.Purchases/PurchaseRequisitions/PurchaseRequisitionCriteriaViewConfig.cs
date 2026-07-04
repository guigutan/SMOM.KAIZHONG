using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Web.Resources;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.EMS.Projects;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请查询实体界面
    /// </summary>
    internal class PurchaseRequisitionCriteriaViewConfig : WebViewConfig<PurchaseRequisitionCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.No);
            View.Property(p => p.PurchaseType);
            View.Property(p => p.PurchaseObjectType).UsePurchaseObjectEditor();
            View.Property(p => p.ProjectId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProjectController>().GetAuditedProjects(pagingInfo, keyword);
            }).HasLabel("项目编码");
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
