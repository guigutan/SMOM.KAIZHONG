SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.MaterialApplyCommand', {
    meta: { text: "领料申请", group: "edit", iconCls: "icon-ApplicationGo icon-blue" },
    canExecute: function (view) {
        var parent = view._parent.getCurrent();
        if (parent == null) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 40) {
            return false;
        }
        if (parent.data.SetupStatus == 30 || parent.data.SetupStatus == 40) {
            return false;
        }
        return view.canAddItem();
    },
    execute: function (view, source) {
        var me = this;
        var parent = view._parent.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.EquipmentSetups.ViewModels.MaterialApplyViewModel",
            module: view._parent.module,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                var entity = new detailView._view._model();
                entity.setEquipmentSetupId(parent.getId());
                detailView._view.setData(entity);
                var win = SIE.Window.show({
                    title: "",
                    width: '70%',
                    height: '80%',
                    items: detailView.getControl(),
                    id: "MaterialApplyCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            if (detailView._view._children.length === 0) {
                                SIE.Msg.showError("界面子列表无权限，请配置".t());
                                return;
                            }

                            var detailChildView = detailView._view._children[0];
                            var detailList = [];
                            SIE.each(detailChildView.getData().data.items, function (model) {
                                detailList.push(model.data);
                            });

                            var signdata = {
                                command: me.meta.command,
                                entityType: me.view.model,
                                parentType: me.view.getParent() ? me.view.getParent().model : ""
                            }

                            SIE.invokeDataQuery({
                                method: 'MaterialApply',
                                params: [detailView._view.getCurrent().data, detailList],
                                action: 'queryer',
                                type: 'SIE.Web.EMS.Purchases.EquipmentSetups.EquipmentSetupDataQueryer',
                                token: view.token,
                                logInfo: signdata,
                                success: function (result) {
                                    win.close();
                                    view._parent.reloadData();
                                }
                            });
                            return false;
                        }
                    }
                });
            }
        });
    }
});