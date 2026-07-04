SIE.defineCommand('SIE.Web.Andon.AndonMonthReports.Commands.AndonMonthReportQuery', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },
    _view: null,

    /**
     * 判断查询方法能否执行
     * @param view 查询逻辑视图
     * @returns 能执行返回true，否则返回false
     */
    execute: function (view) {
        Ext.getBody().mask("数据加载中，请稍等...".t());
        var me = this;
        try {
            me.allow = false;
            var record = view.getCurrent();
            delete record.data['CriteriaModuleKey'];
            delete record.data['CriteriaType'];
            delete record.data["CriteriaString"];
            var istrue = true;
            view.getControl().items.items.forEach(function (item) {
                if (!item.validate()) {
                    istrue = false;
                }
            });
            var mainView = view.getResultView();
            if (mainView) {
                var layout = me.view.mainLayout;
                if (layout) {
                    layout.loadReportData(record.data, mainView.token, layout);
                    me.allow = true;
                    
                }
            }
        } catch (e) {
            me.allow = true;
            throw e;
        }
    }
});