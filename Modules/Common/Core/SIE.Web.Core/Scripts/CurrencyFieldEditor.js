
Ext.define('Sie.Web.CurrencyFieldEditor', {
    extend: 'Ext.form.field.Number',//'Ext.form.NumberField',
    alias: 'widget.currencyField',
    trulyValue: null,
    allowNegative: false,//不允许输入负数
    allowDecimals: true,//允许输入小数
    step: 0.01,
    getFormatValue: function (v) {
        v = typeof v == 'number' ? v : Number(v).toFixed(2);
        v = isNaN(v) ? '' : Ext.util.Format.number(this.fixPrecision(String(v)), "0,000,000.00"); //Ext.util.Format.cnMoney(v);  // formatMoney(v);
        return v;
    },
    getSubmitValue: function () {
        return Number(this.trulyValue).toFixed(2);
    },
    onChange: function (v) {
        this.toggleSpinners();
        this.callParent(arguments);
        this.trulyValue = Number(v).toFixed(2);// this.parseValue(v).toFixed(2);
    },
    validateValue: function (value) {
        var num = this.parseValue(value);
        if (isNaN(num) && num.length > 0) {
            this.markInvalid(String.format(this.nanText, value));
            return false;
        }
        return Ext.form.NumberField.superclass.validateValue.call(this, num);
    },

    onBlur: function () {
        this.setValue(this.trulyValue);
        this.setRawValue(this.getFormatValue(this.trulyValue));

    },
    onFocus: function () {

        if (this.trulyValue) {
            this.setRawValue(this.getFormatValue(this.trulyValue));
        } else this.setRaValue(this.value);
    },
    parseValue: function (value) {
        value = parseFloat(String(value).replace(this.decimalSeparator, ".").replace(/,/g, ""));
        return isNaN(value) ? '' : value;
    },
    getErrors: function (value) {
        return Ext.form.NumberField.superclass.getErrors.call(this, this.trulyValue);
    },
    initComponent: function () {
        var me = this;
        me.callParent();
    }
});





