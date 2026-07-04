Ext.define('SIE.Web.EMS.InventoryBalances.InventoryBalanceBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        SIE.invokeDataQuery({
            method: 'GetInventoryBalanceApproval',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.EMS.InventoryBalances.InventoryBalanceDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result) {
                    var info = res.Result.data.items[0].data;
                    //撤回按钮
                    var cmdCName = "SIE.Web.EMS.InventoryBalances.Commands.CancelBalanceCommand";
                    //审核按钮
                    var cmdEName = "SIE.Web.EMS.InventoryBalances.Commands.ExamineBalanceCommand";

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

        var me = this;
        me.view = view;
        var grid = view.getControl().SIEView.getControl().ownerGrid;
        grid.mon(grid, 'rowclick', me.rowclick, me);

    },
    rowclick: function (g, record, element, rowIndex, e, eOpts) {
        
        var tabs = g.up().SIEView._children;
        var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
        if (!tabPanel) return;
        var tabPanelItems = tabPanel.tabBar.items.items;
        var currentTabModel = tabPanel.getActiveTab().config.items.SIEView.model;
        
        var inventoryTaskFixtureIdAccount = "SIE.EMS.InventoryTasks.InventoryTaskFixtureIdAccount";
        var InventoryTaskFixtureEncode = "SIE.EMS.InventoryTasks.InventoryTaskFixtureEncode";

        var inventoryTaskEquipment = "SIE.EMS.InventoryTasks.InventoryTaskEquipment";
        var inventoryCause = "SIE.EMS.InventoryTasks.InventoryCause";
        
        var inventoryTaskSparePartList = "SIE.EMS.InventoryTasks.InventoryTaskSparePart";
        var inventoryTaskSparePartDetailList = "SIE.EMS.InventoryTasks.InventoryTaskSparePartDetail"; 
        var inventoryTaskSparePartDiffList = "SIE.EMS.InventoryTasks.InventoryTaskSparePartDiff";

        var isChange = false;
        var i = 0;
        
        Ext.each(tabPanelItems, function (item) {
            
            var itemModel = tabPanel.items.items[i].items.items[0].SIEView.model;
            var viewGroup = tabPanel.items.items[i].items.items[0].SIEView.viewGroup;

            i++;
            if (itemModel == inventoryTaskFixtureIdAccount) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }

            if (itemModel == InventoryTaskFixtureEncode) {
                record.data.InventoryAssetObject == 30 ? item.show() : item.hide();
            }

            if (itemModel == inventoryTaskEquipment) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }

            if (itemModel == inventoryCause) {
                record.data.InventoryAssetObject == 10 ? item.show() : item.hide();
            }

            //原因分析
            if (viewGroup == "CauseAnalysisView") {
                (record.data.InventoryAssetObject == 30 || record.data.InventoryAssetObject == 20) ? item.show() : item.hide();
            }
            
            //备件汇总
            if (itemModel == inventoryTaskSparePartList) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }

            //备件明细
            if (itemModel == inventoryTaskSparePartDetailList) {
                record.data.InventoryAssetObject == 20 ? item.show() : item.hide();
            }

            //备件盘点差异
            if (itemModel == inventoryTaskSparePartDiffList) {
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
                showIndex = 4;
            } else if (record.data.InventoryAssetObject == 30) {
                showIndex = 2;
            }

            tabPanel.setActiveTab(showIndex);
        }
    }
});