SIE.defineCommand('SIE.Web.EMS.AssetDisposals.Commands.AddAssetDisposalSparePartSnCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "新增序列号", group: "edit", iconCls: "icon-AddEntity icon-green" },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.AssetDisposals.AssetDisposalSparePart",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "AddAssetDisposalSparePartSnViewGroup",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                detailView.setData(entity);
                assetSnAddwin = SIE.Window.show({
                    title: "新增序列号".t(),
                    width: 680,
                    height: 280,
                    items: detailView.getControl(),
                    id: "AddAssetDisposalSparePartSn",
                    callback: function (btn) {
                        if (btn == "确定".t()) {

                            var formEntity = detailView.getData();

                            if (formEntity.data.SparePartId == null || formEntity.data.WarehouseId == null
                                || formEntity.data.Qty == null || formEntity.data.QualityStatus == null) {
                                SIE.Msg.showError('备件编码、质量状态、入库仓库均不能为空，且数量须大于0！'.t());
                                return false;
                            }

                            SIE.invokeDataQuery({
                                method: 'GetAddAssetDisposalSpareParts',
                                params: [formEntity.data],
                                async: false,
                                action: 'queryer',
                                type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
                                token: view.token,
                                success: function (ret) {

                                    if (ret.Success) {
                                        var store = view.getData();
                                        for (var i = 0; i < ret.Result.data.items.length; i++) {
                                            var record = ret.Result.data.items[i];

                                            record.setSparePartId_Display(formEntity.data.SparePartId_Display);
                                            record.setWarehouseId_Display(formEntity.data.WarehouseId_Display);
                                            store.add(record);

                                            view.mon(record, 'propertyChanged', me.onEntityPropertyChanged, view);
                                        }
                                    }
                                }
                            });
                        }
                    }
                });
                assetSnAddwin.view = view;
            }
        });
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var view = this;

        if (e.property == 'SparePartId') {

            setTimeout(function () {

                if (e.entity.data.SparePartId != null) {

                    if (e.entity.data.ControlMethod == 20) {

                        SIE.invokeDataQuery({
                            type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
                            method: "GetLotNo",
                            params: [],
                            async: false,
                            token: view.token,
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