SIE.defineCommand('SIE.Web.EMS.EquipMaint.Equipments.Accounts.Commands.MaintainAddEquipCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.Equipments.EquipAccounts.EquipAccount' }
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var ResourceId = view.getParent().getCurrent().data.ResourceId;
        var ResourceId_Display = view.getParent().getCurrent().data.ResourceId_Display;
        SIE.AutoUI.getMeta({
            model: 'SIE.Equipments.EquipAccounts.EquipAccountSelect',
            viewGroup: 'MaintainPlanBatchAddList',
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                me._queryBlockProcess(blocks);
                me._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                                
                me._popupWin(ui, source);
                me.cloneStore = view.getData();
                me._reloadTargetViewData();

                //选择了产线则只筛选与产线相关的设备（状态为使用中）                
                ui._view._relations[0]._target._current.setEnableUseState("5;25;40"); // 使用中，待验收，维修
                ui._view._relations[0]._target._current.setResourceId(ResourceId);
                ui._view._relations[0]._target._current.setResourceId_Display(ResourceId_Display);

                var accountUseState = ui._view._relations[0]._target.getControl().getForm().findField("AccountUseState");
                if (accountUseState) {
                    accountUseState.setReadOnly(true);
                }
                if (ui._view._relations[0]._target != null) {
                    ui._view._relations[0]._target.tryExecuteQuery();
                }
                //隐藏查询条件的清除按钮
                var clearCommand = ui._view._relations[0]._target._commands.items.first(function (p) {
                    return p.meta.command == "SIE.cmd.ClearCondition";
                });

                if (clearCommand) {
                    var Id = clearCommand.meta.id;
                    document.getElementById(Id).style.display = "none";
                }


            }
        });
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var lists = me._ownerView.getData().getData().items;
            selections.forEach(function (sel) {                
                if (me._sourceViewSelectItems.indexOf(sel.getId()) === -1)
                    lists.push(sel);
            });
            me._ownerView.getData().setData(lists);
            win.close();
        }
    },
    _popupWin: function (ui, source) {
        /// <summary>
        /// 弹窗口
        /// </summary>
        /// <param name="ui" type="type"></param>
        /// <param name="source" type="type"></param>
        var me = this;
        me._targetView = ui._view;
        if (me.win && me.win.animateTarget == source) {
            return;
        }
        //弹窗
        me.win = SIE.Window.show({
            title: '选择'.t() + me._targetView.label.t(),
            items: ui.getControl(),
            width: 1400,
            height: 520,
            listeners: {
                close: function () {
                    me.lastClickTime = 0;
                }
            },
            //buttons: ['确定', '关闭'], //自定义按钮名称
            callback: function (btn) {
                if (btn === '确定'.t()) {
                    var elapsed = Ext.now() - me.lastClickTime;
                    var interval = me.getExecuteInterval();
                    if (elapsed >= interval) {
                        me.lastClickTime = Ext.now();
                        if (me._targetSelectItems.keys.length > 0) {
                            me.save(me.win);
                            return false; //阻止窗口关闭，在save中根据返回结果处理
                        } else {
                            SIE.Msg.showWarning('没有可提交的数据'.t());   //没有选择数据点击确定时，窗口直接关闭了
                            return false;
                        }
                    }
                    return false;
                }
            }
        });
        me.setGridListeners();
        me._targetSelectItems = { items: [], keys: [] };
    },
});
