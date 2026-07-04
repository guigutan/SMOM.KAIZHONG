using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.ERPInterface.Smom.InventoryControl
{
    /// <summary>
    /// 库存对照表查询实体 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("库存对照表查询实体")]
    public class InventoryControlViewModelCriteria : Criteria
    {
        public InventoryControlViewModelCriteria()
        {
            IsShowDifferent = true;
            IsShowZero = true;
        }

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<InventoryControlViewModelCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<InventoryControlViewModelCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库 ErpWarehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty ErpWarehouseIdProperty = P<InventoryControlViewModelCriteria>.RegisterRefId(e => e.ErpWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? ErpWarehouseId
        {
            get { return (double?)GetRefNullableId(ErpWarehouseIdProperty); }
            set { SetRefNullableId(ErpWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<ErpWarehouse> ErpWarehouseProperty = P<InventoryControlViewModelCriteria>.RegisterRef(e => e.ErpWarehouse, ErpWarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public ErpWarehouse ErpWarehouse
        {
            get { return GetRefEntity(ErpWarehouseProperty); }
            set { SetRefEntity(ErpWarehouseProperty, value); }
        }
        #endregion

        #region 仓库 WarehouseCode
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseCodeProperty = P<InventoryControlViewModelCriteria>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region ERP子库 ErpWarehouseCode
        /// <summary>
        /// ERP子库
        /// </summary>
        [Label("ERP子库")]
        public static readonly Property<string> ErpWarehouseCodeProperty = P<InventoryControlViewModelCriteria>.Register(e => e.ErpWarehouseCode);

        /// <summary>
        /// ERP子库
        /// </summary>
        public string ErpWarehouseCode
        {
            get { return this.GetProperty(ErpWarehouseCodeProperty); }
            set { this.SetProperty(ErpWarehouseCodeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<InventoryControlViewModelCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<InventoryControlViewModelCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region ERP批次号 ErpLotCode
        /// <summary>
        /// ERP批次号
        /// </summary>
        [Label("ERP批次号")]
        public static readonly Property<string> ErpLotCodeProperty = P<InventoryControlViewModelCriteria>.Register(e => e.ErpLotCode);

        /// <summary>
        /// ERP批次号
        /// </summary>
        public string ErpLotCode
        {
            get { return this.GetProperty(ErpLotCodeProperty); }
            set { this.SetProperty(ErpLotCodeProperty, value); }
        }
        #endregion

        #region 只显示差异库存 IsShowDifferent
        /// <summary>
        /// 只显示差异库存
        /// </summary>
        [Label("只显示差异库存")]
        public static readonly Property<bool?> IsShowDifferentProperty = P<InventoryControlViewModelCriteria>.Register(e => e.IsShowDifferent);

        /// <summary>
        /// 只显示差异库存
        /// </summary>
        public bool? IsShowDifferent
        {
            get { return this.GetProperty(IsShowDifferentProperty); }
            set { this.SetProperty(IsShowDifferentProperty, value); }
        }
        #endregion

        #region 不含0库存 NoShowZero
        /// <summary>
        /// 不含0库存
        /// </summary>
        [Label("不含0库存")]
        public static readonly Property<bool?> NoShowZeroProperty = P<InventoryControlViewModelCriteria>.Register(e => e.IsShowZero);

        /// <summary>
        /// 不含0库存
        /// </summary>
        public bool? IsShowZero
        {
            get { return this.GetProperty(NoShowZeroProperty); }
            set { this.SetProperty(NoShowZeroProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <returns>返回结果</returns>
        protected override EntityList Fetch()
        {
            var list = new EntityList<InventoryControlViewModel>();
            //list.AddRange(RT.Service.Resolve<StatisticsController>().GetInventoryControlViewModelData(this));
            //var data = RT.Service.Resolve<StatisticsController>().GetInventoryControlViewModelData(this);
            return list;
        }
    }
}
