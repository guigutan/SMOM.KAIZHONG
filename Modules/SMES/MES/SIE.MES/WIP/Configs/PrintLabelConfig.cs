using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 是否打印外标签配置
    /// </summary>
    [System.ComponentModel.DisplayName("打印外标签")]
    [System.ComponentModel.Description("过站成功是否打印外标签条码")]
    public class PrintLabelConfig : ModuleCategoryConfig<Station, PrintLabelConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override PrintLabelConfigValue DefaultValue { get; } = new PrintLabelConfigValue { IsPrint = false };
    }

    /// <summary>
    /// 是否打印外标签值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否打印外标签")]
    public class PrintLabelConfigValue : ConfigValue
    {
        #region 是否打印外标签 IsPrint
        /// <summary>
        /// 是否打印外标签
        /// </summary>
        [Label("打印外标签")]
        public static readonly Property<bool> IsPrintProperty = P<PrintLabelConfigValue>.Register(e => e.IsPrint);

        /// <summary>
        /// 是否打印外标签
        /// </summary>
        public bool IsPrint
        {
            get { return this.GetProperty(IsPrintProperty); }
            set { this.SetProperty(IsPrintProperty, value); }
        }
        #endregion

        #region 打印机 Printer
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<PrintLabelConfigValue>.Register(e => e.Printer);

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
        /// <returns>是否打印物料标签</returns>
        public override string Display()
        {
            return string.Format("{0}-{1}", IsPrint.ToString(), Printer);
        }
    }
}
