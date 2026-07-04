using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Core.Enums;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.WMS.Statistics.MonthlyBalances
{
    /// <summary>
    /// 月结库存汇总
    /// </summary>
    [RootEntity, Serializable]
    public class MonthlySummary : Entity<double>
    {
        #region 账期 AccountPeriod
        /// <summary>
        /// 账期
        /// </summary>
        [Label("账期")]
        public static readonly Property<string> AccountPeriodProperty = P<MonthlySummary>.Register(e => e.AccountPeriod);

        /// <summary>
        /// 账期
        /// </summary>
        public string AccountPeriod
        {
            get { return GetProperty(AccountPeriodProperty); }
            set { SetProperty(AccountPeriodProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<MonthlySummary>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<MonthlySummary>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<MonthlySummary>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<MonthlySummary>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<MonthlySummary>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<MonthlySummary>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<MonthlySummary>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        /// <summary>
        /// 配置元数据
        /// </summary>
        internal class MonthlySummaryConfig : EntityConfig<MonthlySummary>
        {
            /// <summary>
            /// 配置元数据
            /// </summary>
            protected override void ConfigMeta()
            {
                Func<IQuery> view = () => DB.Query<MonthlyDetail>()
                .Select(p => new
                {
                    id = p.Id.MAX(),
                    Storer_Code = p.StorerCode,
                    Warehouse_Id = p.WarehouseId,
                    Item_Id = p.ItemId,
                    Order_Type = p.OrderType,
                    Account_Period = p.AccountPeriod,
                    Qty = p.Qty.SUM()
                })
                .GroupBy(p => new { p.StorerCode, p.WarehouseId, p.ItemId, p.OrderType, p.AccountPeriod, })

                .ToQuery();

                Meta.MapView(view).MapAllProperties();
            }
        }
    }
}
