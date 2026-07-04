using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 工作区与库位关系
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工作区与库位关系")]
    public partial class WorkAreaLocation : DataEntity
    {
        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        public static readonly IRefIdProperty StorageLocationIdProperty = P<WorkAreaLocation>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<WorkAreaLocation>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 编码 StorageLocationCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<WorkAreaLocation>.RegisterView(e => e.StorageLocationCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
        }
        #endregion

        #region 名称 StorageLocationName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<WorkAreaLocation>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 仓库 WarehouseCode
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseCodeProperty = P<WorkAreaLocation>.RegisterView(e => e.WarehouseCode, p => p.StorageLocation.Warehouse.Code);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 库区 AreaCode
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> AreaCodeProperty = P<WorkAreaLocation>.RegisterView(e => e.AreaCode, p => p.StorageLocation.Area.Code);

        /// <summary>
        /// 库区
        /// </summary>
        public string AreaCode
        {
            get { return this.GetProperty(AreaCodeProperty); }
        }
        #endregion

        #region 状态 StorageLocationState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StorageLocationStateProperty = P<WorkAreaLocation>.RegisterView(e => e.StorageLocationState, p => p.StorageLocation.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State StorageLocationState
        {
            get { return this.GetProperty(StorageLocationStateProperty); }
        }
        #endregion

        #region  WorkArea
        /// <summary>
        /// Id
        /// </summary>
        public static readonly IRefIdProperty WorkAreaIdProperty = P<WorkAreaLocation>.RegisterRefId(e => e.WorkAreaId, ReferenceType.Parent);

        /// <summary>
        /// Id
        /// </summary>
        public double WorkAreaId
        {
            get { return (double)GetRefId(WorkAreaIdProperty); }
            set { SetRefId(WorkAreaIdProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<WorkArea> WorkAreaProperty = P<WorkAreaLocation>.RegisterRef(e => e.WorkArea, WorkAreaIdProperty);

        /// <summary>
        /// 
        /// </summary>
        public WorkArea WorkArea
        {
            get { return GetRefEntity(WorkAreaProperty); }
            set { SetRefEntity(WorkAreaProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class WorkAreaLocationConfig : EntityConfig<WorkAreaLocation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_WORKAREA_LOC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}