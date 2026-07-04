/*
 员工维护的工厂删除命令
 */
SIE.defineCommand('SIE.Web.Resources.Employees.Commands.EmployeeEnterpriseDelCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getCurrent() == null || view.getSelection().length == 0) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var ids = view.getSelectionIds();
        view.execute({
            data: ids,
            SelectedIds: ids,
            success: function (res) {
                view.reloadData();
                view.setCurrent(null, true);
                SIE.Msg.showMessage('删除成功！'.t());
            },
        });
    }
});