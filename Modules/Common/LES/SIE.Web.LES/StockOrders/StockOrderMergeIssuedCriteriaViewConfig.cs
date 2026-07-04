using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;

namespace SIE.LES.StockOrders
{
    internal class StockOrderMergeIssuedCriteriaViewConfig : WebViewConfig<StockOrderMergeIssuedCriteria>
    {
        protected override void ConfigQueryView()
        {           
            View.Property(p => p.WipResourceId).UsePagingLookUpEditor((m, r) => 
            {
                m.DisplayField = WipResource.NameProperty.Name;
                m.BindDisplayField = WipResource.NameProperty.Name;
            }).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(pagingInfo, keyword);
            });
            View.Property(p => p.StockModel);
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            });
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.LastMonth; }).Show(ShowInWhere.All);
            View.Property(p => p.CreateById).UsePagingLookUpEditor(p =>
            {
                p.SearchFieldList.Add(Employee.CodeProperty.Name);
                p.SearchFieldList.Add(Employee.NameProperty.Name);
            }).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
            }).Show(ShowInWhere.All);
        }
    }
}
