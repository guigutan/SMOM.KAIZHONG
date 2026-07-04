SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.SubmitCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-Refuse icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length != 1) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            //待提交Draft = 10, 驳回 Reject = 50,
            if (model.data.ReviewStatus !== 10 && model.data.ReviewStatus != 50) {
                res = false;
                return false;
            }
        });
        return res;
    },

    execute: function (view) {
        var me = this;
        var modelData = view.getCurrent().data;
        view.execute({
            data: modelData,
            success: function (res) {
                SIE.Msg.showMessage("提交成功!".t());
                view.reloadData();
            }
        });
    }
});