Ext.define("SIE.Web.LES.MaterialReturnApplys.Scripts.MaterialReturnDetailBehavior", {
    onDataLoaded: function (view) {
        var me = this;
        // 选择工单车间或车间带出对应线边仓
        var data = view.getData();
        view.mon(data, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = e.entity;
        if (e.property == "ReDetailQuality") {
            var isgood = e.value == 0; // 0-良品 1-不良品
            if (isgood) {
                var lessQty = entity.getAvailableQty() < entity.getLabelQty() ? entity.getAvailableQty() : entity.getLabelQty();
                entity.setReturnQty(lessQty);
            }
            else {
                var lessQty = entity.getNgQty() < entity.getLabelNgQty() ? entity.getNgQty() : entity.getLabelNgQty();
                entity.setReturnQty(lessQty);
            }
        }
    }
})