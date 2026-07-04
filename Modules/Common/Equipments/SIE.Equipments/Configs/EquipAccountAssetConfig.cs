using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 设备台账启用固定资产配置
    /// </summary>
    [System.ComponentModel.DisplayName("设备台账配置项")]
    [System.ComponentModel.Description("用于设备台账配置项,具体规则详细请在配置项中进行配置")]
    public class EquipAccountAssetConfig : ModuleConfig<EquipAccountAssetConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EquipAccountAssetConfigValue defaultValue = new EquipAccountAssetConfigValue { Asset = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipAccountAssetConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 设备台账启用固定资产配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账配置项")]
    public class EquipAccountAssetConfigValue : ConfigValue
    {
        #region 设备台账启用固定资产 Asset
        /// <summary>
        /// 启用固定资产
        /// </summary>
        [Label("启用固定资产")]
        public static readonly Property<bool> EquipTypeIdsProperty = P<EquipAccountAssetConfigValue>.Register(e => e.Asset);

        /// <summary>
        /// 启用固定资产
        /// </summary>
        public bool Asset
        {
            get { return GetProperty(EquipTypeIdsProperty); }
            set { SetProperty(EquipTypeIdsProperty, value); }
        }
        #endregion

        #region 启用立卡 UseCard
        /// <summary>
        /// 启用立卡
        /// </summary>
        [Label("启用立卡")]
        public static readonly Property<bool> UseCardProperty = P<EquipAccountAssetConfigValue>.Register(e => e.UseCard);

        /// <summary>
        /// 启用立卡
        /// </summary>
        public bool UseCard
        {
            get { return this.GetProperty(UseCardProperty); }
            set { this.SetProperty(UseCardProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return "启用立卡:{0},启用固定资产:{1}".L10nFormat(UseCard ? "yes" : "no", Asset ? "yes" : "no");
        }
    }
}
