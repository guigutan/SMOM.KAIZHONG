SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.AuditRejectCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "驳回", group: "edit", iconCls: "icon-NetworkError icon-blue" },

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
            return ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.Reject.value
                && InspectionStatu !== SIE.EMS.Enums.InspectionStatus.Calirated.value;
        }
        return this.callParent(arguments);
    },

    /**
     * @override 执行驳回
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        if (!this.onSaving(view))
            return false;
        SIE.Msg.askQuestion("是否确定驳回？".t(),
            function () {
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                view.getCurrent().dirty = true;
                me.doSave(view);
            });
    },

    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showInstantMessage('驳回成功！'.t());
        window.setTimeout(function () {
            CRT.Event.fire("SIE.EMS.MeteringEquipment.Calibrations.Calibration_refresh");
            CRT.Workbench.closeCurrentTab();
        }, 2000);
    }
});