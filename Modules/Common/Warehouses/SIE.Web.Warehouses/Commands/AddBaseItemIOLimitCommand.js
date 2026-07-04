SIE.defineCommand('SIE.Web.Warehouses.Commands.AddBaseItemIOLimitCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            var item = me.view.getParent().getCurrent();
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setItemId(item.getId());
                    entity.setItemEnableExtendProperty(item.getEnableExtendProperty());
                }
            }, me.view);
        }
    }
});