SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.AddProfitCommand', {
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
            model: "SIE.EMS.InventoryTasks.ViewModels.AddProfitViewModel",
            module: view._parent.module,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                var entity = new detailView._view._model();
                entity.setInventoryTaskId(parent.getId());
                entity.setFactoryId(parent.getFactoryId());
                entity.setTaskManageDeptName(parent.getManageDeptId_Display());
                entity.setAddProfitUIState(30);
                detailView._view.setData(entity);
                detailView._view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, detailView._view);
                var win = SIE.Window.show({
                    title: "新增盘盈",
                    width: '70%',
                    height: '60%',
                    items: detailView.getControl(),
                    id: "AddProfitCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var info = detailView._view.getCurrent().data;
                            if (info.AddProfitUIState === 30) {
                                SIE.Msg.showError("请输入设备编码或勾选无设备编码".t());
                                return false;
                            }

                            var signdata = {
                                command: me.meta.command,
                                entityType: me.view.model,
                                parentType: me.view.getParent() ? me.view.getParent().model : ""
                            }

                            SIE.invokeDataQuery({
                                method: 'AddProfit',
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
        if (e.property === 'NoHaveCode') {
            if (e.value === true) {
                e.entity.setAddProfitUIState(20);
                e.entity.setManageDeptName(e.entity.data.TaskManageDeptName);
                e.entity.setEquipmentCode(null);
                e.entity.setEquipmentName(null);
                e.entity.setAccountUseState(null);
                e.entity.setAccountState(null);
                e.entity.setTypeCategory(null);
                e.entity.setEquipTypeId(null);
                e.entity.setEquipModelId(null);
                e.entity.setEquipModelName(null);
                e.entity.setSpecifications(null);
                e.entity.setUseLevel(null);
                e.entity.setUseDeptId(null);
                e.entity.setUserId(null);
                e.entity.setRealWorkShopId(null);
                e.entity.setRealResourceId(null);
                e.entity.setRealWarehouseId(null);
                e.entity.setStorageLocationId(null);
                e.entity.setRealLocation(null);
                e.entity.setPhotoFilePath(null);
            } else if (e.entity.getEquipmentCode() == "") {
                e.entity.setAddProfitUIState(30);
            }
        }
    }
});