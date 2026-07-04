using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ShipPlan.ViewModels
{
    /// <summary>
    /// 备料计划/发货计划齐套分析-预分配明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("预分配明细")]
    public class StockPlanAssignViewModel : ViewModel
    {
        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("分配数量")]
        public static readonly Property<decimal> QtyProperty = P<StockPlanAssignViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<StockPlanAssignViewModel>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<StockPlanAssignViewModel>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("分配批次")]
        public static readonly Property<string> LotCodeProperty = P<StockPlanAssignViewModel>.Register(e => e.LotCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 备料计划 StockPlan
        /// <summary>
        /// 备料计划Id
        /// </summary>
        public static readonly IRefIdProperty StockPlanIdProperty = P<StockPlanAssignViewModel>.RegisterRefId(e => e.StockPlanId, ReferenceType.Parent);

        /// <summary>
        /// 备料计划Id
        /// </summary>
        public double StockPlanId
        {
            get { return (double)GetRefId(StockPlanIdProperty); }
            set { SetRefId(StockPlanIdProperty, value); }
        }

        /// <summary>
        /// 备料计划
        /// </summary>
        public static readonly RefEntityProperty<DeliveryPlan> StockOrderProperty = P<StockPlanAssignViewModel>.RegisterRef(e => e.StockPlan, StockPlanIdProperty);

        /// <summary>
        /// 备料计划
        /// </summary>
        public DeliveryPlan StockPlan
        {
            get { return GetRefEntity(StockOrderProperty); }
            set { SetRefEntity(StockOrderProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StockPlanAssignViewModel>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<StockPlanAssignViewModel>.Register(e => e.WarehouseName);

        /// <summary>
        /// 批次号
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion
    }
}
