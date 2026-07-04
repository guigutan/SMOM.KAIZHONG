SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.LubricationWorkHourAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var me = this;
        var parCurrent = me.view.getParent()._current;
        if (entity) {
            let startTime = parCurrent.getStartDateTime()
            let endTime = parCurrent.getEndDateTime()
            entity.setStartDateTime(startTime);
            entity.setEndDateTime(endTime);
            if (startTime != null && endTime != null && endTime > startTime) {
                var Hour = (endTime - startTime) / 1000 / 60 / 60; // 小时
                entity.setHours(Hour);
            }
            else {
                entity.setHours("");
            }
        }
    }
});