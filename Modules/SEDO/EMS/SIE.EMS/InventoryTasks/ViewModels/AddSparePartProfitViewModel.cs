using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 新增盘盈 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("新增盘盈")]
    public class AddSparePartProfitViewModel : ViewModel
    {

        #region PDA盘点状态 PdaType
        /// <summary>
        /// PDA盘点状态
        /// </summary>
        [Label("PDA盘点状态")]
        public static readonly Property<int> PdaTypeProperty = P<AddSparePartProfitViewModel>.Register(e => e.PdaType);

        /// <summary>
        /// PDA盘点状态
        /// </summary>
        public int PdaType
        {
            get { return this.GetProperty(PdaTypeProperty); }
            set { this.SetProperty(PdaTypeProperty, value); }
        }
        #endregion


        #region 盘点任务id InventoryTaskId
        /// <summary>
        /// 盘点任务id
        /// </summary>
        [Label("盘点任务id")]
        public static readonly Property<double> InventoryTaskIdProperty = P<AddSparePartProfitViewModel>.Register(e => e.InventoryTaskId);

        /// <summary>
        /// 盘点任务id
        /// </summary>
        public double InventoryTaskId
        {
            get { return this.GetProperty(InventoryTaskIdProperty); }
            set { this.SetProperty(InventoryTaskIdProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty = P<AddSparePartProfitViewModel>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<AddSparePartProfitViewModel>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 自动生成序列号 GenerateSn
        /// <summary>
        /// 自动生成序列号
        /// </summary>
        [Label("自动生成序列号")]
        public static readonly Property<bool> GenerateSnProperty = P<AddSparePartProfitViewModel>.Register(e => e.GenerateSn);

        /// <summary>
        /// 自动生成序列号
        /// </summary>
        public bool GenerateSn
        {
            get { return this.GetProperty(GenerateSnProperty); }
            set { this.SetProperty(GenerateSnProperty, value); }
        }
        #endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<AddSparePartProfitViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<AddSparePartProfitViewModel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<AddSparePartProfitViewModel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<AddSparePartProfitViewModel>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<AddSparePartProfitViewModel>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 实盘良品数 GoodQty
        /// <summary>
        /// 实盘良品数
        /// </summary>
        [Label("实盘良品数")]
        public static readonly Property<int> GoodQtyProperty = P<AddSparePartProfitViewModel>.Register(e => e.GoodQty);

        /// <summary>
        /// 实盘良品数
        /// </summary>
        public int GoodQty
        {
            get { return this.GetProperty(GoodQtyProperty); }
            set { this.SetProperty(GoodQtyProperty, value); }
        }
        #endregion

        #region 实盘不良品数 NgQty
        /// <summary>
        /// 实盘不良品数
        /// </summary>
        [Label("实盘不良品数")]
        public static readonly Property<int> NgQtyProperty = P<AddSparePartProfitViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 实盘不良品数
        /// </summary>
        public int NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 自动生成批次号 GenerateLotNo
        /// <summary>
        /// 自动生成批次号
        /// </summary>
        [Label("自动生成批次号")]
        public static readonly Property<bool> GenerateLotNoProperty = P<AddSparePartProfitViewModel>.Register(e => e.GenerateLotNo);

        /// <summary>
        /// 自动生成批次号
        /// </summary>
        public bool GenerateLotNo
        {
            get { return this.GetProperty(GenerateLotNoProperty); }
            set { this.SetProperty(GenerateLotNoProperty, value); }
        }
        #endregion

        #region 批次号 LotNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<AddSparePartProfitViewModel>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotNo
        {
            get { return this.GetProperty(LotNoProperty); }
            set { this.SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 良品 IsGood
        /// <summary>
        /// 良品
        /// </summary>
        [Label("良品")]
        public static readonly Property<bool> IsGoodProperty = P<AddSparePartProfitViewModel>.Register(e => e.IsGood);

        /// <summary>
        /// 良品
        /// </summary>
        public bool IsGood
        {
            get { return this.GetProperty(IsGoodProperty); }
            set { this.SetProperty(IsGoodProperty, value); }
        }
        #endregion

        #region 不良品 IsNg
        /// <summary>
        /// 不良品
        /// </summary>
        [Label("不良品")]
        public static readonly Property<bool> IsNgProperty = P<AddSparePartProfitViewModel>.Register(e => e.IsNg);

        /// <summary>
        /// 不良品
        /// </summary>
        public bool IsNg
        {
            get { return this.GetProperty(IsNgProperty); }
            set { this.SetProperty(IsNgProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<AddSparePartProfitViewModel>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion


        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<AddSparePartProfitViewModel>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<AddSparePartProfitViewModel>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
        }
        #endregion


        #region 库位 LocationName
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> LocationNameProperty = P<AddSparePartProfitViewModel>.RegisterView(e => e.LocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { this.SetProperty(LocationNameProperty, value); }
        }
        #endregion


        #region 型号规格 Specification
        /// <summary>
        /// 型号规格
        /// </summary>
        [Label("型号规格")]
        public static readonly Property<string> SpecificationProperty = P<AddSparePartProfitViewModel>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 型号规格
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 分类层级 ItemCategoryName
        /// <summary>
        /// 分类层级
        /// </summary>
        [Label("分类层级")]
        public static readonly Property<string> ItemCategoryNameProperty = P<AddSparePartProfitViewModel>.RegisterView(e => e.ItemCategoryName, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 分类层级
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<AddSparePartProfitViewModel>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion
        #endregion
    }
}
