SIE.defineCommand('SIE.Web.Inventory.Piles.Commands.PilePrintCommand', {
    meta: { text: "打印", group: "edit", iconCls: "icon-PrintData icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getCurrent() == null || view.getSelection().length == 0) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        var me = this;
        var printTemplateId = 0;
        var printTemplateName = "";
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.Inventory.Common.DataQuery.PileDataQuery",
            method: 'GetPilePrintTemplate',
            token: me.view.token,
            callback: function (res) {
                if (res.Success) {
                    var data = res.Result;
                    if (data.getData() && data.getData().items.length == 1) {
                        printTemplateId = data.getData().items[0].data.Id;
                        printTemplateName = data.getData().items[0].data.FileName;
                    }
                }
                if (!res.Success) {
                    SIE.Msg.showError(res.Message);
                }
            }
        });
        me._showPrintViewModel(view, printTemplateId, printTemplateName);
    },
    _showPrintViewModel: function (view, printTemplateId, printTemplateName) {
        SIE.AutoUI.getMeta({
            model: 'SIE.Inventory.Piles.PrintPileViewModel',
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

                //获取默认模板
                SIE.Web.Core.CommonFuns.getPrintCache(entity, view.model);

                if (!(entity.getLabelTemplateId() > 0) && printTemplateId != null && printTemplateId > 0) {
                    entity.setLabelTemplateId(printTemplateId);
                    entity.setLabelTemplateId_Display(printTemplateName);
                }
                detailView.setData(entity);
                var ui = detailView.getControl();
                var winprint = SIE.Window.show({
                    title: "打印".t(),
                    width: 400,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var data = detailView.getCurrent().data;
                            if (data.LabelTemplateId == null || data.LabelTemplateId <= 0) {
                                SIE.Msg.showError("模板不能为空!".t());
                                return false;
                            }
                            //设置默认模板
                            SIE.Web.Core.CommonFuns.setPrintCache(entity.data, view.model);

                            SIE.invokeDataQuery({
                                async: false,
                                type: "SIE.Web.Inventory.Common.DataQuery.PileDataQuery",
                                method: 'PrintPileData',
                                token: view.token,
                                params: [view.getSelectionIds(), data.LabelTemplateId],
                                callback: function (r) {
                                    if (r.Success) {
                                        SIE.Web.Core.CommonFuns.ShowPrintPreview(r);
                                        view.loadData();
                                    }
                                    if (!r.Success) {
                                        SIE.Msg.showError(r.Message);
                                    }
                                }
                            });
                        }
                    }
                });
            }
        });
    }
});