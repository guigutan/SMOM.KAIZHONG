using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 打卡记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("打卡记录")]
    public partial class ClockInDetail : DataEntity
    {
        #region 打卡时间 ClockInDate
        /// <summary>
        /// 打卡时间
        /// </summary>
        [Label("打卡时间")]
        public static readonly Property<DateTime> ClockInDateProperty = P<ClockInDetail>.Register(e => e.ClockInDate);

        /// <summary>
        /// 打卡时间
        /// </summary>
        public DateTime ClockInDate
        {
            get { return GetProperty(ClockInDateProperty); }
            set { SetProperty(ClockInDateProperty, value); }
        }
        #endregion

        #region 打卡地点 ClockInAddress
        /// <summary>
        /// 打卡地点
        /// </summary>
        [Label("打卡地点")]
        public static readonly Property<string> ClockInAddressProperty = P<ClockInDetail>.Register(e => e.ClockInAddress);

        /// <summary>
        /// 打卡地点
        /// </summary>
        public string ClockInAddress
        {
            get { return GetProperty(ClockInAddressProperty); }
            set { SetProperty(ClockInAddressProperty, value); }
        }
        #endregion

        #region 员工出勤 ClockIn
        /// <summary>
        /// 员工出勤Id
        /// </summary>
        public static readonly IRefIdProperty ClockInIdProperty = P<ClockInDetail>.RegisterRefId(e => e.ClockInId, ReferenceType.Parent);

        /// <summary>
        /// 员工出勤Id
        /// </summary>
        public double ClockInId
        {
            get { return (double)GetRefId(ClockInIdProperty); }
            set { SetRefId(ClockInIdProperty, value); }
        }

        /// <summary>
        /// 员工出勤
        /// </summary>
        public static readonly RefEntityProperty<EmployeeClockIn> ClockInProperty = P<ClockInDetail>.RegisterRef(e => e.ClockIn, ClockInIdProperty);

        /// <summary>
        /// 员工出勤
        /// </summary>
        public EmployeeClockIn ClockIn
        {
            get { return GetRefEntity(ClockInProperty); }
            set { SetRefEntity(ClockInProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 打卡记录 实体配置
    /// </summary>
    internal class ClockInDetailConfig : EntityConfig<ClockInDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMP_CLOCK_IN_DTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}