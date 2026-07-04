SIE.defineCommand('SIE.Web.EMS.DevicePurs.Commands.EditDeviceTypeCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        var me = this;

        if (entity) {
            this.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var entity = e.entity;

        if (e.property.length > 0) {
            if (e.property == 'TypeCategory') {
                entity.setEquipType(null);
                entity.setEquipTypeName(null);
            }
        }
    }
});