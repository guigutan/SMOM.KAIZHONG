SIE.defineCommand('SIE.Web.MES.TeamManagement.ClockingIns.Commands.EmployeeAttentSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var curData = view.getCurrent();
        if (curData != null) {
            var data = curData.getData();
            return view.getCurrent() != null && data.ClockInDate != null && data.UserEmpType == 2;
        }
        else return false;
    }

});