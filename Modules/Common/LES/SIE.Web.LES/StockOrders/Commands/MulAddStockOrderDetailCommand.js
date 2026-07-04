SIE.defineCommand('SIE.Web.LES.StockOrders.Commands.MulAddStockOrderDetailCommand', {
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    _selectItems:[],
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
        SIE.AutoUI.getMeta({
            model: 'SIE.Web.LES.StockOrders.WorkOrders.StockOrderItemViewModel',
            ignoreChild: true,
            ignoreCommands: false,
            isReadonly: false,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                blocks.mainBlock.gridConfig.selModel = {
                    selType: 'checkboxmodel',
                    singleSelect: false, //是否单选
                    checkOnly: true, //只允许用户通过复选框选中
                    pruneRemoved: true //默认true，翻页保持勾选
                };
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                var listView = ui.getView();
                var store = listView.getData();
                var grid = listView.getControl();
                me.setQueryCriteria(listView, stockType, woId);

                me._selectItems = [];
                // 添加缓存翻页
                me.mon(store, 'load', (store, records, successful, operation, eOpts) => {
                    me.onLoad(me, listView, store, records, successful, operation, eOpts);
                }, me);
                me.mon(grid.getSelectionModel(), {
                    select: (selModel, record, index, eOpts) => {
                        me.onSelect(me, selModel, record, index, eOpts);
                    }
                });

                listView._relations[0]._target.tryExecuteQuery();
                var items = ui.getControl();
                var win = SIE.Window.show({
                    title: "选择物料".t(),
                    width: 900,
                    height: 520,
                    items: items,
                    id: "stockorderselitem001",
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            var sel = me._selectItems;
                            var itemList = [];
                            sel.forEach(p => {
                                itemList.push(p);
                            })
                            var data = {
                                ResourceId: resourceId,
                                ItemList: itemList,
                                WoId: woId
                            }
                            if (itemList.length <= 0) {
                                SIE.Msg.showMessage("没有可提交的数据".t());
                                return;
                            }
                            view.execute({
                                data: data,
                                success: function (res) { //回调
                                    if (res.Success) {
                                        var details = res.Result;
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
                    }
                });
            }
        })
    },
    onSelect: function (me, selModel, record, index, eOpts) {
        /// <summary>
        /// 选择事件
        /// </summary>
        this._selectItems.push(record.getData());
    },
    onLoad: function (me, listView, store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        if (me._selectItems && me._selectItems.length > 0) {
            var selModel = listView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._selectItems.some(function (item) {
                        return item.Id == record.getId() && item.ItemExtProp == record.getItemExtProp();
                    })) {
                        selModel.select(record, true, true);
                    }
                }
            }
        }
    },
    //设置查询条件
    setQueryCriteria: function (dialogView, stockType, woId) {
        var criteria = dialogView._relations[0]._target.getData();
        if (criteria) {
            if (stockType == 0) {
                criteria.setConsumeMode(0);
                criteria.setStockType(0);
            }
            else if (stockType == 1) {
                criteria.setConsumeMode(1);
                criteria.setStockType(1);
                criteria.setWoId(woId);
            }
        }
    },
})