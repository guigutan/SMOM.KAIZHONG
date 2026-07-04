
Ext.define('Portal.StockModule', {
    extend: 'Portal.ModuleBase',
    xtype: 'StockModule',
    config: {
        moduleName: '组件名称-StockModule'
    },
    initParam: function () {
        var me = this;
        me.addOutputParam("stocks_outputSelectValue");
    },
    refreshData: function () {
        alert("StockModule");
    },
    border: false,
    layout: 'fit',
    height: 300,
    items: {
        xtype: 'stocks',
        listeners: {
            itemclick: function (view, record, item, index, e, eOpts) {
                window.EB.emit("outputPropertyChanged", { "stocks_outputSelectValue": record });
            }
        },
    }
});