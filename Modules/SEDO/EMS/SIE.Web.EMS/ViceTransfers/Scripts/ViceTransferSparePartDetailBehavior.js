Ext.define('SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferSparePartDetailBehavior',
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
                if (e.property == "StoreSummaryLotId_Display" || e.property == "StoreSummaryDetailId_Display" || e.property == "TransferQty") {
                    this.getControl().getSelectionModel().deselectAll();
                    for (var i = 0; i < this.getData().getData().items.length; i++) {
                        var selectItem = this.getData().getData().items[i];
                        if (selectItem.getStoreSummaryLotId() != null && selectItem.getStoreSummaryLotId() != "") {
                            this.getControl().getSelectionModel().select(i, true);
                        }
                        if (selectItem.getStoreSummaryDetailId() != null && selectItem.getStoreSummaryDetailId() != "") {
                            this.getControl().getSelectionModel().select(i, true);
                        }
                        if (e.property == "TransferQty" && selectItem.getControlMethod() != 30) {
                            if (selectItem.getTransferQty() > 0) {
                                this.getControl().getSelectionModel().select(i, true);
                            }
                        }
                    }
                }
                //管控方式为【批次】时，根据【批次号+质量状态+来源库位】获取可用库存或不良品数  
                if (e.property == "TransferQty" ||e.property == "StoreSummaryLotId_Display" || e.property == "StorageLocationId" || e.property =="StoreSummaryDetailId_Display")///管控方式为【批次】时
                {
                    SIE.invokeDataQuery({
                        method: 'GetWarehouseLotQty',
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
            }
        }
    });