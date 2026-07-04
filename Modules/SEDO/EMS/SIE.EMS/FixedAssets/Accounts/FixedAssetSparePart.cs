using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.FixedAssets.Accounts
{
	/// <summary>
	/// 固定资产备件清单
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("备件清单")]
	public partial class FixedAssetSparePart : DataEntity
	{
		#region 序列号 StoreSummaryDetail
		/// <summary>
		/// 序列号Id
		/// </summary>
		[Label("序列号")]
		public static readonly IRefIdProperty StoreSummaryDetailIdProperty = P<FixedAssetSparePart>.RegisterRefId(e => e.StoreSummaryDetailId, ReferenceType.Normal);

		/// <summary>
		/// 序列号Id
		/// </summary>
		public double StoreSummaryDetailId
		{
			get { return (double)GetRefId(StoreSummaryDetailIdProperty); }
			set { SetRefId(StoreSummaryDetailIdProperty, value); }
		}

		/// <summary>
		/// 序列号
		/// </summary>
		public static readonly RefEntityProperty<StoreSummaryDetail> StoreSummaryDetailProperty = P<FixedAssetSparePart>.RegisterRef(e => e.StoreSummaryDetail, StoreSummaryDetailIdProperty);

		/// <summary>
		/// 序列号
		/// </summary>
		public StoreSummaryDetail StoreSummaryDetail
		{
			get { return GetRefEntity(StoreSummaryDetailProperty); }
			set { SetRefEntity(StoreSummaryDetailProperty, value); }
		}
        #endregion

        #region 固定资产台账 FixedAssetsAccount
        /// <summary>
        /// 固定资产台账Id
        /// </summary>
        [Label("固定资产台账")]
		public static readonly IRefIdProperty FixedAssetsAccountIdProperty = P<FixedAssetSparePart>.RegisterRefId(e => e.FixedAssetsAccountId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<FixedAssetsAccount> FixedAssetsAccountProperty = P<FixedAssetSparePart>.RegisterRef(e => e.FixedAssetsAccount, FixedAssetsAccountIdProperty);

        /// <summary>
        /// 固定资产台账
        /// </summary>
        public FixedAssetsAccount FixedAssetsAccount
		{
			get { return GetRefEntity(FixedAssetsAccountProperty); }
			set { SetRefEntity(FixedAssetsAccountProperty, value); }
		}
		#endregion

		#region 主备件 IsMajor
		/// <summary>
		/// 主备件
		/// </summary>
		[Label("主备件")]
		public static readonly Property<bool> IsMajorProperty = P<FixedAssetSparePart>.Register(e => e.IsMajor);

		/// <summary>
		/// 主备件
		/// </summary>
		public bool IsMajor
		{
			get { return GetProperty(IsMajorProperty); }
			set { SetProperty(IsMajorProperty, value); }
		}
        #endregion

        #region 视图属性

        #region 序列号 OrderNumberCode
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> OrderNumberCodeProperty = P<FixedAssetSparePart>.RegisterView(e => e.OrderNumberCode, p => p.StoreSummaryDetail.OrderNumberCode);

        /// <summary>
        /// 序列号
        /// </summary>
        public string OrderNumberCode
        {
            get { return this.GetProperty(OrderNumberCodeProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<FixedAssetSparePart>.RegisterView(e => e.SparePartCode, p => p.StoreSummaryDetail.StoreSummary.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
		#endregion

		#region 备件名称 SparePartName
		/// <summary>
		/// 备件名称
		/// </summary>
		[Label("备件名称")]
		public static readonly Property<string> SparePartNameProperty = P<FixedAssetSparePart>.RegisterView(e => e.SparePartName, p => p.StoreSummaryDetail.StoreSummary.SparePart.SparePartName);

		/// <summary>
		/// 备件名称
		/// </summary>
		public string SparePartName
		{
			get { return this.GetProperty(SparePartNameProperty); }
		}
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<FixedAssetSparePart>.RegisterView(e => e.State, p => p.StoreSummaryDetail.StoreSummary.SparePart.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<FixedAssetSparePart>.RegisterView(e => e.Specification, p => p.StoreSummaryDetail.StoreSummary.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
		{
            get { return this.GetProperty(SpecificationProperty); }
        }
        #endregion

        #region 备件类型 SpartType
        /// <summary>
        /// 备件类型
        /// </summary>
        [Label("备件类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<FixedAssetSparePart>.RegisterView(e => e.SpartType, p => p.StoreSummaryDetail.StoreSummary.SparePart.SpartType);

        /// <summary>
        /// 备件类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<FixedAssetSparePart>.RegisterView(e => e.UnitName, p => p.StoreSummaryDetail.StoreSummary.SparePart.Unit.Name);

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
        public static readonly Property<string> WarehouseNameProperty = P<FixedAssetSparePart>.RegisterView(e => e.WarehouseName, p => p.StoreSummaryDetail.Warehouse.Name);

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
        public static readonly Property<string> StorageLocationNameProperty = P<FixedAssetSparePart>.RegisterView(e => e.StorageLocationName, p => p.StoreSummaryDetail.StorageLocation.Name);

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
        public static readonly Property<string> SupplierCodeProperty = P<FixedAssetSparePart>.RegisterView(e => e.SupplierCode, p => p.StoreSummaryDetail.Supplier.Code);

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
        public static readonly Property<string> SupplierNameProperty = P<FixedAssetSparePart>.RegisterView(e => e.SupplierName, p => p.StoreSummaryDetail.Supplier.Name);

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
	/// 固定资产备件清单 实体配置
	/// </summary>
	internal class FixedAssetSparePartConfig : EntityConfig<FixedAssetSparePart>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_SP").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}