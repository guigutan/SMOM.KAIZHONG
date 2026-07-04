SIE.defineCommand('SIE.Web.Inventory.Piles.Commands.BatchGeneratePileCommand', {
    meta: { text: "批量生成", group: "edit", iconCls: "icon-Barcode icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var isHasConfig = true;
        var printTemplateId = 0;
        var printTemplateName = "";
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.Inventory.Common.DataQuery.PileDataQuery",
            method: 'GetPileConfig',
            token: me.view.token,
            callback: function (res) {
                if (res.Success) {
                    var data = res.Result;
                    if (data && data.PrintTemplate) {
                        printTemplateId = data.PrintTemplate.Id;
                        printTemplateName = data.PrintTemplate.FileName;
                    }
                }
                if (!res.Success) {
                    SIE.Msg.showError(res.Message);
                    isHasConfig = false;
                }
            }
        });

        if (isHasConfig) {
            me._showBatchGenerateView(view, printTemplateId, printTemplateName);
        }
    },
    _showBatchGenerateView: function (view, printTemplateId, printTemplateName) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.Inventory.Piles.BatchGeneratePileViewModel",
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                if (printTemplateId != null && printTemplateId > 0) {
                    entity.setTemplateId(printTemplateId);
                    entity.setTemplateId_Display(printTemplateName);
                }
                detailView.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "批量生成".t(),
                    width: 420,
                    height: 250,
                    items: ui,
                    buttons: ['生成并打印'.t(), '生成'.t(), '取消'.t()],
                    callback: function (btn) {
                        var indata = detailView.getCurrent().data;
                        var signdata = {
                            command: me.meta.command,
                            entityType: me.view.model,
                            parentType: ""
                        }
                        if (btn == "生成".t()) {
                            SIE.Msg.wait("生成中,请稍等......".t());

                            if (!me._validateInputData(indata)) {
                                return false;
                            }

                            //生成垛码逻辑
                            SIE.invokeDataQuery({
                                async: false,
                                type: "SIE.Web.Inventory.Common.DataQuery.PileDataQuery",
                                method: 'BatchGeneratePileData',
                                token: me.view.token,
                                params: [indata],
                                logInfo: signdata,
                                callback: function (r) {
                                    if (r.Success) {
                                        SIE.Msg.showInstantMessage("生成完毕".t(), "提示".t(), 3);
                                        win.close();
                                        view.reloadData();
                                    }
                                    if (!r.Success) {
                                        SIE.Msg.showError(r.Message);
                                        return false;
                                    }
                                }
                            });
                        }

                        if (btn == "生成并打印".t()) {
                            if (!me._validateInputData(indata)) {
                                return false;
                            }

                            if (indata.TemplateId == null || indata.TemplateId <= 0) {
                                SIE.Msg.showError("未选择打印模板!".t());
                                return false;
                            }

                            SIE.invokeDataQuery({
                                async: false,
                                type: "SIE.Web.Inventory.Common.DataQuery.PileDataQuery",
                                method: 'GenerateAndPrintPileData',
                                token: me.view.token,
                                logInfo: signdata,
                                params: [indata],
                                callback: function (r) {
                                    if (r.Success) {
                                        SIE.Web.Core.CommonFuns.ShowPrintPreview(r);
                                        win.close();
                                        view.reloadData();
                                    }
                                    if (!r.Success) {
                                        SIE.Msg.showError(r.Message);
                                        return false;
                                    }
                                }
                            });
                        }
                    }
                });
            },
        });
    },
    _validateInputData: function (indata) {
        if (indata.TurnoverBoxModelId == null || indata.TurnoverBoxModelId <= 0) {
            SIE.Msg.showError("型号必须填写，请重新输入!".t());
            return false;
        }

        if (indata.GenerateQty <= 0) {
            SIE.Msg.showError("数量输入有误，请重新输入!".t());
            return false;
        }

        return true;
    }
});