using System;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 发运单中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("发运单中间表")]
    public partial class ShippingOrderInf : DownloadBaseEntity
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<ShippingOrderInf>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 制单人编码 BillerCode
        /// <summary>
        /// 制单人编码
        /// </summary>
        [Label("制单人编码")]
        public static readonly Property<string> BillerCodeProperty = P<ShippingOrderInf>.Register(e => e.BillerCode);

        /// <summary>
        /// 制单人编码
        /// </summary>
        public string BillerCode
        {
            get { return GetProperty(BillerCodeProperty); }
            set { SetProperty(BillerCodeProperty, value); }
        }
        #endregion

        #region 制单日期 BillDate
        /// <summary>
        /// 制单日期
        /// </summary>
        [Label("制单日期")]
        public static readonly Property<DateTime> BillDateProperty = P<ShippingOrderInf>.Register(e => e.BillDate);

        /// <summary>
        /// 制单日期
        /// </summary>
        public DateTime BillDate
        {
            get { return GetProperty(BillDateProperty); }
            set { SetProperty(BillDateProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<ShippingOrderInf>.Register(e => e.CustomerCode);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<ShippingOrderInf>.Register(e => e.SupplierCode);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return GetProperty(SupplierCodeProperty); }
            set { SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 生产部门编码 EnterpriseCode
        /// <summary>
        /// 生产部门编码
        /// </summary>
        [Label("生产部门编码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<ShippingOrderInf>.Register(e => e.EnterpriseCode);

        /// <summary>
        /// 生产部门编码
        /// </summary>
        public string EnterpriseCode
        {
            get { return GetProperty(EnterpriseCodeProperty); }
            set { SetProperty(EnterpriseCodeProperty, value); }
        }
        #endregion

        #region 发货仓库编码 WarehouseCode
        /// <summary>
        /// 发货仓库编码
        /// </summary>
        [Label("发货仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<ShippingOrderInf>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return GetProperty(WarehouseCodeProperty); }
            set { SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 取消标记 CancelFlag
        /// <summary>
        /// 取消标记
        /// </summary>
        [Label("取消标记")]
        public static readonly Property<string> CancelFlagProperty = P<ShippingOrderInf>.Register(e => e.CancelFlag);

        /// <summary>
        /// 取消标记
        /// </summary>
        public string CancelFlag
        {
            get { return GetProperty(CancelFlagProperty); }
            set { SetProperty(CancelFlagProperty, value); }
        }
        #endregion

        #region 取消日期 CancelDate
        /// <summary>
        /// 取消日期
        /// </summary>
        [Label("取消日期")]
        public static readonly Property<string> CancelDateProperty = P<ShippingOrderInf>.Register(e => e.CancelDate);

        /// <summary>
        /// 取消日期
        /// </summary>
        public string CancelDate
        {
            get { return GetProperty(CancelDateProperty); }
            set { SetProperty(CancelDateProperty, value); }
        }
        #endregion

        #region 订单状态 OrderState
        /// <summary>
        /// 订单状态
        /// </summary>
        [Label("订单状态")]
        public static readonly Property<int> OrderStateProperty = P<ShippingOrderInf>.Register(e => e.OrderState);

        /// <summary>
        /// 单据订单状态状态
        /// </summary>
        public int OrderState
        {
            get { return GetProperty(OrderStateProperty); }
            set { SetProperty(OrderStateProperty, value); }
        }
        #endregion

        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<int> OrderTypeProperty = P<ShippingOrderInf>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public int OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 详细地址 Address
        /// <summary>
        /// 详细地址
        /// </summary>
        [Label("详细地址")]
        public static readonly Property<string> AddressProperty = P<ShippingOrderInf>.Register(e => e.Address);

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address
        {
            get { return this.GetProperty(AddressProperty); }
            set { this.SetProperty(AddressProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateTime?> DeliveryDateProperty = P<ShippingOrderInf>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 发货日期 ShippingDate
        /// <summary>
        /// 发货日期
        /// </summary>
        [Label("发货日期")]
        public static readonly Property<DateTime> ShippingDateProperty = P<ShippingOrderInf>.Register(e => e.ShippingDate);

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime ShippingDate
        {
            get { return this.GetProperty(ShippingDateProperty); }
            set { this.SetProperty(ShippingDateProperty, value); }
        }
        #endregion

        #region 交接人 Connecter
        /// <summary>
        /// 交接人
        /// </summary>
        [Label("交接人")]
        public static readonly Property<string> ConnecterProperty = P<ShippingOrderInf>.Register(e => e.Connecter);

        /// <summary>
        /// 交接人
        /// </summary>
        public string Connecter
        {
            get { return this.GetProperty(ConnecterProperty); }
            set { this.SetProperty(ConnecterProperty, value); }
        }
        #endregion

        #region 联系人 Contacts
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactsProperty = P<ShippingOrderInf>.Register(e => e.Contacts);

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        {
            get { return this.GetProperty(ContactsProperty); }
            set { this.SetProperty(ContactsProperty, value); }
        }
        #endregion

        #region 联系电话 ContactNumber
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactNumberProperty = P<ShippingOrderInf>.Register(e => e.ContactNumber);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber
        {
            get { return this.GetProperty(ContactNumberProperty); }
            set { this.SetProperty(ContactNumberProperty, value); }
        }
        #endregion

        #region 运输公司 TransportCompany
        /// <summary>
        /// 运输公司
        /// </summary>
        [Label("运输公司")]
        public static readonly Property<string> TransportCompanyProperty = P<ShippingOrderInf>.Register(e => e.TransportCompany);

        /// <summary>
        /// 运输公司
        /// </summary>
        public string TransportCompany
        {
            get { return this.GetProperty(TransportCompanyProperty); }
            set { this.SetProperty(TransportCompanyProperty, value); }
        }
        #endregion

        #region 运单号 TransportNo
        /// <summary>
        /// 运单号
        /// </summary>
        [Label("运单号")]
        public static readonly Property<string> TransportNoProperty = P<ShippingOrderInf>.Register(e => e.TransportNo);

        /// <summary>
        /// 运单号
        /// </summary>
        public string TransportNo
        {
            get { return this.GetProperty(TransportNoProperty); }
            set { this.SetProperty(TransportNoProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(1000)]
        public static readonly Property<string> RemarkProperty = P<ShippingOrderInf>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 优先级 PriorityType
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<int> PriorityTypeProperty = P<ShippingOrderInf>.Register(e => e.PriorityType);

        /// <summary>
        /// 优先级
        /// </summary>
        public int PriorityType
        {
            get { return this.GetProperty(PriorityTypeProperty); }
            set { this.SetProperty(PriorityTypeProperty, value); }
        }
        #endregion

        #region 取消原因 CancelReason
        /// <summary>
        /// 取消原因
        /// </summary>
        [Label("取消原因")]
        public static readonly Property<string> CancelReasonProperty = P<ShippingOrderInf>.Register(e => e.CancelReason);

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason
        {
            get { return this.GetProperty(CancelReasonProperty); }
            set { this.SetProperty(CancelReasonProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 发运单中间表 实体配置
    /// </summary>
    internal class ShippingOrderInfConfig : EntityConfig<ShippingOrderInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_SHIPPING_ORDER").MapAllProperties();
            Meta.Property(ShippingOrderInf.RemarkProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }
    }
}