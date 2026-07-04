SIE.defineCommand('SIE.Web.EMS.InventoryPlans.Commands.SelectSparePartCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择备件", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'SparePartId',
            targetClassName: 'SIE.EMS.SpareParts.SparePart'
        }
    },
    selectedItems: [],
    canExecute: function (view) {
        var parent = view.getParent().getCurrent();
        if (parent == null || parent.data == null) {
            return false;
        }
        if (parent.isDirty() || parent.getApprovalStatus() != 10) {
            return false;
        }
        return true;
    },


    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var items = [];
            SIE.each(selections.items, function (sel) {
                var id = sel.getId();
                if (me._sourceViewSelectItems.indexOf(id) === -1) {
                    var item = {
                        InventoryPlanId: me._sourceId,
                        SparePartId: id,
                    };
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
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    var criteria = dialogView._relations[0]._target.getData();

                    //查询实体赋值
                    criteria.setState(1);

                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
});
