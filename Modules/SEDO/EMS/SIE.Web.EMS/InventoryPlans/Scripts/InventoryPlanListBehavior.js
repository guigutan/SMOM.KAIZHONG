Ext.define('SIE.Web.EMS.InventoryPlans.InventoryPlanListBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {

        var me = this;
        var gridPanel = view.getControl();
        var grid = gridPanel.actionables.length > 0 ? gridPanel.actionables[0].grid : gridPanel.view.grid;
        view._children[0].getControl().ownerCt.ownerCt.tabBar.items.items.forEach(item => {
            item.up().up().up().hide();
            item.hide();
        });
        grid.mon(grid, 'rowclick', me.rowclick, view);
        SIE.invokeDataQuery({
            method: 'GetInventoryPlanApproval',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.InventoryPlans.InventoryPlanDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result) {
                    var info = res.Result.data.items[0].data;
                    //撤回按钮
                    var cmdCName = "SIE.Web.EMS.InventoryPlans.Commands.CancelInventoryPlanCommand";
                    //审核按钮
                    var cmdEName = "SIE.Web.EMS.InventoryPlans.Commands.ExamineInventoryPlanCommand";

                    //根据配置项【是否启用审批】，【是】则显示该按钮，【否】则隐藏按钮
                    if (info.EnableAudit === false) {
                        let cmd = view.getCmdControl(cmdCName);
                        if (cmd) {
                            cmd.setHidden(true);
                            view._commands.removeAtKey(cmdCName);
                        }
                        let cmdE = view.getCmdControl(cmdEName);
                        if (cmdE) {
                            cmdE.setHidden(true);
                            view._commands.removeAtKey(cmdEName);
                        }
                    }
                    //配置项【是否启用审批流程】，当配置项的值为【是】时，不显示该按钮
                    if (info.EnableApproval) {
                        let cmdE = view.getCmdControl(cmdEName);
                        if (cmdE) {
                            cmdE.setHidden(true);
                            view._commands.removeAtKey(cmdEName);
                        }
                    }
                }
            }
        });

       
    },
    rowclick: function (g, record, element, rowIndex, e, eOpts) {
        var me = this;
        var tabs = g.up().SIEView._children;
        var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
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

        //审核记录
       // var workRecord = "SIE.Equipments.WorkFlows.WorkFlowRecord";
        var inbentoryTask = "SIE.EMS.InventoryTasks.InventoryTask";

        var i = 0;
        var isChange = false;

        Ext.each(tabPanelItems, function (item) {
            var itemModel = tabPanel.items.items[i].items.items[0].SIEView.model;
            i++;

            item.up().up().up().show();
            
            if (itemModel == inventoryCounter) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }

            if (itemModel == inventoryPlanEquipment) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }
            //设备清单
            if (itemModel == equipmentList) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }

            if (itemModel == inventoryPlanFixture) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }

            if (itemModel == inventoryFixtureCounter) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }

            //备件盘点范围
            if (itemModel == inventoryPlanSparePart) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }
            if (itemModel == sparePartList) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }
            if (itemModel == inbentoryTask) {
                item.show();
            }
/*            if (itemModel == workRecord) {
                item.show();
            }*/

            //当前Tab页要隐藏
            if (currentTabModel == itemModel && item.isHidden()) {
                isChange = true;
            }
        });


        //当前Tab页要隐藏,重新显示不同类型的第一个页签
        if (isChange) {
            var showIndex = -1;

            //10 设备；20 备件；30 工治具
            if (e.record.data.InventoryAssetObject == 10) {
                showIndex = 0;
            } else if (e.record.data.InventoryAssetObject == 20) {
                showIndex =1;
            } else if (e.record.data.InventoryAssetObject == 30) {
                showIndex = 2;
            }

            tabPanel.setActiveTab(showIndex);
        }
        var isReadOnly = true; //record.getData().CloseRemark != "" || (record.getData().ApprovalStatus !== 10 && record.getData().ApprovalStatus !== 50);
        me.getChildren().forEach(function (children) {

            isReadOnly = true;
            children.setIsReadonly(isReadOnly);//设置子节点文本域只读//设置按钮禁用
            if (children.model == inventoryCounter || children.model == inventoryFixtureCounter)
            {
                var children_btns = children.getControl().getView().grid.query('button');
                if (isReadOnly) {

                    children_btns.forEach(function (btn) {
                        btn.disable();
                    });
                    var selectionModel_child = children.getControl().getSelectionModel();
                    if (selectionModel_child) {
                        selectionModel_child.mon(selectionModel_child, "selectionchange", function (selmodel, selection) {
                            if (children.getParent().getSelection()[0] != undefined) {
                                var selectData = children.getParent().getSelection()[0].getData();
                                var parentReadOnly = true; //selectData.CloseRemark != "" || (selectData.ApprovalStatus !== 10 && selectData.ApprovalStatus !== 50);
                                children.setIsReadonly(parentReadOnly);

                                children_btns.forEach(function (btn) {
                                    if (parentReadOnly) {
                                        btn.disable();
                                    }
                                });
                                children.getData().getData().items.forEach(function (data) {
                                    data.data.IsReadOnly = parentReadOnly;
                                });
                            }
                        });
                    }
                }

                var tabpanel = children.getControl().up('tabpanel');
                if (tabpanel) {
                    tabpanel.mon(tabpanel, 'tabchange', function () {
                        var selectData = children.getParent().getSelection()[0].getData();
                        var parentReadOnly = true;//selectData.CloseRemark != ""||(selectData.ApprovalStatus !== 10 && selectData.ApprovalStatus !== 50);
                        children.setIsReadonly(parentReadOnly);//设置子节点文本域只读//设置按钮禁用
                        if (children_btns != null) {
                            children_btns.forEach(function (btn) {
                                if (parentReadOnly) {
                                    btn.disable();
                                }
                            });
                        }
                    });
                }
            }
        });
    
    }
    
});