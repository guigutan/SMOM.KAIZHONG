SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.AddStockOrderDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        let parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;
        if (parentCur != null) {
            if (parentCur.data.StockState != SIE.LES.StockOrder.StockState.Created.value) {
                return false;
            }

            if (parentCur.data.DemandMode != 0) {
                return false;
            }
        }

        return true;
    },
    onItemCreated: function (entity) {

        if (entity) {
            let model = entity.data;
            let me = this;
            let bill = me.view.getParent().getCurrent().data;
            let list = me.view.getData().data.items.where(function (p) { return p.getLineNo() != ""; });
            if (list.length > 0) {
                let lineNo = list.select(function (p) { return parseInt(p.getLineNo()); }).max();
                entity.setLineNo((lineNo + 1).toString());
            }
            else {
                entity.setLineNo(1);
            }
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    let data = res.Result;
                    entity.setDemandMode(bill.DemandMode);
                    entity.setStockType(bill.StockType);
                    entity.setWorkOrderId(bill.WorkOrderId);
                    entity.setIsManualRec(data);
                    entity.setDemandTime(new Date());
                    me.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);

                    SIE.invokeDataQuery({
                        type: "SIE.Web.LES.StockOrders.DataQueryer.StockOrderQueryer",
                        method: "GetLineWaresHouse",//获取线边仓
                        params: [bill.ResourceId],
                        token: me.view.token,
                        success: function (res) {
                            if (res.Result != null && res.Result.getData().items.length>0) {
                                var data = res.Result.getData().items[0].getData();
                                entity.setStorageLocationId_Display(data.StorageLocationId_Display);
                                entity.setStorageLocationId(data.StorageLocationId);
                                entity.setWarehouseId_Display(data.WarehouseId_Display);
                                entity.setWarehouseId(data.WarehouseId);
                            }
                        }
                    });

                }
            }, me.view);

        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            var stockOrder = e.entity;
            var stockOrderData = stockOrder.data;
            var token = this.view.token;
            var parentData = this.view.getParent().getCurrent();
            debugger;
            if (e.property === "ItemId" || e.property === "ItemExtPropName") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.LES.StockOrders.DataQueryer.StockOrderQueryer",
                    method: "ChangeItemGetWorkTotalQty",//获取工单总需求
                    params: [stockOrderData.ItemId, parentData.getData().WorkOrderId, stockOrder.ItemExtProp],
                    token: token,
                    success: function (res) {
                        var data = res.Result;
                        e.entity.setWoTotalQty(data.Item1);
                        //e.entity.setItemExtProp(data.Item2);
                        //e.entity.setItemExtPropName(data.Item3);
                        debugger;
                    },
                });
            }
        }
    }
});

