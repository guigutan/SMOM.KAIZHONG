SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.AddReceiveDetailCommand', {
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
        entity.setFactoryId(entity._FixtureReceive.getFactoryId());
        entity.setDepartmentId(entity._FixtureReceive.getDepartmentId());
        entity.setReceiveType(entity._FixtureReceive.getReceiveType());
        if (entity._FixtureReceive.getReceiveType() === 20) {
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
        if (e.property === 'PurchaseOrderItemId') {
            if (e.entity.getReceiveType() === 10) {
                if (e.value !== null) {
                    SIE.invokeDataQuery({
                        method: 'GetFixtureInfo',
                        params: [e.value],
                        action: 'queryer',
                        type: 'SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer',
                        token: me.token,
                        success: function (res) {
                            if (res.Result != null) {
                                var info = res.Result;
                                e.entity.setFixtureEncodeId_Display(info.Code);
                                e.entity.setFixtureEncodeId(info.Id);
                                e.entity.setModelCode(info.ModelCode);
                                e.entity.setModelName(info.ModelName);
                                e.entity.setManageMode(info.ManageMode);
                                e.entity.setExemptionInspect(info.ExemptionInspect);
                                e.entity.setUnitId_Display(info.UnitName);
                                e.entity.setUnitId(info.UnitId);
                                if (info.Code != "")//能带出工治具编码 同时又是采购类型则设置FixtureEncodeId为禁止编辑
                                {
                                    e.entity.setFixtureEncodeIdReadOnly(true);
                                } else
                                    e.entity.setFixtureEncodeIdReadOnly(false);
                            }
                        }
                    });
                } else {
                    e.entity.setFixtureEncodeIdReadOnly(false);
                }
            }
        }
        if ((e.property === 'Price' || e.property === 'TaxRate' || e.property === 'PriceNoTax')&& e.entity.getReceiveType() === 20) {
            debugger;
            e.entity.setPrice("");
            e.entity.setTaxRate("");
            e.entity.setPriceNoTax("");
        }
    }
});