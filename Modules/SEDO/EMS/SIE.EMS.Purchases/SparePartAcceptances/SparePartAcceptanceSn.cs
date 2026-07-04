using SIE.Common;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 序列号明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("序列号明细")]
    [DisplayMember(nameof(Sn))]
    public partial class SparePartAcceptanceSn : DataEntity
    {
        #region 序列号编码 Sn
        /// <summary>
        /// 序列号编码
        /// </summary>
        [Label("序列号编码")]
        [Required]
        public static readonly Property<string> SnProperty = P<SparePartAcceptanceSn>.Register(e => e.Sn);

        /// <summary>
        /// 序列号编码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSn
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSnProperty = P<SparePartAcceptanceSn>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SparePartAcceptanceSn>.Register(e => e.Remark);

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
        public static readonly Property<InspectionResult?> AcceptanceResultProperty = P<SparePartAcceptanceSn>.Register(e => e.AcceptanceResult);

        /// <summary>
        /// 验收结果
        /// </summary>
        public InspectionResult? AcceptanceResult
        {
            get { return GetProperty(AcceptanceResultProperty); }
            set { SetProperty(AcceptanceResultProperty, value); }
        }
        #endregion

        #region 备件验收明细 AcceptDtl
        /// <summary>
        /// 备件验收明细Id
        /// </summary>
        public static readonly IRefIdProperty AcceptDtlIdProperty = P<SparePartAcceptanceSn>.RegisterRefId(e => e.AcceptDtlId, ReferenceType.Parent);

        /// <summary>
        /// 备件验收明细Id
        /// </summary>
        public double AcceptDtlId
        {
            get { return (double)GetRefId(AcceptDtlIdProperty); }
            set { SetRefId(AcceptDtlIdProperty, value); }
        }

        /// <summary>
        /// 备件验收明细
        /// </summary>
        public static readonly RefEntityProperty<SparePartAcceptanceDetail> AcceptDtlProperty = P<SparePartAcceptanceSn>.RegisterRef(e => e.AcceptDtl, AcceptDtlIdProperty);

        /// <summary>
        /// 备件验收明细
        /// </summary>
        public SparePartAcceptanceDetail AcceptDtl
        {
            get { return GetRefEntity(AcceptDtlProperty); }
            set { SetRefEntity(AcceptDtlProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartAcceptanceSn>.RegisterView(e => e.PurchaseOrderNo, p => p.AcceptDtl.PurchaseOrder.OrderNo);

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
        public static readonly Property<int?> PurchaseOrderItemLineNoProperty = P<SparePartAcceptanceSn>.RegisterView(e => e.PurchaseOrderItemLineNo, p => p.AcceptDtl.PurchaseOrderItem.LineNo);

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
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<SparePartAcceptanceSn>.RegisterView(e => e.PurchaseObjectType, p => p.AcceptDtl.PurchaseOrder.PurchaseObjectType);

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
        public static readonly Property<decimal> PriceProperty = P<SparePartAcceptanceSn>.RegisterView(e => e.Price, p => p.AcceptDtl.Price);

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
        public static readonly Property<string> WarehouseNameProperty = P<SparePartAcceptanceSn>.RegisterView(e => e.WarehouseName, p => p.AcceptDtl.Warehouse.Name);

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
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<SparePartAcceptanceSn>.RegisterView(e => e.ApprovalStatus, p => p.AcceptDtl.SparePartAcceptance.ApprovalStatus);

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
    /// 序列号明细 实体配置
    /// </summary>
    internal class SparePartAcceptanceSnConfig : EntityConfig<SparePartAcceptanceSn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_ACPT_SN").MapAllProperties();
            Meta.Property(SparePartAcceptanceSn.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}