Ext.define('SIE.Web.EMS.Control.IpSpMultiLocComboPopup', {
    extend: 'SIE.Web.EMS.Control.IdLinkComboPopup',
    alias: 'widget.IpSpMultiLocComboPopup',
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
            if (me.up("form"))
                entity = me.up("form").SIEView.getData();
            else
                entity = me.up("container").context.record;
            if (entity.data.StorageLocationIds != "" && entity.data.StorageLocationIds != null) {
                entity.data.StorageLocationIds.split(me.separator).forEach(function (item) {
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

        if (data.StorageAreaIds != null && data.StorageAreaIds != "") {
            criteria.setAreaIds(data.StorageAreaIds);
        }
    },
});