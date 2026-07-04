using SIE.CSM.Suppliers;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipModels;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 备件入库查询实体视图配置
    /// </summary>
    internal class SparePartStoreCriteriaViewConfig : WebViewConfig<SparePartStoreCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.StoreCode);
            View.Property(p => p.LinkCode);
            View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
            }); 
            View.Property(p => p.SupplierId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<SupplierController>().GetSuppliers(pagingInfo, keyword);
            }); 
            View.Property(p => p.InboundType);
            View.Property(p => p.InboundStatus);
            View.Property(p => p.SparePartId)
                .UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<SparePartController>().GetSpareParts(pagingInfo, keyword);
                })
                .UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.SparePartName), nameof(r.SparePart.SparePartName));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.SparePartName);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
