using SIE.EMS.AssetScraps;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetScraps
{
    /// <summary>
    /// 资产报废查询实体视图配置
    /// </summary>
    internal class AssetScrapCriteriaViewConfig : WebViewConfig<AssetScrapCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No).Show();
            View.Property(p => p.QureyFactoryId).UseFactoryEditor().Show();
            View.Property(p => p.AssetObject).Show();
            View.Property(p => p.ManageDeptId).UseUserBussinessDepartmentEditor().Show();
            View.Property(p => p.UseDeptId).UseUserBussinessDepartmentEditor().Show();
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.ApprovalStatus).Show();
            View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.All; }).Show();
        }
    }
}
