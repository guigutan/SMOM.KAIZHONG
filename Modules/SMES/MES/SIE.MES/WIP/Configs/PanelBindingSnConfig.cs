using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Stations;
using System;
using System.ComponentModel;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 配置拼板码绑定时SN是否重新生成还是使用已生成的SN
    /// </summary>
    [DisplayName("拼板码绑定的SN条码来源配置")]
    [Description("配置拼板码绑定时SN是否重新生成还是使用已生成的SN")]
    public class PanelBindingSnConfig : ModuleCategoryConfig<Station, PanelBindingSnConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override PanelBindingSnConfigValue DefaultValue { get; } = new PanelBindingSnConfigValue();
    }

    /// <summary>
    /// 打印SN值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("拼板码绑定SN配置值")]
    public class PanelBindingSnConfigValue : ConfigValue
    {
        #region 是否生成Sn IsGenerateSn
        /// <summary>
        /// 是否生成Sn
        /// </summary>
        [Label("是否新生成SN")]
        public static readonly Property<bool> IsGenerateSnProperty = P<PanelBindingSnConfigValue>.Register(e => e.IsGenerateSn);

        /// <summary>
        /// 是否生成Sn
        /// </summary>
        public bool IsGenerateSn
        {
            get { return this.GetProperty(IsGenerateSnProperty); }
            set { this.SetProperty(IsGenerateSnProperty, value); }
        }
        #endregion

        #region 打印机 Printer
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<PanelBindingSnConfigValue>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>是否打印SN</returns>
        public override string Display()
        {
            return "绑定条码来源：{0}  打印机：{1}".L10nFormat(IsGenerateSn ? "新生成SN".L10N() : "已生成SN".L10N(), Printer);
        }
    }
}