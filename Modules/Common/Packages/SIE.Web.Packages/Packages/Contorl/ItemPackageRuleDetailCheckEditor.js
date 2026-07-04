Ext.define('SIE.Web.Packages.Contorl.ItemPackageRuleDetailCheckEditor', {
    extend: 'SIE.grid.column.CheckBox',
    alias: 'widget.itempackageruledetailcheckeditor',
    listeners: {
        checkchange: 'onCheckChange',
    },
    onCheckChange: function (me, rowIndex, checked, record, e, eOpts) {
        if (me.view.store.getData().items.length > 0) {
            me.view.store.getData().items.where(function (p) { return p.getId() != record.getId(); }).forEach(function (item) {
                if (me.dataIndex == "IsInStockLabel") {
                    item.setIsInStockLabel(false);
                }
                if (me.dataIndex == "IsSequence") {
                    item.setIsSequence(false);
                }
                if (me.dataIndex == "IsMinPacking") {
                    item.setIsMinPacking(false);
                }
                if (me.dataIndex == "IsOutStockLabel") {
                    item.setIsOutStockLabel(false);
                }
            });
        }
    },
});