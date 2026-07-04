Ext.define('SIE.Web.EMS.ViceTransfers.Scripts.FixtureDemandListBehavior',
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
            var me = this;
            var parent = me.getParent().getData().data;
            if ((e.property === 'FixtureEncodeId_Display' || e.property === "FixtureQualityState") && e.value !== null) {

                if (parent.WarehouseId == null || parent.WarehouseId == 0) {
                    SIE.Msg.showMessage("请先选择来源仓库!".t());
                    return;
                }
                if (e.entity.getFixtureEncodeId() == null || e.entity.getFixtureEncodeId() == 0) {
                    SIE.Msg.showMessage("请先选择工治具编码!".t());
                    return;
                }

                SIE.invokeDataQuery({
                    method: 'GetFixtureEncodeQty',
                    params: [e.entity.getFixtureEncodeId(), parent.WarehouseId, e.entity.getFixtureQualityState()],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                    token: me.token,
                    success: function (res) {
                        if (res.Result != null) {
                            var info = res.Result;
                            e.entity.setInStorageQty(info.Item1);
                            e.entity.setOnlineQty(info.Item2);
                        }
                    }
                });
            }
        }
    });