using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.EmployeeReports.ClockingIns
{
    /// <summary>
    /// 打卡记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("打卡记录")]
    public class ClockInDetailReport : ClockInDetail
    {
        #region 员工出勤 ClockIn
        /// <summary>
        /// 员工出勤Id
        /// </summary>
        public static new readonly IRefIdProperty ClockInIdProperty = P<ClockInDetailReport>.RegisterRefId(e => e.ClockInId, ReferenceType.Parent);

        /// <summary>
        /// 员工出勤Id
        /// </summary>
        public new double ClockInId
        {
            get { return (double)GetRefId(ClockInIdProperty); }
            set { SetRefId(ClockInIdProperty, value); }
        }

        /// <summary>
        /// 员工出勤
        /// </summary>
        public static new readonly RefEntityProperty<EmployeeClockInReport> ClockInProperty = P<ClockInDetailReport>.RegisterRef(e => e.ClockIn, ClockInIdProperty);

        /// <summary>
        /// 员工出勤
        /// </summary>
        public new EmployeeClockInReport ClockIn
        {
            get { return GetRefEntity(ClockInProperty); }
            set { SetRefEntity(ClockInProperty, value); }
        }
        #endregion
    }
}
