using SIE.Items;
using SIE.LES.LesStockCounts;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;

namespace SIE.Web.LES.LesStockCounts
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class LesStockCountCriteriaViewConfig : WebViewConfig<LesStockCountCriteria>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.SourceBillNo).Show();
                View.Property(p => p.State).UseEnumMutilEditor(p => p.EnumType = typeof(LesCountState)).Show();
                View.Property(p => p.LesStockCountResult).Show();
                View.Property(p => p.Item).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(Item.CodeProperty.Name);
                    p.SearchFieldList.Add(Item.NameProperty.Name);
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemList(keyword, pagingInfo);
                }).Show();
                View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
                {
                    //return RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(c, r);
                    return RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmp(c, r);
                }).Show();
                View.Property(p => p.CountDimension).Show();
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
