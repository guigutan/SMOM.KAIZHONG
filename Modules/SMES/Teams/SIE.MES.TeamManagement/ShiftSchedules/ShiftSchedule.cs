using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TeamManagement.ShiftSchedules
{
    /// <summary>
    /// 班组排班
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ShiftScheduleTableCriteria))]
    [Label("排班表")]
    public partial class ShiftSchedule : DataEntity
    {
        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<ShiftSchedule>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return (double)this.GetRefId(WorkShopIdProperty); }
            set { this.SetRefId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<ShiftSchedule>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
            P<ShiftSchedule>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)this.GetRefId(WipResourceIdProperty); }
            set { this.SetRefId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<ShiftSchedule>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 排班日期 ScheduleDate
        /// <summary>
        /// 排班日期
        /// </summary>
        [Label("排班日期")]
        public static readonly Property<DateTime> ScheduleDateProperty = P<ShiftSchedule>.Register(e => e.ScheduleDate, new PropertyMetadata<DateTime>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ScheduleDate
        {
            get { return this.GetProperty(ScheduleDateProperty); }
            set { this.SetProperty(ScheduleDateProperty, value); }
        }
        #endregion

        #region 班制 ShiftType
        /// <summary>
        /// 班制Id
        /// </summary>
        [Label("班制")]
        public static readonly IRefIdProperty ShiftTypeIdProperty =
            P<ShiftSchedule>.RegisterRefId(e => e.ShiftTypeId, ReferenceType.Normal);

        /// <summary>
        /// 班制Id
        /// </summary>
        public double ShiftTypeId
        {
            get { return (double)this.GetRefId(ShiftTypeIdProperty); }
            set { this.SetRefId(ShiftTypeIdProperty, value); }
        }

        /// <summary>
        /// 班制
        /// </summary>
        public static readonly RefEntityProperty<ShiftType> ShiftTypeProperty =
            P<ShiftSchedule>.RegisterRef(e => e.ShiftType, ShiftTypeIdProperty);

        /// <summary>
        /// 班制
        /// </summary>
        public ShiftType ShiftType
        {
            get { return this.GetRefEntity(ShiftTypeProperty); }
            set { this.SetRefEntity(ShiftTypeProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty =
            P<ShiftSchedule>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)this.GetRefId(ShiftIdProperty); }
            set { this.SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty =
            P<ShiftSchedule>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return this.GetRefEntity(ShiftProperty); }
            set { this.SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty =
            P<ShiftSchedule>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double WorkGroupId
        {
            get { return (double)this.GetRefId(WorkGroupIdProperty); }
            set { this.SetRefId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<SIE.Resources.Employees.WorkGroup> WorkGroupProperty =
            P<ShiftSchedule>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public SIE.Resources.Employees.WorkGroup WorkGroup
        {
            get { return this.GetRefEntity(WorkGroupProperty); }
            set { this.SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 排班表实体配置类
    /// </summary>
    internal class ShiftScheduleEntityConfig : EntityConfig<ShiftSchedule>
    {
        /// <summary>
        /// 配置实体元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_SHIFT_SCHEDULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}