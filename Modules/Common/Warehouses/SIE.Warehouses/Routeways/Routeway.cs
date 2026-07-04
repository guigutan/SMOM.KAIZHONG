using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 巷道基础数据表
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig))]
    [ConditionQueryType(typeof(RoutewayCriteria))]
    [Label("巷道")]
    [DisplayMember(nameof(Name))]
    public partial class Routeway : DataEntity
    {
        #region 巷道代码（编号_库区_仓库编号） Code
        /// <summary>
        /// 巷道代码（编号_库区_仓库编号）
        /// </summary>
        [MaxLength(20)]
        [NotDuplicate]
        [Label("编码")]
        [Required]
        public static readonly Property<string> CodeProperty = P<Routeway>.Register(e => e.Code);

        /// <summary>
        /// 巷道代码（编号_库区_仓库编号）
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 巷道名称 Name
        /// <summary>
        /// 巷道名称
        /// </summary>
        [MaxLength(80)]
        [NotDuplicate]
        [Label("名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<Routeway>.Register(e => e.Name);

        /// <summary>
        /// 巷道名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 巷道所属巷道号 Routeway_Number
        /// <summary>
        /// 所属巷道号
        /// </summary>
        [Label("所属巷道号")]
        [MinValue(1)]
        public static readonly Property<int> RoutewayNumberProperty = P<Routeway>.Register(e => e.RoutewayNumber);

        /// <summary>
        /// 巷道所属巷道号
        /// </summary>
        public int RoutewayNumber
        {
            get { return GetProperty(RoutewayNumberProperty); }
            set { SetProperty(RoutewayNumberProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        [MaxLength(1000)]
        public static readonly Property<string> DescriptionProperty = P<Routeway>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty =
            P<Routeway>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double StorageAreaId
        {
            get { return (double)this.GetRefId(StorageAreaIdProperty); }
            set { this.SetRefId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty =
            P<Routeway>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return this.GetRefEntity(StorageAreaProperty); }
            set { this.SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>     
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<Routeway>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>		
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<Routeway>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<Routeway>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 库区编码 StorageAreaCode
        /// <summary>
        /// 库区编码
        /// </summary>
        [Label("库区编码")]
        public static readonly Property<string> StorageAreaCodeProperty = P<Routeway>.RegisterView(e => e.StorageAreaCode, p => p.StorageArea.Code);

        /// <summary>
        /// 库区编码
        /// </summary>
        public string StorageAreaCode
        {
            get { return this.GetProperty(StorageAreaCodeProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 巷道基础数据表 实体配置
    /// </summary>
    internal class RoutewayConfig : EntityConfig<Routeway>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("WCS_ROUTEWAY").MapAllProperties();
            Meta.Property(Routeway.CodeProperty).ColumnMeta.HasLength(40);
            Meta.Property(Routeway.NameProperty).ColumnMeta.HasLength(160);
            Meta.Property(Routeway.DescriptionProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}