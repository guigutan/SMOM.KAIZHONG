Ext.define('SIE.Web.EMS.EquipRepair.ExperienceDepots.FaultEditorComboList', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.FaultEditorComboList',
    listeners: {
        focus: function () {
            //获取焦点
            var me = this;
            var parent = me.up("form").SIEView.getParent().getCurrent();
            if (parent) {
                var cur = me.up("form").SIEView.getCurrent();
                cur.setEquipTypeId(parent.getEquipTypeId());
                cur.setRepairType(parent.getRepairType());
            }
        },
    },
});