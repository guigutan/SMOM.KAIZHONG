using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// ERP订单
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("ERP订单")]
    [DisplayMember(nameof(No))]
    public partial class ErpSaleOrder : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ErpSaleOrder()
        {
            OrderDate = DateTime.Now;
            DeliveryDate = DateTime.Now;
            OrderDate = DateTime.Today;
            DeliveryDate = DateTime.Today;
        }

        #endregion

        #region 订单号 No
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("订单号")]
        public static readonly Property<string> NoProperty = P<ErpSaleOrder>.Register(e => e.No);

        /// <summary>
        /// 订单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty =
            P<ErpSaleOrder>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double CustomerId
        {
            get { return (double)this.GetRefId(CustomerIdProperty); }
            set { this.SetRefId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty =
            P<ErpSaleOrder>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return this.GetRefEntity(CustomerProperty); }
            set { this.SetRefEntity(CustomerProperty, value); }
        }
        #endregion 

        #region 订单日期 OrderDate
        /// <summary>
        /// 订单日期
        /// </summary>
        [Label("订单日期")]
        public static readonly Property<DateTime> OrderDateProperty = P<ErpSaleOrder>.Register(e => e.OrderDate);

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate
        {
            get { return GetProperty(OrderDateProperty); }
            set { SetProperty(OrderDateProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateTime> DeliveryDateProperty = P<ErpSaleOrder>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate
        {
            get { return GetProperty(DeliveryDateProperty); }
            set { SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<ErpSaleOrder>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// ERP订单 实体配置
    /// </summary>
    internal class ErpOrderConfig : EntityConfig<ErpSaleOrder>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ERP_SALE_ORDER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}