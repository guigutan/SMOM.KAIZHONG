using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("设备保养任务预警时间设置")]
    [System.ComponentModel.Description("保养任务预警时间设置：提前预警时间、超时预警时间、保养确认超时预警时间")]
    public class MaintainAlertTimeConfig : ModuleConfig<MaintainAlertTimeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainAlertTimeConfigValue defaultValue = new MaintainAlertTimeConfigValue { AlertTime = null, ExpiredTime = null, MaintainConfirmExpiredTime = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainAlertTimeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备保养任务预警时间设置")]
    public class MaintainAlertTimeConfigValue : ConfigValue
    {
        #region 提前预警时间(小时) AlertTime
        /// <summary>
        /// 提前预警时间(小时)
        /// </summary>
        [Label("提前预警时间(小时)")]
        public static readonly Property<int?> AlertTimeProperty = P<MaintainAlertTimeConfigValue>.Register(e => e.AlertTime);

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
        public static readonly Property<int?> ExpiredTimeProperty = P<MaintainAlertTimeConfigValue>.Register(e => e.ExpiredTime);

        /// <summary>
        /// 超时预警时间(小时)
        /// </summary>
        public int? ExpiredTime
        {
            get { return this.GetProperty(ExpiredTimeProperty); }
            set { this.SetProperty(ExpiredTimeProperty, value); }
        }
        #endregion

        #region 保养确认超时预警时间(小时) MaintainConfirmExpiredTime
        /// <summary>
        /// 保养确认超时预警时间(小时)
        /// </summary>
        [Label("保养确认超时预警时间(小时)")]
        public static readonly Property<int?> MaintainConfirmExpiredTimeProperty = P<MaintainAlertTimeConfigValue>.Register(e => e.MaintainConfirmExpiredTime);

        /// <summary>
        /// 保养确认超时预警时间(小时)
        /// </summary>
        public int? MaintainConfirmExpiredTime
        {
            get { return this.GetProperty(MaintainConfirmExpiredTimeProperty); }
            set { this.SetProperty(MaintainConfirmExpiredTimeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return AlertTime == null ? "" : (AlertTime.ToString() + ";")
                + ExpiredTime == null ? "" : (ExpiredTime.ToString() + ";")
                + MaintainConfirmExpiredTime == null ? "" : (MaintainConfirmExpiredTime.ToString() + ";");
        }
    }
}
