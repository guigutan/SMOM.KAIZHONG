SIE.defineCommand('SIE.Web.Warehouses.Commands.WorkAreaEmployeeDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length <= 0) {
            return false;
        }

        if (view.getParent() == null || view.getParent().getCurrent() == null) return false;

        return true;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？确认后直接删除！'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.getSelection().length -= sel.length;
                    view.setCurrent(null, true);
                    view.reloadData();
                },
            });
        });
    }
});