SIE.defineCommand('SIE.Web.Warehouses.Commands.InitReasonCommand', {
    meta: { text: "初始化", group: "edit", iconCls: "iconfont icon-Reload icon-blue" },
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