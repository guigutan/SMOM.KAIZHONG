Ext.define('SIE.Web.Andon.Andons.Behaviors.AndonManageListBehavior', {
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getData();
        if (view.getData().last().getAndonManageCode() == '合计') {
            view.getData().last().setFaultTime(null);
            view.getData().last().setTriggerTime(null);
            view.getData().last().setUpdateDate(null);
            view.getData().last().setCreateDate(null);
            view.getData().last().dirty = false;
        }
    },
});