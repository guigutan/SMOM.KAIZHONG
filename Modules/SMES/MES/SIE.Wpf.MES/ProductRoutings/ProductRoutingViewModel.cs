using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings.Technologys;
using System;
using System.Linq;

namespace SIE.Wpf.MES.ProductRoutings
{
    /// <summary>
    /// 产品工艺路线 ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class ProductRoutingViewModel : ViewModel
    {
        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        public static readonly Property<Barcode> BarcodeProperty = P<ProductRoutingViewModel>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public Barcode Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单
        /// </summary>
        public static readonly Property<WorkOrder> WorkOrderProperty = P<ProductRoutingViewModel>.Register(e => e.WorkOrder);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

        #region 产品生产版本 Version
        /// <summary>
        /// 产品生产版本
        /// </summary>
        public static readonly Property<WipProductVersion> VersionProperty = P<ProductRoutingViewModel>.Register(e => e.Version);

        /// <summary>
        /// 产品生产版本
        /// </summary>
        public WipProductVersion Version
        {
            get { return this.GetProperty(VersionProperty); }
            set { this.SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 选中项 SelectedItem
        /// <summary>
        /// 选中项
        /// </summary>
        public static readonly Property<IActivity> SelectedItemProperty = P<ProductRoutingViewModel>.Register(e => e.SelectedItem);

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
        public EntityList<WipProductProcessKeyItem> KeyItemList = new EntityList<WipProductProcessKeyItem>();

        /// <summary>
        /// 产品测试结果
        /// </summary>
        public EntityList<WipProductTestResult> TestResultList = new EntityList<WipProductTestResult>();

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public EntityList<WipProductDefect> DefectList = new EntityList<WipProductDefect>();

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public EntityList<WipProductRepaire> RepaireList = new EntityList<WipProductRepaire>();

        /// <summary>
        /// 产品生产BOM数据
        /// </summary>
        public EntityList<BomViewModel> BomList = new EntityList<BomViewModel>();

        /// <summary>
        /// 产品工艺路线事件列表
        /// </summary>
        public EntityList<WipProductRoutingEvent> RoutingEventList = new EntityList<WipProductRoutingEvent>();
        #endregion

        /// <summary>
        /// 根据条码初始化工艺流程、工序信息
        /// </summary>
        /// <param name="barcode">条码</param>
        public void InitInfo(Barcode barcode)
        {
            Barcode = barcode;
            WorkOrder = RF.GetById<WorkOrder>(Barcode?.WorkOrderId);
            Version = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(Barcode?.Sn);
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
                    KeyItemList.AddRange(process.KeyItemList);
                    TestResultList.AddRange(process.TestResultList);
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
                    process?.KeyItemList?.ForEach(p => BomList.Add(new BomViewModel() { ItemId = p.ItemId, Qty = p.Qty, IsBuckleMaterial = true }));
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
            RoutingEventList.AddRange(RT.Service.Resolve<WipProductRoutingController>().GetWipProductRoutingEvents(Version.Id));
        }

        /// <summary>
        /// 清空产品生产关键件
        /// </summary>
        void ClearList()
        {
            KeyItemList.Clear();
            TestResultList.Clear();
            BomList.Clear();
            DefectList.Clear();
            RepaireList.Clear();
        }
    }
}
