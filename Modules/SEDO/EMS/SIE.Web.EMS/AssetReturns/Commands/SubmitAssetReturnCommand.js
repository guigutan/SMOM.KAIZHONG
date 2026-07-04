SIE.defineCommand('SIE.Web.EMS.AssetReturns.Commands.SubmitAssetReturnCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-Refuse icon-blue" },
    canExecute: function (view) {

        if (view.hasSelectedEntities()) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getApprovalStatus() != 10 && item.getApprovalStatus() != 50) {
                    flag = false;
                    return false;
                }
            });
            return flag;
        }
        else {
            return false;
        }
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        SIE.Msg.askQuestion("是否提交？提交后单据不能修改。".t(), function () {
            view.execute({
                withIds: true,
                selectIds: selectIds,
                success: function (res) {
                    SIE.Msg.showMessage("提交成功!".t());
                    view.reloadData();
                }
            });
        });
    }
});