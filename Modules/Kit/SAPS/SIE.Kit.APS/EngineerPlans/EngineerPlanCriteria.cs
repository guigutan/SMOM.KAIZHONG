using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.EngineerPlans
{
    /// <summary>
    /// 工程计划查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class EngineerPlanCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EngineerPlanCriteria()
        {
            ScheduleDay = new DateRange();
            //PlanFinishDay = new DateRange();
            RequireDelivery = new DateRange();
        }

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<EngineerPlanCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id 
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<EngineerPlanCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<EngineerPlanCriteria>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return GetProperty(SaleOrderNoProperty); }
            set { SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 订单行号 LineNo
        /// <summary>
        /// 订单行号
        /// </summary>
        [Label("订单行号")]
        public static readonly Property<string> LineNoProperty = P<EngineerPlanCriteria>.Register(e => e.LineNo);

        /// <summary>
        /// 订单行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 物料(生产型号) Item
        /// <summary>
        /// 物料(生产型号)Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<EngineerPlanCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料(生产型号)Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 生产型号
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<EngineerPlanCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 生产型号
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<EngineerPlanCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<EngineerPlanCriteria>.Register(e => e.CustomerCode);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<EngineerPlanCriteria>.Register(e => e.CustomerName);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<string> OrderTypeProperty = P<EngineerPlanCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 工程计划状态 PlanState
        /// <summary>
        /// 工程计划状态
        /// </summary>
        [Label("工程计划状态")]
        public static readonly Property<SOMI_PlanState?> PlanStateProperty = P<EngineerPlanCriteria>.Register(e => e.PlanState);

        /// <summary>
        /// 工程计划状态
        /// </summary>
        public SOMI_PlanState? PlanState
        {
            get { return GetProperty(PlanStateProperty); }
            set { SetProperty(PlanStateProperty, value); }
        }
        #endregion

        #region 排单日期  ScheduleDay
        /// <summary>
        /// 排单日期
        /// </summary>
        [Label("排单日期")]
        public static readonly Property<DateRange> ScheduleDayProperty = P<EngineerPlanCriteria>.Register(e => e.ScheduleDay);

        /// <summary>
        ///  排单日期
        /// </summary>
        public DateRange ScheduleDay
        {
            get { return GetProperty(ScheduleDayProperty); }
            set { SetProperty(ScheduleDayProperty, value); }
        }
        #endregion

        #region 落单日期  SortDate
        /// <summary>
        /// 落单日期
        /// </summary>
        [Label("落单日期")]
        public static readonly Property<DateRange> SortDateProperty = P<EngineerPlanCriteria>.Register(e => e.SortDate);

        /// <summary>
        ///  落单日期
        /// </summary>
        public DateRange SortDate
        {
            get { return GetProperty(SortDateProperty); }
            set { SetProperty(SortDateProperty, value); }
        }
        #endregion

        //#region 计划完成日期  PlanFinishDay
        ///// <summary>
        ///// 计划完成日期
        ///// </summary>
        //[Label("计划完成日期")]
        //public static readonly Property<DateRange> PlanFinishDayProperty = P<EngineerCriteria>.Register(e => e.PlanFinishDay);

        ///// <summary>
        /////  计划完成日期
        ///// </summary>
        //public DateRange PlanFinishDay
        //{
        //    get { return GetProperty(PlanFinishDayProperty); }
        //    set { SetProperty(PlanFinishDayProperty, value); }
        //}
        //#endregion

        #region 客户交期 RequireDelivery
        /// <summary>
        /// 客户交期
        /// </summary>
        [Label("客户交期")]
        public static readonly Property<DateRange> RequireDeliveryProperty = P<EngineerPlanCriteria>.Register(e => e.RequireDelivery);

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateRange RequireDelivery
        {
            get { return GetProperty(RequireDeliveryProperty); }
            set { SetProperty(RequireDeliveryProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EngineerPlanController>().Fetch(this);
        }
    }
}
