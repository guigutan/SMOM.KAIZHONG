using SIE.Domain;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.LES.StockOrders;
using SIE.LES.StockOrders.Service;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Warehouses;
using SIE.Web.Items._Extentions_;
using SIE.Web.LES.StockOrders.Commands;
using System.Collections.Generic;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 物料需求明细视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class StockOrderDetailViewConfig : WebViewConfig<StockOrderDetail>
    {
        /// <summary>
        /// 只读视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { ReadonlyView });
            View.AssignAuthorize(typeof(StockOrder));
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadOnlyView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands("SIE.Web.LES.StockOrders.Commands.MulAddStockOrderDetailCommand", "SIE.Web.LES.StockOrders.Commands.MulAddStockOrderDetailPushCommand", WebCommandNames.Delete);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly();
                View.Property(p => p.StockState).Readonly();
                View.Property(p => p.ItemId).UseDataSource((e, c, r) =>
                {
                    var dtl = e as StockOrderDetail;
                    if (dtl == null)
                    {
                        return new EntityList<Item>();
                    }

                    if (dtl.StockType == SIE.LES.PrepareItemType.Push && dtl.WorkOrderId.HasValue)
                    {
                        return RT.Service.Resolve<StockOrderService>().GetWorkOrderBomItems(dtl.WorkOrderId.Value, c, r);
                    }

                    if (dtl.StockType == SIE.LES.PrepareItemType.Pull)
                    {
                        return RT.Service.Resolve<ItemController>().GetConsumeModeItems(c, r, ConsumeMode.Pull);
                    }

                    ConsumeMode? mode = null;
                    if (dtl.StockType == SIE.LES.PrepareItemType.Pull)
                    {
                        mode = ConsumeMode.Pull;
                    }
                    if (dtl.StockType == SIE.LES.PrepareItemType.Push)
                    {
                        mode = ConsumeMode.Push;
                    }

                    return RT.Service.Resolve<ItemController>().GetConsumeModeItems(c, r, mode);
                }).UsePagingLookUpEditor((p, t) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(t.ItemName), nameof(t.Item.Name));
                    keyValues.Add(nameof(t.IsAllowEdit), nameof(t.Item.EnableExtendProperty));
                    keyValues.Add(nameof(t.IsEnableItemExtProp), nameof(t.Item.EnableExtendProperty));
                    keyValues.Add(nameof(t.ConsumeMode), nameof(t.Item.ConsumeMode));
                    p.DicLinkField = keyValues;
                }).Readonly()
                .Cascade(p => p.ItemExtPropName, null).Cascade(p => p.ConsumeMode, null).ShowInList(120);
                View.Property(p => p.ItemName).Readonly().ShowInList(120);
                //View.Property(p => p.IsEnableItemExtProp).ShowInList().Readonly();
                View.Property(p => p.ConsumeMode).Readonly().ShowInList(120);
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.DbField = "ItemExtProp";
                }).Readonly(p => !p.IsEnableItemExtProp || p.DemandMode != SIE.LES.Commons.DemandMode.ManualFillIn || !p.IsAllowEdit).ShowInList(180);
                View.Property(p => p.ProcessId).Readonly(p => p.StockState != StockState.Created);
                View.Property(p => p.Qty).UseItemUnitEditor().Readonly(p => p.DemandMode != SIE.LES.Commons.DemandMode.ManualFillIn || p.StockState != StockState.Created);
                View.Property(p => p.WoTotalQty).Readonly();
                View.Property(p => p.DemandTime).UseDateTimeEditor().Readonly(p => p.DemandMode != SIE.LES.Commons.DemandMode.ManualFillIn || p.StockState != StockState.Created);
                View.Property(p => p.WarehouseId).UseDataSource((e, c, r) =>
                {
                    var itemReqDtl = e as StockOrderDetail;
                    if (itemReqDtl == null)
                    {
                        return new EntityList<Warehouse>();
                    }

                    return RT.Service.Resolve<LinesideWarehouseController>().GeWarehouses(c, r);
                }).Cascade(p => p.StorageLocation, null).Readonly(p => p.StockState != StockState.Created);
                View.Property(p => p.StorageLocationId).UseDataSource((o, e, r) =>
                {
                    var itemReqDtl = o as StockOrderDetail;
                    if (itemReqDtl == null || !itemReqDtl.WarehouseId.HasValue)
                    {
                        return new EntityList<StorageLocation>();
                    }

                    return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(itemReqDtl.WarehouseId.Value, r, e);
                }).UsePagingLookUpEditor(p =>
                {
                    p.ReloadDataOnPopping = true;
                }).Readonly(p => p.StockState != StockState.Created);
                View.Property(p => p.IsManualRec).Readonly(p => p.DemandMode != SIE.LES.Commons.DemandMode.ManualFillIn || p.StockState != StockState.Created);
                View.Property(p => p.UnFinishQty);
                View.Property(p => p.ShipQty).Readonly();
                View.Property(p => p.ReceiveQty).Readonly();
                View.Property(p => p.CancelQty).Readonly();
            }
        }

        ///<summary>
        /// 配置只读视图
        /// </summary>
        protected void ConfigReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList();
                View.Property(p => p.StockState).ShowInList();
                View.Property(p => p.ItemId).ShowInList(120);
                View.Property(p => p.ItemName).Readonly().ShowInList(120);
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList(180);
                View.Property(p => p.ProcessId).ShowInList();
                View.Property(p => p.Qty).ShowInList();
                View.Property(p => p.WoTotalQty).ShowInList();
                View.Property(p => p.DemandTime).ShowInList();
                View.Property(p => p.WarehouseId).ShowInList();
                View.Property(p => p.StorageLocationId).ShowInList();
                View.Property(p => p.IsManualRec).Readonly().ShowInList();
                View.Property(p => p.UnFinishQty);
                View.Property(p => p.ShipQty).ShowInList().Readonly();
                View.Property(p => p.ReceiveQty).ShowInList().Readonly();
                View.Property(p => p.CancelQty).ShowInList().Readonly();
            }
        }
    }
}