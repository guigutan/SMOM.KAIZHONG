Ext.define("SIE.Web.LES.MaterialPreparations.Scripts.WorkOrderPrepareDetailBehavior", {
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getParent().getData();
        if (entity.getWorkOrderId() != null && entity.getCreateBy() != null) {
            me.upDateCanPrepareQty(me, view, entity.getWorkOrderId());
        }
    },
    // 刷新可备料数
    upDateCanPrepareQty: function (me, view, woId) {
        var detailViewStore = view.getData().data.items;
        var preType = view.getParent().getData().getPrepareType();
        SIE.invokeDataQuery({
            method: "GetWorkOrderBomPrepration",
            params: [woId, preType],
            async: false,
            action: 'queryer',
            type: "SIE.Web.LES.MaterialPreparations.MaterialPreparationDataqueryer",
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    var details = res.Result.data.items;
                    if (details.length <= 0) {
                        return;
                    }
                    details.forEach(function (detail) {
                        var data = detailViewStore.find(item => item.getLineNo() == detail.getLineNo() && item.getItemId() == detail.getItemId() && item.getItemExtProp() == detail.getItemExtProp());
                        if (data != null) {
                            data.setCanPrepareQty(detail.getCanPrepareQty());
                            data.markSaved();
                        }
                    });
                }
            }
        })
    },
})