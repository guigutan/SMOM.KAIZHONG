using SIE.Domain;
using SIE.LES.LinesideWarehouses;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Web.LES.Extensions;
using SIE.Web.LES.LinesideWarehouses.Commands;
using SIE.Web.Resources;
using System.Collections.Generic;

namespace SIE.Web.LES.LinesideWarehouses
{
    /// <summary>
    ///产线线边仓
    /// </summary>
    public class LinesideWarehouseViewConfig : WebViewConfig<LinesideWarehouse>
    {

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.LES.LinesideWarehouses.Scripts.LinesideWarehouseBehavior");
            View.UseDefaultCommands().ReplaceCommands(WebCommandNames.Save, typeof(LinesideWarehouseSaveCommand).FullName)
                .ReplaceCommands(WebCommandNames.Add, typeof(LinesideWarehouseAddCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.WipResouceId).UseWorkShopWipResourceEditor().HasLabel("资源").Show(ShowInWhere.All);
                View.Property(p => p.WorkShopId).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EnterpriseController>().GetWorkShops(p, k, null);
                }).HasLabel("车间").Readonly(p => p.WipResouceId != null);
                View.Property(p => p.FactoryId).HasLabel("工厂").Show().Readonly();
                View.Property(p => p.Warehouse).UseDataSource((x, y, z) =>
                {
                    return RT.Service.Resolve<LinesideWarehouseController>().GetAvailableLinesideWarehouses(y, z, true);
                }).HasLabel("仓库").Cascade(p => p.StorageLocationId, null);
                View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
                {
                    var model = o as LinesideWarehouse;
                    if (model == null)
                    {
                        return new EntityList<LinesideWarehouse>();
                    }
                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocationDatas(model.WarehouseId, r, e, true);
                }).HasLabel("库位");
                View.Property(p => p.AutoReceive).Show();
            }
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.WipResouceCode);
            View.Property(p => p.WipResouceName);
            View.Property(p => p.FactoryCode);
            View.Property(p => p.FactoryName);
            View.Property(p => p.WorkShopCode);
            View.Property(p => p.WorkShopName);
            View.Property(p => p.LocaltionCode);
            View.Property(p => p.LocaltionName); 
        }
    }
}
