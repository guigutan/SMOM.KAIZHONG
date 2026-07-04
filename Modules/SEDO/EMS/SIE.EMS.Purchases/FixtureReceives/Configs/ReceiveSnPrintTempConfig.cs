using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.FixtureReceives.Configs
{
    /// <summary>
    /// 序列号编码规则配置
    /// </summary>
    [DisplayName("序列号打印模板配置")]
    [Description("用于配置序列号打印模板")]
    public class ReceiveSnPrintTempConfig : ModuleConfig<ReceiveSnPrintTempConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ReceiveSnPrintTempConfigValue defaultValue = new ReceiveSnPrintTempConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override ReceiveSnPrintTempConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 序列号编码规则配置
    /// </summary>
    [RootEntity, Serializable]
    public class ReceiveSnPrintTempConfigValue : ConfigValue
    {
        #region 打印模板 PrintTemp
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTempIdProperty =
            P<ReceiveSnPrintTempConfigValue>.RegisterRefId(e => e.PrintTempId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double PrintTempId
        {
            get { return (double)this.GetRefId(PrintTempIdProperty); }
            set { this.SetRefId(PrintTempIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTempProperty =
            P<ReceiveSnPrintTempConfigValue>.RegisterRef(e => e.PrintTemp, PrintTempIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public  PrintTemplate PrintTemp
        {
            get { return this.GetRefEntity(PrintTempProperty); }
            set { this.SetRefEntity(PrintTempProperty, value); }
        }
        #endregion


        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            return "序列号打印模板：{0};".L10nFormat(PrintTemp?.FileName);
        }
    }
}
