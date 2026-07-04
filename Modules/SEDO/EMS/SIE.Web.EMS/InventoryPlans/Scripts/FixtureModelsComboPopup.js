Ext.define('SIE.Web.EMS.Control.FixtureModelsComboPopup', {
    extend: 'SIE.Web.EMS.Control.IdLinkComboPopup',
    alias: 'widget.FixtureModelsComboPopup',
    listeners: {
        change: function (field, newValue, oldValue, eOpts) {
            var me = this;
            entity = me.up("form").SIEView.getData();
            var editor = SIE.Web.EMS.InventoryPlans.InventoryScriptsAction.findEditor(me.up("form"), "FixtureEncodes");
            if (editor.getValue() != "" && editor.getValue() != null) {
                entity.setFixtureEncodesIds(null);
                entity.setFixtureEncodes(null);
                editor.setRawValue(null);
                editor.setValue(null);
            }
        }
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
            if (me.up("form"))
                entity = me.up("form").SIEView.getData();
            else
                entity = me.up("container").context.record;
            if (entity.data.FixtureModelIds != "" && entity.data.FixtureModelIds != null) {
                entity.data.FixtureModelIds.split(me.separator).forEach(function (item) {
                    if (item)
                        me._sourceViewSelectItems.push(parseFloat(item));
                });
            }
            me._createLayout(field);
        }
    },

    setQueryCriteria: function (dialogView, data) {
        var criteria = dialogView._relations[0]._target.getData();
        if (data.FixtureTypes != null && data.FixtureTypes != "") {
            criteria.setFixtureTypes(data.FixtureTypes);
        }
    },
});