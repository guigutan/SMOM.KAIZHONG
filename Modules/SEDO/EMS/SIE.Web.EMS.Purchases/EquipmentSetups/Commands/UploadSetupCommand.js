SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.UploadSetupCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "上传", group: "edit", iconCls: "icon-ArrowWithCircleUp icon-green" },
    selectedItems: [],
    canExecute: function (listview) {
        var parent = listview._parent.getCurrent();
        if (parent == null || parent.data == null) {
            return false;
        }
        if (parent.phantom) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 10 && parent.data.ApprovalStatus !== 50) {
            return false;
        }
        return true;
    },
    getEditEntity: function () {
        var model = SIE.getModel('SIE.EMS.Purchases.EquipmentSetups.SetupAttachment');
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
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: this.view.token,
            module: mainView.module,
            model: 'SIE.EMS.Purchases.EquipmentSetups.SetupAttachment',
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                detailView._view.setData(editEntity);
                editEntity.setEquipmentSetupId(parentId);
                detailView._view.syncCmdState();
                detailView._view.mainView = mainView;
                var ui = detailView.getControl();
                SIE.Window.show({
                    title: "文件上传".t(),
                    id: 'UploadSetupCommand_Window',
                    width: 900,
                    height: 300,
                    items: ui,
                    buttons: []
                });
            },
        });
    }
});