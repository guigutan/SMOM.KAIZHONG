using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.AssetScraps.Configs
{
    /// <summary>
    /// 是否启用资产处置配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否启用资产处置配置")]
    [System.ComponentModel.Description("用于配置是否启用资产处置")]
    public class EnableAssetDisposalConfig : ModuleConfig<EnableAssetDisposalConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EnableAssetDisposalConfigValue defaultValue = new EnableAssetDisposalConfigValue { IsEnableAssetDisposal = false};

        /// <summary>
        /// 默认值
        /// </summary>
        public override EnableAssetDisposalConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 是否启用资产处置配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否启用资产处置配置实体")]
    public class EnableAssetDisposalConfigValue : ConfigValue
    {
        #region 是否启用资产处置 IsEnableAssetDisposal
        /// <summary>
        /// 是否启用资产处置
        /// </summary>
        [Label("启用资产处置")]
        public static readonly Property<bool> IsEnableAssetDisposalProperty = P<EnableAssetDisposalConfigValue>.Register(e => e.IsEnableAssetDisposal);

        /// <summary>
        /// 是否启用资产处置
        /// </summary>
        public bool IsEnableAssetDisposal
        {
            get { return this.GetProperty(IsEnableAssetDisposalProperty); }
            set { this.SetProperty(IsEnableAssetDisposalProperty, value); }
        }
        #endregion

        #region 设备报废时固定资产报废 IsEnableFixedAssetScrap
        /// <summary>
        /// 设备报废时固定资产报废
        /// </summary>
        [Label("设备报废时固定资产报废")]
        public static readonly Property<bool> IsEnableFixedAssetScrapProperty = P<EnableAssetDisposalConfigValue>.Register(e => e.IsEnableFixedAssetScrap);

        /// <summary>
        /// 设备报废时固定资产报废
        /// </summary>
        public bool IsEnableFixedAssetScrap
        {
            get { return this.GetProperty(IsEnableFixedAssetScrapProperty); }
            set { this.SetProperty(IsEnableFixedAssetScrapProperty, value); }
        }
        #endregion


        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return (IsEnableAssetDisposal ? "启用资产处置;".L10N() : "不启用资产处置;".L10N());
        }
    }
}
