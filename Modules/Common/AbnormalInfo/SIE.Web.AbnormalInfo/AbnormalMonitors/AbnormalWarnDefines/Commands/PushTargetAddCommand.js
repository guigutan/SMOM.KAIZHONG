SIE.defineCommand('SIE.AbnormalInfo.AbnormalMonitors.Commands.PushTargetAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property != null) {
            var entity = e.entity;
            if (e.property == 'TargetType') {
                entity.setTargetCode(null);
                entity.setTargetName(null);
            }
        }
    }
});
