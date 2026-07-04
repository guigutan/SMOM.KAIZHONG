SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.SavePurOrderCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * @override 保存后处理
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        current.markSaved();
        me.onSavedMsg(view, res);
        CRT.Workbench.closeCurrentTab();
        CRT.Event.fire("SIE.EMS.Purchases.PurchaseOrders.PurchaseOrder_refresh");
    }
});