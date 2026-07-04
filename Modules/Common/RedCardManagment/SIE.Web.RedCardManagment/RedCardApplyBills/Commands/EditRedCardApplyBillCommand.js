SIE.defineCommand('SIE.Web.RedCardManagment.RedCardApplyBills.Commands.EditRedCardApplyBillCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        if (view.getCurrent() != null) {
            if (view.getCurrent().getData().ApplyType == SIE.RedCardManagment.RedCardApplyBills.ApplyType.Auto) {
                return false
            }
            var employeeId = CRT.Context.GlobalContext.getContext('userInfo').EmployeeId;
            var billStatus = view.getCurrent().getData().BillStatus;
            return (billStatus == SIE.RedCardManagment.RedCardApplyBills.BillStatus.ToDo || billStatus == SIE.RedCardManagment.RedCardApplyBills.BillStatus.Reject) && view.getCurrent().getData().WorkflowStarterId == employeeId
        }
    },



   
});