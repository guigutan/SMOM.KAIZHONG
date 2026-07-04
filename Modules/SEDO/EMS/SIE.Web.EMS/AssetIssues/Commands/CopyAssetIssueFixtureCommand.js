SIE.defineCommand('SIE.Web.EMS.AssetIssues.Commands.CopyAssetIssueFixtureCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && view.getSelection()[0].data.ManageMode == 10;
    },
    execute: function (view, source) {

        var me = this;
        var entity = view.getCurrent();
        var store = view.getData();
        var copyEntity = view.copyEntity(entity);  

        copyEntity.data.Qty = 0;
        copyEntity.data.QualityStatus = null;
        copyEntity.data.StorageLocationId = null;
        copyEntity.data.CreateBy = 0;
        copyEntity.data.CreateDate = null;
        copyEntity.data.UpdateBy = 0;
        copyEntity.data.UpdateDate = null;

        store.insert(store.indexOf(entity) + 1, copyEntity);

        view.mon(copyEntity, 'propertyChanged', me.onEntityPropertyChanged, view);

        var warehouseId = view.getParent().getCurrent().data.WarehouseId;
        SIE.invokeDataQuery({
            method: 'GetCanUseNumByWarehouseId',
            params: [warehouseId, copyEntity.data.FixtureEncodeId, copyEntity.data.StorageLocationId, copyEntity.data.QualityStatus],
            async: false,
            action: 'queryer',
            type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Success) {
                    copyEntity.setStoreUsableQty(res.Result);
                }
            }
        });
    },
    onEntityPropertyChanged: function (e) {
        var view = this;

        if (e.property == 'Qty') {

            if (e.entity.data.Qty > 0) {

                view.getControl().getSelectionModel().select(e.entity, true);
            }
            else {
                view.getControl().getSelectionModel().deselect(e.entity);
            }
        }

        if (e.property == 'FixtureEncodeId' || e.property == 'StorageLocationId' || e.property == 'QualityStatus') {

            var warehouseId = view.getParent().getCurrent().data.WarehouseId;
            if (e.entity.data.FixtureEncodeId != null && warehouseId != null) {
                SIE.invokeDataQuery({
                    method: 'GetCanUseNumByWarehouseId',
                    params: [warehouseId, e.entity.data.FixtureEncodeId, e.entity.data.StorageLocationId, e.entity.data.QualityStatus],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                    token: view.token,
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