Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Scripts.UploadAbnormalFileEditor', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.uploadfileeditor_abnormal',
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
        var mainView = form.SIEView;//form.SIEView;
        //var mainView = context.view.grid.SIEView;//form.SIEView;
        var entity = Ext.create("SIE.EMS.EquipRepair.EquipRepairs.ViewModels.ImportFileViewModel");
        var detailView = null;
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: mainView.token,
            module: mainView.module,
            model: "SIE.EMS.EquipRepair.EquipRepairs.ViewModels.ImportFileViewModel",
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
                            var mainData = mainView.getCurrent().data;
                            if (detailView.FileContent == undefined) {
                                win.close();
                                return false;
                            }
                            SIE.invokeDataQuery({
                                type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                                method: "SaveRepairAttachment",
                                params: [detailView.FileContent, detailView.FileName],
                                async: false,
                                token: detailView.token,
                                callback: function (result) {
                                    if (result.Success) {
                                        if (result.Result != "") {
                                            detailView.mainView.getCurrent().setHandoverAttachment(result.Result);
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