Ext.define('SIE.Web.EMS.Purchases.EquipmentSetups.SetupWorkHourBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        if (e.property === 'StartDateTime' || e.property === 'EndDateTime') {
            let start = e.entity.getStartDateTime();
            let end = e.entity.getEndDateTime();
            if (start != null && end != null) {
                let hours = (parseInt(end - start) / 1000 / 3600).toFixed(1);
                e.entity.setHours(hours);
            }
        }
    }
});