Ext.define('SIE.Web.EMS.InventoryTasks.InventoryTaskListBehavior', {
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        me.view = view;
        var grid = view.getControl().SIEView.getControl().ownerGrid;
        view._children[0].getControl().ownerCt.ownerCt.tabBar.items.items.forEach(item => {
            item.up().up().up().hide();
            item.hide();
        });
        grid.mon(grid, 'rowclick', me.rowclick, me);

    },
    rowclick: function (g, record, element, rowIndex, e, eOpts) {
        var mainMe = this;
        var me = this.view;
        var tabs = g.up().SIEView._children;
        var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
        if (!tabPanel) return;
        var tabPanelItems = tabPanel.tabBar.items.items;

        var currentTabModel = tabPanel.getActiveTab().config.items.SIEView.model;

        //设备清单
        var inventoryTaskEquipment = "SIE.EMS.InventoryTasks.InventoryTaskEquipment";
        //设备盘点人
        var inventoryCounter = "SIE.EMS.InventoryTasks.InventoryTaskCounter";
        var inventoryFixtureCounter = "SIE.EMS.InventoryTasks.InventoryTaskFixtureCounter";
        var inventoryTaskFixtureIdAccount = "SIE.EMS.InventoryTasks.InventoryTaskFixtureIdAccount";
        var InventoryTaskFixtureEncode = "SIE.EMS.InventoryTasks.InventoryTaskFixtureEncode";

        var inventoryPlanEquipment = "SIE.EMS.InventoryPlans.InventoryPlanEquipment";
        var inventoryPlanFixture = "SIE.EMS.InventoryPlans.InventoryPlanFixture";

        var inventoryTaskSparePartScope = "SIE.EMS.InventoryTasks.InventoryTaskSparePartScope";
        var inventoryTaskSparePartList = "SIE.EMS.InventoryTasks.InventoryTaskSparePart";
        var inventoryTaskSparePartDetailList = "SIE.EMS.InventoryTasks.InventoryTaskSparePartDetail";
        var inventoryTaskSparePartCounterList = "SIE.EMS.InventoryTasks.InventoryTaskSparePartCounter";

        var isChange = false;

        var i = 0;
        Ext.each(tabPanelItems, function (item) {
            var itemModel = tabPanel.items.items[i].items.items[0].SIEView.model;
            i++;

            item.up().up().up().show();

            //资产类型 10 设备；20 备件； 30 工治具
            //设备盘点人
            if (itemModel == inventoryCounter) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }
            //设备盘点人
            if (itemModel == inventoryPlanEquipment) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }

            if (itemModel == inventoryTaskEquipment) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }

            if (itemModel == inventoryPlanFixture) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }
            if (itemModel == inventoryFixtureCounter) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }
            if (itemModel == inventoryTaskFixtureIdAccount) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }
            if (itemModel == InventoryTaskFixtureEncode) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }

            // 备件盘点范围
            if (itemModel == inventoryTaskSparePartScope) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }

            //备件汇总
            if (itemModel == inventoryTaskSparePartList) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }

            //备件明细
            if (itemModel == inventoryTaskSparePartDetailList) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }

            //备件盘点人
            if (itemModel == inventoryTaskSparePartCounterList) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }

            //当前Tab页要隐藏
            if (currentTabModel == itemModel && item.isHidden()) {
                isChange = true;
            }
        });

        //当前Tab页要隐藏,重新显示不同类型的第一个页签
        if (isChange) {
            var showIndex = -1;
            if (record.data.InventoryAssetObject == 10) {
                showIndex = 0;
            } else if (record.data.InventoryAssetObject == 20) {
                showIndex = 2;
            } else if (record.data.InventoryAssetObject == 30) {
                showIndex = 1;
            }

            tabPanel.setActiveTab(showIndex);
        }

        //Closed = 70, [Label("关闭")]        
        var isReadOnly = (record.data.InventoryTaskStatus == 70 || record.data.InventoryTaskStatus==60);
        var btns = me.getControl().SIEView.getControl().ownerGrid.query('button');
        if (isReadOnly) {
            btns.forEach(function (btn) {
                mainMe.btndisable(btn, isReadOnly);
            });
        }

        me.getChildren().forEach(function (children) {
            if (isReadOnly) {
                children.setIsReadonly(isReadOnly);//设置子节点文本域只读//设置按钮禁用
            }
            if (children.model != inventoryPlanEquipment
                && children.model != inventoryPlanFixture
                && children.model != inventoryTaskSparePartScope) {
                var children_btns = children.getControl().getView().grid.query('button');
                if (isReadOnly) {

                    children_btns.forEach(function (btn) {
                        mainMe.btndisable(btn, isReadOnly);
                    });

                    if (children.model == inventoryCounter || children.model.inventoryFixtureCounter) {
                        children.getData().getData().items.forEach(function (data) {
                            data.data.IsReadOnly = isReadOnly;
                        });
                    }
                }
                var selectionModel_child = children.getControl().getSelectionModel();
                if (selectionModel_child) {
                    selectionModel_child.mon(selectionModel_child, "selectionchange", function (selmodel, selection) {
                        if (children.getParent().getSelection().length > 0) {
                            var selectData = children.getParent().getSelection()[0].getData();
                            var parentReadOnly = (selectData.InventoryTaskStatus == 70 || selectData.InventoryTaskStatus == 60);
                            children.setIsReadonly(parentReadOnly);

                            var selbtns = children.getParent().getControl().SIEView.getControl().ownerGrid.query('button');
                            if (parentReadOnly) {
                                selbtns.forEach(function (btn) {
                                    mainMe.btndisable(btn, parentReadOnly);
                                });
                            }
                            children_btns.forEach(function (btn) {
                                if (parentReadOnly) {
                                    mainMe.btndisable(btn, parentReadOnly);
                                }
                            });
                        }
                    });
                }

            } else {
                var form = children.getControl();////dataChanged"
                form.mon(form.SIEView, "currentChanged", function () {

                    setTimeout(function () {
                        if (children.getParent().getSelection()[0] != undefined) {
                            var selectData = children.getParent().getSelection()[0].getData();
                            var parentReadOnly = (selectData.InventoryTaskStatus == 70 || selectData.InventoryTaskStatus == 60);

                            var selbtns = children.getParent().getControl().SIEView.getControl().ownerGrid.query('button');
                            if (parentReadOnly) {
                                selbtns.forEach(function (btn) {
                                    mainMe.btndisable(btn, parentReadOnly);
                                });
                            }
                        }
                    }, 50);
                }, me);
            }
            var tabpanel = children.getControl().up('tabpanel');
            if (tabpanel) {
                tabpanel.mon(tabpanel, 'tabchange', function () {
                    if (children.getParent().getSelection()[0] != undefined) {
                        var selectData = children.getParent().getSelection()[0].getData();
                        var parentReadOnly = (selectData.InventoryTaskStatus == 70 || selectData.InventoryTaskStatus ==  60);
                        if (parentReadOnly) {
                            children.setIsReadonly(parentReadOnly);//设置子节点文本域只读//设置按钮禁用
                        }
                        if (children_btns != null) {
                            children_btns.forEach(function (btn) {
                                if (parentReadOnly) {
                                    mainMe.btndisable(btn, parentReadOnly);
                                }
                            });
                        }
                        var selbtns = children.getParent().getControl().SIEView.getControl().ownerGrid.query('button');
                        if (parentReadOnly) {
                            selbtns.forEach(function (btn) {
                                mainMe.btndisable(btn, parentReadOnly);
                            });
                        }
                    }
                });
            }
        });


    },
    btndisable: function (btn, isReadOnly) {
        if (btn.tooltip == "添加" || btn.tooltip == "修改" || btn.tooltip == "删除" || btn.tooltip == "保存" || btn.tooltip == "下达"
            || btn.tooltip == "初盘完成" || btn.tooltip == "盘点完成" || btn.tooltip == "上传图片"
            || btn.tooltip == "查看图档" || btn.tooltip == "关闭" || btn.tooltip == "新增盘盈"
            || btn.tooltip == "导出" || btn.tooltip == "导入"

        ) {
            if (isReadOnly) {
                btn.disable();
            }
        }

    },


});