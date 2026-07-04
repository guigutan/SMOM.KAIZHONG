using SIE.Domain;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.AssetDisposals
{
    /// <summary>
    /// 资产处置工治具清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工治具清单")]
    public partial class AssetDisposalFixture : DataEntity
    {
        #region 资产原值 OriginalValue
        /// <summary>
        /// 资产原值
        /// </summary>
        [Label("资产原值")]
        public static readonly Property<decimal> OriginalValueProperty = P<AssetDisposalFixture>.Register(e => e.OriginalValue);

        /// <summary>
        /// 资产原值
        /// </summary>
        public decimal OriginalValue
        {
            get { return GetProperty(OriginalValueProperty); }
            set { SetProperty(OriginalValueProperty, value); }
        }
        #endregion

        #region 资产净值 NetValue
        /// <summary>
        /// 资产净值
        /// </summary>
        [Label("资产净值")]
        public static readonly Property<decimal> NetValueProperty = P<AssetDisposalFixture>.Register(e => e.NetValue);

        /// <summary>
        /// 资产净值
        /// </summary>
        public decimal NetValue
        {
            get { return GetProperty(NetValueProperty); }
            set { SetProperty(NetValueProperty, value); }
        }
        #endregion

        #region 资产残值 ResidualValue
        /// <summary>
        /// 资产残值
        /// </summary>
        [Label("资产残值")]
        public static readonly Property<decimal> ResidualValueProperty = P<AssetDisposalFixture>.Register(e => e.ResidualValue);

        /// <summary>
        /// 资产残值
        /// </summary>
        public decimal ResidualValue
        {
            get { return GetProperty(ResidualValueProperty); }
            set { SetProperty(ResidualValueProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty =
            P<AssetDisposalFixture>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)this.GetRefId(FixtureEncodeIdProperty); }
            set { this.SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty =
            P<AssetDisposalFixture>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return this.GetRefEntity(FixtureEncodeProperty); }
            set { this.SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 工治具ID台账 FixtureAccount
        /// <summary>
        /// 工治具ID台账
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<AssetDisposalFixture>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

        /// <summary>
        /// 工治具ID台账
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具ID台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureIDAccount> FixtureAccountProperty = P<AssetDisposalFixture>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具ID台账
        /// </summary>
        public FixtureIDAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 资产处置 AssetDisposal
        /// <summary>
        /// 资产处置Id
        /// </summary>
        [Label("资产处置")]

        public static readonly IRefIdProperty AssetDisposalIdProperty = P<AssetDisposalFixture>.RegisterRefId(e => e.AssetDisposalId, ReferenceType.Parent);

        /// <summary>
        /// 资产处置Id
        /// </summary>
        public double AssetDisposalId
        {
            get { return (double)GetRefId(AssetDisposalIdProperty); }
            set { SetRefId(AssetDisposalIdProperty, value); }
        }

        /// <summary>
        /// 资产处置
        /// </summary>
        public static readonly RefEntityProperty<AssetDisposal> AssetDisposalProperty = P<AssetDisposalFixture>.RegisterRef(e => e.AssetDisposal, AssetDisposalIdProperty);

        /// <summary>
        /// 资产处置
        /// </summary>
        public AssetDisposal AssetDisposal
        {
            get { return GetRefEntity(AssetDisposalProperty); }
            set { SetRefEntity(AssetDisposalProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 序列号 Code
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> CodeProperty = P<AssetDisposalFixture>.RegisterView(e => e.Code, p => p.FixtureAccount.Code);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
        }
        #endregion

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<AssetDisposalFixture>.RegisterView(e => e.EncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<AssetDisposalFixture>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<AssetDisposalFixture>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 工治具类型 FixtureTypeCode
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeCodeProperty = P<AssetDisposalFixture>.RegisterView(e => e.FixtureTypeCode, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
        }
        #endregion

        #region 固定资产编码 FixedAssetsAccountCode
        /// <summary>
        /// 固定资产编码
        /// </summary>
        [Label("固定资产编码")]
        public static readonly Property<string> FixedAssetsAccountCodeProperty
            = P<AssetDisposalFixture>.RegisterView(e => e.FixedAssetsAccountCode, p => p.FixtureAccount.FixedAssetsAccount.Code);

        /// <summary>
        /// 固定资产编码
        /// </summary>
        public string FixedAssetsAccountCode
        {
            get { return this.GetProperty(FixedAssetsAccountCodeProperty); }
        }
        #endregion

        #region 固定资产名称 FixedAssetsAccountName
        /// <summary>
        /// 固定资产名称
        /// </summary>
        [Label("固定资产名称")]
        public static readonly Property<string> FixedAssetsAccountNameProperty
            = P<AssetDisposalFixture>.RegisterView(e => e.FixedAssetsAccountName, p => p.FixtureAccount.FixedAssetsAccount.Name);

        /// <summary>
        /// 固定资产名称
        /// </summary>
        public string FixedAssetsAccountName
        {
            get { return this.GetProperty(FixedAssetsAccountNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<AssetDisposalFixture>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetDisposalFixture>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 报废类型 ScrapType
        /// <summary>
        /// 报废类型
        /// </summary>
        [Label("报废类型")]
        public static readonly Property<string> ScrapTypeProperty = P<AssetDisposalFixture>.Register(e => e.ScrapType);

        /// <summary>
        /// 报废类型
        /// </summary>
        public string ScrapType
        {
            get { return GetProperty(ScrapTypeProperty); }
            set { SetProperty(ScrapTypeProperty, value); }
        }
        #endregion

        #region 报废原因 Reason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ReasonProperty = P<AssetDisposalFixture>.Register(e => e.Reason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string Reason
        {
            get { return GetProperty(ReasonProperty); }
            set { SetProperty(ReasonProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 资产处置工治具清单 实体配置
    /// </summary>
    internal class AssetScrapFixtureConfig : EntityConfig<AssetDisposalFixture>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ASET_DSPO_FIX").MapAllProperties();
            Meta.Property(AssetDisposalFixture.WarehouseIdProperty).DontMapColumn();
            Meta.Property(AssetDisposalFixture.WarehouseProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}