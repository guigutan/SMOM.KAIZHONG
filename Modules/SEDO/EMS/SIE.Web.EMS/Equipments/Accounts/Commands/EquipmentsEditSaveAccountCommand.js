SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.EquipmentsEditSaveAccountCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    doSave: function (view) {
        var me = this;
        var children = view.getChildren();
        var withChildren = children.length > 0;
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                me.onSaved(view, res);
            }
        });
    }
});