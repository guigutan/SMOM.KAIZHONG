Ext.define('SIE.Web.EMS.Purchases.EquipmentSetups.EquipmentSetupBehavior', {
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
                    method: 'GetNewEquipmentSetup',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.EquipmentSetups.EquipmentSetupDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setSetupNo(info.SetupNo);
                            entity.setSetupStatus(info.SetupStatus);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setPlanStartDate(info.PlanStartDate);
                            entity.setPlanEndDate(info.PlanEndDate);
                        }
                    }
                });
            }
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'OutSource') {
            let childView = me._children.first(function (p) { return p.model === "SIE.EMS.Purchases.EquipmentSetups.EquipmentSetupPlan"; });
            if (childView) {
                let store = childView.getData();
                SIE.each(store, function (entity) {
                    entity.setOutSource(e.value);
                    if (e.value === false) {
                        entity.setSupplierId(null);
                        entity.setSupplierName("");
                        entity.setPurchaseOrderId(null);
                        entity.setContactPerson("");
                        entity.setContactDetail("");
                    }
                });
                childView.setData(store);
            }
        }
    }
});