Ext.define('SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferSparePartBehavior',
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
            var me = this;
            if (e.property.length > 0) {
                if ((e.property === 'SparePartId_Display' || e.property === "QualityStatus") && e.value !== null) {
                    var parent = me.getParent().getData().data;
                    if (parent.WarehouseId == null || parent.WarehouseId == 0) {
                        SIE.Msg.showMessage("请先选择来源仓库!".t());
                        return;
                    }
                    if (e.entity.getSparePartId() == null || e.entity.getSparePartId() == 0) {
                        SIE.Msg.showMessage("请先选择备件编码!".t());
                        return;
                    }

                    SIE.invokeDataQuery({
                        method: 'GetSparePartQty',
                        params: [e.entity.getSparePartId(), parent.WarehouseCode, e.entity.getQualityStatus()],
                        action: 'queryer',
                        type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                        token: me.token,
                        success: function (res) {
                            if (res.Result != null) {
                                var info = res.Result;
                                e.entity.setWhInventory(info);
                            }
                        }
                    });
                }
            }
        }
    });