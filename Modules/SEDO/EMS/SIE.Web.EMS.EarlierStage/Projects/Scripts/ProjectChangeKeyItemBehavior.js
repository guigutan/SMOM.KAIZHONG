Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectChangeKeyItemBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        SIE.each(store, function (entity) {
            entity.setPlanType(entity._ProjectChange.getPlanType());
        });
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            if (e.property === 'BudgetAmount' || e.property === 'ProjectChangeId') {
                var qty = 0;
                me.getData().data.items.forEach(function (p) { qty += p.data.BudgetAmount; });
                var projectChange = me._parent.getCurrent();
                projectChange.setAmount(qty);
            }
        }
    }
});