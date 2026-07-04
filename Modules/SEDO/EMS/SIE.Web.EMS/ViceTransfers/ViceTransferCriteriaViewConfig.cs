using SIE.EMS.ViceTransfers;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.ViceTransfers
{
    /// <summary>
    /// 
    /// </summary>
    public class ViceTransferCriteriaViewConfig : WebViewConfig<ViceTransferCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.TransferNo).Show();
            View.Property(p => p.QureyFactoryId).UseFactoryEditor().Show() ;
            View.Property(p => p.ViceAssetObject).Show();
            View.Property(p => p.OriginWareHouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.TargetWareHouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.TransferStatus).Show();
            View.Property(p => p.ApprovalStatus).Show();
            View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
