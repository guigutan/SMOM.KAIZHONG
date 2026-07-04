Ext.define('SIE.Web.EMS.Control.IpSpMultiWhComboPopup', {
    extend: 'SIE.Web.EMS.Control.IdLinkComboPopup',
    alias: 'widget.IpSpMultiWhComboPopup',
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
                var editor = SIE.Web.EMS.InventoryPlans.InventoryScriptsAction.findEditor(me.up("form"), "StorageAreas");
                if (editor.getValue() != "" && editor.getValue() != null) {
                    entity.setStorageAreaIds(null);
                    entity.setStorageAreas(null);
                    editor.setRawValue(null);
                    editor.setValue(null);
                }

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

            if (entity.data.WarehouseIds != "" && entity.data.WarehouseIds != null) {
                entity.data.WarehouseIds.split(me.separator).forEach(function (item) {
                    if (item) {
                       
                        me._sourceViewSelectItems.push(parseFloat(item));
                    }
                });
            }

            me._createLayout(field);
        }
    },

    //设置查询条件
    setQueryCriteria: function (dialogView, data) {
        var criteria = dialogView._relations[0]._target.getData();
        //隐藏查询条件的清除按钮
        var clearCommand = dialogView._relations[0]._target._commands.items.first(function (p) {
            return p.meta.command == "SIE.cmd.ClearCondition";
        });

        if (clearCommand) {
            var Id = clearCommand.meta.id;
            document.getElementById(Id).style.display = "none";
        }
        if (criteria) {
            //只查询员工有权限的仓库
            criteria.setIsEmployeeWarehouse(true);
        }
    },
});