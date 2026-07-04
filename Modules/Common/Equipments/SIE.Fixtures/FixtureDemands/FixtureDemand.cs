using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures.Enums;
using SIE.Fixtures.FixtureDemands.Config;
using SIE.Fixtures.FixtureDemands.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessSegments;
using SIE.Resources.WipResources;
using System;

namespace SIE.Fixtures.FixtureDemands
{
    /// <summary>
	/// 工治具需求清单
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureDemandCriteria))]
    [EntityWithConfig(typeof(NoConfig), "工治具需求清单单号生成规则", "用于配置工治具需求清单单号规则")]
    [EntityWithConfig(typeof(FixtureDemandsConfig))]
    [EntityWithConfig(typeof(GenerationDemandsConfig))]
    [Label("工治具需求清单")]
    public partial class FixtureDemand : DataEntity
    {
        #region 需求单号 No
        /// <summary>
        /// 需求单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("需求单号")]
        public static readonly Property<string> NoProperty = P<FixtureDemand>.Register(e => e.No);

        /// <summary>
        /// 需求单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 需求明细 DetailList
        /// <summary>
        /// 需求明细
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureDemandDetail>> DetailListProperty = P<FixtureDemand>.RegisterList(e => e.DetailList);
        /// <summary>
        /// 需求明细
        /// </summary>
        public EntityList<FixtureDemandDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 出库明细 UnloadList
        /// <summary>
        /// 出库明细
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureUnload>> UnloadListProperty = P<FixtureDemand>.RegisterList(e => e.UnloadList);
        /// <summary>
        /// 出库明细
        /// </summary>
        public EntityList<FixtureUnload> UnloadList
        {
            get { return this.GetLazyList(UnloadListProperty); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<FixtureDemand>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<FixtureDemand>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 领用状态 ReceiveState
        /// <summary>
        /// 领用状态
        /// </summary>
        [Label("领用状态")]
        public static readonly Property<ReceiveState> ReceiveStateProperty = P<FixtureDemand>.Register(e => e.ReceiveState);

        /// <summary>
        /// 领用状态
        /// </summary>
        public ReceiveState ReceiveState
        {
            get { return GetProperty(ReceiveStateProperty); }
            set { SetProperty(ReceiveStateProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<FixtureDemand>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<FixtureDemand>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 出库状态 DemandState
        /// <summary>
        /// 出库状态
        /// </summary>
        [Label("出库状态")]
        public static readonly Property<DemandState> DemandStateProperty = P<FixtureDemand>.Register(e => e.DemandState);

        /// <summary>
        /// 出库状态
        /// </summary>
        public DemandState DemandState
        {
            get { return GetProperty(DemandStateProperty); }
            set { SetProperty(DemandStateProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        public static readonly IRefIdProperty WorkShopIdProperty = P<FixtureDemand>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return (double)GetRefId(WorkShopIdProperty); }
            set { SetRefId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<FixtureDemand>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 工段 ProcessSegmen
        /// <summary>
        /// 工段Id
        /// </summary>
        [Label("工段")]
        public static readonly IRefIdProperty ProcessSegmentIdProperty =
            P<FixtureDemand>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return (double?)this.GetRefNullableId(ProcessSegmentIdProperty); }
            set { this.SetRefNullableId(ProcessSegmentIdProperty, value); }
        }

        /// <summary>
        /// 工段
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty =
            P<FixtureDemand>.RegisterRef(e => e.ProcessStegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment ProcessStegment
        {
            get { return this.GetRefEntity(ProcessSegmentProperty); }
            set { this.SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 工治具需求时间  DemandTime
        /// <summary>
        /// 工治具需求时间
        /// </summary>
        [Label("工治具需求时间")]
        public static readonly Property<DateTime>  DemandTimeProperty = P<FixtureDemand>.Register(e => e. DemandTime);

        /// <summary>
        /// 工治具需求时间
        /// </summary>
        public DateTime DemandTime
        {
            get { return this.GetProperty( DemandTimeProperty); }
            set { this.SetProperty( DemandTimeProperty, value); }
        }
        #endregion


        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<FixtureDemand>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
            set { this.SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion


        #region 工艺面 ProcessSurface
        /// <summary>
        /// 工艺面
        /// </summary>
        [Label("工艺面")]
        public static readonly Property<Deck?> ProcessSurfaceProperty = P<FixtureDemand>.Register(e => e.ProcessSurface);

        /// <summary>
        /// 工艺面
        /// </summary>
        public Deck? ProcessSurface
        {
            get { return GetProperty(ProcessSurfaceProperty); }
            set { SetProperty(ProcessSurfaceProperty, value); }
        }
        #endregion

        #region 强制关单 Close
        /// <summary>
        /// 强制关单
        /// </summary>
        [Label("强制关单")]
        public static readonly Property<bool?> CloseProperty = P<FixtureDemand>.Register(e => e.Close);

        /// <summary>
        /// 强制关单
        /// </summary>
        public bool? Close
        {
            get { return this.GetProperty(CloseProperty); }
            set { this.SetProperty(CloseProperty, value); }
        }
        #endregion

        #region 关单原因 CloseRemark
        /// <summary>
        /// 关单原因
        /// </summary>
        [Label("关单原因")]
        public static readonly Property<string> CloseRemarkProperty = P<FixtureDemand>.Register(e => e.CloseRemark);

        /// <summary>
        /// CloseRemark
        /// </summary>
        public string CloseRemark
        {
            get { return this.GetProperty(CloseRemarkProperty); }
            set { this.SetProperty(CloseRemarkProperty, value); }
        }
        #endregion

        #region 单据来源 Billsource
        /// <summary>
        /// 单据来源
        /// </summary>
        [Label("单据来源")]
        public static readonly Property<BillSource> BillsourceProperty = P<FixtureDemand>.Register(e => e.Billsource);

        /// <summary>
        /// 单据来源
        /// </summary>
        public BillSource Billsource
        {
            get { return this.GetProperty(BillsourceProperty); }
            set { this.SetProperty(BillsourceProperty, value); }
        }
        #endregion


        #region 视图属性
        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<FixtureDemand>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 产线 ResourceName
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceNameProperty = P<FixtureDemand>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<FixtureDemand>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品 WorkOrderProductCode
        /// <summary>
        /// 产品
        /// </summary>
        [Label("产品")]
        public static readonly Property<string> WorkOrderProductCodeProperty = P<FixtureDemand>.RegisterView(e => e.WorkOrderProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品
        /// </summary>
        public string WorkOrderProductCode
        {
            get { return this.GetProperty(WorkOrderProductCodeProperty); }
            set { this.SetProperty(WorkOrderProductCodeProperty, value); }
        }
        #endregion

        #region 工段编码 ProcessStegmentCode
        /// <summary>
        /// 工段编码
        /// </summary>
        [Label("工段编码")]
        public static readonly Property<string> ProcessStegmentCodeProperty = P<FixtureDemand>.RegisterView(e => e.ProcessStegmentCode, p => p.ProcessStegment.Name);

        /// <summary>
        /// 工段编码
        /// </summary>
        public string ProcessStegmentCode
        {
            get { return this.GetProperty(ProcessStegmentCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工治具需求清单 实体配置
    /// </summary>
    internal class FixtureDemandConfig : EntityConfig<FixtureDemand>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIX_DEMAND").MapAllProperties();
            Meta.Property(FixtureDemand.CloseRemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
