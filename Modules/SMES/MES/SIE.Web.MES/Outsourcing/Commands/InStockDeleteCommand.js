SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.InStockDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },

    canExecute: function (view) {

        if (view.hasSelectedEntities() && view.getSelection().length > 0) {
            // NotStarted = 10, 未开始 才能提交
            var flag = view.getSelection().all(function (c) {
                return c.getState() == 10;
            });
            return flag;
        }
        return false;
    },
    execute: function (listView) {
        var selectIds = listView.getSelectionIds();

        listView.execute({
            withIds: true,
            selectIds: selectIds,
            success: function (res) { //回调
                SIE.Msg.showInstantMessage('删除成功'.t());
                listView.reloadData();
                listView._parent.reloadData();
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
            }
        });
    } 
});