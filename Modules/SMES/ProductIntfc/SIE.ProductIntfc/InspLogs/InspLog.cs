using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.InspLogs.Configs;
using SIE.ProductIntfc.InspLogs.Enums;
using SIE.ProductIntfc.InspSettings;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检日志
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(InspLogCallConfig))]
    [ConditionQueryType(typeof(InspLogCriteria))]
    [Label("报检日志")]
    public partial class InspLog : DataEntity
    {
        /// <summary>
        /// 处理方式快码
        /// </summary>
        public const string ProcessModeCataStr = "ProcessModeCataStr";

        #region 报检数量 InspectionQty
        /// <summary>
        /// 报检数量
        /// </summary>
        [Label("报检数量")]
        public static readonly Property<decimal> InspectionQtyProperty = P<InspLog>.Register(e => e.InspectionQty);

        /// <summary>
        /// 报检数量
        /// </summary>
        public decimal InspectionQty
        {
            get { return GetProperty(InspectionQtyProperty); }
            set { SetProperty(InspectionQtyProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty =
            P<InspLog>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)this.GetRefNullableId(CustomerIdProperty); }
            set { this.SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty =
            P<InspLog>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return this.GetRefEntity(CustomerProperty); }
            set { this.SetRefEntity(CustomerProperty, value); }
        }
        #endregion  

        #region 成品报检单号 InspNo
        /// <summary>
        /// 成品报检单号
        /// </summary>
        [Label("成品报检单号")]
        public static readonly Property<string> InspNoProperty = P<InspLog>.Register(e => e.InspNo);

        /// <summary>
        /// 成品报检单号
        /// </summary>
        public string InspNo
        {
            get { return GetProperty(InspNoProperty); }
            set { SetProperty(InspNoProperty, value); }
        }
        #endregion

        #region 报检时间 InspectionDate
        /// <summary>
        /// 报检时间
        /// </summary>
        [Label("报检时间")]
        public static readonly Property<DateTime?> InspectionDateProperty = P<InspLog>.Register(e => e.InspectionDate);

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime? InspectionDate
        {
            get { return GetProperty(InspectionDateProperty); }
            set { SetProperty(InspectionDateProperty, value); }
        }
        #endregion

        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty =
            P<InspLog>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double ShopId
        {
            get { return (double)this.GetRefId(ShopIdProperty); }
            set { this.SetRefId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty =
            P<InspLog>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return this.GetRefEntity(ShopProperty); }
            set { this.SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<InspLog>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 检验时间 InspectDate
        /// <summary>
        /// 检验时间
        /// </summary>
        [Label("检验时间")]
        public static readonly Property<DateTime?> InspectDateProperty = P<InspLog>.Register(e => e.InspectDate);

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? InspectDate
        {
            get { return this.GetProperty(InspectDateProperty); }
            set { this.SetProperty(InspectDateProperty, value); }
        }
        #endregion

        #region 报检类型 InspType
        /// <summary>
        /// 报检类型
        /// </summary>
        [Label("报检类型")]
        public static readonly Property<InspType> InspTypeProperty = P<InspLog>.Register(e => e.InspType);

        /// <summary>
        /// 报检类型
        /// </summary>
        public InspType InspType
        {
            get { return GetProperty(InspTypeProperty); }
            set { SetProperty(InspTypeProperty, value); }
        }
        #endregion

        #region 报检条码日志明细列表 InspBarcodeLogList
        /// <summary>
        /// 报检条码日志明细列表
        /// </summary>
        [Label("报检明细")]
        public static readonly ListProperty<EntityList<InspBarcodeLog>> InspBarcodeLogListProperty = P<InspLog>.RegisterList(e => e.InspBarcodeLogList);

        /// <summary>
        /// 报检条码日志明细列表
        /// </summary>
        public EntityList<InspBarcodeLog> InspBarcodeLogList
        {
            get { return this.GetLazyList(InspBarcodeLogListProperty); }
        }
        #endregion

        #region 操作人 OperateBy
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperateByIdProperty = P<InspLog>.RegisterRefId(e => e.OperateById, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double? OperateById
        {
            get { return (double?)GetRefNullableId(OperateByIdProperty); }
            set { SetRefNullableId(OperateByIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperateByProperty = P<InspLog>.RegisterRef(e => e.OperateBy, OperateByIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee OperateBy
        {
            get { return GetRefEntity(OperateByProperty); }
            set { SetRefEntity(OperateByProperty, value); }
        }
        #endregion

        #region 报检人 OperateByName
        /// <summary>
        /// 报检人
        /// </summary>
        [Label("报检人")]
        public static readonly Property<string> OperateByNameProperty = P<InspLog>.RegisterView(e => e.OperateByName, p => p.OperateBy.Name);

        /// <summary>
        /// 报检人
        /// </summary>
        public string OperateByName
        {
            get { return this.GetProperty(OperateByNameProperty); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<InspLog>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<InspLog>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<InspLog>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<InspLog>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 资源 ResourceCode
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceCodeProperty = P<InspLog>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion 

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<InspLog>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<InspLog>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<InspLog>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 工单类型 WorkOrderType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> WorkOrderTypeProperty = P<InspLog>.RegisterView(e => e.WorkOrderType, p => p.WorkOrder.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType WorkOrderType
        {
            get { return this.GetProperty(WorkOrderTypeProperty); }
            set { this.SetProperty(WorkOrderTypeProperty, value); }
        }
        #endregion

        #region 产品编码 WorkOrderProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> WorkOrderProductCodeProperty = P<InspLog>.RegisterView(e => e.WorkOrderProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string WorkOrderProductCode
        {
            get { return this.GetProperty(WorkOrderProductCodeProperty); }
        }
        #endregion

        #region 产品名称 WorkOrderProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> WorkOrderProductNameProperty = P<InspLog>.RegisterView(e => e.WorkOrderProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string WorkOrderProductName
        {
            get { return this.GetProperty(WorkOrderProductNameProperty); }
        }
        #endregion

        #region 成品检验单号 CheckNo
        /// <summary>
        /// 成品检验单号
        /// </summary>
        [Label("成品检验单号")]
        public static readonly Property<string> CheckNoProperty = P<InspLog>.Register(e => e.CheckNo);

        /// <summary>
        /// 成品检验单号
        /// </summary>
        public string CheckNo
        {
            get { return GetProperty(CheckNoProperty); }
            set { SetProperty(CheckNoProperty, value); }
        }
        #endregion

        #region 成品检验后处理方式 ProcessMode
        /// <summary>
        /// 处理方式
        /// </summary>
        [Label("处理方式")]
        public static readonly Property<string> ProcessModeProperty = P<InspLog>.Register(e => e.ProcessMode);

        /// <summary>
        /// 处理方式
        /// </summary>
        public string ProcessMode
        {
            get { return GetProperty(ProcessModeProperty); }
            set { SetProperty(ProcessModeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<InspLog>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 缺陷ID列表 DefectIds
        /// <summary>
        /// 缺陷ID列表
        /// </summary>
        [Label("缺陷ID列表")]
        public static readonly Property<string> DefectIdsProperty = P<InspLog>.Register(e => e.DefectIds);

        /// <summary>
        /// 缺陷ID列表
        /// </summary>
        public string DefectIds
        {
            get { return this.GetProperty(DefectIdsProperty); }
            set { this.SetProperty(DefectIdsProperty, value); }
        }
        #endregion

        #region 报工记录Id ReportRecordId
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录Id")]
        public static readonly Property<double?> ReportRecordIdProperty = P<InspLog>.Register(e => e.ReportRecordId);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public double? ReportRecordId
        {
            get { return this.GetProperty(ReportRecordIdProperty); }
            set { this.SetProperty(ReportRecordIdProperty, value); }
        }
        #endregion

        #region 任务单 DispatchTaskNo 
        /// <summary>
        /// 任务单
        /// </summary>
        [Label("任务单")]
        public static readonly Property<string> DispatchTaskNoProperty = P<InspLog>.Register(e => e.DispatchTaskNo);

        /// <summary>
        /// 任务单
        /// </summary>
        public string DispatchTaskNo
        {
            get { return this.GetProperty(DispatchTaskNoProperty); }
            set { this.SetProperty(DispatchTaskNoProperty, value); }
        }
        #endregion

        #region 报检状态 InspState 
        /// <summary>
        /// 报检状态
        /// </summary>
        [Label("报检状态")]
        [Required]
        public static readonly Property<InspState?> InspStateProperty = P<InspLog>.Register(e => e.InspState);

        /// <summary>
        /// 报检状态
        /// </summary>
        public InspState? InspState
        {
            get { return this.GetProperty(InspStateProperty); }
            set { this.SetProperty(InspStateProperty, value); }
        }
        #endregion

        #region 检验状态 InspectionStatus
        /// <summary>
        /// 检验状态
        /// </summary>
        [Label("检验状态")]
        public static readonly Property<InspectionStatus> InspectionStatusProperty = P<InspLog>.Register(e => e.InspectionStatus);

        /// <summary>
        /// 检验状态
        /// </summary>
        public InspectionStatus InspectionStatus
        {
            get { return this.GetProperty(InspectionStatusProperty); }
            set { this.SetProperty(InspectionStatusProperty, value); }
        }
        #endregion

        #region 是否传QMS IsCall
        /// <summary>
        /// 是否传QMS
        /// </summary>
        [Label("是否传QMS")]
        public static readonly Property<bool> IsCallProperty = P<InspLog>.Register(e => e.IsCall);

        /// <summary>
        /// 是否传QMS
        /// </summary>
        public bool IsCall
        {
            get { return this.GetProperty(IsCallProperty); }
            set { this.SetProperty(IsCallProperty, value); }
        }
        #endregion

        #region BS
        #region 车间名称 ShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> ShopNameProperty = P<InspLog>.RegisterView(e => e.ShopName, e => e.Shop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string ShopName
        {
            get { return GetProperty(ShopNameProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<InspLog>.RegisterView(e => e.ResourceName, e => e.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> WorkOrderStateProperty = P<InspLog>.RegisterView(e => e.WorkOrderState, e => e.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState WorkOrderState
        {
            get { return GetProperty(WorkOrderStateProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<InspLog>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 报检日志 实体配置
    /// </summary>
    internal class InspLogConfig : EntityConfig<InspLog>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_INSP_LOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}