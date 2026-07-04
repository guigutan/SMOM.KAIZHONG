Ext.define('SIE.Web.Inventory.LotLpnOnhandBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体实体元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {
            //code here
        },

        /**
         * view生命周期函数--view生成后
         * @param {*} view 生成的view
         */
        onCreated: function (view) {
            //code here
        },

        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var orderDetailDatas = view.getData().data.items;
            var ids = orderDetailDatas.select(function (p) { return p.data.Id; });
            if (ids.length == 0) return;
            if (view.getRelations().length > 0) {
                var criter = view.getRelations().first(function (p) { return p._target.isQueryView; });
                if (criter) {
                    if (criter._target.getData().data.IsWindow) {
                        var clearCommand = criter._target._commands.items.first(function (p) { return p.meta.command == "SIE.cmd.ClearCondition"; });
                        if (clearCommand) {
                            var Id = clearCommand.meta.id;
                            document.getElementById(Id).style.display = "none";
                        }
                    }
                    else {



                        var storeData = view.getControl().getStore().data;
                        var flag = storeData.items.any(function (p) { return p.data.Id == 0; });
                        var itemIds = storeData.items.select(function (p) { return p.data.ItemId; });

                        SIE.invokeDataQuery({
                            type: "SIE.Web.Items.Items.DataQuery.ItemDataQuery",
                            method: "GetDefaultItemUnits",
                            params: [itemIds],
                            async: true,
                            token: view.token,
                            callback: function (res) {
                                if (res.Success && res.Result) {
                                    var datas = res.Result.data.items;
                                    orderDetailDatas.where(function (p) { return p.data.Id > 0 }).forEach(function (p) {
                                        p.setConvertFigre(1);
                                        p.setSecondQty(p.data.Qty);

                                        var itemUnit = datas.first(function (a) {
                                            return a.data.ItemId == p.data.ItemId && a.data.UnitId == p.data.SecondUnitId
                                                || a.data.MainUnitId == p.data.ItemUnitId && a.data.UnitId == p.data.SecondUnitId && a.data.IsBaseUnit;
                                        });
                                        if (itemUnit) {
                                            var strConvertFigre = itemUnit.getNumerator() / itemUnit.getDenominator();
                                            var peQty = (p.data.Qty * (strConvertFigre)).toFixed(itemUnit.getSecondUnitPrecision());
                                            var tyQty = SIE.Web.Items.Scripts.ItemAction.getFormatValue(peQty, itemUnit.getSecondUnitPrecision(), itemUnit.getSecondTrade());                                           
                                            p.setSecondQty(tyQty);
                                            var pe = itemUnit.getMainUnitPrecision();
                                            if (itemUnit.getMainUnitPrecision() < itemUnit.getSecondUnitPrecision())
                                                pe = itemUnit.getSecondUnitPrecision();
                                            p.setConvertFigre(strConvertFigre.toFixed(pe));
                                        }
                                        else {
                                            p.setSecondUnitName(p.data.ItemUnit);
                                        }
                                        p.markSaved();
                                    });
                                }
                            }
                        });



                        if (storeData.items.length > 0 && !flag) {
                            var model = SIE.getModel(view.model);
                            var entity = new model();
                            entity.setItemCode("合计".t());
                            entity.setId(0);
                            var qty = storeData.items.sum(function (p) { return p.data.Qty; });
                            entity.setAvailableQty(Math.floor(qty * 1000) / 1000);
                            var availableQty = storeData.items.sum(function (p) { return p.data.AvailableQty; });
                            entity.setAvailableQty(Math.floor(availableQty * 1000) / 1000);
                            var allottedQty = storeData.items.sum(function (p) { return p.data.AllottedQty; });
                            entity.setAllottedQty(Math.floor(allottedQty * 1000) / 1000);
                            var freezingQty = storeData.items.sum(function (p) { return p.data.FreezingQty; });
                            entity.setFreezingQty(Math.floor(freezingQty * 1000) / 1000);
                            entity.setSecondQty(null);
                            entity.setConvertFigre(null);
                            storeData.add(entity);
                            entity.markSaved();
                        }
                    }
                }
            }
        }
    });