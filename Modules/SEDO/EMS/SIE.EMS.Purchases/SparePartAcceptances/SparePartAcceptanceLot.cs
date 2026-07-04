using SIE.Common;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 批次明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次明细")]
    [DisplayMember(nameof(LotNo))]
    public partial class SparePartAcceptanceLot : DataEntity
    {
        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        [Required]
        public static readonly Property<string> LotNoProperty = P<SparePartAcceptanceLot>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        [MinValue(0)]
        public static readonly Property<int> QtyProperty = P<SparePartAcceptanceLot>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 合格数量 PassQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        [MinValue(0)]
        public static readonly Property<int> PassQtyProperty = P<SparePartAcceptanceLot>.Register(e => e.PassQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public int PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 UnqualifiedQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        [MinValue(0)]
        public static readonly Property<int> UnqualifiedQtyProperty = P<SparePartAcceptanceLot>.Register(e => e.UnqualifiedQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int UnqualifiedQty
        {
            get { return GetProperty(UnqualifiedQtyProperty); }
            set { SetProperty(UnqualifiedQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SparePartAcceptanceLot>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 验收结果 AcceptanceResult
        /// <summary>
        /// 验收结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> AcceptanceResultProperty = P<SparePartAcceptanceLot>.Register(e => e.AcceptanceResult);

        /// <summary>
        /// 验收结果
        /// </summary>
        public InspectionResult? AcceptanceResult
        {
            get { return GetProperty(AcceptanceResultProperty); }
            set { SetProperty(AcceptanceResultProperty, value); }
        }
        #endregion

        #region 备件验收明细 SparePartAcceptanceDetail
        /// <summary>
        /// 备件验收明细Id
        /// </summary>
        public static readonly IRefIdProperty AccepDtlIdProperty = P<SparePartAcceptanceLot>.RegisterRefId(e => e.AccepDtlId, ReferenceType.Parent);

        /// <summary>
        /// 备件验收明细Id
        /// </summary>
        public double AccepDtlId
        {
            get { return (double)GetRefId(AccepDtlIdProperty); }
            set { SetRefId(AccepDtlIdProperty, value); }
        }

        /// <summary>
        /// 备件验收明细
        /// </summary>
        public static readonly RefEntityProperty<SparePartAcceptanceDetail> AccepDtlProperty = P<SparePartAcceptanceLot>.RegisterRef(e => e.AccepDtl, AccepDtlIdProperty);

        /// <summary>
        /// 备件验收明细
        /// </summary>
        public SparePartAcceptanceDetail AccepDtl
        {
            get { return GetRefEntity(AccepDtlProperty); }
            set { SetRefEntity(AccepDtlProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartAcceptanceLot>.RegisterView(e => e.PurchaseOrderNo, p => p.AccepDtl.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
        }
        #endregion

        #region 采购单行号 PurchaseOrderItemLineNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<int?> PurchaseOrderItemLineNoProperty = P<SparePartAcceptanceLot>.RegisterView(e => e.PurchaseOrderItemLineNo, p => p.AccepDtl.PurchaseOrderItem.LineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public int? PurchaseOrderItemLineNo
        {
            get { return this.GetProperty(PurchaseOrderItemLineNoProperty); }
        }
        #endregion

        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<SparePartAcceptanceLot>.RegisterView(e => e.PurchaseObjectType, p => p.AccepDtl.PurchaseOrder.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType? PurchaseObjectType
        {
            get { return this.GetProperty(PurchaseObjectTypeProperty); }
        }
        #endregion

        #region 单价 Price
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal> PriceProperty = P<SparePartAcceptanceLot>.RegisterView(e => e.Price, p => p.AccepDtl.Price);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get { return this.GetProperty(PriceProperty); }
        }
        #endregion

        #region 接收仓库 WarehouseName
        /// <summary>
        /// 接收仓库
        /// </summary>
        [Label("接收仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<SparePartAcceptanceLot>.RegisterView(e => e.WarehouseName, p => p.AccepDtl.Warehouse.Name);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<SparePartAcceptanceLot>.RegisterView(e => e.ApprovalStatus, p => p.AccepDtl.SparePartAcceptance.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 批次明细 实体配置
    /// </summary>
    internal class SparePartAcceptanceLotConfig : EntityConfig<SparePartAcceptanceLot>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_ACPT_LOT").MapAllProperties();
            Meta.Property(SparePartAcceptanceLot.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}