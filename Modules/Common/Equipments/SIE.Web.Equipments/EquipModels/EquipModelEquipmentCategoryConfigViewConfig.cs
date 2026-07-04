using SIE.Common.Catalogs;
using SIE.Equipments.Configs;
using SIE.Equipments.EquipTypes;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipModels
{
    /// <summary>
    /// 设备类别维护配置值视图配置
    /// </summary>
    internal class EquipModelEquipmentCategoryConfigViewConfig : WebViewConfig<EquipModelEquipmentCategoryConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.SpecialCategoryName).Show(ShowInWhere.List).
            UsePagingLookUpGridPopupEditor(p =>
            {
                p.Model = typeof(Catalog).FullName;
                p.XType = "mutilcatalogeditor_special";
                p.DisplayField = "Name";                
            }).HasLabel("特殊设备类别");//特殊设备
            View.Property(p => p.EquipmentMeteringName).Show(ShowInWhere.List)
                .UsePagingLookUpGridPopupEditor(p =>
            {
                p.Model = typeof(Catalog).FullName;
                p.XType = "mutilcatalogeditor_metering";
                p.DisplayField = "Name";                
            }).HasLabel("计量设备类别");//计量设备
            View.Property(c => c.SpecialIds).Show(ShowInWhere.Hide);
            View.Property(c => c.EquipmentMeteringIds).Show(ShowInWhere.Hide);
        }
    }
}