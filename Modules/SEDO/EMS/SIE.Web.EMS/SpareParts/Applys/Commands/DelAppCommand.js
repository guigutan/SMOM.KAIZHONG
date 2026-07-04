SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.DelAppCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0) {
            return false;
        }

        var state = view.getCurrent().getAuditState();
        if (state != 0 && state!=2) {
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
                    var count = res.Result;
                    SIE.Msg.showMessage(Ext.String.format("成功删除{0}条数据。".L10N(),count));
                    view.reloadData();
                },
            });
        });
    }
});