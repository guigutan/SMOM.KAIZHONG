using SIE.Domain;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 固定资产工治具清单
    /// </summary>
    [ChildEntity, Serializable]
	[Label("工治具清单")]
	public partial class FixedAssetFixtureBill : DataEntity
	{
        #region 工治具ID台账 FixtureIDAccount
        /// <summary>
        /// 工治具ID台账Id
        /// </summary>
        [Label("工治具ID")]
		public static readonly IRefIdProperty FixtureIDAccountIdProperty = P<FixedAssetFixtureBill>.RegisterRefId(e => e.FixtureIDAccountId, ReferenceType.Normal);

        /// <summary>
        /// 工治具ID台账Id
        /// </summary>
        public double FixtureIDAccountId
		{
			get { return (double)GetRefId(FixtureIDAccountIdProperty); }
			set { SetRefId(FixtureIDAccountIdProperty, value); }
		}

        /// <summary>
        /// 工治具ID台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureIDAccount> FixtureIDAccountProperty = P<FixedAssetFixtureBill>.RegisterRef(e => e.FixtureIDAccount, FixtureIDAccountIdProperty);

        /// <summary>
        /// 工治具ID台账
        /// </summary>
        public FixtureIDAccount FixtureIDAccount
		{
			get { return GetRefEntity(FixtureIDAccountProperty); }
			set { SetRefEntity(FixtureIDAccountProperty, value); }
		}
        #endregion

        #region 固定资产台账 FixedAssetsAccount
        /// <summary>
        /// 固定资产台账Id
        /// </summary>
        [Label("固定资产台账")]
		public static readonly IRefIdProperty FixedAssetsAccountIdProperty = P<FixedAssetFixtureBill>.RegisterRefId(e => e.FixedAssetsAccountId, ReferenceType.Parent);

        /// <summary>
        /// 固定资产台账Id
        /// </summary>
        public double FixedAssetsAccountId
		{
			get { return (double)GetRefId(FixedAssetsAccountIdProperty); }
			set { SetRefId(FixedAssetsAccountIdProperty, value); }
		}

        /// <summary>
        /// 固定资产台账
        /// </summary>
        public static readonly RefEntityProperty<FixedAssetsAccount> FixedAssetsAccountProperty = P<FixedAssetFixtureBill>.RegisterRef(e => e.FixedAssetsAccount, FixedAssetsAccountIdProperty);

        /// <summary>
        /// 固定资产台账
        /// </summary>
        public FixedAssetsAccount FixedAssetsAccount
		{
			get { return GetRefEntity(FixedAssetsAccountProperty); }
			set { SetRefEntity(FixedAssetsAccountProperty, value); }
		}
		#endregion

		#region 主工治具 IsMajor
		/// <summary>
		/// 主工治具
		/// </summary>
		[Label("主工治具")]
		public static readonly Property<bool> IsMajorProperty = P<FixedAssetFixtureBill>.Register(e => e.IsMajor);

		/// <summary>
		/// 主工治具
		/// </summary>
		public bool IsMajor
		{
			get { return GetProperty(IsMajorProperty); }
			set { SetProperty(IsMajorProperty, value); }
		}
        #endregion

        #region 视图属性

        #region 工治具ID Code
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> CodeProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.Code, p => p.FixtureIDAccount.Code);

        /// <summary>
        /// 工治具ID
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
        public static readonly Property<string> EncodeCodeProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.EncodeCode, p => p.FixtureIDAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
        }
        #endregion

        #region 状态 AccountState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<FixtureAccountState> AccountStateProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.AccountState, p => p.FixtureIDAccount.AccountState);

        /// <summary>
        /// 状态
        /// </summary>
        public FixtureAccountState AccountState
        {
            get { return this.GetProperty(AccountStateProperty); }
            set { SetProperty(AccountStateProperty, value); }
        }
        #endregion

        #region 工治具型号 ModelCode
        /// <summary>
        /// 工治具型号
        /// </summary>
        [Label("工治具型号")]
        public static readonly Property<string> ModelCodeProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.ModelCode, p => p.FixtureIDAccount.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 工治具型号
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
        public static readonly Property<string> ModelNameProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.ModelName, p => p.FixtureIDAccount.FixtureEncode.FixtureModel.Name);

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
        public static readonly Property<string> FixtureTypeCodeProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.FixtureTypeCode, p => p.FixtureIDAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.UnitName, p => p.FixtureIDAccount.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 仓库 WarehouseName
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.WarehouseName, p => p.FixtureIDAccount.Warehouse.Name);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 库位 StorageLocationName
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationNameProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.StorageLocationName, p => p.FixtureIDAccount.Location.Name);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.SupplierCode, p => p.FixtureIDAccount.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<FixedAssetFixtureBill>.RegisterView(e => e.SupplierName, p => p.FixtureIDAccount.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 固定资产工治具清单 实体配置
    /// </summary>
    internal class FixedAssetFixtureBillConfig : EntityConfig<FixedAssetFixtureBill>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_FIX").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}