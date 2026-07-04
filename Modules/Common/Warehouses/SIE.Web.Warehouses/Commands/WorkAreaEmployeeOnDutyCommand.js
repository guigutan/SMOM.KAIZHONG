SIE.defineCommand('SIE.Web.Warehouses.Commands.WorkAreaEmployeeOnDutyCommand', {
    meta: { text: "在岗", group: "edit", iconCls: "icon-EnableUsers icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        var sel = view.getSelection();

        return sel.all(function (p) { return p.getWorkSituation() === 1; });
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('确认设置选中的员工在岗?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        });
    }
});