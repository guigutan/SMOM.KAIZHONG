using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.SO.SaleOrders
{

    /// <summary>
    /// 销售明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("销售明细")]
    public class SaleOrderDetailViewModel : Entity<double>
    {

        #region 销售订单 SaleOrderId
        /// <summary>
        /// 销售订单SaleOrderId
        /// </summary>
        public static readonly IRefIdProperty SaleOrderIdProperty = P<SaleOrderDetailViewModel>.RegisterRefId(e => e.SaleOrderId, ReferenceType.Parent);
        /// <summary>
        /// 销售订单Id
        /// </summary>
        public double SaleOrderId
        {
            get { return (double)GetRefId(SaleOrderIdProperty); }
            set { SetRefId(SaleOrderIdProperty, value); }
        }
        #endregion

        #region 销售订单 SaleOrder
        /// <summary>
        /// 销售订单
        /// </summary>
        public static readonly RefEntityProperty<SaleOrder> SaleOrderProperty = P<SaleOrderDetailViewModel>.RegisterRef(e => e.SaleOrder, SaleOrderIdProperty);

        /// <summary>
        /// 销售订单
        /// </summary>
        public SaleOrder SaleOrder
        {
            get { return GetRefEntity(SaleOrderProperty); }
            set { SetRefEntity(SaleOrderProperty, value); }
        }
        #endregion

        #region 销售订单编码 SalesOrderCode
        /// <summary>
        /// 销售订单编码
        /// </summary>
        [Label("销售订单编码")]
        public static readonly Property<string> SaleOrderCodeProperty = P<SaleOrderDetailViewModel>.RegisterView(e => e.SaleOrderCode, p => p.SaleOrder.Code);

        /// <summary>
        /// 销售订单编码
        /// </summary>
        public string SaleOrderCode
        {
            get { return this.GetProperty(SaleOrderCodeProperty); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<SaleOrderDetailViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<SaleOrderDetailViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SaleOrderDetailViewModel>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<SaleOrderDetailViewModel>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 版本号 Version
        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        [Label("版本号")]
        public static readonly Property<string> VersionProperty = P<SaleOrderDetailViewModel>.Register(e => e.Version);

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 库存组织 Enterprise
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public static readonly IRefIdProperty EnterpriseIdProperty = P<SaleOrderDetailViewModel>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 库存组织
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<SaleOrderDetailViewModel>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 库存组织
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 库存组织编码 EnterpriseCode
        /// <summary>
        /// 库存组织编码
        /// </summary>
        [Label("库存组织编码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<SaleOrderDetailViewModel>.RegisterView(e => e.EnterpriseCode, p => p.Enterprise.Code);

        /// <summary>
        /// 库存组织编码
        /// </summary>
        public string EnterpriseCode
        {
            get { return this.GetProperty(EnterpriseCodeProperty); }
        }

        #endregion

        #region 库存组织名称 EnterpriseName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<SaleOrderDetailViewModel>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
        }

        #endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Required]
        [Label("客户交期")]
        public static readonly Property<DateTime> RequireDeliveryProperty = P<SaleOrderDetailViewModel>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<SaleOrderDetailViewModel>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double CustomerId
        {
            get { return (double)GetRefId(CustomerIdProperty); }
            set { SetRefId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<SaleOrderDetailViewModel>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<SaleOrderDetailViewModel>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
        }
        #endregion

        #region 客户名称  CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<SaleOrderDetailViewModel>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion




        #region 订单行号 LineNo
        /// <summary>
        /// 订单行号
        /// </summary>
        [Label("订单行号")]
        public static readonly Property<string> LineNoProperty = P<SaleOrderDetailViewModel>.Register(e => e.LineNo);

        /// <summary>
        /// 订单行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion


        /// <summary>
        ///  实体配置
        /// </summary>
        internal class SalesOrderDetailConfig3 : EntityConfig<SaleOrderDetailViewModel>
        {
            /// <summary>
            /// 配置元数据
            /// </summary>
            protected override void ConfigMeta()
            {
                Func<IQuery> view = () => DB.Query<SaleOrderDetail>("SaleOrderDetail")
                     .LeftJoin<SaleOrder>("SaleOrder", (x, y) => x.SaleOrderId == y.Id)
                     .LeftJoin<Enterprise>("Enterprise", (x, z) => x.EnterpriseId == z.Id)
                     .LeftJoin<Item>("Item", (x, i) => x.ItemId == i.Id)
                     .LeftJoin<SaleOrder, Customer>("Customer", (x, i) => x.CustomerId == i.Id)
                     .Select<SaleOrder, Enterprise, Item, Customer>(
                         (SalesOrderDetail, SalesOrder, Enterprise, Item, Customer) =>
                             new
                             {
                                 SalesOrderDetail.Id,
                                 Sale_Order_Id = SalesOrder.Id,
                                 Line_No = SalesOrderDetail.LineNo,
                                 Sale_Order_Code = SalesOrder.Code,
                                 Item_Id = SalesOrderDetail.ItemId,
                                 Item_Name = Item.Name,
                                 Item_Code = Item.Code,
                                 Version = SalesOrderDetail.Version,
                                 Customer_Id = Customer.Id,
                                 Customer_Code = Customer.Code,
                                 Customer_Name = Customer.Name,
                                 Enterprise_Id = Enterprise.Id,
                                 Enterprise_Code = Enterprise.Code,
                                 Enterprise_Name = Enterprise.Name,
                                 Require_Delivery = SalesOrderDetail.RequireDelivery
                             })
                     .ToQuery();
                Meta.MapView(view).MapAllProperties();
                Meta.IsTreeEntity = false;
            }
        }
    }
}
