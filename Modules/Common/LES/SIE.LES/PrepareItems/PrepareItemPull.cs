using SIE.Domain;
using SIE.LES.LinesideWarehouses;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.LES
{
    /// <summary>
    /// 备料模式拉式
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PrepareItemPullCriteria))]
    [Label("备料模式拉式")]
    public partial class PrepareItemPull : BasePrepareItem
    {
        #region 最高存量 MaxStock
        /// <summary>
        /// 最高存量
        /// </summary>
        [Label("最高存量")]
        [MinValue(0)]
        public static readonly Property<decimal?> MaxStockProperty = P<PrepareItemPull>.Register(e => e.MaxStock);

        /// <summary>
        /// 最高存量
        /// </summary>
        public decimal? MaxStock
        {
            get { return GetProperty(MaxStockProperty); }
            set { SetProperty(MaxStockProperty, value); }
        }
        #endregion

        #region 安全水位 LowestStage
        /// <summary>
        /// 安全水位
        /// </summary>
        [Label("安全水位")]
        [MinValue(0)]
        public static readonly Property<decimal?> LowestStageProperty = P<PrepareItemPull>.Register(e => e.LowestStage);

        /// <summary>
        /// 安全水位
        /// </summary>
        public decimal? LowestStage
        {
            get { return GetProperty(LowestStageProperty); }
            set { SetProperty(LowestStageProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("备料接收仓库")]
        [Required]
        public static readonly IRefIdProperty WarehouseIdProperty = P<PrepareItemPull>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<PrepareItemPull>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion


        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<PrepareItemPull>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 备料模式拉式 实体配置
    /// </summary>
    internal class PrepareItemPullConfig : EntityConfig<PrepareItemPull>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PREPARE_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}