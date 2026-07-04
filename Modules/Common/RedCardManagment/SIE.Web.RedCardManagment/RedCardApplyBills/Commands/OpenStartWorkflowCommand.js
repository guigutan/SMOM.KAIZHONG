SIE.defineCommand('SIE.Web.RedCardManagment.RedCardApplyBills.Commands.OpenStartWorkflowCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "发起流程", group: "edit", iconCls: "icon-Release icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式
    isTabExist: false,

    canExecute: function (view) {
        if (view.getSelection().length != 1) return false;
        var billStatus = view.getCurrent().getBillStatus();
        var workflowStarterId = view.getCurrent().getWorkflowStarterId();

        var userInfo = CRT.Context.GlobalContext.getContext('userInfo');
        if (userInfo.EmployeeId == workflowStarterId && (billStatus == SIE.RedCardManagment.RedCardApplyBills.BillStatus.ToDo || billStatus == SIE.RedCardManagment.RedCardApplyBills.BillStatus.Reject)) return true;
        return false;
    },

    edit: function (entity) {
        this.editInForm(entity);
    },

    showView: function (entity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            recordId: entity.data.Id,
            title: me.getEditViewTitle(entity),
            isDetail: true,
            params: {
                IsStart:true,
            }
        });
    }

});