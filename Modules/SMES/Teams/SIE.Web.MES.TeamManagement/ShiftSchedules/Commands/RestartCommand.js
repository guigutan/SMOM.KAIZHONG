SIE.defineCommand('SIE.Web.MES.TeamManagement.ShiftSchedules.RestartCommand', {
    meta: { text: "重新开始", group: "business", iconCls: "icon-Refresh icon-blue" },
    execute: function (view) {
        var container = view.MainContainer;
        if (container) {
            container.refreshCalendar();
        }
    }
});