SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.MulAddStockOrderDetailPushCommand', {
    meta: { text: "批量添加推式物料", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;
        if (parentCur != null) {
            if (parentCur.data.StockState != SIE.LES.StockOrder.StockState.Created.value) {
                return false;
            }

            if (parentCur.data.DemandMode != 0) {
                return false;
            }
            if (parentCur.data.StockType != 1) {
                return false;
            }
            if (parentCur.data.WorkOrderId == null) {
                return false;
            }
        }

        return true;
    },
    execute: function (view) {
        var me = this;
        var parentCur = view.getParent().getCurrent();
        var stockType = parentCur.getStockType();
        var woId = parentCur.getWorkOrderId();
        var resourceId = parentCur.getResourceId();
        var lineNo = 0;
        var list = me.view.getData().data.items.where(function (p) { return p.getLineNo() != ""; });
        if (list.length > 0) {
            lineNo = list.select(function (p) { return parseInt(p.getLineNo()); }).max();
        }
        var data = {
            ResourceId: resourceId,
            WoId: woId
        }
        view.execute({
            data: data,
            success: function (res) {
                if (res.Success) {
                    var details = res.Result;
                    if (details.length <= 0) {
                        SIE.Msg.showMessage("当前工单的工单Bom不存在符合要求的数据".t());
                        return;
                    }
                    details.forEach(d => {
                        var entity = view.createNewItem();
                        entity.setLineNo((lineNo + 1).toString());
                        lineNo++;
                        entity.setStockState(d.StockState);
                        entity.setItemId_Display(d.ItemCode);
                        entity.setItemId(d.ItemId);
                        entity.setItemName(d.ItemName);
                        entity.setDemandMode(parentCur.getDemandMode());
                        entity.setStockType(parentCur.getStockType());
                        entity.setWorkOrderId(parentCur.getWorkOrderId());
                        entity.setConsumeMode(d.ConsumeMode);
                        entity.setIsManualRec(d.IsManualRec);
                        entity.setDemandTime(new Date());
                        entity.setWoTotalQty(d.WoTotalQty);
                        entity.setIsEnableItemExtProp(d.IsEnableItemExtProp);
                        entity.setIsAllowEdit(d.IsEnableItemExtProp);
                        entity.setItemExtProp(d.ItemExtProp);
                        entity.setItemExtPropName(d.ItemExtPropName);
                        entity.setStorageLocationId_Display(d.StorName);
                        entity.setStorageLocationId(d.StorageLocationId);
                        entity.setWarehouseId_Display(d.WareName);
                        entity.setWarehouseId(d.WarehouseId);
                        entity.setQty(d.Qty);
                        view.getControl().store.add(entity);
                    });
                }
            }
        });
    }
})