SIE.defineCommand("SIE.Web.ProductIntfc.InspLogs.Commands.InspLogExamineSubmitCommand", {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-Submit icon-blue" },
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showInstantMessage('审核提交成功！'.t());
        CRT.Event.fire("SIE.ProductIntfc.ProductInsps.ProductInsp_refresh");
        view.getControl().up('window').close();
    }
});
