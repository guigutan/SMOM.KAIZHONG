Ext.define('SIE.AbnormalInfo.AbnormalMonitors.Behaviors.PushTargetBehavior',{
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
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
