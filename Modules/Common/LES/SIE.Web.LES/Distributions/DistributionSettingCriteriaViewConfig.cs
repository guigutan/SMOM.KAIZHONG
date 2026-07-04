using SIE.LES.Distributions;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Web.LES.Distributions
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class DistributionSettingCriteriaViewConfig : WebViewConfig<DistributionSettingCriteria>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProductLineId).UseDataSource((e, c, r) =>
                {
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, null, sourceType, c, r);
                }).UsePagingLookUpEditor(p => p.DisplayField = "Name").Show();
                View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(c, r, LibraryType.Entity, true);
                }).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateFormat = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show();
                View.Property(p => p.CreateById).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(Employee.CodeProperty.Name);
                    p.SearchFieldList.Add(Employee.NameProperty.Name);
                }).Show();
            }
        }
    }
}
