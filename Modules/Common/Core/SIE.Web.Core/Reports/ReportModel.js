Ext.define('SIE.Web.Core.Reports.ReportModel', function () {
    return {
        extend: 'Ext.data.Model',
        fields: [
            { name: 'Id', type: 'float' },
            { name: 'ReportInfo', type: 'string' },
            { name: 'Date', type: 'date', dateFormat: 'c' },
            { name: 'PassData', type: 'float', allowNull: true },
            {
                name: 'year',
                calculate: function (data) {
                    return parseInt(Ext.Date.format(data.Date, "Y"), 10);
                }
            }, {
                name: 'month',
                calculate: function (data) {
                    return parseInt(data.Date.getFullYear() + '' + (data.Date.getMonth() + 1));
                }
            }, {
                name: 'day',
                calculate: function (data) {
                    return parseInt(data.Date.getFullYear() + '' + (Ext.Date.format(data.Date, "m")) + '' + data.Date.getDate());
                }
            }, {
                name: 'week',
                calculate: function (data) {
                    return parseInt(Ext.Date.format(data.Date, "W"), 10);
                }
            }
        ]
    };
});


Ext.define('SIE.Web.Core.Reports.PassStore', {
    extend: 'Ext.data.Store',
    alias: 'store.PassStore',
    model: 'SIE.Web.Core.Reports.ReportModel',
    autoLoad: false
});