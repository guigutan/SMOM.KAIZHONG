SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageHistoryCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.Andon.Andons.AndonManageHistory', targetCriteriaClassName:"SIE.Andon.Andons.AndonExperienceCriterial" },
        gridCfg: {
            multiSelect: false,
        },
    },
    meta: { text: "选择历史安灯", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (view, source) {
        var me = this;
        var model = view.model;
        var entity = view.getCurrent();
        if (model) {
            SIE.AutoUI.getMeta({
                model: me.dataParams.targetClassName,
                viewGroup: 'ListView',
                ignoreChild: true,
                ignoreCommands: true,
                isReadonly: true,
                ignoreQuery: false,
                isAggt: true,
                callback: function (res) {
                    var blocks = res;
                    me._queryBlockProcess(blocks);
                    me._gridBlockProcess(blocks);
                    var ui = SIE.AutoUI.generateAggtControl(blocks);
                    me._popupWin(ui, source);
                    ui._view._relations[0]._target.tryExecuteQuery();
                }
            });
        }
    },
    save: function (win) {
        var me = this;
        var select = me._targetView.getSelection()[0].data;
        var entity = me._ownerView.getData();
        entity.setAndonManageClass(select.AndonManageClass);
        entity.setAndonTypeId_Display(select.AndonTypeId_Display);
        entity.setAndonTypeId(select.AndonTypeId);
        entity.setAndonId_Display(select.AndonId_Display);
        entity.setAndonId(select.AndonId);
        entity.setSolution(select.Solution);
        entity.setDepartment(select.Department);
        entity.setPriority(select.Priority);
        entity.setDefect(select.Defect);
        entity.setProblemDesc(select.ProblemDesc);

        entity.setFactoryId_Display(select.FactoryId_Display);
        entity.setFactoryId(select.FactoryId);
        entity.setWorkShopId_Display(select.WorkShopId_Display);
        entity.setWorkShopId(select.WorkShopId);
        entity.setWipResourceId_Display(select.WipResourceId_Display);
        entity.setWipResourceName(select.WipResourceName);
        entity.setWipResourceId(select.WipResourceId);
        entity.setStationId_Display(select.StationId_Display);
        entity.setStationId(select.StationId);
        entity.setEquipAccountId_Display(select.EquipAccountId_Display);
        entity.setEquipAccountId(select.EquipAccountId);
        entity.setEquipAccountName(select.EquipAccountName);
        entity.setWorkGroup(select.WorkGroup);

        entity.setWorkOrderId_Display(select.WorkOrderId_Display);
        entity.setWorkOrderId(select.WorkOrderId);
        entity.setProcessId_Display(select.ProcessId_Display);
        entity.setProcessId(select.ProcessId);
        entity.setBarCode(select.BarCode);
        entity.setLineStop(select.LineStop);
        entity.setAskMaterial(select.AskMaterial);
        entity.setLineStopFlag(select.LineStopFlag);
        entity.setAskMaterialFlag(select.AskMaterialFlag);
        win.close();
    },

});
