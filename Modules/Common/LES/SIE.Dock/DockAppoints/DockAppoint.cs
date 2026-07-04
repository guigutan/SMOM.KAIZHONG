using SIE.Common.Configs;
using SIE.Dock.DockAppoints.Configs;
using SIE.Dock.DockMaintains;
using SIE.Dock.ViewModels;
using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Dock.DockAppoints
{
    /// <summary>
	/// 月台预约
	/// </summary>
	[RootEntity, Serializable]
    [EntityWithConfig(typeof(DockAppointNoConfig))]
    [ConditionQueryType(typeof(DockAppointCriteria))]
    [Label("月台预约")]
    [DisplayMember(nameof(DockAppoint.No))]
    public partial class DockAppoint : DataEntity
    {
        #region 预约号 No
        /// <summary>
        /// 预约号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("预约号")]
        public static readonly Property<string> NoProperty = P<DockAppoint>.Register(e => e.No);

        /// <summary>
        /// 预约号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<DockAppoint>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 预约日期 AppointDate
        /// <summary>
        /// 预约日期
        /// </summary>
        [Label("预约日期")]
        public static readonly Property<DateTime> AppointDateProperty = P<DockAppoint>.Register(e => e.AppointDate);

        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime AppointDate
        {
            get { return GetProperty(AppointDateProperty); }
            set { SetProperty(AppointDateProperty, value); }
        }
        #endregion

        #region 预约开始时间 AppointStartDate
        /// <summary>
        /// 预约开始时间
        /// </summary>
        [Label("预约开始时间")]
        public static readonly Property<DateTime> AppointStartDateProperty = P<DockAppoint>.Register(e => e.AppointStartDate);

        /// <summary>
        /// 预约开始时间
        /// </summary>
        public DateTime AppointStartDate
        {
            get { return GetProperty(AppointStartDateProperty); }
            set { SetProperty(AppointStartDateProperty, value); }
        }
        #endregion

        #region 预约结束时间 AppointEndDate
        /// <summary>
        /// 预约结束时间
        /// </summary>
        [Label("预约结束时间")]
        public static readonly Property<DateTime> AppointEndDateProperty = P<DockAppoint>.Register(e => e.AppointEndDate);

        /// <summary>
        /// 预约结束时间
        /// </summary>
        public DateTime AppointEndDate
        {
            get { return GetProperty(AppointEndDateProperty); }
            set { SetProperty(AppointEndDateProperty, value); }
        }
        #endregion

        #region 公司名称 CompanyName
        /// <summary>
        /// 公司名称
        /// </summary>
        [Label("公司名称")]
        public static readonly Property<string> CompanyNameProperty = P<DockAppoint>.Register(e => e.CompanyName);

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
        public static readonly Property<string> CarNumProperty = P<DockAppoint>.Register(e => e.CarNum);

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
        public static readonly Property<string> ContactsProperty = P<DockAppoint>.Register(e => e.Contacts);

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
        public static readonly Property<string> ContactNumProperty = P<DockAppoint>.Register(e => e.ContactNum);

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
        public static readonly Property<string> IDNumberProperty = P<DockAppoint>.Register(e => e.IDNumber);

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber
        {
            get { return GetProperty(IDNumberProperty); }
            set { SetProperty(IDNumberProperty, value); }
        }
        #endregion

        #region 是否取消预约 IsCancelAppoint
        /// <summary>
        /// 是否取消预约
        /// </summary>
        [Label("是否取消预约")]
        public static readonly Property<bool> IsCancelAppointProperty = P<DockAppoint>.Register(e => e.IsCancelAppoint);

        /// <summary>
        /// 是否取消预约
        /// </summary>
        public bool IsCancelAppoint
        {
            get { return GetProperty(IsCancelAppointProperty); }
            set { SetProperty(IsCancelAppointProperty, value); }
        }
        #endregion

        #region 取消预约时间 CancelAppointDate
        /// <summary>
        /// 取消预约时间
        /// </summary>
        [Label("取消预约时间")]
        public static readonly Property<DateTime?> CancelAppointDateProperty = P<DockAppoint>.Register(e => e.CancelAppointDate);

        /// <summary>
        /// 取消预约时间
        /// </summary>
        public DateTime? CancelAppointDate
        {
            get { return GetProperty(CancelAppointDateProperty); }
            set { SetProperty(CancelAppointDateProperty, value); }
        }
        #endregion

        #region 取消预约原因 CancelReason
        /// <summary>
        /// 取消预约原因
        /// </summary>
        [Label("取消预约原因")]
        public static readonly Property<string> CancelReasonProperty = P<DockAppoint>.Register(e => e.CancelReason);

        /// <summary>
        /// 取消预约原因
        /// </summary>
        public string CancelReason
        {
            get { return GetProperty(CancelReasonProperty); }
            set { SetProperty(CancelReasonProperty, value); }
        }
        #endregion

        #region 微信ID WeChatID
        /// <summary>
        /// 微信ID
        /// </summary>
        [Label("微信ID")]
        public static readonly Property<string> WeChatIDProperty = P<DockAppoint>.Register(e => e.WeChatID);

        /// <summary>
        /// 微信ID
        /// </summary>
        public string WeChatID
        {
            get { return GetProperty(WeChatIDProperty); }
            set { SetProperty(WeChatIDProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<DockAppoint>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 月台编码 DockMaintain
        /// <summary>
        /// 月台编码Id
        /// </summary>
        [Label("月台编码")]
        public static readonly IRefIdProperty DockMaintainIdProperty = P<DockAppoint>.RegisterRefId(e => e.DockMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 月台编码Id
        /// </summary>
        public double DockMaintainId
        {
            get { return (double)GetRefId(DockMaintainIdProperty); }
            set { SetRefId(DockMaintainIdProperty, value); }
        }

        /// <summary>
        /// 月台编码
        /// </summary>
        public static readonly RefEntityProperty<DockMaintain> DockMaintainProperty = P<DockAppoint>.RegisterRef(e => e.DockMaintain, DockMaintainIdProperty);

        /// <summary>
        /// 月台编码
        /// </summary>
        public DockMaintain DockMaintain
        {
            get { return GetRefEntity(DockMaintainProperty); }
            set { SetRefEntity(DockMaintainProperty, value); }
        }
        #endregion

        #region 取消预约人 CancelAppointBy
        /// <summary>
        /// 取消预约人Id
        /// </summary>
        [Label("取消预约人")]
        public static readonly IRefIdProperty CancelAppointByProperty = P<DockAppoint>.RegisterRefId(e => e.CancelAppointBy, ReferenceType.Normal);

        /// <summary>
        /// 取消预约人Id
        /// </summary>
        public double? CancelAppointBy
        {
            get { return (double?)GetRefNullableId(CancelAppointByProperty); }
            set { SetRefNullableId(CancelAppointByProperty, value); }
        }

        /// <summary>
        /// 取消预约人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CancelAppointProperty = P<DockAppoint>.RegisterRef(e => e.CancelAppoint, CancelAppointByProperty);

        /// <summary>
        /// 取消预约人
        /// </summary>
        public Employee CancelAppoint
        {
            get { return GetRefEntity(CancelAppointProperty); }
            set { SetRefEntity(CancelAppointProperty, value); }
        }
        #endregion

        #region 预约类型 AppointType
        /// <summary>
        /// 预约类型
        /// </summary>
        [Label("预约类型")]
        public static readonly Property<AppointType> AppointTypeProperty = P<DockAppoint>.Register(e => e.AppointType);

        /// <summary>
        /// 预约类型
        /// </summary>
        public AppointType AppointType
        {
            get { return GetProperty(AppointTypeProperty); }
            set { SetProperty(AppointTypeProperty, value); }
        }
        #endregion

        #region 预约地点 YardZone
        /// <summary>
        /// 预约地点Id
        /// </summary>
        [Label("预约地点")]
        public static readonly IRefIdProperty YardZoneIdProperty =
            P<DockAppoint>.RegisterRefId(e => e.YardZoneId, ReferenceType.Normal);

        /// <summary>
        /// 预约地点Id
        /// </summary>
        public double YardZoneId
        {
            get { return (double)this.GetRefId(YardZoneIdProperty); }
            set { this.SetRefId(YardZoneIdProperty, value); }
        }

        /// <summary>
        /// 预约地点
        /// </summary>
        public static readonly RefEntityProperty<YardZone> YardZoneProperty =
            P<DockAppoint>.RegisterRef(e => e.YardZone, YardZoneIdProperty);

        /// <summary>
        /// 预约地点
        /// </summary>
        public YardZone YardZone
        {
            get { return this.GetRefEntity(YardZoneProperty); }
            set { this.SetRefEntity(YardZoneProperty, value); }
        }
        #endregion

        #region 预计占用(H) UseHours
        /// <summary>
        /// 预计占用(H)
        /// </summary>
        [Label("预计占用(H)")]
        public static readonly Property<double> UseHoursProperty = P<DockAppoint>.Register(e => e.UseHours);

        /// <summary>
        /// 预计占用(H)
        /// </summary>
        public double UseHours
        {
            get { return this.GetProperty(UseHoursProperty); }
            set { this.SetProperty(UseHoursProperty, value); }
        }
        #endregion

        #region 预计占用显示 UseHoursDisplay
        /// <summary>
        /// 预计占用显示
        /// </summary>
        [Label("预计占用显示")]
        public static readonly Property<double> UseHoursDisplayProperty = P<DockAppoint>.Register(e => e.UseHoursDisplay);

        /// <summary>
        /// 预计占用显示
        /// </summary>
        public double UseHoursDisplay
        {
            get { return this.GetProperty(UseHoursDisplayProperty); }
            set { this.SetProperty(UseHoursDisplayProperty, value); }
        }
        #endregion

        #region 来源类型 DockSourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<DockSourceType> DockSourceTypeProperty = P<DockAppoint>.Register(e => e.DockSourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public DockSourceType DockSourceType
        {
            get { return this.GetProperty(DockSourceTypeProperty); }
            set { this.SetProperty(DockSourceTypeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 月台编码 DockMaintainCode
        /// <summary>
        /// 月台编码
        /// </summary>
        [Label("月台编码")]
        public static readonly Property<string> DockMaintainCodeProperty = P<DockAppoint>.RegisterView(e => e.DockMaintainCode, p => p.DockMaintain.Code);

        /// <summary>
        /// 月台编码
        /// </summary>
        public string DockMaintainCode
        {
            get { return this.GetProperty(DockMaintainCodeProperty); }
        }
        #endregion

        #region 月台名称 DockMaintainName
        /// <summary>
        /// 月台名称
        /// </summary>
        [Label("月台名称")]
        public static readonly Property<string> DockMaintainNameProperty = P<DockAppoint>.RegisterView(e => e.DockMaintainName, p => p.DockMaintain.Name);

        /// <summary>
        /// 月台名称
        /// </summary>
        public string DockMaintainName
        {
            get { return this.GetProperty(DockMaintainNameProperty); }
        }
        #endregion

        #region 预约地点名称 YardZoneName
        /// <summary>
        /// 预约地点名称
        /// </summary>
        [Label("预约地点名称")]
        public static readonly Property<string> YardZoneNameProperty = P<DockAppoint>.RegisterView(e => e.YardZoneName, p => p.YardZone.Name);

        /// <summary>
        /// 排队地点名称
        /// </summary>
        public string YardZoneName
        {
            get { return this.GetProperty(YardZoneNameProperty); }
        }
        #endregion

        #region 预约地点经度 Longitude
        /// <summary>
        /// 排队地点经度
        /// </summary>
        [Label("排队地点经度")]
        public static readonly Property<double> LongitudeProperty = P<DockAppoint>.RegisterView(e => e.Longitude, p => p.YardZone.Longitude);

        /// <summary>
        /// 排队地点经度
        /// </summary>
        public double Longitude
        {
            get { return this.GetProperty(LongitudeProperty); }
        }
        #endregion

        #region 排队地点纬度 Latitude
        /// <summary>
        /// 排队地点纬度
        /// </summary>
        [Label("排队地点纬度")]
        public static readonly Property<double> LatitudeProperty = P<DockAppoint>.RegisterView(e => e.Latitude, p => p.YardZone.Latitude);

        /// <summary>
        /// 排队地点纬度
        /// </summary>
        public double Latitude
        {
            get { return this.GetProperty(LatitudeProperty); }
        }
        #endregion

        #region 排队取号围栏距离(km) Distance
        /// <summary>
        /// 排队取号围栏距离(km)
        /// </summary>
        [Label("排队取号围栏距离")]
        public static readonly Property<double> DistanceProperty = P<DockAppoint>.RegisterView(e => e.Distance, p => p.YardZone.Distance);

        /// <summary>
        /// 排队取号围栏距离(km)
        /// </summary>
        public double Distance
        {
            get { return this.GetProperty(DistanceProperty); }
        }
        #endregion
        #endregion

        #region 不映射数据库
        #region 预约月台 AppointDock
        /// <summary>
        /// 预约月台
        /// </summary>
        [Label("预约月台*")]
        public static readonly Property<string> AppointDockProperty = P<DockAppoint>.Register(e => e.AppointDock);

        /// <summary>
        /// 预约月台
        /// </summary>
        public string AppointDock
        {
            get { return GetProperty(AppointDockProperty); }
            set { SetProperty(AppointDockProperty, value); }
        }
        #endregion
                       
        #endregion
    }

    /// <summary>
    /// 月台预约 实体配置
    /// </summary>
    internal class DockAppointConfig : EntityConfig<DockAppoint>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DOCK_APPOINT").MapAllProperties();
            Meta.Property(DockAppoint.RemarkProperty).ColumnMeta.HasLength(4000);            
            Meta.Property(DockAppoint.AppointDockProperty).DontMapColumn();                                 
            Meta.Property(DockAppoint.UseHoursDisplayProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}