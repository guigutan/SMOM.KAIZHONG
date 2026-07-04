SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageLocationItemDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？确认后直接删除！'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.setCurrent(null, true);
                    view.reloadData();
                },
            });
        });
    }
});