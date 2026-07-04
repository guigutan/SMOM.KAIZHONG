SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SaveEquipRepairCommand', {
    extend:'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var data = view.getCurrent().data;
        var detailData = view.findChild("SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill").getCurrent().data;

        data.DeviceAbnormalId = detailData.DeviceAbnormalId;
        data.DeviceAbnormalRemark = detailData.DeviceAbnormalRemark;
        data.ProduceState = detailData.ProduceState;
        data.UrgentDegree = detailData.UrgentDegree;
        if (data.EquipAccountId == null && data.SparePartId == null) {
            SIE.Msg.showError("设备或备件不能空！".t());
            return false;
        }

        if (data.DeviceAbnormalId == null && data.DeviceAbnormalRemark.length <= 0) {
            SIE.Msg.showError("【故障现象】与【故障现象（备注）】至少有一个不为空！".t());
            return false;
        }
        if (data.RepairType == 0) {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                method: "CheckPlanWithUnFinishRepairBill",
                params: [data.EquipAccountId],
                async: false,
                token: view.token,
                callback: function (res) {
                    if (res.Success) {
                        if (res.Result.length > 0) {
                            SIE.Msg.askQuestion(Ext.String.format("设备存在未完成的维修单，报修人{0},是否继续报修？".t(), res.Result), function () {
                                me.dosave(view, data);
                            });
                        }
                        else {
                            me.dosave(view, data);
                        }
                    } else {
                        SIE.Msg.showError(res.Message);
                    }
                }
            })
        }
        else {
            me.dosave(view, data);
        }
    },

    // 保存
    dosave: function (view, data) {
        var me = this;
        view.execute({
            data: data,
            callback: function (res) {
                if (res.Success) {
                    me.onSavedMsg(view);
                }
            }
        })
    },

    /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view) {
        var isClose = false;
        view.getCurrent().markSaved();
        SIE.Msg.showInstantMessage('提交成功'.t(), '报修'.t(), 3, function () {
            isClose = true;
            CRT.Workbench.closeCurrentTab();
        });
        if (!isClose) {
            Ext.defer(function () {
                CRT.Workbench.closeCurrentTab();
            }, 3000);
        }
    }
});