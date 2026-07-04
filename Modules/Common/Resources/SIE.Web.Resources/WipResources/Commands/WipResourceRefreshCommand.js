SIE.defineCommand('SIE.Web.Resources.WipResources.Commands.WipResourceRefreshCommand', {
    meta: { text: "同步资源", group: "business", iconCls: "icon-ListConfig icon-blue" },
    execute: function (view, source) {
        SIE.Msg.wait("正在同步资源，请稍等...".t());
        view.execute({
            data: {},
            success: function (res) {
                if (res.Result)
                    SIE.Msg.showMessage(Ext.String.format('同步出错:{0}', res.Result));
                else
                    SIE.Msg.showMessage('同步成功'.L10N());
            }
        });
    }
});