SIE.defineCommand('SIE.Web.Packages.QrCodeParseRules.Commands.EditQrCodeParseRuleCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length > 1) {
            return false;
        }

        var sel = view.getSelection();
        if (sel.any(function (p) { return p.getState() === SIE.Domain.State.Enable.value; }))
            return false;
        return true;
    }
});