Ext.define('SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountListViewBehavior',
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
            var grid = gridPanel.actionables[0].grid;
            grid.mon(grid, 'rowclick', me.rowclick, view);
            this._disableButton(view);

        },
        _disableButton(view) {
            var cmdAddName = "SIE.Web.Equipments.EquipAccounts.Commands.AddAccountCommand";
            var cmdAddSubName = "SIE.Web.Equipments.EquipAccounts.Commands.AddChildAccountCommand";
            var cmdEditName = "SIE.Web.Equipments.EquipAccounts.Commands.EditAccountCommand";
            SIE.invokeDataQuery({
                method: 'GetUseCard',
                params: [],
                action: 'queryer',
                type: 'SIE.Web.Equipments.EquipAccounts.DataQuery.EquipAccountDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        var cmd = view.getCmdControl(cmdAddName);
                        if (cmd) {
                            cmd.setHidden(true);
                            view._commands.removeAtKey(cmdAddName);
                        }
                        var cmdSubmit = view.getCmdControl(cmdAddSubName);
                        if (cmdSubmit) {
                            cmdSubmit.setHidden(true);
                            view._commands.removeAtKey(cmdAddSubName);
                        }
                        var cmdEdit = view.getCmdControl(cmdEditName);
                        if (cmdEdit) {
                            cmdEdit.setHidden(true);
                            view._commands.removeAtKey(cmdEditName);
                        }
                    }
                }
            });

        },


        _onEntityPropertyChanged: function (e) {
            var data = e.entity;
            if (e.property.length > 0) {
                if (e.property.indexOf('QualityState') >= 0) {
                    if (data.getQualityState() == 0) {
                        data.setState(0);
                        data.setUseState(5);
                    } else if (data.getQualityState() == 1 || data.getQualityState() == 2) {
                        data.setState(1);
                        data.setUseState(0);
                    } else if (data.getQualityState() == 3) {
                        data.setState(2);
                        data.setUseState(0);
                    } else if (data.getQualityState() == 4) {
                        data.setState(2);
                        data.setUseState(20);
                    }
                }
                if (e.property.indexOf('EquipModelId') >= 0 && data.getEquipTypeIsCheck()) {
                    data.setQualityState(null);
                }
            }
        },
        rowclick: function (g, record, element, rowIndex, e, eOpts) {
            var tabs = g.up().SIEView._children;
            var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
            if (!tabPanel) return;

            var tabPanelItems = tabPanel.tabBar.items.items;
            var currentTab = tabPanel.getActiveTab().title;
            var flag = false;

            var view = record.belongsView;
            // 仪器参数
            var equipParamView = view.findChild("SIE.EMS.Equipments.Accounts.EquipParam");
            // 位置列表
            var locationView = view.findChild("SIE.Equipments.EquipAccountLocations.EquipAccountLocation");
            // 电子行业
            var electricityView = view.getChildren().first(function (e) {
                return e.viewGroup === "EISBaseDataViewGroup";
            });
            // 缸槽管理
            var slotView = view.findChild("SIE.Equipments.EquipAccounts.EquipAccountSlot");


            Ext.each(tabPanelItems, function (item) {
                //if ("校验项目" == item.title) {
                //    entity.CheckCategory != null ? item.show() : item.hide();
                //}
                if (equipParamView && equipParamView.label == item.title) {
                    entity.CheckCategory != null ? item.show() : item.hide();
                }
                else if (locationView && locationView.label == item.title) {
                    record.data.IndustryCategory == "1" ? item.show() : item.hide();
                }
                else if (electricityView && electricityView.label == item.title) {
                    record.data.IndustryCategory == "1" ? item.show() : item.hide();
                }
                //else if ("设备能力" == item.title) {
                //    record.data.IndustryCategory == "2" ? item.show() : item.hide();
                //}
                else if (slotView && slotView.label == item.title) {
                    record.data.IndustryCategory == "2" ? item.show() : item.hide();
                }
                //else if ("PCB行业基础数据" == item.title) {
                //    record.data.IndustryCategory == "2" ? item.show() : item.hide();
                //}

                if (item.isHidden() && currentTab == item.title)
                    flag = true;
            });

            if (flag)
                tabPanel.setActiveTab(0);
            else
                tabPanel.setActiveTab(currentTab);
        }
    });