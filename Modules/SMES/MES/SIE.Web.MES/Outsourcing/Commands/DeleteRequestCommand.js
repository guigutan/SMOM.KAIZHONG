SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.DeleteRequestCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.getOutsourcingState() != 10) { return false; }
        return true;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？确认后直接删除！'.t(), sel.length), function () {
            view.execute({
                withIds: true,
                selectIds: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.setCurrent(null, true);
                    view.reloadData();
                },
            });
        });
    }
});