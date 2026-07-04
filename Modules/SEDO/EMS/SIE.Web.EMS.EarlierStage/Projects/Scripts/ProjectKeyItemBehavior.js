Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectKeyItemBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        SIE.each(store, function (entity) {
            var plantype = view._parent.getCurrent().getPlanType();
            entity.setPlanType(plantype);
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
            if (e.property === 'BudgetAmount' || e.property === 'ProjectId') {
                var qty = 0;
                me.getData().data.items.forEach(function (p) { qty += p.data.BudgetAmount; });
                var project = me._parent.getCurrent();
                project.setAmount(qty);
            }
        }
    }
});