SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.OutboundConfirmSnDetailDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view == null || view.getParent() == null || view.getParent().getCurrent() == null) {
            return false;
        }

        if (view.getParent().getCurrent().getState() == 0 || view.getParent().getCurrent().getState() == 2)
            return true;

        return false;
    },
    execute: function (view) {
        var me = this;
        view.execute({
            data: view.getSelectionIds(),
            success: function (res) {
                if (res.Success) {
                    me.view.reloadData();
                }
            }
        });
    }
});
