Ext.define('SIE.Web.Warehouses.ViewBehaviors.StorageLocationInfoBehavior', {
    onDataLoaded: function (view) {
        var me = this;
        var p = view.getParent();
        if (p) {
            var cur = view.getCurrent();
            if (cur.getForm() != "") {
                if (cur.associateView.formConfig.items.length > 0) {
                    if (cur.associateView.formConfig.items[7] != null) {
                        var store = cur.associateView.formConfig.items[7].store;
                        if (store.data != null) {
                            var arrData = eval(store.data);
                            arrData.forEach(function (p) {
                                if (p.Code == cur.getForm()) {
                                    cur.data.Form_Display = p.Name;
                                }
                            });
                        }
                    }
                }
            } else {
                cur.data.Form_Display = "";
            }

            cur.markSaved();
        }
    }
});

//SIE:classEnd
Ext.define('SIE.Web.Warehouses.ViewBehaviors.StorageLocationLayinInfoBehavior', {
    onDataLoaded: function (view) {
        var me = this;
        var p = view.getParent();
        if (p) {
            var cur = view.getCurrent();
            if (cur.getRoHsGradeValue() != "") {
                if (cur.associateView.formConfig.items.length > 0) {
                    if (cur.associateView.formConfig.items[6] != null) {
                        var store = cur.associateView.formConfig.items[6].store;
                        if (store.data != null) {
                            var arrData = eval(store.data);
                            arrData.forEach(function(p) {
                                if (p.Code == cur.getRoHsGradeValue()) {
                                    cur.data.RoHsGradeValue_Display = p.Name;
                                }
                            });
                        }
                    }
                }
            } else {
                cur.data.RoHsGradeValue_Display = "";
            }

            cur.markSaved();
        }
    }
});