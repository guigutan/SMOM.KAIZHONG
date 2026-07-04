SIE.defineCommand('SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Commands.ItemUrgentOrderAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    var data = res.Result;
                    entity.setNo(data.No);
                    entity.setOrderState(data.OrderState);
                    entity.setQty(data.Qty);
                    entity.setDemandTime(data.DemandTime);
                }
            }, me.view);
        }
        this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
    },
    _onEntityPropertyChanged: function (e) {
        var me = this;
        var data = e.entity.data;
        if (e.property.length > 0) {
            if (e.property.indexOf('IsReceive') >= 0) {
                if (entity.getIsReceive()) {
                    entity.setReceiveQty(entity.getQty());
                }
            }
        }
    }
})