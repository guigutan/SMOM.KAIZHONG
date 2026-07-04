SIE.defineCommand('SIE.Web.MES.WorkReportPlans.Commands.SetDefaultCommand', {
    meta: { text: "设置默认", group: "edit", iconCls: "icon-ClipboardPaperCheck icon-green" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !=1) {
            return false;
        }
        if (view.getSelection().length == 1) {
            return view.getSelection()[0].getIsDefault() == false && !view.getSelection()[0].isNew();
        }
        return false;
    },
    execute: function (view, source) {
        var selectModels = view.getSelection();
        var selectIds = view.getSelectionIds(selectModels);
        SIE.Msg.askQuestion(Ext.String.format('你确定的设置选择的{0}条数据为默认报工方案吗？确认后直接完成！'.t(), selectIds.length), function () {
            view.execute({
                withIds: true,
                selectIds: selectIds,
                success: function (res) {
                    SIE.Msg.showMessage("设置默认成功!".t());
                    view.reloadData();
                },
                error: function (res) {
                    SIE.Msg.showError(res.Message);
                }
            });
        });
    }
});