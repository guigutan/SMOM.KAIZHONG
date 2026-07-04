Ext.define('Ext.ux.form.YearEditor', {
    extend: 'Ext.form.field.Date',
    alias: 'widget.yearField',
    xtype: 'yearField',
    requires: ['SIE.Web.Core.YearComponent'],
    format: 'Y',
    selectYear: new Date(new Date().getFullYear(), 0, 1),
    createPicker: function () {
        var me = this;
        var yearComponent = new SIE.Web.Core.YearComponent({
            value: new Date(),
            renderTo: document.body,
            floating: true,
            hidden: true,
            focusOnShow: true,
            listeners: {
                scope: me,
                select: me.onSelect,
                cancelclick: me.onCancelClick,
                okclick: me.onOKClick
            }
        });
        me.yearComponent = yearComponent;
        return yearComponent;
    },
    onCancelClick: function () {
        var me = this;
        me.selectYear = null;
        me.collapse();
    },
    onOKClick: function () {
        var me = this;
        me.selectYear = new Date(me.yearComponent.value[1], 0, 1);
        if (me.selectYear) {
            me.setValue(me.selectYear);
            me.fireEvent('select', me, me.selectYear);
        }
        me.collapse();
    },
    onSelect: function (m, d) {
        var me = this;
        me.selectYear = new Date((d[0] + 1) + '/1/' + d[1]);
    },
    listeners: {
        change: function (editor, newValue, oldValue, eOpts) {
            if (oldValue && oldValue.length >= 3) {
                var isDate = oldValue instanceof Date;
                if (newValue instanceof Date && !isDate) {
                    if (newValue.getMonth() !== 0) {
                        editor.setValue(new Date(newValue.getFullYear(), 0, 1));
                    }
                }
            }
        }
    }
});