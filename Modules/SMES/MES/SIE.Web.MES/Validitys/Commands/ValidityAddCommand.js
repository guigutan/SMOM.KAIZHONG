SIE.defineCommand("SIE.Web.MES.Validitys.Commands.ValidityAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        this.view.execute({
            data: model,
            success: function (res) {
                var data = res.Result;
                entity.setEffective(data.Effective);
            }
        }, me.view);
    }
});

