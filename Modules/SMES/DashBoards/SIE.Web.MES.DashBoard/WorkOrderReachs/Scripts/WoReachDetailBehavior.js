Ext.define('SIE.Web.MES.DashBoard.WoReachDetailBehavior', {
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        me.tabId = params.tabId;
        view.token = params.token;
        me.view = view;
        me.loadData(params.columnFieldValue, params.criData);
        me.registerEvent();
    },

    loadData: function (columnFieldValue, criData) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetWoReachDetail',
            params: [columnFieldValue, criData, { pageNumber: 1, pageSize: 25, isNeedCount: true }],
            action: 'queryer',
            type: 'SIE.Web.MES.DashBoard.WorkOrderReachs.WoReachDataQueryer',
            token: me.view.token,
            success: function (res) {
                if (res.Success === true) {
                    res.Result.data.items.forEach(function (item) { item.markSaved() });
                    me.view.setData(res.Result);
                }
            }
        });
    },

    registerEvent: function () {
        var me = this;
        CRT.Event.listen('woReachClick', function (columnFieldValue, criData, title) {
            var tab = CRT.Workbench.getTabById(me.tabId);
            if (tab) {
                CRT.Workbench.getTabPanel().setActiveItem(tab);
                tab.setTitle(title);
            }
            me.loadData(columnFieldValue, criData);
        });
    }
});