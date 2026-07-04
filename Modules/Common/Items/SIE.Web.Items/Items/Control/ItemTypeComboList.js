Ext.define('SIE.Web.Items.Control.ItemTypeComboList', {
    extend: 'SIE.control.XComboBox',
    alias: 'widget.itemTypeComboList',
    triggerCls: "x-form-arrow-trigger",
    listeners: {
        select: function (combo, record, eOpts) {
            var me = this;
            var itemCategory = me.up('container').context.record.belongsView._children[1];
            var categoryData = itemCategory.getData();
            for (var i = 0; i < categoryData.data.items.length; i++) {
                categoryData.data.items[i].setItemCategoryId(null);
                categoryData.data.items[i].setItemCategoryName("");
            }
        }
    },
});