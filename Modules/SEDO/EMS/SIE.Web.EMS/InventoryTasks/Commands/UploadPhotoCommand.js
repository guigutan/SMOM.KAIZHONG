SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.UploadPhotoCommand', {
    meta: { text: "上传图片", group: "edit", iconCls: "icon-Upload icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var entity = Ext.create("SIE.Web.EMS.Editors.ViewModels.ImportFileViewModel");
        var detailView = null;
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: view.token,
            module: view.module,
            model: "SIE.Web.EMS.Editors.ViewModels.ImportFileViewModel",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                detailView = SIE.AutoUI.createDetailView(mainBlock);
                detailView._setDefaultValue(entity);
                detailView.setData(entity);
                detailView.mainView = view;
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "文件".t(),
                    width: 450, height: 150,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            if (detailView.FileContent == undefined) {
                                win.close();
                                return false;
                            }
                            SIE.invokeDataQuery({
                                type: "SIE.Web.EMS.InventoryPlans.InventoryPlanDataQueryer",
                                method: "SaveInventoryPlanPhoto",
                                params: [detailView.FileContent, detailView.FileName, view.getCurrent().store.config.model, view.getCurrent().getId()],
                                async: false,
                                token: detailView.token,
                                callback: function (result) {
                                    if (result.Success) {
                                        if (result.Result != "") {
                                            view.getCurrent().setPhotoFilePath(result.Result);
                                            return false;
                                        }
                                    }
                                },
                            });
                        }
                        if (btn == "取消".t()) {
                            win.close();
                        }
                    }
                });
            },
        });
    }
});