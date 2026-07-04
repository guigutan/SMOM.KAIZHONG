SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentSetups.Commands.ReassignmentCommand', {
    meta: { text: "转派", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var selectModels = view.getSelection();
        if (selectModels.length == 0) return false;
        var res = true;
        SIE.each(selectModels, function (model) {
            if (model.data.ApprovalStatus !== 10 && model.data.ApprovalStatus !== 50) {
                res = false;
                return false;
            }
        });
        return res;
    },
    execute: function (view, source) {
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.EquipmentSetups.ViewModels.SelPrincipalViewModel",
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "选择员工".t(),
                    width: 480,
                    height: 200,
                    items: detailView.getControl(),
                    id: "ReassignmentCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var model = detailView.getData().data;
                            if (model.PrincipalId <= 0) {
                                SIE.Msg.showError("请选择员工".t());
                                return false;
                            }
                            var selectModels = view.getSelection();
                            var selectIds = view.getSelectionIds(selectModels);
                            view.execute({
                                withIds: true,
                                data: model.PrincipalId,
                                selectIds: selectIds,
                                success: function (resu) {
                                    SIE.Msg.showMessage("转派成功!".t());
                                    view.reloadData();
                                }
                            });
                            win.close();
                            return false;
                        }
                    }
                });
            }
        });
    }
});