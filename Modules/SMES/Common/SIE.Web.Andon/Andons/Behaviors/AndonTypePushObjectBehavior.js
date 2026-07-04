Ext.define('SIE.Web.Andon.Andons.Behaviors.AndonTypePushObjectBehavior',{
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
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
