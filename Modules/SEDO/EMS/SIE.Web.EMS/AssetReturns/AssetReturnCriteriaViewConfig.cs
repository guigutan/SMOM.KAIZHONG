using SIE.EMS.AssetReturns;
using SIE.Warehouses;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetReturns
{
    /// <summary>
    /// 资产归还查询实体视图配置
    /// </summary>
    internal class AssetReturnCriteriaViewConfig : WebViewConfig<AssetReturnCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ReturnNo).Show();
            View.Property(p => p.QureyFactoryId).UseFactoryEditor().Show();
            View.Property(p => p.RequisitionNo).Show();
            View.Property(p => p.AssetObject).Show();
            View.Property(p => p.ApplyDepartmentId).UseUserBussinessDepartmentEditor().Show();
            View.Property(p => p.LendingDepartmentId).UseUserBussinessDepartmentEditor().Show();
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.ApprovalStatus).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; }).Show();
        }
    }
}
