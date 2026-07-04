SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackingUnitDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length <= 0) {
            return false;
        }

        if (view.getSelection().any(function (p) { return p.data.IsMasterUnit; }))
            return false;
        return true;
    },
});