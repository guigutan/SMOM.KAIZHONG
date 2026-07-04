Ext.define('SIE.Web.MES.WorkOrders.WorkOrderUnionBarcodeBehavior', {

    /**
     * view生命周期函数--view准备完成
     * @param {DetailView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        view.token = params.token;
        view.workOrder = params.workOrder;
        var model = me.initModel(view);
        view.setData(model);
        model.children = view.getChildren();
        model.children.forEach(function (child) { child._pagingBar.removeAll() });
        me.initChildrenData(view, model);
        Ext.ComponentQuery.query('textfield[name=WoReworkSn]')[0].focus(true, 100);
    },

    initModel: function (view) {
        var workOrder = view.workOrder;
        var model = new SIE.Web.MES.WorkOrders.Reworks.WorkOrderUnionBarcode();
        model.setWorkOrderId(workOrder.Id);
        model.setWorkOrderNo(workOrder.No);
        model.setPlanQty(workOrder.PlanQty);
        model.setIsUseExist(workOrder.UseOldSn);
        model.ownerView = view;
        model.token = view.token;
        SIE.invokeDataQuery({
            method: 'GetUnionBarcodeCount',
            params: [workOrder.Id],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.Reworks.ReworkDataQueryer',
            token: view.token,
            success: function (res) {
                var data = res.Result.data;
                if (data.RelevancyQty == null || data.RelevancyQty == "")
                    data.RelevancyQty = 0;
                model.setRelevancyQty(data.RelevancyQty);
            }
        });
        return model;
    },
    initChildrenData: function (view, model) {
        SIE.invokeDataQuery({
            method: 'GetUnionBarcodeData',
            params: [model.getWorkOrderId(), model.getWorkOrderNo()],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.Reworks.ReworkDataQueryer',
            token: view.token,
            success: function (res) {
                
                var re = res.Result;
                var barcodeChild = view._children[0];
                if (barcodeChild) {
                    var barList = barcodeChild.getControl();
                    var store = barList.getStore();
                    store.setData(re.BarcodeList);
                    barList.setStore(store);
                    barList.store.data.each(function (p) { p.commit(); });
                    // barcodeChild.getData().data.items.forEach(function (item) { item.markSaved() });
                }
                var keyChild = view._children[1];
                if (keyChild) {
                    var keyList = keyChild.getControl();
                    var keyStore = keyList.getStore();
                    keyStore.setData(re.KeyItemList)
                    keyList.setStore(keyStore);
                    keyList.store.data.each(function (p) { p.commit(); });
                    //keyChild.getData().data.items.forEach(function (item) { item.markSaved() });
                }
                view.getData().setRelevancyQty(re.BarcodeList.length);
                view.getCurrent().markSaved();
                view.syncCmdState();    //刷新按钮状态
            },
            error: function (res) {
                SIE.Msg.showError(res.Message);
                return false;
            }
        });
    }
});