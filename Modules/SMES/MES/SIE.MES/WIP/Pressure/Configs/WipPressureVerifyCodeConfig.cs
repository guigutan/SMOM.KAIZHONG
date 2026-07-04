using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.MES.WIP.Pressure.Configs
{
    /// <summary>
    /// 耐压采集SN超打配置
    /// </summary>
    [DisplayName("耐压采集SN超打配置")]
    [Description("耐压采集SN超打配置")]
    public class WipPressureVerifyCodeConfig : ModuleConfig<WipPressureVerifyCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override WipPressureVerifyCodeConfigValue DefaultValue { get; } = new WipPressureVerifyCodeConfigValue { VerifyCode = "123456", OverPrintPercent = 5 };
    }


    /// <summary>
    /// 耐压采集SN超打配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("耐压采集SN超打配置值")]
    public class WipPressureVerifyCodeConfigValue : ConfigValue
    {

        #region 超打验证码 VerifyCode
        /// <summary>
        /// 超打验证码
        /// </summary>
        [Label("超打验证码")]
        public static readonly Property<string> VerifyCodeProperty = P<WipPressureVerifyCodeConfigValue>.Register(e => e.VerifyCode);

        /// <summary>
        /// 超打验证码
        /// </summary>
        public string VerifyCode
        {
            get { return this.GetProperty(VerifyCodeProperty); }
            set { this.SetProperty(VerifyCodeProperty, value); }
        }
        #endregion

        #region 超打百分比(%) OverPrintPercent
        /// <summary>
        /// 超打百分比(%)
        /// </summary>
        [Label("超打百分比(%)")]
        public static readonly Property<decimal> OverPrintPercentProperty = P<WipPressureVerifyCodeConfigValue>.Register(e => e.OverPrintPercent);

        /// <summary>
        /// 超打百分比(%)
        /// </summary>
        public decimal OverPrintPercent
        {
            get { return this.GetProperty(OverPrintPercentProperty); }
            set { this.SetProperty(OverPrintPercentProperty, value); }
        }
        #endregion

        #region 耐压工序是否跳过 IsNotValidatePressureReport
        /// <summary>
        /// 耐压工序是否跳过
        /// </summary>
        [Label("耐压工序是否跳过")]
        public static readonly Property<bool> IsNotValidatePressureReportProperty = P<WipPressureVerifyCodeConfigValue>.Register(e => e.IsNotValidatePressureReport);

        /// <summary>
        /// 耐压工序是否跳过
        /// </summary>
        public bool IsNotValidatePressureReport
        {
            get { return this.GetProperty(IsNotValidatePressureReportProperty); }
            set { this.SetProperty(IsNotValidatePressureReportProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>串口</returns>
        public override string Display()
        {
            return "超打验证码: {0} 超打百分比: {1}%".L10nFormat(VerifyCode, OverPrintPercent);
        }
    }
}