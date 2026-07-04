Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Scripts.MultiEmployeeComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.MultiEmployeeComboPopup',
        /**
    * 确定事件
    * @param btn--
    * @returns
    */initComponent: function () {
        var me = this;
        me.callParent();
        me.multiSelect = 'MULTI';
        me.separator = ',';
    },

    //setMULTIValue: function () {
    //    var me = this;
    //    var displayVal = "";
    //    var valueVal = "";

    //    //获取当前选择框的展示值
    //    me._targetSelectItems.items.forEach(function (model) {
    //        displayVal += me.separator + model.data[me.displayField];
    //        valueVal += me.separator + model.data[me.valueField];
    //    });

    //    displayVal = displayVal.substring(me.separator.length);
    //    valueVal = valueVal.substring(me.separator.length);

    //    var entity;
    //    if (!me.up("form"))
    //        entity = me.up("container").context.record;
    //    else
    //        entity = me.up("form").SIEView.getData();

    //    if (entity) {
    //        entity.setRepairEmployees(displayVal);
    //        entity.setRepairEmployeeIds(valueVal);
    //    }

    //    me.checkChange();

    //    me.dirty = true;
    //},

    cancelSetValue: function () {
        var entity;
        var me = this;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
    },

    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            var entity;
            me.setMULTIValue();
            me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.cancelSetValue();
            me.isCanceling = true;
            return true;
        }
    },

    _createLayout: function (field) {
        var me = this;
        if (!me.model)
            SIE.Msg.showWarning('请设置数据关联实体'.t());

        var entity;
        if (!me.up("form"))
            entity = me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();

        SIE.AutoUI.getMeta({
            model: me.model,
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (block) {
                var mainBlock;
                var queryId;

                if (block.mainBlock)
                    mainBlock = block.mainBlock;
                else
                    mainBlock = block;

                var gridConfig = mainBlock.gridConfig;

                me._queryBlockProcess(block);
                me._gridBlockProcess(block);

                gridConfig.selModel = {
                    injectCheckbox: 0, //checkbox位于哪一列，默认值为0
                    selType: 'checkboxmodel', //checkbox
                    checkOnly: true, //只能通过checkbox选择
                    mode: 'MULTI' //(multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
                };

                var view = SIE.AutoUI.generateAggtControl(block);
                var dialogView = view._view;
                var queryView = dialogView.getRelations()[0].getTarget();

                if (queryView) {
                    var clearCommand = queryView._commands.items.first(function (p) {
                        return p.meta.command == "SIE.cmd.ClearCondition";
                    });
                    if (clearCommand) {
                        queryId = clearCommand.meta.id;
                    }

                    var queryEntity = queryView.getData();

                    //设置设备 为 表单的中的设备ID
                    queryEntity.setEquipAccountId(entity.getEquipAccountId());
                    queryEntity.setEquipAccountId(entity.getEquipAccountId());
                    queryEntity.setEquipAccountId_Display(entity.getEquipAccountId_Display());
                    queryEntity.setEquipAccountName(entity.getEquipAccountId_Display());
                }

                me._popupWin(view, me.inputEl);

                dialogView.loadData({
                    callback: function (res) {
                        if (queryId) {
                            document.getElementById(queryId).style.display = "none";
                        }
                    }
                });

                me._layouted = true;
            }
        });
    },

    /**
     * 查询块设置-只读为false
     * @param block 块配置
     */
    _queryBlockProcess: function (block) {
        if (block.surrounders && block.surrounders.length) {
            var surround = block.surrounders[0];
            var items = surround.mainBlock.formConfig.items;
            for (var i = 0; i < items.length; ++i) {
                var item = items[i];

                if (item.name == "EquipAccountId") {
                    item.readOnly = true;
                } else {
                    item.readOnly = false;
                }
            }
        }
    },
});