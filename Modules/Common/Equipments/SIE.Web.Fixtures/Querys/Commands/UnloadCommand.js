SIE.defineCommand('SIE.Web.Fixtures.Querys.Commands.UnloadCommand', {
    meta: { text: "出库", group: "edit", iconCls: "icon-WarehouseExport icon-blue" },
    /**
     * @override 是否执行
     * @param {} view 视图
     * @returns {}
     */
    canExecute: function (view) {
        var curEntity = view.getCurrent();
        if (curEntity !== null && curEntity.data.RepairBeforeState == 10)
            return true;
        return false;
    },

    /**
     * @override 执行
     * @param {} view 视图
     * @param {} source
     * @returns {}
     */
    execute: function (view, source) {
        var me = this;
        var curEntity = view.getCurrent();
        var curData = curEntity.getData();
        var criteriaData = view.getConditionView().getData().data;
        SIE.AutoUI.getMeta({
            model: "SIE.Fixtures.Querys.ViewModels.UnloadViewModel",
            module: 'SIE.Fixtures.Querys.ViewModels.FixtureQueryViewModel,SIE.Fixtures',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                me.detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = Ext.create("SIE.Fixtures.Querys.ViewModels.UnloadViewModel");
                entity.setResourceId(criteriaData.ResourceId);
                entity.setResourceId_Display(criteriaData.ResourceCode);
                entity.setWorkOrderId(criteriaData.WorkOrderId);
                entity.setWorkOrderId_Display(criteriaData.WorkOrderNo);
                entity.setFixtureAccountId(curData.FixtureAccountId);
                entity.setAccountCode(curData.AccountCode);
                entity.setEncodeCode(curData.EncodeCode);
                entity.setManageMode(curData.ManageMode);
                entity.setUnloadQty(curData.Qty);
                entity.setWarehouseId(curData.WarehouseId);
                entity.setLocationId(curData.LocationId);
                entity.setQty(curData.Qty);
                me.detailView.setData(entity);
                var ui = me.detailView.getControl();
                var win = SIE.Window.show({
                    title: "出库".t(),
                    width: '40%',
                    height: '25%',
                    layout: 'fit',
                    plain: true,
                    buttonAlign: 'right',
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var detailData = me.detailView.getCurrent().getData();
                            if (detailData.UnloadQty <= 0) {
                                SIE.Msg.showInstantMessage('出库数量不能小于等于0，请重新填写！'.t());
                                return false;
                            }

                            if (detailData.UnloadQty > detailData.Qty) {
                                SIE.Msg.showInstantMessage('出库数量不能大于在库总数，请重新填写！'.t());
                                return false;
                            }

                            var data = {};
                            data = detailData;

                            var indata = {};
                            indata.Data = Ext.encode(data);

                            view.execute({
                                data: indata,
                                success: function (res) {
                                    var errMsg = res.Result;
                                    if (errMsg !== '') {
                                        SIE.Msg.showError(errMsg);
                                        return;
                                    }
                                    else
                                        me.view.loadData();
                                },
                            });
                        }
                    }
                });
            }
        });
    },
});