Ext.define('SIE.Web.Dock.Editors.StringTimeEditor', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.StringTimeEditor',

    editor: {
        xtype: 'combobox',
        displayField: 'Name',
        valueField: 'Value',
        queryMode: 'local',
        forceSelection: true,
        store: {
            fields: ["Name", "Value"],
            data: [
                { Name: "00:00", Value: "00:00" },
                { Name: "00:30", Value: "00:30" },
            ]
        }
    }
});


Ext.define('CustomTimeEditor', {
    extend: 'Ext.grid.CellEditor',
    alias: 'widget.customtimeeditor',

    field: {
        xtype: 'combo',
        editable: false,
        queryMode: 'local',
        displayField: 'time',
        valueField: 'time',
        store: {
            fields: ['time'],
            data: generateTimeData()
        }
    },

    constructor: function (config) {
        this.callParent(arguments);

        var field = this.field;
        field.on('expand', function () {
            field.getPicker().getEl().setHeight(200); // 设置下拉框的高度
        });
    }
});

function generateTimeData() {
    var data = [];
    var startTime = new Date(0, 0, 0, 0, 0); // 设置起始时间为00:00

    while (startTime.getHours() < 23 || (startTime.getHours() === 23 && startTime.getMinutes() <= 30)) {
        var timeString = Ext.Date.format(startTime, 'H:i');
        data.push({
            time: timeString
        });

        startTime = Ext.Date.add(startTime, Ext.Date.MINUTE, 30); // 增加30分钟
    }

    return data;
}