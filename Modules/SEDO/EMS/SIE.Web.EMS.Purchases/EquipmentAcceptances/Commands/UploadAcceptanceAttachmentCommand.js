SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.UploadAcceptanceAttachmentCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "上传", group: "edit", iconCls: "icon-ArrowWithCircleUp icon-green" },
    selectedItems: [],
    canExecute: function (listview) {
        var parent = listview._parent.getCurrent();

        if (parent == null || parent.data == null) {
            return false;
        }

        //主表状态为【待提交 10】、【驳回 50】的数据才能点击
        if (parent.data.ApprovalStatus !== 10 && parent.data.ApprovalStatus !== 50) {
            return false;
        }

        return true;
    },
    getEditEntity: function () {
        var model = SIE.getModel('SIE.EMS.Purchases.EquipmentAcceptances.EquipmentAcceptanceAttachment');
        var entity = new model();
        entity.token = this.view.token;
        return entity;
    },
    execute: function (view, source) {
        var editEntity = this.getEditEntity();
        this.showView(editEntity);
    },
    showView: function (editEntity) {
        var me = this;
        var mainView = me.view;
        var parent = mainView._parent.getCurrent();
        var parentId = parent.data.Id;

        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: false,
                isDetail: true,
                ignoreQuery: true,
                viewGroup: "DetailsView",
                token: this.view.token,
                module: mainView.module,
                model: 'SIE.EMS.Purchases.EquipmentAcceptances.EquipmentAcceptanceAttachment',
                callback: function (res) {
                    var detailView = SIE.AutoUI.generateAggtControl(res);
                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.Purchases.EquipmentAcceptances.DataQueryers.AcceptanceAttachmentDataQueryer",
                        method: "GetAcceptanceAttachmentDataForUpload",
                        params: [parentId],
                        async: false,
                        token: mainView.token,
                        callback: function (resOfGetModel) {
                            if (resOfGetModel.Success && resOfGetModel.Result != null) {
                                var defaultInfo = resOfGetModel.Result;
                                if (defaultInfo != null) {
                                    detailView._view._setDefaultValue(editEntity);
                                    detailView._view.setData(editEntity);
                                    editEntity.data = defaultInfo.Item1;
                                    detailView._view.syncCmdState();
                                    detailView.mainView = mainView;
                                    var ui = detailView.getControl();

                                    //确认上传按钮有使用此对象
                                    SIE.Window.show({
                                        title: "文件上传".t(),
                                        id: 'UploadAcceptanceAttachmentCommand_Window',
                                        width: 600,
                                        height: 220,
                                        items: ui,
                                        buttons: []
                                    });
                                }
                            }
                        }
                    });

                },
            });
        }
    }
});