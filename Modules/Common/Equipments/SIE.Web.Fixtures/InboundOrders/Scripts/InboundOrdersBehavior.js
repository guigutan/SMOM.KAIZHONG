Ext.define('SIE.Web.Fixtures.InboundOrders.Scripts.InboundOrdersBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            meFixtureModelBehavior = this;
            var gridPanel = view.getControl();
            if (gridPanel.actionables) {
                var grid = gridPanel.ownerGrid;;
                grid.mon(grid, 'rowclick', meFixtureModelBehavior.rowclick, view);
            }
        },
        onDataLoaded: function (view) {
            var entity = view.getData();
            view.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, view);
            this.setDetailsManageMode(entity, entity.getData().ManageMode == 5);
        },
        
        /**
         * onEntityPropertyChanged 属性变更事件
         * @param {*} e 参数
         */
        onEntityPropertyChanged: function (e) {
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === 'ManageMode') {
                    
                    if (e.value != e.oldvalue) {
                        if (entity.belongsView.viewGroup == "DetailsView") {
                            meFixtureModelBehavior.setDetailsManageMode(entity, e.value === 5);
                        }
                        else
                            meFixtureModelBehavior.setDetailsManageMode(entity, e.value === 5);
                    }
                }
            }
        },
        setDetailsManageMode: function (entity, visiable) {
            if (entity.belongsView) {
                var tabs = entity.belongsView.getChildren();
                var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
                if (!tabPanel) return;

                var tabPanelItems = tabPanel.tabBar.items.items;
                var currentTab = tabPanel.getActiveTab().title;
                var flag = false;

                Ext.each(tabPanelItems, function (item) {
                    if (("ID类入库明细").indexOf(item.title) >= 0) {
                        visiable ? item.show() : item.hide();
                    }
                    if (("编码类入库明细").indexOf(item.title) >= 0) {
                        !visiable ? item.show() : item.hide();
                    }
                    if (("采购订单").indexOf(item.title) >= 0) {
                        !visiable ? item.show() : item.hide();
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

            Ext.each(tabPanelItems, function (item) {
                
                if (("ID类入库明细").indexOf(item.title) >= 0) {
                    record.data.ManageMode == 5 ? item.show() : item.hide();
                }
                if (("编码类入库明细").indexOf(item.title) >= 0 || ("采购订单").indexOf(item.title) >= 0) {
                    record.data.ManageMode == 10 ? item.show() : item.hide();
                }
                if (item.isHidden() && currentTab == item.title)
                    flag = true;
            });
            if (record.data.ManageMode == 5)
                tabPanel.setActiveTab(0);
            else
                tabPanel.setActiveTab(1);

        }
    });


