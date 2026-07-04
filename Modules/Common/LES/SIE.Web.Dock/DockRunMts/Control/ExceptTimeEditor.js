Ext.define('SIE.Web.Dock.ExceptTimeEditor', {
    extend: 'Ext.form.field.Date',
    alias: 'widget.ExceptTimeEditor',
    requires: ['SIE.control.DateTimePicker'],
    initComponent: function () {
        this.format = this.format;
        this.callParent();
    },
    format: 'Y-m-d H:i:s',
    formatText: '',
    minText: null,
    maxText: null,
    invalidText: '{0}不是有效日期',
    createPicker: function () {
        var me = this,
            format = Ext.String.format;
        this.rawDate = this.value || this.config.dateTimeValue;
        me.minValue = new Date(this.config.minValue);
        me.maxValue = new Date(this.config.maxValue);
        return Ext.create('SIE.control.DateTimePicker', {
            ownerCt: me.ownerCt,
            //                  renderTo: document.body,
            floating: true,
            //                  hidden: true,
            focusOnShow: true,
            minDate: me.minValue,
            maxDate: me.maxValue,
            bindname: me.name,
            disabledDatesRE: me.disabledDatesRE,
            disabledDatesText: me.disabledDatesText,
            disabledDays: me.disabledDays,
            disabledDaysText: me.disabledDaysText,
            format: me.format,
            showToday: me.showToday,
            startDay: me.startDay,
            minText: format(me.minText, me.formatDate(me.minValue)),
            maxText: format(me.maxText, me.formatDate(me.maxValue)),
            listeners: {
                scope: me,
                select: me.onSelect,
            },
            keyNavConfig: {
                esc: function () {
                    me.collapse();
                }
            }
        });
    },
    //展开的时候设置它的默认值
    onExpand: function () {
        var value = this.rawDate;
        if (value != null) {
            value = Ext.isDate(value) ? value : (value ? new Date(value) : new Date());
            this.picker.setValue(Ext.isDate(value) ? value : this.createInitialDate());
        } else {
            var value = new Date();
        }
        
        this.rawDate = this.value;
    },
    collapse: function () {
        var me = this;
        var srcElement = event.srcElement;
        var isSelectDate = (' ' + srcElement.className + ' ').indexOf(' ' + 'x-datepicker-date' + ' ') > -1
            || srcElement.textContent == '确认'.t()
            || (me.picker && !Ext.fly(srcElement).up('#' + me.picker.el.dom.id)) || me.getRawValue() === "";

        if (!isSelectDate) {
            me.el.dom.focus();
            return;
        }

        if (me.isExpanded && !me.destroyed && !me.destroying) {
            var openCls = me.openCls,
                picker = me.picker,
                aboveSfx = '-above';
            // hide the picker and set isExpanded flag
            picker.hide();
            me.isExpanded = false;
            // remove the openCls
            me.bodyEl.removeCls([
                openCls,
                openCls + aboveSfx
            ]);
            picker.el.removeCls(picker.baseCls + aboveSfx);
            if (!me.ariaStaticRoles[me.ariaRole]) {
                me.ariaEl.dom.setAttribute('aria-expanded', false);
            }
            // remove event listeners
            me.touchListeners.destroy();
            me.scrollListeners.destroy();
            Ext.un('resize', me.alignPicker, me);
            me.fireEvent('collapse', me);
            me.onCollapse();
        }
    },

    getErrors: function (value) {
        var errors = this.callParent(arguments);
        for (var i = 0; i < errors.length; i++) {
            if (!errors[i]) {
                errors.splice(i, 1);
                i--;
            }
        }
        return errors;
    },

});