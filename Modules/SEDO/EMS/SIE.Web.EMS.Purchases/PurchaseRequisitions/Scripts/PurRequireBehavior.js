Ext.define('SIE.Web.EMS.Purchases.PurchaseRequisitions.PurRequireBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.action = params.action;
            if (params.action === 0) {
                SIE.invokeDataQuery({
                    method: 'GetNewPurchaseRequisition',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.PurchaseRequisitions.PurRequireDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setNo(info.No);
                            entity.setCurrency(info.Currency);
                            entity.setPurchaseObjectType(info.PurchaseObjectType);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setPurchaseType(info.PurchaseType);
                        }
                    }
                });
            }
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'PurchaseType' || e.property === 'ProjectId') {
            if (e.property === 'PurchaseType')
                e.entity.setProjectId(null);
            if (e.property === 'ProjectId' && e.value == null)
                e.entity.setProjectName("");
            var childView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.PurchaseRequisitions.PurchaseRequisitionItem"; });
            if (childView) {
                var store = childView.getData();
                store.removeAll();
                childView.setData(store);
            }
        }
        if (e.property === 'Currency') {
            e.entity.setAmountUnit(e.value);
        }
        if (e.property === 'PurchaseObjectType') {
            let payView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.PurchaseRequisitions.PurchaseRequisitionItem"; });
            if (payView) {
                let payStore = payView.getData();
                SIE.each(payStore, function (entity) {
                    entity.setPurchaseObjectType(e.value);
                    entity.setObjectCodeInfoId(null);
                    entity.setObjectCodeInfoId_Display(null);
                    entity.setObjectCode(null);
                    entity.setSpecification(null);
                    entity.setDescription(null);
                    entity.setItemUnitId(null);
                });
                payView.setData(payStore);
            }
        }
    }
});