SIE.defineCommand('SIE.Web.Elec.MES.TurnoverTools.Commands.TurnoverToolDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var selection = view.getSelection();
        if (selection == null || selection.length == 0)
            return false;
        if (selection.any(function (p) { return p.getState() !== 5 }))
            return false;
        return true;
    },
    execute: function (view, source) {
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？删除后，需要再次点击保存！'.t(), sel.length), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) {
                    view.removeSelection();
                    view.setCurrent(null, true);
                },
            });
        });
    }
});