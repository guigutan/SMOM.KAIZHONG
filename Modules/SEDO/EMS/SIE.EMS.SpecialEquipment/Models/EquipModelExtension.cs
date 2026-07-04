using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpecialEquipment.Models
{
    /// <summary>
    /// 设备型号扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备型号扩展")]
    [CompiledPropertyDeclarer]
    public static class EquipModelExtension
    {
        #region EquipModelRegularInspection EquipModelRegularInspectionList (设备检验规程列表)
        /// <summary>
        /// 特殊设备列表 扩展属性。
        /// </summary>
        [Label("设备检验规程列表")] 
        public static readonly ListProperty<EntityList<EquipModelRegularInspection>> EquipModelRegularInspectionListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelRegularInspection>>("EquipModelRegularInspectionList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取 特殊设备列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>特殊设备列表</returns>
        public static EntityList<EquipModelRegularInspection> GetEquipModelRegularInspectionList(EquipModel me)
        {
            return me.GetProperty(EquipModelRegularInspectionListProperty);
        }

        /// <summary>
        /// 设置特殊设备列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetEquipModelRegularInspectionList(EquipModel me, EntityList<EquipModelRegularInspection> value)
        {
            me.SetProperty(EquipModelRegularInspectionListProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 设备型号维护 实体配置
    /// </summary>
    public class EquipModelConfig : EntityConfig<EquipModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_MODEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
