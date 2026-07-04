using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Configs;
using SIE.ObjectModel;
using SIE.Packages.Packings.Enums;
using System;

namespace SIE.MES.WIP.Packings.Configs
{
    /// <summary>
    /// 包装采集配置
    /// </summary>
    [System.ComponentModel.DisplayName("包装采集配置")]
    [System.ComponentModel.Description("标签，打印")]
    public class WipPackingConfig : ModuleCategoryConfig<ResourceStation, WipPackingConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly WipPackingConfigValue defaultValue = new WipPackingConfigValue { AutoDoPackingMode = AutoDoPackingMode.AutoCasePacking, IsAutoPrintPackageLabel = true, IsContinuityControl = true, PackingRuleValidMode = PackingRuleValidMode.Current };

        /// <summary>
        /// 默认值
        /// </summary>
        public override WipPackingConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 包装采集配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装便捷性配置")]
    [DisplayMember(nameof(Id))]
    public class WipPackingConfigValue : ConfigValue
    {
        #region 自动打印包装标签 IsAutoPrintPackageLabel
        /// <summary>
        /// 自动打印包装标签
        /// </summary>
        [Label("自动打印包装标签")]
        public static readonly Property<bool> IsAutoPrintPackageLabelProperty = P<WipPackingConfigValue>.Register(e => e.IsAutoPrintPackageLabel);

        /// <summary>
        /// 自动打印包装标签
        /// </summary>
        public bool IsAutoPrintPackageLabel
        {
            get { return this.GetProperty(IsAutoPrintPackageLabelProperty); }
            set { this.SetProperty(IsAutoPrintPackageLabelProperty, value); }
        }
        #endregion

        #region 自动打包 AutoDoPackingMode
        /// <summary>
        /// 自动打包
        /// </summary>
        [Label("自动打包")]
        public static readonly Property<AutoDoPackingMode> AutoDoPackingProperty = P<WipPackingConfigValue>.Register(e => e.AutoDoPackingMode);

        /// <summary>
        /// 自动打包
        /// </summary>
        public AutoDoPackingMode AutoDoPackingMode
        {
            get { return this.GetProperty(AutoDoPackingProperty); }
            set { this.SetProperty(AutoDoPackingProperty, value); }
        }
        #endregion

        #region 连续扫码控制 IsContinuityControl
        /// <summary>
        /// 连续扫码控制
        /// </summary>
        [Label("连续扫码控制")]
        public static readonly Property<bool> IsContinuityControlProperty = P<WipPackingConfigValue>.Register(e => e.IsContinuityControl);

        /// <summary>
        /// 连续扫码控制
        /// </summary>
        public bool IsContinuityControl
        {
            get { return this.GetProperty(IsContinuityControlProperty); }
            set { this.SetProperty(IsContinuityControlProperty, value); }
        }
        #endregion

        #region 包装规则兼容型验证方式（验证规格） PackingRuleValidMode
        /// <summary>
        /// 包装规则兼容型验证方式（验证规格）
        /// </summary>
        [Label("包装规则兼容型难方式（验证规格）")]
        public static readonly Property<PackingRuleValidMode> PackingRuleValidModeProperty = P<WipPackingConfigValue>.Register(e => e.PackingRuleValidMode);

        /// <summary>
        /// 包装规则兼容型验证方式（验证规格）
        /// </summary>
        public PackingRuleValidMode PackingRuleValidMode
        {
            get { return this.GetProperty(PackingRuleValidModeProperty); }
            set { this.SetProperty(PackingRuleValidModeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示包装规则兼容型验证方式
        /// </summary>
        /// <returns>打包</returns>
        public override string Display()
        {
            return "打包:[{0}] | 打印标签:[{1}] | 连续扫码控制:[{2}] | 规格验证:[{3}]".L10nFormat(AutoDoPackingMode.ToLabel().L10N(), IsAutoPrintPackageLabel.ToString(), IsContinuityControl.ToString(), PackingRuleValidMode.ToLabel().L10N());
        }
    }
}