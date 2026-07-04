
Ext.define('Portal.ModuleTestPanel', {
    extend: 'Portal.ModuleBase',
    xtype: 'ModuleTestPanel',
    config: {
        moduleName:'ModuleTestPanel'
    },
    initParam: function () {
        var me = this;
        me.addInputParam("ModuleTestPanel_inputvalue");
        //me.addOutputParam("ModuleTestPanel_outputvalue");
    },
    refreshData: function () {
        alert("ModuleTestPanel");
    },
    listeners: {
        moduletestpanel_inputvaluechange: function (view, value, oldValue) {
            view.down("component").setHtml(value.data.name);
        }
    },
    layout: 'fit',
    height: 300,
    items: [{
        xtype: 'component',
        padding: '5',
        html: '组件通信测试'
    }]
});