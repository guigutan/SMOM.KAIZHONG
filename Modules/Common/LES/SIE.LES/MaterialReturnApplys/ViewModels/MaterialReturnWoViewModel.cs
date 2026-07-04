using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.LES.MaterialPreparations.ViewModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.MaterialReturnApplys.ViewModels
{
    /// <summary>
    /// 退料申请工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaterialReturnWoCriteria))]
    [Label("退料申请工单")]
    public class MaterialReturnWoViewModel : Entity<double>
    {
        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<MaterialReturnWoViewModel>.Register(e => e.WoNo);

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
        public static readonly Property<double> ProductIdProperty = P<MaterialReturnWoViewModel>.Register(e => e.ProductId);

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
        public static readonly Property<string> ProductCodeProperty = P<MaterialReturnWoViewModel>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<MaterialReturnWoViewModel>.Register(e => e.ProductName);

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
        public static readonly Property<double> FactoryIdProperty = P<MaterialReturnWoViewModel>.Register(e => e.FactoryId);

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
        public static readonly Property<string> FactoryProperty = P<MaterialReturnWoViewModel>.Register(e => e.Factory);

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
        public static readonly Property<double?> WorkshopIdProperty = P<MaterialReturnWoViewModel>.Register(e => e.WorkshopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
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
        public static readonly Property<string> WorkshopProperty = P<MaterialReturnWoViewModel>.Register(e => e.Workshop);

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
        public static readonly Property<double?> WipResourceIdProperty = P<MaterialReturnWoViewModel>.Register(e => e.WipResourceId);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? WipResourceId
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
        public static readonly Property<string> WipResourceProperty = P<MaterialReturnWoViewModel>.Register(e => e.WipResource);

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
        public static readonly Property<decimal> PlanQtyProperty = P<MaterialReturnWoViewModel>.Register(e => e.PlanQty);

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
        public static readonly Property<WorkOrderState> WoStateProperty = P<MaterialReturnWoViewModel>.Register(e => e.WoState);

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
        public static readonly Property<DateTime> PlanBeginDateProperty = P<MaterialReturnWoViewModel>.Register(e => e.PlanBeginDate);

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
        public static readonly Property<DateTime> PlanEndDateProperty = P<MaterialReturnWoViewModel>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return this.GetProperty(PlanEndDateProperty); }
            set { this.SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 项目号Id ProjectId
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号Id")]
        public static readonly Property<double?> ProjectIdProperty = P<MaterialReturnWoViewModel>.Register(e => e.ProjectId);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectId
        {
            get { return this.GetProperty(ProjectIdProperty); }
            set { this.SetProperty(ProjectIdProperty, value); }
        }
        #endregion

        #region 项目号 ProjectCode
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectCodeProperty = P<MaterialReturnWoViewModel>.Register(e => e.ProjectCode);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
            set { this.SetProperty(ProjectCodeProperty, value); }
        }
        #endregion

    }
}
