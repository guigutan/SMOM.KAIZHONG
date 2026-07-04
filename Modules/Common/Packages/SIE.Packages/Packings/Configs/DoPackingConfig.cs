using SIE.Common.Configs;
using SIE.Domain;
using SIE.Packages.Packings.Enums;
using SIE.Warehouses;
using System;

namespace SIE.Packages.Packings.Configs
{
    /// <summary>
    /// 包装配置
    /// </summary>
    [System.ComponentModel.DisplayName("包装配置")]
    [System.ComponentModel.Description("自动生成标签，打印,连续")]
    public class DoPackingConfig : ModuleCategoryConfig<Warehouse, DoPackingConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly DoPackingConfigValue defaultValue = new DoPackingConfigValue { AutoDoPackingMode = AutoDoPackingMode.AutoPacking, IsAutoPrintPackageLabel = true, IsContinuityControl = true };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DoPackingConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 包装配置值
    /// </summary>
    [RootEntity, Serializable]
    public class DoPackingConfigValue : ConfigValue
    {
        /// <summary>
        /// 自动打印包装标签
        /// </summary>
        public static readonly Property<bool> IsAutoPrintPackageLabelProperty = P<DoPackingConfigValue>.Register(e => e.IsAutoPrintPackageLabel);

        /// <summary>
        /// 自动打印包装标签
        /// </summary>
        public bool IsAutoPrintPackageLabel
        {
            get { return this.GetProperty(IsAutoPrintPackageLabelProperty); }
            set { this.SetProperty(IsAutoPrintPackageLabelProperty, value); }
        }

        /// <summary>
        /// 自动打包
        /// </summary>
        public static readonly Property<AutoDoPackingMode> AutoDoPackingProperty = P<DoPackingConfigValue>.Register(e => e.AutoDoPackingMode);

        /// <summary>
        /// 自动打包
        /// </summary>
        public AutoDoPackingMode AutoDoPackingMode
        {
            get { return this.GetProperty(AutoDoPackingProperty); }
            set { this.SetProperty(AutoDoPackingProperty, value); }
        }

        /// <summary>
        /// 连续扫码控制
        /// </summary>
        public static readonly Property<bool> IsContinuityControlProperty = P<DoPackingConfigValue>.Register(e => e.IsContinuityControl);

        /// <summary>
        /// 连续扫码控制
        /// </summary>
        public bool IsContinuityControl
        {
            get { return this.GetProperty(IsContinuityControlProperty); }
            set { this.SetProperty(IsContinuityControlProperty, value); }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>打包</returns>
        public override string Display()
        {
            return "打包:[{0}] | 打印标签:[{1}] | 连续扫码控制:[{2}] ".L10nFormat(AutoDoPackingMode.ToLabel().L10N(), IsAutoPrintPackageLabel.ToString(), IsContinuityControl.ToString());
        }
    }


}
