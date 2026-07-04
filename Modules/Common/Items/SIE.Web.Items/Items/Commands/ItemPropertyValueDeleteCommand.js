SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemPropertyValueDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }
        return true;
    },

    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) {
                    var errMsg = res.Result;
                    if (errMsg == '删除成功')
                        view.getParent().reloadData();
                },
            });
        });
    }
});