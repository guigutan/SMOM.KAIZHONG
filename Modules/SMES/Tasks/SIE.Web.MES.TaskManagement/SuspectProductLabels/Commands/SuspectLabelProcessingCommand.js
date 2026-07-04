SIE.defineCommand("SIE.Web.MES.TaskManagement.SuspectProductLabels.Commands.SuspectLabelProcessingCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "可疑品处理", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view) {
        if (view && view.getCurrent()) {

            var entity = view.getCurrent();
            var me = this;

            SIE.AutoUI.getMeta({
                model: "SIE.MES.TaskManagement.SuspectProductLabels.ViewModels.ProcessingViewModel",
                module: "SIE.MES.TaskManagement.SuspectProductLabels.SuspectProductLabel,SIE.MES.TaskManagement",
                isDetail: true,
                ignoreQuery: true,
                isAggt: true,
                viewGroup: "ProcessingView",
                async: true,
                callback: function (res) {
                    //var mainBlock;
                    //if (res.mainBlock)
                    //    mainBlock = res.mainBlock;
                    //else
                    //    mainBlock = res;

                    //var detailView = SIE.AutoUI.createDetailView(mainBlock);
                    //var detailEntity = new detailView._model();
                    //detailEntity.setSuspectProductLabel(entity.getBatchNo());
                    //detailEntity.setSuspectProductLabelId(entity.getId());
                    //detailEntity.setQty(entity.getQty());
                    //detailEntity.setItemDesc(entity.getProductDesc());
                    //detailView._setDefaultValue(detailEntity);
                    //detailView.setData(detailEntity);
                    //var ui = detailView.getControl();

                    var ui = SIE.AutoUI.generateAggtControl(res);
                    var detailEntity = new ui._view._model();
                    detailEntity.setSuspectProductLabel(entity.getBatchNo());
                    detailEntity.setSuspectProductLabelId(entity.getId());
                    detailEntity.setQty(entity.getQty());
                    detailEntity.setItemDesc(entity.getProductDesc());
                    me._mainView = ui._view;
                    ui._view._setDefaultValue(detailEntity);
                    ui._view.setData(detailEntity);

                    var win = SIE.Window.show({
                        title: "可疑品处理".t(),
                        width: 800,
                        height: 500,
                        items: ui.getControl(),
                        buttons: ['确认', '关闭'],
                        callback: function (btn) {
                            if (btn == "确认".t()) {
                                me.view.execute({
                                    data: { viewModel: detailEntity.data, details: me._mainView.getChildren()[0].getData().data.items.select(p => p.data) },
                                    success: function (result) {
                                        if (result.Result != "")
                                        {
                                            SIE.Msg.showMessage(result.Result.t());
                                        }
                                        else {
                                            SIE.Msg.showMessage("处理完成".t());
                                            win.close();
                                        }
                                    }
                                });
                                return false;
                            }
                        }
                    });
                }
            });
        }
    },
});
