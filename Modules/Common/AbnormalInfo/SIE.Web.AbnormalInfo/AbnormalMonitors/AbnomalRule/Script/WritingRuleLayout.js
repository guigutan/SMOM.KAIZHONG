Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.WritingRuleLayout',
    {
        extend: 'SIE.autoUI.layouts.Common',
        title: "",
        module: null,
        layout: function (regions) {
            var me = this;
            var mainView = regions.main._view;
            var childrenUI = this._layoutChildren(regions, mainView);
            me.setViewController(mainView);
            var ctl = mainView.getController();
            var topicTree = ctl.initTopicTree(mainView);
            return Ext.widget('container', {
                border: 0,
                layout: 'border',
                scrollable: true,
                defaults: {
                    split: true,
                    layout: 'fit',
                    border: 0
                },
                items: [
                    topicTree,
                    {
                        collapsible: false,
                        region: 'center',
                        baseCls: 'my-panel-no-border',
                        items: childrenUI
                    },
                ]
            });
        },

        _layoutChildren: function (regions, mainView) {
            var me = this;
            var main = regions.main;
            this.isLayoutChildrenHorizonal = main.getView().isLayoutChildrenHorizonal;
            this.isLayoutChildrenGroupHorizonal = main.getView().isLayoutChildrenGroupHorizonal;
            this.layoutSize = main.getView().layoutSize;
            var children = regions.children;
            var cardPanels = [];

            var mainControl = main.getControl();

            if (children.length === 0) {
                return mainControl;
            }

            //Create a tab here
            var tabPanel = {
                xtype: 'tabpanel',
                cls: 'custom_tabpanel', //用于样式特殊修改
                border: 0,
                activeTab: 0,
                listeners: {
                    tabchange: this._tabChange
                },
                bodyStyle: {
                    border: 0
                },
                defaults: {
                    layout: 'fit',
                    border: 0,
                    autoScroll: true
                },
                items: []
            };

            //排在前面的panel
            var layerPanel;

            Ext.each(children, function (child, index) {
                child_view = child.getView();
                if (index === 2) {
                    layerPanel = child.getControl();
                    //层别条件视图
                    mainView.layerView = layerPanel.SIEView;
                }
                else {
                    if (index === 0) prePanel = 'tab';
                    else child_view.inactive = true;
                    tabPanel.items.push({
                        title: child_view.getMeta().label,
                        items: child.getControl()
                    });
                }
            });

            var view = main.getView();

            var secondPanel = {
                xtype: 'panel',
                bodyStyle: {
                    border: 0
                },
                layout: {
                    type: me.isLayoutChildrenGroupHorizonal ? 'hbox' : 'vbox',
                    pack: 'start',
                    align: 'stretch'
                },
                defaults: {
                    margin: me.isLayoutChildrenGroupHorizonal ? '0 5 0 0' : '0 0 5 0',
                    flex: 1,
                    layout: 'fit',
                    border: false
                },
                items: []
            };
            if (tabPanel.items.length > 0) {
                if (cardPanels.length === 0) {
                    tabPanel.margin = 0;
                }
                secondPanel.items.push(tabPanel);
            }

            if (tabPanel.items.length > 0) {
                secondPanel = tabPanel;
            }
            if (view.formConfig)
                return this._layoutFormChildrenCore(mainControl, secondPanel, layerPanel);
        },

        _layoutFormChildrenCore: function (mainControl, secondPanel, layerPanel) {
            var me = this;
            return Ext.widget('container', {
                layout: 'border',
                autoScroll: true,
                defaults: {
                    collapsible: false,
                    split: false,
                    layout: 'fit',
                    border: 0
                },
                items: [{
                    region: 'north',
                    minHeight: 100,
                    height: "40%",
                    items: layerPanel
                }, {
                    region: 'center',
                    minHeight: 100,
                    height: "60%",
                    items: me.createMainView(mainControl, secondPanel)
                }]
            });
        },
        /**
         * 创建主块
         * @param {any} childrenUI
         */
        createMainView: function (mainControl, secondPanel) {
            var me = this;
            return Ext.widget('container', {
                border: 0,
                layout: 'border',
                scrollable: true,
                defaults: {
                    split: true,
                    layout: 'fit',
                    border: 0
                },
                items: [
                    {
                        region: 'north',
                        height: 50,
                        items: mainControl,
                    },
                    {
                        collapsible: false,
                        region: 'center',
                        margin: '5 0 0 0',
                        items: secondPanel
                    }
                ]
            });
        },      
        /**
          * 指定视图控制器
          * @param {any} view
          */
        setViewController: function (view) {
            SIE.Web.Core.CommonFuns.getViewController(view, SIE.Web.AbnormalInfo.AnomalyMonitors.WritingRuleController);
        }
    });