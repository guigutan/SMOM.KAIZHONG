using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs.Reports
{
    /// <summary>
    /// 排程导入详情
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SchedulingInfReportCriteria))]
    [Label("排程导入详情")]
    public class SchedulingInfReport : Entity<string>
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<SchedulingInfReport>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<SchedulingInfReport>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<SchedulingInfReport>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<SchedulingInfReport>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState?> StateProperty = P<SchedulingInfReport>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<SchedulingInfReport>.Register(e => e.PlanBeginDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
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
        public static readonly Property<DateTime> PlanEndDateProperty = P<SchedulingInfReport>.Register(e => e.PlanEndDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<SchedulingInfReport>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<SchedulingInfReport>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 是否已导入 IsImport
        /// <summary>
        /// 是否已导入
        /// </summary>
        [Label("是否已导入")]
        public static readonly Property<YesNo> IsImportProperty = P<SchedulingInfReport>.Register(e => e.IsImport);

        /// <summary>
        /// 是否已导入
        /// </summary>
        public YesNo IsImport
        {
            get { return this.GetProperty(IsImportProperty); }
            set { this.SetProperty(IsImportProperty, value); }
        }
        #endregion

        #region 工序任务单生成数量 ImportQty
        /// <summary>
        /// 工序任务单生成数量
        /// </summary>
        [Label("工序任务单生成数量")]
        public static readonly Property<decimal?> ImportQtyProperty = P<SchedulingInfReport>.Register(e => e.ImportQty);

        /// <summary>
        /// 工序任务单生成数量
        /// </summary>
        public decimal? ImportQty
        {
            get { return this.GetProperty(ImportQtyProperty); }
            set { this.SetProperty(ImportQtyProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopProperty = P<SchedulingInfReport>.Register(e => e.WorkShop);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
            set { this.SetProperty(WorkShopProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<SchedulingInfReport>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 工单类型 Type
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> TypeProperty = P<SchedulingInfReport>.Register(e => e.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<SchedulingInfReport>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<SchedulingInfReport>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<SchedulingInfReport>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 工序数量 ProcessQty
        /// <summary>
        /// 工序数量
        /// </summary>
        [Label("工序数量")]
        public static readonly Property<decimal> ProcessQtyProperty = P<SchedulingInfReport>.Register(e => e.ProcessQty);

        /// <summary>
        /// 工序数量
        /// </summary>
        public decimal ProcessQty
        {
            get { return this.GetProperty(ProcessQtyProperty); }
            set { this.SetProperty(ProcessQtyProperty, value); }
        }
        #endregion

        #region 已报工数量 ReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<SchedulingInfReport>.Register(e => e.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return GetProperty(ReportQtyProperty); }
            set { SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 工序排程数量 SchedulingQty
        /// <summary>
        /// 工序排程数量
        /// </summary>
        [Label("工序排程数量")]
        public static readonly Property<decimal?> SchedulingQtyProperty = P<SchedulingInfReport>.Register(e => e.SchedulingQty);

        /// <summary>
        /// 工序排程数量
        /// </summary>
        public decimal? SchedulingQty
        {
            get { return this.GetProperty(SchedulingQtyProperty); }
            set { this.SetProperty(SchedulingQtyProperty, value); }
        }
        #endregion

        #region 下单时间 UpdateDate
        /// <summary>
        /// 下单时间
        /// </summary>
        [Label("下单时间")]
        public static readonly Property<DateTime?> UpdateDateProperty = P<SchedulingInfReport>.Register(e => e.UpdateDate);

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { this.SetProperty(UpdateDateProperty, value); }
        }
        #endregion

        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int> InvOrgIdProperty = P<SchedulingInfReport>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        #region 是否排程退回 IsSchedulingInfReturn
        /// <summary>
        /// 是否排程退回
        /// </summary>
        [Label("是否排程退回")]
        public static readonly Property<YesNo?> IsSchedulingInfReturnProperty = P<SchedulingInfReport>.Register(e => e.IsSchedulingInfReturn);

        /// <summary>
        /// 是否排程退回
        /// </summary>
        public YesNo? IsSchedulingInfReturn
        {
            get { return this.GetProperty(IsSchedulingInfReturnProperty); }
            set { this.SetProperty(IsSchedulingInfReturnProperty, value); }
        }
        #endregion

        #region 排程退回原因 SchedulingInfReturnReason
        /// <summary>
        /// 排程退回原因
        /// </summary>
        [Label("排程退回原因")]
        public static readonly Property<string> SchedulingInfReturnReasonProperty = P<SchedulingInfReport>.Register(e => e.SchedulingInfReturnReason);

        /// <summary>
        /// 排程退回原因
        /// </summary>
        public string SchedulingInfReturnReason
        {
            get { return this.GetProperty(SchedulingInfReturnReasonProperty); }
            set { this.SetProperty(SchedulingInfReturnReasonProperty, value); }
        }
        #endregion

        #region 产线编码 MachineCode
        /// <summary>
        /// 产线编码
        /// </summary>
        [Label("产线编码")]
        public static readonly Property<string> MachineCodeProperty = P<SchedulingInfReport>.Register(e => e.MachineCode);

        /// <summary>
        /// 产线编码
        /// </summary>
        public string MachineCode
        {
            get { return this.GetProperty(MachineCodeProperty); }
            set { this.SetProperty(MachineCodeProperty, value); }
        }
        #endregion

        #region 标准产能 StandardCapacity
        /// <summary>
        /// 标准产能
        /// </summary>
        [Label("标准产能")]
        public static readonly Property<decimal?> StandardCapacityProperty = P<SchedulingInfReport>.Register(e => e.StandardCapacity);

        /// <summary>
        /// 标准产能
        /// </summary>
        public decimal? StandardCapacity
        {
            get { return this.GetProperty(StandardCapacityProperty); }
            set { this.SetProperty(StandardCapacityProperty, value); }
        }
        #endregion

        #region 排程导入时间 ImportTime
        /// <summary>
        /// 排程导入时间
        /// </summary>
        [Label("排程导入时间")]
        public static readonly Property<DateTime?> ImportTimeProperty = P<SchedulingInfReport>.Register(e => e.ImportTime);

        /// <summary>
        /// 排程导入时间
        /// </summary>
        public DateTime? ImportTime
        {
            get { return this.GetProperty(ImportTimeProperty); }
            set { this.SetProperty(ImportTimeProperty, value); }
        }
        #endregion

        #region 校验是否通过 IsCheck
        /// <summary>
        /// 校验是否通过
        /// </summary>
        [Label("校验是否通过")]
        public static readonly Property<bool?> IsCheckProperty = P<SchedulingInfReport>.Register(e => e.IsCheck);

        /// <summary>
        /// 校验是否通过
        /// </summary>
        public bool? IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        #region 是否已下发 IsGenerateTask
        /// <summary>
        /// 是否已下发
        /// </summary>
        [Label("是否已下发")]
        public static readonly Property<YesNo> IsGenerateTaskProperty = P<SchedulingInfReport>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否已下发
        /// </summary>
        public YesNo IsGenerateTask
        {
            get { return this.GetProperty(IsGenerateTaskProperty); }
            set { this.SetProperty(IsGenerateTaskProperty, value); }
        }
        #endregion

        #region MRP控制者 Mrb
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrbProperty = P<SchedulingInfReport>.Register(e => e.Mrb);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string Mrb
        {
            get { return this.GetProperty(MrbProperty); }
            set { this.SetProperty(MrbProperty, value); }
        }
        #endregion

        #region 不映射数据库

        #region 工序待排程数量 WaitSchedulingQty
        /// <summary>
        /// 工序待排程数量
        /// </summary>
        [Label("工序待排程数量")]
        public static readonly Property<decimal> WaitSchedulingQtyProperty = P<SchedulingInfReport>.Register(e => e.WaitSchedulingQty);

        /// <summary>
        /// 工序待排程数量
        /// </summary>
        public decimal WaitSchedulingQty
        {
            get { return this.GetProperty(WaitSchedulingQtyProperty); }
            set { this.SetProperty(WaitSchedulingQtyProperty, value); }
        }
        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<DispatchTaskStatus?> TaskStatusProperty = P<SchedulingInfReport>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus? TaskStatus
        {
            get { return GetProperty(TaskStatusProperty); }
            set { SetProperty(TaskStatusProperty, value); }
        }
        #endregion

        #endregion


    }

    internal class SchedulingInfReportConfig : EntityConfig<SchedulingInfReport>
    {
        protected override void ConfigMeta()
        {
            Meta.MapView("V_SCHEDULING_INF_REPORT").MapAllProperties();
            Meta.Property(SchedulingInfReport.WaitSchedulingQtyProperty).DontMapColumn();
            Meta.Property(SchedulingInfReport.TaskStatusProperty).DontMapColumn();
        }
    }
}
