SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonTypePushObjectAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property != null) {
            var entity = e.entity;
            if (e.property == 'Type') {
                entity.setCode(null);
                entity.setName(null);
            }
        }
    }
});
