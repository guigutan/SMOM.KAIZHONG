SIE.defineCommand('SIE.Web.EMS.AssetDisposals.Commands.AddAssetDisposalSparePartCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;

        me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property == 'SparePartId') {

            setTimeout(function () {

                if (e.entity.data.SparePartId != null) {

                    if (e.entity.data.ControlMethod == 20) {

                        SIE.invokeDataQuery({
                            type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
                            method: "GetLotNo",
                            params: [],
                            async: false,
                            token: me.view.token,
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