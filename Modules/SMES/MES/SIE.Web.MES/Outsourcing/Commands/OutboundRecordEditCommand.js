SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.OutboundRecordEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }

        var p = view.getCurrent();

        if (p == null) {
            return false;
        }
        if (p.getSN() == "" && p.getLotNo() == "" && p.getState()==10) {
            return true;
        }
    },
});