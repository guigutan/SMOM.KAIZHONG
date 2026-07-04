SIE.defineCommand('SIE.Web.EMS.Purchases.PurchaseOrders.Commands.HistoryOrderQueryCommand', {
    meta: {
        text: "清空",
        title: "清除过滤条件".t(),
        tooltip: "清除过滤条件".t(),
        tooltipType: "title",
        iconCls: "icon-ClearFilter icon-blue"
    },
    execute: function (view) {
        var data = view.getCurrent().data;
        var model = view.getModel();
        var entity = Ext.create(model);
        entity.setPurchaseObjectType(data.PurchaseObjectType);
        view.setCurrent(entity);
        if (data) {
            for (pro in data) {
                if (Ext.String.endsWith(pro, '_Display')) {
                    data[pro.replace('_Display', '')] = null;
                }
            }
        }
        Ext.each(view.getControl().query('dateRange'), function (n) {
            entity.data[n.name] = { dateType: 1, BeginValue: null, EndValue: null };
            n.clearValue();
        });
        Ext.each(view.getControl().query('pagingLookUp'), function (n) {
            if (n._targetSelectItems) {
                n._targetSelectItems = { items: [], keys: [] };
            }

            if (n.lastSelectionRecord) {
                n.lastSelectionRecord = { value: [], rawValue: "" };
            }
            n.setRawValue("");
        });
        Ext.each(view.getCurrent().query('combo_popup'), function (n) {
            if (n._targetSelectItems) {
                n._targetSelectItems = { items: [], keys: [] };
            }

            if (n.lastSelectionRecord) {
                n.lastSelectionRecord = { value: [], rawValue: "" };
            }
            n.setRawValue("");
            n.setValue("");
        });
        Ext.each(view.getCurrent().query('spinRange'), function (n) {
            n.clearValue();
        });
        Ext.each(view.getCurrent().query('dataarray'), function (n) {
            n.clearValue();
        });
        Ext.each(view.getCurrent().query('textRange'), function (n) {
            n.clearValue();
        });
        //view.clearCondition();
    }
});