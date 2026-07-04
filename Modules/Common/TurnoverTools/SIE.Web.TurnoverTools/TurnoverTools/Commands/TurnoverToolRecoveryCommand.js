SIE.defineCommand('SIE.Web.MES.TurnoverTools.Commands.TurnoverToolRecoveryCommand', {
    meta: { text: "回收", group: "edit", hierarchy: "状态", iconCls: "icon-Sync icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();
        if (current == null)
            return false;
        else return current.data.State == 15;
    },
    execute: function (listView, source) {
        var me = this;
        listView.execute({
            data: listView.getCurrent().getId(),
            callback: function (res) {
                if (res.Success) {
                    listView.reloadData();
                }
            }
        });
    },
});