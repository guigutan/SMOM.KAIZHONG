using SIE.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Reworks;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace SIE.Web.MES.WorkOrders.Reworks
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

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WorkOrderUnionBarcode>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
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
            get
            {
                return this.GetLazyList(KeyItemListProperty);
            }


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
            return BarcodeList.Count(b => b.CodeState == CodeState.NotAssociated);
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
        /// 构造方法
        /// </summary>
        /// <param name="workOrder">工单</param>
        public WorkOrderUnionBarcode(WorkOrder workOrder)
        {
            WorkOrder = workOrder;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderUnionBarcode()
        {
        }
    }

    /// <summary>
    /// 条码关联控制器
    /// </summary>
    public class WoUnionBarcode
    {
        /// <summary>
        /// 关联条码
        /// </summary>
        /// <param name="snList">条码集合</param>
        /// <param name="woBarcode">关联条码集合</param>
        /// <returns>关联结果</returns>
        internal StringBuilder UnionBarcodes(string[] snList, WorkOrderUnionBarcode woBarcode)
        {
            var barcodes = RT.Service.Resolve<BarcodeController>().GetBarcodesBySns(snList.ToList());

            var unionBarcodes = RT.Service.Resolve<ReworkController>().GetUnionBarcodesBySnList(snList.ToList());

            var workOrderIds = barcodes.Where(x => x.WorkOrderId != null)
                .Select(x => x.WorkOrderId.Value)
                .Distinct()
                .ToList();

            var workOrderProcessBoms = RT.Service.Resolve<WorkOrderController>().GetWoProcessBomList(workOrderIds);

            StringBuilder str = new StringBuilder();
            foreach (var sn in snList)
            {
                try
                {
                    AddUnionBarcode(sn, woBarcode, barcodes, unionBarcodes, workOrderProcessBoms);
                }
                catch (Exception ex)
                {
                    str.AppendLine(ex.GetBaseException().Message);
                }
            }

            return str;
        }


        /// <summary>
        /// 添加关联条码
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="woBarcode">工单关联条码</param>
        /// <param name="barcodes"></param>
        /// <param name="unionBarcodes"></param>
        /// <param name="workOrderProcessBoms"></param>
        internal void AddUnionBarcode(string sn, WorkOrderUnionBarcode woBarcode, EntityList<Barcode> barcodes, EntityList<UnionBarcode> unionBarcodes, EntityList<WorkOrderProcessBom> workOrderProcessBoms)
        {
            var barcode = barcodes.FirstOrDefault(x => x.Sn == sn);

            if (barcode == null)
            {
                throw new ValidationException("未找到条码：{0}!".L10nFormat(sn));
            }

            if (barcode.IsScraped) //报废条码不能关联
            {
                throw new ValidationException("条码{0}已报废!".L10nFormat(sn));
            }

            if (barcode.IsPending) //挂起条码不能关联
            {
                throw new ValidationException("条码{0}已挂起!".L10nFormat(sn));
            }

            if (unionBarcodes.Any(x => x.OriginalBarcode == sn))
            {
                throw new ValidationException("条码：{0}已被替换，无法重复关联!".L10nFormat(sn));
            }

            UnionBarcode bv = new UnionBarcode()
            {
                OriginalBarcode = sn,
                WorkOrder = woBarcode.WorkOrder,
                CodeState = CodeState.NotAssociated,
                OldWorkOrderId = barcode.WorkOrderId.HasValue ? barcode.WorkOrderId.Value : 0,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            //TODO 平台后面提供批量获取ID的方法后，再优化这个地方
            bv.GenerateId();

            ////使用已有条码选项
            if (woBarcode.IsUseExist)
            {
                bv.ReworkBarcode = sn;
            }

            AddKeyItem(barcode.WorkOrderId, woBarcode, workOrderProcessBoms);

            woBarcode.BarcodeList.Insert(0, bv);
        }

        /// <summary>
        /// 添加相关关键件
        /// </summary>
        /// <param name="workOrderId">关联条码工单ID</param>
        /// <param name="woBarcode">工单关联条码</param>
        /// <param name="workOrderProcessBoms">工序BOM列表</param>
        private void AddKeyItem(double? workOrderId, WorkOrderUnionBarcode woBarcode,
            EntityList<WorkOrderProcessBom> workOrderProcessBoms)
        {
            if (!workOrderId.HasValue)
            {
                return;
            }

            var processBomList = workOrderProcessBoms.Where(x => x.WorkOrderId == workOrderId.Value);

            foreach (var i in processBomList)
            {
                if (woBarcode.KeyItemList.Any(p => p.ItemId == i.ItemId))
                {
                    continue;
                }

                var keyItem = new KeyItemUnboundConfig
                {
                    Item = i.Item,
                    ItemCode = i.Item.Code,
                    ItemName = i.Item.Name,
                    ItemId = i.ItemId,
                    IsUnbound = false,
                    WorkOrderId = woBarcode.WorkOrder.Id,
                    WorkOrder = woBarcode.WorkOrder,
                    Unit = i.Unit,
                    UnitName = i.Unit?.Name,
                    SingleQty = i.SingleQty,
                    PersistenceStatus = PersistenceStatus.New,
                    OldWorkOrderId = workOrderId.Value,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                keyItem.GenerateId();
                woBarcode.KeyItemList.Add(keyItem);
            }
        }

        /// <summary>
        /// 移除关联条码后根据当前已经关联的条码的工单bom
        /// 更新关键件配置，不存在的物料则删除
        /// </summary>
        /// <param name="woBarcode">工单关联条码</param>   
        public void UpdateKeyItemConfigs(WorkOrderUnionBarcode woBarcode)
        {
            var workOrderIds = woBarcode.BarcodeList.Select(p => p.OldWorkOrderId).ToList();
            var lessKeyIds = woBarcode.KeyItemList.Where(p => workOrderIds.Contains(p.OldWorkOrderId)).Select(p => p.Id).Distinct();
            foreach (var keyItem in woBarcode.KeyItemList)
            {
                if (!lessKeyIds.Contains(keyItem.Id))
                    keyItem.PersistenceStatus = PersistenceStatus.Deleted;
            }
        }
    }

    /// <summary>
    /// 视图配置
    /// </summary>
    internal class WorkOrderUnionBarcodeViewConfig : WebViewConfig<WorkOrderUnionBarcode>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.MES.WorkOrders.WorkOrderUnionBarcodeBehavior");
            View.UseChildrenGroupAsHorizontal();
            View.UseDetail(columnCount: 2);
            View.UseCommands("SIE.Web.MES.WorkOrders.Reworks.UnionBarcodeSaveCommand");
            View.Property(p => p.WorkOrderNo).HasLabel("工单").ShowInDetail().Readonly();
            View.Property(p => p.PlanQty).HasLabel("计划数量").Readonly();
            View.Property(p => p.RelevancyQty).Readonly();
            View.Property(p => p.ScanQty);
            View.Property(p => p.Sn).UseDisplayEditor(p => p.XType = "ReworkSnEditor").ShowInDetail(columnSpan: 3);
            View.Property(p => p.IsUseExist).Readonly().UseCheckEditor();
            View.AttachChildrenProperty(typeof(UnionBarcode), (o) =>
            {
                return new EntityList<UnionBarcode>();
            }, childLayoutType: ChildLayoutType.Card).Show(ChildShowInWhere.Detail).HasLabel("关联的条码");
            View.AttachChildrenProperty(typeof(KeyItemUnboundConfig), (o) =>
            {
                return new EntityList<KeyItemUnboundConfig>();
            }, childLayoutType: ChildLayoutType.Card).Show(ChildShowInWhere.Detail).HasLabel("关键件解绑");
        }
    }
}