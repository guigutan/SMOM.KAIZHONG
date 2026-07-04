SIE.defineCommand('SIE.Web.Elec.MES.TurnoverTools.Commands.CancelModelDedicatedCommand', {
    meta: { text: "取消专用", group: "edit", iconCls: "icon-DeleteUsers icon-blue" },
    canExecute: function (listView) {
        var selectModels = listView.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.IsDedicated === false) {
                res = false;
            }
        });
        return res;
    },
    execute: function (listView) {
        var selectModels = listView.getSelectionIds();
        var msg = Ext.String.format('你确定取消选择的{0}条数据为专用容器吗？'.L10N(), selectModels.length);

        SIE.Msg.askQuestion(msg, function () {
            listView.execute({
                withIds: true,
                selectIds: selectModels,
                success: function (res) { //回调
                    SIE.Msg.showInstantMessage('取消成功');
                    listView.reloadData();
                }
            });
        });
    }
});