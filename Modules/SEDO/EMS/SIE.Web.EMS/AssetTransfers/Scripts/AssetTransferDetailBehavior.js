Ext.define('SIE.Web.EMS.AssetTransfers.AssetTransferDetailBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onDataLoaded: function (view) {
        var me = this;
        var entity = view.getCurrent();
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        //var parent=
        if (e.property === 'EquipAccountId') {
            if (e.value == "10") {//工厂内转移
                e.entity.setTargetFactoryId(e.entity.data.SourceFactoryId);
                e.entity.setTargetManageDeptId(e.entity.data.ManageDeptId);
                e.entity.setTargetUseDepartId(e.entity.data.UseDeptId);

                e.entity.setTargetFactoryId_Display(e.entity.data.SourceFactoryId_Display);
                e.entity.setTargetManageDeptId_Display(e.entity.data.ManageDeptId_Display);
                e.entity.setTargetUseDepartId_Display(e.entity.data.UseDeptId_Display);
            }
        }
        

    }
});