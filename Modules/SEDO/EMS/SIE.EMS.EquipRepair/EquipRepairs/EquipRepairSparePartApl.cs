using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 点检计划备件更换
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检计划备件更换")]
    public partial class EquipRepairSparePartApl : DataEntity
    {
        #region 设备维修单 EquipRepairBill
        /// <summary>
        /// 设备维修单Id
        /// </summary>
        [Label("设备维修单")]
        public static readonly IRefIdProperty EquipRepairBillIdProperty =
            P<EquipRepairSparePartApl>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Parent);

        /// <summary>
        /// 设备维修单Id
        /// </summary>
        public double EquipRepairBillId
        {
            get { return (double)this.GetRefId(EquipRepairBillIdProperty); }
            set { this.SetRefId(EquipRepairBillIdProperty, value); }
        }

        /// <summary>
        /// 设备维修单
        /// </summary>
        public static readonly RefEntityProperty<EquipRepairBill> EquipRepairBillProperty =
            P<EquipRepairSparePartApl>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

        /// <summary>
        /// 设备维修单
        /// </summary>
        public EquipRepairBill EquipRepairBill
        {
            get { return this.GetRefEntity(EquipRepairBillProperty); }
            set { this.SetRefEntity(EquipRepairBillProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<EquipRepairSparePartApl>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<EquipRepairSparePartApl>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
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
        public static readonly Property<int> ApplyQtyProperty = P<EquipRepairSparePartApl>.Register(e => e.ApplyQty);

        /// <summary>
        /// 申请数量
        /// </summary>
        public int ApplyQty
        {
            get { return this.GetProperty(ApplyQtyProperty); }
            set { this.SetProperty(ApplyQtyProperty, value); }
        }
        #endregion

        #region 出库仓库 OutStockWarehouse
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库")]
        public static readonly IRefIdProperty OutStockWarehouseIdProperty =
            P<EquipRepairSparePartApl>.RegisterRefId(e => e.OutStockWarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> OutStockWarehouseProperty =
            P<EquipRepairSparePartApl>.RegisterRef(e => e.OutStockWarehouse, OutStockWarehouseIdProperty);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public Warehouse OutStockWarehouse
        {
            get { return this.GetRefEntity(OutStockWarehouseProperty); }
            set { this.SetRefEntity(OutStockWarehouseProperty, value); }
        }
        #endregion

        #region 备件申请单明细 ApplyDetail
        /// <summary>
        /// 备件申请单明细Id
        /// </summary>
        [Label("备件申请单明细")]
        public static readonly IRefIdProperty ApplyDetailIdProperty =
            P<EquipRepairSparePartApl>.RegisterRefId(e => e.ApplyDetailId, ReferenceType.Normal);

        /// <summary>
        /// 备件申请单明细Id
        /// </summary>
        public double? ApplyDetailId
        {
            get { return (double?)this.GetRefNullableId(ApplyDetailIdProperty); }
            set { this.SetRefNullableId(ApplyDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件申请单明细
        /// </summary>
        public static readonly RefEntityProperty<ApplyDetail> ApplyDetailProperty =
            P<EquipRepairSparePartApl>.RegisterRef(e => e.ApplyDetail, ApplyDetailIdProperty);

        /// <summary>
        /// 备件申请单明细
        /// </summary>
        public ApplyDetail ApplyDetail
        {
            get { return this.GetRefEntity(ApplyDetailProperty); }
            set { this.SetRefEntity(ApplyDetailProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipRepairSparePartApl>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 是否已申请 IsApply
        /// <summary>
        /// 是否已申请
        /// </summary>
        [Label("是否已申请")]
        public static readonly Property<bool> IsApplyProperty = P<EquipRepairSparePartApl>.Register(e => e.IsApply);

        /// <summary>
        /// 是否已申请
        /// </summary>
        public bool IsApply
        {
            get { return this.GetProperty(IsApplyProperty); }
            set { this.SetProperty(IsApplyProperty, value); }
        }
        #endregion

        #region 不映射数据库字段

        #region 库存数 StoreQty
        /// <summary>
        /// 库存数
        /// </summary>
        [Label("库存数")]
        public static readonly Property<decimal> StoreQtyProperty = P<EquipRepairSparePartApl>.Register(e => e.StoreQty);

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
        public static readonly Property<string> SparePartCodeViewProperty = P<EquipRepairSparePartApl>.RegisterView(e => e.SparePartCodeView, p => p.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameViewProperty = P<EquipRepairSparePartApl>.RegisterView(e => e.SparePartNameView, p => p.SparePart.SparePartName);

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
        public static readonly Property<string> SpecificationViewProperty = P<EquipRepairSparePartApl>.RegisterView(e => e.SpecificationView, p => p.SparePart.Specification);

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
        public static readonly Property<string> UnitViewProperty = P<EquipRepairSparePartApl>.RegisterView(e => e.UnitView, p => p.SparePart.Unit.Name);

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
        public static readonly Property<AuditState> AppStateViewProperty = P<EquipRepairSparePartApl>.RegisterView(e => e.AppStateView, p => p.ApplyDetail.SparePartApp.AuditState);

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
        public static readonly Property<string> ApplyNoViewProperty = P<EquipRepairSparePartApl>.RegisterView(e => e.ApplyNoView, p => p.ApplyDetail.SparePartApp.No);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string ApplyNoView
        {
            get { return this.GetProperty(ApplyNoViewProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 点检计划配件更换 实体配置
    /// </summary>
    internal class EquipRepairSparePartAplConfig : EntityConfig<EquipRepairSparePartApl>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(EquipRepairSparePartApl.RemarkProperty, new StringLengthRangeRule() { Max = 2000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_REPAIR_SP_APL").MapAllProperties();
            Meta.Property(EquipRepairSparePartApl.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairSparePartApl.StoreQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}