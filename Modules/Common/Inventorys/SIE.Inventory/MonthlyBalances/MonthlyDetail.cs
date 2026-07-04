using SIE.Core.Enums;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.Inventory.TransactionProcessing;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using SIE.Common.InvOrg;

namespace SIE.WMS.Statistics.MonthlyBalances
{
    /// <summary>
    /// 月结库存汇总
    /// </summary>
    [RootEntity, Serializable]
    public class MonthlyDetail : Entity<double>
    {
        #region 账期 AccountPeriod
        /// <summary>
        /// 账期
        /// </summary>
        [Label("账期")]
        public static readonly Property<string> AccountPeriodProperty = P<MonthlyDetail>.Register(e => e.AccountPeriod);

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
        public static readonly Property<string> StorerCodeProperty = P<MonthlyDetail>.Register(e => e.StorerCode);

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
        public static readonly IRefIdProperty WarehouseIdProperty = P<MonthlyDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<MonthlyDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

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
        public static readonly IRefIdProperty ItemIdProperty = P<MonthlyDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<MonthlyDetail>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<OrderType> OrderTypeProperty = P<MonthlyDetail>.Register(e => e.OrderType);

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
        public static readonly Property<decimal> QtyProperty = P<MonthlyDetail>.Register(e => e.Qty);

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
        internal class MonthlyDetailConfig : EntityConfig<MonthlyDetail>
        {
            /// <summary>
            /// 配置元数据
            /// </summary>
            protected override void ConfigMeta()
            {
                var trans = RF.Find<InvTransaction>().EntityMeta;
                var transaction_date = trans.Property(InvTransaction.TransactionDateProperty).ColumnMeta.ColumnName;
                var order_type = trans.Property(InvTransaction.OrderTypeProperty).ColumnMeta.ColumnName;
                var item_id = trans.Property(InvTransaction.ItemIdProperty).ColumnMeta.ColumnName;
                var create_by = trans.Property(InvTransaction.CreateByProperty).ColumnMeta.ColumnName;
                var qty = trans.Property(InvTransaction.QtyProperty).ColumnMeta.ColumnName;
                var storer_code = trans.Property(InvTransaction.StorerCodeProperty).ColumnMeta.ColumnName;
                var from_warehouse_id = trans.Property(InvTransaction.FromWarehouseIdProperty).ColumnMeta.ColumnName;

                var to_warehouse_id = trans.Property(InvTransaction.ToWarehouseIdProperty).ColumnMeta.ColumnName;
                string isphantom = trans.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
                var invOrgId = trans.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
                if (RT.IsOnServer() || RT.IsDebuggingEnabled)
                {
                    var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<InvTransaction>()).DbSetting;

                    Meta.EnablePhantoms();
                    Meta.EnableInvOrg();
                    if (setting.IsSqlserverDbServer())
                    {

                        var view = @"(SELECT t.id,t.{1},t.{2} warehouse_id,t.{4},t.{5},convert(char(6), t.{6}, 112) Account_Period,t.{7},t.{8},{9},{10}
								  FROM {0} t
								 WHERE t.to_warehouse_id IS NOT NULL and {9} =0
								UNION ALL
								SELECT t.id,t.{1},t.{3} warehouse_id ,t.{4},t.{5} ,convert(char(6), t.{6}, 112) Account_Period,-t.{7},t.{8},{9},{10}
								  FROM {0} t
								 WHERE t.from_warehouse_id IS NOT NULL  and {9} =0)".FormatArgs(trans.TableMeta.TableName,
                                     storer_code, to_warehouse_id, from_warehouse_id, item_id, order_type, transaction_date, qty, create_by, isphantom, invOrgId);

                        Meta.MapView(view).MapAllProperties();
                    }
                    else
                    {
                        var view = @"(SELECT t.id,t.{1},t.{2} warehouse_id,t.{4},t.{5},to_char(t.{6},'yyyymm') Account_Period,t.{7},t.{8},{9},{10}
								  FROM {0} t
								 WHERE t.to_warehouse_id IS NOT NULL and {9} =0
								UNION ALL
								SELECT t.id,t.{1},t.{3} warehouse_id ,t.{4},t.{5} ,to_char(t.{6},'yyyymm') Account_Period,-t.{7},t.{8},{9},{10}
								  FROM {0} t
								 WHERE t.from_warehouse_id IS NOT NULL  and {9} =0)".FormatArgs(trans.TableMeta.TableName,
                                         storer_code, to_warehouse_id, from_warehouse_id, item_id, order_type, transaction_date, qty, create_by, isphantom, invOrgId);
                        Meta.MapView(view).MapAllProperties();
                    }
                }
            }
        }
    }
}
