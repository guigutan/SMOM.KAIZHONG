using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockQueues.Configs
{
    /// <summary>
    /// 月台排队配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(NumberRule))]
    [Label("月台排队配置项内容")]
    public class DockQueueNumberConfigValue : ConfigValue
    {
        #region 排队号生成规则 NumberRule
        /// <summary>
        /// 排队号生成规则Id
        /// </summary>
        [Label("排队号生成规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<DockQueueNumberConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 排队号生成规则Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefId(NumberRuleIdProperty); }
            set { this.SetRefId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 排队号生成规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<DockQueueNumberConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 排队号生成规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 默认打印模板 PrintTemplate
        /// <summary>
        /// 默认打印模板Id
        /// </summary>
        [Label("默认打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<DockQueueNumberConfigValue>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 默认打印模板Id
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefId(PrintTemplateIdProperty); }
            set { this.SetRefId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 默认打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<DockQueueNumberConfigValue>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 默认打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 最大推迟次数 MaxDelayNum
        /// <summary>
        /// 最大推迟次数
        /// </summary>
        [MinValue(0)]
        [Label("最大推迟次数")]
        public static readonly Property<int> MaxDelayNumProperty = P<DockQueueNumberConfigValue>.Register(e => e.MaxDelayNum);

        /// <summary>
        /// 最大推迟次数
        /// </summary>
        public int MaxDelayNum
        {
            get { return this.GetProperty(MaxDelayNumProperty); }
            set { this.SetProperty(MaxDelayNumProperty, value); }
        }
        #endregion

        #region 作业超时强制签出时间(H) CheckOutTimeOut
        /// <summary>
        /// 作业超时强制签出时间(H)
        /// </summary>
        [MinValue(0)]
        [Label("作业超时强制签出时间(H)")]
        public static readonly Property<double> CheckOutTimeOutProperty = P<DockQueueNumberConfigValue>.Register(e => e.CheckOutTimeOut);

        /// <summary>
        /// 作业超时强制签出时间(H)
        /// </summary>
        public double CheckOutTimeOut
        {
            get { return this.GetProperty(CheckOutTimeOutProperty); }
            set { this.SetProperty(CheckOutTimeOutProperty, value); }
        }
        #endregion

        #region 签到超时强制推迟时间(Min) CheckOutDelay
        /// <summary>
        /// 签到超时强制推迟时间(Min)
        /// </summary>
        [MinValue(0)]
        [Label("签到超时强制推迟时间(Min)")]
        public static readonly Property<int> CheckOutDelayProperty = P<DockQueueNumberConfigValue>.Register(e => e.CheckOutDelay);

        /// <summary>
        /// 签到超时强制推迟时间(Min)
        /// </summary>
        public int CheckOutDelay
        {
            get { return this.GetProperty(CheckOutDelayProperty); }
            set { this.SetProperty(CheckOutDelayProperty, value); }
        }
        #endregion

        #region 分配月台后自动签到 AutoCheckIn
        /// <summary>
        /// 分配月台后自动签到
        /// </summary>
        [Label("分配月台后自动签到")]
        public static readonly Property<bool> AutoCheckInProperty = P<DockQueueNumberConfigValue>.Register(e => e.AutoCheckIn);

        /// <summary>
        /// 分配月台后自动签到
        /// </summary>
        public bool AutoCheckIn
        {
            get { return this.GetProperty(AutoCheckInProperty); }
            set { this.SetProperty(AutoCheckInProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string ruleName = "NIL";
            string printName = "NIL";

            if (NumberRule != null)
            {
                ruleName = NumberRule.Name;
            }

            if (PrintTemplate != null)
            {
                printName = PrintTemplate.FileName;
            }
            return "生成规则：{0},打印模板：{1}".L10nFormat(ruleName, printName);
        }
    }
}
