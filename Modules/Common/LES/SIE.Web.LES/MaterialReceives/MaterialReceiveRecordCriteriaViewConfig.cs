using SIE.Domain;
using SIE.LES.MaterialReceives;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收查询视图配置
    /// </summary>
    public class MaterialReceiveRecordCriteriaViewConfig : WebViewConfig<MaterialReceiveRecordCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SoNo);
                View.Property(p => p.StockOrderNo);
                View.Property(p => p.ItemCode);
                View.Property(p => p.ItemName);
                //View.Property(p => p.LabelNo);
                //View.Property(p => p.LotNo);
                View.Property(p => p.Warehouse).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
                });
                View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                    if (workshop == null || workshop.Count <= 0)
                        return new EntityList<Enterprise>();
                    workshop.ForEach(p => p.TreePId = null);
                    return workshop;
                }).Show();
                View.Property(p => p.ResourceId).UsePagingLookUpEditor((m, r) => 
                {
                    m.DisplayField = WipResource.NameProperty.Name;
                    m.BindDisplayField = WipResource.NameProperty.Name;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResourcesByKeyword(pagingInfo, keyword);
                });

                View.Property(p => p.State);
                View.Property(p => p.ReceiveType);
                View.Property(p => p.ReceiveById).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetEmployees(pagingInfo, keyword);
                })
;
                View.Property(p => p.ReceiveTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Week);
            }
        }
    }
}
