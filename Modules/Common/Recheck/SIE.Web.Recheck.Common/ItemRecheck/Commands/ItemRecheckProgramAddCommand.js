SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.ItemRecheckProgramAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setCode(data.Code);
                    entity.setState(1);                   
                }
            }, me.view);
        }
    },
});