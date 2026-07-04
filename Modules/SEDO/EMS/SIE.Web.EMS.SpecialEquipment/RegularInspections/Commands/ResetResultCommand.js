SIE.defineCommand('SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.ResetResultCommand', {
    meta: { text: "重置结果", group: "edit", iconCls: "icon-EditEntity icon-green" },
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
        var entity = view.getData().data.items;
        Ext.Array.forEach(entity, function (data) {
            data.setInspectionResult(null);
        })
    }
});