SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.DeleteRepairDetailCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.isNew()) {
            return true;
        }
        return false;
    },
})