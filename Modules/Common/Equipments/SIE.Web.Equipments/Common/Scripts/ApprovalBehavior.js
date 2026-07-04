Ext.define('SIE.Web.Equipments.Common.Scripts.ApprovalBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var viewType = 1;  //界面类型（1：设备立卡，）
        var ApprovalCmdName = "SIE.Web.Equipments.EquipmentCards.Commands.AuditEquipCardCommand";
        var CancelCmdName = "SIE.Web.Equipments.EquipmentCards.Commands.CancelEquipCardCommand"

        SIE.invokeDataQuery({
            method: 'GetEnableApproval',
            params: [viewType],
            action: 'queryer',
            type: 'SIE.Web.Equipments.Common.DataQuerys.EquipmentDataQuery',
            token: view.token,
            success: function (res) {
                if (!res.Result) {
                    if (ApprovalCmdName != null) {
                        var approvalCmd = view.getCmdControl(ApprovalCmdName);
                        if (approvalCmd) {
                            approvalCmd.setHidden(true);
                            view._commands.removeAtKey(ApprovalCmdName);
                        }
                    }
                    if (CancelCmdName != null) {
                        var cancelCmd = view.getCmdControl(CancelCmdName);
                        if (cancelCmd) {
                            cancelCmd.setHidden(true);
                            view._commands.removeAtKey(CancelCmdName);
                        }
                    }
                }
            }
        });
    }
});