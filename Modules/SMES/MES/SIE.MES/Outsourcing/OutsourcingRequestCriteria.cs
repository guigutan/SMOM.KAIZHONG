using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("查询实体")]
    public partial class OutsourcingRequestCriteria : Criteria
    {
        #region 需求单号 NO
        /// <summary>
        /// 需求单号
        /// </summary>
        [Label("需求单号")]
        public static readonly Property<string> NOProperty = P<OutsourcingRequestCriteria>.Register(e => e.NO);

        /// <summary>
        /// 需求单号
        /// </summary>
        public string NO
        {
            get { return GetProperty(NOProperty); }
            set { SetProperty(NOProperty, value); }
        }
        #endregion

        #region 委外状态 OutsourcingState
        /// <summary>
        /// 委外状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OutsourcingState?> OutsourcingStateProperty = P<OutsourcingRequestCriteria>.Register(e => e.OutsourcingState);

        /// <summary>
        /// 委外状态
        /// </summary>
        public OutsourcingState? OutsourcingState
        {
            get { return GetProperty(OutsourcingStateProperty); }
            set { SetProperty(OutsourcingStateProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrder
        /// <summary>
        /// 工单号Id
        /// </summary>
        [Label("工单号")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<OutsourcingRequestCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单号Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单号
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<OutsourcingRequestCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单号
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 起始工序 BeginProcess
        /// <summary>
        /// 起始工序Id
        /// </summary>
        [Label("起始工序")]
        public static readonly IRefIdProperty BeginProcessIdProperty = P<OutsourcingRequestCriteria>.RegisterRefId(e => e.BeginProcessId, ReferenceType.Normal);

        /// <summary>
        /// 起始工序Id
        /// </summary>
        public double? BeginProcessId
        {
            get { return (double?)GetRefNullableId(BeginProcessIdProperty); }
            set { SetRefNullableId(BeginProcessIdProperty, value); }
        }

        /// <summary>
        /// 起始工序
        /// </summary>
        public static readonly RefEntityProperty<Process> BeginProcessProperty = P<OutsourcingRequestCriteria>.RegisterRef(e => e.BeginProcess, BeginProcessIdProperty);

        /// <summary>
        /// 起始工序
        /// </summary>
        public Process BeginProcess
        {
            get { return GetRefEntity(BeginProcessProperty); }
            set { SetRefEntity(BeginProcessProperty, value); }
        }
        #endregion

        #region 结束工序 EndProcess
        /// <summary>
        /// 结束工序Id
        /// </summary>
        [Label("结束工序")]
        public static readonly IRefIdProperty EndProcessIdProperty = P<OutsourcingRequestCriteria>.RegisterRefId(e => e.EndProcessId, ReferenceType.Normal);

        /// <summary>
        /// 结束工序Id
        /// </summary>
        public double? EndProcessId
        {
            get { return (double?)GetRefNullableId(EndProcessIdProperty); }
            set { SetRefNullableId(EndProcessIdProperty, value); }
        }

        /// <summary>
        /// 结束工序
        /// </summary>
        public static readonly RefEntityProperty<Process> EndProcessProperty = P<OutsourcingRequestCriteria>.RegisterRef(e => e.EndProcess, EndProcessIdProperty);

        /// <summary>
        /// 结束工序
        /// </summary>
        public Process EndProcess
        {
            get { return GetRefEntity(EndProcessProperty); }
            set { SetRefEntity(EndProcessProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<OutsourcingRequestCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<OutsourcingRequestCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 生产资源 WipResource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<OutsourcingRequestCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<OutsourcingRequestCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<OutsourcingRequestCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<OutsourcingRequestCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateRange> PlanBeginDateProperty = P<OutsourcingRequestCriteria>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateRange PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
            set { this.SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 供应商Id supplierIds
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商Id")]
        public static readonly Property<string> SupplierIdsProperty = P<OutsourcingRequestCriteria>.Register(e => e.SupplierIds);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public string SupplierIds
        {
            get { return this.GetProperty(SupplierIdsProperty); }
            set { this.SetProperty(SupplierIdsProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierNames
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNamesProperty = P<OutsourcingRequestCriteria>.Register(e => e.SupplierNames);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierNames
        {
            get { return this.GetProperty(SupplierNamesProperty); }
            set { this.SetProperty(SupplierNamesProperty, value); }
        }
        #endregion

        #region 产品名称 ProduceName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProduceNameProperty = P<OutsourcingRequestCriteria>.Register(e => e.ProduceName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProduceName
        {
            get { return this.GetProperty(ProduceNameProperty); }
            set { this.SetProperty(ProduceNameProperty, value); }
        }
        #endregion

        #region 发料状态 OutboundState
        /// <summary>
        /// 发料状态
        /// </summary>
        [Label("发料状态")]
        public static readonly Property<OutboundState?> OutboundStateProperty = P<OutsourcingRequestCriteria>.Register(e => e.OutboundState);

        /// <summary>
        /// 发料状态
        /// </summary>
        public OutboundState? OutboundState
        {
            get { return this.GetProperty(OutboundStateProperty); }
            set { this.SetProperty(OutboundStateProperty, value); }
        }
        #endregion

        #region 报工状态 ReportState
        /// <summary>
        /// 报工状态
        /// </summary>
        [Label("报工状态")]
        public static readonly Property<ReportState?> ReportStateProperty = P<OutsourcingRequestCriteria>.Register(e => e.ReportState);

        /// <summary>
        /// 报工状态
        /// </summary>
        public ReportState? ReportState
        {
            get { return this.GetProperty(ReportStateProperty); }
            set { this.SetProperty(ReportStateProperty, value); }
        }
        #endregion

        #region 委外工厂 OutFactory
        /// <summary>
        /// 委外工厂
        /// </summary>
        [Label("委外工厂")]
        public static readonly Property<string> OutFactoryProperty = P<OutsourcingRequestCriteria>.Register(e => e.OutFactory);

        /// <summary>
        /// 委外工厂
        /// </summary>
        public string OutFactory
        {
            get { return this.GetProperty(OutFactoryProperty); }
            set { this.SetProperty(OutFactoryProperty, value); }
        }
        #endregion

        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<OutsourcingRequestCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<OutsourcingRequestCriteria>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OutsourcingRequestController>().Fetch(this);
        }

    }
}