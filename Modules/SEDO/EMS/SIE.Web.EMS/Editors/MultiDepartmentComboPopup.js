Ext.define('SIE.Web.EMS.Editors.MultiDepartmentComboPopup', {
    extend: 'SIE.Web.EMS.Control.IdLinkComboPopup',
    alias: 'widget.emsMultiDepartmentComboPopup',
    /**
     * 初始化
     */
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

            if (entity.data.DepartmentIds != "" && entity.data.DepartmentIds != null) {
                entity.data.DepartmentIds.split(me.separator).forEach(function (item) {
                    if (item) {
                        me._sourceViewSelectItems.push(parseFloat(item));
                    }
                });
            }

            me._createLayout(field);
        }
    },
});