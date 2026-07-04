SIE.defineCommand('SIE.Web.Fixtures.InboundOrders.Commands.SumbitInboundCommand', {
    meta: { text: "提交", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    extend: 'SIE.cmd.FormSave',
    canExecute: function (view) {
        var curModel = view.getCurrent();

        if (curModel != null) {
            if (view.getData().getInboundStatus() == 10 || view.getData().getInboundStatus() == 20) {
                view.getCurrent().dirty = true;
                return true;
            }
        } else return false;
    },
    onSaved: function (view, res) {
        if (res.Success) {
            SIE.Msg.showMessage("提交成功！".t());
            warehouseImport_win.ParentView.reloadData();
            warehouseImport_win.close();
        }
    }
});
