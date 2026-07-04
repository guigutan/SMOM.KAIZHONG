SIE.defineCommand('SIE.Web.MES.WorkOrders.SelectPackageRuleCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (listView) {
        var parent = listView.getParent();
        if (parent == null) return false;
        var parData = parent.getData();
        if (parData == null) return false;
        return parent != null && parData.data != null && parData.data.ProductId > 0;
    },
    execute: function (listView, source) {
        var me = this;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                ignoreCommands: true,
                isAggt: true,
                viewGroup: "ReadonlyView",
                token: listView.token,
                model: "SIE.Packages.ItemPackageRule",
                //module: "SIE.MES.WorkOrders.WorkOrder,SIE.MES",
                callback: function (res) {
                    var view = SIE.AutoUI.generateAggtControl(res);
                    var ui = view.getControl();
                    var ruleview = view;
                    //设置看不见的条件,让页面只能查工单产品的数据
                    var wodata = listView.getParent().getData().data;
                    var dialogView = view.getView();
                    if (dialogView.getRelations() && dialogView.getRelations().length > 0 && dialogView.getRelations()[0].getTarget() && view.getView().getRelations()[0].getTarget().viewGroup === 'QueryView') {
                        var queryView = dialogView.getRelations()[0].getTarget();
                        var clearCM = queryView.getCommands().items.first(function (p) { return p.meta.name == "SIE.cmd.ClearCondition" });
                        clearCM.canVisible = function () { return false; };
                        queryView.syncCmdState();
                        queryView.getData().data.ItemId = wodata.ProductId;
                        var win = SIE.Window.show({
                            title: "选择 产品包装规则".t(),
                            width: 950,
                            height: 500,
                            items: ui,
                            id: "ItemPackageRule001",
                            callback: function (btn) {
                                if (btn == "确定".t()) {
                                    var ruleId = ruleview._view.getCurrent().getData().Id;
                                    me.getProductPackRuleById(ruleId, listView, win);
                                    return false;
                                }
                            }
                        });
                        queryView.tryExecuteQuery();
                    }
                    
                }
            });
        }
    },
    /**
      * 设置选择的包装规则
      * @param id 包装规则id    
      * @param listView 列表视图    
      */
    getProductPackRuleById: function (id, listView,win) {
        var woId = listView.getParent().getData().data.Id;
        SIE.invokeDataQuery({
            method: 'ProductChangedGetPackRuleById',
            params: [id, woId],
            action: 'queryer',
            type: 'SIE.Web.MES.WorkOrders.WorkOrderDataQueryer',
            token: listView.token,
            success: function (res) {
                var ruleList = listView.getControl();
                var store = ruleList.getStore();
                store.setData(res.Result.data);
                ruleList.setStore(store);
                listView._parent.getData().dirty = true
                listView._parent.syncCmdState(listView._parent, true)
                win.close();
            }
        });
    },
});