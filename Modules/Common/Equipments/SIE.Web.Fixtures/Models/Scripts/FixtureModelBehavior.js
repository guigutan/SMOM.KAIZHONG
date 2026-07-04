Ext.define('SIE.Web.Fixtures.Models.Scripts.FixtureModelBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            meFixtureModelBehavior = this;
            var gridPanel = view.getControl();
            if (gridPanel.actionables) {
                var grid = gridPanel.actionables[0].grid;
                grid.mon(grid, 'rowclick', meFixtureModelBehavior.rowclick, view);
            }
        },
        onDataLoaded: function (view) {
            var entity = view.getData();
            view.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, view);
            this.setDetailsIndustryProperties(entity, entity.getData().IndustryProperties == 20);

            if (entity.belongsView && entity.belongsView.viewGroup == "DetailsView" && entity.isNew()) {
                SIE.invokeDataQuery({
                    method: 'GetCode',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.Fixtures.Models.DataQueryers.FixtureEncodeDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            entity.setCode(res.Result);
                        }
                    }
                });

            }
        },
        /**
         * onEntityPropertyChanged 属性变更事件
         * @param {*} e 参数
         */
        onEntityPropertyChanged: function (e) {
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === 'LoadingManage' && e.value === 0) {
                    entity.setOnlineHour(0);
                    entity.setMaintainEnforce(false);
                }
                if (e.property === 'IndustryProperties') {
                    if (e.value != e.oldvalue) {
                        if (entity.belongsView.viewGroup == "DetailsView") {
                            meFixtureModelBehavior.setDetailsIndustryProperties(entity, e.value === 20);
                        }
                        else
                            meFixtureModelBehavior.setDetailsIndustryProperties(entity, e.value === 20);
                    }
                }
                if (e.value == true) {
                    if (e.property === 'IsFeeder') {
                        entity.belongsView.getChildren()[2].getData().setSlotType(null);
                        entity.belongsView.getChildren()[2].getData().setIsFeeder(true);
                        entity.belongsView.getChildren()[2].getData().setIsScraper(false);
                        entity.belongsView.getChildren()[2].getData().setIsSteelNet(false);

                    }
                    if (e.property === 'IsSteelNet') {
                        entity.belongsView.getChildren()[2].getData().setSlotType(null);
                        entity.belongsView.getChildren()[2].getData().setIsFeeder(false);
                        entity.belongsView.getChildren()[2].getData().setIsScraper(false);
                        entity.belongsView.getChildren()[2].getData().setIsSteelNet(true);
                    }
                    if (e.property === 'IsScraper') {
                        entity.belongsView.getChildren()[2].getData().setSlotType(null);
                        entity.belongsView.getChildren()[2].getData().setIsFeeder(false);
                        entity.belongsView.getChildren()[2].getData().setIsScraper(true);
                        entity.belongsView.getChildren()[2].getData().setIsSteelNet(false);
                    }
                }
            }
        },
        setDetailsIndustryProperties: function (entity, visiable) {
            if (entity.belongsView) {
                var tabs = entity.belongsView.getChildren();
                var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
                if (!tabPanel) return;

                var tabPanelItems = tabPanel.tabBar.items.items;
                var currentTab = tabPanel.getActiveTab().title;
                var flag = false;
                var electricity = tabs.first(function (e) {
                    return e.viewGroup === "EIS_BaseData_ViewGroup";
                });

                Ext.each(tabPanelItems, function (item) {
                    if (electricity && electricity.label === item.title) {
                        item.show();
                        var activeTab = tabPanel.getActiveTab();
                        if (activeTab != electricity.label) {
                            tabPanel.setActiveTab(2);
                        }
                        entity.belongsView.getChildren()[2].getData().setSlotType(null);
                        entity.belongsView.getChildren()[2].getData().setIsFeeder(false);
                        entity.belongsView.getChildren()[2].getData().setIsScraper(false);
                        visiable ? item.show() : item.hide();
                        if (activeTab != electricity.label) {
                            tabPanel.setActiveTab(activeTab);
                        }
                    }
                    if (item.isHidden() && currentTab == item.title)
                        flag = true;
                });
                if (flag)
                    tabPanel.setActiveTab(0);
                else
                    tabPanel.setActiveTab(currentTab);
            }
        },
        rowclick: function (g, record, element, rowIndex, e, eOpts) {
            var tabs = g.up().SIEView._children;
            var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
            if (!tabPanel) return;

            var tabPanelItems = tabPanel.tabBar.items.items;
            var currentTab = tabPanel.getActiveTab().title;
            var flag = false;
            var electricity = tabs.first(function (e) {
                return e.viewGroup === "EIS_BaseData_ViewGroup";
            });

            Ext.each(tabPanelItems, function (item) {
                if (electricity && electricity.label == item.title) {
                    record.data.IndustryProperties == 20 ? item.show() : item.hide();
                }
                if (item.isHidden() && currentTab == item.title)
                    flag = true;
            });

            if (flag)
                tabPanel.setActiveTab(0);
            else
                tabPanel.setActiveTab(currentTab);
        }
    });


