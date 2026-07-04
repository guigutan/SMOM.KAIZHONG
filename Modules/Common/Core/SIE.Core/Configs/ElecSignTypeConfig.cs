using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 签名方式配置项
    /// </summary>
    [System.ComponentModel.DisplayName("签名方式配置项")]
    [System.ComponentModel.Description("签名方式配置项")]
    public class ElecSignTypeConfig : GlobalConfig<ElecSignTypeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ElecSignTypeConfigValue defaultValue = new ElecSignTypeConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ElecSignTypeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// RFID类型配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("签名方式配置项")]
    public class ElecSignTypeConfigValue : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ElecSignTypeConfigValue()
        {
            ElecSignTypeValue = ElecSignTypeValue.UserPwd;
        }

        /// <summary>
        /// 电子签名密码方式
        /// </summary>
        [Label("电子签名方式")]
        public static readonly Property<ElecSignTypeValue> ElecSignTypeValueProperty = P<ElecSignTypeConfigValue>.Register(e => e.ElecSignTypeValue);

        /// <summary>
        /// 电子签名密码方式
        /// </summary>
        public ElecSignTypeValue ElecSignTypeValue
        {
            get { return this.GetProperty(ElecSignTypeValueProperty); }
            set { this.SetProperty(ElecSignTypeValueProperty, value); }
        }

        /// <summary>
        /// 显示电子签名方式
        /// </summary>
        /// <returns>电子签名方式</returns>
        public override string Display()
        {
            return ElecSignTypeValue.ToLabel().L10N();
        }
    }
}
