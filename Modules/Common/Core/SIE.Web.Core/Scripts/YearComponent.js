Ext.define('SIE.Web.Core.YearComponent', {
    extend: 'Ext.Component',
    alias: 'widget.uxYearpicker',
    alternateClassName: 'ux.uxYearpicker',
    requires: [
        'Ext.XTemplate',
        'Ext.util.ClickRepeater',
        'Ext.Date',
        'Ext.button.Button'
    ],
    isYearPicker: true,
    focusable: true,
    childEls: [
        'bodyEl', 'prevEl', 'nextEl', 'yearEl', 'buttons'
    ],
    renderTpl: [
        '<div id="{id}-bodyEl" data-ref="bodyEl" class="{baseCls}-body">',
        '<div id="{id}-yearEl" data-ref="yearEl" style="width: 100%;" class="{baseCls}-years">',
        '<div style="margin-left:25%;" class="{baseCls}-yearnav">',
        '<div class="{baseCls}-yearnav-button-ct">',
        '<a id="{id}-prevEl" data-ref="prevEl" class="{baseCls}-yearnav-button {baseCls}-yearnav-prev" hidefocus="on" role="button"></a>',
        '</div>',
        '<div class="{baseCls}-yearnav-button-ct">',
        '<a id="{id}-nextEl" data-ref="nextEl" class="{baseCls}-yearnav-button {baseCls}-yearnav-next" hidefocus="on" role="button"></a>',
        '</div>',
        '</div>',
        '<tpl for="years">',
        '<div class="{parent.baseCls}-item {parent.baseCls}-year">',
        '<a hidefocus="on" class="{parent.baseCls}-item-inner" role="button">{.}</a>',
        '</div>',
        '</tpl>',
        '</div>',
        '<div class="' + Ext.baseCSSPrefix + 'clear"></div>',
        '<tpl if="showButtons">',
        '<div id="{id}-buttons" data-ref="buttons" class="{baseCls}-buttons">{%',
        'var me=values.$comp, okBtn=me.okBtn, cancelBtn=me.cancelBtn;',
        'okBtn.ownerLayout = cancelBtn.ownerLayout = me.componentLayout;',
        'okBtn.ownerCt = cancelBtn.ownerCt = me;',
        'Ext.DomHelper.generateMarkup(okBtn.getRenderTree(), out);',
        'Ext.DomHelper.generateMarkup(cancelBtn.getRenderTree(), out);',
        '%}</div>',
        '</tpl>',
        '</div>'
    ],

    okText: '确定'.t(),
    cancelText: '取消'.t(),
    baseCls: Ext.baseCSSPrefix + 'monthpicker',
    showButtons: true,
    footerButtonUI: 'default',
    measureWidth: 35,
    measureMaxHeight: 20,
    smallCls: Ext.baseCSSPrefix + 'monthpicker-small',
    totalYears: 12,
    yearOffset: 12,
    alignOnScroll: false,

    initComponent: function () {
        var me = this;

        me.selectedCls = me.baseCls + '-selected';
        if (me.small) {
            me.addCls(me.smallCls);
        }
        me.setValue(me.value);
        me.activeYear = me.getYear(new Date().getFullYear() - 4, -4);

        if (me.showButtons) {
            me.okBtn = new Ext.button.Button({
                ui: me.footerButtonUI,
                text: me.okText,
                handler: me.onOkClick,
                scope: me
            });
            me.cancelBtn = new Ext.button.Button({
                ui: me.footerButtonUI,
                text: me.cancelText,
                handler: me.onCancelClick,
                scope: me
            });
        }
        this.callParent();
    },

    beforeRender: function () {
        var me = this;
        if (me.padding && !me.width) {
            me.cacheWidth();
        }
        me.callParent();

        Ext.apply(me.renderData, {
            years: me.getYears(),
            showButtons: me.showButtons
        });
    },

    cacheWidth: function () {
        var me = this,
            padding = me.parseBox(me.padding),
            widthEl = Ext.getBody().createChild({
                cls: me.baseCls + ' ' + me.borderBoxCls,
                style: 'position:absolute;top:-1000px;left:-1000px;',
                html: ' '
            });
        me.self.prototype.width = widthEl.getWidth() + padding.left + padding.right;
        widthEl.destroy();
    },

    afterRender: function () {
        var me = this,
            body = me.bodyEl;

        me.callParent();
        me.el.on('mousedown', me.onElClick, me, {
            translate: false
        });
        body.on({
            scope: me,
            click: 'onBodyClick',
            dblclick: 'onBodyClick'
        });
        me.years = body.select('.' + me.baseCls + '-year a');
        me.backRepeater = new Ext.util.ClickRepeater(me.prevEl, {
            handler: Ext.Function.bind(me.adjustYear, me, [-me.totalYears])
        });
        me.prevEl.addClsOnOver(me.baseCls + '-yearnav-prev-over');
        me.nextRepeater = new Ext.util.ClickRepeater(me.nextEl, {
            handler: Ext.Function.bind(me.adjustYear, me, [me.totalYears])
        });
        me.nextEl.addClsOnOver(me.baseCls + '-yearnav-next-over');
        me.updateBody();
    },

    setValue: function (value) {
        var me = this,
            active = me.activeYear,
            year;

        if (!value) {
            me.value = [null, null];
        } else if (Ext.isDate(value)) {
            me.value = [value.getMonth(), value.getFullYear()];
        } else {
            me.value = [value[0], value[1]];
        }
        if (me.rendered) {
            year = me.value[1];
            if (year !== null) {
                if ((year < active || year > active + me.yearOffset)) {
                    me.activeYear = year - me.yearOffset + 1;
                }
            }
            me.updateBody();
        }
        return me;
    },

    getValue: function () {
        return this.value;
    },

    hasSelection: function () {
        var value = this.value;
        return value[0] !== null && value[1] !== null;
    },

    getYears: function () {
        var me = this,
            offset = me.yearOffset,
            start = me.activeYear,
            end = start + offset,
            i = start,
            years = [];

        for (; i < end; ++i) {
            years.push(i);
        }
        return years;
    },

    updateBody: function () {
        var me = this,
            years = me.years,
            yearNumbers = me.getYears(),
            cls = me.selectedCls,
            value = me.getYear(null),
            year,
            yearItems, y, yLen, el;

        if (me.rendered) {
            years.removeCls(cls);
            yearItems = years.elements;
            yLen = yearItems.length;

            for (y = 0; y < yLen; y++) {
                el = Ext.fly(yearItems[y]);
                year = yearNumbers[y];
                el.dom.innerHTML = year;
                if (year === value) {
                    el.addCls(cls);
                }
            }
        }
    },

    getYear: function (defaultValue, offset) {
        var year = this.value[1];
        offset = offset || 0;
        return year === null ? defaultValue : year + offset;
    },

    onElClick: function (e) {
        e.stopEvent();
    },

    onBodyClick: function (e, t) {
        var me = this,
            isDouble = e.type === 'dblclick';
        if (e.getTarget('.' + me.baseCls + '-year')) {
            e.stopEvent();
            me.onYearClick(t, isDouble);
        }
    },

    adjustYear: function (offset) {
        if (typeof offset !== 'number') {
            offset = this.totalYears;
        }
        this.activeYear += offset;
        this.updateBody();
    },

    onOkClick: function () {
        this.fireEvent('okclick', this, this.value);
    },

    onCancelClick: function () {
        this.fireEvent('cancelclick', this);
    },

    onYearClick: function (target, isDouble) {
        var me = this;
        me.value[1] = me.activeYear + me.years.indexOf(target);
        me.updateBody();
        me.fireEvent('year' + (isDouble ? 'dbl' : '') + 'click', me, me.value);
        me.fireEvent('select', me, me.value);

    },

    beforeDestroy: function () {
        var me = this;
        me.years = me.months = null;
        Ext.destroyMembers(me, 'backRepeater', 'nextRepeater', 'okBtn', 'cancelBtn');
        me.callParent();
    },

    onDestroy: function () {
        Ext.destroyMembers(this, 'okBtn', 'cancelBtn');
        this.callParent();
    },

    privates: {
        finishRenderChildren: function () {
            var me = this;

            this.callParent(arguments);

            if (this.showButtons) {
                me.okBtn.finishRender();
                me.cancelBtn.finishRender();
            }
        }
    }
});