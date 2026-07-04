using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.EquipModelExtensions
{
    /// <summary>
    /// 设备型号扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备型号扩展")]
    [CompiledPropertyDeclarer]
    public static class EquipModelExtension
    {
        #region EquipModelCalibration EquipModelCalibrationList (设备检验规程列表)
        /// <summary>
        /// 设备型号 扩展属性。
        /// </summary>
        [Label("计量校验规程列表")]
        public static readonly ListProperty<EntityList<EquipModelCalibration>> EquipModelCalibrationListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelCalibration>>("EquipModelCalibrationList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取设备型号属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>计量校验规程</returns>
        public static EntityList<EquipModelCalibration> GetEquipModelCalibrationList(EquipModel me)
        {
            return me.GetProperty(EquipModelCalibrationListProperty);
        }

        /// <summary>
        /// 设置计量校验规程 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetEquipModelCalibrationList(EquipModel me, EntityList<EquipModelCalibration> value)
        {
            me.SetProperty(EquipModelCalibrationListProperty, value);
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
