Ext.define('SIE.Web.EMS.InventoryPlans.InventoryPlanEquipBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var parent = view._parent.getCurrent();
        var entity = view.getCurrent();
        entity.setFactoryId(parent.data.FactoryId);
    }
});