SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendSubmitCommand", {
    meta: { text: "提交", group: "edit", iconCls: "icon-Submit icon-blue" },
    canExecute: function (view) {
        var selModel = view.getSelection();
        if (selModel == null) {
            return false;
        }
        if (selModel.length <= 0) {
            return false;
        }
        var flag = true;
        for (var i = 0; i < selModel.length; i++) {
            var data = selModel[i].getData();
            if (data.LendState != 0) { // 只有状态为保存的单据才能提交
                flag = false;
                break;
            }
        }
        return flag;
    },
    execute: function (view) {
        var me = this;
        var selIds = view.getSelectionIds();
        view.execute({
            withIds: true,
            selectIds: selIds,
            success: function (res) { //回调
                SIE.Msg.showInstantMessage('提交成功'.t());
                view.reloadData();
            }
        })
    }
})