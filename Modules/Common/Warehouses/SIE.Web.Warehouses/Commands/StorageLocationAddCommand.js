SIE.defineCommand('SIE.Web.Warehouses.Commands.StorageLocationAddCommand', {
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
                    entity.setCode(data.Code);
                    entity.setLibraryType(0);
                    entity.setState(1);
                    entity.belongsView.refresh();
                }
            }, me.view);
        }
    }
});