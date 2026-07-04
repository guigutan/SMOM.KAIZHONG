SIE.defineCommand('SIE.Web.Packages.QrCodeParseRules.Commands.EditQrCodeParseRuleDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getParent() == null || view.getParent().getCurrent() == null)
            return false;

        if (view.getParent().getCurrent().isNew()) return false;

        if (view.getParent().getCurrent().getState() === SIE.Domain.State.Enable.value)
            return false;

        return true;
    }
});