using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Packages.Packings.Enums;
using SIE.Warehouses;
using System;
using System.ComponentModel;

namespace SIE.Tech.Stations.Configs
{
    #region 是否称重配置项
    /// <summary>
    /// 是否称重配置项
    /// </summary>
    [System.ComponentModel.DisplayName("直接包装采集-是否称重配置项")]
    [System.ComponentModel.Description("用于配置资源工位是否启用称重")]
    public class DirectWeightConfig : ModuleCategoryConfig<Station, DirectWeightConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DirectWeightConfigValue DefaultValue { get; } = new DirectWeightConfigValue();
    }

    /// <summary>
    /// 是否称重配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否称重配置值")]
    public class DirectWeightConfigValue : ConfigValue
    {
        #region 是否称重 IsWeight
        /// <summary>
        /// 是否称重
        /// </summary>
        [Label("是否称重")]
        public static readonly Property<bool> IsWeightProperty = P<DirectWeightConfigValue>.Register(e => e.IsWeight);

        /// <summary>
        /// 是否称重
        /// </summary>
        public bool IsWeight
        {
            get { return this.GetProperty(IsWeightProperty); }
            set { this.SetProperty(IsWeightProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return IsWeight ? "启用称重".L10N() : "未启用称重".L10N();
        }
    }
    #endregion

    #region 包装采集配置
    /// <summary>
    /// 包装采集配置
    /// </summary>
    [System.ComponentModel.DisplayName("直接包装采集-包装采集配置")]
    [System.ComponentModel.Description("标签，打印")]
    public class DirectWipPackingConfig : ModuleCategoryConfig<Station, DirectWipPackingConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly DirectWipPackingConfigValue defaultValue = new DirectWipPackingConfigValue { IsAutoPrintPackageLabel = true, IsContinuityControl = true, PackingRuleValidMode = PackingRuleValidMode.Current };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DirectWipPackingConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 包装采集配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("直接包装采集-包装采集配置")]
    [DisplayMember(nameof(Id))]
    public class DirectWipPackingConfigValue : ConfigValue
    {
        #region 自动打包 AutoDoPackingMode
        /// <summary>
        /// 自动打包
        /// </summary>
        [Label("自动打包")]
        public static readonly Property<DirectAutoDoPackingMode> AutoDoPackingModeProperty = P<DirectWipPackingConfigValue>.Register(e => e.AutoDoPackingMode);

        /// <summary>
        /// 自动打包
        /// </summary>
        public DirectAutoDoPackingMode AutoDoPackingMode
        {
            get { return this.GetProperty(AutoDoPackingModeProperty); }
            set { this.SetProperty(AutoDoPackingModeProperty, value); }
        }
        #endregion


        #region 自动打印包装标签 IsAutoPrintPackageLabel
        /// <summary>
        /// 自动打印包装标签
        /// </summary>
        [Label("自动打印包装标签")]
        public static readonly Property<bool> IsAutoPrintPackageLabelProperty = P<DirectWipPackingConfigValue>.Register(e => e.IsAutoPrintPackageLabel);

        /// <summary>
        /// 自动打印包装标签
        /// </summary>
        public bool IsAutoPrintPackageLabel
        {
            get { return this.GetProperty(IsAutoPrintPackageLabelProperty); }
            set { this.SetProperty(IsAutoPrintPackageLabelProperty, value); }
        }
        #endregion

        #region 连续扫码控制 IsContinuityControl
        /// <summary>
        /// 连续扫码控制
        /// </summary>
        [Label("连续扫码控制")]
        public static readonly Property<bool> IsContinuityControlProperty = P<DirectWipPackingConfigValue>.Register(e => e.IsContinuityControl);

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
        [Label("验证方式")]
        public static readonly Property<PackingRuleValidMode> PackingRuleValidModeProperty = P<DirectWipPackingConfigValue>.Register(e => e.PackingRuleValidMode);

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
    #endregion

    #region 包装生成单据配置
    /// <summary>
    /// 包装生成单据配置
    /// </summary>
    [System.ComponentModel.DisplayName("直接包装采集-包装生成单据配置")]
    [System.ComponentModel.Description("包装生成单据配置，仓库")]
    public class DirectWipPackingBillConfig : ModuleCategoryConfig<Station, DirectWipPackingBillConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public DirectWipPackingBillConfigValue defaultValue { get; } = new DirectWipPackingBillConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override DirectWipPackingBillConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 包装生成单据配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装生成单据配置，仓库")]
    [DisplayMember(nameof(Id))]
    public class DirectWipPackingBillConfigValue : ConfigValue
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<DirectWipPackingBillConfigValue>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<DirectWipPackingBillConfigValue>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>仓库名称</returns>
        public override string Display()
        {
            return "入库仓:[{0}]".L10nFormat(Warehouse?.Name);
        }
    }
    #endregion

    #region 包装条码打印方式配置
    /// <summary>
    /// 包装条码打印方式配置
    /// </summary>
    [DisplayName("直接包装采集-包装条码打印方式配置")]
    [Description("用于配置工位的包装条码打印方式")]
    public class DirectPackingPrintModeConfig : ModuleCategoryConfig<Station, DirectPackingPrintModeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DirectPackingPrintModeConfigValue DefaultValue { get; } = new DirectPackingPrintModeConfigValue() { PrintMode = PrintMode.Online };
    }

    /// <summary>
    /// 包装条码打印方式配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装条码打印方式配置值")]
    public class DirectPackingPrintModeConfigValue : ConfigValue
    {
        #region 打印方式 PrintMode
        /// <summary>
        /// 打印方式
        /// </summary>
        [Label("打印方式")]
        public static readonly Property<PrintMode> PrintModeProperty = P<DirectPackingPrintModeConfigValue>.Register(e => e.PrintMode);

        /// <summary>
        /// 打印方式
        /// </summary>
        public PrintMode PrintMode
        {
            get { return this.GetProperty(PrintModeProperty); }
            set { this.SetProperty(PrintModeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>包装条码打印方式</returns>
        public override string Display()
        {
            return "包装条码打印方式：{0}".L10nFormat(PrintMode.ToLabel().L10N());
        }
    }
    #endregion


    #region 新包装采集-包装条码打印方式配置
    /// <summary>
    /// 新包装采集-包装条码打印方式配置
    /// </summary>
    [DisplayName("新包装采集-包装条码打印方式配置")]
    [Description("用于配置工位的包装条码打印方式")]
    public class NewPackingPrintModeConfig : ModuleCategoryConfig<Station, NewPackingPrintModeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override NewPackingPrintModeConfigValue DefaultValue { get; } = new NewPackingPrintModeConfigValue() { PrintMode = PrintMode.Online };
    }

    /// <summary>
    /// 包装条码打印方式配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("新包装采集-包装条码打印方式配置值")]
    public class NewPackingPrintModeConfigValue : ConfigValue
    {
        #region 打印方式 PrintMode
        /// <summary>
        /// 打印方式
        /// </summary>
        [Label("打印方式")]
        public static readonly Property<PrintMode> PrintModeProperty = P<NewPackingPrintModeConfigValue>.Register(e => e.PrintMode);

        /// <summary>
        /// 打印方式
        /// </summary>
        public PrintMode PrintMode
        {
            get { return this.GetProperty(PrintModeProperty); }
            set { this.SetProperty(PrintModeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>包装条码打印方式</returns>
        public override string Display()
        {
            return "新包装条码打印方式：{0}".L10nFormat(PrintMode.ToLabel().L10N());
        }
    }
    #endregion

    /// <summary>
    /// 包装号打印模式
    /// </summary>
    public enum PrintMode
    {
        /// <summary>
        /// 提前打印
        /// </summary>
        [Label("提前打印")]
        InAdvance,

        /// <summary>
        /// 在线打印
        /// </summary>
        [Label("在线打印")]
        Online
    }
}
