SIE.defineCommand('SIE.Web.EMS.AssetRequisitions.Commands.AddAssetRequisitionFixtureCommand', {
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
        entity.setWarehouseId(formEntity.getWarehouseId());

        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property == 'EstimatedAmount') {

            var fixtureChildStore = me.view.getData();
            var amountList = fixtureChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getEstimatedAmount()); })
                .select(function (p) { return parseFloat(p.getEstimatedAmount()); });
            var amount = amountList.sum();

            me.view.getParent().getData().setAmount(amount);
        }

        if (e.property == 'FixtureEncodeId') {

            var warehouseId = me.view.getParent().getCurrent().data.WarehouseId;
            if (e.entity.data.FixtureEncodeId != null && warehouseId != null) {
                SIE.invokeDataQuery({
                    method: 'GetCanUseNumByWarehouseId',
                    params: [warehouseId, e.entity.data.FixtureEncodeId],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetRequisitions.DataQueryer.AssetRequisitionDataQueryer',
                    token: me.view.token,
                    success: function (res) {
                        if (res.Success) {
                            e.entity.setStoreUsableQty(res.Result);
                        }
                    }
                });
            }
            else {
                e.entity.setStoreUsableQty(0);
            }
        }
    }

});