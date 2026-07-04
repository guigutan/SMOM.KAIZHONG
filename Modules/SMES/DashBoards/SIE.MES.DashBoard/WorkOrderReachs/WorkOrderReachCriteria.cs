using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.TeamManagement;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 工单准时达成率率报表 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工单准时达成率率报表")]
    public class WorkOrderReachCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderReachCriteria()
        {
            PlanDate = new DateRange();
        }

        #region 车间 WorkShop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<WorkOrderReachCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<WorkOrderReachCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 Line
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<WorkOrderReachCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WorkOrderReachCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty =
            P<WorkOrderReachCriteria>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double? ShiftId
        {
            get { return (double?)this.GetRefNullableId(ShiftIdProperty); }
            set { this.SetRefNullableId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Resources.ShiftTypes.Shift> ShiftProperty =
            P<WorkOrderReachCriteria>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Resources.ShiftTypes.Shift Shift
        {
            get { return this.GetRefEntity(ShiftProperty); }
            set { this.SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 产品机型 Model
        /// <summary>
        /// 产品机型Id
        /// </summary>
        [Label("产品机型")]
        public static readonly IRefIdProperty ModelIdProperty = P<WorkOrderReachCriteria>.RegisterRefId(e => e.ModelId, ReferenceType.Normal);

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double? ModelId
        {
            get { return (double?)GetRefNullableId(ModelIdProperty); }
            set { SetRefNullableId(ModelIdProperty, value); }
        }

        /// <summary>
        /// 产品机型
        /// </summary>
        [Label("产品机型")]
        public static readonly RefEntityProperty<ProductModel> ModelProperty = P<WorkOrderReachCriteria>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 产品机型
        /// </summary>
        public ProductModel Model
        {
            get { return GetRefEntity(ModelProperty); }
            set { SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 工单计划时间 PlanDate
        /// <summary>
        /// 计划开始或计划结束时间在查询范围
        /// </summary>
        [Label("计划时间")]
        public static readonly Property<DateRange> PlanDateProperty = P<WorkOrderReachCriteria>.Register(e => e.PlanDate);

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateRange PlanDate
        {
            get { return GetProperty(PlanDateProperty); }
            set { SetProperty(PlanDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 当前列属性名称
        /// </summary>
        public string ColumnFieldName { get; set; }

        /// <summary>
        /// 当前列属性值
        /// </summary>
        public string ColumnFieldValue { get; set; }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>实体列表</returns>
        protected override EntityList Fetch()
        {
            EntityList<WoReachReportViewModel> modelList = new EntityList<WoReachReportViewModel>();
            modelList.Add(RT.Service.Resolve<WorkOrderReachController>().GetWorkOrderReach(this));
            return modelList;
        }

        #region 类型 DateType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<DateType> DateTypeProperty = P<WorkOrderReachCriteria>.Register(e => e.DateType);

        /// <summary>
        /// 类型
        /// </summary>
        public DateType DateType
        {
            get { return this.GetProperty(DateTypeProperty); }
            set { this.SetProperty(DateTypeProperty, value); }
        }
        #endregion
    }
}
