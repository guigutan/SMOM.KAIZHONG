SIE.defineCommand('SIE.Web.EMS.AssetTransfers.Commands.AddDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var parent = me.view.getParent().getData();
        if (parent) {
            entity.setSourceFactoryId(parent.getSourceFactoryId());
            entity.setIsAsset(parent.getIsAsset());
            entity.setManageDeptId(parent.getManageDeptId());
            entity.setUseDeptId(parent.getUseDeptId());
            entity.setParentTargetFactoryId(parent.getTargetFactoryId());
        }
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = me.getCurrent();
        if (e.property === 'EquipAccountId_Display' && e.value !== null) {
            if (me.getParent().getData().data.TransferType === 10) {
                SIE.invokeDataQuery({
                    method: 'GetEquipAccountInfoById',
                    params: [entity.data.EquipAccountId],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetTransfers.AssetTransferDataQueryer',
                    token: me.token,
                    success: function (res) {
                        if (res.Result != null) {
                            var info = res.Result;

                            e.entity.setResponsibleId_Display(info.OriginalResponsibleId_Display);
                            e.entity.setResponsibleId(info.OriginalResponsibleId) ;
                            e.entity.setWorkshopId_Display(info.OriginalWorkshopId_Display);
                            e.entity.setWorkshopId(info.OriginalWorkshopId);
                            
                            e.entity.setResourceId_Display(info.OriginalResourceId_Display);
                            e.entity.setResourceId(info.OriginalResourceId)
                            e.entity.setLocation(info.OriginalLocation);

                            e.entity.setWarehouseId_Display(info.OriginalWarehouseId_Display);
                            e.entity.setWarehouseId(info.OriginalWarehouseId);
                           
                            e.entity.setStorageLocationId_Display(info.OrinialStorageLocationId_Display);
                            e.entity.setStorageLocationId(info.OrinialStorageLocationId);
                            
                            e.entity.setKeeperId_Display(info.OrignalKeeperId_Display);
                            e.entity.setKeeperId(info.OrignalKeeperId);
                        }
                    }
                });
            }
        }
    }
});