using SIE.Common.Configs;
using SIE.Domain;
using SIE.Items;
using SIE.Kit.MES.CallMaterials.Configs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 工单匹配
    /// </summary>
    [ChildEntity, Serializable]
    [EntityWithConfig(typeof(CallMaterialMatchConfig))]
    [Label("工单匹配")]
    public class CallMatchWorkOrder : ViewModel
    {
        #region 替代料编码 AlternativeCode
        /// <summary>
        /// 替代料编码
        /// </summary>
        [Label("替代料编码")]
        public static readonly Property<string> AlternativeCodeProperty = P<CallMatchWorkOrder>.Register(e => e.AlternativeCode);

        /// <summary>
        /// 替代料编码
        /// </summary>
        public string AlternativeCode
        {
            get { return GetProperty(AlternativeCodeProperty); }
            set { SetProperty(AlternativeCodeProperty, value); }
        }
        #endregion

        #region 替代料名称 AlternativeName
        /// <summary>
        /// 替代料名称
        /// </summary>
        [Label("替代料名称")]
        public static readonly Property<string> AlternativeNameProperty = P<CallMatchWorkOrder>.Register(e => e.AlternativeName);

        /// <summary>
        /// 替代料名称
        /// </summary>
        public string AlternativeName
        {
            get { return GetProperty(AlternativeNameProperty); }
            set { SetProperty(AlternativeNameProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<CallMatchWorkOrder>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty = P<CallMatchWorkOrder>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<CallMatchWorkOrder>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        [Label("工单")]
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<CallMatchWorkOrder>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<CallMatchWorkOrder>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<CallMatchWorkOrder>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 叫料工单 CallWorkOrder
        /// <summary>
        /// 叫料工单Id
        /// </summary>
        [Label("叫料工单")]
        public static readonly IRefIdProperty CallWorkOrderIdProperty = P<CallMatchWorkOrder>.RegisterRefId(e => e.CallWorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 叫料工单Id
        /// </summary>
        public double CallWorkOrderId
        {
            get { return (double)GetRefId(CallWorkOrderIdProperty); }
            set { SetRefId(CallWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 叫料工单
        /// </summary>
        [Label("叫料工单")]
        public static readonly RefEntityProperty<CallMaterialWorkOrder> CallWorkOrderProperty = P<CallMatchWorkOrder>.RegisterRef(e => e.CallWorkOrder, CallWorkOrderIdProperty);

        /// <summary>
        /// 叫料工单
        /// </summary>       
        public CallMaterialWorkOrder CallWorkOrder
        {
            get { return GetRefEntity(CallWorkOrderProperty); }
            set { SetRefEntity(CallWorkOrderProperty, value); }
        }
        #endregion

        #region 是否继续使用 IsUse
        /// <summary>
        /// 是否继续使用
        /// </summary>
        [Label("是否继续使用")]
        public static readonly Property<bool> IsUseProperty = P<CallMatchWorkOrder>.Register(e => e.IsUse);

        /// <summary>
        /// 是否继续使用
        /// </summary>
        public bool IsUse
        {
            get { return GetProperty(IsUseProperty); }
            set { SetProperty(IsUseProperty, value); }
        }
        #endregion

        #region 物料占比 MatchRate
        /// <summary>
        /// 物料占比
        /// </summary>
        [Label("物料占比")]
        public static readonly Property<double> MatchRateProperty = P<CallMatchWorkOrder>.Register(e => e.MatchRate);

        /// <summary>
        /// 物料占比
        /// </summary>
        public double MatchRate
        {
            get { return GetProperty(MatchRateProperty); }
            set { SetProperty(MatchRateProperty, value); }
        }
        #endregion

        #region 是否继续使用 IsChange
        /// <summary>
        /// 监控继续使用的栏位是否有改变
        /// </summary>
        [Label("监控继续使用的栏位是否有改变")]
        public static readonly Property<bool> IsChangeProperty = P<CallMatchWorkOrder>.Register(e => e.IsChange);

        /// <summary>
        /// 监控继续使用的栏位是否有改变
        /// </summary>
        public bool IsChange
        {
            get { return GetProperty(IsChangeProperty); }
            set { SetProperty(IsChangeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<CallMatchWorkOrder>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<CallMatchWorkOrder>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料单位 ItemUnit
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("物料单位")]
        public static readonly Property<string> ItemUnitProperty = P<CallMatchWorkOrder>.Register(e => e.ItemUnit);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit
        {
            get { return GetProperty(ItemUnitProperty); }
            set { SetProperty(ItemUnitProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<CallMatchWorkOrder>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == IsUseProperty.Name)
            {
                if (RT.Service.Resolve<CallMaterialController>().GetCallMatchItem(CallWorkOrderId, WorkOrderId, ItemId) == null)
                    IsChange = true;
            }
        }
    }
}