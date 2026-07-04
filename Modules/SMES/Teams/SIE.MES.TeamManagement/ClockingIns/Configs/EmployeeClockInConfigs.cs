using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 员工出勤配置值
    /// </summary>
    [RootEntity, Serializable]
    public class EmployeeClockInSetConfigValue : ConfigValue
    {
        #region 上班打卡时间 OnDutyTimeType
        /// <summary>
        /// 上班打卡时间
        /// </summary>
        [Label("上班打卡时间")]
        public static readonly Property<OnDutyType> OnDutyTimeTypeProperty = P<EmployeeClockInSetConfigValue>.Register(e => e.OnDutyTimeType);

        /// <summary>
        /// 上班打卡时间
        /// </summary>
        public OnDutyType OnDutyTimeType
        {
            get { return this.GetProperty(OnDutyTimeTypeProperty); }
            set { this.SetProperty(OnDutyTimeTypeProperty, value); }
        }
        #endregion

        #region 下班打卡时间 OffDutyTimeType
        /// <summary>
        /// 下班打卡时间
        /// </summary>
        [Label("下班打卡时间")]
        public static readonly Property<OnDutyType> OffDutyTimeTypeProperty = P<EmployeeClockInSetConfigValue>.Register(e => e.OffDutyTimeTypeType);

        /// <summary>
        /// 下班打卡时间
        /// </summary>
        public OnDutyType OffDutyTimeTypeType
        {
            get { return this.GetProperty(OffDutyTimeTypeProperty); }
            set { this.SetProperty(OffDutyTimeTypeProperty, value); }
        }
        #endregion

        #region 允许时间范围
        #region 班次开始前 ShiftBeginBefore 
        /// <summary>
        /// 班次开始前
        /// </summary>
        [Label("班次开始时间前（分钟）")]
        public static readonly Property<int> ShiftBeginBeforeProperty = P<EmployeeClockInSetConfigValue>.Register(e => e.ShiftBeginBefore);

        /// <summary>
        /// 班次开始前
        /// </summary>
        public int ShiftBeginBefore
        {
            get { return this.GetProperty(ShiftBeginBeforeProperty); }
            set { this.SetProperty(ShiftBeginBeforeProperty, value); }
        }
        #endregion

        #region 班次开始时间后 ShiftBeginAfter 
        /// <summary>
        /// 班次开始后
        /// </summary>
        [Label("班次开始时间后（分钟）")]
        public static readonly Property<int> ShiftBeginAfterProperty = P<EmployeeClockInSetConfigValue>.Register(e => e.ShiftBeginAfter);

        /// <summary>
        /// 班次开始后
        /// </summary>
        public int ShiftBeginAfter
        {
            get { return this.GetProperty(ShiftBeginAfterProperty); }
            set { this.SetProperty(ShiftBeginAfterProperty, value); }
        }
        #endregion

        #region 班次结束前 ShiftEndBefore 
        /// <summary>
        /// 班次结束前
        /// </summary>
        [Label("班次结束时间前（分钟）")]
        public static readonly Property<int> ShiftEndBeforeProperty = P<EmployeeClockInSetConfigValue>.Register(e => e.ShiftEndBefore);

        /// <summary>
        /// 班次结束前
        /// </summary>
        public int ShiftEndBefore
        {
            get { return this.GetProperty(ShiftEndBeforeProperty); }
            set { this.SetProperty(ShiftEndBeforeProperty, value); }
        }
        #endregion

        #region 班次结束时间后 ShiftEndAfter 
        /// <summary>
        /// 班次结束后
        /// </summary>
        [Label("班次结束时间后（分钟）")]
        public static readonly Property<int> ShiftEndAfterProperty = P<EmployeeClockInSetConfigValue>.Register(e => e.ShiftEndAfter);

        /// <summary>
        /// 班次结束后
        /// </summary>
        public int ShiftEndAfter
        {
            get { return this.GetProperty(ShiftEndAfterProperty); }
            set { this.SetProperty(ShiftEndAfterProperty, value); }
        }
        #endregion
        #endregion

        /// <summary>
        /// 获取显示值
        /// </summary>
        /// <returns>显示值</returns>
        public override string Display()
        {
            return base.Display();
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        public EmployeeClockInSetConfigValue()
        {
            OnDutyTimeType = OnDutyType.Earliest;
            OffDutyTimeTypeType = OnDutyType.Latest;
            ShiftBeginBefore = 30;
            ShiftBeginAfter = 30;
            ShiftEndBefore = 30;
            ShiftEndAfter = 30;
        }
    }

    /// <summary>
    /// 员工出勤配置
    /// </summary>
    [DisplayName("员工出勤配置")]
    [Description("员工出勤配置")]
    public class EmployeeClockInSetConfig : ModuleConfig<EmployeeClockInSetConfigValue>
    {
        /// <summary>
        /// 员工出勤配置值
        /// </summary>
        readonly EmployeeClockInSetConfigValue defVal = new EmployeeClockInSetConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override EmployeeClockInSetConfigValue DefaultValue
        {
            get { return defVal; }
        }
    }
}
