using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.EmployeeReports.ClockingIns
{
    /// <summary>
    /// 员工出勤
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EmployeeClockInReportCriteria))]
    [EntityWithConfig(typeof(EmployeeClockInSetConfig))]
    [Label("员工出勤")]
    public class EmployeeClockInReport : EmployeeClockIn
    {
        #region 打卡记录列表 ClockInDetail
        /// <summary>
        /// 打卡记录列表
        /// </summary>
        public static new readonly ListProperty<EntityList<ClockInDetailReport>> ClockInDetailProperty = P<EmployeeClockInReport>.RegisterList(e => e.ClockInDetail);

        /// <summary>
        /// 打卡记录列表
        /// </summary>
        public new EntityList<ClockInDetailReport> ClockInDetail
        {
            get { return this.GetLazyList(ClockInDetailProperty); }
        }
        #endregion    
    }
}
