SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.EngineerConfirmCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "工程确认", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    selectedItems: [],
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0)
            return false;

        for (i = 0; i < this.selectedItems.length; i++) {
            var item = this.selectedItems[i];
            //维修状态为 【待评分】时，才能工程确认
            if (item.data.RepairState != 4)
                return false;
        }
        return true;
    },
    onSaving: function (view) {
        view.getCurrent().dirty = true;
        return this.callParent(arguments);
    },
    onSaved: function (view, source) {
        var me = this;
        var editEntity = this.view.getCurrent();
        var isShowErrMsg = false;

        //验证维修报告是否填写完整
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
            method: "VerifyRepairReport",
            params: [editEntity.data.Id],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Result.ErrMsg != "") {
                    SIE.Msg.showError(res.Result.ErrMsg);
                    isShowErrMsg = true;
                }
            }
        });
        if (!isShowErrMsg)
            SIE.AutoUI.getMeta({
                async: false,
                ignoreCommands: true,
                isDetail: true,
                ignoreQuery: true,
                model: this.view.model,
                viewGroup: "EngineerConfirmViewGroup",
                callback: function (meta) {
                    meta.token = me.view.token;
                    me.viewMeta = meta;
                    var detailView = SIE.AutoUI.generateAggtControl(meta);

                    //var model = SIE.getModel(me.view.model);
                    //var entity = new model();

                    var entity = editEntity;

                    //查询出维修响应时间、执行时间、总工时
                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                        method: "GetRepairTime",
                        params: [editEntity.data.Id],
                        async: false,
                        token: meta.token,
                        callback: function (res) {
                            entity.setRespondTime(res.Result.RespondTime);
                            entity.setExecuteTime(res.Result.ExecuteTime);
                            entity.setRepairTotalTime(res.Result.RepairTotalTime);

                            detailView._view._setDefaultValue(entity);
                            detailView._view.setData(entity);

                            var win = SIE.Window.show({
                                title: '工程确认'.t(),
                                width: '60%',
                                height: '80%',
                                items: detailView.getControl(),
                                callback: function (btn) {
                                    var retFlag = false;
                                    if (btn === "确定".t()) {
                                        entity.data.Id = editEntity.data.Id;
                                        entity.data.RepairType = editEntity.data.RepairType;
                                        entity.data.EquipAccountId = editEntity.data.EquipAccountId;

                                        var detailArr = [];
                                        var tpmList = detailView._view._children[0].getData().getData().items;
                                        if (tpmList.length == 0) {
                                            SIE.Msg.showError('没有评分项目，请先添加!'.t());
                                            return false;
                                        }
                                        Ext.each(tpmList, function (item) {
                                            item.data.EquipRepairBillId = editEntity.data.Id;
                                            item.data.EngineerAttachment = item.data.ProjectNameView;
                                            detailArr.push(item.data);
                                        });

                                        var indata = {};
                                        var data = { repairBill: entity.data, detailList: detailArr };

                                        //indata.Data = Ext.encode(data);
                                        indata = data;
                                        view.execute({
                                            data: indata,
                                            success: function (res) {
                                                retFlag = true;
                                                SIE.Msg.showMessage('确认完成'.t());
                                                view.reloadData();
                                                win.close();
                                            },
                                            error: function (res) {
                                                SIE.Msg.showError(res.Message);
                                            }
                                        });

                                        if (retFlag) {
                                            return true;
                                        }
                                        else {
                                            return false;
                                        }
                                    }
                                }
                            });
                        }
                    });
                }
            });
    }
});