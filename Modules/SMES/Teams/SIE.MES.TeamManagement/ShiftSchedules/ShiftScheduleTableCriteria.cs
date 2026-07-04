using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 班组排班表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("排班表查询实体")]
    public class ShiftScheduleTableCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftScheduleTableCriteria()
        {
            ScheduleDate = new DateRange();
            ScheduleDate.DateTimePart = DateTimePart.Date;
            ScheduleDate.DateRangeType = DateRangeType.Month;
            ScheduleDate.AllowDateRangeType = DateRangeType.Custom | DateRangeType.LastMonth | DateRangeType.Month | DateRangeType.Week;
        }

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty =
            P<ShiftScheduleTableCriteria>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double?)this.GetRefNullableId(WorkGroupIdProperty); }
            set { this.SetRefNullableId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<SIE.Resources.Employees.WorkGroup> WorkGroupProperty =
            P<ShiftScheduleTableCriteria>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public SIE.Resources.Employees.WorkGroup WorkGroup
        {
            get { return this.GetRefEntity(WorkGroupProperty); }
            set { this.SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<ShiftScheduleTableCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<ShiftScheduleTableCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<ShiftScheduleTableCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<ShiftScheduleTableCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 开始日期 ScheduleDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateRange> ScheduleDateProperty = P<ShiftScheduleTableCriteria>.Register(e => e.ScheduleDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateRange ScheduleDate
        {
            get { return this.GetProperty(ScheduleDateProperty); }
            set { this.SetProperty(ScheduleDateProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>数据</returns>
        protected override EntityList Fetch()
        {
            return new EntityList<ShiftSchedule>();
        }
    }
}