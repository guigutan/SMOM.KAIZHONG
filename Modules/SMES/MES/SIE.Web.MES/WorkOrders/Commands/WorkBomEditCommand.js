SIE.defineCommand("SIE.Web.MES.WorkOrders.Commands.WorkBomEditCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            if (e.property == "SingleQty") {
                var planQty = e.entity.getWorkOrder().getPlanQty();
                var requestQty = e.entity.getSingleQty() * planQty;
                e.entity.setRequireQty(requestQty);
            }
        }
    }
});
