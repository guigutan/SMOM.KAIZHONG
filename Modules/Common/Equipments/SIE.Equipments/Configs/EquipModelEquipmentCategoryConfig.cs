using SIE.Common.Configs;
using SIE.Equipments.EquipModels;
using System.ComponentModel;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 用于配置特种设备类别 计量设备类别
    /// </summary>
    [DisplayName("维护设备类别")]
    [Description("用于配置特种设备类别/计量设备类别")]
    [ConfigForEntity(typeof(EquipModel))]
    public class EquipModelEquipmentCategoryConfig : ModuleConfig<EquipModelEquipmentCategoryConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EquipModelEquipmentCategoryConfigValue defaultValue = new EquipModelEquipmentCategoryConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipModelEquipmentCategoryConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}