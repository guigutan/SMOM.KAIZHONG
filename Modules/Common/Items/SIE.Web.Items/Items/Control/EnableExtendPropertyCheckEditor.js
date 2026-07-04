Ext.define('SIE.Web.Items.Items.control.EnableExtendPropertyCheckEditor', {
    extend: 'SIE.grid.column.CheckBox',
    alias: 'widget.enableExtendPropertyCheckEditor',
    listeners: {
        checkchange: 'onCheckChange',
    },
    onCheckChange: function (me, rowIndex, checked, record, e, eOpts) {
        if (checked) {
            SIE.Msg.askQuestion('物料的库存、收发流转, 将启用扩展属性! 是否启用？'.t(),
                function () {
                    me.view.store.getData().items[rowIndex].setEnableExtendProperty(true);
                },
                function () {
                    me.view.store.getData().items[rowIndex].setEnableExtendProperty(false);
                });
        }
        else {
            SIE.Msg.askQuestion('物料取消扩展属性管理，请确认此物料是否存在库存、未完结单据! 是否取消?'.t(),
                function () {
                    me.view.store.getData().items[rowIndex].setEnableExtendProperty(false);
                },
                function () {
                    me.view.store.getData().items[rowIndex].setEnableExtendProperty(true);
                });
        }
    },
});