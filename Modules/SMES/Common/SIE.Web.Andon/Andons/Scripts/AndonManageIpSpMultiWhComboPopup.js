Ext.define('SIE.Web.Andon.Andons.Scripts.AndonManageIpSpMultiWhComboPopup', {
    extend: 'SIE.Web.Andon.Andons.Scripts.AndonManageIdLinkComboPopup',
    alias: 'widget.AndonManageIpSpMultiWhComboPopup',
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

            if (entity.data.DefectIds != "" && entity.data.DefectIds != null) {
                entity.data.DefectIds.split(me.separator).forEach(function (item) {
                    if (item) {
                        me._sourceViewSelectItems.push(parseFloat(item));
                    }
                });
            }

            me._createLayout(field);
        }
    },
});
