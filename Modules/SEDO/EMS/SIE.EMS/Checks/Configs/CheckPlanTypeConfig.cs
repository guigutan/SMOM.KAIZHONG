using SIE.Common.Configs;
using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.ObjectModel;
using SIE.Resources.CalendarSchemes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SIE.EMS.Enums;

namespace SIE.EMS.Checks.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("设备点检类型维护")]
    [System.ComponentModel.Description("通过配置项维护设备点检类型")]
    public class CheckPlanTypeConfig : ModuleConfig<CheckPlanTypeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckPlanTypeConfigValue defaultValue = new CheckPlanTypeConfigValue {
            CalendarSchemeId = RF.GetAll<CalendarScheme>().FirstOrDefault(p => p.IsDefault == YesNo.Yes)?.Id,
            CalendarScheme = RF.GetAll<CalendarScheme>().FirstOrDefault(p=>p.IsDefault == YesNo.Yes), 
            CheckPlanType = CheckPlanType.Day, Frequency = 8, };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckPlanTypeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检计划类型配置")]
    public class CheckPlanTypeConfigValue : ConfigValue
    {
        #region 日历方案 CalendarScheme
        /// <summary>
        /// 日历方案Id
        /// </summary>
        [Label("日历方案")]
        public static readonly IRefIdProperty CalendarSchemeIdProperty =
            P<CheckPlanTypeConfigValue>.RegisterRefId(e => e.CalendarSchemeId, ReferenceType.Normal);

        /// <summary>
        /// 日历方案Id
        /// </summary>
        public double? CalendarSchemeId
        {
            get { return (double?)this.GetRefId(CalendarSchemeIdProperty); }
            set { this.SetRefId(CalendarSchemeIdProperty, value); }
        }

        /// <summary>
        /// 日历方案
        /// </summary>
        public static readonly RefEntityProperty<CalendarScheme> CalendarSchemeProperty =
            P<CheckPlanTypeConfigValue>.RegisterRef(e => e.CalendarScheme, CalendarSchemeIdProperty);

        /// <summary>
        /// 日历方案
        /// </summary>
        public CalendarScheme CalendarScheme
        {
            get { return this.GetRefEntity(CalendarSchemeProperty); }
            set { this.SetRefEntity(CalendarSchemeProperty, value); }
        }
        #endregion

        #region 点检计划类型 CheckPlanType
        /// <summary>
        /// 点检计划类型
        /// </summary>
        [Label("点检计划类型")]
        public static readonly Property<CheckPlanType> CheckPlanTypeProperty = P<CheckPlanTypeConfigValue>.Register(e => e.CheckPlanType);

        /// <summary>
        /// 点检计划类型
        /// </summary>
        public CheckPlanType CheckPlanType
        {
            get { return this.GetProperty(CheckPlanTypeProperty); }
            set { this.SetProperty(CheckPlanTypeProperty, value); }
        }
        #endregion

        #region 频次/小时 Frequency
        /// <summary>
        /// 频次/小时
        /// </summary>
        [Label("频次/小时")]
        public static readonly Property<int?> FrequencyProperty = P<CheckPlanTypeConfigValue>.Register(e => e.Frequency);

        /// <summary>
        /// 频次/小时
        /// </summary>
        public int? Frequency
        {
            get { return this.GetProperty(FrequencyProperty); }
            set { this.SetProperty(FrequencyProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return CheckPlanType == CheckPlanType.Day ? "按日点检".L10N() :
                (CheckPlanType == CheckPlanType.Shift ? "按班点检".L10N() : "按时间频次点检".L10N());
        }
    }
}
