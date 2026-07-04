SIE.defineCommand('SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.AddColumnCommand', {
    meta: { text: "添加数据列", group: "edit", iconCls: "icon-AddEntity icon-green" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式
    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (parent) {
            //不为待审核，审核中,已审核，已校验
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
        this.addColumn(view);
    },
    /**添加列 */
    addColumn: function (view) {
        var ctl = view.getController();
        var dynamicColumns = ctl.getDynamicColumns(view);
        var columnHeaderIndex = dynamicColumns + 1;
        ctl.addDynamicColumn(view, columnHeaderIndex, view.dynamicIndex += 1);
    }
});