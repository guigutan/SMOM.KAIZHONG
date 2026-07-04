SIE.defineCommand('SIE.Web.EMS.InventoryBalances.Commands.EditInventoryCauseCommand', {
    meta: { text: "编辑", group: "edit", iconCls: "icon-TableEdit icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var parent = view._parent.getCurrent();
        if (parent == null) {
            return false;
        }
        if (parent.data.ApprovalStatus !== 10 && parent.data.ApprovalStatus !== 50) return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var cur = view.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.InventoryTasks.InventoryCause",
            module: view._parent.module,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                detailView._view.setData(cur);
                var win = SIE.Window.show({
                    title: "",
                    width: '70%',
                    height: '50%',
                    items: detailView.getControl(),
                    id: "EditInventoryCauseCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var info = detailView._view.getCurrent().data;

                            var signdata = {
                                command: me.meta.command,
                                entityType: me.view.model,
                                parentType: me.view.getParent() ? me.view.getParent().model : ""
                            }

                            SIE.invokeDataQuery({
                                method: 'EditInventoryCause',
                                params: [info],
                                action: 'queryer',
                                type: 'SIE.Web.EMS.InventoryBalances.InventoryBalanceDataQueryer',
                                token: view.token,
                                logInfo: signdata,
                                success: function (result) {
                                    win.close();
                                    view._parent.reloadData();
                                }
                            });
                            return false;
                        }
                    }
                });
            }
        });
    }
});