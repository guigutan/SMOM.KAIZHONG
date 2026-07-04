SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.EquipAccountSlotSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit" },
    doSave: function (view) {
        var me = this;

        //此功能父ID丢失，要手动赋值
        var parentId = view.getParent().getCurrent().getId();
        view.getData().data.items.forEach(function (item) {
            item.setEquipAccountId(parentId);
        });

        var children = view.getChildren();
        var withChildren = children.length > 0;
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                me.onSaved(view, res);
            }
        });
    },
});