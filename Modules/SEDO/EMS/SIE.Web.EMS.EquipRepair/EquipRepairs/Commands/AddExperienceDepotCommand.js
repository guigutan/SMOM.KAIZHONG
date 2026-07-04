SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddExperienceDepotCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "加入经验库", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    canExecute: function (view) {
        if (view.getParent()) {
            if (view.getParent().getCurrent())
                return !view.getParent().getCurrent().data.reportBtnDisable;
            else
                return false;
        }
        else
            return false;
    },
    execute: function (view, source) {
        var me = this;
        var data = view.getCurrent().data;
        var parent = view._parent.getCurrent();
        var signdata = {
            command: me.meta.command,
            entityType: me.view.model,
            parentType: me.view.getParent() ? me.view.getParent().model : ""
        }

        //验证维修报告是否加入过经验库
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
            method: "VerifyRepairReportIsAddDeopt",
            params: [data.RepairNo],
            async: false,
            token: view.token,
            logInfo: signdata,
            callback: function (res) {
                console.log(res);

                if (res.Result) {
                    SIE.Msg.askQuestion(Ext.String.format('当前维修单已同步过维修经验库，是否覆盖？'.L10N()), function () {
                        data.RepairType = parent.getRepairType();
                        data.SparePartId = parent.getSparePartId();
                        data.EquipAccountId = parent.getEquipAccountId();
                        data.EquipModelId = parent.getEquipModelId();
                        data.EquipTypeId = parent.getEquipTypeId();

                        view.execute({
                            data: data,
                            withIds: true,
                            selectIds: [1],                            
                            success: function (res) {
                                SIE.Msg.showMessage('覆盖成功'.t());
                            }
                        });
                    });
                }
                else {
                    SIE.Msg.askQuestion(Ext.String.format('确定加入经验库？'.L10N()), function () {                        
                        view.execute({
                            data: data,
                            withIds: true,
                            selectIds: [0],                            
                            success: function (res) {
                                SIE.Msg.showMessage('加入成功'.t());
                            }
                        });
                    });
                }
            }
        });

        //SIE.AutoUI.getMeta({
        //    async: false,
        //    ignoreCommands: false,
        //    isDetail: true,
        //    ignoreQuery: true,
        //    viewGroup: "DetailsView",
        //    token: listView.token,

        //    model: "SIE.EMS.EquipRepair.EquipRepairs.ViewModels.ExperienceDepotViewModel",
        //    callback: function (res) {
        //        var mainBlock;
        //        if (res.mainBlock)
        //            mainBlock = res.mainBlock;
        //        else
        //            mainBlock = res;
        //        var detailView = SIE.AutoUI.createDetailView(mainBlock);
        //        detailView.listView = listView;
        //        var ui = detailView.getControl();
        //        var curEntity = listView.getCurrent();
        //        var model = SIE.getModel('SIE.EMS.EquipRepair.EquipRepairs.ViewModels.ExperienceDepotViewModel');
        //        var entity = new model();

        //        entity.setId(curEntity.getId());
        //        entity.setRepairNo(curEntity.getRepairNo());
        //        entity.setRepairType(curEntity.getRepairType());
        //        entity.setEquipAccountType(curEntity.getEquipAccountType());
        //        entity.setEquipAccountMode(curEntity.getEquipAccountMode());
        //        entity.setEquipAccountId(curEntity.getEquipAccountId());
        //        entity.setEquipAccountId_Display(curEntity.getEquipAccountId_Display());
        //        entity.setEquipAccountName(curEntity.getEquipAccountName());
        //        entity.setDeviceAbnormalId(curEntity.getDeviceAbnormalId());
        //        entity.setDeviceAbnormalId_Display(curEntity.getDeviceAbnormalId_Display());
        //        entity.setDeviceAbnormalRemark(curEntity.getDeviceAbnormalRemark());
        //        entity.setFaultDescriptionId(curEntity.getFaultDescriptionId());
        //        entity.setFaultDescriptionId_Display(curEntity.getFaultDescriptionId_Display());
        //        entity.setFaultDescriptionRemark(curEntity.getFaultDescriptionRemark());
        //        entity.setDeviceAbnormalCode(curEntity.getDeviceAbnormalCode());
        //        entity.setFaultReason(curEntity.getFaultReason());
        //        entity.setFaultPart(curEntity.getFaultPart());
        //        entity.setFaultCategoryId(curEntity.getFaultCategoryId());
        //        entity.setFaultCategoryId_Display(curEntity.getFaultCategoryId_Display());
        //        entity.setRepairMethod(curEntity.getRepairMethod());
        //        entity.setPreventionAdvice(curEntity.getPreventionAdvice());

        //        detailView.setData(entity);

        //        var win = SIE.Window.show({
        //            title: "添加经验库".t(),
        //            width: 1000,
        //            height: 480,
        //            items: ui,
        //            buttons: [{
        //                xtype: "button", text: "同步", handler: function () {
        //                    if (!detailView.validateData())
        //                        return;

        //                    var entityData = ui.viewModel.data.p.data;
        //                    var thisId = entityData.Id;//获取当前对象id

        //                    //设置值
        //                    var repairNo = entityData.RepairNo;
        //                    var repairType = entityData.RepairType;
        //                    var equipAccountType = entityData.EquipAccountType;
        //                    var equipAccountMode = entityData.EquipAccountMode;
        //                    var equipAccountId = entityData.EquipAccountId;
        //                    var equipAccountId_Display = entityData.EquipAccountId_Display;
        //                    var equipAccountName = entityData.EquipAccountName;
        //                    var deviceAbnormalId = entityData.DeviceAbnormalId;
        //                    var deviceAbnormalId_Display = entityData.DeviceAbnormalId_Display;
        //                    var deviceAbnormalRemark = entityData.DeviceAbnormalRemark;
        //                    var faultDescriptionId = entityData.FaultDescriptionId;
        //                    var faultDescriptionId_Display = entityData.FaultDescriptionId_Display;
        //                    var faultDescriptionRemark = entityData.FaultDescriptionRemark;
        //                    var deviceAbnormalCode = entityData.DeviceAbnormalCode;
        //                    var faultReason = entityData.FaultReason;
        //                    var faultPart = entityData.FaultPart;
        //                    var faultCategoryId = entityData.FaultCategoryId;
        //                    var faultCategoryId_Display = entityData.FaultCategoryId_Display;
        //                    var repairMethod = entityData.RepairMethod;
        //                    var preventionAdvice = entityData.PreventionAdvice;

        //                    var lv = detailView.listView;//获取视图

        //                    var me = this;
        //                    lv.execute({
        //                        data: {
        //                            Id: thisId,
        //                            RepairNo : repairNo,
        //                            RepairType : repairType,
        //                            EquipAccountType: equipAccountType,
        //                            EquipAccountMode: equipAccountMode,
        //                            EquipAccountId: equipAccountId,
        //                            EquipAccountName: equipAccountName,
        //                            DeviceAbnormalId: deviceAbnormalId,
        //                            DeviceAbnormalRemark: deviceAbnormalRemark,
        //                            FaultDescriptionId: faultDescriptionId,
        //                            FaultDescriptionRemark: faultDescriptionRemark,
        //                            DeviceAbnormalCode: deviceAbnormalCode,
        //                            FaultReason: faultReason,
        //                            FaultPart: faultPart,
        //                            FaultCategoryId: faultCategoryId,
        //                            RepairMethod: repairMethod,
        //                            PreventionAdvice: preventionAdvice,
        //                        },
        //                        success: function (res) {
        //                            if (res.Result == true) {
        //                                me.up('window').close();
        //                                //lv.reloadData();
        //                            }
        //                        }
        //                    });
        //                }
        //            },
        //            {
        //                xtype: "button", text: "取消", handler: function () {
        //                    this.up('window').close();
        //                }
        //            }]
        //        });
        //    }
        //});
    },
});