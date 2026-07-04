Ext.define('SIE.Web.Andon.Andons.Scripts.AndonManageUploadAttachmentEditor', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.andonuploadattachment',
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
        if (me.up().up().config.items.SIEView._current.data.State != 50) {
            me._createLayout(field);
        }
    },
    _createLayout: function (field) {
        var me = this;
        var form = me.up('form');
        var mainView = form.SIEView;

        var entity = Ext.create("SIE.Andon.Andons.ViewModels.AndonManageAttachmentViewModel");
        var detailView = null;
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: mainView.token,
            module: mainView.module,
            model: "SIE.Andon.Andons.ViewModels.AndonManageAttachmentViewModel",
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
                                type: "SIE.Web.Andon.Andons.DataQuery.AndonManageDataQuery",
                                method: "AndonManageSaveAttachment",
                                params: [detailView.FileContent, detailView.FileName],
                                async: false,
                                token: detailView.token,
                                callback: function (result) {
                                    if (result.Success) {
                                        if (result.Result != "") {
                                            detailView.mainView.getCurrent().setAttachment(result.Result);
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
