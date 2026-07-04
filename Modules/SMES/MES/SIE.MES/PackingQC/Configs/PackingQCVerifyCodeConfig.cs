using SIE.Common.Configs;
using SIE.Common.Configs.Module;
using SIE.Domain;
using SIE.MES.WIP.Pressure.Configs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC.Configs
{
    /// <summary>
    /// 包装SN重复配置
    /// </summary>
    [DisplayName("包装SN重复配置")]
    [Description("包装SN重复配置")]
    public class PackingQCVerifyCodeConfig:ModuleConfig<PackingQCVerifyCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override PackingQCVerifyCodeConfigValue DefaultValue { get; } = new PackingQCVerifyCodeConfigValue { VerifyCode = "123456"};

    }

    /// <summary>
    /// 包装SN重复配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装SN重复配置")]
    public class PackingQCVerifyCodeConfigValue : ConfigValue
    {
        #region SN重复验证码 VerifyCode
        /// <summary>
        /// SN重复验证码
        /// </summary>
        [Label("SN重复验证码")]
        public static readonly Property<string> VerifyCodeProperty = P<PackingQCVerifyCodeConfigValue>.Register(e => e.VerifyCode);

        /// <summary>
        /// SN重复验证码
        /// </summary>
        public string VerifyCode
        {
            get { return this.GetProperty(VerifyCodeProperty); }
            set { this.SetProperty(VerifyCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>串口</returns>
        public override string Display()
        {
            return "SN重复验证: {0} ".L10nFormat(VerifyCode);
        }
    }
}
