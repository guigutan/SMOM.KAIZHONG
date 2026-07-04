SIE.defineCommand('SIE.Web.Resources.CalendarSchemes.Commands.CalendarSchemeWeekAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },

    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                var data = res.Result;
                var year = new Date().getFullYear();
                var month = new Date().getMonth() + 1;
                var day = new Date().getDate() + 1;
                me.view.getCurrent().setActiveDate(year + "-" + month + "-" + day);
                if (data) {
                    me.view.getCurrent().setShiftTypeId_Display(data.Name);
                    me.view.getCurrent().setShiftTypeId(data.Id);
                }
            }
        }, me.view);
    }
});