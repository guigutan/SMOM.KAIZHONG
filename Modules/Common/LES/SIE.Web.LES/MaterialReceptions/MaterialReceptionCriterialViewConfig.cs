using SIE.LES.MaterialReceptions;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.MaterialReceptions
{
    /// <summary>
    /// 物料接收查询视图配置
    /// </summary>
    public class MaterialReceptionCriterialViewConfig : WebViewConfig<MaterialReceptionCriterial>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.StockOrderNo);
                View.Property(p => p.State);
                View.Property(p => p.ItemCode);
                View.Property(p => p.ItemName);
                View.Property(p => p.LabelNo);
                View.Property(p => p.LotNo);
                View.Property(p => p.Warehouse).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
                });
                View.Property(p => p.WorkOrderNo);
                View.Property(p => p.Resource).UsePagingLookUpEditor((m, r) => 
                {
                    m.DisplayField = WipResource.NameProperty.Name;
                    m.BindDisplayField = WipResource.NameProperty.Name;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(pagingInfo, keyword);
                });
                View.Property(p => p.SoNo);
                View.Property(p => p.Receiver).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                })
;
                View.Property(p => p.ReceiverTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            }
        }
    }
}
