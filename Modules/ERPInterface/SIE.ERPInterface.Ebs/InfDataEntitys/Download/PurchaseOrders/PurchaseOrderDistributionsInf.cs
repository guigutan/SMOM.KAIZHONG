using SIE.Domain;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.EBS.InfDataEntitys.Download
{
    /// <summary>
    /// 采购订单分配行中间表
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("采购订单分配行中间表")]
    public partial class PurchaseOrderDistributionsInf : DownloadBaseEntity
    {
        #region 采购订单ERPID PoErpId
        /// <summary>
        /// 采购订单ERPID
        /// </summary>
        [Label("采购订单ERPID")]
        public static readonly Property<string> PoErpIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.PoErpId);

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
        public static readonly Property<string> PoLineErpIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.PoLineErpId);

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
        public static readonly Property<string> PoNoProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.PoNo);

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
        public static readonly Property<string> PoLineNoProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.PoLineNo);

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PoLineNo
        {
            get { return GetProperty(PoLineNoProperty); }
            set { SetProperty(PoLineNoProperty, value); }
        }
        #endregion

        #region 采购订单发运行ERPID LineLocationErpId
        /// <summary>
        /// 采购订单发运行ERPID
        /// </summary>
        [Label("采购订单发运行ERPID")]
        public static readonly Property<string> LineLocationErpIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.LineLocationErpId);

        /// <summary>
        /// 采购订单发运行ERPID
        /// </summary>
        public string LineLocationErpId
        {
            get { return GetProperty(LineLocationErpIdProperty); }
            set { SetProperty(LineLocationErpIdProperty, value); }
        }
        #endregion

        #region 采购订单发运行号 LineLocationNo
        /// <summary>
        /// 采购订单发运行号
        /// </summary>
        [Label("采购订单发运行号")]
        public static readonly Property<string> LineLocationNoProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.LineLocationNo);

        /// <summary>
        /// 采购订单发运行号
        /// </summary>
        public string LineLocationNo
        {
            get { return GetProperty(LineLocationNoProperty); }
            set { SetProperty(LineLocationNoProperty, value); }
        }
        #endregion

        #region 来源分配ID PoReleaseId
        /// <summary>
        /// 来源分配ID
        /// </summary>
        [Label("来源分配ID")]
        public static readonly Property<int> PoReleaseIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.PoReleaseId);

        /// <summary>
        /// 来源分配ID
        /// </summary>
        public int PoReleaseId
        {
            get { return GetProperty(PoReleaseIdProperty); }
            set { SetProperty(PoReleaseIdProperty, value); }
        }
        #endregion

        #region 采购申请单分配ID DeliverToLocationId
        /// <summary>
        /// 采购申请单分配ID
        /// </summary>
        [Label("采购申请单分配ID")]
        public static readonly Property<int?> DeliverToLocationIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.DeliverToLocationId);

        /// <summary>
        /// 采购申请单分配ID
        /// </summary>
        public int? DeliverToLocationId
        {
            get { return GetProperty(DeliverToLocationIdProperty); }
            set { SetProperty(DeliverToLocationIdProperty, value); }
        }
        #endregion

        #region 收货人 DeliverToPersonId
        /// <summary>
        /// 收货人
        /// </summary>
        [Label("收货人")]
        public static readonly Property<int?> DeliverToPersonIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.DeliverToPersonId);

        /// <summary>
        /// 收货人
        /// </summary>
        public int? DeliverToPersonId
        {
            get { return GetProperty(DeliverToPersonIdProperty); }
            set { SetProperty(DeliverToPersonIdProperty, value); }
        }
        #endregion

        #region 数量 QuantityOrdered
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QuantityOrderedProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.QuantityOrdered);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal QuantityOrdered
        {
            get { return GetProperty(QuantityOrderedProperty); }
            set { SetProperty(QuantityOrderedProperty, value); }
        }
        #endregion

        #region 已交货数量 QuantityDelivered
        /// <summary>
        /// 已交货数量
        /// </summary>
        [Label("已交货数量")]
        public static readonly Property<decimal> QuantityDeliveredProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.QuantityDelivered);

        /// <summary>
        /// 已交货数量
        /// </summary>
        public decimal QuantityDelivered
        {
            get { return GetProperty(QuantityDeliveredProperty); }
            set { SetProperty(QuantityDeliveredProperty, value); }
        }
        #endregion

        #region 开票数量 QuantityBilled
        /// <summary>
        /// 开票数量
        /// </summary>
        [Label("开票数量")]
        public static readonly Property<decimal> QuantityBilledProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.QuantityBilled);

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
        public static readonly Property<decimal> QuantityCancelledProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.QuantityCancelled);

        /// <summary>
        /// 取消数量
        /// </summary>
        public decimal QuantityCancelled
        {
            get { return GetProperty(QuantityCancelledProperty); }
            set { SetProperty(QuantityCancelledProperty, value); }
        }
        #endregion

        #region 帐套ID SetOfBooksId
        /// <summary>
        /// 帐套ID
        /// </summary>
        [Label("帐套ID")]
        public static readonly Property<string> SetOfBooksIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.SetOfBooksId);

        /// <summary>
        /// 帐套ID
        /// </summary>
        public string SetOfBooksId
        {
            get { return GetProperty(SetOfBooksIdProperty); }
            set { SetProperty(SetOfBooksIdProperty, value); }
        }
        #endregion

        #region 汇率 Rate
        /// <summary>
        /// 汇率
        /// </summary>
        [Label("汇率")]
        public static readonly Property<double?> RateProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.Rate);

        /// <summary>
        /// 汇率
        /// </summary>
        public double? Rate
        {
            get { return GetProperty(RateProperty); }
            set { SetProperty(RateProperty, value); }
        }
        #endregion

        #region 汇率日期 RateDate
        /// <summary>
        /// 汇率日期
        /// </summary>
        [Label("汇率日期")]
        public static readonly Property<DateTime?> RateDateProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.RateDate);

        /// <summary>
        /// 汇率日期
        /// </summary>
        public DateTime? RateDate
        {
            get { return GetProperty(RateDateProperty); }
            set { SetProperty(RateDateProperty, value); }
        }
        #endregion

        #region 负担标志 EncumberedFlag
        /// <summary>
        /// 负担标志
        /// </summary>
        [Label("负担标志")]
        public static readonly Property<string> EncumberedFlagProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.EncumberedFlag);

        /// <summary>
        /// 负担标志
        /// </summary>
        public string EncumberedFlag
        {
            get { return GetProperty(EncumberedFlagProperty); }
            set { SetProperty(EncumberedFlagProperty, value); }
        }
        #endregion

        #region 外协工单号 WoNo
        /// <summary>
        /// 外协工单号
        /// </summary>
        [Label("外协工单号")]
        public static readonly Property<string> WoNoProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.WoNo);

        /// <summary>
        /// 外协工单号
        /// </summary>
        public string WoNo
        {
            get { return GetProperty(WoNoProperty); }
            set { SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 外协工单ERPID WoErpId
        /// <summary>
        /// 外协工单ERPID
        /// </summary>
        [Label("外协工单ERPID")]
        public static readonly Property<string> WoErpIdProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.WoErpId);

        /// <summary>
        /// 外协工单ERPID
        /// </summary>
        public string WoErpId
        {
            get { return GetProperty(WoErpIdProperty); }
            set { SetProperty(WoErpIdProperty, value); }
        }
        #endregion

        #region 工序 WipOperationSeqNum
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> WipOperationSeqNumProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.WipOperationSeqNum);

        /// <summary>
        /// 工序
        /// </summary>
        public string WipOperationSeqNum
        {
            get { return GetProperty(WipOperationSeqNumProperty); }
            set { SetProperty(WipOperationSeqNumProperty, value); }
        }
        #endregion

        #region 工序序号 WipResourceSeqNum
        /// <summary>
        /// 工序序号
        /// </summary>
        [Label("工序序号")]
        public static readonly Property<string> WipResourceSeqNumProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.WipResourceSeqNum);

        /// <summary>
        /// 工序序号
        /// </summary>
        public string WipResourceSeqNum
        {
            get { return GetProperty(WipResourceSeqNumProperty); }
            set { SetProperty(WipResourceSeqNumProperty, value); }
        }
        #endregion

        #region 更新人编码 UpdateByCode
        /// <summary>
        /// 更新人编码
        /// </summary>
        [Label("更新人编码")]
        public static readonly Property<string> UpdateByCodeProperty = P<PurchaseOrderDistributionsInf>.Register(e => e.UpdateByCode);

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
    /// 采购订单分配行中间表 实体配置
    /// </summary>
    internal class PurchaseOrderDistributionsInfConfig : EntityConfig<PurchaseOrderDistributionsInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PO_DIS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}