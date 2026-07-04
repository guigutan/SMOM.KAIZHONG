Ext.define('SIE.Web.Equipments.EquipModels.Scripts.EquipModelBehavior',
    {
        /**
        * 数据加载完毕的处理事件
        * @param {*} view 生成的view
        */
        onDataLoaded: function (view) {
            var me = this;
            if (view) {
                var entity = view.getCurrent();

                if (entity) {
                    entity.Behavior = me;
                    me.tabEvent(view, entity);
                    me.bindEvent(view, entity);
                }
            }
        },

        /**
         * bindEvent 绑定事件
         * @param {any} me
         * @param {any} entity
         */
        bindEvent: function (view, entity) {
            var me = this;
            view.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, view);
        },

        tabEvent: function (view, entity) {
            var tabs = view._children;
            var tabPanel = tabs ? tabs[0].getControl().ownerCt.ownerCt : null;
            if (!tabPanel) return;
            var tabPanelItems = tabPanel.tabBar.items.items;
            var currentTab = tabPanel.getActiveTab().title;
            var flag = false;

            var hideTabCount = 0;
            Ext.each(tabPanelItems, function (item) {
                debugger;
                if (("位置列表" + "电子行业").indexOf(item.title) >= 0) {
                    if (entity.getData().IndustryCategory === 1) {
                        if (!tabPanel.isVisible()) {
                            tabPanel.show();
                        }
                        item.show();
                    } else {
                        item.hide();
                        hideTabCount++;
                    }
                } else if (("PCB行业基础数据").indexOf(item.title) >= 0) {
                    if (entity.getData().IndustryCategory === 2) {
                        if (!tabPanel.isVisible()) {
                            tabPanel.show();
                        }
                        item.show();
                    } else {
                        item.hide();
                        hideTabCount++;
                    }
                }

                if (item.isHidden() && currentTab == item.title) {
                    flag = true;
                }
            });

            if (hideTabCount == tabPanelItems.length) {
                tabPanel.hide();
            }
            else {
                if (flag) {
                    tabPanel.setActiveTab(0);
                }
                else {
                    tabPanel.setActiveTab(currentTab);
                }
            }
        },

        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {
            var view = e.entity.belongsView;
            var entity = view.getCurrent();
            if (entity) {
                e.entity.Behavior.tabEvent(view, entity);
            }
        },
    });