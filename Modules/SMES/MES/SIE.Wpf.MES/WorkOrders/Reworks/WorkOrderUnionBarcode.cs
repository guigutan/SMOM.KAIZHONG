using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.WIP.Reworks;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;
using SIE.ObjectModel;
using System;
using System.Linq;
using System.Text;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 工单关联条码VeiwModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单关联条码")]
    public class WorkOrderUnionBarcode : ViewModel
    {
        #region 关键件是否全选 SelectKeyItems
        /// <summary>
        /// 关键件是否全选
        /// </summary> 
        public static readonly Property<bool> SelectKeyItemsProperty = P<WorkOrderUnionBarcode>.Register(e => e.SelectKeyItems);

        /// <summary>
        /// 关键件是否全选
        /// </summary>
        public bool SelectKeyItems
        {
            get { return this.GetProperty(SelectKeyItemsProperty); }
            set { this.SetProperty(SelectKeyItemsProperty, value); }
        }
        #endregion 

        #region 关联条码是否全选 SelectBarcodes
        /// <summary>
        /// 关联条码是否全选
        /// </summary> 
        public static readonly Property<bool> SelectBarcodesProperty = P<WorkOrderUnionBarcode>.Register(e => e.SelectBarcodes);

        /// <summary>
        /// 关联条码是否全选
        /// </summary>
        public bool SelectBarcodes
        {
            get { return this.GetProperty(SelectBarcodesProperty); }
            set { this.SetProperty(SelectBarcodesProperty, value); }
        }
        #endregion

        #region 是否命令触发 IsFromCmd
        /// <summary>
        /// 是否命令触发
        /// </summary> 
        public static readonly Property<bool> IsFromCmdProperty = P<WorkOrderUnionBarcode>.Register(e => e.IsFromCmd);

        /// <summary>
        /// 是否命令触发
        /// </summary>
        public bool IsFromCmd
        {
            get { return this.GetProperty(IsFromCmdProperty); }
            set { this.SetProperty(IsFromCmdProperty, value); }
        }
        #endregion 

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<WorkOrderUnionBarcode>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<WorkOrderUnionBarcode>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion 

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrderUnionBarcode>.RegisterView(e => e.PlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
        }
        #endregion

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> SnProperty = P<WorkOrderUnionBarcode>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 关联条码 BarcodeList
        /// <summary>
        /// 关联条码
        /// </summary>
        [Label("关联条码")]
        public static readonly ListProperty<EntityList<UnionBarcode>> BarcodeListProperty = P<WorkOrderUnionBarcode>.RegisterList(e => e.BarcodeList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<UnionBarcode>()
        });

        /// <summary>
        /// 工单与排产关系
        /// </summary>
        public EntityList<UnionBarcode> BarcodeList
        {
            get { return this.GetLazyList(BarcodeListProperty); }
        }
        #endregion

        #region 关键件 KeyItemList
        /// <summary>
        /// 关键件
        /// </summary>
        [Label("关键件")]
        public static readonly ListProperty<EntityList<KeyItemUnboundConfig>> KeyItemListProperty = P<WorkOrderUnionBarcode>.RegisterList(e => e.KeyItemList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<KeyItemUnboundConfig>()
        });

        /// <summary>
        /// 关键件
        /// </summary>
        public EntityList<KeyItemUnboundConfig> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion  

        #region 已关联数量 RelevancyQty
        /// <summary>
        /// 已关联数量
        /// </summary>
        [Label("已关联数量")]
        public static readonly Property<int> RelevancyQtyProperty = P<WorkOrderUnionBarcode>.Register(e => e.RelevancyQty);

        /// <summary>
        /// 已关联数量
        /// </summary>
        public int RelevancyQty
        {
            get { return GetProperty(RelevancyQtyProperty); }
            set { SetProperty(RelevancyQtyProperty, value); }
        }

        /// <summary>
        /// 刷新已关联数量
        /// </summary>
        internal void RefreshRelevancyQty()
        {
            RelevancyQty = RT.Service.Resolve<ReworkController>().GetUnionBarcodeCount(WorkOrderId);
        }
        #endregion

        #region 当前扫描数 ScanQty
        /// <summary>
        /// 当前扫描数
        /// </summary>
        [Label("当前扫描数")]
        public static readonly Property<int> ScanQtyProperty = P<WorkOrderUnionBarcode>.RegisterReadOnly(
            e => e.ScanQty, e => e.GetScanQty(), SnProperty);

        /// <summary>
        /// 当前扫描数
        /// </summary>
        public int ScanQty
        {
            get { return this.GetProperty(ScanQtyProperty); }
        }

        /// <summary>
        /// 获取当前扫描数
        /// </summary>
        /// <returns>当前扫描数</returns>
        private int GetScanQty()
        {
            return BarcodeList.Where(b => b.CodeState == CodeState.NotAssociated).Count();
        }
        #endregion

        #region 使用已有条码 IsUseExist
        /// <summary>
        /// 使用已有条码
        /// </summary>
        [Label("使用已有条码")]
        public static readonly Property<bool> IsUseExistProperty = P<WorkOrderUnionBarcode>.Register(e => e.IsUseExist);

        /// <summary>
        /// 使用已有条码
        /// </summary>
        public bool IsUseExist
        {
            get { return this.GetProperty(IsUseExistProperty); }
            set { this.SetProperty(IsUseExistProperty, value); }
        }
        #endregion

        /// <summary>
        /// 工单控制器
        /// </summary>
        WorkOrderController woController = RT.Service.Resolve<WorkOrderController>();

        /// <summary>
        /// 返修控制器
        /// </summary>
        ReworkController reworkController = RT.Service.Resolve<ReworkController>();

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="workOrder">工单</param>
        public WorkOrderUnionBarcode(WorkOrder workOrder)
        {
            WorkOrder = workOrder;
            RefreshUnionBarcodes();
            RefreshKeyItems();
            ////获取返工配置
            var isUseOld = reworkController.GetReworkIsUseOld(workOrder.No);
            IsUseExist = isUseOld;
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == SnProperty)
            {
                try
                {
                    UnionBarcode(Sn);
                }
                catch (Exception exc)
                {
                    exc.Alert();
                }
                finally
                {
                    Sn = null;
                }
            }
            else if (e.Property == WorkOrderIdProperty)
            {
                //工单切换时刷新
                RefreshRelevancyQty();
            }
        }

        /// <summary>
        /// 关联条码
        /// </summary>
        /// <param name="barcodes">条码集合</param>
        /// <returns>关联结果</returns>
        internal StringBuilder UnionBarcodes(string[] barcodes)
        {
            StringBuilder str = new StringBuilder();
            foreach (var curBarcode in barcodes)
            {
                try
                {
                    UnionBarcode(curBarcode);
                }
                catch (Exception ex)
                {
                    str.AppendLine(ex.GetBaseException().Message);
                }
            }

            return str;
        }

        /// <summary>
        /// 关联条码
        /// </summary>
        /// <param name="sn">条码</param>
        private void UnionBarcode(string sn)
        {
            if (!ValidateUnionBarcode(sn))
                return;
            AddUnionBarcode(sn);
            NotifyPropertyChanged(ScanQtyProperty);
        }

        /// <summary>
        /// 验证关联条码
        /// </summary>
        /// <param name="sn">条码</param>
        /// <returns>验证通过返回true，否则返回false</returns>
        bool ValidateUnionBarcode(string sn)
        {
            if (sn.IsNullOrWhiteSpace())
                return false;
            if (BarcodeList.Any(b => b.OriginalBarcode == sn))
                return false;
            if (WorkOrder.PlanQty < RelevancyQty + ScanQty + 1)
                throw new ValidationException("已超过工单计划数量：{0}".L10nFormat(WorkOrder.PlanQty));
            return true;
        }

        /// <summary>
        /// 添加关联条码
        /// </summary>
        /// <param name="sn">条码</param>
        private void AddUnionBarcode(string sn)
        {
            var barcode = reworkController.ValidateUnionBarcode(WorkOrder, sn, IsUseExist);
            UnionBarcode bv = new UnionBarcode()
            {
                OriginalBarcode = sn,
                WorkOrder = this.WorkOrder,
                CodeState = CodeState.NotAssociated,
                OldWorkOrderId = barcode.WorkOrderId.HasValue ? barcode.WorkOrderId.Value : 0,
            };
            bv.GenerateId();
            ////使用已有条码选项
            if (IsUseExist)
                bv.ReworkBarcode = sn;
            AddKeyItem(barcode.WorkOrderId);
            BarcodeList.Insert(0, bv);
        }

        /// <summary>
        /// 添加相关关键件
        /// </summary>
        /// <param name="workOrderId">关联条码工单ID</param>
        private void AddKeyItem(double? workOrderId)
        {
            if (!workOrderId.HasValue)
                return;
            var curWorkOrder = RF.GetById<WorkOrder>(workOrderId);
            if (curWorkOrder == null)
                return;
            var processBomList = curWorkOrder?.ProcessBomList;
            foreach (var i in processBomList)
            {
                if (KeyItemList.Any(p => p.ItemId == i.ItemId))
                    continue;
                var keyItem = new KeyItemUnboundConfig
                {
                    Item = i.Item,
                    ItemId = i.ItemId,
                    IsUnbound = false,
                    WorkOrderId = this.WorkOrder.Id,
                    WorkOrder = this.WorkOrder,
                    Unit = i.Unit,
                    SingleQty = i.SingleQty,
                    OldWorkOrderId = workOrderId.Value
                };
                keyItem.GenerateId();
                KeyItemList.Add(keyItem);
            }
        }

        /// <summary>
        /// 刷新已关联条码列表
        /// </summary>
        internal void RefreshUnionBarcodes()
        {
            BarcodeList.Clear();
            var list = reworkController.GetUnionBarcodes(WorkOrder?.No);
            if (list.Count > 0)
                BarcodeList.AddRange(list);
            BarcodeList.MarkSaved();
        }

        /// <summary>
        /// 刷新关键件列表
        /// </summary>
        internal void RefreshKeyItems()
        {
            KeyItemList.Clear();
            var list = reworkController.GetTaskKeyItemUnboundConfigs(WorkOrderId);
            if (list.Count > 0)
                KeyItemList.AddRange(list);
            KeyItemList.MarkSaved();
            SelectKeyItems = false;
        }

        /// <summary>
        /// 移除关联条码后根据当前扫描的条码集合的关键件，
        /// 更新关键件配置，不存在的物料则删除
        /// </summary>       
        /// <param name="workOrderIds">已关联条码工单集合</param>       
        internal void UpdateKeyItemConfigs()
        {
            var workOrderIds = BarcodeList.Select(p => p.OldWorkOrderId).ToList();
            var lessKeys = KeyItemList.Where(p => workOrderIds.Contains(p.OldWorkOrderId)).ToList();
            KeyItemList.Clear();
            KeyItemList.AddRange(lessKeys);
        }
    }

    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WorkOrderUnionBarcodeViewConfig : WPFViewConfig<WorkOrderUnionBarcode>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.UseDetail(columnCount: 2, dialogHeight: 500, dialogWidth: 600);
            View.UseCommands(typeof(UnionBarcodeSaveCommand));
            View.Property(p => p.WorkOrder.No).HasLabel("工单").ShowInDetail();
            View.Property(p => p.WorkOrder.PlanQty).HasLabel("计划数量");
            View.Property(p => p.RelevancyQty).Readonly();
            View.Property(p => p.ScanQty);
            View.Property(p => p.Sn).UseEditor("BarcodeEditor").ShowInDetail(columnSpan: 3, height: 40);
            View.Property(p => p.IsUseExist).Readonly().UseCheckEditor();
            View.ChildrenProperty(p => p.BarcodeList).Show(ChildShowInWhere.Detail).HasLabel("关联的条码");
        }
    }
}