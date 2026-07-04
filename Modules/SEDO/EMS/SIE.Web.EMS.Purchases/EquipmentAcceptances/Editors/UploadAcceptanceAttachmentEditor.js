Ext.define('SIE.Web.EMS.Purchases.EquipmentAcceptances.Editors.UploadAcceptanceAttachmentEditor', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.uploadacceptanceattachmenteditor',
    triggerCls: 'ux-form-edit-trigger',
    border: false,
    //初始化
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    //触发器事件
    onTriggerClick: function (field, trigger, e) {
        var me = this;
        me._createLayout(field);
    },
    _createLayout: function (field) {
        var me = this;
        var form = me.up('form');

        var mainView = form.SIEView;

        var entity = Ext.create("SIE.Web.EMS.Editors.ViewModels.ImportFileViewModel");
        var detailView = null;
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: mainView.token,
            module: mainView.module,
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
                detailView.mainView = mainView;
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
                                type: "SIE.Web.EMS.Purchases.EquipmentAcceptances.DataQueryers.AcceptanceAttachmentDataQueryer",
                                method: "SaveEquipmentAcceptanceAttachment",
                                params: [detailView.FileContent, detailView.FileName],
                                async: false,
                                token: detailView.token,
                                callback: function (result) {
                                    if (result.Success) {
                                        if (result.Result != "") {
                                            detailView.mainView.getCurrent().setFilePath(result.Result);
                                            detailView.mainView.getCurrent().setFileExtesion(detailView.FileExtesion);
                                            detailView.mainView.getCurrent().setFileSize(detailView.FileSize);
                                            detailView.mainView.getCurrent().setFileName(detailView.FileName);
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
    },
});