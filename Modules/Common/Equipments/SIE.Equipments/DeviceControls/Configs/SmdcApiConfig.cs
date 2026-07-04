using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.DeviceControls.Configs
{
    /// <summary>
    /// SMDC接口配置
    /// </summary>
    [System.ComponentModel.DisplayName("SMDC接口配置")]
    [System.ComponentModel.Description("SMDC接口配置")]
    public class SmdcApiConfig : GlobalConfig<SmdcApiConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly SmdcApiConfigValue defaultValue = new SmdcApiConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override SmdcApiConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// SMDC接口配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("SMDC接口配置")]
    public class SmdcApiConfigValue : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmdcApiConfigValue()
        {
            ApiUrl = string.Empty;
        }

        #region Api地址 ApiUrl
        /// <summary>
        /// Api地址
        /// </summary>
        [Label("Api地址")]
        public static readonly Property<string> ApiUrlProperty = P<SmdcApiConfigValue>.Register(e => e.ApiUrl);

        /// <summary>
        /// Api地址
        /// </summary>
        public string ApiUrl
        {
            get { return this.GetProperty(ApiUrlProperty); }
            set { this.SetProperty(ApiUrlProperty, value); }
        }
        #endregion



        /// <summary>
        /// 显示SMDC接口配置值
        /// </summary>
        /// <returns>返回SMDC接口配置值</returns>
        public override string Display()
        {
            return "Api地址:{0}".L10nFormat(ApiUrl);
        }
    }
}
