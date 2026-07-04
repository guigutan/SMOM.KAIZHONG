using SIE.Dock.DockAppoints.Service;
using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockAppoints
{
    /// <summary>
	/// 月台预约查询实体
	/// </summary>
	[QueryEntity, Serializable]
    [Label("月台预约查询实体")]
    public partial class DockAppointCriteria : Criteria
    {
        #region No
        /// <summary>
        /// 预约号
        /// </summary>
        [Label("预约号")]
        public static readonly Property<string> NoProperty = P<DockAppointCriteria>.Register(e => e.No);

        /// <summary>
        /// 预约号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<DockAppointCriteria>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 月台编码 DockMaintainCode
        /// <summary>
        /// 月台编码
        /// </summary>
        [Label("月台编码")]
        public static readonly Property<string> DockMaintainCodeProperty = P<DockAppointCriteria>.Register(e => e.DockMaintainCode);

        /// <summary>
        /// 月台编码
        /// </summary>
        public string DockMaintainCode
        {
            get { return GetProperty(DockMaintainCodeProperty); }
            set { SetProperty(DockMaintainCodeProperty, value); }
        }
        #endregion

        #region 月台名称 DockMaintainName
        /// <summary>
        /// 月台名称
        /// </summary>
        [Label("月台名称")]
        public static readonly Property<string> DockMaintainNameProperty = P<DockAppointCriteria>.Register(e => e.DockMaintainName);

        /// <summary>
        /// 月台名称
        /// </summary>
        public string DockMaintainName
        {
            get { return GetProperty(DockMaintainNameProperty); }
            set { SetProperty(DockMaintainNameProperty, value); }
        }
        #endregion

        #region 预约地点 YardZone
        /// <summary>
        /// 预约地点Id
        /// </summary>
        [Label("预约地点")]
        public static readonly IRefIdProperty YardZoneIdProperty =
            P<DockAppointCriteria>.RegisterRefId(e => e.YardZoneId, ReferenceType.Normal);

        /// <summary>
        /// 预约地点Id
        /// </summary>
        public double? YardZoneId
        {
            get { return (double?)GetRefNullableId(YardZoneIdProperty); }
            set { SetRefNullableId(YardZoneIdProperty, value); }
        }

        /// <summary>
        /// 预约地点
        /// </summary>
        public static readonly RefEntityProperty<YardZone> YardZoneProperty =
            P<DockAppointCriteria>.RegisterRef(e => e.YardZone, YardZoneIdProperty);

        /// <summary>
        /// 预约地点
        /// </summary>
        public YardZone YardZone
        {
            get { return this.GetRefEntity(YardZoneProperty); }
            set { this.SetRefEntity(YardZoneProperty, value); }
        }
        #endregion

        #region CarNum
        /// <summary>
        /// 车牌号
        /// </summary>
        [Label("车牌号")]
        public static readonly Property<string> CarNumProperty = P<DockAppointCriteria>.Register(e => e.CarNum);

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNum
        {
            get { return GetProperty(CarNumProperty); }
            set { SetProperty(CarNumProperty, value); }
        }
        #endregion

        #region Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<DockAppointCriteria>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return GetProperty(ContactsProperty); }
            set { SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 预约类型 AppointType
        /// <summary>
        /// 预约类型
        /// </summary>
        [Label("预约类型")]
        public static readonly Property<AppointType?> AppointTypeProperty = P<DockAppointCriteria>.Register(e => e.AppointType);

        /// <summary>
        /// 预约类型
        /// </summary>
        public AppointType? AppointType
        {
            get { return GetProperty(AppointTypeProperty); }
            set { SetProperty(AppointTypeProperty, value); }
        }
        #endregion

        #region 预约时间 AppointDate
        /// <summary>
        /// 预约时间
        /// </summary>
        [Label("预约时间")]
        public static readonly Property<DateRange> AppointDateProperty = P<DockAppointCriteria>.Register(e => e.AppointDate);

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateRange AppointDate
        {
            get { return GetProperty(AppointDateProperty); }
            set { SetProperty(AppointDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DockAppointService>().GetDockAppoints(this);
        }
    }
}
