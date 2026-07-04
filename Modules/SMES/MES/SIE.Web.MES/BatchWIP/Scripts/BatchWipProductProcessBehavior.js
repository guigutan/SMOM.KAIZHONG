Ext.define("SIE.Web.MES.BatchWip.BatchWipProductProcessBehavior", {

    onViewReady: function (view) {
        var me = this;
        me.view = view;
        var grid = view.getControl();
        if (grid) {
            grid.mon(grid, 'rowdblclick', this.gridDlclick, me);
        }
    },

    gridDlclick: function () {
        var me = this;
        var view = me.view;
        var current = view.getCurrent();
        var title = me.getTitle(view);
        var id = 'menu_' + 'SIE.MES.BatchWIP.Products.BatchWipProductProcessDetail,SIE.MES'.replace(/[.|,]/g, '');
        var tabItem = CRT.Workbench.getTabById(id);
        if (tabItem) {
            if (current)
                CRT.Event.fire('batchDtlClick', id, title, current.getId());
            return;
        }
        CRT.Workbench.addPage({
            tabId: id,
            title: title,
            entityType: 'SIE.MES.BatchWIP.Products.BatchWipProductProcessDetail',
            params: {
                token: view.token,
                processId: current.getId()
            }
        });
    },

    getTitle: function (view) {
        if (view._parent && view._parent._current) {
            var batchNo = view._parent._current.getBatchNo();
            return Ext.String.format('批次{0}入站明细'.L10N(), batchNo);
        }
        else {
            return '批次入站明细'.L10N();
        }
    }
});