using SIE.CSM.Suppliers;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收查询界面
    /// </summary>
    internal class SparePartAcceptanceCriteriaViewConfig : WebViewConfig<SparePartAcceptanceCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryId).UseFactoryEditor();
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.AcceptanceNo);
            View.Property(p => p.ReceiveNo);
            View.Property(p => p.ReceiveType);            
            View.Property(p => p.SparePartCode);
            View.Property(p => p.SparePartName);
            View.Property(p => p.ControlMethod);
            View.Property(p => p.PurchaseOrderNo);
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            });
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
