SIE.defineCommand('SIE.Web.DIST.GoodsIssueDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {//选中项
            return false;
        }
        var entity = view.getCurrent().data;
        if (entity.DistributionQty <= 0) {//选中项的配送数量小于等于0
            return true;
        }
        return false;
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