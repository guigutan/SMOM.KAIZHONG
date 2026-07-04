Ext.define('SIE.Web.LES.StockOrder.StockOrderAction', {
    statics: {
        onEntityPropertyChanged: function (e) {
            var entity = e.entity;
            var me = this;
            if (e.property.length > 0 && e.entity.getData()[e.property] != e.oldvalue) {
                //推式需求计算方式变更
                if (e.property == 'PushDemandMode') {
                    entity.setDemandMode(entity.data.PushDemandMode);
                }
                //拉式需求计算方式变更
                if (e.property == 'PullDemandMode') {
                    entity.setDemandMode(entity.data.PullDemandMode);
                }
                //需求计算方式变更
                if (e.property == 'NumberOfSets' || e.property == 'DemandMode' || e.property == 'WorkOrderId' && entity.data.DemandMode == 1) {
                    if (entity.data.DemandMode == 6 && (entity.data.NumberOfSets == null || entity.data.NumberOfSets == 0)) {
                        if (e.entity.belongsView && e.entity.belongsView.getChildren()[0].getData().count() > 0) {
                            entity._StockOrderDetailList.removeAll();
                        }
                        return;
                    }
                    var data = entity.data;
                    if (e.entity.belongsView && e.entity.belongsView.getChildren()[0].getData().count() > 0) {
                        entity._StockOrderDetailList.removeAll();
                    }

                    if (data.DemandMode != 0) {
                        SIE.invokeDataQuery({
                            method: 'GetRequireItemData',
                            action: 'queryer',
                            params: [data],
                            type: 'SIE.Web.LES.StockOrders.DataQueryer.StockOrderQueryer',
                            token: me.token,
                            success: function success(res) {
                                entity._StockOrderDetailList.removeAll();
                                if (res.Result && res.Result.length > 0) {
                                    for (const element of res.Result) {
                                        var data = element;
                                        var childView = me.getChildren()[0];
                                        var newEntity = childView.createNewItem();
                                        var lineNo = 0;
                                        var list = childView.getControl().store.data.items.where(function (p) { return p.getLineNo() != ""; });
                                        if (list.length > 0) {
                                            var tempLineNoList = list.select(function (p) { return parseInt(p.getLineNo()); });
                                            lineNo = tempLineNoList.max() + 1;
                                        } else {
                                            lineNo = lineNo + 1;
                                        }

                                        newEntity.setLineNo(lineNo);
                                        newEntity.setItemId(data.ItemId);
                                        newEntity.setItemId_Display(data.ItemCode);
                                        newEntity.setItemName(data.ItemName);
                                        if (data.RequireQty < 0) {
                                            newEntity.setQty(0);
                                        } else {
                                            newEntity.setQty(data.RequireQty);
                                        }
                                        newEntity.setWoTotalQty(data.WoTotalQty);
                                        newEntity.setWarehouseId(data.WarehouseId);
                                        newEntity.setWarehouseId_Display(data.WarehouseCode);
                                        newEntity.setStorageLocationId(data.LocId);
                                        newEntity.setStorageLocationId_Display(data.LocCode);
                                        newEntity.setDemandTime(data.RequireDate);
                                        newEntity.setIsManualRec(data.IsEnabelManualRec);
                                        newEntity.setIsAllowEdit(data.IsAllowEdit);
                                        newEntity.setItemExtProp(data.ItemExtProp);
                                        newEntity.setItemExtPropName(data.ItemExtPropName);
                                        childView.getControl().store.add(newEntity);
                                    }
                                }
                            }
                        });
                    }
                }
                if (e.property == 'WorkOrderId' && entity.data.DemandMode == 0) {
                    entity._StockOrderDetailList.removeAll();
                }
            }
        },
    }
});