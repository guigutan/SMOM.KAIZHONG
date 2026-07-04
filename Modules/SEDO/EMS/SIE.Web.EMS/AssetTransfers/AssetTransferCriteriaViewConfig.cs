using SIE.EMS.AssetTransfers;
using SIE.Resources.Employees;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.EMS.AssetTransfers
{

    /// <summary>
    /// 资产调拨查询视图
    /// </summary>
    public class AssetTransferCriteriaViewConfig : WebViewConfig<AssetTransferCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.TransferNo).Show();
            View.Property(p => p.QureyFactoryId).UseFactoryEditor();
            View.Property(p => p.TransferType).Show();
            View.Property(p => p.ManageDeptId).UseUserBussinessDepartmentEditor();
            View.Property(p => p.TargetManageDeptId).UseUserBussinessDepartmentEditor();
            View.Property(p => p.ApprovalStatus).Show();
            View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
