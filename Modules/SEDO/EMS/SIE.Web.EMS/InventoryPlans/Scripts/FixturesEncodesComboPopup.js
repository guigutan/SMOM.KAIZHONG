Ext.define('SIE.Web.EMS.Control.MultiLocComboPopup', {
        extend: 'SIE.Web.EMS.Control.IdLinkComboPopup',
    alias: 'widget.FixturesEncodesComboPopup',
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
            if (entity.data.FixtureEncodeIds != "" && entity.data.FixtureEncodeIds != null) {
                entity.data.FixtureEncodeIds.split(me.separator).forEach(function (item) {
                    if (item)
                        me._sourceViewSelectItems.push(parseFloat(item));
                });
            }
            me._createLayout(field);
        }
    },

    setQueryCriteria: function (dialogView, data) {
        var criteria = dialogView._relations[0]._target.getData();
        if (data.FixtureModels != null && data.FixtureModels != "") {
            criteria.setFixtureModelName(data.FixtureModels);
        }
        if (data.FixtureTypes != null && data.FixtureTypes != "") {
            criteria.setFixtureTypes(data.FixtureTypes);
        }
    }
});