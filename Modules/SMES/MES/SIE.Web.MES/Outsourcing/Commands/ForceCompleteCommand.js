SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.ForceCompleteCommand', {
    meta: { text: "强制完成", group: "edit", iconCls: "icon-ArrowWithCircleDown icon-green" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length <= 0) {
            return false;
        }
        // NotStarted = 10, 未开始 才能强制完成
        var flag = view.getSelection().all(function (c) {
            return c.getOutsourcingState() == 30 && !c.isNew();
        });

        return flag;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        SIE.Msg.askQuestion(Ext.String.format('你确定强制完成选择的{0}条数据吗？确认后直接强制完成！'.t(), selectIds.length), function () {
            SIE.Msg.wait("正在强制完成......".t());
            view.execute({
                withIds: true,
                selectIds: selectIds,
                success: function (res) {
                    SIE.Msg.showMessage("强制完成成功!".t());
                    view.reloadData();
                },
                error: function (res) {
                    SIE.Msg.showError(res.Message);
                }
            });
        });
    }
});