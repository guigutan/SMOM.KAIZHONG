Ext.define('SIE.Web.EMS.FixedAssets.Accounts.Scripts.FixedAssetDeviceBillBehavior', {
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

        if (e.property == 'IsMajor') {

            if (e.entity.data.IsMajor) {

                for (var i = 0; i < e.entity.store.getCount(); i++) {
                    var record = e.entity.store.getAt(i);

                    if (e.entity.data.EquipAccountCode != record.data.EquipAccountCode) {
                        record.setIsMajor(false);
                    }
                } 
           }
        }
   }
});