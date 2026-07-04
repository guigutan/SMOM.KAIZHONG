SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.EditDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null&&entity.data.Result==null) {
            return true;
        }
        return false;
    },
});