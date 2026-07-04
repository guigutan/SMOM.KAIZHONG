SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.AbandonCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "放弃", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    canExecute: function (view) {
        var transList = view.getSelection();
        if (transList == null || transList.length == 0) {
            return false;
        }

        var trans = view.getCurrent();
        if (trans == null) {
            return false;
        }

        for (var i = 0; i < transList.length; i++) {
            if (transList[i].data.State != trans.data.State) return false;
        }
        //2:失败状态
        if (trans.data.State != 2 || view.getData().isDirty()) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('确定[放弃]选中的事务?'.t()), function () {
            view.execute({
                data: view.getCurrent().data,
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        });
    }
});