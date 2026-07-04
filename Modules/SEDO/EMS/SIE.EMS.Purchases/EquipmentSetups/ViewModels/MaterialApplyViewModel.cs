using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 领料申请 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("领料申请")]
    public class MaterialApplyViewModel : ViewModel
    {
        #region 安装调试id EquipmentSetupId
        /// <summary>
        /// 安装调试id
        /// </summary>
        [Label("安装调试id")]
        public static readonly Property<double> EquipmentSetupIdProperty = P<MaterialApplyViewModel>.Register(e => e.EquipmentSetupId);

        /// <summary>
        /// 安装调试id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return this.GetProperty(EquipmentSetupIdProperty); }
            set { this.SetProperty(EquipmentSetupIdProperty, value); }
        }
        #endregion

        #region 需求时间 DemandTime
        /// <summary>
        /// 需求时间
        /// </summary>
        [Label("需求时间")]
        public static readonly Property<DateTime?> DemandTimeProperty = P<MaterialApplyViewModel>.Register(e => e.DemandTime);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime? DemandTime
        {
            get { return this.GetProperty(DemandTimeProperty); }
            set { this.SetProperty(DemandTimeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<MaterialApplyViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 领料申请明细 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("领料申请明细")]
    public class MaterialApplyDetailViewModel : ViewModel
    {
        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty = P<MaterialApplyDetailViewModel>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<MaterialApplyDetailViewModel>.RegisterRef(e => e.SparePart, SparePartIdProperty);

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
        public static readonly Property<decimal> ApplyQtyProperty = P<MaterialApplyDetailViewModel>.Register(e => e.ApplyQty);

        /// <summary>
        /// 申请数量
        /// </summary>
        public decimal ApplyQty
        {
            get { return GetProperty(ApplyQtyProperty); }
            set { SetProperty(ApplyQtyProperty, value); }
        }
        #endregion

        #region 出库仓库 Warehouse
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<MaterialApplyDetailViewModel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 出库仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 出库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<MaterialApplyDetailViewModel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<MaterialApplyDetailViewModel>.Register(e => e.WarehouseName);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<MaterialApplyDetailViewModel>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<MaterialApplyDetailViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 库存可用数 WarehouseQty
        /// <summary>
        /// 库存可用数
        /// </summary>
        [Label("库存可用数")]
        public static readonly Property<decimal> WarehouseQtyProperty = P<MaterialApplyDetailViewModel>.Register(e => e.WarehouseQty);

        /// <summary>
        /// 库存可用数
        /// </summary>
        public decimal WarehouseQty
        {
            get { return this.GetProperty(WarehouseQtyProperty); }
            set { this.SetProperty(WarehouseQtyProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<MaterialApplyDetailViewModel>.Register(e => e.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
            set { this.SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 类型 PartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType?> PartTypeProperty = P<MaterialApplyDetailViewModel>.Register(e => e.PartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType? PartType
        {
            get { return this.GetProperty(PartTypeProperty); }
            set { this.SetProperty(PartTypeProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<MaterialApplyDetailViewModel>.Register(e => e.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod? ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
        }
        #endregion
    }
}
