SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.TransferEquipRepairCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "转派", group: "edit", iconCls: "icon-PeopleChange icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            if (item.data.RepairState != 1 && item.data.RepairState != 2 && item.data.RepairState != 6)//“待维修”、“维修中”、“暂停中”
                return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var editEntity = me.view.getCurrent();

        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            model: me.view.model,
            viewGroup: "TransferRepairViewGroup",
            callback: function (meta) {
                meta.token = me.view.token;
                me.viewMeta = meta;
                var detailView = SIE.AutoUI.generateAggtControl(meta);

                SIE.invokeDataQuery({
                    type: 'SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery',
                    method: 'GetEquipRepairInfo',
                    params: [editEntity.data.Id],
                    async: false,
                    action: 'queryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            editEntity.data = res.Result.data.items[0].data;
                            detailView._view._setDefaultValue(editEntity);
                            detailView._view.setData(editEntity);

                            editEntity.data.OriginalRepairMasterId = editEntity.data.RepairMasterId;
                            editEntity.data.OriginalRepairEmployeeIds = editEntity.data.RepairEmployeeIds;
                            editEntity.data.OriginalRepairEmployees = editEntity.data.RepairEmployees;

                            var cfg = {
                                associateCmd: me,
                                viewMeta: me.viewMeta,
                                entity: editEntity,
                                editMode: me.view.editMode,
                                title: "转派".t(),
                                confirm: function (isNoSave) {
                                    var retFlag = false;
                                    var indata = {};
                                    var data = editEntity.data;
                                    indata.Data = Ext.encode(data);

                                    if (data.RepairWay == null) {
                                        SIE.Msg.showError('派工类型不能为空！'.t());
                                        return false;
                                    }
                                    if (data.TransferReason == null || data.TransferReason == "") {
                                        SIE.Msg.showError('转派原因不能为空！'.t());
                                        return false;
                                    }

                                    if (editEntity.data.OriginalRepairMasterId == editEntity.data.RepairMasterId
                                        && editEntity.data.OriginalRepairEmployees == editEntity.data.RepairEmployees) {
                                        SIE.Msg.showError('维修责任人与维修人员未改变，无需转派！'.t());
                                        return false;
                                    }

                                    view.execute({
                                        data: indata,
                                        async: false,//设置成同步，即可消除弹窗无法关闭或者马上关闭的问题
                                        success: function (resExecute) { //回调
                                            retFlag = true;
                                            view.reloadData();
                                            Ext.MessageBox.alert("提示".t(), "派工成功".t());
                                        },
                                        error: function (resExecute) {
                                            SIE.Msg.showError(resExecute.Message);
                                        }
                                    });

                                    return retFlag;

                                }
                            };
                            //子视图弹框显示
                            SIE.App.showDialog(cfg);
                        }
                    }
                });
            }
        });
    }
});