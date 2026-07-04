SIE.defineCommand('SIE.Web.EMS.AssetDisposals.Commands.AddAssetDisposalEquipmentCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var formEntity = me.view.getParent().getCurrent();

        entity.setFactoryId(formEntity.getFactoryId());
        entity.setManageDeptId(formEntity.getManageDeptId());
        entity.setUseDeptId(formEntity.getUseDeptId());
        entity.setWarehouseId(formEntity.getWarehouseId());
        entity.setIsFixAsset(formEntity.getIsFixAsset());

        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property == 'NetValue') {

            var equipChildStore = me.view.getData();
            var amountList = equipChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getNetValue()); })
                .select(function (p) { return parseFloat(p.getNetValue()); });
            var amount = amountList.sum();

            e.entity._AssetDisposal.setAmount(amount);
        }
    }

});