using SIE.Defects;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("可疑品标签查询")]
    public class SuspectProductLabelCriteria : Criteria
    {
        #region 任务单号 DispatchTaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> DispatchTaskNoProperty = P<SuspectProductLabelCriteria>.Register(e => e.DispatchTaskNo);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string DispatchTaskNo
        {
            get { return this.GetProperty(DispatchTaskNoProperty); }
            set { this.SetProperty(DispatchTaskNoProperty, value); }
        }
        #endregion

        #region 可疑品标签 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("可疑品标签")]
        public static readonly Property<string> BatchNoProperty = P<SuspectProductLabelCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<SuspectProductLabelCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<SuspectProductLabelCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<SuspectProductLabelCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<SuspectProductLabelCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单状态 WoState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<string> WoStateProperty = P<SuspectProductLabelCriteria>.Register(e => e.WoState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public string WoState
        {
            get { return this.GetProperty(WoStateProperty); }
            set { this.SetProperty(WoStateProperty, value); }
        }
        #endregion

        #region 是否显示关闭工单 ShowCloseWo
        /// <summary>
        /// 是否显示关闭工单
        /// </summary>
        [Label("是否显示关闭工单")]
        public static readonly Property<bool> ShowCloseWoProperty = P<SuspectProductLabelCriteria>.Register(e => e.ShowCloseWo);

        /// <summary>
        /// 是否显示关闭工单
        /// </summary>
        public bool ShowCloseWo
        {
            get { return this.GetProperty(ShowCloseWoProperty); }
            set { this.SetProperty(ShowCloseWoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<SuspectProductLabelCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 旧物料编码 OldProductCode
        /// <summary>
        /// 旧物料编码
        /// </summary>
        [Label("旧物料编码")]
        public static readonly Property<string> OldProductCodeProperty = P<SuspectProductLabelCriteria>.Register(e => e.OldProductCode);

        /// <summary>
        /// 旧物料编码
        /// </summary>
        public string OldProductCode
        {
            get { return this.GetProperty(OldProductCodeProperty); }
            set { this.SetProperty(OldProductCodeProperty, value); }
        }
        #endregion

        #region 来源标签 ProcessBatchNo
        /// <summary>
        /// 来源标签
        /// </summary>
        [Label("来源标签")]
        public static readonly Property<string> ProcessBatchNoProperty = P<SuspectProductLabelCriteria>.Register(e => e.ProcessBatchNo);

        /// <summary>
        /// 来源标签
        /// </summary>
        public string ProcessBatchNo
        {
            get { return this.GetProperty(ProcessBatchNoProperty); }
            set { this.SetProperty(ProcessBatchNoProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人Id
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreateByIdProperty =
            P<SuspectProductLabelCriteria>.RegisterRefId(e => e.CreateById, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? CreateById
        {
            get { return (double?)this.GetRefNullableId(CreateByIdProperty); }
            set { this.SetRefNullableId(CreateByIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateByProperty =
            P<SuspectProductLabelCriteria>.RegisterRef(e => e.CreateBy, CreateByIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee CreateBy
        {
            get { return this.GetRefEntity(CreateByProperty); }
            set { this.SetRefEntity(CreateByProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<SuspectProductLabelCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 可疑品子标签 SubBatchNo
        /// <summary>
        /// 可疑品子标签
        /// </summary>
        [Label("可疑品子标签")]
        public static readonly Property<string> SubBatchNoProperty = P<SuspectProductLabelCriteria>.Register(e => e.SubBatchNo);

        /// <summary>
        /// 可疑品子标签
        /// </summary>
        public string SubBatchNo
        {
            get { return this.GetProperty(SubBatchNoProperty); }
            set { this.SetProperty(SubBatchNoProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<SuspectProductLabelCriteria>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double? DefectId
        {
            get { return (double?)this.GetRefNullableId(DefectIdProperty); }
            set { this.SetRefNullableId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty =
            P<SuspectProductLabelCriteria>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 处理人 HandleBy
        /// <summary>
        /// 处理人Id
        /// </summary>
        [Label("处理人")]
        public static readonly IRefIdProperty HandleByIdProperty =
            P<SuspectProductLabelCriteria>.RegisterRefId(e => e.HandleById, ReferenceType.Normal);

        /// <summary>
        /// 处理人Id
        /// </summary>
        public double? HandleById
        {
            get { return (double?)this.GetRefNullableId(HandleByIdProperty); }
            set { this.SetRefNullableId(HandleByIdProperty, value); }
        }

        /// <summary>
        /// 处理人
        /// </summary>
        public static readonly RefEntityProperty<Employee> HandleByProperty =
            P<SuspectProductLabelCriteria>.RegisterRef(e => e.HandleBy, HandleByIdProperty);

        /// <summary>
        /// 处理人
        /// </summary>
        public Employee HandleBy
        {
            get { return this.GetRefEntity(HandleByProperty); }
            set { this.SetRefEntity(HandleByProperty, value); }
        }
        #endregion

        #region 处理时间 HandleDate
        /// <summary>
        /// 处理时间
        /// </summary>
        [Label("处理时间")]
        public static readonly Property<DateRange> HandleDateProperty = P<SuspectProductLabelCriteria>.Register(e => e.HandleDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateRange HandleDate
        {
            get { return this.GetProperty(HandleDateProperty); }
            set { this.SetProperty(HandleDateProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopProperty = P<SuspectProductLabelCriteria>.Register(e => e.WorkShop);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
            set { this.SetProperty(WorkShopProperty, value); }
        }
        #endregion

        #region 处理状态 HandleState
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<SuspectHandleState?> HandleStateProperty = P<SuspectProductLabelCriteria>.Register(e => e.HandleState);

        /// <summary>
        /// 处理状态
        /// </summary>
        public SuspectHandleState? HandleState
        {
            get { return this.GetProperty(HandleStateProperty); }
            set { this.SetProperty(HandleStateProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SuspectProductLabelController>().GetSuspectProductLabels(this);
        }
    }
}
