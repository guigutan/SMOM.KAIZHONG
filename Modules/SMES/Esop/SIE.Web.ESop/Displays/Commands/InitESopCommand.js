SIE.defineCommand('SIE.Web.ESop.Displays.Commands.InitESopCommand', {
    meta: { text: "初始化ESOP", group: "edit", iconCls: "icon-Initialization icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        me.view.execute({
            data: {},
            success: function (res) {
                if (res.Result)
                    SIE.Msg.showMessage(res.Result);
                else
                    SIE.Msg.showInstantMessage('初始化成功'.t());
                me.view.loadData();
            },
        }, me.view);
    },
});