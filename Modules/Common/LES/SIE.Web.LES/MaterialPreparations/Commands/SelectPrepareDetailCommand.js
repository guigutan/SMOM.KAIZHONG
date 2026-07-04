SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.SelectPrepareDetailCommand", {
    meta: { text: "选择", group: "edit", iconCls: "iconfont icon-PlaylistCheck icon-green" },
    _selectItems: [],
    canExecute: function (view) {
        var parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;
        if (parentCur.getWorkOrderId() == null) return false;
        return true;
    },
    execute: function (view) {
        var me = this;
        var parentCur = view.getParent().getCurrent();
        var woId = parentCur.getWorkOrderId();
        var preType = parentCur.getPrepareType();
        SIE.AutoUI.getMeta({
            model: 'SIE.LES.MaterialPreparations.MaterialPreparationDetailSelect',
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
                me.setCriteria(criteria, woId, preType);
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
                    title: "选择物料".t(),
                    width: 900,
                    height: 520,
                    items: items,
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            if (me._selectItems.length <= 0) {
                                SIE.Msg.showMessage("请选择数据".t());
                                return;
                            }
                            me._selectItems.forEach(item => {
                                var entity = view.createNewItem();
                                entity.setMaterialPreparationId(parentCur.getId());
                                entity.setLineNo(item.LineNo);
                                entity.setItemId_Display(item.ItemCode);
                                entity.setItemId(item.ItemId);
                                entity.setItemName(item.ItemName);
                                entity.setItemConsumeMode(item.ItemConsumeMode);
                                entity.setUnitName(item.UnitName);
                                entity.setEnableExtendProperty(item.EnableExtendProperty);
                                entity.setBomNeedQty(item.BomNeedQty);
                                entity.setCanPrepareQty(item.CanPrepareQty);
                                entity.setQty(item.Qty);
                                view.getControl().store.add(entity);
                            })
                        }
                    }
                })
            }
        });
    },
    setCriteria: function (criteria, woId, preType) {
        criteria.setWoId(woId);
        criteria.setPreType(preType);
    },
    onSelect: function (me, selModel, record, index, eOpts) {
        /// <summary>
        /// 选择事件
        /// </summary>
        this._selectItems.push(record.getData());
    },
    onDeselect: function (me, selModel, record, index, eOpts) {
        // 取消选择
        var details = me.view.getData().data.items;
        var idx = -1;
        var indetail = false;
        for (var i = 0; i < this._selectItems.length; i++) {
            var item = this._selectItems[i];
            if (item.LineNo == record.getLineNo()) {
                idx = i;
                break;
            }
        }
        for (var i = 0; i < details.length; i++) {
            var detail = details[i];
            if (detail.getLineNo() == record.getLineNo()) {
                indetail = true;
                break;
            }
        }
        if (idx > -1 && !indetail) {
            Ext.Array.removeAt(this._selectItems, idx);
        }
        else {
            selModel.select(record, true, true);
        }
    },
    onLoad: function (me, listView, store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        var details = me.view.getData().data.items;
        var selModel = listView.getSelectionModel();
        if (records && records.length > 0) {
            for (var i = 0, len = records.length; i < len; i++) {
                var record = records[i];
                if (details.some(function (item) {
                    return item.getLineNo() == record.getLineNo();
                }) || me._selectItems.some(function (item) {
                    return item.LineNo == record.getLineNo();
                })) {
                    selModel.select(record, true, true);
                }
            }
        }
    },
})