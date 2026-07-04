SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.SaveDemandCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        this.callParent(arguments);
        SIE.Msg.showInstantMessage('保存成功！'.t(), '提示'.t(), 3);
        window.setTimeout(function () {
            CRT.Event.fire("SIE.Fixtures.FixtureDemands.FixtureDemand_refresh");
            CRT.Workbench.closeCurrentTab();
        }, 3000);
    }
});