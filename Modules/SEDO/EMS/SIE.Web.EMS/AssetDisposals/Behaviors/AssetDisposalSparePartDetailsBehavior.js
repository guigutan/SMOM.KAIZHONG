Ext.define('SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalSparePartDetailsBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var me = this;
            var store = view.getData();
            view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {

            if (e.property == 'SparePartId') {

                setTimeout(function () {

                    if (e.entity.data.SparePartId != null) {

                        if (e.entity.data.ControlMethod == 20) {

                            SIE.invokeDataQuery({
                                type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
                                method: "GetLotNo",
                                params: [],
                                async: false,
                                token: e.entity.belongsView.token,
                                success: function (res) {
                                    if (res.Success) {
                                        e.entity.setLotNo(res.Result);
                                    }
                                }
                            });
                        }
                        else {
                            e.entity.setLotNo("");
                        }
                    }

                    e.entity.setQty(e.entity.data.SparePartId != null && e.entity.data.ControlMethod == 30 ? 1 : null);

                }, 0);
            }
        }
    });