SIE.defineCommand('SIE.Web.LES.PrepareItems.Commands.AddPrepareItemPullCommand', {
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
                    me.view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me.view);
                }
            }, me.view);
        }
    },
    onEntityPropertyChanged: function (e) {
        var entity = e.entity;
        var view = e.entity.belongsView;
        if (e.property.length > 0 && !(e.value == e.oldvalue)) {
            if (e.property == 'ItemId') {
                SIE.invokeDataQuery({
                    async: false,
                    type: "SIE.Web.LES.PrepareItems.Model.QueryPrepareItemPull",
                    method: 'QueryMaxStock',
                    action: 'queryer',
                    token: this.token,
                    params: [entity.data.WarehouseId, entity.data.ItemId],
                    success: function success(res) {
                        if (res.Result != 0) {
                            entity.setMaxStock(res.Result);
                        }
                    },
                });
            }
        }
    },
});