using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Configs
{
    /// <summary>
    /// 产品BOM版本配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品BOM版本配置")]
    public class ProductBomVersionConfigValue : ConfigValue
    {
        #region 版本 Version
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static readonly IRefIdProperty VersionIdProperty =
            P<ProductBomVersionConfigValue>.RegisterRefId(e => e.VersionId, ReferenceType.Normal);

        /// <summary>
        /// 版本Id
        /// </summary>
        public double? VersionId
        {
            get { return (double?)this.GetRefNullableId(VersionIdProperty); }
            set { this.SetRefNullableId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> VersionProperty =
            P<ProductBomVersionConfigValue>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 版本
        /// </summary>
        public NumberRule Version
        {
            get { return this.GetRefEntity(VersionProperty); }
            set { this.SetRefEntity(VersionProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (Version == null)
                return "NIL";

            return Version.Name;
        }
    }

    /// <summary>
    /// 产品BOM版本配置
    /// </summary>
    [System.ComponentModel.DisplayName("产品BOM版本生成规则")]
    [System.ComponentModel.Description("用于添加产品BOM时版本的生成规则")]
    public class ProductBomVersionConfig : ModuleConfig<ProductBomVersionConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ProductBomVersionConfigValue defaultValue = new ProductBomVersionConfigValue { Version = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ProductBomVersionConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}
