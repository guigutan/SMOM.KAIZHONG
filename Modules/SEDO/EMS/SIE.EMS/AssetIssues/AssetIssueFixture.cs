using SIE;
using SIE.Domain;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.Enums;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.AssetIssues
{
    /// <summary>
    /// 发放工治具清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("发放工治具清单")]
    public partial class AssetIssueFixture : DataEntity
    {
        #region 资产发放 AssetIssue
        /// <summary>
        /// 资产发放Id
        /// </summary>
        [Label("资产发放")]
        public static readonly IRefIdProperty AssetIssueIdProperty = P<AssetIssueFixture>.RegisterRefId(e => e.AssetIssueId, ReferenceType.Parent);

        /// <summary>
        /// 资产发放Id
        /// </summary>
        public double AssetIssueId
        {
            get { return (double)GetRefId(AssetIssueIdProperty); }
            set { SetRefId(AssetIssueIdProperty, value); }
        }

        /// <summary>
        /// 资产发放
        /// </summary>
        public static readonly RefEntityProperty<AssetIssue> AssetIssueProperty = P<AssetIssueFixture>.RegisterRef(e => e.AssetIssue, AssetIssueIdProperty);

        /// <summary>
        /// 资产发放
        /// </summary>
        public AssetIssue AssetIssue
        {
            get { return GetRefEntity(AssetIssueProperty); }
            set { SetRefEntity(AssetIssueProperty, value); }
        }
        #endregion

        #region 领用申请工治具清单 AssetRequisitionFixture
        /// <summary>
        /// 领用申请工治具清单Id
        /// </summary>
        [Label("领用申请工治具清单")]
        public static readonly IRefIdProperty AssetRequisitionFixtureIdProperty = P<AssetIssueFixture>.RegisterRefId(e => e.AssetRequisitionFixtureId, ReferenceType.Normal);

        /// <summary>
        /// 领用申请工治具清单Id
        /// </summary>
        public double AssetRequisitionFixtureId
        {
            get { return (double)GetRefId(AssetRequisitionFixtureIdProperty); }
            set { SetRefId(AssetRequisitionFixtureIdProperty, value); }
        }

        /// <summary>
        /// 领用申请工治具清单
        /// </summary>
        public static readonly RefEntityProperty<AssetRequisitionFixture> AssetRequisitionFixtureProperty = P<AssetIssueFixture>.RegisterRef(e => e.AssetRequisitionFixture, AssetRequisitionFixtureIdProperty);

        /// <summary>
        /// 领用申请工治具清单
        /// </summary>
        public AssetRequisitionFixture AssetRequisitionFixture
        {
            get { return GetRefEntity(AssetRequisitionFixtureProperty); }
            set { SetRefEntity(AssetRequisitionFixtureProperty, value); }
        }
        #endregion

        #region ID类工治具台账 FixtureAccount
        /// <summary>
        /// ID类工治具台账Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty FixtureAccountIdProperty =
            P<AssetIssueFixture>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

        /// <summary>
        /// ID类工治具台账Id
        /// </summary>
        public double? FixtureAccountId
        {
            get { return (double?)this.GetRefNullableId(FixtureAccountIdProperty); }
            set { this.SetRefNullableId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// ID类工治具台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureIDAccount> FixtureAccountProperty =
            P<AssetIssueFixture>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// ID类工治具台账
        /// </summary>
        public FixtureIDAccount FixtureAccount
        {
            get { return this.GetRefEntity(FixtureAccountProperty); }
            set { this.SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 本次发放数量 Qty
        /// <summary>
        /// 本次发放数量
        /// </summary>
        [Label("本次发放数量")]
        public static readonly Property<int?> QtyProperty = P<AssetIssueFixture>.Register(e => e.Qty);

        /// <summary>
        /// 本次发放数量
        /// </summary>
        public int? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<FixtureQualityState?> QualityStatusProperty = P<AssetIssueFixture>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState? QualityStatus
        {
            get { return GetProperty(QualityStatusProperty); }
            set { SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 发放库位 StorageLocation
        /// <summary>
        /// 发放库位Id
        /// </summary>
        [Label("发放库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<AssetIssueFixture>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 发放库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 发放库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<AssetIssueFixture>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 发放库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 归还单号 ReturnNo
        /// <summary>
        /// 归还单号
        /// </summary>
        [Label("归还单号")]
        public static readonly Property<string> ReturnNoProperty = P<AssetIssueFixture>.Register(e => e.ReturnNo);

        /// <summary>
        /// 归还单号
        /// </summary>
        public string ReturnNo
        {
            get { return GetProperty(ReturnNoProperty); }
            set { SetProperty(ReturnNoProperty, value); }
        }
        #endregion

        #region 实际归还日期 ReturnDate
        /// <summary>
        /// 实际归还日期
        /// </summary>
        [Label("实际归还日期")]
        public static readonly Property<DateTime?> ReturnDateProperty = P<AssetIssueFixture>.Register(e => e.ReturnDate);

        /// <summary>
        /// 实际归还日期
        /// </summary>
        public DateTime? ReturnDate
        {
            get { return GetProperty(ReturnDateProperty); }
            set { SetProperty(ReturnDateProperty, value); }
        }
        #endregion

        #region 归还状态 ReturnStatus
        /// <summary>
        /// 归还状态
        /// </summary>
        [Label("归还状态")]
        public static readonly Property<ReturnStatus> ReturnStatusProperty = P<AssetIssueFixture>.Register(e => e.ReturnStatus);

        /// <summary>
        /// 归还状态
        /// </summary>
        public ReturnStatus ReturnStatus
        {
            get { return GetProperty(ReturnStatusProperty); }
            set { SetProperty(ReturnStatusProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 申请行号 LineNo
        /// <summary>
        /// 申请行号
        /// </summary>
        [Label("申请行号")]
        public static readonly Property<string> LineNoProperty = P<AssetIssueFixture>.RegisterView(e => e.LineNo, p => p.AssetRequisitionFixture.LineNo);

        /// <summary>
        /// 申请行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 工治具编码Id FixtureEncodeId
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码Id")]
        public static readonly Property<double> FixtureEncodeIdProperty = P<AssetIssueFixture>.RegisterView(e => e.FixtureEncodeId, p => p.AssetRequisitionFixture.FixtureEncodeId);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return this.GetProperty(FixtureEncodeIdProperty); }
            set { this.SetProperty(FixtureEncodeIdProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeProperty = P<AssetIssueFixture>.RegisterView(e => e.FixtureEncode, p => p.AssetRequisitionFixture.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncode
        {
            get { return this.GetProperty(FixtureEncodeProperty); }
            set { this.SetProperty(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<AssetIssueFixture>.RegisterView(e => e.ModelCode, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<AssetIssueFixture>.RegisterView(e => e.ModelName, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<AssetIssueFixture>.RegisterView(e => e.FixtureType, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
            set { this.SetProperty(FixtureTypeProperty, value); }
        }
        #endregion

        #region 管控方式 ManageMode
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<AssetIssueFixture>.RegisterView(e => e.ManageMode, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set { this.SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<AssetIssueFixture>.RegisterView(e => e.UnitName, p => p.AssetRequisitionFixture.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion
        #endregion

        #region 不映射数据库的属性

        #region 未发放数量 NotPickQty
        /// <summary>
        /// 未发放数量
        /// </summary>
        [Label("未发放数量")]
        public static readonly Property<int> NotPickQtyProperty = P<AssetIssueFixture>.Register(e => e.NotPickQty);

        /// <summary>
        /// 未发放数量
        /// </summary>
        public int NotPickQty
        {
            get { return this.GetProperty(NotPickQtyProperty); }
            set { this.SetProperty(NotPickQtyProperty, value); }
        }
        #endregion

        #region 发放仓库 Warehouse
        /// <summary>
        /// 发放仓库Id
        /// </summary>
        [Label("发放仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetIssueFixture>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发放仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发放仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetIssueFixture>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发放仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位库存数 StoreUsableQty
        /// <summary>
        /// 库位库存数
        /// </summary>
        [Label("库位库存数")]
        public static readonly Property<int> StoreUsableQtyProperty = P<AssetIssueFixture>.Register(e => e.StoreUsableQty);

        /// <summary>
        /// 库位库存数
        /// </summary>
        public int StoreUsableQty
        {
            get { return this.GetProperty(StoreUsableQtyProperty); }
            set { this.SetProperty(StoreUsableQtyProperty, value); }
        }
        #endregion

        #region 是否已选明细行 IsSelected
        /// <summary>
        /// 是否已选明细行
        /// </summary>
        [Label("是否已选明细行")]
        public static readonly Property<bool> IsSelectedProperty = P<AssetIssueFixture>.Register(e => e.IsSelected);

        /// <summary>
        /// 是否已选明细行
        /// </summary>
        public bool IsSelected
        {
            get { return this.GetProperty(IsSelectedProperty); }
            set { this.SetProperty(IsSelectedProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 发放工治具清单 实体配置
    /// </summary>
    internal class AssetIssueFixtureConfig : EntityConfig<AssetIssueFixture>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_ISSUE_FIX").MapAllProperties();
            Meta.Property(AssetIssueFixture.NotPickQtyProperty).DontMapColumn();
            Meta.Property(AssetIssueFixture.StoreUsableQtyProperty).DontMapColumn();
            Meta.Property(AssetIssueFixture.WarehouseIdProperty).DontMapColumn();
            Meta.Property(AssetIssueFixture.WarehouseProperty).DontMapColumn();
            Meta.Property(AssetIssueFixture.IsSelectedProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}