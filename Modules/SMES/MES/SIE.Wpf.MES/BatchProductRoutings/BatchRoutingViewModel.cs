using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.Routings.Technologys;
using SIE.Wpf.MES.ProductRoutings;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchProductRoutings
{
    /// <summary>
    /// 批次产品工艺路线视图模型
    /// </summary>
    [RootEntity]
    public class BatchRoutingViewModel : ViewModel
    {
        #region 批次 Batch
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<WipBatch> BatchProperty = P<BatchRoutingViewModel>.Register(e => e.Batch);

        /// <summary>
        /// 批次
        /// </summary>
        public WipBatch Batch
        {
            get { return this.GetProperty(BatchProperty); }
            set { this.SetProperty(BatchProperty, value); }
        }
        #endregion 

        #region 工单 WorkOrder
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<WorkOrder> WorkOrderProperty = P<BatchRoutingViewModel>.Register(e => e.WorkOrder);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

        #region 生产版本 Version
        /// <summary>
        /// 生产版本
        /// </summary>
        [Label("生产版本")]
        public static readonly Property<BatchWipProductVersion> VersionProperty = P<BatchRoutingViewModel>.Register(e => e.Version);

        /// <summary>
        /// 生产版本
        /// </summary>
        public BatchWipProductVersion Version
        {
            get { return this.GetProperty(VersionProperty); }
            set { this.SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 批次关系 BatchRelation
        /// <summary>
        /// 批次关系
        /// </summary>
        [Label("批次关系")]
        public static readonly Property<BatchRelation> BatchRelationProperty = P<BatchRoutingViewModel>.Register(e => e.BatchRelation);

        /// <summary>
        /// 批次关系
        /// </summary>
        public BatchRelation BatchRelation
        {
            get { return this.GetProperty(BatchRelationProperty); }
            set { this.SetProperty(BatchRelationProperty, value); }
        }
        #endregion

        #region 选中项 SelectedItem
        /// <summary>
        /// 选中项
        /// </summary>
        public static readonly Property<IActivity> SelectedItemProperty = P<BatchRoutingViewModel>.Register(e => e.SelectedItem);

        /// <summary>
        /// 选中项
        /// </summary>
        public IActivity SelectedItem
        {
            get { return this.GetProperty(SelectedItemProperty); }
            set { this.SetProperty(SelectedItemProperty, value); }
        }
        #endregion

        #region 运行时产品 product
        /// <summary>
        /// 运行时产品
        /// </summary>
        public product _product;
        #endregion

        #region 列表属性
        /// <summary>
        /// 产品生产关键件
        /// </summary>
        public EntityList<BatchWipProductProcessKeyItem> KeyItemList = new EntityList<BatchWipProductProcessKeyItem>();

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public EntityList<BatchWipProductDefect> DefectList = new EntityList<BatchWipProductDefect>();

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public EntityList<BatchWipProductRepaire> RepaireList = new EntityList<BatchWipProductRepaire>();

        /// <summary>
        /// 产品生产BOM数据
        /// </summary>
        public EntityList<BomViewModel> BomList = new EntityList<BomViewModel>();

        /// <summary>
        /// 产品工艺路线事件列表
        /// </summary>
        public EntityList<BatchWipProductRoutingEvent> RoutingEventList = new EntityList<BatchWipProductRoutingEvent>();
        #endregion 

        /// <summary>
        /// 根据条码初始化工艺流程、工序信息
        /// </summary>
        /// <param name="batch">批次</param>
        public void InitInfo(WipBatch batch)
        {
            Batch = batch;
            WorkOrder = RF.GetById<WorkOrder>(Batch?.WorkOrderId);
            Version = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductVersion(GetWipBatchNo());
            BatchRelation = RT.Service.Resolve<BatchManageController>().GetBatchRelation(batch?.BatchNo, BarcodeType.BatchBarocde);
            SelectedItem = null;
            RefreshEvent();
            ClearList();
        }

        /// <summary>
        /// 选中节点变更
        /// </summary>
        public void SelectedNodeChanged()
        {
            RefreshProcessInfo();
        }

        /// <summary>
        /// 刷新工序信息
        /// </summary>
        void RefreshProcessInfo()
        {
            ClearList();
            if (Version != null && SelectedItem != null && SelectedItem.Type == ActivityType.Interaction)
            {
                var process = Version.ProcessList.FirstOrDefault(p => p.ProcessId == SelectedItem.ProcessId);
                DefectList.AddRange(Version.DefectList.Where(p => p.ProcessId == SelectedItem.ProcessId));
                RepaireList.AddRange(Version.RepaireList.Where(p => p.ProcessId == SelectedItem.ProcessId));
                if (process != null)
                {
                    var keyItems = process.DetailList.SelectMany(p => p.KeyItemList);
                    if (keyItems.Count() > 0)
                        KeyItemList.AddRange(keyItems);
                }
            }
            RefreshBomInfo();
        }

        /// <summary>
        /// 刷新bom信息
        /// </summary>
        void RefreshBomInfo()
        {
            if (SelectedItem == null)
                return;
            if (Version == null)
                //未上线，取工单的
                SelectedItem.ProcessBoms.ForEach(p => BomList.Add(new BomViewModel() { ItemId = p.ItemId, Qty = p.Qty, IsBuckleMaterial = p.IsBuckleMaterial }));
            else
            {
                if (Version.IsFinish)
                {
                    //完工，取关键件
                    var process = Version.ProcessList.FirstOrDefault(p => p.ProcessId == SelectedItem.ProcessId);
                    process?.DetailList?.SelectMany(p => p.KeyItemList).ForEach(p => BomList.Add(new BomViewModel() { ItemId = p.ItemId, Qty = p.Qty, IsBuckleMaterial = true }));
                    return;
                }
                //在制取运行时产品的bom
                var rtProcess = _product?.Routing?.Processes?.FirstOrDefault(p => p.ProcessId == SelectedItem.ProcessId);
                rtProcess?.Boms?.ForEach(p => BomList.Add(new BomViewModel() { ItemId = p.ItemId, Qty = p.Qty, IsBuckleMaterial = true }));
            }
        }

        /// <summary>
        /// 根据选中的条码更新工序BOM页签信息
        /// </summary>
        public void UpdateProcessBomToModel()
        {
            if (SelectedItem != null)
            {
                SelectedItem.ProcessBoms.Clear();
                BomList.Where(p => p.Item != null).ForEach(p => SelectedItem.ProcessBoms.Add(new ProcessBom() { ItemId = p.ItemId.ConvertTo<double>(0), Qty = p.Qty, IsBuckleMaterial = p.IsBuckleMaterial }));
            }
        }

        /// <summary>
        /// 刷新产品工艺路线修改事件
        /// </summary>
        public void RefreshEvent()
        {
            RoutingEventList.Clear();
            if (Version == null)
                return;
            RoutingEventList.AddRange(RT.Service.Resolve<BatchWipProductRoutingController>().GetWipProductRoutingEvents(Version.Id));
        }

        /// <summary>
        /// 清空产品生产关键件
        /// </summary>
        void ClearList()
        {
            KeyItemList.Clear();
            BomList.Clear();
            DefectList.Clear();
            RepaireList.Clear();
        }

        /// <summary>
        /// 获取生产批次号
        /// </summary>
        /// <returns>生产批次号</returns>
        internal string GetWipBatchNo()
        {
            if (Batch == null)
                return string.Empty;
            if (Batch.IsChild)
                return (Batch as SubWipBatch)?.WipBatch?.BatchNo;
            return Batch.BatchNo;
        }
    }
}