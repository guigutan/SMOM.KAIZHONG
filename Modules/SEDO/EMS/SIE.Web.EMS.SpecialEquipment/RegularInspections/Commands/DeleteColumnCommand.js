SIE.defineCommand('SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.DeleteColumnCommand', {
    meta: { text: "删除数据列", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
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

    execute: function (view, source) {
        var ctl = view.getController();
        ctl.removeDynamicColumn(view);
    }
});