SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.WithdrawCommand', {
    meta: { text: "撤回", group: "edit", iconCls: "icon-FileReturn icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length != 1) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ReviewStatus !== 20) {
                res = false;
                return false;
            }
        });
        return res;
    },

    execute: function (view) {
        var modelData = view.getCurrent().data;
        view.execute({
            data: modelData,
            success: function (res) {
                SIE.Msg.showMessage("撤回成功!".t());
                view.reloadData();
            }
        });
    }
});