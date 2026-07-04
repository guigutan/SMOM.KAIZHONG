Ext.define('SIE.Web.EMS.Control.FixtureTypesComboPopup', {
    extend: 'SIE.Web.EMS.Control.IdLinkComboPopup',
    alias: 'widget.FixtureTypesComboPopup',
    listeners: {
        change: function (field, newValue, oldValue, eOpts) {
            var me = this;
            var entity = me.up("form").SIEView.getData();
            var editor = SIE.Web.EMS.InventoryPlans.InventoryScriptsAction.findEditor(me.up("form"), "FixtureModels");
            if (entity) {
                if (editor.getValue() != "" && editor.getValue() != null) {
                    entity.setFixtureModelIds(null);
                    entity.setFixtureModels(null);
                    editor.setRawValue(null);
                    editor.setValue(null);
                }
                var locEditor = SIE.Web.EMS.InventoryPlans.InventoryScriptsAction.findEditor(me.up("form"), "FixtureEncodes");
                if (locEditor.getValue() != "" && locEditor.getValue() != null) {
                    entity.setFixtureEncodeIds(null);
                    entity.setFixtureEncodes(null);
                    locEditor.setRawValue(null);
                    locEditor.setValue(null);
                }
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
            if (entity.data.FixtureTypeIds != "" && entity.data.FixtureTypeIds != null) {
                entity.data.FixtureTypeIds.split(me.separator).forEach(function (item) {
                    if (item)
                        me._sourceViewSelectItems.push(parseFloat(item));
                });
            }
            me._createLayout(field);
        }
    },
});