using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel.View;
using SIE.Web.Common;
using System.Collections.Generic;

namespace SIE.Web.Equipments.DeviceIOTParas
{
    /// <summary>
    /// 设备物联参数视图配置
    /// </summary>
    public class DeviceIOTParaViewConfig : WebViewConfig<DeviceIOTPara>
    {       

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit,
                WebCommandNames.Delete, WebCommandNames.Save);

            View.Property(p => p.Code);
            View.Property(p => p.Name);

            //设备型号
            View.Property(p => p.EquipModel)
                .UsePagingLookUpEditor(
                (m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ModelName), nameof(e.EquipModel.Name));
                    keyValues.Add(nameof(e.DeviceType), nameof(e.EquipModel.TypeName));
                    m.DicLinkField = keyValues;
                }).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
                })
            ;

            View.Property(p => p.ModelName);
            View.Property(p => p.DeviceType).UseCatalogEditor(e => { e.CatalogType = EquipType.EquipTypeCatalogType; e.CatalogReloadData = true; });
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.DeviceType);
            View.Property(p => p.ModelName);
            View.Property(p => p.Code);
            View.Property(p => p.CreateDate).UseDateRangeEditor(e => e.DateRangeType = ObjectModel.DateRangeType.Month);
        }
    }
}
