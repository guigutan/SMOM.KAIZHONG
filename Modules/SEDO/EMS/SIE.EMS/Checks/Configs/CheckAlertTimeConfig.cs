using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("设备点检任务预警时间设置")]
    [System.ComponentModel.Description("点检任务预警时间设置：提前预警时间、超时预警时间、点检确认超时预警时间")]
    public class CheckAlertTimeConfig : ModuleConfig<CheckAlertTimeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckAlertTimeConfigValue defaultValue = new CheckAlertTimeConfigValue { AlertTime = 1,ExpiredTime=1,CheckConfirmExpiredTime=1 };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckAlertTimeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备点检任务预警时间设置")]
    public class CheckAlertTimeConfigValue : ConfigValue
    {
        #region 提前预警时间(小时) AlertTime
        /// <summary>
        /// 提前预警时间(小时)
        /// </summary>
        [Label("提前预警时间(小时)")]
        public static readonly Property<int?> AlertTimeProperty = P<CheckAlertTimeConfigValue>.Register(e => e.AlertTime);

        /// <summary>
        /// 提前预警时间(小时)
        /// </summary>
        public int? AlertTime
        {
            get { return this.GetProperty(AlertTimeProperty); }
            set { this.SetProperty(AlertTimeProperty, value); }
        }
        #endregion

        #region 超时预警时间(小时) ExpiredTime
        /// <summary>
        /// 超时预警时间(小时)
        /// </summary>
        [Label("超时预警时间(小时)")]
        public static readonly Property<int?> ExpiredTimeProperty = P<CheckAlertTimeConfigValue>.Register(e => e.ExpiredTime);

        /// <summary>
        /// 超时预警时间(小时)
        /// </summary>
        public int? ExpiredTime
        {
            get { return this.GetProperty(ExpiredTimeProperty); }
            set { this.SetProperty(ExpiredTimeProperty, value); }
        }
        #endregion

        #region 点检确认超时预警时间(小时) CheckConfirmExpiredTime
        /// <summary>
        /// 点检确认超时预警时间(小时)
        /// </summary>
        [Label("点检确认超时预警时间(小时)")]
        public static readonly Property<int?> CheckConfirmExpiredTimeProperty = P<CheckAlertTimeConfigValue>.Register(e => e.CheckConfirmExpiredTime);

        /// <summary>
        /// 点检确认超时预警时间(小时)
        /// </summary>
        public int? CheckConfirmExpiredTime
        {
            get { return this.GetProperty(CheckConfirmExpiredTimeProperty); }
            set { this.SetProperty(CheckConfirmExpiredTimeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return AlertTime==null?"": (AlertTime.ToString()+";")
                + ExpiredTime == null ? "" : (ExpiredTime.ToString() + ";")
                + CheckConfirmExpiredTime == null ? "" : (CheckConfirmExpiredTime.ToString() + ";");
        }
    }
}
