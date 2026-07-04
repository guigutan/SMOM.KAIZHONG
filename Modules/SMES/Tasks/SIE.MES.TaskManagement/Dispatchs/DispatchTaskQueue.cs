using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 生产任务队列
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产任务队列")]
    public class DispatchTaskQueue : DataEntity
    {
        #region 生产序号 Seq
        /// <summary>
        /// 生产序号
        /// </summary>
        [Label("生产序号")]
        public static readonly Property<int> SeqProperty = P<DispatchTaskQueue>.Register(e => e.Seq);

        /// <summary>
        /// 生产序号
        /// </summary>
        public int Seq
        {
            get { return this.GetProperty(SeqProperty); }
            set { this.SetProperty(SeqProperty, value); }
        }
        #endregion

        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<DispatchTaskQueue>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<DispatchTaskQueue>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<DispatchTaskQueue>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<DispatchTaskQueue>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 是否生产完成 IsFinished
        /// <summary>
        /// 是否生产完成
        /// </summary>
        [Label("是否生产完成")]
        public static readonly Property<bool> IsFinishedProperty = P<DispatchTaskQueue>.Register(e => e.IsFinished);

        /// <summary>
        /// 是否生产完成
        /// </summary>
        public bool IsFinished
        {
            get { return this.GetProperty(IsFinishedProperty); }
            set { this.SetProperty(IsFinishedProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 任务单号 DispatchTaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> DispatchTaskNoProperty = P<DispatchTaskQueue>.RegisterView(e => e.DispatchTaskNo, p => p.DispatchTask.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string DispatchTaskNo
        {
            get { return this.GetProperty(DispatchTaskNoProperty); }
        }

        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<DispatchTaskStatus> TaskStatusProperty = P<DispatchTaskQueue>.RegisterView(e => e.TaskStatus, p => p.DispatchTask.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus TaskStatus
        {
            get { return this.GetProperty(TaskStatusProperty); }
        }

        #endregion

        #region IOT状态 IotStatus
        /// <summary>
        /// IOT状态
        /// </summary>
        [Label("IOT状态")]
        public static readonly Property<IotStatus> IotStatusProperty = P<DispatchTaskQueue>.RegisterView(e => e.IotStatus, p => p.DispatchTask.IotStatus);

        /// <summary>
        /// IOT状态
        /// </summary>
        public IotStatus IotStatus
        {
            get { return this.GetProperty(IotStatusProperty); }
        }

        #endregion

        #region IOT设备数量 IotQty
        /// <summary>
        /// IOT设备数量
        /// </summary>
        [Label("IOT设备数量")]
        public static readonly Property<decimal> IotQtyProperty = P<DispatchTaskQueue>.RegisterView(e => e.IotQty, p => p.DispatchTask.IotQty);

        /// <summary>
        /// IOT设备数量
        /// </summary>
        public decimal IotQty
        {
            get { return this.GetProperty(IotQtyProperty); }
        }

        #endregion

        #region 手工报工数量 ManualReportQty
        /// <summary>
        /// 手工报工数量
        /// </summary>
        [Label("手工报工数量")]
        public static readonly Property<decimal> ManualReportQtyProperty = P<DispatchTaskQueue>.RegisterView(e => e.ManualReportQty, p => p.DispatchTask.ManualReportQty);

        /// <summary>
        /// 手工报工数量
        /// </summary>
        public decimal ManualReportQty
        {
            get { return this.GetProperty(ManualReportQtyProperty); }
        }

        #endregion

        #region 已报工数量 ReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<DispatchTaskQueue>.RegisterView(e => e.ReportQty, p => p.DispatchTask.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return this.GetProperty(ReportQtyProperty); }
        }

        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<DispatchTaskQueue>.RegisterView(e => e.SuspectQty, p => p.DispatchTask.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
        }

        #endregion               

        #region 产品穴位 CavityCount
        /// <summary>
        /// 产品穴位
        /// </summary>
        [Label("产品穴位")]
        public static readonly Property<decimal> CavityCountProperty = P<DispatchTaskQueue>.RegisterView(e => e.CavityCount, p => p.DispatchTask.CavityCount);

        /// <summary>
        /// 产品穴位
        /// </summary>
        public decimal CavityCount
        {
            get { return this.GetProperty(CavityCountProperty); }
        }

        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<DispatchTaskQueue>.RegisterView(e => e.DispatchQty, p => p.DispatchTask.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return this.GetProperty(DispatchQtyProperty); }
        }

        #endregion

        #region 排程完成时间 PlanEndTime
        /// <summary>
        /// 排程完成时间
        /// </summary>
        [Label("排程完成时间")]
        public static readonly Property<DateTime?> PlanEndTimeProperty = P<DispatchTaskQueue>.RegisterView(e => e.PlanEndTime, p => p.DispatchTask.PlanEndTime);

        /// <summary>
        /// 排程完成时间
        /// </summary>
        public DateTime? PlanEndTime
        {
            get { return this.GetProperty(PlanEndTimeProperty); }
        }

        #endregion

        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<DispatchTaskQueue>.RegisterView(e => e.WorkOrderId, p => p.DispatchTask.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }

        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<DispatchTaskQueue>.RegisterView(e => e.WorkOrderNo, p => p.DispatchTask.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion 

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<Core.WorkOrders.WorkOrderState> WorkOrderStateProperty = P<DispatchTaskQueue>.RegisterView(e => e.WorkOrderState, p => p.DispatchTask.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public Core.WorkOrders.WorkOrderState WorkOrderState
        {
            get { return this.GetProperty(WorkOrderStateProperty); }
        }
        #endregion

        #region 工单是否暂停 IsPause
        /// <summary>
        /// 工单是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<DispatchTaskQueue>.RegisterView(e => e.IsPause, p => p.DispatchTask.WorkOrder.IsPause);

        /// <summary>
        /// 工单是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return this.GetProperty(IsPauseProperty); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<DispatchTaskQueue>.RegisterView(e => e.ProductId, p => p.DispatchTask.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DispatchTaskQueue>.RegisterView(e => e.ProductCode, p => p.DispatchTask.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<DispatchTaskQueue>.RegisterView(e => e.ProductName, p => p.DispatchTask.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<DispatchTaskQueue>.RegisterView(e => e.UnitName, p => p.DispatchTask.Product.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }

        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<DispatchTaskQueue>.RegisterView(e => e.ShortDescription, p => p.DispatchTask.Product.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion

        #region 饼重 ItemWeight
        /// <summary>
        /// 饼重
        /// </summary>
        [Label("饼重")]
        public static readonly Property<string> ItemWeightProperty = P<DispatchTaskQueue>.RegisterView(e => e.ItemWeight, p => p.DispatchTask.Product.Weight);

        /// <summary>
        /// 饼重
        /// </summary>
        public string ItemWeight
        {
            get { return this.GetProperty(ItemWeightProperty); }
        }

        #endregion

        #region 净重单位 WeightUnit
        /// <summary>
        /// 净重单位
        /// </summary>
        [Label("净重单位")]
        public static readonly Property<string> WeightUnitProperty = P<DispatchTaskQueue>.RegisterView(e => e.WeightUnit, p => p.DispatchTask.Product.WeightUnit);

        /// <summary>
        /// 净重单位
        /// </summary>
        public string WeightUnit
        {
            get { return this.GetProperty(WeightUnitProperty); }
        }

        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<DispatchTaskQueue>.RegisterView(e => e.ItemExtPropName, p => p.DispatchTask.WorkOrder.ItemExtPropName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
        }
        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<DispatchTaskQueue>.RegisterView(e => e.MrpController, p => p.DispatchTask.Product.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<DispatchTaskQueue>.RegisterView(e => e.ProcessCode, p => p.DispatchTask.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<DispatchTaskQueue>.RegisterView(e => e.ProcessName, p => p.DispatchTask.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 产品规格型号名称 SpecificationModelName
        /// <summary>
        /// 产品规格型号名称
        /// </summary>
        [Label("产品规格型号名称")]
        public static readonly Property<string> SpecificationModelNameProperty = P<DispatchTaskQueue>.RegisterView(e => e.SpecificationModelName, p => p.DispatchTask.Product.SpecificationModel);

        /// <summary>
        /// 产品规格型号名称
        /// </summary>
        public string SpecificationModelName
        {
            get { return this.GetProperty(SpecificationModelNameProperty); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<DispatchTaskQueue>.RegisterView(e => e.WorkShopName, p => p.DispatchTask.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<DispatchTaskQueue>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<DispatchTaskQueue>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 生产管理者 Fevor
        /// <summary>
        /// 生产管理者
        /// </summary>
        [Label("生产管理者")]
        public static readonly Property<string> FevorProperty = P<DispatchTaskQueue>.RegisterView(e => e.Fevor, p => p.DispatchTask.WorkOrder.Fevor);

        /// <summary>
        /// 生产管理者
        /// </summary>
        public string Fevor
        {
            get { return this.GetProperty(FevorProperty); }
        }
        #endregion

        #region 交货容差 Uebto
        /// <summary>
        /// 交货容差
        /// </summary>
        [Label("交货容差")]
        public static readonly Property<string> UebtoProperty = P<DispatchTaskQueue>.RegisterView(e => e.Uebto, p => p.DispatchTask.WorkOrder.Uebto);

        /// <summary>
        /// 交货容差
        /// </summary>
        public string Uebto
        {
            get { return this.GetProperty(UebtoProperty); }
        }

        #endregion

        #region 资源类型 ResourceSourceType
        /// <summary>
        /// 资源类型
        /// </summary>
        [Label("资源类型")]
        public static readonly Property<SyncSourceType?> ResourceSourceTypeProperty = P<DispatchTaskQueue>.RegisterView(e => e.ResourceSourceType, p => p.Resource.SourceType);

        /// <summary>
        /// 资源类型
        /// </summary>
        public SyncSourceType? ResourceSourceType
        {
            get { return this.GetProperty(ResourceSourceTypeProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库

        #region 是否选中 IsSelected
        /// <summary>
        /// 是否选中
        /// </summary>
        [Label("是否选中")]
        public static readonly Property<bool> IsSelectedProperty = P<DispatchTaskQueue>.Register(e => e.IsSelected);

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return this.GetProperty(IsSelectedProperty); }
            set { this.SetProperty(IsSelectedProperty, value); }
        }
        #endregion

        #region 父级旧料号 ParShortDescription
        /// <summary>
        /// 父级旧料号
        /// </summary>
        [Label("父级旧料号")]
        public static readonly Property<string> ParShortDescriptionProperty = P<DispatchTaskQueue>.Register(e => e.ParShortDescription);

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string ParShortDescription
        {
            get { return this.GetProperty(ParShortDescriptionProperty); }
            set { this.SetProperty(ParShortDescriptionProperty, value); }
        }
        #endregion

        #region 实际报工数 ActualReportQty
        /// <summary>
        /// 实际报工数
        /// </summary>
        [Label("实际报工数")]
        public static readonly Property<decimal> ActualReportQtyProperty = P<DispatchTaskQueue>.RegisterReadOnly(
            e => e.ActualReportQty, e => e.GetActualReportQty(), ReportQtyProperty, SuspectQtyProperty);
        /// <summary>
        /// 实际报工数
        /// </summary>

        public decimal ActualReportQty
        {
            get { return this.GetProperty(ActualReportQtyProperty); }
        }
        private decimal GetActualReportQty()
        {
            return ReportQty + SuspectQty;
        }
        #endregion

        #region 报工进度 ReportProgress
        /// <summary>
        /// 报工进度
        /// </summary>
        [Label("报工进度")]
        public static readonly Property<decimal> ReportProgressProperty = P<DispatchTaskQueue>.RegisterReadOnly(
            e => e.ReportProgress, e => e.GetReportProgress(), ReportQtyProperty, SuspectQtyProperty, DispatchQtyProperty);
        /// <summary>
        /// 报工进度
        /// </summary>

        public decimal ReportProgress
        {
            get { return this.GetProperty(ReportProgressProperty); }
        }
        private decimal GetReportProgress()
        {
            if (DispatchQty == 0)
                return 0;
            return Math.Round(ActualReportQty * 100 / DispatchQty, 2);
        }
        #endregion

        #region 分单数量 Zcode
        /// <summary>
        /// 分单数量
        /// </summary>
        [Label("分单数量")]
        public static readonly Property<decimal> ZcodeProperty = P<DispatchTaskQueue>.Register(e => e.Zcode);

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode
        {
            get { return this.GetProperty(ZcodeProperty); }
            set { this.SetProperty(ZcodeProperty, value); }
        }
        #endregion

        #region IOT可疑品数 IotSuspectQty
        /// <summary>
        /// IOT可疑品数
        /// </summary>
        [Label("IOT可疑品数")]
        public static readonly Property<decimal> IotSuspectQtyProperty = P<DispatchTaskQueue>.Register(e => e.IotSuspectQty);

        /// <summary>
        /// IOT可疑品数
        /// </summary>
        public decimal IotSuspectQty
        {
            get { return this.GetProperty(IotSuspectQtyProperty); }
            set { this.SetProperty(IotSuspectQtyProperty, value); }
        }
        #endregion

        #region IOT良品数(当前) IotOkQty
        /// <summary>
        /// IOT良品数(当前)
        /// </summary>
        [Label("IOT良品数(当前)")]
        public static readonly Property<decimal> IotOkQtyProperty = P<DispatchTaskQueue>.RegisterReadOnly(
            e => e.IotOkQty, e => e.GetIotOkQty(), IotQtyProperty, IotSuspectQtyProperty);
        /// <summary>
        /// IOT良品数(当前)
        /// </summary>

        public decimal IotOkQty
        {
            get { return this.GetProperty(IotOkQtyProperty); }
        }
        private decimal GetIotOkQty()
        {
            var iotQty = IotQty * CavityCount + ManualReportQty;
            var qty = iotQty - ActualReportQty - IotSuspectQty;  //刷新当前良品数
            if (qty < 0)
                qty = 0;
            return qty;
        }
        #endregion

        #endregion
    }


    /// <summary>
    /// 实体配置
    /// </summary>
    internal class DispatchTaskQueueEntityConfig : EntityConfig<DispatchTaskQueue>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_DISP_TASK_QUEUE").MapAllProperties();
            Meta.Property(DispatchTaskQueue.IsSelectedProperty).DontMapColumn();
            Meta.Property(DispatchTaskQueue.ParShortDescriptionProperty).DontMapColumn();
            Meta.Property(DispatchTaskQueue.ZcodeProperty).DontMapColumn();
            //Meta.Property(DispatchTaskQueue.IotOkQtyProperty).DontMapColumn();
            Meta.Property(DispatchTaskQueue.IotSuspectQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
