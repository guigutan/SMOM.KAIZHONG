SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.DeleteRecordCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },
    canExecute: function (view) {
        var parent = view.getParent();
        var entity = view.getCurrent();
        if (parent != null && parent.getCurrent()&&parent.getCurrent().data.Result==null&&entity != null) {
            return true;
        }
        return false;
    },
})