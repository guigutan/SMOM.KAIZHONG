SIE.defineCommand("SIE.Web.MES.BarcodeProcesses.Commands.AfterAddProcessDetailCommand", {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.MES.BarcodeProcesses.SingleProcess' }
    },
    meta: { text: "添加", group: "edit", iconCls: "icon-Add icon-blue" },
    canExecute: function (view) {
        var parent = view._parent;
        if (parent == null) {
            return false;
        }
        var parentData = parent.getCurrent();
        if (parentData == null) {
            return false;
        }
        return true;
    },
    getViewMeta: function (source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: me.dataParams.targetClassName,
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            viewGroup:"SelectSingleProcessViewStr",
            callback: function (res) {
                me.getMetacallback(res, source);
            }
        });
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var parent = me.view._parent.getCurrent();
        var indata = {};
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var processIds = [];
            SIE.each(selections.items, function (sel) {
                var id = sel.getId();
                processIds.push(id);
            });
            if (items.length > 0) {
                indata = { BarcodeId: parent.getId(), ProcessIds: processIds };
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        if (res.Success) {
                            me.addNewDatas(me.view, res.Result);
                        }
                        win.close();
                    }
                }, me._ownerView);
                return true;
            }
        }
        Ext.Msg.alert('提示'.t(), '没有可提交的数据'.t());
    },
    addNewDatas: function (view, datas) {
        datas.forEach(item => {
            var entity = view.createNewItem();
            entity.setBarcodeProcessId(item.BarcodeProcessId);
            entity.setProcessId_Display(item.ProcessCode);
            entity.setProcessId(item.ProcessId);
            entity.setProcessName(item.ProcessName);
            entity.setIsCheck(item.IsCheck);
        });
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        //var me = this;
        //var storeProcess = me.view.getControl().getStore().data.items
        //var selectProcessIds = [];
        //storeProcess.forEach(item => {
        //    selectProcessIds.push(item.getProcessId());
        //});
        //if ((selectProcessIds && selectProcessIds.length > 0)
        //    || (me._targetSelectItems && me._targetSelectItems.items.length > 0)) {
        //    var selModel = me._targetView.getSelectionModel();
        //    if (records && records.length > 0) {
        //        for (var i = 0, len = records.length; i < len; i++) {
        //            var record = records[i];
        //            if (selectProcessIds.indexOf(record.getId()) > -1) {
        //                selModel.select(record, true, true); //勾选上.
        //            }
        //            if (me._targetSelectItems.keys.indexOf(record.getId()) > -1) {
        //                selModel.select(record, true, true);
        //            }
        //        }
        //    }
        //}
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
})