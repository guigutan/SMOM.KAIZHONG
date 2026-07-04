Ext.define('SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionEquipmentDetailsBehavior',
{
    /**
     * view生命周期函数--数据加载后
     * @param {any} view
     */
    onDataLoaded: function (view) {
        var me = this;
        var equipChildStore = view.getData();
        var formEntity = view.getParent().getCurrent();
        view.mon(equipChildStore, 'propertyChanged', me.onEntityPropertyChanged, view);

        for (var i = 0; i < equipChildStore.getCount(); i++) {
            var equipRecord = equipChildStore.getAt(i);
            equipRecord.setFactoryId(formEntity.data.FactoryId);
            equipRecord.setLendingDepartmentId(formEntity.data.LendingDepartmentId);
        }
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {

        if (e.property == 'EstimatedAmount') {

            var equipChildStore = e.entity.store;
            var amountList = equipChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getEstimatedAmount()); })
                .select(function (p) { return parseFloat(p.getEstimatedAmount()); });
            var amount = amountList.sum();

            e.entity._AssetRequisition.setAmount(amount);
        }
    }
});