SIE.defineCommand('SIE.Web.EMS.DevicePurs.Commands.SelBillCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'EquipAccountId',
            targetClassName: 'SIE.Equipments.EquipAccounts.EquipAccount'
        }
    },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null;
        }
        return false;
    },

    /// <summary>
    /// 保存选择的操作列表。
    /// </summary>
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var items = [];
            SIE.each(selections.items, function (sel) {
                var id = sel.getId();
                if (me._sourceViewSelectItems.indexOf(id) === -1) {
                    var item = { SourceId: me._sourceId, DevicePurId: id };
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

    /// <summary>
    /// 加载弹窗视图的数据
    /// </summary>
    _reloadTargetViewData: function () {
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);

        var dialogView = me._targetView;

        if (me._targetView !== null) {

            var store = dialogView.getData();

            if (store !== null) {
                
                me.mon(store, 'load', me.onLoad, this);

                 //存在查询面板时
                if (dialogView._relations[0]) {
                    //隐藏清除条件的按钮
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));

                    var criteria = dialogView._relations[0]._target.getData().data;
                     
                    criteria.IsLoadAll = true;
                     
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
});