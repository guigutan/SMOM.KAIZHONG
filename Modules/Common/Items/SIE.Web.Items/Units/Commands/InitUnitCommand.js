SIE.defineCommand('SIE.Web.Items.Units.Commands.InitUnitCommand', {
    meta: { text: "初始化", group: "edit", iconCls: "iconfont icon-Reload icon-blue" },
    canExecute: function (view) {
        var curdata = view.getData();
        if (curdata != null) {
            for (i = 0, len = curdata.data.items.length; i < len; i++) {
                var item = curdata.data.items[i];
                if (item.data.IsInit) {
                    return false;
                }
            }
        }
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('您确认进行初始化操作吗?'.t()), function () {
            view.execute({
                data: {},
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        });
    }
});