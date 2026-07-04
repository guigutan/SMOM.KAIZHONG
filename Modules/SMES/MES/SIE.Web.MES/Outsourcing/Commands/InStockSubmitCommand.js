SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.InStockSubmitCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-ArrowWithCircleDown icon-green" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length <= 0) {
            return false;
        }

        // NotStarted = 10, 未开始 才能提交
        var flag = view.getSelection().all(function (c) {
            return c.getState() == 10 && !c.isDirty();
        });

        return flag;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        view.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) {
                SIE.Msg.showMessage("提交成功!".t());
                view.reloadData();
                if (view._parent) {
                    view._parent.reloadData();
                }
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
            }
        });
    }
});