SIE.defineCommand('SIE.Web.Warehouses.Commands.WarehouseListCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    _setCopyEntity: function (data) {
        this.callParent(arguments);
        var oldData = this.view.getCurrent().data;
        data.Code = oldData.Code + "-复制".t();
        data.Name = oldData.Name;
    }
});