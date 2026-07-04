SIE.defineCommand('SIE.Web.Warehouses.Commands.WorkAreaEmployeeOffDutyCommand', {
    meta: { text: "离岗", group: "edit", iconCls: "icon-DisableUsers icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        var sel = view.getSelection();

        return sel.all(function (p) { return p.getWorkSituation() === 0; });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('确认设置选中的员工离岗?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        });
    }
});