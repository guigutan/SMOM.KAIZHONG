Ext.define("SIE.Web.LES.MaterialReturnApplys.Scripts.MaterialReturnBehavior", {

    onViewReady: function (view) {
        var me = this;
        this.view = view;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            entity.setNo(params.No);
            entity.setReStatus(params.ReStatus);
            entity.setReType(params.ReType);
        }
    },

    onDataLoaded: function (view) {
        var me = this;
        // 选择工单车间或车间带出对应线边仓
        var entity = view.getCurrent();
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = e.entity;
        if (e.property == "ReType") {
            entity.setWoNo(null);
            entity.setWorkOrderId(null);

            entity.setWorkShopId_Display(null);
            entity.setWorkShopId(null);

            entity.setWipResourceId_Display(null);
            entity.setWipResourceId(null);

            entity.setWarehouseId_Display(null);
            entity.setWarehouseId(null);

            entity.setStorageLocationId_Display(null);
            entity.setStorageLocationId(null);

            var detailView = this.findChild('SIE.LES.MaterialReturnApplys.MaterialReturnApplyDetail');
            var detailViewStore = detailView.getData();
            detailViewStore.removeAll();
        }
        if (e.property == "WorkOrderId") {
            var detailView = this.findChild('SIE.LES.MaterialReturnApplys.MaterialReturnApplyDetail');
            var detailViewStore = detailView.getData();
            detailViewStore.removeAll();
        }
        if (e.property == "WorkShopId" && e.value != null) {
            var detailView = this.findChild('SIE.LES.MaterialReturnApplys.MaterialReturnApplyDetail');
            var detailViewStore = detailView.getData();
            detailViewStore.removeAll();
            var workShopId = entity.getWorkShopId();
            var wipResourceId = entity.getWipResourceId();
            SIE.invokeDataQuery({
                method: "GetLinesideWarehouse",
                params: [workShopId, wipResourceId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.LES.MaterialReturnApplys.MaterialReturnApplyDataQueryer",
                token: this.token,
                callback: function (res) {
                    if (res.Success) {
                        var data = res.Result;
                        entity.setWarehouseId_Display(data.WarehouseName);
                        entity.setWarehouseId(data.WarehouseId);
                        entity.setStorageLocationId_Display(data.StorageLocationName);
                        entity.setStorageLocationId(data.StorageLocationId);
                    }
                }
            })
        }
    }
});