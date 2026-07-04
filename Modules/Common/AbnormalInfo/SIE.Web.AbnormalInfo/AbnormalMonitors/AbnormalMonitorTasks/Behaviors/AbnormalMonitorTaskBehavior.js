Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.Behaviors.AbnormalMonitorTaskBehavior',
    {
        view: null,
        beforeCreate: function (meta) {
            if (!meta || !meta.gridConfig) return;
            var columns = meta.gridConfig.columns;
            if (Ext.isEmpty(columns)) return;
            var column = columns.first(function (c) { return c.dataIndex == "WarnTimes"; });
            if (Ext.isEmpty(column)) return;
            column.xtype = 'widgetcolumn';
            column.widget = {
                align: 'center',
                //width:80,
                constrainAlign: 'center',
                xtype: 'AbnormalYellowRedLightDisplay',
                bind: '{record.WarnTimes}',
            };
            //column.align = 'center';
        },
    });