using SIE.Core.WorkOrders;
using SIE.Items;
using SIE.LES.MaterialMoves;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialMoves
{
    /// <summary>
    /// 挪料记录查询条件
    /// </summary>
    public class MaterialMoveRecordCriteriaViewConfig : WebViewConfig<MaterialMoveRecordCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SourceWo).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, k);
                }).Show();
                View.Property(p => p.TargetWo).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(p, k);
                }).Show();
                View.Property(p => p.Item).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetEnableItemList(k, p);
                }).Show();
                View.Property(p => p.Reason).UseCatalogEditor(p => { p.CatalogType = MaterialMoveRecord.MaterialMoveReasonStr; p.CatalogReloadData = true; });
                View.Property(p => p.Warehouse).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(p, k);
                }).Show();
                View.Property(p => p.SourceType).Show();
                View.Property(p => p.Creater).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployeeList(p, k);
                }).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
            }
        }
    }
}
