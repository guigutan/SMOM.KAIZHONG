SIE.defineCommand('SIE.Web.Dock.YardZones.Commands.EditAddressCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        var me = this;
        if (entity) {
            this.mon(entity, 'propertyChanged', SIE.Web.CSM.AddressAction.onEntityPropertyChanged, this);
        }
    },
});