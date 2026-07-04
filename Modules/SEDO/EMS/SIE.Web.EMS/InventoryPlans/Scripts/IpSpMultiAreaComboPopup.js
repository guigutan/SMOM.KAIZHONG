Ext.define('SIE.Web.EMS.Control.IpSpMultiAreaComboPopup', {
    extend: 'SIE.Web.EMS.Control.IdLinkComboPopup',
    alias: 'widget.IpSpMultiAreaComboPopup',
    /**
     * 初始化
     */
    initComponent: function () {
        var me = this;
        me.editable = false;
        me.callParent();
    },
    listeners: {
        change: function (field, newValue, oldValue, eOpts) {
            var me = this;
            var entity = me.up("form").SIEView.getData();

            if (entity) {

                var locEditor = SIE.Web.EMS.InventoryPlans.InventoryScriptsAction.findEditor(me.up("form"), "StorageLocations");
                if (locEditor.getValue() != "" && locEditor.getValue() != null) {
                    entity.setStorageLocationIds(null);
                    entity.setStorageLocations(null);
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

            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            }
            else {
                entity = me.up("container").context.record;
            }

            if (entity.data.StorageAreaIds != "" && entity.data.StorageAreaIds != null) {
                entity.data.StorageAreaIds.split(me.separator).forEach(function (item) {
                    if (item) {
                        me._sourceViewSelectItems.push(parseFloat(item));
                    }
                });
            }
            me._createLayout(field);
        }
    },

    setQueryCriteria: function (dialogView, data) {
        //隐藏查询条件的清除按钮
        var clearCommand = dialogView._relations[0]._target._commands.items.first(function (p) {
            return p.meta.command == "SIE.cmd.ClearCondition";
        });

        if (clearCommand) {
            var Id = clearCommand.meta.id;
            document.getElementById(Id).style.display = "none";
        }

        var criteria = dialogView._relations[0]._target.getData();
        if (data.Warehouses != null && data.Warehouses != "") {
            criteria.setWarehouses(data.Warehouses);
        }
    },
});