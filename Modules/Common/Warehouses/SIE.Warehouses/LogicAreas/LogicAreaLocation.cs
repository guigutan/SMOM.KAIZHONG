using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 逻辑分区与库位关系表
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("逻辑分区与库位关系表")]
	public partial class LogicAreaLocation : DataEntity
	{
		#region 库位 StorageLocation
		/// <summary>
		/// 库位Id
		/// </summary>
		public static readonly IRefIdProperty StorageLocationIdProperty = P<LogicAreaLocation>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

		/// <summary>
		/// 库位Id
		/// </summary>
		public double StorageLocationId
		{
			get { return (double)GetRefId(StorageLocationIdProperty); }
			set { SetRefId(StorageLocationIdProperty, value); }
		}

		/// <summary>
		/// 库位
		/// </summary>
		public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<LogicAreaLocation>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

		/// <summary>
		/// 库位
		/// </summary>
		public StorageLocation StorageLocation
		{
			get { return GetRefEntity(StorageLocationProperty); }
			set { SetRefEntity(StorageLocationProperty, value); }
		}
		#endregion

		#region 逻辑分区 LogicArea
		/// <summary>
		/// 逻辑分区Id
		/// </summary>
		public static readonly IRefIdProperty LogicAreaIdProperty = P<LogicAreaLocation>.RegisterRefId(e => e.LogicAreaId, ReferenceType.Normal);

		/// <summary>
		/// 逻辑分区Id
		/// </summary>
		public double LogicAreaId
		{
			get { return (double)GetRefId(LogicAreaIdProperty); }
			set { SetRefId(LogicAreaIdProperty, value); }
		}

		/// <summary>
		/// 逻辑分区
		/// </summary>
		public static readonly RefEntityProperty<LogicArea> LogicAreaProperty = P<LogicAreaLocation>.RegisterRef(e => e.LogicArea, LogicAreaIdProperty);

		/// <summary>
		/// 逻辑分区
		/// </summary>
		public LogicArea LogicArea
		{
			get { return GetRefEntity(LogicAreaProperty); }
			set { SetRefEntity(LogicAreaProperty, value); }
		}
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<LogicAreaLocation>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<LogicAreaLocation>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<LogicAreaLocation>.RegisterView(e => e.WarehouseCode, p => p.StorageLocation.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 仓库 WarehouseName
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<LogicAreaLocation>.RegisterView(e => e.WarehouseName, p => p.StorageLocation.Warehouse.Name);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 库区 AreaCode
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> AreaCodeProperty = P<LogicAreaLocation>.RegisterView(e => e.AreaCode, p => p.StorageLocation.Area.Code);

        /// <summary>
        /// 库区
        /// </summary>
        public string AreaCode
        {
            get { return this.GetProperty(AreaCodeProperty); }
        }
        #endregion

        #region 是否立库 IsAutomatedArea
        /// <summary>
        /// 是否立库
        /// </summary>
        [Label("是否立库")]
        public static readonly Property<bool> IsAutomatedAreaProperty = P<LogicAreaLocation>.RegisterView(e => e.IsAutomatedArea, p => p.StorageLocation.Area.IsAutomatedArea);

        /// <summary>
        /// 是否立库
        /// </summary>
        public bool IsAutomatedArea
        {
            get { return this.GetProperty(IsAutomatedAreaProperty); }
        }
        #endregion
    }

	/// <summary>
	/// 逻辑分区与库位关系表 实体配置
	/// </summary>
	internal class LogicAreaLocationConfig : EntityConfig<LogicAreaLocation>
	{
		protected override void ConfigMeta()
		{
			Meta.MapTable("WH_LOGCI_AREA_LOC").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}