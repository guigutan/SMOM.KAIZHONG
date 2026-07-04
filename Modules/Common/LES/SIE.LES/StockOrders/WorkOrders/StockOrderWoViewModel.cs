using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.StockOrders.WorkOrders
{
    /// <summary>
    /// 备料单工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StockOrderWoViewModelCriteria))]
    [Label("备料单工单")]
    public class StockOrderWoViewModel : Entity<double>
    {
        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<StockOrderWoViewModel>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<StockOrderWoViewModel>.Register(e => e.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<StockOrderWoViewModel>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<StockOrderWoViewModel>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工厂Id FactoryId
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂Id")]
        public static readonly Property<double> FactoryIdProperty = P<StockOrderWoViewModel>.Register(e => e.FactoryId);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
            set { this.SetProperty(FactoryIdProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<StockOrderWoViewModel>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 车间Id WorkshopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double> WorkshopIdProperty = P<StockOrderWoViewModel>.Register(e => e.WorkshopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkshopId
        {
            get { return this.GetProperty(WorkshopIdProperty); }
            set { this.SetProperty(WorkshopIdProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkshopProperty = P<StockOrderWoViewModel>.Register(e => e.Workshop);

        /// <summary>
        /// 车间
        /// </summary>
        public string Workshop
        {
            get { return this.GetProperty(WorkshopProperty); }
            set { this.SetProperty(WorkshopProperty, value); }
        }
        #endregion

        #region 生产资源Id WipResourceId
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源Id")]
        public static readonly Property<double> WipResourceIdProperty = P<StockOrderWoViewModel>.Register(e => e.WipResourceId);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double WipResourceId
        {
            get { return this.GetProperty(WipResourceIdProperty); }
            set { this.SetProperty(WipResourceIdProperty, value); }
        }
        #endregion

        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源
        /// </summary>
        [Label("生产资源")]
        public static readonly Property<string> WipResourceProperty = P<StockOrderWoViewModel>.Register(e => e.WipResource);

        /// <summary>
        /// 生产资源
        /// </summary>
        public string WipResource
        {
            get { return this.GetProperty(WipResourceProperty); }
            set { this.SetProperty(WipResourceProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<StockOrderWoViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 工单状态 WoState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> WoStateProperty = P<StockOrderWoViewModel>.Register(e => e.WoState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState WoState
        {
            get { return this.GetProperty(WoStateProperty); }
            set { this.SetProperty(WoStateProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<StockOrderWoViewModel>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
            set { this.SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划结束时间 PlanEndDate
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<StockOrderWoViewModel>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return this.GetProperty(PlanEndDateProperty); }
            set { this.SetProperty(PlanEndDateProperty, value); }
        }
        #endregion
    }
}
