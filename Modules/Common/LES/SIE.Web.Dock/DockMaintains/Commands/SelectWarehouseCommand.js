SIE.defineCommand('SIE.Web.Dock.DockMaintains.Commands.SelectWarehouseCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'WarehouseId', targetClassName: 'SIE.Warehouses.Warehouse' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    _sourceViewSelectItems: [],
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = [];
        me.view.getData().data.items.forEach(function (item) {
            if (item)
                me._sourceViewSelectItems.push(parseFloat(item[0].WarehouseId));
        });
        //me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);
        var dialogView = me._targetView;
        var clearCommand = dialogView._relations[0]._target._commands.items.first(function (p) { return p.meta.command == "SIE.cmd.ClearCondition"; });
        if (clearCommand) {
            var Id = clearCommand.meta.id;
            document.getElementById(Id).style.display = "none";
        }
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                    dialogView._relations[0]._target.tryExecuteQuery();
                    //me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var warehouseId = item.getId();
                if (me._sourceViewSelectItems.indexOf(warehouseId) === -1) {
                    var warehouseData = { DockMaintainId: me._sourceId, Id: warehouseId };
                    operationDatas.push(warehouseData);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();  //关闭模态窗口
                    me._ownerView.loadChildData(true); //重载视图数据
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
    setQueryCriteria: function (dialogView, data) {
        var criteria = dialogView._relations[0]._target.getData();
        criteria.setState(1);
    },
    // end 
});