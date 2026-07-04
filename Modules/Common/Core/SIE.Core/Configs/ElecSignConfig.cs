using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 电子签名密码配置项
    /// </summary>
    [System.ComponentModel.DisplayName("电子签名密码配置项")]
    [System.ComponentModel.Description("电子签名密码配置项")]
    public class ElecSignConfig : GlobalConfig<ElecSignConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ElecSignConfigValue defaultValue = new ElecSignConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ElecSignConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// RFID类型配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("电子签名密码配置项")]
    public class ElecSignConfigValue : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ElecSignConfigValue()
        {
            ElecSignType = ElecSignType.UserPwd;
        }

        /// <summary>
        /// 电子签名密码方式
        /// </summary>
        [Label("电子签名密码")]
        public static readonly Property<ElecSignType> ElecSignTypeProperty = P<ElecSignConfigValue>.Register(e => e.ElecSignType);

        /// <summary>
        /// 电子签名密码方式
        /// </summary>
        public ElecSignType ElecSignType
        {
            get { return this.GetProperty(ElecSignTypeProperty); }
            set { this.SetProperty(ElecSignTypeProperty, value); }
        }

        /// <summary>
        /// 显示电子签名方式
        /// </summary>
        /// <returns>电子签名方式</returns>
        public override string Display()
        {
            return ElecSignType.ToLabel();
        }
    }
}
