  Ext.define('SIE.Web.ESop.Documents.Scripts.DocFileUploadContextEditor', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.docfileUploadContextEditor',
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

        var form, context, current, data, className, ConfigValue, ModuleType;
        form = me.up('form');
        if (form) {
            var view = form.SIEView;
            if (view) {
                current = view.getData();
                data = current.getData();
            } else {
                context = me.up('container').context;
                current = context.view.grid.SIEView.getCurrent();
            }
        } else {
            context = me.up('container').context;
            current = context.view.grid.SIEView.getCurrent();
        }

        var me = this;
        var mainView = context.view.grid.SIEView;//form.SIEView;
        var entity = Ext.create("SIE.ESop.Documents.ViewModels.ImportFileViewModel");
        var detailView = null;
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: mainView.token,
            module: mainView.module,
            model: "SIE.ESop.Documents.ViewModels.ImportFileViewModel",
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
                ui.items.items[0].fieldLabel = ""//隐藏*，该*本身的label就为空字符。此处*无意义
                var win = SIE.Window.show({
                    title: "文件".t(),
                    width: 450, height: 150,
                    items: ui,
                    callback: function (btn) {

                        if (btn == "确定".t()) {
                            if (detailView.FileContent == undefined) {
                                win.close(detailView.mainView.getCurrent());
                                return false;
                            }

                            SIE.invokeDataQuery({
                                type: "SIE.Web.ESop.Documents.DataQuerys.DocumentsDataQuery",
                                method: "SaveListAttachment",
                                params: [detailView.FileContent, detailView.FileName],
                                async: false,
                                token: detailView.token,
                                callback: function (result) {
                                    if (result.Success) {
                                        debugger;
                                        if (result.Result != "") {

                                            console.log(result.Result);
                                            var current = detailView.mainView.getCurrent();
                                            current.setFileName(result.Result.FileName);
                                            current.setMd5(result.Result.Md5);
                                            current.setFileSize(result.Result.Size);
                                            current.setFilePath(result.Result.FilePath);
                                            current.setFileExtension(result.Result.FileExtension);
                                            current.setDocumentType(result.Result.DocumentType);
                                            current.setIsProcessed(result.Result.IsProcessed);
                                            return false;
                                        }
                                        else {
                                            SIE.Msg.showMessage(result.Message);
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