Ext.define('SIE.Web.Fixtures.MaintainTasks.MaintainTaskBehavior', {
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getCurrent();
        entity.setPassQty(entity.getQty());
        entity.setNgQty(0);
        view.mon(view._current, 'propertyChanged', me._onMaintainTaskChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    _onMaintainTaskChanged: function (e) {
        var me = this;
        var entity = e.entity;
        if ((e.property === "PassQty" || e.property === "NgQty") && e.value != null) {
            var details = entity._Details.data.items;
            Ext.each(details, function (detail) {
                var result = detail.getMaintainResult();
                if (result == null) {
                    SIE.Msg.showMessage("【保养项目】中所有【项目保养结论】都给出后才能维护【合格数量】和【不合格数量】!".t());
                    return;
                }
            });
            var qty = entity.getQty();
            if (entity.getPassQty() > qty) {
                SIE.Msg.showMessage("【合格数量】不能大于【治具数量】!".t());
                return;
            }
            if (entity.getNgQty() > qty) {
                SIE.Msg.showMessage("【不合格数量】不能大于【治具数量】!".t());
                return;
            }
            if (e.property === "PassQty" && e.value != null) {
                entity.setNgQty(qty - entity.getPassQty());
            }
            if (e.property === "NgQty" && e.value != null) {
                entity.setPassQty(qty - entity.getNgQty());
            }
        }
    }
});