using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 工位叫料ViewModel
    /// </summary>
    [Label("工位叫料")]
    [RootEntity, Serializable]
    public class CallMateriaInStationViewModel : DataEntity
    {
        #region 生产工单 WorkOrder
        /// <summary>
        /// 生产工单Id
        /// </summary>
        [Label("生产工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<CallMateriaInStationViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 生产工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 生产工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<CallMateriaInStationViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 生产工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 配送工位 SendtoStation
        /// <summary>
        /// 配送工位Id
        /// </summary>
        [Label("配送工位")]
        public static readonly IRefIdProperty SendtoStationIdProperty =
            P<CallMateriaInStationViewModel>.RegisterRefId(e => e.SendtoStationId, ReferenceType.Normal);

        /// <summary>
        /// 配送工位Id
        /// </summary>
        public double? SendtoStationId
        {
            get { return (double?)this.GetRefNullableId(SendtoStationIdProperty); }
            set { this.SetRefNullableId(SendtoStationIdProperty, value); }
        }

        /// <summary>
        /// 配送工位
        /// </summary>
        public static readonly RefEntityProperty<Station> SendtoStationProperty =
            P<CallMateriaInStationViewModel>.RegisterRef(e => e.SendtoStation, SendtoStationIdProperty);

        /// <summary>
        /// 配送工位
        /// </summary>
        public Station SendtoStation
        {
            get { return this.GetRefEntity(SendtoStationProperty); }
            set { this.SetRefEntity(SendtoStationProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<CallMateriaInStationViewModel>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<CallMateriaInStationViewModel>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<Priority> PriorityProperty = P<CallMateriaInStationViewModel>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public Priority Priority
        {
            get { return this.GetProperty(PriorityProperty); }
            set { this.SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 叫料原因 ReasonList
        /// <summary>
        /// 叫料原因
        /// </summary>
        [Label("叫料原因")]
        public static readonly ListProperty<EntityList<CallMaterialReason>> ReasonListProperty = P<CallMateriaInStationViewModel>.RegisterList(e => e.ReasonList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as CallMateriaInStationViewModel).LoadReasonList()
        });

        /// <summary>
        /// 叫料原因
        /// </summary>
        /// <returns>上料原因列表</returns>
        public EntityList<CallMaterialReason> ReasonList
        {
            get { return this.GetLazyList(ReasonListProperty); }
        }

        /// <summary>
        /// 加载上料原因
        /// </summary>
        /// <returns>上料原因列表</returns>
        private EntityList<CallMaterialReason> LoadReasonList()
        {
            return new EntityList<CallMaterialReason>();
        }
        #endregion

        #region 工位物料列表 StationItemList
        /// <summary>
        /// 工位物料列表
        /// </summary>
        [Label("工位物料列表")]
        public static readonly ListProperty<EntityList<StationMateriaViewModel>> StationItemListProperty = P<CallMateriaInStationViewModel>.RegisterList(e => e.StationItemList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as CallMateriaInStationViewModel).LoadStationItemList()
        });

        /// <summary>
        /// 工位物料列表
        /// </summary>
        public EntityList<StationMateriaViewModel> StationItemList
        {
            get { return (EntityList<StationMateriaViewModel>)this.GetLazyList(StationItemListProperty, new EagerLoadOptions().LoadWith(StationItemListProperty)); }
        }

        /// <summary>
        /// 获取工位剩余物料信息
        /// </summary>
        /// <returns>工位物料信息</returns>
        private EntityList<StationMateriaViewModel> LoadStationItemList()
        {
            var modelList = new EntityList<StationMateriaViewModel>();
            if (WorkOrderId == null || SendtoStationId == null)
                return modelList;

            modelList.AddRange(RT.Service.Resolve<CallMaterialController>().GetStationMateriaViewModels(WorkOrderId.Value, ProcessId, SendtoStationId.Value, SendtoStation.ResourceId));
            return modelList;
        }
        #endregion

        #region 叫料清单 CallMaterialBillList
        /// <summary>
        /// 叫料清单
        /// </summary>
        [Label("叫料清单")]
        public static readonly ListProperty<EntityList<CallMaterialBillDetail>> CallMaterialBillListProperty = P<CallMateriaInStationViewModel>.RegisterList(e => e.CallMaterialBillList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as CallMateriaInStationViewModel).LoadDetailList()
        });

        /// <summary>
        /// 叫料清单
        /// </summary>
        public EntityList<CallMaterialBillDetail> CallMaterialBillList
        {
            get { return (EntityList<CallMaterialBillDetail>)this.GetLazyList(CallMaterialBillListProperty, new EagerLoadOptions().LoadWith(CallMaterialBillListProperty)); }
        }

        /// <summary>
        /// 获取在途未完成的叫料单明细列表
        /// </summary>
        /// <returns>叫料单明细列表</returns>
        private EntityList<CallMaterialBillDetail> LoadDetailList()
        {
            if (WorkOrderId == null || SendtoStationId == null)
                return new EntityList<CallMaterialBillDetail>();

            return RT.Service.Resolve<CallMaterialController>().GetCallMateriaDetails(WorkOrderId.Value, SendtoStationId.Value);
        }
        #endregion
    }
}
