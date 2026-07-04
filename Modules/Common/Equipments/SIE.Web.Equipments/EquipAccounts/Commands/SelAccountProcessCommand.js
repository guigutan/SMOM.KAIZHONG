SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.SelAccountProcessCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProcessId', targetClassName: 'SIE.Tech.Processs.Process' }
    },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.CreateBy !== null;
        }
        return false;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var lists = me._ownerView.getData().getData().items;
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var processId = item.getId();
                if (me._sourceViewSelectItems.indexOf(processId) === -1) {
                    var result = me.validateData(processId, lists);
                    if (result) {
                        var equipProcess = { EquipAccountId: me._sourceId, ProcessId: processId };
                        operationDatas.push(equipProcess);
                    }
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
    validateData: function (processId, lists) {
        lists.forEach(function (list) {
            if (list.getId() == processId)
                return false;
        });
        return true;
    },
});