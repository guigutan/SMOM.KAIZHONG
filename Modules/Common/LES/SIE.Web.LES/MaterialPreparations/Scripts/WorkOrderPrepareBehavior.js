Ext.define("SIE.Web.LES.MaterialPreparations.Scripts.WorkOrderPrepareBehavior", {

    onViewReady: function (view) {
        var me = this;
        this.view = view;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            entity.setNo(params.No);
            entity.setPrepareType(params.PrepareType);
        }
    },
    

    onDataLoaded: function (view) {
        var me = this;
        this.view = view;
        var entity = view.getData();
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property == 'WorkOrderId') {
            var detailView = e.entity.belongsView.findChild('SIE.LES.MaterialPreparations.MaterialPreparationDetail');
            var detailViewStore = detailView.getData();
            detailViewStore.removeAll();
            var woId = e.value;
            var preType = e.entity.getPrepareType();
            SIE.invokeDataQuery({
                method: "GetWorkOrderBomPrepration",
                params: [woId, preType],
                async: false,
                action: 'queryer',
                type: "SIE.Web.LES.MaterialPreparations.MaterialPreparationDataqueryer",
                token: this.token,
                success: function (res) {
                    if (res.Success) {
                        var details = res.Result.data.items;
                        if (details.length <= 0) {
                            if (preType == 0) {
                                SIE.Msg.showMessage("工单BOM可备用数为0".t());
                            }
                            else {
                                SIE.Msg.showMessage("工单BOM没有推式物料".t());
                            }
                            return;
                        }
                        details.forEach(function(detail) {
                            detail.setItemId_Display(detail.getItemCode());
                            detailViewStore.add(detail);
                        });
                    }
                }
            })
        }
        if (e.property == "WorkShopId" && e.value != null) {
            var entity = e.entity;
            var workShopId = entity.getWorkShopId();
            var wipResourceId = entity.getResourceId();
            SIE.invokeDataQuery({
                method: "GetLinesideWarehouse",
                params: [workShopId, wipResourceId],
                async: false,
                action: 'queryer',
                type: "SIE.Web.LES.MaterialPreparations.MaterialPreparationDataqueryer",
                token: this.token,
                callback: function (res) {
                    if (res.Success) {
                        var data = res.Result;
                        entity.setLineSideWarehouseId(data.WarehouseId);
                    }
                }
            })
        }
    }
});