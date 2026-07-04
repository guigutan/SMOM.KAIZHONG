SIE.defineCommand('SIE.Web.Fixtures.InboundOrders.Commands.SaveInboundCommand', {
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.FormSave',
    onSaved: function (view, res) {
        if (res.Success) {
            SIE.Msg.showMessage("保存成功！".t());
            warehouseImport_win.ParentView.reloadData();
            warehouseImport_win.close();
        }
    }
});
