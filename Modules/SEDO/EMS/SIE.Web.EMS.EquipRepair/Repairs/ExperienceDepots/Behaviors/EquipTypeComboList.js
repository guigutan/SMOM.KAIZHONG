Ext.define('SIE.Web.EMS.EquipRepair.ExperienceDepots.EquipTypeComboList', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.EquipTypeComboList',
    listeners: {
        change: function (field, value, eOpts) {
            var me = this;

            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record;
            }

            if (value == null) {
                entity.setEquipTypeId(null);
            }

            if (entity != null && entity.isDirty() && entity.belongsView._children[0]) {
                var fault = entity.belongsView._children[0].getCurrent();
                fault.setFaultPhenomenonId(null);
                fault.setFaultPhenomenonId_Display(null);
                fault.setFaultPhenomenon(null);
            }
        }
    },
});