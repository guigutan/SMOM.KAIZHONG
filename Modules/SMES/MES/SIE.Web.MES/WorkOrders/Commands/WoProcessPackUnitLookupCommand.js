SIE.defineCommand('SIE.Web.MES.WorkOrders.WoProcessPackUnitLookupCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProcessId', targetClassName: 'SIE.Tech.Processs.Process' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        var p = view.getParent();
        if (!p) { return true; }
        var entity = p.getCurrent();
        if (entity === null) { return false };
        return true;
    },
    _loadSourceViewAllData: function (view, source) {
        /// <summary>
        /// 加载源视图的所有数据(不分页)
        /// </summary>
        var me = this;
        if (view) {
            var cfg = {
                scope: this,
                callback: function (records, operation, success) {
                    this.cloneStore._loaded = success;

                    var parent = view.getParent();
                    var sourceId;
                    if (parent) {
                        sourceId = parent.getCurrent().getId();
                    }
                    else {
                        if (view.getCurrent()) {   //没有父实体时，当前实体可能为空
                            sourceId = view.getCurrent().getId();
                        }
                    }
                    me._sourceId = sourceId;
                    var model = view.model;
                    if (model) {
                        SIE.AutoUI.getMeta({
                            model: me.dataParams.targetClassName,
                            ignoreChild: true,
                            ignoreCommands: true,
                            isReadonly: true,
                            ignoreQuery: true,
                            viewGroup: 'PackingUnitProcess',
                            isAggt: true,
                            callback: function (res) {
                                var blocks = res;
                                me._queryBlockProcess(blocks);
                                me._gridBlockProcess(blocks);
                                var ui = SIE.AutoUI.generateAggtControl(blocks);
                                me._popupWin(ui, source);
                                me._reloadTargetViewData();
                            }
                        });
                    }
                }
            }

            var store = view.getData();
            this.cloneStore = store.clone({ pageSize: this.gridCfg.pageSize }); //克隆数据store
            this.cloneStore.load(cfg);
        }
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        var dialogView = me._targetView;
        dialogView.getData().data.clear();
        var dialogstore = dialogView.getControl().getStore();
        dialogView.getControl().setStore(dialogstore);
        var routingProcess = me.view._parent._parent._children.first(function (p) { return p.model == 'SIE.MES.WorkOrders.WorkOrderRoutingProcess'; });
        var routData = routingProcess.getData().data.items.where(function (p) { return p.data.ProcessType == 20 || p.data.ProcessType == 40; });
        dialogView.getControl().getStore().data.add(routData);
        me.setSelected(dialogstore.getData().items);
    },
    setSelected: function (records) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        var me = this;
        var currPackId = me._ownerView._parent._current.data.Id;
        var sourceViewSelectItems = me._ownerView.getData().data.items.where(function (p) { return p.data.PackageRuleId == currPackId; });
        if (records.length > 0 && sourceViewSelectItems.length > 0) {
            var selModel = me._targetView.getSelectionModel();
            for (var i = 0, len = records.length; i < len; i++) {
                var record = records[i];
                if (sourceViewSelectItems.where(function (p) { return p.data.ProcessId == record.getId(); }).length > 0) {
                    selModel.select(record, true, true); //勾选上.
                }
            }
        }
    },
    save: function (win) {
        var me = this; 
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var packUnitList = [];
            selections.forEach(function (process) {
                var model = new SIE.MES.WorkOrders.WorkOrderProcessPackingUnit();
                model.setProcessId(process.getProcessId());
                model.setProcessId_Display(process.getName());
                model.setPackageRuleId(me._sourceId);
                packUnitList.push(model);
            });
            if (packUnitList.length > 0) {
                me._ownerView._parent._current._WorkOrderProcessPackingUnitList.setData(packUnitList);
                var childControl = me._ownerView._parent._children[0].getControl();
                childControl.setStore(childControl.store);
                me._ownerView._parent._parent.getData().dirty = true;
                me._ownerView._parent._parent.syncCmdState(me._ownerView._parent._parent, true);
            }
            win.close();
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
    _popupWin: function (ui, source) {
        /// <summary>
        /// 弹窗口
        /// </summary>
        /// <param name="ui" type="type"></param>
        /// <param name="source" type="type"></param>
        var me = this;
        me._targetView = ui._view;
        if (me.win && me.win.animateTarget == source) {
            return;
        }
        //弹窗
        me.win = SIE.Window.show({
            title: ('选择' + me._targetView.label).t(),
            animateTarget: source, items: ui.getControl(),
            width: 400, height: 400,
            //buttons: ['确定', '关闭'], //自定义按钮名称
            callback: function (btn) {
                if (btn === '确定'.t()) {
                    if (me._targetSelectItems.keys.length > 0) {
                        me.save(me.win);
                        return false; //阻止窗口关闭，在save中根据返回结果处理
                    } else {
                        SIE.Msg.showWarning('没有可提交的数据'.t());   //没有选择数据点击确定时，窗口直接关闭了
                        return false;
                    }
                }
            }
        });
        me.setGridListeners();
        me._targetSelectItems = { items: [], keys: [] };
    },
});