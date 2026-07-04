SIE.defineCommand('SIE.Web.MES.ItemFixture.Commands.FixtureItemDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            debugger;
            if (model.data.State == 1)
                res = false;
        });
        return res;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？'.t(), sel.length), function () {
            view.execute({
                data: view.getCurrent().data.Id,
                success: function (res) {
                    view.reloadData();
                    view.setCurrent(null, true);
                },
            });
        });
    }
});