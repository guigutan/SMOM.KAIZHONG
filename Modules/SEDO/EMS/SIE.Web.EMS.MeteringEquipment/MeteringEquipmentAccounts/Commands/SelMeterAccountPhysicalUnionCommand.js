SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands.SelMeterAccountPhysicalUnionCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'PhysicalUnionId',
            targetClassName: 'SIE.Equipments.DeviceIOTParas.Details.PhysicalUnionSel',
            targetCriteriaClassName: 'SIE.Equipments.DeviceIOTParas.Criterias.PhysicalUnionSelCriteria'
        }
    },
    canExecute: function (view) {
        var entity = view.getParent();
        if (entity != null && entity.getParent() && entity.getParent().getCurrent()) {
            return entity.getParent().getCurrent().data.CreateBy !== null;
        }
        return false;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var physicalUnionId = item.getId();
                if (me._sourceViewSelectItems.indexOf(physicalUnionId) === -1) {
                    var equipPhysicalUnion = { EquipAccountId: me._sourceId, PhysicalUnionId: physicalUnionId };
                    operationDatas.push(equipPhysicalUnion);
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
                    var code = me.view.getParent().getParent().getCurrent().getCode();
                    var name = me.view.getParent().getParent().getCurrent().getName();
                    var criteria = dialogView._relations[0]._target.getData();
                    criteria.setEquipAccountId(me._sourceId);
                    criteria.setIsReadOnly(true);
                    criteria.setCode(code);
                    criteria.setName(name)

                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
});