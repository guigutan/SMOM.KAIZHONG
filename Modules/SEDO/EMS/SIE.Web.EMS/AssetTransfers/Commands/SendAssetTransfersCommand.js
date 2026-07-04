SIE.defineCommand('SIE.Web.EMS.AssetTransfers.Commands.SendAssetTransfersCommand', {
    meta: { text: "发货", group: "edit", iconCls: "icon-PaperPlane icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ApprovalStatus !== 40 || model.data.TransferStatus != 10) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        var me = this;
        var editEntity = this.view.getCurrent();
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            buttons:["确定".t(),"取消".t()],
            model: this.view.model,
            viewGroup: "UploadView",
            callback: function (meta) {
                meta.token = me.view.token;
                me.viewMeta = meta;
                var detailView = SIE.AutoUI.generateAggtControl(meta);
                me.detailView = detailView;

                var entity = editEntity;
                detailView._view._setDefaultValue(entity);
                detailView._view.setData(entity);
                var win = SIE.Window.show({
                    title: '发货'.t(),
                    width: '50%',
                    height: '60%',
                    items: detailView.getControl(),
                    callback: function (btn) {
                        var retFlag = false;
                        if (btn === "确定".t()) {
                            entity.data.Id = editEntity.data.Id;
                            view.execute({
                                data: entity.data.Id,
                                success: function (res) {
                                    retFlag = true;
                                    SIE.Msg.showMessage('发货完成'.t());
                                    view.reloadData();
                                    win.close();
                                },
                                error: function (res) {
                                    SIE.Msg.showError(res.Message);
                                }
                            });
                            if (retFlag)
                                return true;
                            else
                                return false;
                        } else {
                            win.close();
                        }
                    }
                });
            }
        });
    }
});