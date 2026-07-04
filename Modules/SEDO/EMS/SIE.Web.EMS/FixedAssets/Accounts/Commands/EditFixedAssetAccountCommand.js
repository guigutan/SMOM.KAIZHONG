SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.EditFixedAssetAccountCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.isNew()) { return false; }

        //待提交Draft = 10, 驳回 Reject = 50,
        if (p.getReviewStatus() != 50 && p.getReviewStatus() != 10) {
            return false;
        }
        return true;
    }
})