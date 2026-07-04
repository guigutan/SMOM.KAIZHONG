using SIE.Common.Configs;
using SIE.Common.Employees;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockQueues.Configs;
using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockQueues
{
    /// <summary>
    /// 月台排队
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(DockQueueNumberConfig))]
    [ConditionQueryType(typeof(DockQueueCriteria))]
    [Label("月台排队")]
    public partial class DockQueue : DataEntity
    {
        #region 排队号 No
        /// <summary>
        /// 排队号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("排队号")]
        public static readonly Property<string> NoProperty = P<DockQueue>.Register(e => e.No);

        /// <summary>
        /// 排队号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 推迟次数 DelayNum
        /// <summary>
        /// 推迟次数
        /// </summary>
        [MinValue(0)]
        [Label("推迟次数")]
        public static readonly Property<int> DelayNumProperty = P<DockQueue>.Register(e => e.DelayNum);

        /// <summary>
        /// 推迟次数
        /// </summary>
        public int DelayNum
        {
            get { return GetProperty(DelayNumProperty); }
            set { SetProperty(DelayNumProperty, value); }
        }
        #endregion

        #region 分配时间 DistributionTime
        /// <summary>
        /// 分配时间
        /// </summary>
        [Label("分配时间")]
        public static readonly Property<DateTime?> DistributionTimeProperty = P<DockQueue>.Register(e => e.DistributionTime);

        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime? DistributionTime
        {
            get { return GetProperty(DistributionTimeProperty); }
            set { SetProperty(DistributionTimeProperty, value); }
        }
        #endregion

        #region 上次分配时间 LastDistriTime
        /// <summary>
        /// 上次分配时间
        /// </summary>
        [Label("上次分配时间")]
        public static readonly Property<DateTime?> LastDistriTimeProperty = P<DockQueue>.Register(e => e.LastDistriTime);

        /// <summary>
        /// 上次分配时间
        /// </summary>
        public DateTime? LastDistriTime
        {
            get { return GetProperty(LastDistriTimeProperty); }
            set { SetProperty(LastDistriTimeProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<DockQueue>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 公司名称 CompanyName
        /// <summary>
        /// 公司名称
        /// </summary>
        [Label("公司名称")]
        public static readonly Property<string> CompanyNameProperty = P<DockQueue>.Register(e => e.CompanyName);

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get { return GetProperty(CompanyNameProperty); }
            set { SetProperty(CompanyNameProperty, value); }
        }
        #endregion

        #region 车牌号 CarNum
        /// <summary>
        /// 车牌号
        /// </summary>
        [Label("车牌号")]
        public static readonly Property<string> CarNumProperty = P<DockQueue>.Register(e => e.CarNum);

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNum
        {
            get { return GetProperty(CarNumProperty); }
            set { SetProperty(CarNumProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<DockQueue>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return GetProperty(ContactsProperty); }
            set { SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 联系电话 ContactNum
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactNumProperty = P<DockQueue>.Register(e => e.ContactNum);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNum
        {
            get { return GetProperty(ContactNumProperty); }
            set { SetProperty(ContactNumProperty, value); }
        }
        #endregion

        #region 身份证号 IDNumber
        /// <summary>
        /// 身份证号
        /// </summary>
        [Label("身份证号")]
        public static readonly Property<string> IDNumberProperty = P<DockQueue>.Register(e => e.IDNumber);

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber
        {
            get { return GetProperty(IDNumberProperty); }
            set { SetProperty(IDNumberProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<DockQueue>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 签到时间 CheckInTime
        /// <summary>
        /// 签到时间
        /// </summary>
        [Label("签到时间")]
        public static readonly Property<DateTime?> CheckInTimeProperty = P<DockQueue>.Register(e => e.CheckInTime);

        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime? CheckInTime
        {
            get { return GetProperty(CheckInTimeProperty); }
            set { SetProperty(CheckInTimeProperty, value); }
        }
        #endregion

        #region 签出时间 CheckOutTime
        /// <summary>
        /// 签出时间
        /// </summary>
        [Label("签出时间")]
        public static readonly Property<DateTime?> CheckOutTimeProperty = P<DockQueue>.Register(e => e.CheckOutTime);

        /// <summary>
        /// 签出时间
        /// </summary>
        public DateTime? CheckOutTime
        {
            get { return GetProperty(CheckOutTimeProperty); }
            set { SetProperty(CheckOutTimeProperty, value); }
        }
        #endregion

        #region 作业时间(Min) JobTime
        /// <summary>
        /// 作业时间(Min)
        /// </summary>
        [Label("作业时间(Min)")]
        public static readonly Property<decimal?> JobTimeProperty = P<DockQueue>.Register(e => e.JobTime);

        /// <summary>
        /// 作业时间(Min)
        /// </summary>
        public decimal? JobTime
        {
            get { return GetProperty(JobTimeProperty); }
            set { SetProperty(JobTimeProperty, value); }
        }
        #endregion

        #region 微信ID WeChatID
        /// <summary>
        /// 微信ID
        /// </summary>
        [Label("微信ID")]
        public static readonly Property<string> WeChatIDProperty = P<DockQueue>.Register(e => e.WeChatID);

        /// <summary>
        /// 微信ID
        /// </summary>
        public string WeChatID
        {
            get { return GetProperty(WeChatIDProperty); }
            set { SetProperty(WeChatIDProperty, value); }
        }
        #endregion

        #region 取号方式 TakeNoWay
        /// <summary>
        /// 取号方式
        /// </summary>
        [Label("取号方式")]
        public static readonly Property<TakeNoWay> TakeNoWayProperty = P<DockQueue>.Register(e => e.TakeNoWay);

        /// <summary>
        /// 取号方式
        /// </summary>
        public TakeNoWay TakeNoWay
        {
            get { return GetProperty(TakeNoWayProperty); }
            set { SetProperty(TakeNoWayProperty, value); }
        }
        #endregion

        #region 优先级 QueuePriority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<QueuePriority> QueuePriorityProperty = P<DockQueue>.Register(e => e.QueuePriority);

        /// <summary>
        /// 优先级
        /// </summary>
        public QueuePriority QueuePriority
        {
            get { return GetProperty(QueuePriorityProperty); }
            set { SetProperty(QueuePriorityProperty, value); }
        }
        #endregion

        #region 状态 QueueState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<QueueState> QueueStateProperty = P<DockQueue>.Register(e => e.QueueState);

        /// <summary>
        /// 状态
        /// </summary>
        public QueueState QueueState
        {
            get { return GetProperty(QueueStateProperty); }
            set { SetProperty(QueueStateProperty, value); }
        }
        #endregion

        #region 排队类型 AppointType
        /// <summary>
        /// 排队类型
        /// </summary>
        [Label("排队类型")]
        public static readonly Property<AppointType> AppointTypeProperty = P<DockQueue>.Register(e => e.AppointType);

        /// <summary>
        /// 排队类型
        /// </summary>
        public AppointType AppointType
        {
            get { return GetProperty(AppointTypeProperty); }
            set { SetProperty(AppointTypeProperty, value); }
        }
        #endregion

        #region 预约号 DockAppoint
        /// <summary>
        /// 预约号Id
        /// </summary>
        [Label("预约号")]
        public static readonly IRefIdProperty DockAppointIdProperty = P<DockQueue>.RegisterRefId(e => e.DockAppointId, ReferenceType.Normal);

        /// <summary>
        /// 预约号Id
        /// </summary>
        public double? DockAppointId
        {
            get { return (double?)GetRefNullableId(DockAppointIdProperty); }
            set { SetRefNullableId(DockAppointIdProperty, value); }
        }

        /// <summary>
        /// 预约号
        /// </summary>
        public static readonly RefEntityProperty<DockAppoint> DockAppointProperty = P<DockQueue>.RegisterRef(e => e.DockAppoint, DockAppointIdProperty);

        /// <summary>
        /// 预约号
        /// </summary>
        public DockAppoint DockAppoint
        {
            get { return GetRefEntity(DockAppointProperty); }
            set { SetRefEntity(DockAppointProperty, value); }
        }
        #endregion

        #region 排队地点 YardZone
        /// <summary>
        /// 排队地点Id
        /// </summary>
        [Label("排队地点")]
        public static readonly IRefIdProperty YardZoneIdProperty = P<DockQueue>.RegisterRefId(e => e.YardZoneId, ReferenceType.Normal);

        /// <summary>
        /// 排队地点Id
        /// </summary>
        public double YardZoneId
        {
            get { return (double)GetRefId(YardZoneIdProperty); }
            set { SetRefId(YardZoneIdProperty, value); }
        }

        /// <summary>
        /// 排队地点
        /// </summary>
        public static readonly RefEntityProperty<YardZone> YardZoneProperty = P<DockQueue>.RegisterRef(e => e.YardZone, YardZoneIdProperty);

        /// <summary>
        /// 排队地点
        /// </summary>
        public YardZone YardZone
        {
            get { return GetRefEntity(YardZoneProperty); }
            set { SetRefEntity(YardZoneProperty, value); }
        }
        #endregion

        #region 分配月台编码 AssignDock
        /// <summary>
        /// 分配月台编码Id
        /// </summary>
        [Label("分配月台编码")]
        public static readonly IRefIdProperty AssignDockIdProperty = P<DockQueue>.RegisterRefId(e => e.AssignDockId, ReferenceType.Normal);

        /// <summary>
        /// 分配月台编码Id
        /// </summary>
        public double? AssignDockId
        {
            get { return (double?)GetRefNullableId(AssignDockIdProperty); }
            set { SetRefNullableId(AssignDockIdProperty, value); }
        }

        /// <summary>
        /// 分配月台编码
        /// </summary>
        public static readonly RefEntityProperty<DockMaintain> AssignDockProperty = P<DockQueue>.RegisterRef(e => e.AssignDock, AssignDockIdProperty);

        /// <summary>
        /// 分配月台编码
        /// </summary>
        public DockMaintain AssignDock
        {
            get { return GetRefEntity(AssignDockProperty); }
            set { SetRefEntity(AssignDockProperty, value); }
        }
        #endregion

        #region 取消人 CancelEmployee
        /// <summary>
        /// 取消人Id
        /// </summary>
        [Label("取消人")]
        public static readonly IRefIdProperty CancelEmployeeIdProperty = P<DockQueue>.RegisterRefId(e => e.CancelEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 取消人Id
        /// </summary>
        public double? CancelEmployeeId
        {
            get { return (double?)GetRefNullableId(CancelEmployeeIdProperty); }
            set { SetRefNullableId(CancelEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 取消人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CancelEmployeeProperty = P<DockQueue>.RegisterRef(e => e.CancelEmployee, CancelEmployeeIdProperty);

        /// <summary>
        /// 取消人
        /// </summary>
        public Employee CancelEmployee
        {
            get { return GetRefEntity(CancelEmployeeProperty); }
            set { SetRefEntity(CancelEmployeeProperty, value); }
        }
        #endregion

        #region 取消时间 CancelTime
        /// <summary>
        /// 取消时间
        /// </summary>
        [Label("取消时间")]
        public static readonly Property<DateTime?> CancelTimeProperty = P<DockQueue>.Register(e => e.CancelTime);

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelTime
        {
            get { return GetProperty(CancelTimeProperty); }
            set { SetProperty(CancelTimeProperty, value); }
        }
        #endregion

        #region 取消原因 CancelReason
        /// <summary>
        /// 取消原因
        /// </summary>
        [Label("取消原因")]
        public static readonly Property<string> CancelReasonProperty = P<DockQueue>.Register(e => e.CancelReason);

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason
        {
            get { return GetProperty(CancelReasonProperty); }
            set { SetProperty(CancelReasonProperty, value); }
        }
        #endregion

        #region 优先系数 PrioritySeq
        /// <summary>
        /// 优先系数
        /// </summary>
        [Label("优先系数")]
        public static readonly Property<int> PrioritySeqProperty = P<DockQueue>.Register(e => e.PrioritySeq);

        /// <summary>
        /// 优先系数
        /// </summary>
        public int PrioritySeq
        {
            get { return this.GetProperty(PrioritySeqProperty); }
            set { this.SetProperty(PrioritySeqProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 预约月台编码 AppointDockCode
        /// <summary>
        /// 预约月台编码
        /// </summary>
        [Label("预约月台编码")]
        public static readonly Property<string> AppointDockCodeProperty = P<DockQueue>.RegisterView(e => e.AppointDockCode, p => p.DockAppoint.DockMaintain.Code);

        /// <summary>
        /// 预约月台编码
        /// </summary>
        public string AppointDockCode
        {
            get { return this.GetProperty(AppointDockCodeProperty); }
        }
        #endregion

        #region 预约月台名称 AppointDockName
        /// <summary>
        /// 预约月台名称
        /// </summary>
        [Label("预约月台名称")]
        public static readonly Property<string> AppointDockNameProperty = P<DockQueue>.RegisterView(e => e.AppointDockName, p => p.DockAppoint.DockMaintain.Name);

        /// <summary>
        /// 预约月台名称
        /// </summary>
        public string AppointDockName
        {
            get { return this.GetProperty(AppointDockNameProperty); }
        }
        #endregion

        #region 预约开始时间 AppointStartDate
        /// <summary>
        /// 预约开始时间
        /// </summary>
        [Label("预约开始时间")]
        public static readonly Property<DateTime> AppointStartDateProperty = P<DockQueue>.RegisterView(e => e.AppointStartDate, p => p.DockAppoint.AppointStartDate);

        /// <summary>
        /// 预约开始时间
        /// </summary>
        public DateTime AppointStartDate
        {
            get { return this.GetProperty(AppointStartDateProperty); }
        }
        #endregion

        #region 预约结束时间 AppointEndDate
        /// <summary>
        /// 预约结束时间
        /// </summary>
        [Label("预约结束时间")]
        public static readonly Property<DateTime> AppointEndDateProperty = P<DockQueue>.RegisterView(e => e.AppointEndDate, p => p.DockAppoint.AppointEndDate);

        /// <summary>
        /// 预约结束时间
        /// </summary>
        public DateTime AppointEndDate
        {
            get { return this.GetProperty(AppointEndDateProperty); }
        }
        #endregion

        #region 分配月台名称 AssignDockName
        /// <summary>
        /// 分配月台名称
        /// </summary>
        [Label("分配月台名称")]
        public static readonly Property<string> AssignDockNameProperty = P<DockQueue>.RegisterView(e => e.AssignDockName, p => p.AssignDock.Name);

        /// <summary>
        /// 分配月台名称
        /// </summary>
        public string AssignDockName
        {
            get { return this.GetProperty(AssignDockNameProperty); }
        }
        #endregion

        #region 园片区编码 YardZoneCode
        /// <summary>
        /// 园片区编码
        /// </summary>
        [Label("园片区编码")]
        public static readonly Property<string> YardZoneCodeProperty = P<DockQueue>.RegisterView(e => e.YardZoneCode, p => p.YardZone.Code);

        /// <summary>
        /// 园片区编码
        /// </summary>
        public string YardZoneCode
        {
            get { return this.GetProperty(YardZoneCodeProperty); }
        }
        #endregion

        #region 排队地点名称 YardZoneName
        /// <summary>
        /// 排队地点名称
        /// </summary>
        [Label("排队地点名称")]
        public static readonly Property<string> YardZoneNameProperty = P<DockQueue>.RegisterView(e => e.YardZoneName, p => p.YardZone.Name);

        /// <summary>
        /// 排队地点名称
        /// </summary>
        public string YardZoneName
        {
            get { return this.GetProperty(YardZoneNameProperty); }
        }
        #endregion

        #region 预约号编码
        /// <summary>
        /// 预约号编码
        /// </summary>
        [Label("预约号编码")]
        public static readonly Property<string> DockAppointNoProperty = P<DockQueue>.RegisterView(e => e.DockAppointNo, p => p.DockAppoint.No);

        /// <summary>
        /// 预约号编码
        /// </summary>
        public string DockAppointNo
        {
            get { return this.GetProperty(DockAppointNoProperty); }
        }
        #endregion

        #region 取消人名称 CancelByName
        /// <summary>
        /// 取消人名称
        /// </summary>
        [Label("取消人")]
        public static readonly Property<string> CancelByNameProperty = P<DockQueue>.RegisterView(e => e.CancelByName, p => p.CancelEmployee.Name);

        /// <summary>
        /// 取消人名称
        /// </summary>
        public string CancelByName
        {
            get { return this.GetProperty(CancelByNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 月台排队 实体配置
    /// </summary>
    internal class DockQueueConfig : EntityConfig<DockQueue>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DOCK_QUEUE").MapAllProperties();
            Meta.Property(DockQueue.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(DockQueue.PrioritySeqProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}