SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SelStandardSparePartAplCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择标准备件", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    modelCode: null,
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'SparePartId',
            targetClassName: 'SIE.EMS.SpareParts.StandardSparePartSel'
        }
    },
    selectedItems: [],
    canExecute: function (listview) {
        if (listview.getParent().getCurrent()) {
            var parentEntity = listview.getParent().getCurrent();

            var curId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId.toString();
            var item = parentEntity;
            var employeeIdsArr = [Ext.isEmpty(item.data.RepairMasterId) ? "" : item.data.RepairMasterId.toString()];
            if (!Ext.isEmpty(item.data.RepairEmployeeIds))
                employeeIdsArr = employeeIdsArr.concat(item.data.RepairEmployeeIds.split(','));
            if (item.data.RepairState == 0 || item.data.RepairState == 4 || item.data.RepairState == 5 || item.data.RepairState == 7 || item.data.RepairState == 8 || employeeIdsArr.indexOf(curId) < 0)
                return false;
            return true;
        }
        else
            return false;
    },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var sparePartId = item.getId();
                if (me._sourceViewSelectItems.indexOf(sparePartId) === -1) {
                    var equipRepairBill = { EquipRepairBillId: me._sourceId, SparePartId: sparePartId };
                    operationDatas.push(equipRepairBill);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();
                    me._ownerView.loadChildData(true);
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.L10N());
        }
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
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));

                    //查询实体赋值
                    var EquipModelId = me.view.getParent().getCurrent().getEquipModelId();
                    var criteria = dialogView._relations[0]._target.getData();
                    criteria.setEquipModelId(EquipModelId);

                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    }
});