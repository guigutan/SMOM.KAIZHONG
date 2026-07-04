SIE.defineCommand('SIE.Web.EMS.DevicePurs.Commands.AddDeviceTypeCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
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