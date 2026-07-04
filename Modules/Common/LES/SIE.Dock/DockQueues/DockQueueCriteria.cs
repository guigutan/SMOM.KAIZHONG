using SIE.Dock.DockAppoints;
using SIE.Dock.DockMaintains;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.DockQueues.Service;
using SIE.Dock.YardZones;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockQueues
{
    /// <summary>
	/// 月台排队查询实体
	/// </summary>
	[QueryEntity, Serializable]
    [Label("月台排队查询实体")]
    public partial class DockQueueCriteria : Criteria
    {
        #region 排队号 No
        /// <summary>
        /// 排队号
        /// </summary>
        [Label("排队号")]
        public static readonly Property<string> NoProperty = P<DockQueueCriteria>.Register(e => e.No);

        /// <summary>
        /// 排队号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 状态 QueueState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<QueueState?> QueueStateProperty = P<DockQueueCriteria>.Register(e => e.QueueState);

        /// <summary>
        /// 状态
        /// </summary>
        public QueueState? QueueState
        {
            get { return this.GetProperty(QueueStateProperty); }
            set { this.SetProperty(QueueStateProperty, value); }
        }
        #endregion

        #region 排队类型 AppointType
        /// <summary>
        /// 排队类型
        /// </summary>
        [Label("排队类型")]
        public static readonly Property<AppointType?> AppointTypeProperty = P<DockQueueCriteria>.Register(e => e.AppointType);

        /// <summary>
        /// 排队类型
        /// </summary>
        public AppointType? AppointType
        {
            get { return this.GetProperty(AppointTypeProperty); }
            set { this.SetProperty(AppointTypeProperty, value); }
        }
        #endregion

        #region 排队地点 YardZone
        /// <summary>
        /// 排队地点Id
        /// </summary>
        [Label("排队地点")]
        public static readonly IRefIdProperty YardZoneIdProperty =
            P<DockQueueCriteria>.RegisterRefId(e => e.YardZoneId, ReferenceType.Normal);

        /// <summary>
        /// 排队地点Id
        /// </summary>
        public double? YardZoneId
        {
            get { return (double?)GetRefNullableId(YardZoneIdProperty); }
            set { SetRefNullableId(YardZoneIdProperty, value); }
        }

        /// <summary>
        /// 排队地点
        /// </summary>
        public static readonly RefEntityProperty<YardZone> YardZoneProperty =
            P<DockQueueCriteria>.RegisterRef(e => e.YardZone, YardZoneIdProperty);

        /// <summary>
        /// 排队地点
        /// </summary>
        public YardZone YardZone
        {
            get { return this.GetRefEntity(YardZoneProperty); }
            set { this.SetRefEntity(YardZoneProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<DockQueueCriteria>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return this.GetProperty(BillNoProperty); }
            set { this.SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 预约号 DockAppointNo
        /// <summary>
        /// 预约号
        /// </summary>
        [Label("预约号")]
        public static readonly Property<string> DockAppointNoProperty = P<DockQueueCriteria>.Register(e => e.DockAppointNo);

        /// <summary>
        /// 预约号
        /// </summary>
        public string DockAppointNo
        {
            get { return this.GetProperty(DockAppointNoProperty); }
            set { this.SetProperty(DockAppointNoProperty, value); }
        }
        #endregion

        #region 分配月台编码 DockMaintainCode
        /// <summary>
        /// 分配月台编码
        /// </summary>
        [Label("分配月台编码")]
        public static readonly Property<string> DockMaintainCodeProperty = P<DockQueueCriteria>.Register(e => e.DockMaintainCode);

        /// <summary>
        /// 分配月台编码
        /// </summary>
        public string DockMaintainCode
        {
            get { return this.GetProperty(DockMaintainCodeProperty); }
            set { this.SetProperty(DockMaintainCodeProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<DockQueueCriteria>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return this.GetProperty(ContactsProperty); }
            set { this.SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 车牌号 CarNum
        /// <summary>
        /// 车牌号
        /// </summary>
        [Label("车牌号")]
        public static readonly Property<string> CarNumProperty = P<DockQueueCriteria>.Register(e => e.CarNum);

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNum
        {
            get { return this.GetProperty(CarNumProperty); }
            set { this.SetProperty(CarNumProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<DockQueueCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DockQueueService>().GetDockQueues(this);
        }
    }
}
