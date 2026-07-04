SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.SelCalibrationEquipmentCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        var parent = view.getParent();
        if (parent == null) {
            return false;
        }
        var parData = parent.getData();
        if (parData == null) {
            return false;
        }
        return parent != null && parData.data != null;
    },
    execute: function (listView, source) {
        var me = this;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                ignoreCommands: true,
                isAggt: true,
                // viewGroup: "ReadonlyView",
                token: listView.token,
                model: "SIE.EMS.MeteringEquipment.Calibrations.ViewModels.SelCalibrationEquipmentModel",
                callback: function (res) {
                    var view = SIE.AutoUI.generateAggtControl(res);
                    var ui = view.getControl();
                    var itemview = view;
                    //设置看不见的条件
                    var wodata = listView.getParent().getData().data;
                    var dialogView = view.getView();
                    if (dialogView.getRelations() && dialogView.getRelations().length > 0 && dialogView.getRelations()[0].getTarget() && view.getView().getRelations()[0].getTarget().viewGroup === 'QueryView') {
                        //设置隐藏的查询条件
                        var queryView = dialogView.getRelations()[0].getTarget();
                        queryView.getData().data.InspectionRuleId = wodata.InspectionRuleId;

                        //隐藏清空按钮,防止清空隐藏的查询条件
                        var clearCM = queryView.getCmdControl("SIE.cmd.ClearCondition");
                        clearCM.setHidden(true);
                        var cmds = queryView.getCommands();
                        cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                        cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));

                        var win = SIE.Window.show({
                            title: "选择设备清单".t(),
                            width: 950,
                            height: 500,
                            items: ui,
                            id: "SelCalibrationEquipmentModell000",
                            callback: function (btn) {
                                if (btn == "确定".t()) {
                                    var datas = itemview._view.getSelection();
                                    var itemList = listView.getParent().getControl();
                                    var detaildatas = listView.getData();
                                    var detaildatasIds = [];
                                    for (var i = 0; i < detaildatas.data.length; i++) {
                                        detaildatasIds.push(detaildatas.data.items[i].getMeteringEquipmentAccountId());
                                    }
                                    for (let data of datas) {
                                        var resdata = data;
                                        if (detaildatasIds.indexOf(resdata.data.EquipAccountId) > -1) {
                                            continue;
                                        }
                                        var newData = listView.addNew();
                                        newData.data.MeteringEquipmentAccountId = resdata.data.EquipAccountId;
                                        newData.data.MeteringEquipmentAccountCode = resdata.data.Code;
                                        newData.data.MeteringEquipmentAccountName = resdata.data.Name;
                                        newData.data.Specifications = resdata.data.Specifications;
                                        newData.data.IsDowngrade = resdata.data.IsDowngrade;
                                        newData.data.PrecisionClass = resdata.data.PrecisionClass;
                                        detaildatas.add(newData);
                                    }
                                    itemList.SIEView._children[0].getControl().setStore(detaildatas);
                                    listView._parent.getData().dirty = true
                                    listView._parent.syncCmdState(listView._parent, true)
                                    win.close();

                                    return false;
                                }
                            }
                        });
                        queryView.tryExecuteQuery();
                    }
                }
            });
        }
    }
});