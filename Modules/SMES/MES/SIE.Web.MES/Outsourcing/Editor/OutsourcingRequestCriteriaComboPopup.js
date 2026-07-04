Ext.define('SIE.Web.MES.Outsourcing.Editor.OutsourcingRequestCriteriaComboPopup', {
    extend: 'SIE.Web.MES.Outsourcing.Editor.OutsourcingRequestIdLinkComboPopup',
    alias: 'widget.OutsourcingRequestCriteriaComboPopup',
    initComponent: function () {
        var me = this;
        me.editable = false;
        me.callParent();
    },
    
    //触发器事件
    onTriggerClick: function (field, trigger, e) {
        var me = this;
        if (field.readOnly)
            return;
        if (me._winNum == 0) {
            me._winNum = 1;
            me._sourceViewSelectItems = [];
            var entity = null;

            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            }
            else {
                entity = me.up("container").context.record;
            }

            if (entity.data.SupplierIds != "" && entity.data.SupplierIds != null) {
                entity.data.SupplierIds.split(me.separator).forEach(function (item) {
                    if (item) {
                        me._sourceViewSelectItems.push(parseFloat(item));
                    }
                });
            }

            me._createLayout(field);
        }
    },
});
