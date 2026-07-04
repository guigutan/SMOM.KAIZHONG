using SIE.Equipments.DeviceIOTParas.Criterias;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Web.Common;

namespace SIE.Web.Equipments.DeviceIOTParas.Criterias
{
    /// <summary>
    /// 物联参数视图配置
    /// </summary>
    public class DeviceIOTParaCirteriaViewConfig : WebViewConfig<DeviceIOTParaCirteria>
    {
        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            base.ConfigQueryView();
            View.Property(p => p.Code);
            View.Property(p => p.EquipModelId).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(p, k);
            });
            View.Property(p => p.TypeCategory).UseCatalogEditor(e=> { e.CatalogType = EquipType.EquipTypeCatalogType;e.CatalogReloadData = true; });
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month);
        }
    }
}
