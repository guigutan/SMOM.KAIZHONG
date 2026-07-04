SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.SelCalibrationItemCommand', {
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
                token: listView.token,
                model: "SIE.EMS.MeteringEquipment.Calibrations.ViewModels.SelCalibrationItemModel",
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
                        queryView.getData().data.ProjectType = wodata.ProjectType;
                        queryView.getData().data.Name = wodata.Name;

                        //隐藏清空按钮,防止清空隐藏的查询条件
                        var clearCM = queryView.getCmdControl("SIE.cmd.ClearCondition");
                        clearCM.setHidden(true);
                        var cmds = queryView.getCommands();
                        cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                        cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));

                        var win = SIE.Window.show({
                            title: "选择检验项目".t(),
                            width: 950,
                            height: 500,
                            items: ui,
                            id: "SelCalibrationItemModell000",
                            callback: function (btn) {
                                if (btn == "确定".t()) {
                                    var datas = itemview._view.getSelection();
                                    var itemList = listView.getParent().getControl();
                                    var detaildatas = listView.getData();
                                    var detaildatasIds = [];
                                    for (var i = 0; i < detaildatas.data.length; i++) {
                                        detaildatasIds.push(detaildatas.data.items[i].getProjectDetailId());
                                    }

                                    for (let data of datas) {
                                        var resdata = data;
                                        if (detaildatasIds.indexOf(resdata.data.ProjectDetailId) > -1) {
                                            continue;
                                        }
                                        var newData = listView.addNew();
                                        newData.data.ProjectDetailId = resdata.data.ProjectDetailId;
                                        newData.data.Name = resdata.data.Name;
                                        newData.data.ProjectCycleId = resdata.data.ProjectCycleId;
                                        newData.data.Part = resdata.data.Part;
                                        newData.data.Consumable = resdata.data.Consumable;
                                        newData.data.Method = resdata.data.Method;
                                        newData.data.Standard = resdata.data.Standard;
                                        newData.data.MinValue = resdata.data.MinValue;
                                        newData.data.MaxValue = resdata.data.MaxValue;
                                        newData.data.Unit = resdata.data.Unit;
                                        newData.data.UseTime = resdata.data.UseTime;
                                        newData.data.ProjectType = resdata.data.ProjectType;
                                        newData.data.CycleType = resdata.data.CycleType;
                                        detaildatas.add(newData);
                                    }
                                    itemList.SIEView._children[1].getControl().setStore(detaildatas);
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