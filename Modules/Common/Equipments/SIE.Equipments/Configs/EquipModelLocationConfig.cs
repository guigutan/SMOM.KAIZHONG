using SIE.Common.Configs;
using SIE.Equipments.EquipModels;
using System.ComponentModel;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 用于配置需具体设备类型的设备型号需维护位置列表信息
    /// </summary>
    [DisplayName("位置列表维护条件配置")]
    [Description("用于配置需具体设备类型的设备型号需维护位置列表信息")]
    [ConfigForEntity(typeof(EquipModel))]
    public class EquipModelsLocationConfig : ModuleConfig<EquipModelLocationConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EquipModelLocationConfigValue defaultValue = new EquipModelLocationConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipModelLocationConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}