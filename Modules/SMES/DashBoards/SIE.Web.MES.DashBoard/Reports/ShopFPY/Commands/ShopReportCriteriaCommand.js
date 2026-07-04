SIE.defineCommand('SIE.Web.MES.DashBoard.Reports.ProductFPY.Commands.ShopReportCriteriaCommand', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },
    /**
     * @property {Boolean}
     * 是否允许查询，反正恶意查询 
     */
    allow: true,

    /**
     * @property {Boolean}
     * 是否已经注册数据加载完成事件
     */
    register: false,
    canExecute: function (view, source) {
        var current = view.getCurrent();
        return this.allow && current != null;
    },

    /**
     * 判断查询方法能否执行
     * @param view 查询逻辑视图
     * @returns 能执行返回true，否则返回false
     */
    execute: function (view) {
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
                    layout.loadShopReportData(record.data, mainView.token);
                    me.allow = true;
                }
            }
        } catch (e) {
            me.allow = true;
            throw e;
        }
    }
});