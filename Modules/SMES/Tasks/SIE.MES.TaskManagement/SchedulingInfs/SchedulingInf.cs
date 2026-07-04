using SIE.Common.Configs;
using SIE.Domain;
using SIE.Items;
using SIE.MES.LineAndon;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.SchedulingInfs.Configs;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs
{
    /// <summary>
    /// MES排程导入中间表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SchedulingInfCriteria))]
    [EntityWithConfig(typeof(SchedulingInfCheckConfig))]
    [Label("MES排程导入中间表")]
    public class SchedulingInf : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<SchedulingInf>.Register(e => e.Factory);

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
            P<SchedulingInf>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<SchedulingInf>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

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
            P<SchedulingInf>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<SchedulingInf>.RegisterRef(e => e.Process, ProcessIdProperty);

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
            P<SchedulingInf>.RegisterRefId(e => e.AndonLineId, ReferenceType.Normal);

        /// <summary>
        /// 线别Id
        /// </summary>
        public double AndonLineId
        {
            get { return (double)this.GetRefId(AndonLineIdProperty); }
            set { this.SetRefId(AndonLineIdProperty, value); }
        }

        /// <summary>
        /// 线别
        /// </summary>
        public static readonly RefEntityProperty<AndonLine> AndonLineProperty =
            P<SchedulingInf>.RegisterRef(e => e.AndonLine, AndonLineIdProperty);

        /// <summary>
        /// 线别
        /// </summary>
        public AndonLine AndonLine
        {
            get { return this.GetRefEntity(AndonLineProperty); }
            set { this.SetRefEntity(AndonLineProperty, value); }
        }
        #endregion

        #region 标准产能 StandardCapacity
        /// <summary>
        /// 标准产能
        /// </summary>
        [Label("标准产能")]
        public static readonly Property<decimal?> StandardCapacityProperty = P<SchedulingInf>.Register(e => e.StandardCapacity);

        /// <summary>
        /// 标准产能
        /// </summary>
        public decimal? StandardCapacity
        {
            get { return this.GetProperty(StandardCapacityProperty); }
            set { this.SetProperty(StandardCapacityProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<SchedulingInf>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<SchedulingInf>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<SchedulingInf>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 入库日期 InStorageDate
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("入库日期")]
        public static readonly Property<DateTime?> InStorageDateProperty = P<SchedulingInf>.Register(e => e.InStorageDate);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? InStorageDate
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
        public static readonly Property<DateTime> BeginDateProperty = P<SchedulingInf>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate
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
        public static readonly Property<DateTime> EndDateProperty = P<SchedulingInf>.Register(e => e.EndDate);

        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime EndDate
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
        public static readonly Property<bool?> IsCheckProperty = P<SchedulingInf>.Register(e => e.IsCheck);

        /// <summary>
        /// 校验是否通过
        /// </summary>
        public bool? IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        #region 校验异常原因 CheckMsg
        /// <summary>
        /// 校验异常原因
        /// </summary>
        [Label("校验异常原因")]
        public static readonly Property<string> CheckMsgProperty = P<SchedulingInf>.Register(e => e.CheckMsg);

        /// <summary>
        /// 校验异常原因
        /// </summary>
        public string CheckMsg
        {
            get { return this.GetProperty(CheckMsgProperty); }
            set { this.SetProperty(CheckMsgProperty, value); }
        }
        #endregion

        #region 是否作废 IsCancel
        /// <summary>
        /// 是否作废
        /// </summary>
        [Label("是否作废")]
        public static readonly Property<bool?> IsCancelProperty = P<SchedulingInf>.Register(e => e.IsCancel);

        /// <summary>
        /// 是否作废
        /// </summary>
        public bool? IsCancel
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
        public static readonly Property<DateTime?> CancelTimeProperty = P<SchedulingInf>.Register(e => e.CancelTime);

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime? CancelTime
        {
            get { return this.GetProperty(CancelTimeProperty); }
            set { this.SetProperty(CancelTimeProperty, value); }
        }
        #endregion

        #region 作废人 Cancer
        /// <summary>
        /// 作废人Id
        /// </summary>
        [Label("作废人")]
        public static readonly IRefIdProperty CancerIdProperty =
            P<SchedulingInf>.RegisterRefId(e => e.CancerId, ReferenceType.Normal);

        /// <summary>
        /// 作废人Id
        /// </summary>
        public double? CancerId
        {
            get { return (double?)this.GetRefNullableId(CancerIdProperty); }
            set { this.SetRefNullableId(CancerIdProperty, value); }
        }

        /// <summary>
        /// 作废人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CancerProperty =
            P<SchedulingInf>.RegisterRef(e => e.Cancer, CancerIdProperty);

        /// <summary>
        /// 作废人
        /// </summary>
        public Employee Cancer
        {
            get { return this.GetRefEntity(CancerProperty); }
            set { this.SetRefEntity(CancerProperty, value); }
        }
        #endregion

        #region 作废原因 CancelReason
        /// <summary>
        /// 作废原因
        /// </summary>
        [Label("作废原因")]
        public static readonly Property<string> CancelReasonProperty = P<SchedulingInf>.Register(e => e.CancelReason);

        /// <summary>
        /// 作废原因
        /// </summary>
        public string CancelReason
        {
            get { return this.GetProperty(CancelReasonProperty); }
            set { this.SetProperty(CancelReasonProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<SchedulingInf>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region MRP控制者 Mrb
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrbProperty = P<SchedulingInf>.RegisterView(e => e.Mrb, p => p.WorkOrder.WorkShop.Code);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string Mrb
        {
            get { return this.GetProperty(MrbProperty); }
        }
        #endregion

        #region 工作中心 WorkCenterCode
        /// <summary>
        /// 工作中心
        /// </summary>
        [Label("工作中心")]
        public static readonly Property<string> WorkCenterCodeProperty = P<SchedulingInf>.RegisterView(e => e.WorkCenterCode, p => p.AndonLine.WorkCenter.Code);

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterCode
        {
            get { return this.GetProperty(WorkCenterCodeProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SchedulingInf>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<SchedulingInf>.RegisterView(e => e.Unit, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<SchedulingInf>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

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
        public static readonly Property<string> ProcessNameProperty = P<SchedulingInf>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 资源 EquipmentNo
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> EquipmentNoProperty = P<SchedulingInf>.RegisterView(e => e.EquipmentNo, p => p.AndonLine.Equipment.Code);

        /// <summary>
        /// 资源
        /// </summary>
        public string EquipmentNo
        {
            get { return this.GetProperty(EquipmentNoProperty); }
        }
        #endregion

        #region 产线/机台编码 MachineCode
        /// <summary>
        /// 产线/机台编码
        /// </summary>
        [Label("产线/机台编码")]
        public static readonly Property<string> MachineCodeProperty = P<SchedulingInf>.RegisterView(e => e.MachineCode, p => p.AndonLine.MachineCode);

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public string MachineCode
        {
            get { return this.GetProperty(MachineCodeProperty); }
        }
        #endregion

        #region 工单计划数量 WoPlanQty
        /// <summary>
        /// 工单计划数量
        /// </summary>
        [Label("工单计划数量")]
        public static readonly Property<decimal> WoPlanQtyProperty = P<SchedulingInf>.RegisterView(e => e.WoPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单计划数量
        /// </summary>
        public decimal WoPlanQty
        {
            get { return this.GetProperty(WoPlanQtyProperty); }
        }
        #endregion

        #region 下单日期 WorkOrderUpdate
        /// <summary>
        /// 下单日期
        /// </summary>
        [Label("下单日期")]
        public static readonly Property<DateTime> WorkOrderUpdateProperty = P<SchedulingInf>.RegisterView(e => e.WorkOrderUpdate, p => p.WorkOrder.UpdateDate);

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime WorkOrderUpdate
        {
            get { return this.GetProperty(WorkOrderUpdateProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库



        #endregion

        #region 是否排程退回 IsSchedulingInfReturn
        /// <summary>
        /// 是否排程退回
        /// </summary>
        [Label("是否排程退回")]
        public static readonly Property<YesNo?> IsSchedulingInfReturnProperty = P<SchedulingInf>.Register(e => e.IsSchedulingInfReturn);

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
        public static readonly Property<string> SchedulingInfReturnReasonProperty = P<SchedulingInf>.Register(e => e.SchedulingInfReturnReason);

        /// <summary>
        /// 排程退回原因
        /// </summary>
        public string SchedulingInfReturnReason
        {
            get { return this.GetProperty(SchedulingInfReturnReasonProperty); }
            set { this.SetProperty(SchedulingInfReturnReasonProperty, value); }
        }
        #endregion
    }

    internal class SchedulingInfConfig : EntityConfig<SchedulingInf>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("SCHEDULING_INF").MapAllProperties();
            Meta.Property(SchedulingInf.CheckMsgProperty).MapColumn().HasLength("2000");
            Meta.Property(SchedulingInf.SchedulingInfReturnReasonProperty).MapColumn().HasLength("4000");
            Meta.Property(SchedulingInf.CancelReasonProperty).MapColumn().HasLength("4000");
            Meta.EnablePhantoms();
        }
    }

}
