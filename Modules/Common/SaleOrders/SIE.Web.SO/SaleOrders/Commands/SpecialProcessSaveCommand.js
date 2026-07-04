SIE.defineCommand('SIE.Web.SO.SaleOrders.Commands.SpecialProcessSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    execute: function (view, source) {
        var me = this;
        me.doSave(view);
        me.view.getParent().reloadData();
    },
    onSaved: function (view, res) {
        view._parent._current.setSpecialProcessStr(res.Result);
    }
});
