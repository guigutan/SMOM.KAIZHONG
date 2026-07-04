using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 拉式物料扣料有效期配置
    /// </summary>
    [System.ComponentModel.DisplayName("拉式物料扣料有效期")]
    [System.ComponentModel.Description("拉式物料不够扣数时，只能扣减上料X天内的物料，已用完且超过有效期的拉式物料有可能被清空")]
    [ConfigForEntity(typeof(LoadItem))]
    public class PullItemExpiryConfig : ModuleConfig<PullItemExpiryConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly PullItemExpiryConfigValue defaultValue = new PullItemExpiryConfigValue { Expiry = 15 };

        /// <summary>
        /// 默认值
        /// </summary>
        public override PullItemExpiryConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 拉式物料扣料有效期配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("拉式物料扣料有效期")]
    public class PullItemExpiryConfigValue : ConfigValue
    {
        #region 有效期 Expiry
        /// <summary>
        /// 有效期
        /// </summary>
        [Label("有效期")]
        public static readonly Property<int> ExpiryProperty = P<PullItemExpiryConfigValue>.Register(e => e.Expiry);

        /// <summary>
        /// 有效期
        /// </summary>
        public int Expiry
        {
            get { return this.GetProperty(ExpiryProperty); }
            set { this.SetProperty(ExpiryProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return "{0}天".L10nFormat(Expiry);
        }
    }
}
