Ext.define('SIE.Web.EMS.Purchases.PurchaseOrders.HistoryOrderBehavior', {
    /**
    * view聚合后
    * @param {*} view 生成的view
    */
    onViewReady: function (view) {
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            entity.setPurchaseObjectType(params.PurchaseObjectType);
            entity.setObjectCodeInfo(params.OldObjectCode);
            entity.setObjectName(params.OldObjectName);
            entity.setSupplierId_Display(params.OldSupplierIdDisplay);
            entity.setSupplierId(params.OldSupplierId);
            entity.setSupplierName(params.OldSupplierName);
            var time = new Date(new Date().getFullYear() - 3, 0, 1);
            entity.data.CreateDate = { BeginValue: time, EndValue: new Date() };
            view.tryExecuteQuery();
            entity.setObjectCodeInfo(params.OldObjectCode);
            entity.setSupplierId_Display(params.OldSupplierIdDisplay);
            entity.setSupplierId(params.OldSupplierId);
        }
    }
});