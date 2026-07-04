using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(CallMaterialWoCriteria))]
    [Label("叫料工单")]
    public partial class CallMaterialWorkOrder : DataEntity
    {
        #region 叫料顺序 Index
        /// <summary>
        /// 叫料顺序
        /// </summary>
        [Label("叫料顺序")]
        public static readonly Property<int> IndexProperty = P<CallMaterialWorkOrder>.Register(e => e.Index);

        /// <summary>
        /// 叫料顺序
        /// </summary>
        public int Index
        {
            get { return GetProperty(IndexProperty); }
            set { SetProperty(IndexProperty, value); }
        }
        #endregion

        #region 是否已叫料 IsCalled
        /// <summary>
        /// 是否已叫料
        /// </summary>
        [Label("是否已叫料")]
        public static readonly Property<bool> IsCalledProperty = P<CallMaterialWorkOrder>.Register(e => e.IsCalled);

        /// <summary>
        /// 是否已叫料
        /// </summary>
        public bool IsCalled
        {
            get { return GetProperty(IsCalledProperty); }
            set { SetProperty(IsCalledProperty, value); }
        }
        #endregion 

        #region 工单匹配列表 MatchList
        /// <summary>
        /// 工单匹配列表
        /// </summary>
        [Label("工单匹配")]
        public static readonly ListProperty<EntityList<CallMatchWorkOrder>> MatchListProperty = P<CallMaterialWorkOrder>.RegisterList(e => e.MatchList);

        /// <summary>
        /// 工单匹配列表
        /// </summary>
        public EntityList<CallMatchWorkOrder> MatchList
        {
            get { return this.GetLazyList(MatchListProperty); }
        }
        #endregion

        #region 叫料单列表 BillList
        /// <summary>
        /// 叫料单列表
        /// </summary>
        [Label("叫料单")]
        public static readonly ListProperty<EntityList<CallMaterialBill>> BillListProperty = P<CallMaterialWorkOrder>.RegisterList(e => e.BillList);

        /// <summary>
        /// 叫料单列表
        /// </summary>
        public EntityList<CallMaterialBill> BillList
        {
            get { return this.GetLazyList(BillListProperty); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<CallMaterialWorkOrder>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<CallMaterialWorkOrder>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 发送WMS失败 IsSendedFail
        /// <summary>
        /// 发送WMS失败
        /// </summary>
        [Label("发送WMS失败")]
        public static readonly Property<bool> IsSendedFailProperty = P<CallMaterialWorkOrder>.Register(e => e.IsSendedFail);

        /// <summary>
        /// 发送WMS失败
        /// </summary>
        public bool IsSendedFail
        {
            get { return this.GetProperty(IsSendedFailProperty); }
            set { this.SetProperty(IsSendedFailProperty, value); }
        }
        #endregion

        #region 失败原因 FailReason
        /// <summary>
        /// 失败原因
        /// </summary>
        [Label("失败原因")]
        public static readonly Property<string> FailReasonProperty = P<CallMaterialWorkOrder>.Register(e => e.FailReason);

        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailReason
        {
            get { return this.GetProperty(FailReasonProperty); }
            set { this.SetProperty(FailReasonProperty, value); }
        }
        #endregion

        #region 子列表总数 ChildNum
        /// <summary>
        /// 子列表总数
        /// </summary>
        [Label("子列表总数")]
        public static readonly Property<int> ChildNumProperty = P<CallMaterialWorkOrder>.Register(e => e.ChildNum);

        /// <summary>
        /// 子列表总数
        /// </summary>
        public int ChildNum
        {
            get { return GetProperty(ChildNumProperty); }
            set { SetProperty(ChildNumProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 工单状态 WoState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderStates> WoStateProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoState, p => p.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderStates WoState
        {
            get { return this.GetProperty(WoStateProperty); }
        }
        #endregion

        #region 是否暂停 WoIsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("属性名")]
        public static readonly Property<YesNo> WoIsPauseProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoIsPause, p => p.WorkOrder.IsPause);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public YesNo WoIsPause
        {
            get { return this.GetProperty(WoIsPauseProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 计划数 WoPlanQty
        /// <summary>
        /// 计划数
        /// </summary>
        [Label("计划数")]
        public static readonly Property<decimal> WoPlanQtyProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数
        /// </summary>
        public decimal WoPlanQty
        {
            get { return this.GetProperty(WoPlanQtyProperty); }
        }
        #endregion

        #region 完工数 WoFinishQty
        /// <summary>
        /// 完工数
        /// </summary>
        [Label("完工数")]
        public static readonly Property<decimal> WoFinishQtyProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoFinishQty, p => p.WorkOrder.FinishQty);

        /// <summary>
        /// 完工数
        /// </summary>
        public decimal WoFinishQty
        {
            get { return this.GetProperty(WoFinishQtyProperty); }
        }
        #endregion

        #region 车间 WoWorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WoWorkShopNameProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoWorkShopName, p => p.WorkOrder.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WoWorkShopName
        {
            get { return this.GetProperty(WoWorkShopNameProperty); }
        }
        #endregion

        #region 资源 WoResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> WoResourceNameProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoResourceName, p => p.WorkOrder.Resource.Name);

        /// <summary>
        /// 资源
        /// </summary>
        public string WoResourceName
        {
            get { return this.GetProperty(WoResourceNameProperty); }
        }
        #endregion

        #region 计划开始时间 WoPlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> WoPlanBeginDateProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoPlanBeginDate, p => p.WorkOrder.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime WoPlanBeginDate
        {
            get { return this.GetProperty(WoPlanBeginDateProperty); }
        }
        #endregion

        #region 计划结束时间 WoPlanEndDate
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> WoPlanEndDateProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoPlanEndDate, p => p.WorkOrder.PlanEndDate);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime WoPlanEndDate
        {
            get { return this.GetProperty(WoPlanEndDateProperty); }
        }
        #endregion

        #region 实际开始时间 WoActuStartDate
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime> WoActuStartDateProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoActuStartDate, p => p.WorkOrder.ActuStartDate);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime WoActuStartDate
        {
            get { return this.GetProperty(WoActuStartDateProperty); }
        }
        #endregion

        #region 实际结束时间 WoActuFinishDate
        /// <summary>
        /// 实际结束时间
        /// </summary>
        [Label("实际结束时间")]
        public static readonly Property<DateTime> WoActuFinishDateProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoActuFinishDate, p => p.WorkOrder.ActuFinishDate);

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime WoActuFinishDate
        {
            get { return this.GetProperty(WoActuFinishDateProperty); }
        }
        #endregion

        #region 制单人 WoMakerName
        /// <summary>
        /// 制单人
        /// </summary>
        [Label("制单人")]
        public static readonly Property<string> WoMakerNameProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoMakerName, p => p.WorkOrder.Maker.Name);

        /// <summary>
        /// 制单人
        /// </summary>
        public string WoMakerName
        {
            get { return this.GetProperty(WoMakerNameProperty); }
        }
        #endregion

        #region 制单时间 WoMakeDate
        /// <summary>
        /// 制单时间
        /// </summary>
        [Label("制单时间")]
        public static readonly Property<DateTime> WoMakeDateProperty = P<CallMaterialWorkOrder>.RegisterView(e => e.WoMakeDate, p => p.WorkOrder.MakeDate);

        /// <summary>
        /// 制单时间
        /// </summary>
        public DateTime WoMakeDate
        {
            get { return this.GetProperty(WoMakeDateProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 叫料工单 实体配置
    /// </summary>
    internal class CallMaterialWorkOrderConfig : EntityConfig<CallMaterialWorkOrder>
    {
        /// <summary>
        /// 配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_WO").MapAllPropertiesExcept(CallMaterialWorkOrder.ChildNumProperty);
            Meta.Property(CallMaterialWorkOrder.FailReasonProperty).ColumnMeta.HasLength(500);
            Meta.EnablePhantoms();
        }
    }
}