SIE.defineCommand('SIE.Web.Tech.Processs.Commands.EditProcessCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "iconfont icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        var me = this;
        if (entity) {
            this.mon(entity, 'propertyChanged', SIE.Web.Tech.ProcessCommonFun.ProcessPropertyChanged, me);
        }
    },
});
