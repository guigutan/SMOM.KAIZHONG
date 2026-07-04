SIE.defineCommand('SIE.Web.EMS.AssetRequisitions.Commands.AddAssetRequisitionEquipmentCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var formEntity = me.view.getParent().getCurrent();
        var lineNo = me.view.getData().getCount();
        var poData = me.view.getData().getData();
        if (lineNo > 1) {
            var tempLineNoList = poData.items.where(function (p) { return !Ext.isEmpty(p.getLineNo()); })
                .select(function (p) { return parseInt(p.getLineNo()); });
            lineNo = tempLineNoList.max() + 1;
        }
        entity.setLineNo(lineNo);
        entity.setFactoryId(formEntity.getFactoryId());
        entity.setLendingDepartmentId(formEntity.getLendingDepartmentId());
        entity.setAssetRequisitionWarehouseId(formEntity.getWarehouseId());

        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property == 'EstimatedAmount') {

            var equipChildStore = me.view.getData();
            var amountList = equipChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getEstimatedAmount()); })
                .select(function (p) { return parseFloat(p.getEstimatedAmount()); });
            var amount = amountList.sum();

            e.entity._AssetRequisition.setAmount(amount);
        }
    }

});