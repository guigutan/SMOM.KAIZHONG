using DocumentFormat.OpenXml.EMMA;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Common.Resources;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.KZ.Print;
using SIE.KZ.Print.Common;
using SIE.ManagedProperty;
using SIE.MES.ItemEquipAccount;
using SIE.MES.LineAndon;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.PreStartupSetupRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// 生产报工基类 视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产报工基类")]
    public class KZTaskReportViewModelBase : DataCollectionViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZTaskReportViewModelBase()
        {

        }


        /// <summary>
        /// KZ报工帮助类
        /// </summary>
        public KZReportHelper kZReportHelper;

        /// <summary>
        /// IOT报工定时器
        /// </summary>
        public System.Timers.Timer ReportTimer;

        /// <summary>
        /// 标签打印数据
        /// </summary>
        public WipBatchData PrintData { get; set; }

        /// <summary>
        /// 手动报工
        /// </summary>
        public bool IsReportManual { get; set; }


        #region IOT数采模式 IotMode
        /// <summary>
        /// IOT数采模式
        /// </summary>
        [Label("IOT数采模式")]
        public static readonly Property<IotMode> IotModeProperty = P<KZTaskReportViewModelBase>.Register(e => e.IotMode);

        /// <summary>
        /// IOT数采模式
        /// </summary>
        public IotMode IotMode
        {
            get { return this.GetProperty(IotModeProperty); }
            set { this.SetProperty(IotModeProperty, value); }
        }
        #endregion

        #region IOT实体 IotEntity
        /// <summary>
        /// IOT实体
        /// </summary>
        [Label("IOT实体")]
        public static readonly Property<string> IotEntityProperty = P<KZTaskReportViewModelBase>.Register(e => e.IotEntity);

        /// <summary>
        /// IOT实体
        /// </summary>
        public string IotEntity
        {
            get { return this.GetProperty(IotEntityProperty); }
            set { this.SetProperty(IotEntityProperty, value); }
        }
        #endregion

        #region 报工人员 ReportEmployee
        /// <summary>
        /// 报工人员Id
        /// </summary>
        [Label("报工人员")]
        public static readonly IRefIdProperty ReportEmployeeIdProperty =
            P<KZTaskReportViewModelBase>.RegisterRefId(e => e.ReportEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 报工人员Id
        /// </summary>
        public double? ReportEmployeeId
        {
            get { return (double?)this.GetRefNullableId(ReportEmployeeIdProperty); }
            set { this.SetRefNullableId(ReportEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 报工人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReportEmployeeProperty =
            P<KZTaskReportViewModelBase>.RegisterRef(e => e.ReportEmployee, ReportEmployeeIdProperty);

        /// <summary>
        /// 报工人员
        /// </summary>
        public Employee ReportEmployee
        {
            get { return this.GetRefEntity(ReportEmployeeProperty); }
            set { this.SetRefEntity(ReportEmployeeProperty, value); }
        }
        #endregion

        #region 分单数量 Zcode
        /// <summary>
        /// 分单数量
        /// </summary>
        [Label("分单数量")]
        public static readonly Property<decimal> ZcodeProperty = P<KZTaskReportViewModelBase>.Register(e => e.Zcode);

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode
        {
            get { return this.GetProperty(ZcodeProperty); }
            set { this.SetProperty(ZcodeProperty, value); }
        }
        #endregion

        #region 合格数 OkQty
        /// <summary>
        /// 合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> OkQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal OkQty
        {
            get { return GetProperty(OkQtyProperty); }
            set { SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数 NgQty
        /// <summary>
        /// 不合格数
        /// </summary>
        [Label("不合格数")]
        public static readonly Property<decimal> NgQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 剩余最大报工数量 MaxRemainQty
        /// <summary>
        /// 剩余最大报工数量
        /// </summary>
        [Label("剩余最大报工数量")]
        public static readonly Property<decimal> MaxRemainQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.MaxRemainQty);

        /// <summary>
        /// 剩余最大报工数量
        /// </summary>
        public decimal MaxRemainQty
        {
            get { return this.GetProperty(MaxRemainQtyProperty); }
            set { this.SetProperty(MaxRemainQtyProperty, value); }
        }
        #endregion

        #region 最大报工数量 MaxReportQty
        /// <summary>
        /// 最大报工数量
        /// </summary>
        [Label("最大报工数量")]
        public static readonly Property<decimal> MaxReportQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.MaxReportQty);

        /// <summary>
        /// 最大报工数量
        /// </summary>
        public decimal MaxReportQty
        {
            get { return this.GetProperty(MaxReportQtyProperty); }
            set { this.SetProperty(MaxReportQtyProperty, value); }
        }
        #endregion

        #region 工序剩余可报工数 ProcessMaxRemainQty
        /// <summary>
        /// 工序剩余可报工数
        /// </summary>
        [Label("工序剩余可报工数")]
        public static readonly Property<decimal> ProcessMaxRemainQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.ProcessMaxRemainQty);

        /// <summary>
        /// 工序剩余可报工数
        /// </summary>
        public decimal ProcessMaxRemainQty
        {
            get { return this.GetProperty(ProcessMaxRemainQtyProperty); }
            set { this.SetProperty(ProcessMaxRemainQtyProperty, value); }
        }
        #endregion

        #region 余料称重

        #region 物料标签号 Sn
        /// <summary>
        /// 物料标签号
        /// </summary>
        [Label("物料标签号")]
        public static readonly Property<string> SnProperty = P<KZTaskReportViewModelBase>.Register(e => e.Sn);

        /// <summary>
        /// 物料标签号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 物料标签Id ItemLabelId
        /// <summary>
        /// 物料标签Id
        /// </summary>
        [Label("物料标签Id")]
        public static readonly Property<double> ItemLabelIdProperty = P<KZTaskReportViewModelBase>.Register(e => e.ItemLabelId);

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double ItemLabelId
        {
            get { return this.GetProperty(ItemLabelIdProperty); }
            set { this.SetProperty(ItemLabelIdProperty, value); }
        }
        #endregion

        #region 上料记录Id FeedingRecordId
        /// <summary>
        /// 上料记录Id
        /// </summary>
        [Label("上料记录Id")]
        public static readonly Property<double> FeedingRecordIdProperty = P<KZTaskReportViewModelBase>.Register(e => e.FeedingRecordId);

        /// <summary>
        /// 上料记录Id
        /// </summary>
        public double FeedingRecordId
        {
            get { return this.GetProperty(FeedingRecordIdProperty); }
            set { this.SetProperty(FeedingRecordIdProperty, value); }
        }
        #endregion

        #region 物料批次号 Lot
        /// <summary>
        /// 物料批次号
        /// </summary>
        [Label("物料批次号")]
        public static readonly Property<string> LotProperty = P<KZTaskReportViewModelBase>.Register(e => e.Lot);

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
            set { this.SetProperty(LotProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<KZTaskReportViewModelBase>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<KZTaskReportViewModelBase>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料单位 ItemUnit
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("物料单位")]
        public static readonly Property<string> ItemUnitProperty = P<KZTaskReportViewModelBase>.Register(e => e.ItemUnit);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit
        {
            get { return this.GetProperty(ItemUnitProperty); }
            set { this.SetProperty(ItemUnitProperty, value); }
        }
        #endregion

        #region 状态 ItemLabelState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> ItemLabelStateProperty = P<KZTaskReportViewModelBase>.Register(e => e.ItemLabelState);

        /// <summary>
        /// 状态
        /// </summary>
        public string ItemLabelState
        {
            get { return this.GetProperty(ItemLabelStateProperty); }
            set { this.SetProperty(ItemLabelStateProperty, value); }
        }
        #endregion

        #region 上料数量 FeedingQty
        /// <summary>
        /// 上料数量
        /// </summary>
        [Label("上料数量")]
        public static readonly Property<decimal> FeedingQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.FeedingQty);

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal FeedingQty
        {
            get { return this.GetProperty(FeedingQtyProperty); }
            set { this.SetProperty(FeedingQtyProperty, value); }
        }
        #endregion

        #region 下料数量 BlankingQty
        /// <summary>
        /// 下料数量
        /// </summary>
        [Label("下料数量")]
        public static readonly Property<decimal> BlankingQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.BlankingQty);

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal BlankingQty
        {
            get { return this.GetProperty(BlankingQtyProperty); }
            set { this.SetProperty(BlankingQtyProperty, value); }
        }
        #endregion

        #region 理论剩余数量 RemainingQty
        /// <summary>
        /// 理论剩余数量
        /// </summary>
        [Label("理论剩余数量")]
        public static readonly Property<decimal> RemainingQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.RemainingQty);

        /// <summary>
        /// 理论剩余数量
        /// </summary>
        public decimal RemainingQty
        {
            get { return this.GetProperty(RemainingQtyProperty); }
            set { this.SetProperty(RemainingQtyProperty, value); }
        }
        #endregion

        #region 实际重量数量 ActualQty
        /// <summary>
        /// 实际重量数量
        /// </summary>
        [Label("实际重量数量")]
        public static readonly Property<decimal> ActualQtyProperty = P<KZTaskReportViewModelBase>.Register(e => e.ActualQty);

        /// <summary>
        /// 实际重量数量
        /// </summary>
        public decimal ActualQty
        {
            get { return this.GetProperty(ActualQtyProperty); }
            set { this.SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #endregion

        #region 模具 Model
        /// <summary>
        /// 模具
        /// </summary>
        [Label("模具")]
        public static readonly Property<string> ModelProperty = P<KZTaskReportViewModelBase>.Register(e => e.Model);

        /// <summary>
        /// 模具
        /// </summary>
        public string Model
        {
            get { return this.GetProperty(ModelProperty); }
            set { this.SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 模具穴位 ModelCavityCount
        /// <summary>
        /// 模具穴位
        /// </summary>
        [Label("模具穴位")]
        public static readonly Property<int> ModelCavityCountProperty = P<KZTaskReportViewModelBase>.Register(e => e.ModelCavityCount);

        /// <summary>
        /// 模具穴位
        /// </summary>
        public int ModelCavityCount
        {
            get { return this.GetProperty(ModelCavityCountProperty); }
            set { this.SetProperty(ModelCavityCountProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<KZTaskReportViewModelBase>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<KZTaskReportViewModelBase>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<KZTaskReportViewModelBase>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<KZTaskReportViewModelBase>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源名称 MultipleResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> MultipleResourceNameProperty = P<KZTaskReportViewModelBase>.RegisterReadOnly(
            e => e.MultipleResourceName, e => e.GetMultipleResourceName(), ResourceIdProperty);
        /// <summary>
        /// 资源名称
        /// </summary>

        public string MultipleResourceName
        {
            get { return this.GetProperty(MultipleResourceNameProperty); }
        }
        private string GetMultipleResourceName()
        {
            if (Resource == null)
                return "+";
            else
                return Resource.Name;
        }
        #endregion

        #region 是否本地打印 IsLocalPrint
        /// <summary>
        /// 是否本地打印
        /// </summary>
        [Label("是否本地打印")]
        public static readonly Property<bool> IsLocalPrintProperty = P<KZTaskReportViewModelBase>.Register(e => e.IsLocalPrint);

        /// <summary>
        /// 是否本地打印
        /// </summary>
        public bool IsLocalPrint
        {
            get { return this.GetProperty(IsLocalPrintProperty); }
            set { this.SetProperty(IsLocalPrintProperty, value); }
        }
        #endregion

        #region 是否远程打印 IsRemotePrint
        /// <summary>
        /// 是否远程打印
        /// </summary>
        [Label("是否远程打印")]
        public static readonly Property<bool> IsRemotePrintProperty = P<KZTaskReportViewModelBase>.Register(e => e.IsRemotePrint);

        /// <summary>
        /// 是否远程打印
        /// </summary>
        public bool IsRemotePrint
        {
            get { return this.GetProperty(IsRemotePrintProperty); }
            set { this.SetProperty(IsRemotePrintProperty, value); }
        }
        #endregion

        #region Printer 打印机
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<KZTaskReportViewModelBase>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }
        #endregion

        #region Template 模板
        /// <summary>
        /// 模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty = P<KZTaskReportViewModelBase>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<KZTaskReportViewModelBase>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 是否显示加载动画 IsShowLoading
        /// <summary>
        /// 是否显示加载动画
        /// </summary>
        [Label("是否显示加载动画")]
        public static readonly Property<bool> IsShowLoadingProperty = P<KZTaskReportViewModelBase>.Register(e => e.IsShowLoading);

        /// <summary>
        /// 是否显示加载动画
        /// </summary>
        public bool IsShowLoading
        {
            get { return this.GetProperty(IsShowLoadingProperty); }
            set { this.SetProperty(IsShowLoadingProperty, value); }
        }
        #endregion

        #region 报工资源列表 ResourceList
        /// <summary>
        /// 报工资源列表
        /// </summary>
        public static readonly ListProperty<EntityList<WipResource>> ResourceListProperty = P<KZTaskReportViewModelBase>.RegisterList(e => e.ResourceList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTask) => { return new EntityList<WipResource>(); }
        });

        /// <summary>
        /// 报工资源列表
        /// </summary>
        public EntityList<WipResource> ResourceList
        {
            get { return this.GetLazyList(ResourceListProperty); }
        }
        #endregion

        #region 任务队列 DispatchTaskQueue
        /// <summary>
        /// 任务队列Id
        /// </summary>
        [Label("任务队列")]
        public static readonly IRefIdProperty DispatchTaskQueueIdProperty =
            P<KZTaskReportViewModelBase>.RegisterRefId(e => e.DispatchTaskQueueId, ReferenceType.Normal);

        /// <summary>
        /// 任务队列Id
        /// </summary>
        public double? DispatchTaskQueueId
        {
            get { return (double?)this.GetRefNullableId(DispatchTaskQueueIdProperty); }
            set { this.SetRefNullableId(DispatchTaskQueueIdProperty, value); }
        }

        /// <summary>
        /// 任务队列
        /// </summary>
        public static readonly RefEntityProperty<DispatchTaskQueue> DispatchTaskQueueProperty =
            P<KZTaskReportViewModelBase>.RegisterRef(e => e.DispatchTaskQueue, DispatchTaskQueueIdProperty);

        /// <summary>
        /// 任务队列
        /// </summary>
        public DispatchTaskQueue DispatchTaskQueue
        {
            get { return this.GetRefEntity(DispatchTaskQueueProperty); }
            set { this.SetRefEntity(DispatchTaskQueueProperty, value); }
        }
        #endregion

        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<KZTaskReportViewModelBase>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return (double?)this.GetRefNullableId(DispatchTaskIdProperty); }
            set { this.SetRefNullableId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<KZTaskReportViewModelBase>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 派工任务列表 DispatchTaskList
        /// <summary>
        /// 报工资源列表
        /// </summary>
        public static readonly ListProperty<EntityList<DispatchTask>> DispatchTaskListProperty = P<KZTaskReportViewModelBase>.RegisterList(e => e.DispatchTaskList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTask) => { return new EntityList<DispatchTask>(); }
        });

        /// <summary>
        /// 报工资源列表
        /// </summary>
        public EntityList<DispatchTask> DispatchTaskList
        {
            get { return this.GetLazyList(DispatchTaskListProperty); }
        }
        #endregion

        #region 生产任务队列 DispatchTaskQueueList
        /// <summary>
        /// 生产任务队列
        /// </summary>
        [Label("生产任务队列")]
        public static readonly ListProperty<EntityList<DispatchTaskQueue>> DispatchTaskQueueListProperty = P<KZTaskReportViewModelBase>.RegisterList(e => e.DispatchTaskQueueList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTaskQueue) => { return new EntityList<DispatchTaskQueue>(); }
        });

        /// <summary>
        /// 生产任务队列
        /// </summary>
        public EntityList<DispatchTaskQueue> DispatchTaskQueueList
        {
            get { return this.GetLazyList(DispatchTaskQueueListProperty); }
        }
        #endregion

        #region 共模任务队列 ReportTaskQueueList
        /// <summary>
        /// 共模任务队列
        /// </summary>
        [Label("共模任务队列")]
        public static readonly ListProperty<EntityList<DispatchTaskQueue>> ReportTaskQueueListProperty = P<KZTaskReportViewModelBase>.RegisterList(e => e.ReportTaskQueueList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTaskQueue) => { return new EntityList<DispatchTaskQueue>(); }
        });

        /// <summary>
        /// 共模任务队列
        /// </summary>
        public EntityList<DispatchTaskQueue> ReportTaskQueueList
        {
            get { return this.GetLazyList(ReportTaskQueueListProperty); }
        }
        #endregion

        #region 上料任务列表 FeedingDispatchTaskList
        /// <summary>
        /// 上料任务列表
        /// </summary>
        public static readonly ListProperty<EntityList<DispatchTask>> FeedingDispatchTaskListProperty = P<KZTaskReportViewModelBase>.RegisterList(e => e.FeedingDispatchTaskList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTask) => { return new EntityList<DispatchTask>(); }
        });

        /// <summary>
        /// 上料任务列表
        /// </summary>
        public EntityList<DispatchTask> FeedingDispatchTaskList
        {
            get { return this.GetLazyList(FeedingDispatchTaskListProperty); }
        }
        #endregion

        #region 是否手动报工 ManualReport
        /// <summary>
        /// 是否手动报工
        /// </summary>
        [Label("是否手动报工")]
        public static readonly Property<bool> ManualReportProperty = P<KZTaskReportViewModelBase>.Register(e => e.ManualReport);

        /// <summary>
        /// 是否手动报工
        /// </summary>
        public bool ManualReport
        {
            get { return this.GetProperty(ManualReportProperty); }
            set { this.SetProperty(ManualReportProperty, value); }
        }
        #endregion

        #region 上料任务单 FeedingDispatchTask
        /// <summary>
        /// 上料任务单Id
        /// </summary>
        [Label("上料任务单")]
        public static readonly IRefIdProperty FeedingDispatchTaskIdProperty =
            P<KZTaskReportViewModelBase>.RegisterRefId(e => e.FeedingDispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 上料任务单Id
        /// </summary>
        public double? FeedingDispatchTaskId
        {
            get { return (double?)this.GetRefNullableId(FeedingDispatchTaskIdProperty); }
            set { this.SetRefNullableId(FeedingDispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 上料任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> FeedingDispatchTaskProperty =
            P<KZTaskReportViewModelBase>.RegisterRef(e => e.FeedingDispatchTask, FeedingDispatchTaskIdProperty);

        /// <summary>
        /// 上料任务单
        /// </summary>
        public DispatchTask FeedingDispatchTask
        {
            get { return this.GetRefEntity(FeedingDispatchTaskProperty); }
            set { this.SetRefEntity(FeedingDispatchTaskProperty, value); }
        }
        #endregion

        #region 提示信息

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public override void ShowError(string error)
        {
            ClearInfos();
            if (error == null)
            {
                return;
            }
            string errMsg = error;
            if (errMsg.Contains("执行失败:"))
            {
                var errMsgArr = errMsg.Split(new String[] { "执行失败:" }, StringSplitOptions.RemoveEmptyEntries);
                if (errMsgArr.Length > 1)
                    errMsg = errMsgArr[1];
            }
            Error = errMsg.Replace("\r\n", string.Empty);

            if (IotMode != IotMode.MultiStation)
            {
                if (ReportTimer?.Enabled == true)
                    CRT.MessageService.ShowInstantMessage(errMsg, "错误", 5);
                else
                    CRT.MessageService.ShowError(errMsg);
            }
        }
        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            if (IotMode == IotMode.MultiStation)
                ShowError(message); //多工位模式,不适合弹窗提示
            else
                CRT.MessageService.ShowMessage(message);
        }
        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="timeout"></param>
        public void ShowInstantMessage(string message, string caption = null, int timeout = 15)
        {
            if (IotMode == IotMode.MultiStation)
                ShowError(message); //多工位模式,不适合弹窗提示
            else
                CRT.MessageService.ShowInstantMessage(message, caption, timeout);
        }
        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="tips">提示信息</param>
        public new void ShowTips(string tips)
        {
            if (tips == null)
            {
                return;
            }
            ClearInfos();
            Tips = tips.Replace("\r\n", string.Empty);

            CRT.MessageService.ShowMessage(Tips);
        }

        /// <summary>
        /// 清空提示信息
        /// </summary>
        protected override void ClearInfos()
        {
            Error = null;
            Tips = null;
        }

        #endregion

        #region 加载数据列表

        #region 获取资源列表
        /// <summary>
        /// 获取资源列表
        /// </summary>
        public virtual void LoadResourceList(bool loadAll = false,string keyword = "")
        {
            if (ReportEmployee == null)
                return;

            var resourceIds = new List<double>();

            if (!loadAll)
            {
                TaskQueryInfo info = new TaskQueryInfo()
                {
                    EmployeeId = ReportEmployee.Id,
                    //ResourceId = ResourceId,
                    TaskType = 1,
                };
                var status = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Dispatching, DispatchTaskStatus.ToDispatch, DispatchTaskStatus.Executing, DispatchTaskStatus.Pause/*, DispatchTaskStatus.Finished, DispatchTaskStatus.Closed*/ };
                PagingInfo pagingInfo = new PagingInfo(info.PageNumber ?? 1, info.PageSize ?? int.MaxValue - 1, true);
                var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByEmployee(info, status, pagingInfo, true);
                resourceIds = tasks.Select(p => p.ResourceId ?? 0).Distinct().ToList(); //有任务的资源Id
            }
            var resources = RT.Service.Resolve<WipResourceController>().GetWipResources(ReportEmployee.Id,null,keyword); //员工有权限的资源

            CRT.MainThread.InvokeIfRequired(() =>
            {
                ResourceList.Clear();
                
                if (resourceIds.Count > 0)
                    ResourceList.AddRange(resources.Where(p => resourceIds.Contains(p.Id)).OrderBy(p => p.Name));
                else
                    ResourceList.AddRange(resources.OrderBy(p => p.Name));
            });
        }
        #endregion

        #region 获取任务单列表

        /// <summary>
        /// 获取任务单
        /// </summary>
        public virtual DispatchTask LoadTask(double? taskId)
        {
            var dispatchTask = RF.GetById<DispatchTask>(taskId, new EagerLoadOptions().LoadWithViewProperty());
            RefreshMaxRemainQty(dispatchTask);
            SetParShortDescription(dispatchTask);

            return dispatchTask;
        }
        /// <summary>
        /// 获取旧料号
        /// </summary>
        /// <param name="dispatchTask"></param>
        protected void SetParShortDescription(DispatchTask dispatchTask)
        {
            if (dispatchTask == null)
                return;
            var parentItem = RT.Service.Resolve<ItemController>().GetParentItemByItemId(dispatchTask.ProductId);
            if (parentItem != null)
                dispatchTask.ParShortDescription = parentItem.Bismt;
        }

        /// <summary>
        /// 刷新最大剩余报工数量
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <returns></returns>
        protected void RefreshMaxRemainQty(DispatchTask dispatchTask)
        {
            MaxReportQty = 0;
            MaxRemainQty = 0;
            ProcessMaxRemainQty = 0;
            if (dispatchTask != null)
            {
                var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(dispatchTask);
                MaxReportQty = tuple.Item1;
                MaxRemainQty = tuple.Item2;
                ProcessMaxRemainQty = RT.Service.Resolve<DispatchController>().GetProcessMaxRemainQty(dispatchTask);
            }
        }

        /// <summary>
        /// 获取任务单列表
        /// </summary>
        public virtual void LoadTaskList()
        {
            if (ReportEmployee == null)
                return;
            var queueTaskIds = DispatchTaskQueueList.Select(p => p.DispatchTaskId).ToList();

            TaskQueryInfo info = new TaskQueryInfo()
            {
                EmployeeId = ReportEmployee.Id,
                ResourceId = ResourceId,
                TaskType = 1,
            };
            var status = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Dispatching, DispatchTaskStatus.ToDispatch, DispatchTaskStatus.Executing, DispatchTaskStatus.Pause/*, DispatchTaskStatus.Finished, DispatchTaskStatus.Closed*/ };
            PagingInfo pagingInfo = new PagingInfo(info.PageNumber ?? 1, info.PageSize ?? int.MaxValue - 1, true);
            var firstProcess = false;
            //var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            //if (this.IsReportManual == true && config != null && config.IsFirstProcess == true)
            //{
            //    firstProcess = true;
            //}
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByEmployee(info, status, pagingInfo, !firstProcess, firstProcess).OrderBy(p => p.ProductCode).ThenBy(p => p.PlanEndTime).ToList();

            CRT.MainThread.InvokeIfRequired(() =>
            {
                DispatchTaskList.Clear();
                if (IsReportManual)
                    DispatchTaskList.AddRange(tasks);
                else
                    DispatchTaskList.AddRange(tasks.Where(p => !queueTaskIds.Contains(p.Id)));
                DispatchTaskList.MarkSaved();
            });
        }
        #endregion

        #region 获取生产队列

        /// <summary>
        /// 获取生产队列
        /// </summary>
        public virtual void LoadTaskQueueList(EntityList<DispatchTaskQueue> queues = null)
        {
            if (Resource == null)
                return;

            if (queues == null)
                queues = RT.Service.Resolve<DispatchController>().GetDispatchTaskQueueList(Resource.Id, false, new EagerLoadOptions().LoadWithViewProperty());

            CRT.MainThread.InvokeIfRequired(() =>
            {
                DispatchTaskQueueList.Clear();
                DispatchTaskQueueList.AddRange(queues);
                DispatchTaskQueueList.MarkSaved();
            });
        }

        /// <summary>
        /// 添加生产队列任务
        /// </summary>
        /// <param name="task"></param>
        public virtual void AddQueueTask(DispatchTask task)
        {
            if (task == null)
                return;
            if (DispatchTaskQueueList.Any(p => p.DispatchTaskId == task.Id))
            {
                ShowError("生产队列已存在该任务,请勿重复加入");
                return;
            }


            if (IotMode == IotMode.CommonMode)
            {
                //共模验证模具组
                var productIds = DispatchTaskQueueList.Select(p => p.DispatchTask.ProductId).Distinct().ToList();
                RT.Service.Resolve<EquipAccountItemController>().ValidateModelGroup(task.ProductId, productIds);
            }
            else
            {
                if (DispatchTaskQueueList.Any(p => p.DispatchTask.ProductId != task.ProductId))
                {
                    ShowError("该任务的产品与生产队列中产品不一致,不允许加入");
                    return;
                }
            }

            var maxSeq = RT.Service.Resolve<DispatchController>().GetMaxDispatchTaskQueueSeq(Resource.Id);
            var queue = new DispatchTaskQueue()
            {
                Seq = maxSeq + 10,
                DispatchTask = task,
                Resource = Resource
            };
            RF.Save(queue);
            CRT.MessageService.ShowInstantMessage("加入成功".L10nFormat(), "提示", 1);
            DispatchTaskList.Remove(task);
            LoadTaskQueueList();
        }

        /// <summary>
        /// 移除生产队列任务
        /// </summary>
        /// <param name="queue"></param>
        public virtual void RemoveQueueTask(DispatchTaskQueue queue)
        {
            if (queue == null)
                return;
            if (queue.TaskStatus == DispatchTaskStatus.Executing)
            {
                throw new ValidationException("执行中任务请先暂停后再操作,请确认");
            }
            queue.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(queue);
            DispatchTaskQueueList.Remove(queue);
            DispatchTaskQueueList.MarkSaved();
        }

        /// <summary>
        /// 完成生产队列任务
        /// </summary>
        /// <param name="queue"></param>
        public virtual void FinishQueueTask(DispatchTaskQueue queue)
        {
            if (queue == null)
                return;
            queue.IsFinished = true;
            queue.PersistenceStatus = PersistenceStatus.Modified;
            RF.Save(queue);
            var queues = DispatchTaskQueueList.Where(p => p.Id != queue.Id).ToList();
            CRT.MainThread.InvokeIfRequired(() =>
            {
                DispatchTaskQueueList.Clear();
                DispatchTaskQueueList.AddRange(queues);
                DispatchTaskQueueList.MarkSaved();
            });
        }

        /// <summary>
        /// 完成生产队列任务
        /// </summary>
        /// <param name="queueList"></param>
        public virtual void FinishQueueTask(EntityList<DispatchTaskQueue> queueList)
        {
            if (queueList == null || queueList.Count == 0)
                return;
            queueList.ForEach(queue =>
            {
                queue.IsFinished = true;
                queue.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(queue);
            });
            var ids = queueList.Select(p => p.Id).ToList();
            var queues = DispatchTaskQueueList.Where(p => !ids.Contains(p.Id)).ToList();
            DispatchTaskQueueList.Clear();
            DispatchTaskQueueList.AddRange(queues);
            DispatchTaskQueueList.MarkSaved();
        }
        /// <summary>
        /// 加载首个队列任务
        /// </summary>
        public virtual void LoadFirstQueueTask()
        {
            DispatchTaskQueue = null;
            DispatchTask = null;
            if (DispatchTaskQueueList.Count == 0)
            {
                throw new ValidationException("生产队列中没有生产的任务,请确认");
            }

            DispatchTaskQueue = DispatchTaskQueueList.OrderBy(p => p.Seq).FirstOrDefault(p => !p.IsFinished && p.TaskStatus != DispatchTaskStatus.Finished);
            if (DispatchTaskQueue == null)
                throw new ValidationException("生产队列中没有可生产的任务,请确认");

            DispatchTask = LoadTask(DispatchTaskQueue.DispatchTaskId);

        }
        #endregion

        #region 获取上料下料信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        public virtual void LoadScanFeeding(double taskId)
        {
            var task = RT.Service.Resolve<DispatchController>().GetDispatchTask(taskId);

            CRT.MainThread.InvokeIfRequired(() =>
            {
                FeedingDispatchTask = task;
            });
        }

        /// <summary>
        /// 获取下料标签信息列表
        /// </summary>
        //public virtual void LoadDeductionList()
        //{
        //    if (ResourceId == null)
        //        return;
        //    var pdaInfos = RT.Service.Resolve<DispatchController>().BlankingGetFeedRecords(ResourceId.Value, null);
        //    foreach (var pdaInfo in pdaInfos)
        //    {
        //        CsDeductionLabelInfo info = new CsDeductionLabelInfo();

        //    }
        //}

        /// <summary>
        /// 获取上料任务单列表
        /// </summary>
        public virtual void LoadFeedingTaskList()
        {
            if (ResourceId == null)
                return;

            var PdaInfos = RT.Service.Resolve<DispatchController>().AssemblyGetTasks(ResourceId.Value, null);
            var taskIds = PdaInfos.Select(p => p.TaskId).Distinct().ToList();
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(taskIds);
            //判断主界面是否已经选择了某个任务单，如果存在就将主界面任务单置顶
            if (DispatchTask != null)
            {
                //判断是否待上料任务中是否存在主界面上的任务单
                var task = tasks.FirstOrDefault(p => p.Id == DispatchTask.Id);
                if (task != null)
                {
                    //存在就先将它从数据中移除，然后在插入，这样能保证永远置顶
                    tasks.Remove(task);
                    tasks.Insert(0, task);
                }
            }

            CRT.MainThread.InvokeIfRequired(() =>
            {
                FeedingDispatchTaskList.Clear();
                FeedingDispatchTaskList.AddRange(tasks);
            });
        }
        #endregion

        #endregion

        #region IOT报工定时器

        /// <summary>
        /// 启动IOT报工定时器
        /// </summary>
        public virtual void StartIOTReportTimer()
        {
            if (ReportTimer == null)
            {
                ReportTimer = new System.Timers.Timer();
                ReportTimer.AutoReset = true;
                ReportTimer.Elapsed += (s, e) =>
                {
                    ReportTimer.Interval = 15000;
                    ReportTimerElapsed();
                };
                ReportTimer.Interval = 1000;
            }
            if (ReportTimer.Enabled == false)
                ReportTimer.Enabled = true;
            IsShowLoading = true;
        }

        /// <summary>
        /// 停止IOT报工定时器
        /// </summary>
        public void StopIOTReportTimer()
        {
            if (ReportTimer != null && ReportTimer.Enabled == true)
            {
                ReportTimer.Enabled = false;
            }
            IsShowLoading = false;
        }

        /// <summary>
        /// 定时报工事件
        /// </summary>
        public virtual void ReportTimerElapsed()
        {
            throw new ValidationException("未实现!");

        }

        #endregion

        /// <summary>
        /// 开工
        /// </summary>
        /// <returns></returns>
        public virtual async Task StartWork()
        {
            throw new ValidationException("未实现!");

        }
        /// <summary>
        /// 报工
        /// </summary>
        /// <param name="okQty"></param>
        /// <param name="suspectQty"></param>
        /// <param name="isIotReport"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<PdaPrintInfo> TaskReport(decimal okQty, decimal suspectQty, bool? isIotReport = null, Action action = null)
        {
            try
            {
                if (okQty == 0 && suspectQty == 0)
                    throw new ValidationException("请输入报工数或可疑品数");

                SubmitPdaReportInfo reportInfo = new SubmitPdaReportInfo()
                {
                    DispatchTaskId = DispatchTaskId ?? 0,
                    ReportQty = okQty,
                    GoodQty = okQty,
                    SuspectQty = suspectQty,
                    ReportEmployeeId = ReportEmployee.Id,
                    IsValidatePrepare = ((DispatchTaskQueue?.Seq == 10 && isIotReport == true) || isIotReport != true) ? true : false, //isIotReport=false的时候(即手动报工)，还要判断是当前队列任务的序号是不是10，是的就要做开机准备，其他情况不需要
                    IsTaskFinish = true,
                    ResourceId = ResourceId ?? 0,
                    SourceType = isIotReport == true ? SIE.MES.TaskManagement.Reports.Enums.SourceType.Report_IOT : SIE.MES.TaskManagement.Reports.Enums.SourceType.Report_Manual,
                };
                //手动报工的时候，才要弹窗，不要影响其他
                if (isIotReport != true)
                {
                    var msg = RT.Service.Resolve<ReportController>().ReportValid(reportInfo);
                    if (!msg.IsNullOrEmpty())
                    {
                        if (CRT.MessageService.AskQuestion(msg, "确认"))
                        {
                            reportInfo.IsTaskFinish = true;
                        }
                        else
                        {
                            reportInfo.IsTaskFinish = false;
                        }
                    }
                }
                if (isIotReport != null)
                {
                    reportInfo.IsReportManual = false;
                }

                //提交报工
                List<PdaPrintInfo> printInfos = RT.Service.Resolve<ReportController>().SubmitPdaReportInfo(reportInfo);

                //DispatchTask = LoadTask(DispatchTaskId);

                SuspectQty = 0;

                action?.Invoke();

                return printInfos;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                DispatchTask = LoadTask(DispatchTaskId);
            }
        }
        /// <summary>
        /// 异常报工
        /// </summary>
        public virtual void ExceptionReport()
        {
        }

        #region 暂停/执行
        /// <summary>
        /// 暂停
        /// </summary>
        public virtual bool PauseDispatchTasks(List<double> taskIds, WipResource resource)
        {
            try
            {
                var tasks = RT.Service.Resolve<ReportController>().PauseIOTWorkTask(taskIds, resource);

                return true;

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual bool PauseDispatchTask(DispatchTask task, WipResource resource)
        {
            try
            {
                task = RT.Service.Resolve<ReportController>().PauseIOTWorkTask(task, resource);

                return task.TaskStatus == DispatchTaskStatus.Pause;

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            return false;
        }


        #endregion

        #region 打印

        /// <summary>
        /// 测试打印
        /// </summary>
        void testPrint()
        {

            var hostName = Dns.GetHostName();
            PrintData = new KZ.Print.Common.WipBatchData()
            {
                InvOrgId = RT.InvOrg.Value,
                DeviceCode = Dns.GetHostAddresses(hostName).FirstOrDefault(p => p.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
                DeviceName = hostName,
                ResourceCode = Resource.Code,
                //PrinterName = Printer,
                Data = new List<KZ.Print.Common.PrintInfo>() { new KZ.Print.Common.PrintInfo() { BatchNo = "GB2509100001", Qty = 1 } }
            };
            kZReportHelper.ShowLabelPrintControl(true);
        }

        /// <summary>
        /// 打印报工批次标签
        /// </summary>
        /// <param name="printInfos"></param>
        /// <param name="autoPrint"></param>
        public void PrintLabels(List<PdaPrintInfo> printInfos, bool autoPrint = false)
        {
            var hostName = Dns.GetHostName();
            PrintData = new KZ.Print.Common.WipBatchData()
            {
                InvOrgId = RT.InvOrg.Value,
                DeviceCode = Dns.GetHostAddresses(hostName).FirstOrDefault(p => p.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
                DeviceName = hostName,
                ResourceCode = Resource.Code,
                //PrinterName = Printer,
                Data = printInfos.Where(p=>p.Qty>0).Select(p => new KZ.Print.Common.PrintInfo()
                {
                    BatchNo = p.BatchNo,
                    IsSuspectProduct = p.IsSuspectProduct,
                    PrintCmd = p.PrintCmd,
                    Qty = p.Qty,
                    ResourceCode = p.ResourceCode,
                    ResourceName = p.ResourceName,
                    PrintTemplateId = p.PrintTemplateId,
                    ProcessCode = p.ProcessCode,
                }).ToList()
            };
            try
            {

                if (IotMode == IotMode.MultiStation)    //多工位模式,不弹框打印
                {
                    PrintData.PrinterName = Printer;
                    PrintLabels(PrintData, IsLocalPrint);
                }
                else
                    kZReportHelper.ShowLabelPrintControl(autoPrint);

                if (autoPrint)
                    StartIOTReportTimer();
            }
            catch (Exception ex)
            {
                StopIOTReportTimer();
                if (CRT.MessageService.AskQuestion("打印失败: {0} \r\n是否进行重试?".FormatArgs(ex.Message), "打印提示"))
                {
                    PrintLabels(printInfos, autoPrint);
                }
                if (autoPrint)
                    StartIOTReportTimer();

            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printData"></param>
        /// <param name="isLocalPrint"></param>
        public virtual void PrintLabels(WipBatchData printData, bool isLocalPrint)
        {

            if (IsLocalPrint)
            {
                //本地打印
                PrintTemplate template = RT.Service.Resolve<PrinterController>().GetConfigPrintTemplate(printData);
                if (template == null)
                    throw new ValidationException("打印模板不存在，请检查相关配置".FormatArgs());

                if (printData.PrinterName.IsNullOrEmpty())
                    throw new ValidationException("打印机不能为空".L10nFormat(printData.ResourceCode));

                //打印数据
                var labelNos = printData.Data.Select(p => p.BatchNo).ToList();
                var labels = RT.Service.Resolve<WipBatchController>().GetWipBatches(labelNos);

                // 2.根据类型获取报表类型
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                // 3.要打印的数据对象实例
                var printable = new WipBatchPrintable();

                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(template.Id);

                foreach (var item in labels)
                {
                    item.PrintProcessCode = printData.Data?.FirstOrDefault(p => p.BatchNo == item.BatchNo)?.ProcessCode;
                }

                report.Print(printable, filePath, printData.PrinterName, () =>
                {
                    return labels;
                }, () =>
                {

                });
            }
            else
            {
                //API打印
                printData.PrinterName = "";
                RT.Service.Resolve<SIE.KZ.Print.PrinterController>().PrintLabels(printData);
            }
        }

        /// <summary>
        /// 获取模具信息
        /// </summary>
        public void LoadModel()
        {
            Model = null;
            ModelCavityCount = 0;
            if (ResourceId > 0)
            {
                //获取模具
                var preRecord = RT.Service.Resolve<PreStartupSetupRecordsController>().GetLastPreStartupSetupRecord(ResourceId ?? 0, SIE.MES.Common.CheckerFixtureType.Mold);
                Model = preRecord?.ToolCode;

            }
            if (Model.IsNotEmpty())
            {
                var equip = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCodeNoAuth(Model);
                int count = 0;
                if (int.TryParse(equip?.Acupoint, out count))
                {
                    ModelCavityCount = count;
                }
            }
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (this is KZScanReportViewModel || this.IotMode == IotMode.Process)  //扫码报工或者过程数采,实体不作处理
                return;

            if (e.Property == ResourceIdProperty)
            {
                Resource = RF.GetById<WipResource>(ResourceId);
                //限定资源列表
                if (Workstation != null)
                {
                    Workstation.Resource = Resource;
                    Workstation.Process = null;
                    Workstation.Station = null;
                    Workstation.Resources.Clear();
                    Workstation.Resources.Add(Resource);
                }

                ClearInfos();
                Printer = null;

                IotEntity = null;
                DispatchTaskQueue = null;
                DispatchTask = null;
                CRT.MainThread.InvokeIfRequired(() =>
                {
                    ReportTaskQueueList.Clear();
                    ReportTaskQueueList.MarkSaved();

                    DispatchTaskQueueList.Clear();
                    DispatchTaskQueueList.MarkSaved();
                });

                Task.Run(() =>
                {
                    //获取模具
                    LoadModel();

                    //根据产线与安灯区域获取打印机
                    var andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(Resource?.Code);
                    if (andonLine != null)
                    {
                        IotEntity = andonLine.AndonEntity;
                        IsLocalPrint = andonLine.IsLocalPrint;
                        Printer = andonLine.IsLocalPrint ? SIE.Common.Properties.Settings.Default.PrinterName : andonLine.PrinterIp;
                    }

                    LoadTaskQueueList();

                    if (DispatchTaskQueueList.Count > 0)
                        LoadFirstQueueTask();

                });

            }
            else if (e.Property == DispatchTaskIdProperty)
            {
                MaxReportQty = 0;
                MaxRemainQty = 0;
                ProcessMaxRemainQty = 0;
                Zcode = 0;
                if (DispatchTaskId == null)
                    return;
                DispatchTask = LoadTask(DispatchTaskId);
                if (DispatchTask != null)
                {
                    //获取工序分单数量
                    if (DispatchTask.WorkOrder != null)
                    {
                        var layoutInfo = DispatchTask.WorkOrder.LayoutInfoList.Where(p => p.ProcessCode == DispatchTask.ProcessCode).FirstOrDefault();
                        Zcode = layoutInfo?.Zcode ?? 1;
                    }
                }
            }
            else if (e.Property == IsLocalPrintProperty)
            {
                IsRemotePrint = !IsLocalPrint;
            }
            else if (e.Property == IsRemotePrintProperty)
            {
                IsLocalPrint = !IsRemotePrint;
            }
        }

    }
}
