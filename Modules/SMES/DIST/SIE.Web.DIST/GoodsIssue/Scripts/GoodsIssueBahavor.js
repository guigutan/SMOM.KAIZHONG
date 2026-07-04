Ext.define('SIE.Web.DIST.GoodsIssueBahavor', {
    /**
     * view生命周期函数--view准备完成
     * @param {DetailView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        view.token = params.token;
        if (params.isEdit && params.isEdit === true) {
            me.initLoadevent(view);
        }
    },

    initLoadevent: function (targetView) {
        var me = this;
        var proView = targetView._children[0];
        var gridPanel = proView.getControl();
        proView.loadData();
        gridPanel.mon(proView.getData(), 'load', me.setProValue, proView);
    },
    setProValue: function (store, records, successful, operation, eOpts) {
        var me = this;
        var goodIssue = me._parent;
        SIE.invokeDataQuery({
            method: 'GetPropertyValueViewModel',
            params: [goodIssue.getData().data.Id],
            action: 'queryer',
            type: 'SIE.Web.DIST.GoodsIssueDataQueryer',
            token: goodIssue.token,
            success: function (res) {
                if (res.Result.data && res.Result.data.items.length > 0) {
                    res.Result.data.items.forEach(function (p) {
                        p.data.Values = p.data.Value.split(',');
                        p.data.DefinitionValueId = p.data.Value;
                        p.data.DefinitionId_Display = p.data.DefinitionName;
                    });
                    me.getData().add(res.Result.data.items);
                    me._parent.getData().markSaved();
                }
            }
        });
    }
});