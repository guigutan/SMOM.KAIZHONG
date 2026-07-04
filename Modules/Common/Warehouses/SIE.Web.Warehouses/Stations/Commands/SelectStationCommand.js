SIE.defineCommand('SIE.Web.Warehouses.Stations.Commands.SelectStationCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'StationId', targetClassName: 'SIE.Warehouses.Stations.Station' },
    },
    meta: { text: "选择", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getParent().getSelection() == null || view.getParent().getSelection().length != 1) {
            return false;
        }
        var parCur = view.getParent().getCurrent();
        if (parCur != null && (!parCur.isDirty()))
            return true;
        return false;
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = [];
        me.view.getData().data.items.forEach(function (item) {
            if (item)
                me._sourceViewSelectItems.push(parseFloat(item[0].StationId));
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
                    me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },

    setQueryCriteria: function (dialogView, data) {
        var criteria = dialogView._relations[0]._target.getData();
        if (data.getStationGroupType() == 1) {
            criteria.setIsInStation(true);
        } else if (data.getStationGroupType() == 2) {
            criteria.setIsOutStation(true);
        } else if (data.getStationGroupType() == 3) {
            criteria.setIsPickStation(true);
        } else if (data.getStationGroupType() == 4 || data.getStationGroupType() == 5) {
            criteria.setIsCountStation(true);
        }
        criteria.setWarehouseId(data.data.WarehouseId);
        criteria.setWarehouseId_Display(data.data.WarehouseId_Display);
        criteria.setState(SIE.Domain.State.Enable.value);
        var stateField = dialogView._relations[0]._target.getControl().getForm().findField("State");
        if (stateField) {
            stateField.setReadOnly(true);
        }
        var whField = dialogView._relations[0]._target.getControl().getForm().findField("WarehouseId");
        if (whField) {
            whField.setReadOnly(true);
        }

        //var stationTypeField = dialogView._relations[0]._target.getControl().getForm().findField("StationType");
        //if (stationTypeField) {
        //    stationTypeField.setReadOnly(true);
        //}
    },

    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var stationId = item.getId();
                if (me._sourceViewSelectItems.indexOf(stationId) === -1) {
                    var station = { StationGroupId: me._sourceId, StationId: stationId };
                    operationDatas.push(station);
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
    }
});