using SIE.Domain;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.EBS.InfDataEntitys.Download
{
    /// <summary>
    /// 采购订单发运行中间表
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("采购订单发运行中间表")]
    public partial class PurchaseOrderLocationsInf : DownloadBaseEntity
    {
        #region 发运号 LocNo
        /// <summary>
        /// 发运号
        /// </summary>
        [Label("发运号")]
        public static readonly Property<string> LocNoProperty = P<PurchaseOrderLocationsInf>.Register(e => e.LocNo);

        /// <summary>
        /// 发运号
        /// </summary>
        public string LocNo
        {
            get { return GetProperty(LocNoProperty); }
            set { SetProperty(LocNoProperty, value); }
        }
        #endregion

        #region 采购订单ERPID PoErpId
        /// <summary>
        /// 采购订单ERPID
        /// </summary>
        [Label("采购订单ERPID")]
        public static readonly Property<string> PoErpIdProperty = P<PurchaseOrderLocationsInf>.Register(e => e.PoErpId);

        /// <summary>
        /// 采购订单ERPID
        /// </summary>
        public string PoErpId
        {
            get { return GetProperty(PoErpIdProperty); }
            set { SetProperty(PoErpIdProperty, value); }
        }
        #endregion

        #region 采购订单行ERPID PoLineErpId
        /// <summary>
        /// 采购订单行ERPID
        /// </summary>
        [Label("采购订单行ERPID")]
        public static readonly Property<string> PoLineErpIdProperty = P<PurchaseOrderLocationsInf>.Register(e => e.PoLineErpId);

        /// <summary>
        /// 采购订单行ERPID
        /// </summary>
        public string PoLineErpId
        {
            get { return GetProperty(PoLineErpIdProperty); }
            set { SetProperty(PoLineErpIdProperty, value); }
        }
        #endregion

        #region 采购订单号 PoNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PoNoProperty = P<PurchaseOrderLocationsInf>.Register(e => e.PoNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PoNo
        {
            get { return GetProperty(PoNoProperty); }
            set { SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 采购订单行号 PoLineNo
        /// <summary>
        /// 采购订单行号
        /// </summary>
        [Label("采购订单行号")]
        public static readonly Property<string> PoLineNoProperty = P<PurchaseOrderLocationsInf>.Register(e => e.PoLineNo);

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PoLineNo
        {
            get { return GetProperty(PoLineNoProperty); }
            set { SetProperty(PoLineNoProperty, value); }
        }
        #endregion

        #region  PoReleaseId
        /// <summary>
        /// 
        /// </summary>
        [Label("")]
        public static readonly Property<int> PoReleaseIdProperty = P<PurchaseOrderLocationsInf>.Register(e => e.PoReleaseId);

        /// <summary>
        /// 
        /// </summary>
        public int PoReleaseId
        {
            get { return GetProperty(PoReleaseIdProperty); }
            set { SetProperty(PoReleaseIdProperty, value); }
        }
        #endregion

        #region 数量 Quantity
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal?> QuantityProperty = P<PurchaseOrderLocationsInf>.Register(e => e.Quantity);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Quantity
        {
            get { return GetProperty(QuantityProperty); }
            set { SetProperty(QuantityProperty, value); }
        }
        #endregion

        #region 需求日期 NeedByDate
        /// <summary>
        /// 需求日期
        /// </summary>
        [Label("需求日期")]
        public static readonly Property<DateTime?> NeedByDateProperty = P<PurchaseOrderLocationsInf>.Register(e => e.NeedByDate);

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? NeedByDate
        {
            get { return GetProperty(NeedByDateProperty); }
            set { SetProperty(NeedByDateProperty, value); }
        }
        #endregion

        #region 承诺日期 PromisedDate
        /// <summary>
        /// 承诺日期
        /// </summary>
        [Label("承诺日期")]
        public static readonly Property<DateTime?> PromisedDateProperty = P<PurchaseOrderLocationsInf>.Register(e => e.PromisedDate);

        /// <summary>
        /// 承诺日期
        /// </summary>
        public DateTime? PromisedDate
        {
            get { return GetProperty(PromisedDateProperty); }
            set { SetProperty(PromisedDateProperty, value); }
        }
        #endregion

        #region 接收数量 QuantityReceived
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<decimal> QuantityReceivedProperty = P<PurchaseOrderLocationsInf>.Register(e => e.QuantityReceived);

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal QuantityReceived
        {
            get { return GetProperty(QuantityReceivedProperty); }
            set { SetProperty(QuantityReceivedProperty, value); }
        }
        #endregion

        #region 接受数量 QuantityAccepted
        /// <summary>
        /// 接受数量
        /// </summary>
        [Label("接受数量")]
        public static readonly Property<decimal> QuantityAcceptedProperty = P<PurchaseOrderLocationsInf>.Register(e => e.QuantityAccepted);

        /// <summary>
        /// 接受数量
        /// </summary>
        public decimal QuantityAccepted
        {
            get { return GetProperty(QuantityAcceptedProperty); }
            set { SetProperty(QuantityAcceptedProperty, value); }
        }
        #endregion

        #region 拒绝数量 QuantityRejected
        /// <summary>
        /// 拒绝数量
        /// </summary>
        [Label("拒绝数量")]
        public static readonly Property<decimal> QuantityRejectedProperty = P<PurchaseOrderLocationsInf>.Register(e => e.QuantityRejected);

        /// <summary>
        /// 拒绝数量
        /// </summary>
        public decimal QuantityRejected
        {
            get { return GetProperty(QuantityRejectedProperty); }
            set { SetProperty(QuantityRejectedProperty, value); }
        }
        #endregion

        #region 开票数量 QuantityBilled
        /// <summary>
        /// 开票数量
        /// </summary>
        [Label("开票数量")]
        public static readonly Property<decimal> QuantityBilledProperty = P<PurchaseOrderLocationsInf>.Register(e => e.QuantityBilled);

        /// <summary>
        /// 开票数量
        /// </summary>
        public decimal QuantityBilled
        {
            get { return GetProperty(QuantityBilledProperty); }
            set { SetProperty(QuantityBilledProperty, value); }
        }
        #endregion

        #region 取消数量 QuantityCancelled
        /// <summary>
        /// 取消数量
        /// </summary>
        [Label("取消数量")]
        public static readonly Property<decimal> QuantityCancelledProperty = P<PurchaseOrderLocationsInf>.Register(e => e.QuantityCancelled);

        /// <summary>
        /// 取消数量
        /// </summary>
        public decimal QuantityCancelled
        {
            get { return GetProperty(QuantityCancelledProperty); }
            set { SetProperty(QuantityCancelledProperty, value); }
        }
        #endregion

        #region 发运数量 QuantityShipped
        /// <summary>
        /// 发运数量
        /// </summary>
        [Label("发运数量")]
        public static readonly Property<decimal?> QuantityShippedProperty = P<PurchaseOrderLocationsInf>.Register(e => e.QuantityShipped);

        /// <summary>
        /// 发运数量
        /// </summary>
        public decimal? QuantityShipped
        {
            get { return GetProperty(QuantityShippedProperty); }
            set { SetProperty(QuantityShippedProperty, value); }
        }
        #endregion

        #region 前导日期 LeadTime
        /// <summary>
        /// 前导日期
        /// </summary>
        [Label("前导日期")]
        public static readonly Property<DateTime?> LeadTimeProperty = P<PurchaseOrderLocationsInf>.Register(e => e.LeadTime);

        /// <summary>
        /// 前导日期
        /// </summary>
        public DateTime? LeadTime
        {
            get { return GetProperty(LeadTimeProperty); }
            set { SetProperty(LeadTimeProperty, value); }
        }
        #endregion

        #region 前导时间单位 LeadTimeUnit
        /// <summary>
        /// 前导时间单位
        /// </summary>
        [Label("前导时间单位")]
        public static readonly Property<string> LeadTimeUnitProperty = P<PurchaseOrderLocationsInf>.Register(e => e.LeadTimeUnit);

        /// <summary>
        /// 前导时间单位
        /// </summary>
        public string LeadTimeUnit
        {
            get { return GetProperty(LeadTimeUnitProperty); }
            set { SetProperty(LeadTimeUnitProperty, value); }
        }
        #endregion

        #region 开始日期 StartDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateTime?> StartDateProperty = P<PurchaseOrderLocationsInf>.Register(e => e.StartDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate
        {
            get { return GetProperty(StartDateProperty); }
            set { SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 结束日期 EndDate
        /// <summary>
        /// 结束日期
        /// </summary>
        [Label("结束日期")]
        public static readonly Property<DateTime?> EndDateProperty = P<PurchaseOrderLocationsInf>.Register(e => e.EndDate);

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 关闭标记 ClosedFlag
        /// <summary>
        /// 关闭标记
        /// </summary>
        [Label("关闭标记")]
        public static readonly Property<string> ClosedFlagProperty = P<PurchaseOrderLocationsInf>.Register(e => e.ClosedFlag);

        /// <summary>
        /// 关闭标记
        /// </summary>
        public string ClosedFlag
        {
            get { return GetProperty(ClosedFlagProperty); }
            set { SetProperty(ClosedFlagProperty, value); }
        }
        #endregion

        #region 关闭编码 ClosedCode
        /// <summary>
        /// 关闭编码
        /// </summary>
        [Label("关闭编码")]
        public static readonly Property<string> ClosedCodeProperty = P<PurchaseOrderLocationsInf>.Register(e => e.ClosedCode);

        /// <summary>
        /// 关闭编码
        /// </summary>
        public string ClosedCode
        {
            get { return GetProperty(ClosedCodeProperty); }
            set { SetProperty(ClosedCodeProperty, value); }
        }
        #endregion

        #region 关闭原因 ClosedReason
        /// <summary>
        /// 关闭原因
        /// </summary>
        [Label("关闭原因")]
        public static readonly Property<string> ClosedReasonProperty = P<PurchaseOrderLocationsInf>.Register(e => e.ClosedReason);

        /// <summary>
        /// 关闭原因
        /// </summary>
        public string ClosedReason
        {
            get { return GetProperty(ClosedReasonProperty); }
            set { SetProperty(ClosedReasonProperty, value); }
        }
        #endregion

        #region 关闭日期 ClosedDate
        /// <summary>
        /// 关闭日期
        /// </summary>
        [Label("关闭日期")]
        public static readonly Property<DateTime?> ClosedDateProperty = P<PurchaseOrderLocationsInf>.Register(e => e.ClosedDate);

        /// <summary>
        /// 关闭日期
        /// </summary>
        public DateTime? ClosedDate
        {
            get { return GetProperty(ClosedDateProperty); }
            set { SetProperty(ClosedDateProperty, value); }
        }
        #endregion

        #region 送货方式 ShipmentType
        /// <summary>
        /// 送货方式
        /// </summary>
        [Label("送货方式")]
        public static readonly Property<string> ShipmentTypeProperty = P<PurchaseOrderLocationsInf>.Register(e => e.ShipmentType);

        /// <summary>
        /// 送货方式
        /// </summary>
        public string ShipmentType
        {
            get { return GetProperty(ShipmentTypeProperty); }
            set { SetProperty(ShipmentTypeProperty, value); }
        }
        #endregion

        #region 允差 QtyRcvTolerance
        /// <summary>
        /// 允差
        /// </summary>
        [Label("允差")]
        public static readonly Property<decimal?> QtyRcvToleranceProperty = P<PurchaseOrderLocationsInf>.Register(e => e.QtyRcvTolerance);

        /// <summary>
        /// 允差
        /// </summary>
        public decimal? QtyRcvTolerance
        {
            get { return GetProperty(QtyRcvToleranceProperty); }
            set { SetProperty(QtyRcvToleranceProperty, value); }
        }
        #endregion

        #region 接收类型 ReceivingRoutingId
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<string> ReceivingRoutingIdProperty = P<PurchaseOrderLocationsInf>.Register(e => e.ReceivingRoutingId);

        /// <summary>
        /// 接收类型
        /// </summary>
        public string ReceivingRoutingId
        {
            get { return GetProperty(ReceivingRoutingIdProperty); }
            set { SetProperty(ReceivingRoutingIdProperty, value); }
        }
        #endregion

        #region 外协工单号 WoNo
        /// <summary>
        /// 外协工单号
        /// </summary>
        [Label("外协工单号")]
        public static readonly Property<string> WoNoProperty = P<PurchaseOrderLocationsInf>.Register(e => e.WoNo);

        /// <summary>
        /// 外协工单号
        /// </summary>
        public string WoNo
        {
            get { return GetProperty(WoNoProperty); }
            set { SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 更新人编码 UpdateByCode
        /// <summary>
        /// 更新人编码
        /// </summary>
        [Label("更新人编码")]
        public static readonly Property<string> UpdateByCodeProperty = P<PurchaseOrderLocationsInf>.Register(e => e.UpdateByCode);

        /// <summary>
        /// 更新人编码
        /// </summary>
        public string UpdateByCode
        {
            get { return GetProperty(UpdateByCodeProperty); }
            set { SetProperty(UpdateByCodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 采购订单发运行中间表 实体配置
    /// </summary>
    internal class PurchaseOrderLocationsInfConfig : EntityConfig<PurchaseOrderLocationsInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PO_LOC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}