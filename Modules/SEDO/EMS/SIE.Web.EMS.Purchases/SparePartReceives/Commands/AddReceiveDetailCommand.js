SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.AddReceiveDetailCommand', {
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
        entity.setFactoryId(entity._SparePartReceive.getFactoryId());
        entity.setDepartmentId(entity._SparePartReceive.getDepartmentId());
        entity.setReceiveType(entity._SparePartReceive.getReceiveType());
        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;

        //ReceiveType 接收类型； 10 采购接收; 20 赠品接收;
        if (e.property === 'PurchaseOrderItemId' && e.value !== null
            && (e.entity.getReceiveType() === 10 || e.entity.getReceiveType() === 20)) {
            SIE.invokeDataQuery({
                method: 'GetSparePartInfo',
                params: [e.value],
                action: 'queryer',
                type: 'SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer',
                token: me.token,
                success: function (res) {
                    if (res.Result != null) {
                        var info = res.Result.data.items[0].data;
                        e.entity.setSparePartId_Display(info.SparePartCode);
                        e.entity.setSparePartId(info.Id);
                        e.entity.setSparePartName(info.SparePartName);
                        e.entity.setControlMethod(info.ControlMethod);
                        e.entity.setUnitName(info.UnitName);
                        e.entity.setExemptionInspect(info.ExemptionInspect);
                    }
                }
            });
        }

        if (e.property === 'Price' || e.property === 'TaxRate') {
            let price = e.entity.getPrice();
            let taxRate = e.entity.getTaxRate();
            let priceNoTax = price * (1 - (taxRate / 100)).toFixed(2);
            e.entity.setPriceNoTax(priceNoTax);
        }
    }
});