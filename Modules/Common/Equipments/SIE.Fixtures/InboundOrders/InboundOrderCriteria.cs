using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures.Models;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.InboundOrders
{

    /// <summary>
    /// 工治具查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class InboundOrderCriteria : Criteria
    {

        #region 治具入库单号 InboundOrderNo
        /// <summary>
        /// 治具入库单号
        /// </summary>
        [Label("工治具入库单号")]
        public static readonly Property<string> InboundOrderNoProperty = P<InboundOrderCriteria>.Register(e => e.InboundOrderNo);

        /// <summary>
        /// 工治具入库单号
        /// </summary>
        public string InboundOrderNo
        {
            get { return this.GetProperty(InboundOrderNoProperty); }
            set { this.SetProperty(InboundOrderNoProperty, value); }
        }
        #endregion


        #region 入库类型 InboundType
        /// <summary>
        /// 入库类型
        /// </summary>
        [Label("入库类型")]
        public static readonly Property<FixtureInboundType?> InboundTypeProperty = P<InboundOrderCriteria>.Register(e => e.InboundType);

        /// <summary>
        /// 入库类型
        /// </summary>
        public FixtureInboundType? InboundType
        {
            get { return GetProperty(InboundTypeProperty); }
            set { SetProperty(InboundTypeProperty, value); }
        }
        #endregion


        #region 工治具接收单号 ReceiptOrderNo
        /// <summary>
        /// 工治具接收单号
        /// </summary>
        [Label("接收单号")]
        public static readonly Property<string> ReceiptOrderNoProperty = P<InboundOrderCriteria>.Register(e => e.ReceiptOrderNo);

        /// <summary>
        /// 工治具接收单号
        /// </summary>
        public string ReceiptOrderNo
        {
            get { return GetProperty(ReceiptOrderNoProperty); }
            set { SetProperty(ReceiptOrderNoProperty, value); }
        }
        #endregion

        #region 验收单号 AcceptanceOrderNo
        /// <summary>
        /// 工治具验收单号
        /// </summary>
        [Label("验收单号")]
        public static readonly Property<string> AcceptanceOrderNoProperty = P<InboundOrderCriteria>.Register(e => e.AcceptanceOrderNo);

        /// <summary>
        /// 工治具验收单号
        /// </summary>
        public string AcceptanceOrderNo
        {
            get { return GetProperty(AcceptanceOrderNoProperty); }
            set { SetProperty(AcceptanceOrderNoProperty, value); }
        }
        #endregion



        #region 状态 InboundStatus
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InboundStatus?> InboundStatusProperty = P<InboundOrderCriteria>.Register(e => e.InboundStatus);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InboundStatus? InboundStatus
        {
            get { return GetProperty(InboundStatusProperty); }
            set { SetProperty(InboundStatusProperty, value); }
        }
        #endregion


        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<InboundOrderCriteria>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double? FixtureEncodeId
        {
            get { return (double?)GetRefNullableId(FixtureEncodeIdProperty); }
            set { SetRefNullableId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<InboundOrderCriteria>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Required]
        [Label("管理方式")]
        public static readonly Property<ManageMode?> ManageModeProperty = P<InboundOrderCriteria>.Register(e => e.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode? ManageMode
        {
            get { return GetProperty(ManageModeProperty); }
            set { SetProperty(ManageModeProperty, value); }
        }
        #endregion


        #region 入库时间 InboundTime
        /// <summary>
        /// 入库时间
        /// </summary>
        [Label("入库时间")]
        public static readonly Property<DateRange> InboundTimeProperty = P<InboundOrderCriteria>.Register(e => e.InboundTime);

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateRange InboundTime
        {
            get { return this.GetProperty(InboundTimeProperty); }
            set { this.SetProperty(InboundTimeProperty, value); }
        }
        #endregion


        #region 采购订单 PurchaseOrderNo
        /// <summary>
        /// 采购订单
        /// </summary>
        [Label("采购订单")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<InboundOrderCriteria>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购订单
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
            set { this.SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion



        /// <summary>
        /// 重写数据查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InboundOrderController>().GetInboundOrders(this);
        }
    }
}
