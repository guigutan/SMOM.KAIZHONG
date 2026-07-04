SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.SelectReDetailCommand", {
    meta: { text: "选择", group: "edit", iconCls: "iconfont icon-PlaylistCheck icon-green" },
    _selectItems: [],
    execute: function (view) {
        var me = this;
        var parentCur = view.getParent().getCurrent();
        var woId = parentCur.getWorkOrderId();
        var wareId = parentCur.getWarehouseId();
        var workShopId = parentCur.getWorkShopId();
        var storageId = parentCur.getStorageLocationId();
        var projectNo = parentCur.getProjectId_Display();
        var parentId = parentCur.getId();

        var lineNo = 0;
        var list = view.getData().data.items;
        if (list.length > 0) {
            lineNo = list.where(function (p) { return p.getLineNo() != ""; }).select(function (p) { return parseInt(p.getLineNo()); }).max() + 1;
        }
        else {
            lineNo = 1;
        }

        SIE.AutoUI.getMeta({
            model: 'SIE.LES.MaterialReturnApplys.MaterialReturnApplyDetailSelect',
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
                var criteria = listView._relations[0]._target.getData();
                me.setCriteria(criteria, woId, wareId, storageId, workShopId, projectNo);
                var queryType = me.setQueryType(woId, wareId);

                var store = listView.getData();
                var grid = listView.getControl();
                me._selectItems = [];
                // 添加缓存翻页
                me.mon(store, 'load', (store, records, successful, operation, eOpts) => {
                    me.onLoad(me, listView, store, records, successful, operation, eOpts);
                }, me);
                me.mon(grid.getSelectionModel(), {
                    select: (selModel, record, index, eOpts) => {
                        me.onSelect(me, selModel, record, index, eOpts);
                    },
                    deselect: function (selModel, record, index, eOpts) {
                        me.onDeselect(me, selModel, record, index, eOpts);
                    }
                });
                listView._relations[0]._target.tryExecuteQuery();

                var items = ui.getControl();
                var win = SIE.Window.show({
                    title: "选择明细".t(),
                    width: 900,
                    height: 520,
                    items: items,
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            if (me._selectItems.length <= 0) {
                                SIE.Msg.showMessage("请选择数据".t());
                                return;
                            }
                            if (queryType == 0) // 查询批次lpn库存
                            {
                                SIE.Msg.wait("正在计算库存,请稍等......".t());
                                SIE.invokeDataQuery({
                                    type: "SIE.Web.LES.MaterialReturnApplys.MaterialReturnApplyDataQueryer",
                                    method: "CaseLpnOnHand",
                                    params: [me._selectItems, wareId, storageId],
                                    async: false,
                                    token: view.token,
                                    callback: function (res) {
                                        if (res.Success) {
                                            var details = res.Result;
                                            me.addDetail(me, view, lineNo, details);
                                        } else {
                                            SIE.Msg.showError(res.Message);
                                        }
                                        SIE.Msg.close();
                                    }
                                });
                            }
                            else {
                                me.addDetail(me, view, lineNo, me._selectItems);
                            }
                        }
                    }
                })
            }
        })
    },
    addDetail: function (me, view, lineNo, list) {
        list.forEach(item => {
            item.LineNo = lineNo++;
            var entity = view.createNewItem();
            entity.setLineNo(item.LineNo);
            entity.setWoDemandReportId(item.WoDemandReportId);
            entity.setItemId(item.ItemId);
            entity.setItemCode(item.ItemCode);
            entity.setItemName(item.ItemName);
            entity.setUnitName(item.UnitName);
            entity.setEnableExtendProperty(item.EnableExtendProperty);
            entity.setIsBatch(item.IsBatch);
            entity.setIsSeri(item.IsSeri);
            entity.setItemExtProp(item.ItemExtProp);
            entity.setItemExtPropName(item.ItemExtPropName);
            entity.setItemLabelId(item.ItemLabelId);
            entity.setLabel(item.Label);
            entity.setLot(item.Lot);
            entity.setLabelQty(item.LabelQty);
            entity.setLabelNgQty(item.LabelNgQty);
            entity.setAvailableQty(item.AvailableQty);
            entity.setNgQty(item.NgQty);
            entity.setReDetailQuality(item.ReDetailQuality);
            entity.setCtrlMode(item.CtrlMode);
            entity.setReceivedQty(item.ReceivedQty);
            entity.setMovedInQty(item.MovedInQty);
            entity.setFeedQty(item.FeedQty);
            entity.setMovedOutQty(item.MovedOutQty);
            entity.setReturnQtyInTransit(item.ReturnQtyInTransit);
            entity.setNgReturnQtyInTransit(item.NgReturnQtyInTransit);
            entity.setWoReturnQty(item.WoReturnQty);
            entity.setWoNgReturnQty(item.WoNgReturnQty);
            entity.setReturnQty(item.ReturnQty);
            entity.setLpnId(item.LpnId);
            entity.setLpnQty(item.LpnQty);
            entity.setNgLpnId(item.NgLpnId);
            entity.setNgLpnQty(item.NgLpnQty);

            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
            view.getControl().store.add(entity);
        })
    },

    setQueryType: function (woId, wareId) {
        if (wareId != null && woId != null) {
            return 0;
        }
        else {
            return 1;
        }
    },
    setCriteria: function (criteria, woId, wareId, storageId, workShopId, projectNo) {
        criteria.setWoId(woId);
        criteria.setWareId(wareId);
        criteria.setWorkShopId(workShopId);
        criteria.setStorageId(storageId);
        criteria.setProjectNo(projectNo);
    },
    onSelect: function (me, selModel, record, index, eOpts) {
        /// <summary>
        /// 选择事件
        /// </summary>
        this._selectItems.push(record.getData());
    },
    onDeselect: function (me, selModel, record, index, eOpts) {
        // 取消选择
        var idx = -1;
        for (var i = 0; i < this._selectItems.length; i++) {
            var item = this._selectItems[i];
            if (item.ItemId == record.getItemId()
                && item.ItemExtProp == record.getItemExtProp() && item.WoDemandReportId == record.getWoDemandReportId()
                && item.ItemLabelId == record.getItemLabelId() && item.LpnId == record.getLpnId()) {
                idx = i;
                break;
                }
        }
        if (idx > -1) {
            Ext.Array.removeAt(this._selectItems, idx);
        }
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
                        return (item.ItemId == record.getItemId() && item.ItemExtProp == record.getItemExtProp()
                            && item.ItemLabelId == record.getItemLabelId() && item.LpnId == record.getLpnId());
                    })) {
                        selModel.select(record, true, true);
                    }
                }
            }
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = e.entity;
        if (e.property == "ReDetailQuality") {
            var isgood = e.value == 0; // 0-良品 1-不良品
            if (isgood) {
                var lessQty = entity.getAvailableQty() < entity.getLabelQty() ? entity.getAvailableQty() : entity.getLabelQty();
                entity.setReturnQty(lessQty);
            }
            else {
                var lessQty = entity.getNgQty() < entity.getLabelNgQty() ? entity.getNgQty() : entity.getLabelNgQty();
                entity.setReturnQty(lessQty);
            }
        }
    }
})