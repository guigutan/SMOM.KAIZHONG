
using SIE.Domain;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts.Applys.Details
{
    /// <summary>
    /// 申请单明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("申请单明细")]
    public class ApplyDetail : DataEntity
    {
        #region 备件申请单 SparePartApp
        /// <summary>
        /// 备件申请单Id
        /// </summary>
        [Label("备件申请单")]
        public static readonly IRefIdProperty SparePartAppIdProperty =
            P<ApplyDetail>.RegisterRefId(e => e.SparePartAppId, ReferenceType.Parent);

        /// <summary>
        /// 备件申请单Id
        /// </summary>
        public double SparePartAppId
        {
            get { return (double)this.GetRefId(SparePartAppIdProperty); }
            set { this.SetRefId(SparePartAppIdProperty, value); }
        }

        /// <summary>
        /// 备件申请单
        /// </summary>
        public static readonly RefEntityProperty<SparePartApp> SparePartAppProperty =
            P<ApplyDetail>.RegisterRef(e => e.SparePartApp, SparePartAppIdProperty);

        /// <summary>
        /// 备件申请单
        /// </summary>
        public SparePartApp SparePartApp
        {
            get { return this.GetRefEntity(SparePartAppProperty); }
            set { this.SetRefEntity(SparePartAppProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<ApplyDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<ApplyDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件部位 SparePartPart
        /// <summary>
        /// 备件部位
        /// </summary>
        [Label("备件部位")]
        public static readonly Property<string> SparePartPartProperty = P<ApplyDetail>.Register(e => e.SparePartPart);

        /// <summary>
        /// 备件部位
        /// </summary>
        public string SparePartPart
        {
            get { return this.GetProperty(SparePartPartProperty); }
            set { this.SetProperty(SparePartPartProperty, value); }
        }
        #endregion       

        #region 状态 AuditState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<AuditState> AuditStateProperty = P<ApplyDetail>.RegisterView(e => e.AuditState, p => p.SparePartApp.AuditState);

        /// <summary>
        /// 状态
        /// </summary>
        public AuditState AuditState
        {
            get { return this.GetProperty(AuditStateProperty); }
        }
        #endregion        

        #region 申请数量 ApplyAmount
        /// <summary>
        /// 申请数量
        /// </summary>a
        [Label("申请数量")]
        [Required]
        [MinValue(1)]
        public static readonly Property<int> ApplyAmountProperty = P<ApplyDetail>.Register(e => e.ApplyAmount);

        /// <summary>
        /// 申请数量
        /// </summary>
        public int ApplyAmount
        {
            get { return this.GetProperty(ApplyAmountProperty); }
            set { this.SetProperty(ApplyAmountProperty, value); }
        }
        #endregion

        #region 库存数 DepotAmount
        /// <summary>
        /// 库存数
        /// </summary>
        [Label("库存数")]
        public static readonly Property<int> DepotAmountProperty = P<ApplyDetail>.Register(e => e.DepotAmount);

        /// <summary>
        /// 库存数
        /// </summary>
        public int DepotAmount
        {
            get { return this.GetProperty(DepotAmountProperty); }
            set { this.SetProperty(DepotAmountProperty, value); }
        }
        #endregion

        #region 出库数量 OutDepotAmount
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> OutDepotAmountProperty = P<ApplyDetail>.Register(e => e.OutDepotAmount);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutDepotAmount
        {
            get { return this.GetProperty(OutDepotAmountProperty); }
            set { this.SetProperty(OutDepotAmountProperty, value); }
        }
        #endregion

        #region 剩余数量 ResidueAmount
        ///// <summary>
        ///// 剩余数量
        ///// </summary>
        //[Label("剩余数量")]
        //public static readonly Property<int>  ResidueAmountProperty = P<ApplyDetail>.Register(e => e. ResidueAmount);

        ///// <summary>
        ///// 剩余数量
        ///// </summary>
        //public int  ResidueAmount
        //{
        //    get { return this.GetProperty( ResidueAmountProperty); }
        //    set { this.SetProperty( ResidueAmountProperty, value); }
        //}
        #endregion

        #region 使用数量  UseAmount
        /// <summary>
        /// 使用数量
        /// </summary>
        [Label("使用数量")]
        public static readonly Property<int> UseAmountProperty = P<ApplyDetail>.Register(e => e.UseAmount);

        /// <summary>
        /// 使用数量
        /// </summary>
        public int UseAmount
        {
            get { return this.GetProperty(UseAmountProperty); }
            set { this.SetProperty(UseAmountProperty, value); }
        }
        #endregion

        #region 出库仓库 Warehouse
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<ApplyDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<ApplyDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 申请单号 ApplyNo
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        public static readonly Property<string> ApplyNoProperty = P<ApplyDetail>.RegisterView(e => e.ApplyNo, p => p.SparePartApp.No);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string ApplyNo
        {
            get { return this.GetProperty(ApplyNoProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<ApplyDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<ApplyDetail>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 备件类型编码 SparePartTypeCode
        /// <summary>
        /// 备件类型编码
        /// </summary>
        [Label("备件类型编码")]
        public static readonly Property<string> SparePartTypeCodeProperty = P<ApplyDetail>.RegisterView(e => e.SparePartTypeCode, p => p.SparePart.ItemCategory.Code);

        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode
        {
            get { return this.GetProperty(SparePartTypeCodeProperty); }
        }
        #endregion

        #region 设备型号 EquipModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelCodeProperty = P<ApplyDetail>.RegisterView(e => e.EquipModelCode, p => p.SparePart.SpartEquipModel.Code);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<ApplyDetail>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 设备型号Id EquipModelId
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<double?> EquipModelIdProperty = P<ApplyDetail>.RegisterView(e => e.EquipModelId, p => p.SparePartApp.EquipModelId);

        /// <summary>
        /// 设备型号
        /// </summary>
        public double? EquipModelId
        {
            get { return this.GetProperty(EquipModelIdProperty); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType?> SpartTypeProperty = P<ApplyDetail>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType? SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<ApplyDetail>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #endregion


    }

    /// <summary>
    /// 备件申请 实体配置
    /// </summary>
    internal class ApplyDetailConfig : EntityConfig<ApplyDetail>
    {

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_APPLY_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}