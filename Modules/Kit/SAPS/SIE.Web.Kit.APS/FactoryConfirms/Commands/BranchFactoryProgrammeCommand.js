SIE.defineCommand('SIE.Web.Kit.APS.FactoryConfirms.Commands.BranchFactoryProgrammeCommand', {
    meta: { text: "订单智能分厂", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var flag = true;
        if (view.getSelection().length > 0 && view.getCurrent() !== null) {
            Ext.each(view.getSelection(), function (item) {
                if (item.getLineState() != 0) {
                    flag = false;
                }
            });
        }
        return flag;
    },
    execute: function (view, source) {
        var count = 0;
        var selections = view.getSelection();
        if (selections != null && selections.length > 0) {
            for (var i = 0; i < selections.length; i++) {
                if (selections[i].getLineState() != 0)
                    count++;
            }
            if (count > 0) {
                SIE.Msg.showMessage("存在非新建状态销售订单!".t());
            } else {
                SIE.AutoUI.getMeta({
                    async: false,
                    ignoreCommands: false,
                    isDetail: false,
                    ignoreQuery: true,
                    token: view.token,
                    model: "SIE.Kit.APS.FactoryConfirms.BranchFactoryProgramme",
                    module: "SIE.Kit.APS.FactoryConfirms.FactoryConfirmsViewModel,SIE.Kit.APS",
                    callback: function (res) {
                        var detailView = SIE.AutoUI.generateAggtControl(res);
                        var listView = detailView._view;
                        var ui = detailView.getControl();
                        var win = SIE.Window.show({
                            title: "分配工厂规则".t(),
                            width: 600,
                            height: 600,
                            items: ui,
                            callback: function (btn) {
                                if (btn == "确定".t()) {
                                    SIE.Msg.wait("正在智能分厂，请稍等...".t());
                                    var branchProgramme = [];
                                    var detail = detailView._view.getCurrent().getEntityChildren().items[0].getData().items;
                                    Ext.each(detail, function (entity) { branchProgramme.push(entity.data) });
                                    var factoryConfirm = [];
                                    var sales = view.getSelection();
                                    Ext.each(sales, function (entity) { factoryConfirm.push(entity.data) });
                                    view.execute({
                                        data: { FactoryConfirm: factoryConfirm, BranchProgramme: branchProgramme },
                                        withIds: true,
                                        success: function (res) { //回调
                                            if (res.Success) {
                                                var results = res.Result;
                                                if (results == "") {
                                                    SIE.Msg.showMessage("请先设置库存组织!".t());
                                                }
                                                else {
                                                    Ext.each(view.getSelection(), function (item) {
                                                        Ext.each(results, function (result) {
                                                            if (result.DateID == item.data.Id) {
                                                                item.setEnterpriseId(null);
                                                                item.setEnterpriseId_Display(null);
                                                                item.setEnterpriseName(null);

                                                                item.setEnterpriseId_Display(result.EnterpriseCode);
                                                                item.setEnterpriseName(result.EnterpriseName);
                                                                item.setEnterpriseId(result.EnterpriseId);
                                                            }
                                                        })
                                                    })
                                                    SIE.Msg.showMessage("所选订单智能分厂完成，请确认分配结果后再提交".t());
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                        });

                        SIE.invokeDataQuery({
                            async: false,
                            type: "SIE.Web.Kit.APS.FactoryConfirms.DataQuery.BranchFactoryProgrammeDataQueryer",
                            method: 'ExpandBranchFactory',
                            token: view.token,
                            callback: function (res) {
                                if (res.Success) {
                                    listView.loadData(
                                        res.Result.getData()
                                    );
                                    var selected = res.Result.getData().items[0];
                                    listView.selectEntities(selected);
                                    listView.reloadData();
                                }
                                if (!res.Success) {
                                    win.close();
                                    SIE.Msg.showError(res.Message);
                                }
                            }
                        });
                    }

                });
            }
        }
        else {
            SIE.Msg.showMessage("请选择要分配的销售订单!".t());
        }

    }
})
