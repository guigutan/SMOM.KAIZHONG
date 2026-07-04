Ext.define("SIE.Web.MES.BatchWIP.Products.BatchWipProductProcessDtlBehavior", {
    outputView: null,
    onViewReady: function (view) {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        if (params)
            view.token = params.token;
        me.view = view;
        me.loadData(params.processId);
        me.registerEvent();
    },

    loadData: function (processId) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetBatchWipProductProcessInDetail',
            params: [processId, null],
            action: 'queryer',
            type: 'SIE.Web.MES.BatchWIP.Products.BatchWipDataQueryer',
            token: me.view.getToken(),
            success: function (res) {
                var control = me.view.getControl();
                if (control) {
                    control.setStore(null);
                    control.setStore(res.Result);
                    if (me.outputView) {
                        me.outputView.setData(null);
                    }
                    control.getStore().getData().items.forEach(function (item) {
                        item.markSaved()
                    });
                }
            }
        });
    },

    registerEvent: function () {
        var me = this;
        CRT.Event.listen('batchDtlClick', function (tabId, title, processId) {
            var tab = CRT.Workbench.getTabById(tabId);
            if (tab) {
                CRT.Workbench.getTabPanel().setActiveItem(tab);
                tab.setTitle(title);
            }
            me.loadData(processId);
        });
        var view = me.view;
        view.mon(view, 'currentChanged', this.currentChanged, this);
    },

    currentChanged: function () {
        var me = this;
        if (!me.outputView)
            me.getOutputView();
        if (me.outputView)
            me.outputView.loadData();
    },

    getOutputView: function () {
        var me = this;
        me.outputView = me.view._children.first(function (child) { return child.model === 'SIE.MES.BatchWIP.Products.BatchWipProductProcessDetail' });
    }
});