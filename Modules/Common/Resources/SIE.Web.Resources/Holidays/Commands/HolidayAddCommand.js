SIE.defineCommand('SIE.Web.Resources.Holidays.Commands.HolidayAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        return true;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setBeginDate(data.BeginDate)
                    entity.setEndDate(data.EndDate)
                }
            }, me.view);
        }
    }
});