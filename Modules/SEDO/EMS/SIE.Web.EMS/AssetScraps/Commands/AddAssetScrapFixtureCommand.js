SIE.defineCommand('SIE.Web.EMS.AssetScraps.Commands.AddAssetScrapFixtureCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var formEntity = me.view.getParent().getCurrent();

        entity.setWarehouseId(formEntity.data.WarehouseId);
        entity.setIsFixAsset(formEntity.data.IsFixAsset);

        entity.setScrapWarehouseId_Display(formEntity.data.WarehouseId_Display);
        entity.setScrapWarehouseId(formEntity.data.WarehouseId);
        entity.setScrapLocationId_Display(formEntity.data.ScrapLocationCode);
        entity.setScrapLocationId(formEntity.data.ScrapLocationId);

        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property == 'ScrapNetValue') {

            var fixtureChildStore = me.view.getData();
            var amountList = fixtureChildStore.getData().items.where(function (p) { return !Ext.isEmpty(p.getScrapNetValue()); })
                .select(function (p) { return parseFloat(p.getScrapNetValue()); });
            var amount = amountList.sum();

            e.entity._AssetScrap.setAmount(amount);
        }

        if (e.property == 'FixtureEncodeId' || e.property == 'StorageLocationId' || e.property == 'FixtureQualityState') {

            var warehouseId = me.view.getParent().getCurrent().data.WarehouseId;
            if (e.entity.data.FixtureEncodeId != null && warehouseId != null) {
                SIE.invokeDataQuery({
                    method: 'GetCanUseNumByWarehouseId',
                    params: [warehouseId, e.entity.data.FixtureEncodeId, e.entity.data.StorageLocationId, e.entity.data.FixtureQualityState],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
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