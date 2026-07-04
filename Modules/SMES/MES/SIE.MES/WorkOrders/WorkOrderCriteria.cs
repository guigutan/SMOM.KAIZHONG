using SIE.Common;
using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.MES.BlueLable;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工单查询")]
    public partial class WorkOrderCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderCriteria()
        {
            PlanBeginDate = new DateRange();
            PlanBeginDate.DateTimePart = DateTimePart.Date;  //选择日期格式为天
            PlanBeginDate.DateRangeType = DateRangeType.Week;  //默认日期为本周

            PlanEndDate = new DateRange();
            PlanEndDate.DateTimePart = DateTimePart.Date;
            PlanEndDate.DateRangeType = DateRangeType.Week;
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WorkOrderCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 客户订单号 CustomerOrderNo
        /// <summary>
        /// 客户订单号
        /// </summary>
        [Label("客户订单号")]
        public static readonly Property<string> CustomerOrderNoProperty = P<WorkOrderCriteria>.Register(e => e.CustomerOrderNo);

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo
        {
            get { return GetProperty(CustomerOrderNoProperty); }
            set { SetProperty(CustomerOrderNoProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<WorkOrderCriteria>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return GetProperty(SaleOrderNoProperty); }
            set { SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 工艺单编号 ProcessTechOrderCode
        /// <summary>
        /// 工艺单编号
        /// </summary>
        [Label("工艺单编号")]
        public static readonly Property<string> ProcessTechOrderCodeProperty = P<WorkOrderCriteria>.Register(e => e.ProcessTechOrderCode);

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string ProcessTechOrderCode
        {
            get { return GetProperty(ProcessTechOrderCodeProperty); }
            set { SetProperty(ProcessTechOrderCodeProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateRange> PlanBeginDateProperty = P<WorkOrderCriteria>.Register(e => e.PlanBeginDate, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateRange PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateRange> PlanEndDateProperty = P<WorkOrderCriteria>.Register(e => e.PlanEndDate, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateRange PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<WorkOrderCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 车间ID Workshop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty =
            P<WorkOrderCriteria>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)this.GetRefNullableId(WorkshopIdProperty); }
            set { this.SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty =
            P<WorkOrderCriteria>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return this.GetRefEntity(WorkshopProperty); }
            set { this.SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<WorkOrderCriteria>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<WorkOrderCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<WorkOrderCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<string> StateProperty = P<WorkOrderCriteria>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public string State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否暂停 IsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo?> IsPauseProperty = P<WorkOrderCriteria>.Register(e => e.IsPause);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public YesNo? IsPause
        {
            get { return this.GetProperty(IsPauseProperty); }
            set { this.SetProperty(IsPauseProperty, value); }
        }
        #endregion

        #region 齐套状态 KitType
        /// <summary>
        /// 齐套状态
        /// </summary>
        [Label("齐套状态")]
        public static readonly Property<KitType?> KitTypeProperty = P<WorkOrderCriteria>.Register(e => e.KitType);

        /// <summary>
        /// 齐套状态
        /// </summary>
        public KitType? KitType
        {
            get { return GetProperty(KitTypeProperty); }
            set { SetProperty(KitTypeProperty, value); }
        }
        #endregion

        #region 工单来源 SourceType
        /// <summary>
        /// 工单来源
        /// </summary>
        [Label("工单来源")]
        public static readonly Property<SourceType?> SourceProperty = P<WorkOrderCriteria>.Register(e => e.Source);

        /// <summary>
        /// 工单来源
        /// </summary>
        public SourceType? Source
        {
            get { return this.GetProperty(SourceProperty); }
            set { this.SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 计划单号 PlanNo
        /// <summary>
        /// 计划单号
        /// </summary>
        [Label("计划单号")]
        public static readonly Property<string> PlanNoProperty = P<WorkOrderCriteria>.Register(e => e.PlanNo);

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanNo
        {
            get { return this.GetProperty(PlanNoProperty); }
            set { this.SetProperty(PlanNoProperty, value); }
        }
        #endregion

        #region 组合板工单 PanelWorkOrder
        ///// <summary>
        ///// 组合板工单Id
        ///// </summary>
        //[Label("组合板工单")]
        //public static readonly IRefIdProperty PanelWorkOrderIdProperty = P<WorkOrderCriteria>.RegisterRefId(e => e.PanelWorkOrderId, ReferenceType.Normal);

        ///// <summary>
        ///// 组合板工单Id
        ///// </summary>
        //public double? PanelWorkOrderId
        //{
        //    get { return (double?)GetRefNullableId(PanelWorkOrderIdProperty); }
        //    set { SetRefNullableId(PanelWorkOrderIdProperty, value); }
        //}

        ///// <summary>
        ///// 组合板工单
        ///// </summary>
        //public static readonly RefEntityProperty<WorkOrder> PanelWorkOrderProperty = P<WorkOrderCriteria>.RegisterRef(e => e.PanelWorkOrder, PanelWorkOrderIdProperty);

        ///// <summary>
        ///// 组合板工单
        ///// </summary>
        //public WorkOrder PanelWorkOrder
        //{
        //    get { return GetRefEntity(PanelWorkOrderProperty); }
        //    set { SetRefEntity(PanelWorkOrderProperty, value); }
        //}
        #endregion

        #region 组合板工单 PanelWorkOrderNo
        /// <summary>
        /// 组合板工单
        /// </summary>
        [Label("组合板工单")]
        public static readonly Property<string> PanelWorkOrderNoProperty = P<WorkOrderCriteria>.Register(e => e.PanelWorkOrderNo);

        /// <summary>
        /// 组合板工单
        /// </summary>
        public string PanelWorkOrderNo
        {
            get { return GetProperty(PanelWorkOrderNoProperty); }
            set { SetProperty(PanelWorkOrderNoProperty, value); }
        }
        #endregion

        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<WorkOrderCriteria>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectMaintainId
        {
            get { return (double?)this.GetRefNullableId(ProjectMaintainIdProperty); }
            set { this.SetRefNullableId(ProjectMaintainIdProperty, value); }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        public static readonly RefEntityProperty<ProjectMaintain> ProjectMaintainProperty =
            P<WorkOrderCriteria>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain ProjectMaintain
        {
            get { return this.GetRefEntity(ProjectMaintainProperty); }
            set { this.SetRefEntity(ProjectMaintainProperty, value); }
        }
        #endregion

        #region 旧料号 ProductShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ProductShortDescriptionProperty = P<WorkOrderCriteria>.Register(e => e.ProductShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ProductShortDescription
        {
            get { return GetProperty(ProductShortDescriptionProperty); }
            set { SetProperty(ProductShortDescriptionProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<WorkOrderCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 修改时间 UpdateDate
        /// <summary>
        /// 修改时间
        /// </summary>
        [Label("修改时间")]
        public static readonly Property<DateRange> UpdateDateProperty = P<WorkOrderCriteria>.Register(e => e.UpdateDate);

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateRange UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { this.SetProperty(UpdateDateProperty, value); }
        }
        #endregion

        #region 是否显示关闭工单 IsClose
        /// <summary>
        /// 是否显示关闭工单
        /// </summary>
        [Label("是否显示关闭工单")]
        public static readonly Property<bool?> IsCloseProperty = P<WorkOrderCriteria>.Register(e => e.IsClose);

        /// <summary>
        /// 是否显示关闭工单
        /// </summary>
        public bool? IsClose
        {
            get { return this.GetProperty(IsCloseProperty); }
            set { this.SetProperty(IsCloseProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>工单列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(this);
        }
    }
}
