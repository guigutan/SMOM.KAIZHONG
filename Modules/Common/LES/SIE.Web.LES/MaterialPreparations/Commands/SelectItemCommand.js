SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.SelectItemCommand", {
    meta: { text: "选择", group: "edit", iconCls: "iconfont icon-PlaylistCheck icon-green" },
    _selectItems: [],
    canExecute: function (view) {
        var parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;

        return true;
    },
    execute: function (view) {
        var me = this;
        var parentCur = view.getParent().getCurrent();
        var lineNo = 0;
        var list = me.view.getData().data.items.where(function (p) { return p.getLineNo() != ""; });
        if (list.length > 0) {
            lineNo = list.select(function (p) { return parseInt(p.getLineNo()); }).max();
        }
        SIE.AutoUI.getMeta({
            model: 'SIE.LES.MaterialPreparations.ViewModels.MaterialPrepareItemViewModel',
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
                                entity.setLineNo((lineNo + 1).toString());
                                lineNo++;
                                entity.setItemId_Display(item.ItemCode);
                                entity.setItemId(item.ItemId);
                                entity.setItemName(item.ItemName);
                                entity.setItemConsumeMode(item.ConsumeMode);
                                entity.setUnitName(item.UnitName);
                                entity.setEnableExtendProperty(item.EnableExtendProperty);
                                view.getControl().store.add(entity);
                            })
                        }
                    }
                })
            }
        })
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
            if (item.ItemId == record.getItemId()) {
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
                        return item.ItemId == record.getItemId();
                    })) {
                        selModel.select(record, true, true);
                    }
                }
            }
        }
    },
})