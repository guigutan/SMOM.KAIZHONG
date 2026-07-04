using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 润滑备件申请
    /// </summary>
    [ChildEntity, Serializable]
    [Label("润滑备件申请")]
    public partial class LubricationSparePartApply : DataEntity
    {
        #region 润滑记录 Lubrication
        /// <summary>
        /// 润滑记录Id
        /// </summary>
        public static readonly IRefIdProperty LubricationIdProperty = P<LubricationSparePartApply>.RegisterRefId(e => e.LubricationId, ReferenceType.Parent);

        /// <summary>
        /// 润滑记录Id
        /// </summary>
        public double LubricationId
        {
            get { return (double)GetRefId(LubricationIdProperty); }
            set { SetRefId(LubricationIdProperty, value); }
        }

        /// <summary>
        /// 润滑记录
        /// </summary>
        public static readonly RefEntityProperty<Lubrication> LubricationProperty = P<LubricationSparePartApply>.RegisterRef(e => e.Lubrication, LubricationIdProperty);

        /// <summary>
        /// 润滑记录
        /// </summary>
        public Lubrication Lubrication
        {
            get { return GetRefEntity(LubricationProperty); }
            set { SetRefEntity(LubricationProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        public static readonly IRefIdProperty SparePartIdProperty = P<LubricationSparePartApply>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<LubricationSparePartApply>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 申请数量 ApplyQty
        /// <summary>
        /// 申请数量
        /// </summary>
        [Label("申请数量")]
        public static readonly Property<int> ApplyQtyProperty = P<LubricationSparePartApply>.Register(e => e.ApplyQty);

        /// <summary>
        /// 申请数量
        /// </summary>
        public int ApplyQty
        {
            get { return GetProperty(ApplyQtyProperty); }
            set { SetProperty(ApplyQtyProperty, value); }
        }
        #endregion

        #region 出库仓库 OutStockWarehouse
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库")]
        public static readonly IRefIdProperty OutStockWarehouseIdProperty = P<LubricationSparePartApply>.RegisterRefId(e => e.OutStockWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 出库仓库Id
        /// </summary>
        public double? OutStockWarehouseId
        {
            get { return (double?)this.GetRefNullableId(OutStockWarehouseIdProperty); }
            set { this.SetRefNullableId(OutStockWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 出库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> OutStockWarehouseProperty = P<LubricationSparePartApply>.RegisterRef(e => e.OutStockWarehouse, OutStockWarehouseIdProperty);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public Warehouse OutStockWarehouse
        {
            get { return this.GetRefEntity(OutStockWarehouseProperty); }
            set { this.SetRefEntity(OutStockWarehouseProperty, value); }
        }
        #endregion

        #region 备件申请单 ApplyDetail
        /// <summary>
        /// 备件申请单Id
        /// </summary>
        public static readonly IRefIdProperty ApplyDetailIdProperty = P<LubricationSparePartApply>.RegisterRefId(e => e.ApplyDetailId, ReferenceType.Normal);

        /// <summary>
        /// 备件申请单Id
        /// </summary>
        public double? ApplyDetailId
        {
            get { return (double?)GetRefNullableId(ApplyDetailIdProperty); }
            set { SetRefNullableId(ApplyDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件申请单
        /// </summary>
        public static readonly RefEntityProperty<ApplyDetail> ApplyDetailProperty = P<LubricationSparePartApply>.RegisterRef(e => e.ApplyDetail, ApplyDetailIdProperty);

        /// <summary>
        /// 备件申请单
        /// </summary>
        public ApplyDetail ApplyDetail
        {
            get { return GetRefEntity(ApplyDetailProperty); }
            set { SetRefEntity(ApplyDetailProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<LubricationSparePartApply>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 已申请 IsApply
        /// <summary>
        /// 已申请
        /// </summary>
        [Label("已申请")]
        public static readonly Property<bool> IsApplyProperty = P<LubricationSparePartApply>.Register(e => e.IsApply);

        /// <summary>
        /// 已申请
        /// </summary>
        public bool IsApply
        {
            get { return GetProperty(IsApplyProperty); }
            set { SetProperty(IsApplyProperty, value); }
        }
        #endregion

        #region 不映射数据库字段

        #region 库存数 StoreQty
        /// <summary>
        /// 库存数
        /// </summary>
        [Label("库存数")]
        public static readonly Property<decimal> StoreQtyProperty = P<LubricationSparePartApply>.Register(e => e.StoreQty);

        /// <summary>
        /// 库存数
        /// </summary>
        public decimal StoreQty
        {
            get { return this.GetProperty(StoreQtyProperty); }
            set { this.SetProperty(StoreQtyProperty, value); }
        }
        #endregion

        #endregion

        #region 视图属性

        #region 备件编码 SparePartCodeView
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeViewProperty = P<LubricationSparePartApply>.RegisterView(e => e.SparePartCodeView, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCodeView
        {
            get { return this.GetProperty(SparePartCodeViewProperty); }
        }
        #endregion

        #region 备件名称 SparePartNameView
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameViewProperty = P<LubricationSparePartApply>.RegisterView(e => e.SparePartNameView, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartNameView
        {
            get { return this.GetProperty(SparePartNameViewProperty); }
        }
        #endregion

        #region 规格型号 SpecificationView
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationViewProperty = P<LubricationSparePartApply>.RegisterView(e => e.SpecificationView, p => p.SparePart.Specification);

        /// <summary>
        /// 是否以旧换新
        /// </summary>
        public string SpecificationView
        {
            get { return this.GetProperty(SpecificationViewProperty); }
        }
        #endregion

        #region 单位 UnitView
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitViewProperty = P<LubricationSparePartApply>.RegisterView(e => e.UnitView, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitView
        {
            get { return this.GetProperty(UnitViewProperty); }
        }
        #endregion

        #region 申请单状态 AppStateView
        /// <summary>
        /// 申请单状态
        /// </summary>
        [Label("申请单状态")]
        public static readonly Property<AuditState> AppStateViewProperty = P<LubricationSparePartApply>.RegisterView(e => e.AppStateView, p => p.ApplyDetail.SparePartApp.AuditState);

        /// <summary>
        /// 申请单状态
        /// </summary>
        public AuditState AppStateView
        {
            get { return this.GetProperty(AppStateViewProperty); }
        }
        #endregion

        #region 申请单号 ApplyNoView
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        public static readonly Property<string> ApplyNoViewProperty = P<LubricationSparePartApply>.RegisterView(e => e.ApplyNoView, p => p.ApplyDetail.SparePartApp.No);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string ApplyNoView
        {
            get { return this.GetProperty(ApplyNoViewProperty); }
        }
        #endregion

        #region 出库仓库名称 OutStockWarehouseName
        /// <summary>
        /// 出库仓库名称
        /// </summary>
        [Label("出库仓库名称")]
        public static readonly Property<string> OutStockWarehouseNameProperty = P<LubricationSparePartApply>.RegisterView(e => e.OutStockWarehouseName, p => p.OutStockWarehouse.Name);

        /// <summary>
        /// 出库仓库名称
        /// </summary>
        public string OutStockWarehouseName
        {
            get { return this.GetProperty(OutStockWarehouseNameProperty); }
        }
        #endregion
        #endregion

    }

    /// <summary>
    /// 润滑备件申请 实体配置
    /// </summary>
    internal class LubricationSparePartApplyConfig : EntityConfig<LubricationSparePartApply>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_LUBR_SP_APL").MapAllProperties();
            Meta.Property(LubricationSparePartApply.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.Property(LubricationSparePartApply.StoreQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}