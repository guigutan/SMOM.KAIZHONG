using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// 库存详情
	/// </summary>
	[ChildEntity, Serializable]
    [ConditionQueryType(typeof(FixtureAccountStockCriteria))]
    [Label("工治具库存详情")]
    public partial class FixtureAccountStock : DataEntity
    {
        #region 总数量 TotalQty
        /// <summary>
        /// 总数量
        /// </summary>
        [Label("总数量")]
        public static readonly Property<int> TotalQtyProperty = P<FixtureAccountStock>.Register(e => e.TotalQty);

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalQty
        {
            get { return GetProperty(TotalQtyProperty); }
            set { SetProperty(TotalQtyProperty, value); }
        }
        #endregion

        #region 合格数量 PassQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<int> PassQtyProperty = P<FixtureAccountStock>.Register(e => e.PassQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public int PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<int> NgQtyProperty = P<FixtureAccountStock>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<int> ScrapQtyProperty = P<FixtureAccountStock>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public int ScrapQty
        {
            get { return GetProperty(ScrapQtyProperty); }
            set { SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 工治具仓库 Warehouse
        /// <summary>
        /// 工治具仓库Id
        /// </summary>
        public static readonly IRefIdProperty FixtureWarehouseIdProperty = P<FixtureAccountStock>.RegisterRefId(e => e.FixtureWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 工治具仓库Id
        /// </summary>
        public double FixtureWarehouseId
        {
            get { return (double)GetRefId(FixtureWarehouseIdProperty); }
            set { SetRefId(FixtureWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 工治具仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> FixtureWarehouseProperty = P<FixtureAccountStock>.RegisterRef(e => e.Warehouse, FixtureWarehouseIdProperty);

        /// <summary>
        /// 工治具仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(FixtureWarehouseProperty); }
            set { SetRefEntity(FixtureWarehouseProperty, value); }
        }
        #endregion

        #region 工治具库位 StorageLocation
        /// <summary>
        /// 工治具库位Id
        /// </summary>
        public static readonly IRefIdProperty FixtureStorageLocationIdProperty = P<FixtureAccountStock>.RegisterRefId(e => e.FixtureStorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 工治具库位Id
        /// </summary>
        public double? FixtureStorageLocationId
        {
            get { return (double?)GetRefNullableId(FixtureStorageLocationIdProperty); }
            set { SetRefNullableId(FixtureStorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 工治具库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> FixtureStorageLocationProperty = P<FixtureAccountStock>.RegisterRef(e => e.StorageLocation, FixtureStorageLocationIdProperty);

        /// <summary>
        /// 工治具库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(FixtureStorageLocationProperty); }
            set { SetRefEntity(FixtureStorageLocationProperty, value); }
        }
        #endregion

        #region 工治具台账 FixtureAccount
        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureAccountStock>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Parent);

        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureAccountStock>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具台账
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty = P<FixtureAccountStock>.RegisterView(e => e.AccountCode, p => p.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
            set { this.SetProperty(AccountCodeProperty, value); }
        }
        #endregion

        #region 工治具编码Id EncodeId
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码Id")]
        public static readonly Property<double> EncodeIdProperty = P<FixtureAccountStock>.RegisterView(e => e.EncodeId, p => p.FixtureAccount.FixtureEncodeId);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double EncodeId
        {
            get { return this.GetProperty(EncodeIdProperty); }
            set { this.SetProperty(EncodeIdProperty, value); }
        }
        #endregion

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<FixtureAccountStock>.RegisterView(e => e.EncodeCode, p => p.FixtureAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
            set { this.SetProperty(EncodeCodeProperty, value); }
        }
        #endregion

        #region 工治具型号编码 ModelCode
        /// <summary>
        /// 工治具型号编码
        /// </summary>
        [Label("工治具型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureAccountStock>.RegisterView(e => e.ModelCode, p => p.FixtureAccount.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 工治具型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 工治具型号名称 ModelName
        /// <summary>
        /// 工治具型号名称
        /// </summary>
        [Label("工治具型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureAccountStock>.RegisterView(e => e.ModelName, p => p.FixtureAccount.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 工治具型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureAccountStock>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double? FixtureTypeId
        {
            get { return (double?)this.GetRefNullableId(FixtureTypeIdProperty); }
            set { this.SetRefNullableId(FixtureTypeIdProperty, value); }
        }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public static readonly RefEntityProperty<FixtureType> FixtureTypeProperty =
            P<FixtureAccountStock>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 工治具类型编码 FixtureTypeCode
        /// <summary>
        /// 工治具类型编码
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeCodeProperty = P<FixtureAccountStock>.RegisterView(e => e.FixtureTypeCode, p => p.FixtureAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型编码
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
        }
        #endregion


        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<FixtureAccountStock>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<FixtureAccountStock>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 库位编码 LocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> LocationCodeProperty = P<FixtureAccountStock>.RegisterView(e => e.LocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode
        {
            get { return this.GetProperty(LocationCodeProperty); }
            set { this.SetProperty(LocationCodeProperty, value); }
        }
        #endregion

        #region 库位名称 LocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> LocationNameProperty = P<FixtureAccountStock>.RegisterView(e => e.LocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { this.SetProperty(LocationNameProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 库存详情 实体配置
    /// </summary>
    internal class FixtureAccountStockConfig : EntityConfig<FixtureAccountStock>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ACC_STOCK").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
