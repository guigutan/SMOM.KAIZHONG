SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageCallMaterialAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加物料", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            var factoryId = me.view._parent.getData().getFactoryId();
            var wipId = me.view._parent.getData().getWipResourceId();
            var workShopId = me.view._parent.getData().getWorkShopId();
            var workOrId = me.view._parent.getData().getWorkOrderId();
            var processId = me.view._parent.getData().getProcessId();
            model.WipId = wipId;
            model.WorkShopId = workShopId;
            model.WorkOrderId = workOrId;
            model.ProcessId = processId;
            model.FactoryId = factoryId;
            this.view.execute({
                data: model,
                success: function (res) {
                    var data = res.Result;
                    entity.setWareHouseId_Display(data.WareHouseName);
                    entity.setWareHouseId(data.WareHouseId);
                    entity.setStorageLocationId_Display(data.LocationName);
                    entity.setStorageLocationId(data.StorageLocationId);
                    entity.setHand(data.Hand);
                }
            }, me.view);
        }
    }
});
