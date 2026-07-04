SIE.defineCommand('SIE.Web.Resources.ShiftTypes.Commands.ShiftResetAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },

    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                me.view.getCurrent().setBeginTime(new Date(new Date(new Date().toDateString()).getTime()));
                me.view.getCurrent().setEndTime(new Date(new Date(new Date().toDateString()).getTime() + 24 * 60 * 60 * 1000 - 1));
            }
        }, me.view);
        
    }
});