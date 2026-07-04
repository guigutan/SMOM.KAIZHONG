SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.WorkBomAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green"},
    onItemCreated: function (entity) {
        this.callParent();
        var me = this;
        var lineNo = 0;
        var list = me.view.getData().data.items;
        if (list.length > 1) {
            lineNo = list.where(function (p) { return p.getLineNo() != ""; }).select(function (p) { return parseInt(p.getLineNo()); }).max();
            entity.setLineNo((parseInt(lineNo) + 1).toString());
        }
        else {
            entity.setLineNo(1);
        }
        entity.mon(entity, 'propertyChanged', me.WorkBomPropertyChanged, this.view)
    },
    WorkBomPropertyChanged: function (e) {
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