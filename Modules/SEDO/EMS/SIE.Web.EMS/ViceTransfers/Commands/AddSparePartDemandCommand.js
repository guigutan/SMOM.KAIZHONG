SIE.defineCommand('SIE.Web.EMS.ViceTransfers.Commands.AddSparePartDemandCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var lineNo = me.view.getData().count();
        var poData = me.view.getData().getData();
        if (lineNo > 1) {
            var tempLineNoList = poData.items.where(function (p) { return (p.getLineNo() != null && p.getLineNo() != ""); })
                .select(function (p) { return parseInt(p.getLineNo()); });
            lineNo = tempLineNoList.max() + 1;
        }
        entity.setLineNo(lineNo);
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        var entity = me.getCurrent();
        var parent = me.getParent().getData().data;
        if (parent.ViceAssetObject == 10)//备件
        {
            if ((e.property === 'SparePartId_Display' || e.property === "QualityStatus") && e.value !== null) {

                if (parent.WarehouseId == null || parent.WarehouseId == 0) {
                    SIE.Msg.showMessage("请先选择来源仓库!".t());
                    return;
                }
                if (entity.getSparePartId() == null || entity.getSparePartId() == 0) {
                    SIE.Msg.showMessage("请先选择备件编码!".t());
                    return;
                }

                SIE.invokeDataQuery({
                    method: 'GetSparePartQty',
                    params: [entity.getSparePartId(), parent.WarehouseCode, entity.getQualityStatus()],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                    token: me.token,
                    success: function (res) {
                        if (res.Result != null) {
                            var info = res.Result;
                            entity.setWhInventory(info);
                        }
                    }
                });
            }

        }
    }
});