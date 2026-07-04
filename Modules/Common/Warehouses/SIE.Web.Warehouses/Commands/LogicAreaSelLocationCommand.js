SIE.defineCommand('SIE.Web.Warehouses.Commands.LogicAreaSelLocationCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'StorageLocationId', targetClassName: 'SIE.Warehouses.StorageLocation',
        },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);
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
                    me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    _queryBlockProcess: function (block) {
        /// <summary>
        /// 查询块处理-只读为false
        /// </summary>
        /// <param name="block" type="type"></param>
        if (block.surrounders) {
            var surround = block.surrounders["0"];
            if (surround) {
                var items = surround.mainBlock.formConfig.items;
                for (var i = 0, len = items.length; i < len; i++) {
                    var item = items[i];
                    if (item.name == "WarehouseId" || item.name == "IsAutomatedArea") {
                        item.readOnly = true;
                    } else {
                        item.readOnly = false;
                    }
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
                var locationId = item.getId();
                if (me._sourceViewSelectItems.indexOf(locationId) === -1) {
                    var location = { LogicAreaId: me._sourceId, StorageLocationId: locationId };
                    operationDatas.push(location);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    me.view.getParent().getCurrent().setIsAutomatedArea(res.Result);                     
                    me.view.getParent().getCurrent().markSaved();
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
        criteria.setWarehouseId(data.getWarehouseId());
        criteria.setWarehouseId_Display(data.getWarehouseCode());

        var items = data.belongsView._children[0].getData().data.items;

        if (items.length > 0) {
            if (items[0].data.IsAutomatedArea)
                criteria.setIsAutomatedArea(1);
            else
                criteria.setIsAutomatedArea(0);
        }
    },
    // end 
});