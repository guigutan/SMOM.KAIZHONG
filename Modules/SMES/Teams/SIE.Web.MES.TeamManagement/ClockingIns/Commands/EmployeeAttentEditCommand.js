SIE.defineCommand('SIE.Web.MES.TeamManagement.ClockingIns.Commands.EmployeeAttentEditCommand', {
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        var curData = view.getCurrent();
        if (curData != null) {
            var data = curData.getData();
            return view.getCurrent() != null && data.ClockInDate != null && data.UserEmpType == 2;
        }
        else return false;
    }
});