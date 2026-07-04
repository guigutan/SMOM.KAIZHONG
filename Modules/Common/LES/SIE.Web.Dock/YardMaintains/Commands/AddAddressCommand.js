SIE.defineCommand('SIE.Web.Dock.YardMaintains.Commands.AddAddressCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        me = this;
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setState(1);
                }
            }, me.view);

            this.mon(entity, 'propertyChanged', SIE.Web.CSM.AddressAction.onEntityPropertyChanged, this);
        }
    },
});