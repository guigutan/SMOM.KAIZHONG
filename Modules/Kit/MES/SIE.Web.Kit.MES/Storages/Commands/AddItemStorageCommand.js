SIE.defineCommand('SIE.Web.Kit.MES.Storages.Commands.AddItemStorageCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var parentId = me.view._parent.getCurrent().getId();
        entity.setStorageLocationId(parentId);
    }
});