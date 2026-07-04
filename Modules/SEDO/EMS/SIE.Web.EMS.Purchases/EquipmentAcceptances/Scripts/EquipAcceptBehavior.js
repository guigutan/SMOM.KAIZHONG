Ext.define('SIE.Web.EMS.Purchases.EquipmentAcceptances.EquipAcceptBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var fromEntity = view.getCurrent();
        fromEntity.setMessage("扫描设备编码/原厂序列号".t());
    }
});