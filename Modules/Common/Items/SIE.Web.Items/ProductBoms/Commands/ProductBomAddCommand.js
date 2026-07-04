SIE.defineCommand('SIE.Web.Items.ProductBoms.Commands.ProductBomAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                var version = res.Result;
                entity.setVersion(version);
            }
        }, me.view);
    }
});