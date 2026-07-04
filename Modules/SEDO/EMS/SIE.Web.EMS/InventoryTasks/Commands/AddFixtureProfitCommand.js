SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.AddFixtureProfitCommand', {
    meta: { text: "新增盘盈", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        //主表盘点状态为【盘点中、初盘完成、复盘中】才可点击
        var parent = view._parent.getCurrent();
        if (parent == null) {
            return false;
        }
        if (parent.data.InventoryTaskStatus !== 20 && parent.data.InventoryTaskStatus !== 30 && parent.data.InventoryTaskStatus !== 40) {
            return false;
        }
        return view.canAddItem();
    },
    execute: function (view, source) {
        var me = this;
        var parent = view._parent.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.InventoryTasks.ViewModels.AddFixtureProfitViewModel",
            module: view._parent.module,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                var entity = new detailView._view._model();
                entity.setInventoryTaskId(parent.getId());
                detailView._view.setData(entity);
                detailView._view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, detailView._view);
                var win = SIE.Window.show({
                    title: "新增盘盈".t(),
                    width: '70%',
                    height: '60%',
                    items: detailView.getControl(),
                    id: "AddFixtureProfitCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var info = detailView._view.getCurrent().data;
                            if (info.GenerateSn != true && info.Sn ==="" && info.ManageMode == 5) {
                                SIE.Msg.showError("请输入序列号".t());
                                return false;
                            }

                            var signdata = {
                                command: me.meta.command,
                                entityType: me.view.model,
                                parentType: me.view.getParent() ? me.view.getParent().model : ""
                            }

                            SIE.invokeDataQuery({
                                method: 'AddFixtureProfit',
                                params: [info],
                                action: 'queryer',
                                type: 'SIE.Web.EMS.InventoryTasks.InventoryTaskDataQueryer',
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
    },
    onEntityPropertyChanged: function (e) {
        
    }
});