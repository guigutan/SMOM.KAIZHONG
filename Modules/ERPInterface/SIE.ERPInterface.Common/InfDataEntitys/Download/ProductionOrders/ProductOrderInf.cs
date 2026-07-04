using System;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 生产订单中间表
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("生产订单中间表")]
    public class ProductOrderInf : DownloadBaseEntity
    {
        #region 生产订单编号 Code
        /// <summary>
        /// 生产订单编号
        /// </summary>
        [Label("生产订单编号")]
        public static readonly Property<string> CodeProperty = P<ProductOrderInf>.Register(e => e.Code);

        /// <summary>
        /// 生产订单编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料编号 ItemCode
        /// <summary>
        /// 物料编号
        /// </summary>
        [Label("物料编号")]
        public static readonly Property<string> ItemCodeProperty = P<ProductOrderInf>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 订单数量 Qty
        /// <summary>
        /// 订单数量
        /// </summary>
        [Label("订单数量")]
        public static readonly Property<decimal> QtyProperty = P<ProductOrderInf>.Register(e => e.Qty);

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<int> PriorityProperty = P<ProductOrderInf>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 制程工艺路线编码 RouteCode
        /// <summary>
        /// 制程工艺路线编码
        /// </summary>
        [Label("制程工艺路线编码")]
        public static readonly Property<string> RouteCodeProperty = P<ProductOrderInf>.Register(e => e.RouteCode);

        /// <summary>
        /// 制程工艺路线编码
        /// </summary>
        public string RouteCode
        {
            get { return GetProperty(RouteCodeProperty); }
            set { SetProperty(RouteCodeProperty, value); }
        }
        #endregion

        //#region 工单类型 OrderType
        ///// <summary>
        ///// 工单类型
        ///// </summary>
        //[Label("工单类型")]
        //public static readonly Property<ProductOrderType?> OrderTypeProperty = P<ProductOrderInf>.Register(e => e.OrderType);

        ///// <summary>
        ///// 工单类型
        ///// </summary>
        //public ProductOrderType? OrderType
        //{
        //    get { return GetProperty(OrderTypeProperty); }
        //    set { SetProperty(OrderTypeProperty, value); }
        //}
        //#endregion

        #region 指定工厂编号 FactoryCode
        /// <summary>
        /// 指定工厂编号
        /// </summary>
        [Label("指定工厂编号")]
        public static readonly Property<string> FactoryCodeProperty = P<ProductOrderInf>.Register(e => e.FactoryCode);

        /// <summary>
        /// 指定工厂编号
        /// </summary>
        public string FactoryCode
        {
            get { return GetProperty(FactoryCodeProperty); }
            set { SetProperty(FactoryCodeProperty, value); }
        }
        #endregion

        #region 销售订单编号 SaleNo
        /// <summary>
        /// 销售订单编号
        /// </summary>
        [Label("销售订单编号")]
        public static readonly Property<string> SaleNoProperty = P<ProductOrderInf>.Register(e => e.SaleNo);

        /// <summary>
        /// 销售订单编号
        /// </summary>
        public string SaleNo
        {
            get { return GetProperty(SaleNoProperty); }
            set { SetProperty(SaleNoProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<ProductOrderInf>.Register(e => e.CustomerCode);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Label("客户交期")]
        public static readonly Property<DateTime> RequireDeliveryProperty = P<ProductOrderInf>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        #region 工厂交期 PromiseDelivery
        /// <summary>
        /// 工厂交期
        /// </summary>
        [Label("工厂交期")]
        public static readonly Property<DateTime> PromiseDeliveryProperty = P<ProductOrderInf>.Register(e => e.PromiseDelivery);

        /// <summary>
        /// 工厂交期
        /// </summary>
        public DateTime PromiseDelivery
        {
            get { return GetProperty(PromiseDeliveryProperty); }
            set { SetProperty(PromiseDeliveryProperty, value); }
        }
        #endregion

        #region 齐料日期 RawMaterialDate
        /// <summary>
        /// 齐料日期
        /// </summary>
        [Label("齐料日期")]
        public static readonly Property<DateTime?> RawMaterialDateProperty = P<ProductOrderInf>.Register(e => e.RawMaterialDate);

        /// <summary>
        /// 齐料日期
        /// </summary>
        public DateTime? RawMaterialDate
        {
            get { return GetProperty(RawMaterialDateProperty); }
            set { SetProperty(RawMaterialDateProperty, value); }
        }
        #endregion

        #region 建议开工日期 SuggestStart
        /// <summary>
        /// 建议开工日期
        /// </summary>
        [Label("建议开工日期")]
        public static readonly Property<DateTime?> SuggestStartProperty = P<ProductOrderInf>.Register(e => e.SuggestStart);

        /// <summary>
        /// 建议开工日期
        /// </summary>
        public DateTime? SuggestStart
        {
            get { return GetProperty(SuggestStartProperty); }
            set { SetProperty(SuggestStartProperty, value); }
        }
        #endregion

        #region 建议结束日期 SuggestEnd
        /// <summary>
        /// 建议结束日期
        /// </summary>
        [Label("建议结束日期")]
        public static readonly Property<DateTime?> SuggestEndProperty = P<ProductOrderInf>.Register(e => e.SuggestEnd);

        /// <summary>
        /// 建议结束日期
        /// </summary>
        public DateTime? SuggestEnd
        {
            get { return GetProperty(SuggestEndProperty); }
            set { SetProperty(SuggestEndProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 生产订单中间表 实体配置
    /// </summary>
    internal class ProductOrderInfConfig : EntityConfig<ProductOrderInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_PRODUCT_ORDER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
