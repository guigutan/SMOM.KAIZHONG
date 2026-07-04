using SIE.Domain;
using SIE.Items;
using SIE.MES.LineAndon;
using SIE.MES.TaskManagement.SchedulingInfs.Reports;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs
{
    /// <summary>
    /// MES排程中间表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("MES排程中间表查询实体")]
    public class SchedulingInfCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<SchedulingInfCriteria>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<SchedulingInfCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<SchedulingInfCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<SchedulingInfCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<SchedulingInfCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 线别 AndonLine
        /// <summary>
        /// 线别Id
        /// </summary>
        [Label("线别")]
        public static readonly IRefIdProperty AndonLineIdProperty =
            P<SchedulingInfCriteria>.RegisterRefId(e => e.AndonLineId, ReferenceType.Normal);

        /// <summary>
        /// 线别Id
        /// </summary>
        public double? AndonLineId
        {
            get { return (double?)this.GetRefNullableId(AndonLineIdProperty); }
            set { this.SetRefNullableId(AndonLineIdProperty, value); }
        }

        /// <summary>
        /// 线别
        /// </summary>
        public static readonly RefEntityProperty<AndonLine> AndonLineProperty =
            P<SchedulingInfCriteria>.RegisterRef(e => e.AndonLine, AndonLineIdProperty);

        /// <summary>
        /// 线别
        /// </summary>
        public AndonLine AndonLine
        {
            get { return this.GetRefEntity(AndonLineProperty); }
            set { this.SetRefEntity(AndonLineProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<SchedulingInfCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<SchedulingInfCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 下单日期 WorkOrderUpdate
        /// <summary>
        /// 下单日期
        /// </summary>
        [Label("下单日期")]
        public static readonly Property<DateRange> WorkOrderUpdateProperty = P<SchedulingInfCriteria>.Register(e => e.WorkOrderUpdate);

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateRange WorkOrderUpdate
        {
            get { return this.GetProperty(WorkOrderUpdateProperty); }
            set { this.SetProperty(WorkOrderUpdateProperty, value); }
        }
        #endregion

        #region 入库日期 InStorageDate
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("入库日期")]
        public static readonly Property<DateRange> InStorageDateProperty = P<SchedulingInfCriteria>.Register(e => e.InStorageDate);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateRange InStorageDate
        {
            get { return this.GetProperty(InStorageDateProperty); }
            set { this.SetProperty(InStorageDateProperty, value); }
        }
        #endregion

        #region 开始日期 BeginDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateRange> BeginDateProperty = P<SchedulingInfCriteria>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateRange BeginDate
        {
            get { return this.GetProperty(BeginDateProperty); }
            set { this.SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 完成日期 EndDate
        /// <summary>
        /// 完成日期
        /// </summary>
        [Label("完成日期")]
        public static readonly Property<DateRange> EndDateProperty = P<SchedulingInfCriteria>.Register(e => e.EndDate);

        /// <summary>
        /// 完成日期
        /// </summary>
        public DateRange EndDate
        {
            get { return this.GetProperty(EndDateProperty); }
            set { this.SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 校验是否通过 IsCheck
        /// <summary>
        /// 校验是否通过
        /// </summary>
        [Label("校验是否通过")]
        public static readonly Property<YesNo?> IsCheckProperty = P<SchedulingInfCriteria>.Register(e => e.IsCheck);

        /// <summary>
        /// 校验是否通过
        /// </summary>
        public YesNo? IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        #region 是否排程退回 IsSchedulingInfReturn
        /// <summary>
        /// 是否排程退回
        /// </summary>
        [Label("是否排程退回")]
        public static readonly Property<YesNo?> IsSchedulingInfReturnProperty = P<SchedulingInfCriteria>.Register(e => e.IsSchedulingInfReturn);

        /// <summary>
        /// 是否排程退回
        /// </summary>
        public YesNo? IsSchedulingInfReturn
        {
            get { return this.GetProperty(IsSchedulingInfReturnProperty); }
            set { this.SetProperty(IsSchedulingInfReturnProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<SchedulingInfCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 是否作废 IsCancel
        /// <summary>
        /// 是否作废
        /// </summary>
        [Label("是否作废")]
        public static readonly Property<YesNo?> IsCancelProperty = P<SchedulingInfCriteria>.Register(e => e.IsCancel);

        /// <summary>
        /// 是否作废
        /// </summary>
        public YesNo? IsCancel
        {
            get { return this.GetProperty(IsCancelProperty); }
            set { this.SetProperty(IsCancelProperty, value); }
        }
        #endregion

        #region 作废时间 CancelTime
        /// <summary>
        /// 作废时间
        /// </summary>
        [Label("作废时间")]
        public static readonly Property<DateRange> CancelTimeProperty = P<SchedulingInfCriteria>.Register(e => e.CancelTime);

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateRange CancelTime
        {
            get { return this.GetProperty(CancelTimeProperty); }
            set { this.SetProperty(CancelTimeProperty, value); }
        }
        #endregion

        #region 是否已下发 IsGenerateTask
        /// <summary>
        /// 是否已下发
        /// </summary>
        [Label("是否已下发")]
        public static readonly Property<YesNo?> IsGenerateTaskProperty = P<SchedulingInfCriteria>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否已下发
        /// </summary>
        public YesNo? IsGenerateTask
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
        public static readonly Property<string> MrbProperty = P<SchedulingInfCriteria>.Register(e => e.Mrb);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string Mrb
        {
            get { return this.GetProperty(MrbProperty); }
            set { this.SetProperty(MrbProperty, value); }
        }
        #endregion



        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<SchedulingInfController>().CriteriaSchedulingInf(this);
        }
    }
}
