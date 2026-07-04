Ext.define('SIE.Web.EMS.InventoryPlans.InventoryPlanBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        view.IsAddNew = false;
        if (entity.phantom) {
            SIE.invokeDataQuery({
                method: 'GetNewInventoryPlan',
                params: [],
                action: 'queryer',
                type: 'SIE.Web.EMS.InventoryPlans.InventoryPlanDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        var info = res.Result.data.items[0].data;
                        entity.setPlanNo(info.PlanNo);
                        entity.setApprovalStatus(info.ApprovalStatus);
                        entity.setApplyDate(info.ApplyDate);
                        entity.setInventoryExecuteType(info.InventoryExecuteType);
                        entity.setPlanEndDate(info.PlanEndDate);
                        entity.setInventoryAssetObject(info.InventoryAssetObject);
                        var userInfo = CRT.Context.GlobalContext.getContext('userInfo');
                        entity.setResponsibleId_Display(userInfo.Name);
                        entity.setResponsibleId(userInfo.EmployeeId);
                    }
                }
            });
            view.IsAddNew = true;
        }

        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        if (!view.IsAddNew) {
            var temp = entity.getInventoryAssetObject();
            entity.setInventoryAssetObject(0);
            entity.setInventoryAssetObject(temp);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'FactoryId') {
            let childView = me._children.first(function (p) { return p.model === "SIE.EMS.InventoryPlans.InventoryPlanEquipment"; });
            let child = childView.getCurrent();
            child.setFactoryId(e.value);
            child.setManageDeptId(null);
            child.setUseDeptId(null);
            child.setWorkShopId(null);
        }

        if (e.property == "InventoryAssetObject") {
            //10 设备；20 备件；30 工治具
            if (e.value != 10 && e.value != 20 && e.value != 30) {
                return;
            }
            var tabPanel = this._children[0].getControl().ownerCt.ownerCt;
            if (!tabPanel) return;
            var tabPanelItems = tabPanel.tabBar.items.items;
            var currentTabModel = tabPanel.getActiveTab().config.items.SIEView.model;
            var inventoryCounter = "SIE.EMS.InventoryPlans.InventoryCounter";
            var inventoryFixtureCounter = "SIE.EMS.InventoryPlans.InventoryFixtureCounter";

            var inventoryPlanEquipment = "SIE.EMS.InventoryPlans.InventoryPlanEquipment";
            var inventoryPlanFixture = "SIE.EMS.InventoryPlans.InventoryPlanFixture";

            var inventoryPlanSparePart = "SIE.EMS.InventoryPlans.InventoryPlanSparePart";

            //设备清单
            var equipmentList = "SIE.EMS.InventoryPlans.EquipmentList";

            //备件清单
            var sparePartList = "SIE.EMS.InventoryPlans.SparePartList";

            var i = 0;
            var isChange = false;

            Ext.each(tabPanelItems, function (item) {
                var itemModel = tabPanel.items.items[i].items.items[0].SIEView.model;
                i++;

                if (itemModel == inventoryPlanEquipment) {
                    e.value == 10 ? item.show() : item.hide();
                }

                if (itemModel == inventoryCounter) {
                    e.value == 10 ? item.show() : item.hide();
                }

                //设备清单
                if (itemModel == equipmentList) {
                    e.value == 10 ? item.show() : item.hide();
                }

                if (itemModel == inventoryFixtureCounter) {
                    e.value == 30 ? item.show() : item.hide();
                }

                if (itemModel == inventoryPlanFixture) {
                    e.value == 30 ? item.show() : item.hide();
                }

                //备件盘点范围
                if (itemModel == inventoryPlanSparePart) {
                    e.value == 20 ? item.show() : item.hide();
                }

                if (itemModel == sparePartList) {
                    e.value == 20 ? item.show() : item.hide();
                }

                //当前Tab页要隐藏
                if (currentTabModel == itemModel && item.isHidden()) {
                    isChange = true;
                }
            });

            //当前Tab页要隐藏,重新显示不同类型的第一个页签
            if (isChange) {
                var showIndex = -1;

                //10 设备；20 备件；30 工治具
                if (e.value == 10) {
                    showIndex = 0;
                } else if (e.value == 20) {
                    showIndex = 1;
                } else if (e.value == 30) {
                    showIndex = 2;
                }

                tabPanel.setActiveTab(showIndex);
            }

            if (me.IsAddNew) {
                if (e.value === 10)//清空所有设备的子页签
                {
                    var inventoryCounterChildView = this._children.find(m => m.model == "SIE.EMS.InventoryPlans.InventoryCounter");
                    if (!inventoryCounterChildView) {
                        SIE.Msg.showError("界面子列表【盘点人（设备）】无权限，请配置".t());
                        return;
                    }
                    inventoryCounterChildView.getData().data.removeAll();

                    var inventoryPlanEquipmentChildView = this._children.find(m => m.model == "SIE.EMS.InventoryPlans.InventoryPlanEquipment");
                    if (!inventoryPlanEquipmentChildView) {
                        SIE.Msg.showError("界面子列表【盘点范围（设备）】无权限，请配置".t());
                        return;
                    }
                    inventoryPlanEquipmentChildView.getControl().form.reset();
                }

                if (e.value === 30)//清空所有工治具的子页签
                {
                    var inventoryFixtureCounterChildView = this._children.find(m => m.model == "SIE.EMS.InventoryPlans.InventoryFixtureCounter");
                    if (!inventoryFixtureCounterChildView) {
                        SIE.Msg.showError("界面子列表【盘点人（工治具）】无权限，请配置".t());
                        return;
                    }
                    inventoryFixtureCounterChildView.getData().data.removeAll();

                    var inventoryPlanFixtureChildView = this._children.find(m => m.model == "SIE.EMS.InventoryPlans.InventoryPlanFixture");
                    if (!inventoryPlanFixtureChildView) {
                        SIE.Msg.showError("界面子列表【盘点范围（工治具）】无权限，请配置".t());
                        return;
                    }
                    inventoryPlanFixtureChildView.getControl().form.reset();
                }

                if (e.value === 20)//清空所有备件的子页签
                {
                    var inventoryPlanSparePartChildView = this._children.find(m => m.model == "SIE.EMS.InventoryPlans.InventoryPlanSparePart");
                    if (!inventoryPlanSparePartChildView) {
                        SIE.Msg.showError("界面子列表【盘点范围（备件）】无权限，请配置".t());
                        return;
                    }
                    inventoryPlanSparePartChildView.getControl().form.reset();
                }
            }
        }
    }
});