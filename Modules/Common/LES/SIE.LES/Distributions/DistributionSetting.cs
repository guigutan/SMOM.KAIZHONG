using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 配送设置
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DistributionSettingCriteria))]
    [Label("配送设置")]
    public class DistributionSetting: DataEntity, IStateEntity
    {
        #region 目标产线 ProductLine
        /// <summary>
        /// 目标产线Id
        /// </summary>
        [Label("目标产线")]
        public static readonly IRefIdProperty ProductLineIdProperty =
            P<DistributionSetting>.RegisterRefId(e => e.ProductLineId, ReferenceType.Normal);

        /// <summary>
        /// 目标产线Id
        /// </summary>
        public double ProductLineId
        {
            get { return (double)this.GetRefId(ProductLineIdProperty); }
            set { this.SetRefId(ProductLineIdProperty, value); }
        }

        /// <summary>
        /// 目标产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ProductLineProperty =
            P<DistributionSetting>.RegisterRef(e => e.ProductLine, ProductLineIdProperty);

        /// <summary>
        /// 目标产线
        /// </summary>
        public WipResource ProductLine
        {
            get { return this.GetRefEntity(ProductLineProperty); }
            set { this.SetRefEntity(ProductLineProperty, value); }
        }
        #endregion

        #region 来源仓库 Warehouse
        /// <summary>
        /// 来源仓库Id
        /// </summary>
        [Label("来源仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<DistributionSetting>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 来源仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 来源仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<DistributionSetting>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 来源仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 集货库区 Area
        /// <summary>
        /// 集货库区Id
        /// </summary>
        [Label("集货库区")]
        public static readonly IRefIdProperty AreaIdProperty =
            P<DistributionSetting>.RegisterRefId(e => e.AreaId, ReferenceType.Normal);

        /// <summary>
        /// 集货库区Id
        /// </summary>
        public double? AreaId
        {
            get { return (double?)this.GetRefNullableId(AreaIdProperty); }
            set { this.SetRefNullableId(AreaIdProperty, value); }
        }

        /// <summary>
        /// 集货库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> AreaProperty =
            P<DistributionSetting>.RegisterRef(e => e.Area, AreaIdProperty);

        /// <summary>
        /// 集货库区    
        /// </summary>
        public StorageArea Area
        {
            get { return this.GetRefEntity(AreaProperty); }
            set { this.SetRefEntity(AreaProperty, value); }
        }
        #endregion

        #region 集货库位 StorageLocation
        /// <summary>
        /// 集货库位Id
        /// </summary>
        [Label("集货库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<DistributionSetting>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 集货库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 集货库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<DistributionSetting>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 集货库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<DistributionSetting>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 配送单管理 实体配置
    /// </summary>
    internal class DistributionSettingConfig : EntityConfig<DistributionSetting>
    {         
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DISTRIBUTION_SETTING").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
