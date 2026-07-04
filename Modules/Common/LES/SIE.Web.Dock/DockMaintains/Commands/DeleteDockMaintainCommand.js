SIE.defineCommand('SIE.Web.Dock.DockMaintains.Commands.DeleteDockMaintainCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        if (view.getCurrent() != null) {
            if (view.getCurrent().isNew())
                return true;
        }

        var sel = view.getSelection();
        if (sel.any(function (p) { return p.getState() === SIE.Domain.State.Enable.value; }))
            return false;
        return true;
    }
});