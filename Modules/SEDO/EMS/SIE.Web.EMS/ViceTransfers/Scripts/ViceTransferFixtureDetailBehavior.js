Ext.define('SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferFixtureDetailBehavior',
    {
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onDataLoaded: function (view) {
            var me = this;
            var entity = view.getData();
            debugger;
            if (entity)
                view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, view);

        },
        _onEntityPropertyChanged: function (e) {
            debugger;
            if (e.property.length > 0) {
                //管控方式为【编码】时 选择来源库位的时候获取 根据【工治具编码+质量状态+来源库位】获取合格数量或不合格数量
                if (e.entity.getManageMode() == 10 && e.property == "StorageLocationId_Display") {
                    SIE.invokeDataQuery({
                        method: 'GetFixtureStorageLocationQty',
                        params: [e.entity.getData()],
                        action: 'queryer',
                        type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                        token: this.token,
                        success: function (res) {
                            if (res.Result != null) {
                                e.entity.setSourceInventoryQty(res.Result);
                            }
                        }
                    });
                }
                if (e.property == "FixtureStatus") {
                    if (e.value == 10) {//在库
                        //在线时才查询，在库时为空，管控方式为【ID】时，为【1】,管控方式为【编码】时，获取工治具编码台账中该工治具编码的【在线】字段
                        e.entity.setOnlineQty(0);

                    } else {//在线

                        if (e.entity.getManageMode() == 5) {
                            e.entity.setOnlineQty(1);
                        }
                        if (e.entity.getManageMode() == 10)//获取工治具编码台账中该工治具编码的【在线】字段
                        {
                            SIE.invokeDataQuery({
                                method: 'GetFixtureEncodeOnlineQty',
                                params: [e.entity.getData()],
                                action: 'queryer',
                                type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                                token: this.token,
                                success: function (res) {
                                    if (res.Result != null) {
                                        e.entity.setOnlineQty(res.Result);
                                    }
                                }
                            });
                        }
                    }

                }



                if (e.property == "FixtureIDAccountId_Display" || e.property == "TransferQty") {
                    this.getControl().getSelectionModel().deselectAll();
                    for (var i = 0; i < this.getData().getData().items.length; i++) {
                        var selectItem = this.getData().getData().items[i];
                        if (selectItem.getFixtureIDAccountId() != null && selectItem.getFixtureIDAccountId() != "") {
                            this.getControl().getSelectionModel().select(i, true);
                        }
                        if (e.property == "TransferQty" && selectItem.getManageMode() == 10) {
                            if (selectItem.getTransferQty() > 0) {
                                this.getControl().getSelectionModel().select(i, true);
                            }
                        }
                    }


                    if (e.property == "FixtureIDAccountId_Display") {//选择完工治具ID后获取来源库位
                        SIE.invokeDataQuery({
                            method: 'GetFixtureIDAccountLocation',
                            params: [e.entity.getData()],
                            action: 'queryer',
                            type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                            token: this.token,
                            success: function (res) {
                                if (res.Result != null) {
                                    e.entity.setStorageLocationId_Display(res.Result.Item2);
                                    e.entity.setStorageLocationId(res.Result.Item1);
                                    e.entity.setWorkShop(res.Result.Item3);
                                    e.entity.setResoures(res.Result.Item4);
                                }
                            }
                        });
                    }

                }
                //修改时，清空序列号、来源库位字段
                if (e.property == "FixtureStatus") {
                    e.entity.setStorageLocationId(null);
                    e.entity.setStorageLocationId_Display("");
                    e.entity.setFixtureIDAccountId(null);
                    e.entity.setFixtureIDAccountId_Display("");

                }
            }
        }
    });