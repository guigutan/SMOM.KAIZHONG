SIE.defineCommand('SIE.Web.MES.WorkOrders.SaveAndClosedWorkOrderCommand', {
    extend: 'SIE.Web.MES.WorkOrders.SaveWorkOrderCommand',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveIncrease icon-blue" },
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showToast('保存成功'.t(), '完成'.t()); 
        window.setTimeout(function () {
            CRT.Workbench.closeCurrentTab();
            CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        }, 1000); 
    }
});