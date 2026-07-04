SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.EditRecordCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var parent = view.getParent();
        var entity = view.getCurrent();
        if (parent != null && parent.getCurrent()&&parent.getCurrent().data.Result==null&&entity != null) {
            return true;
        }
        return false;
    },
});