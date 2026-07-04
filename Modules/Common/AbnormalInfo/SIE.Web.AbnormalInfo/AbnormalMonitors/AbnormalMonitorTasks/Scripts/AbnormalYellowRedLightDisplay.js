Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.Scripts.AbnormalYellowRedLightDisplay', {
    extend: 'Ext.sparkline.Pie',
    xtype: 'AbnormalYellowRedLightDisplay',
    alias: 'widget.AbnormalYellowRedLightDisplay',

    onUpdate: function () {
        var me = this,
            value = me.values.first();
        me.callParent(arguments);
         //0 无 1 黄灯 2 红灯
        var pieColor = '#fff';
        if (value == 0)
            pieColor = '#fff';
        else if (value == 1)
            pieColor = '#FBD417';
        else if (value >= 2)
            pieColor = 'red';
        me.setConfig('sliceColors', [pieColor]);
    },

    updateDisplay: function () {
        //重载，不显示tip
    },
});