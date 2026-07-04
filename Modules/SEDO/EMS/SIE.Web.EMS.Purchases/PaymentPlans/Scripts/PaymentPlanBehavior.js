Ext.define('SIE.Web.EMS.Purchases.PaymentPlans.PaymentPlanBehavior', {
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
                    method: 'GetNewPaymentPlan',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.PaymentPlans.PaymentPlanDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setPaymentOrderNo(info.PaymentOrderNo);
                            entity.setApprovalStatus(info.ApprovalStatus);
                        }
                    }
                });
            }
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'PurchaseOrderId') {
            if (e.value === null) {
                e.entity.setFactoryId(null);
                e.entity.setDepartmentId(null);
                e.entity.setSupplierId(null);
                e.entity.setSupplierName("");
                e.entity.setPaymentTermsId(null);
                e.entity.setPaymentDate(null);
                e.entity.setAmount(null);
            } else {
                SIE.invokeDataQuery({
                    method: 'GetPurOrderInfo',
                    params: [e.value],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Purchases.PaymentPlans.PaymentPlanDataQueryer',
                    token: me.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            e.entity.setFactoryId(info.FactoryId);
                            e.entity.setFactoryId_Display(info.FactoryId_Display);
                            e.entity.setDepartmentId(info.DepartmentId);
                            e.entity.setDepartmentId_Display(info.DepartmentId_Display);
                            e.entity.setSupplierId(info.SupplierId);
                            e.entity.setSupplierId_Display(info.SupplierId_Display);
                            e.entity.setSupplierName(info.SupplierName);
                            e.entity.setPaymentDate(null);
                            e.entity.setAmount(null);
                        }
                    }
                });
            }
        }
    }
});