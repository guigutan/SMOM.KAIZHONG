SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.AllPassCommand', {
    meta: { text: "全部合格", group: "edit", iconCls: "icon-EditEntity icon-green" },
    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (parent) {
            var ApprovalStatu = parent.getApprovalStatus();
            var InspectionStatu = parent.getInspectionStatus();
            return ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.PendingReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.UnderReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.Audited.value
                && InspectionStatu !== SIE.EMS.Enums.InspectionStatus.Calirated.value;
        }
        return this.callParent(arguments);
    },

    canVisible: function (view) {
        var parent = view.getParent().getCurrent();
        if (parent) {
            var ApprovalStatu = parent.getApprovalStatus();
            var InspectionStatu = parent.getInspectionStatus();
            return ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.PendingReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.UnderReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.Audited.value
                && InspectionStatu !== SIE.EMS.Enums.InspectionStatus.Calirated.value;
        }
        return this.callParent(arguments);
    },

    execute: function (view) {
        var parent = view.getParent().getCurrent();
        var entity = view.getData().data.items;
        Ext.Array.forEach(entity, function (data) {
            data.setInspectionResult(1);
            if (data.getInspectionDate() == null) {
                data.setInspectionDate(parent.getActualInspectionDate());
            }
        })
    }
});