using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Text;
using Item = SIE.Items.Item;
using SIE.Core.WorkOrders;

namespace SIE.LES.MaterialPreparations.ViewModels
{
    /// <summary>
    /// 工单备料汇总
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkOrderMpViewModelCriteria))]
    [Label("工单备料汇总")]
    public class WorkOrderMpViewModel : Entity<double>
    {
        #region 工单编号 No
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> NoProperty = P<WorkOrderMpViewModel>.Register(e => e.No);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WorkOrderMpViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WorkOrderMpViewModel>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工单状态 WoState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> WoStateProperty = P<WorkOrderMpViewModel>.Register(e => e.WoState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState WoState
        {
            get { return this.GetProperty(WoStateProperty); }
            set { this.SetProperty(WoStateProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrderMpViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<WorkOrderMpViewModel>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<WorkOrderMpViewModel>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 工单类型 WoType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> WoTypeProperty = P<WorkOrderMpViewModel>.Register(e => e.WoType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType WoType
        {
            get { return this.GetProperty(WoTypeProperty); }
            set { this.SetProperty(WoTypeProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<WorkOrderMpViewModel>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
            set { this.SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<WorkOrderMpViewModel>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return this.GetProperty(PlanEndDateProperty); }
            set { this.SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 实际开始时间 ActuStartDate
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> ActuStartDateProperty = P<WorkOrderMpViewModel>.Register(e => e.ActuStartDate);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActuStartDate
        {
            get { return this.GetProperty(ActuStartDateProperty); }
            set { this.SetProperty(ActuStartDateProperty, value); }
        }
        #endregion

        #region 实际完成时间 ActuFinishDate
        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Label("实际完成时间")]
        public static readonly Property<DateTime?> ActuFinishDateProperty = P<WorkOrderMpViewModel>.Register(e => e.ActuFinishDate);

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActuFinishDate
        {
            get { return this.GetProperty(ActuFinishDateProperty); }
            set { this.SetProperty(ActuFinishDateProperty, value); }
        }
        #endregion

        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<WorkOrderMpViewModel>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { this.SetProperty(FactoryNameProperty, value); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<WorkOrderMpViewModel>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WorkOrderMpViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<WorkOrderMpViewModel>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return this.GetProperty(SaleOrderNoProperty); }
            set { this.SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 客户订单号 CustomerOrderNo
        /// <summary>
        /// 客户订单号
        /// </summary>
        [Label("客户订单号")]
        public static readonly Property<string> CustomerOrderNoProperty = P<WorkOrderMpViewModel>.Register(e => e.CustomerOrderNo);

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo
        {
            get { return this.GetProperty(CustomerOrderNoProperty); }
            set { this.SetProperty(CustomerOrderNoProperty, value); }
        }
        #endregion
    }
}
