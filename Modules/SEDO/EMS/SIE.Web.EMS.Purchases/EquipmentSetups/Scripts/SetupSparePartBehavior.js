Ext.define('SIE.Web.EMS.Purchases.EquipmentSetups.SetupSparePartBehavior', {
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        SIE.each(store, function (entity) {
            entity.setLotInfoId(entity.getLotNo());
            entity.setLotInfoId_Display(entity.getLotNo());
            entity.setSnInfoId(entity.getSn());
            entity.setSnInfoId_Display(entity.getSn());
            entity.markSaved();
        });
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        if (e.property === 'PartOutDepotDetailId') {
            e.entity.setSparePartId(null);
            e.entity.setLotInfoId(null);
            e.entity.setSnInfoId(null);
            e.entity.setLotNo("");
            e.entity.setSn("");
            e.entity.setUseQty(null);
            e.entity.setSparePartName("");
            e.entity.setSpecification("");
            e.entity.setControlMethod(null);
            e.entity.setSurplusQty(null);
            e.entity.setUnitId(null);
        }
    }
});