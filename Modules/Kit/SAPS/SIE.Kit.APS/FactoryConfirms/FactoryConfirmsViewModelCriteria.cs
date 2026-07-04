using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.SO.SaleOrders;
using System;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 厂别确认查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("厂别确认")]
    public class FactoryConfirmsViewModelCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FactoryConfirmsViewModelCriteria()
        {
        }
        #endregion

        #region 销售订单编码 SalesOrderCode
        /// <summary>
        /// 销售订单编码
        /// </summary>
        [Label("销售订单编码")]
        public static readonly Property<string> SalesOrderCodeProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.SalesOrderCode);

        /// <summary>
        /// 销售订单编码
        /// </summary>
        public string SalesOrderCode
        {
            get { return this.GetProperty(SalesOrderCodeProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 行业类型 IndustryType
        /// <summary>
        /// 行业类型
        /// </summary>
        [Label("行业类型")]
        public static readonly Property<string> IndustryTypeProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.IndustryType);

        /// <summary>
        /// 行业类型
        /// </summary>
        public string IndustryType
        {
            get { return GetProperty(IndustryTypeProperty); }
            set { SetProperty(IndustryTypeProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<string> OrderTypeProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 产品类型 ProductType
        /// <summary>
        /// 产品类型
        /// </summary>
        [Label("产品类型")]
        public static readonly Property<string> ProductTypeProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.ProductType);

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return GetProperty(ProductTypeProperty); }
            set { SetProperty(ProductTypeProperty, value); }
        }
        #endregion  

        #region 是否新单 IsNew
        /// <summary>
        /// 是否新单
        /// </summary>
        [Label("是否新单")]
        public static readonly Property<bool?> IsNewProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.IsNew);

        /// <summary>
        /// 是否新单
        /// </summary>
        public bool? IsNew
        {
            get { return GetProperty(IsNewProperty); }
            set { SetProperty(IsNewProperty, value); }
        }
        #endregion

        #region 行状态 LineState
        /// <summary>
        /// 行状态
        /// </summary>
        [Label("行状态")]
        public static readonly Property<LineState?> LineStateProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.LineState);

        /// <summary>
        /// 行状态
        /// </summary>
        public LineState? LineState
        {
            get { return GetProperty(LineStateProperty); }
            set { SetProperty(LineStateProperty, value); }
        }
        #endregion

        #region 库存组织 Enterprise
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public static readonly IRefIdProperty EnterpriseIdProperty = P<FactoryConfirmsViewModelCriteria>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double? EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 库存组织
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<FactoryConfirmsViewModelCriteria>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 库存组织
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Label("客户交期")]
        public static readonly Property<DateRange> RequireDeliveryProperty = P<FactoryConfirmsViewModelCriteria>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateRange RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FactoryConfirmsController>().GetFactoryConfirmList(this);
        }
    }
}
