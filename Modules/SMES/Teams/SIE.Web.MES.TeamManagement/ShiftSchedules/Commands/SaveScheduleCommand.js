SIE.defineCommand('SIE.Web.MES.TeamManagement.ShiftSchedules.SaveScheduleCommand', {
    meta: { text: "保存", group: "business", iconCls: "icon-SaveEntity icon-blue" },
    execute: function (view) {
        var container = view.MainContainer;
        if (container) {
            container.saveShiftSchedule(view.token);
        }
    }
});