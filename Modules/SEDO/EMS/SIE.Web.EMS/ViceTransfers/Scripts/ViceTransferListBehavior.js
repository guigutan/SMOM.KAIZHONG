Ext.define('SIE.Web.EMS.ViceTransfers.Scripts.ViceTransferListBehavior',
    {
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            var entity = view.getData();
            if (entity)
                view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);

            var gridPanel = view.getControl();
            var grid = gridPanel.actionables.length > 0 ? gridPanel.actionables[0].grid : gridPanel.view.grid;
            grid.mon(grid, 'rowclick', me.rowclick, view);
            SIE.invokeDataQuery({
                method: 'GetViceTransferApproval',
                params: [],
                action: 'queryer',
                type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        var info = res.Result.data.items[0].data;
                        //撤回按钮
                        var cmdCName = "SIE.Web.EMS.ViceTransfers.Commands.CancelCommand";
                        //审核按钮
                        var cmdEName = "SIE.Web.EMS.ViceTransfers.Commands.ApprovalCommand";

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
        _onEntityPropertyChanged: function (e) {
            var data = e.entity;
            if (e.property.length > 0) {

            }
        },

        rowclick: function (g, record, element, rowIndex, e, eOpts) {
            var me = this;
            var tabs = g.up().SIEView._children;
            var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
            if (!tabPanel) return;
            var tabPanelItems = tabPanel.tabBar.items.items;
            var currentTabModel = tabPanel.getActiveTab().config.items.SIEView.model;
            var flag = false;
            var sparePart = "SIE.EMS.ViceTransfers.ViceTransferSparePart";
            var fixture = "SIE.EMS.ViceTransfers.ViceTransferFixture";

            var sparePartDetail = "SIE.EMS.ViceTransfers.ViceTransferSparePartDetail";
            var fixtureDetail = "SIE.EMS.ViceTransfers.ViceTransferFixtureDetail";
            var i = 0;
            var showIndex = -1;
            Ext.each(tabPanelItems, function (item) {
                var itemModel = tabPanel.items.items[i].items.items[0].SIEView.model;
                i++;
                if (itemModel == sparePart) {
                    record.data.ViceAssetObject == 10 ? item.show() : item.hide();
                }
                if (itemModel == sparePartDetail) {
                    record.data.ViceAssetObject == 10 ? item.show() : item.hide();
                }
                if (itemModel == fixtureDetail) {
                    record.data.ViceAssetObject == 20 ? item.show() : item.hide();
                }
                if (itemModel == fixture) {
                    record.data.ViceAssetObject == 20 ? item.show() : item.hide();
                }
                if (!item.isHidden()) {
                    if (currentTabModel == itemModel) {
                        showIndex = (i - 1);
                    }
                    else if (showIndex < 0)
                    {
                        showIndex = (i - 1);
                    }
                }

            });
            tabPanel.setActiveTab(showIndex);
        }
    });