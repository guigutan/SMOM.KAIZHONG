SIE.defineCommand('SIE.Web.EMS.Checks.Commands.EditChkSelEquipAccountCheckProjectCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'EquipAccountCheckProjectId', targetClassName: 'SIE.EMS.Equipments.EquipAccountCheckProject' }
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck" },
    _sourceId: null,

    canExecute: function (view) {
        var pv = view.getParent();
        if (!pv) { return true; }
        var entity = pv.getCurrent();
        var result = entity !== null;
        if (result) {
            result = pv.getSelection().length === 1;
        }
        if (pv.getSelection().length > 1) return false;
        return result;
    },

    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var items = [];
            SIE.each(selections, function (sel) {
                var equipAssetId = sel.getEquipAssetId();
                var checkProjectDetailId = sel.getCheckProjectDetailId();
                if (me._sourceViewSelectItems.indexOf(id) === -1) {
                    var item = { CheckPlanId: me._sourceId, EquipAssetId: equipAssetId, checkProjectDetailId:checkProjectDetailId };
                    items.push(item);
                }
            });
            if (items.length > 0) {
                indata = items;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        win.close();
                        me._ownerView.loadChildData(true);
                    }
                }, me._ownerView);
                return true;
            }
        }
        Ext.Msg.alert('提示'.t(), '没有可提交的数据'.t());
    },

    execute: function (view, source) {
        var me = this;

        me._sourceId = me.view.getParent().getCurrent().getData().Id;
        var model = view.model;
        if (model) {
            SIE.AutoUI.getMeta({
                model: me.dataParams.targetClassName,
                viewGroup: 'CheckPlanProject',
                ignoreChild: true,
                ignoreCommands: true,
                isReadonly: true,
                ignoreQuery: false,
                isAggt: true,
                callback: function (res) {
                    var blocks = res;
                    me._queryBlockProcess(blocks);
                    me._gridBlockProcess(blocks);
                    //隐藏非指定的查询条件
                    blocks.surrounders[0].mainBlock.formConfig.items.forEach(function (item) {
                        if (item.name === 'EquipAccountId' || item.name === 'Level' || item.name === 'CycleType')
                            item.hidden = true;
                    });
                    var ui = SIE.AutoUI.generateAggtControl(blocks);

                    //禁用清除过滤条件按钮
                    if(ui._view.getConditionView()._commands && ui._view.getConditionView()._commands.items){
                        ui._view.getConditionView()._commands.items.forEach(function (item) {
                            if (item.meta.name === 'SIE.cmd.ClearCondition')
                                item.canExecute = function(view){return false;}
                        });  
                    }

                    //设定默认值
                    var store = ui._view.getConditionView().getData();
                    store.setEquipAccountId(me._equipAccountId);//台账ID
                    store.setLevel(0);//级别
                    var cycleType = me.view.getParent().getCurrent().getData().CheckCycleType === 0 ? 1 : 0;
                    store.setCycleType(cycleType);//周期类型
                    me._popupWin(ui, source);
                    me.cloneStore = view.getData(); //克隆数据store
                    me._reloadTargetViewData();
                }
            });
        }

    }
});
