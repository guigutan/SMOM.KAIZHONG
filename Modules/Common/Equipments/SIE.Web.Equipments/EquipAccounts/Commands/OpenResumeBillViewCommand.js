SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.OpenResumeBillViewCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "打开", group: "edit", iconCls: "icon-Search icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();
        if (current === null) {
            return false;
        }
        var no = current.getNo();
        if (no == "") {
            return false;
        }
        return true;
    },
    execute: function (listView, source) {
        var no = listView.getCurrent().getNo();
        var type = listView.getCurrent().getResumeType();
        if (no != "") {
            var rawId = "";
            var tabId = "";
            //报修 维修
            if ((type == 0 || type == 1)) {
                rawId = "SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill,SIE.EMS.EquipRepair";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill',
                    module: listView.module,
                    title: ('维修管理_' + no).L10N(),
                    isAggt: true,
                    params: {
                        RepairNo: listView.getCurrent().getNo()
                    }
                });
            }

            //保养
            if ((type == 2)) {
                rawId = "SIE.EMS.Maintains.Records.MaintainRecord,SIE.EMS";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.Maintains.Records.MaintainRecord',
                    module: listView.module,
                    title: ('设备保养记录_' + no).L10N(),
                    isAggt: true,
                    params: {
                        MaintainNo: listView.getCurrent().getNo()
                    }
                });
            }
            //点检
            if ((type == 4)) {
                rawId = "SIE.EMS.Checks.Records.CheckRecord,SIE.EMS";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.Checks.Records.CheckRecord',
                    module: listView.module,
                    title: ('设备点检记录_' + no).L10N(),
                    isAggt: true,
                    params: {
                        CheckPlanNo: listView.getCurrent().getNo()
                    }
                });
            }
            //润滑
            if ((type == 10)) {
                rawId = "SIE.EMS.Lubrications.Lubrication,SIE.EMS";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.Lubrications.Lubrication',
                    module: listView.module,
                    title: ('润滑记录_' + no).L10N(),
                    isAggt: true,
                    params: {
                        LubricationNo: listView.getCurrent().getNo()
                    }
                });
            }
            //特种设备定检
            if ((type == 12)) {
                rawId = "SIE.EMS.SpecialEquipment.RegularInspections.RegularInspection,SIE.EMS.SpecialEquipment";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.SpecialEquipment.RegularInspections.RegularInspection',
                    module: listView.module,
                    title: ('特种设备定检_' + no).L10N(),
                    isAggt: true,
                    params: {
                        InspectionNo: listView.getCurrent().getNo()
                    }
                });
            }
            //计量设备定检
            if ((type == 13)) {
                rawId = "SIE.EMS.MeteringEquipment.Calibrations.Calibration,SIE.EMS.MeteringEquipment";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.MeteringEquipment.Calibrations.Calibration',
                    module: listView.module,
                    title: ('计量设备定检_' + no).L10N(),
                    isAggt: true,
                    params: {
                        InspectionNo: listView.getCurrent().getNo()
                    }
                });
            }
            //资产调拨
            if (type == 11) {
                rawId = "SIE.EMS.AssetTransfers.AssetTransfer,SIE.EMS";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.AssetTransfers.AssetTransfer',
                    module: listView.module,
                    title: ('资产调拨_' + no).L10N(),
                    isAggt: true,
                    params: {
                        AssetTransferNo: listView.getCurrent().getNo()
                    }
                });
            }
            //计划维修
            if (type == 18) {
                rawId = "SIE.EMS.EquipRepair.PlanRepairs.PlanRepair,SIE.EMS.EquipRepair";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.EquipRepair.PlanRepairs.PlanRepair',
                    module: listView.module,
                    title: ('计划维修_' + no).L10N(),
                    isAggt: true,
                    params: {
                        PlanRepairNo: listView.getCurrent().getNo()
                    }
                });
            }
            //设备立卡
            if (type == 23) {
                rawId = "SIE.Equipments.EquipmentCards.EquipmentCard,SIE.Equipments";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.Equipments.EquipmentCards.EquipmentCard',
                    module: listView.module,
                    title: ('设备立卡_' + no).L10N(),
                    isAggt: true,
                    params: {
                        Code: listView.getCurrent().getNo()
                    }
                });
            }
            if (type == 14 || type == 15 || type == 16 || type == 17) {
                rawId = "SIE.EMS.IdleArchives.IdleArchive,SIE.EMS";
                tabId = ('tab_' + rawId).replace(/[.|,]/g, '') + no;
                CRT.Workbench.addPage({
                    tabId: tabId,
                    entityType: 'SIE.EMS.IdleArchives.IdleArchive',
                    module: listView.module,
                    title: ('闲置封存_' + no).L10N(),
                    isAggt: true,
                    params: {
                        IdleArchiveNo: listView.getCurrent().getNo()
                    }
                });
            }

            //
        }
    }
});