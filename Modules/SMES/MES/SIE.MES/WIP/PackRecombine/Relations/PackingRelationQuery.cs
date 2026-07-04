

using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.PackRecombine.Relations
{
    /// <summary>
    /// 包装关系
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PackingRelationQueryCriteria))]
    [Label("包装关系")]
    [DisplayMember(nameof(PackageNo))]
    public partial class PackingRelationQuery : PackingRelation
    {
        #region 工单 WorkOrder //重写子类工单
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static new readonly IRefIdProperty WorkOrderIdProperty = P<PackingRelationQuery>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public new double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<PackingRelationQuery>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工序 Process //重写子类工序
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static new readonly IRefIdProperty ProcessIdProperty = P<PackingRelationQuery>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public new double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty = P<PackingRelationQuery>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station //重写子类工位
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static new readonly IRefIdProperty StationIdProperty = P<PackingRelationQuery>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public new double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty = P<PackingRelationQuery>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion
                
        #region 视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PackingRelationQuery>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<PackingRelationQuery>.RegisterView(e => e.ProductId, p => p.WorkOrder.Product.Id);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品 ProductName
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        public static readonly Property<string> ProductNameProperty = P<PackingRelationQuery>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 产线Id ResourceId
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线Id")]
        public static readonly Property<double> ResourceIdProperty = P<PackingRelationQuery>.RegisterView(e => e.ResourceId, p => p.WorkOrder.Resource.Id);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 产线 ResourceName
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceNameProperty = P<PackingRelationQuery>.RegisterView(e => e.ResourceName, p => p.WorkOrder.Resource.Name);

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<PackingRelationQuery>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 工位 StationName
        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<PackingRelationQuery>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
            set { this.SetProperty(StationNameProperty, value); }
        }
        #endregion
        #endregion
    }

    internal class PackingRelationQueryConfig : EntityConfig<PackingRelationQuery>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PKG_RELATION").MapAllProperties();
            Meta.SupportTree();
            Meta.EnablePhantoms();
        }
    }
}
