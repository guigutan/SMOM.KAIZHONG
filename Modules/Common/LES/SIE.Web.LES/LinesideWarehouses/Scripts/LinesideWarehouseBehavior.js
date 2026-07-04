Ext.define("SIE.Web.LES.LinesideWarehouses.Scripts.LinesideWarehouseBehavior", {
    onDataLoaded: function (view) {
        var me = this;
        this.view = view;
        var entity = view.getData();
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = e.entity;
        if (e.property == "WipResouceId") {
            SIE.invokeDataQuery({
                type: "SIE.Web.LES.LinesideWarehouses.LinesideWarehouseDataQueryer",
                method: "GetWipResourceInfo",
                params: [e.value],
                async: false,
                token: me.view.token,
                callback: function (res) {
                    if (res.Success) {
                        var info = res.Result;
                        if (info != null) {
                            entity.setFactoryId_Display(info.FactoryName);
                            entity.setFactoryId(info.FactoryId);
                            entity.setWorkShopId_Display(info.WorkShopName);
                            entity.setWorkShopId(info.WorkShopId);
                        }
                    }
                }
            })
        }
        else if (e.property == "WorkShopId" && e.entity.getWipResouceId() == null) {
            SIE.invokeDataQuery({
                type: "SIE.Web.LES.LinesideWarehouses.LinesideWarehouseDataQueryer",
                method: "GetWorkShopInfo",
                params: [e.value],
                async: false,
                token: me.view.token,
                callback: function (res) {
                    if (res.Success) {
                        var info = res.Result;
                        if (info != null) {
                            entity.setFactoryId_Display(info.FactoryName);
                            entity.setFactoryId(info.FactoryId);
                        }
                    }
                }
            })
        }
    }
});