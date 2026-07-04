SIE.defineCommand('SIE.Web.Warehouses.Commands.WarehouseAddressCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-blue" },
    _setCopyEntity: function (data) {
        var oldData = this.view.getCurrent().data;
        data.AddressType = oldData.AddressType;
    }
});