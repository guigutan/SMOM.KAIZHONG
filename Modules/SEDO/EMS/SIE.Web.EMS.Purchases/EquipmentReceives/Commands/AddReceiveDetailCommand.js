SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.AddReceiveDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var lineNo = me.view.getData().count();
        var poData = me.view.getData().getData();
        if (lineNo > 1) {
            var tempLineNoList = poData.items.where(function (p) { return p.getLineNo() != null; })
                .select(function (p) { return parseInt(p.getLineNo()); });
            lineNo = tempLineNoList.max() + 1;
        }
        entity.setLineNo(lineNo);
        entity.setFactoryId(entity._EquipmentReceive.getFactoryId());
        entity.setDepartmentId(entity._EquipmentReceive.getDepartmentId());
        entity.setReceiveType(entity._EquipmentReceive.getReceiveType());
        if (entity._EquipmentReceive.getReceiveType() === 20) {
            entity.setGiveaway(true);
            entity.setPrice(0);
        }
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'PurchaseOrderItemId' && e.value !== null && e.entity.getReceiveType() === 10) {
            SIE.invokeDataQuery({
                method: 'GetEquipModelInfo',
                params: [e.value],
                action: 'queryer',
                type: 'SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer',
                token: me.token,
                success: function (res) {
                    if (res.Result != null) {
                        var info = res.Result.data.items[0].data;
                        e.entity.setEquipModelId_Display(info.Code);
                        e.entity.setEquipModelId(info.Id);
                        e.entity.setEquipModelName(info.Name);
                        e.entity.setOrderEquipModelId(info.Id);
                    }
                }
            });
        }
        if (e.property === 'EquipModelId') {
            let old = e.entity.getOrderEquipModelId();
            if (old !== null && e.value !== null && old !== e.value) {
                e.entity.setGiveaway(true);
            }
        }
        if (e.property === 'Price' && e.value > 0) {
            if (e.entity.getGiveaway() === true)
                e.entity.setPrice(0);
        }
        if (e.property === 'Giveaway' && e.value === true) {
            e.entity.setPrice(0);
        }
    }
});