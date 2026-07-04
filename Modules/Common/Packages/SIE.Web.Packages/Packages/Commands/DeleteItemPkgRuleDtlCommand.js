SIE.defineCommand('SIE.Web.Packages.Packages.Commands.DeleteItemPkgRuleDtlCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length <= 0) {
            return false;
        }

        var sel = view.getSelection();
        if (sel.any(function (p) { return p.getIsMasterUnit() === true; })) {
            return false;
        }

        return true;
    }
});