SIE.defineCommand('SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.InputSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },

    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current) {
            var ApprovalStatu = current.getApprovalStatus();
            var InspectionStatu = current.getInspectionStatus();
            return ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.PendingReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.UnderReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.Audited.value
                && InspectionStatu !== SIE.EMS.Enums.InspectionStatus.Calirated.value
                && current.isDirty();
        }
        return this.callParent(arguments);
    },

    canVisible: function (view, source) {
        var current = view.getCurrent();
        if (current) {
            var ApprovalStatu = current.getApprovalStatus();
            var InspectionStatu = current.getInspectionStatus();
            return ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.PendingReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.UnderReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.Audited.value
                && InspectionStatu !== SIE.EMS.Enums.InspectionStatus.Calirated.value;
        }
        return this.callParent(arguments);
    },

    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var me = this;
        var ent = view.getCurrent();

        if (ent.getInspectionStatus() === SIE.EMS.Enums.InspectionStatus.Pending.value) {
            ent.setInspectionStatus(SIE.EMS.Enums.InspectionStatus.Under.value); //改为校验中
        }
        //数据已保存到服务器，修改状态
        ent.markSaved();
        CRT.Event.fire(view.model + "_refresh", ent.data.Id);
        me.onSavedMsg(view, res);
        view.syncCmdState();
    }
});