Ext.define('SIE.Web.MES.WorkOrders.Editors.TemplateBarcodeRule', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.TemplateBarcodeRule',
    triggerCls: "x-form-arrow-trigger",
    _onRowdblClick: function (vthis, record, element, rowIndex, e, eOpts) {
        var me = this;
        if (record) {
            me._SelectItems = [];
            var value = me.getValue();
            if (value == record.data.Id) {
                me._win.hide();
                return;
            }
            me.setValue(record);
            me._SelectItems.push(record);
            me._win.hide();
            var templateView = e.view.up().ownerCt.up().up().SIEView;
            var data = templateView._current;
            data.setLabelTemplateId("");
            data.setPackingTemplateId("");
            var woView = templateView._parent;
            woView._current[templateView.getAssociateKey()].data.items[0].dirty = true
            woView.getData().dirty = true
            woView.syncCmdState(woView, true);
        }
    },
});

Ext.define('SIE.Web.MES.WorkOrders.Editors.TemplateLabel', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.TemplateLabel',
    triggerCls: "x-form-arrow-trigger",
    _onRowdblClick: function (vthis, record, element, rowIndex, e, eOpts) {
        var me = this;
        if (record) {
            me._SelectItems = [];
            var value = me.getValue();
            if (value == record.data.Id) {
                me._win.hide();
                return;
            }
            me.setValue(record);
            me._SelectItems.push(record);
            me._win.hide();
            var templateView = e.view.up().ownerCt.up().up().SIEView;
            var woView = templateView._parent;

            if (woView._current[templateView.getAssociateKey()].data.items.length > 0) {
                woView._current[templateView.getAssociateKey()].data.items[0].dirty = true
            }
            woView.getData().dirty = true
            woView.syncCmdState(woView, true);
        }
    },
});