Ext.define('Ext.ux.form.YearMonthField', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.yearMonthField',

    format: "Y-m",

    altFormats: "m/y|m/Y|m-y|m-Y|my|mY|y/m|Y/m|y-m|Y-m|ym|Ym",

    triggerCls: Ext.baseCSSPrefix + 'form-date-trigger',

    matchFieldWidth: false,

    startDay: new Date(),

    initComponent: function () {
        var me = this;


        me.disabledDatesRE = null;

        me.callParent();
    },

    initValue: function () {
        var me = this,
            value = me.value;

        if (Ext.isString(value)) {
            me.value = Ext.Date.parse(value, this.format);
        }
        if (me.value)
            me.startDay = me.value;
        me.callParent();
    },

    rawToValue: function (rawValue) {
        return rawValue || Ext.Date.parse(rawValue, this.format) || null;
    },

    valueToRaw: function (value) {
        return this.formatDate(value);
    },



    formatDate: function (date) {
        if (Ext.isDate(date))
            return Ext.Date.dateFormat(date, this.format);
        else {
            var d = new Date(date);
            if (Ext.isDate(d) && d.getYear() > 0)
                return Ext.Date.dateFormat(d, this.format);
            else
                return date;
        }

    },
    createPicker: function () {
        var me = this,
            format = Ext.String.format;

        return Ext.create('Ext.picker.Month', {
            pickerField: me,
            ownerCt: me.ownerCt,
            renderTo: document.body,
            floating: true,
            shadow: false,
            focusOnShow: true,
            listeners: {
                scope: me,
                cancelclick: me.onCancelClick,
                okclick: me.onOkClick,
                yeardblclick: me.onOkClick,
                monthdblclick: me.onOkClick
            }
        });
    },

    onExpand: function () {
        this.picker.setValue(this.startDay);
    },

    onOkClick: function (picker, value) {
        var me = this,
            month = value[0],
            year = value[1],
            date = new Date(year, month, 1);
        me.startDay = date;
        me.setValue(date);
        this.picker.hide();
    },

    onCancelClick: function () {
        this.picker.hide();
    }
});