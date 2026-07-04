Ext.define('SIE.Web.ERPInterface.InventoryControl.InventoryControlGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    _layout: function (aggtMeta, regions) {
        /// <summary>
        /// 对所有区域进行布局。
        /// </summary>
        /// <param name="aggtMeta" type="SIE.Web.ClientMetaModel.ClientAggtMeta"></param>
        /// <param name="regions" type="SIE.autoUI.Regions"></param>
        /// <returns type="Ext.Component" />
        var layout = null;
        if (aggtMeta.layoutClass) {
            layout = Ext.create(aggtMeta.layoutClass);
        }
        else {
            layout = new SIE.Web.ERPInterface.InventoryControlLayout();
        }
        var res = layout.layout(regions);
        return res;
    }
});

Ext.define('SIE.Web.ERPInterface.InventoryControlLayout', {

    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'InventoryControlLayout',
    ListData: [],
    DetailData: [],
    ErpListData:[],
    statics: {
        setAllData(ListData, DetailData, ErpListData) {
            var me = this;
            me.ListData = ListData;
            me.DetailData = DetailData;
            me.ErpListData = ErpListData;
        },
        setListData(view) {
            var me = this;
            console.log(me.ListData);
            var dataStore = Ext.create('Ext.data.TreeStore', {
                data: me.ListData,
            });
            view._relations[0]._target.getControl().setStore(dataStore);
        },
        ClearListData(view) {
            var me = this;
            //console.log(me.ListData);
            var dataStore = Ext.create('Ext.data.TreeStore', {
                data: [],
            });
            Ext.getCmp('InventroyDetailGrid1').setStore(dataStore);
            Ext.getCmp('InventroyDetailGrid2').setStore(dataStore);
        }
    },
    _layoutChildren: function (regions) {
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
        var prePanel;
        var me = this;
        var view = main.getView();
        //var pcbColumn = view.config.gridConfig.columns.first(function (p) { return p.dataIndex == 'SetQty'; });
        //if (pcbColumn) { me.IsPcbView = true; }
        //else {
        //    me.IsPcbView = false;
        //}
        Ext.each(children, function (child, index) {
            if (child.getView().childLayoutType === 1) {
                if (index === 0) prePanel = 'card';
                cardPanels.push({
                    title: child.getView().getMeta().label,
                    items: child.getControl()
                });
            } else {
                if (index === 0) prePanel = 'tab';
                if (child.getView().getMeta().label == "仓库明细".t()) {
                    var treeCtl = me.createLeftTreeCtl();
                    var rightCtl = me.createRightTreeCtl();
                    var panelControl = me.createPanel(treeCtl, rightCtl);
                    view.mon(view, 'currentChanged', me._currentChanged, child.getView());
                    tabPanel.items.push({
                        title: child.getView().getMeta().label,
                        items: panelControl
                    });
                } else {
                    tabPanel.items.push({
                        title: child.getView().getMeta().label,
                        items: child.getControl()
                    });
                }
            }
        });

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
                width: 100,
                layout: 'fit',
                border: false
            },
            items: []
        };
        if (prePanel === 'card') {
            if (cardPanels.length > 0) {
                cardPanels.forEach(function (child, index) {
                    if (index + 1 == cardPanels.length) {
                        child.margin = 0;
                    }
                    secondPanel.items.push(child);
                });
            }
            if (tabPanel.items.length > 0) {
                if (cardPanels.length === 0) {
                    tabPanel.margin = 0;
                }
                secondPanel.items.push(tabPanel);
            }
        } else {
            if (tabPanel.items.length > 0) {
                if (cardPanels.length === 0) {
                    tabPanel.margin = 0;
                }
                secondPanel.items.push(tabPanel);
            }
            if (cardPanels.length > 0) {
                cardPanels.forEach(function (child, index) {
                    if (index + 1 == cardPanels.length) {
                        child.margin = 0;
                    }
                    secondPanel.items.push(child);
                });
            }
        }

        if (tabPanel.items.length > 0 && cardPanels.length === 0) {
            secondPanel = tabPanel;
        }
        if (view.formConfig)
            return this._layoutFormChildrenCore(mainControl, secondPanel);
        return this.layoutChildrenCore(mainControl, secondPanel, me.isLayoutChildrenHorizonal);
    },
    createPanel: function (treeCtl,rightCtl) {
        return {
            xtype: 'panel',
            bodyStyle: {
                border: 0
            },
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [treeCtl,{xtype: 'splitter'},rightCtl],
        };
    },
    _currentChanged: function (parm) {
        var me = this;//父列表       
        if (me._parent._current != null) {
            var LineNo = me._parent._current.data.LineNo;
            var dtlList = SIE.Web.ERPInterface.InventoryControlLayout.DetailData;
            var ErpList = SIE.Web.ERPInterface.InventoryControlLayout.ErpListData;
            var detailList = dtlList.filter(function (Item) {
                return Item.ParentLineNo == LineNo;
            });
            var ErpData = ErpList.filter(function (Item) {
                return Item.ParentLineNo == LineNo;
            });
            //var store = Ext.getCmp('packingLabelExtTreeGrid').getStore();
            //if (detailList.length > 0) {
            //    var dataStore = Ext.create('Ext.data.TreeStore', {
            //        data: detailList,
            //    });
            //    Ext.getCmp('InventroyDetailGrid').setStore(dataStore);
            //}
            var dataStore1 = Ext.create('Ext.data.TreeStore', {
               data: detailList,
            });
            var dataStore2 = Ext.create('Ext.data.TreeStore', {
                data: ErpData,
            });
            Ext.getCmp('InventroyDetailGrid1').setStore(dataStore1);
            Ext.getCmp('InventroyDetailGrid2').setStore(dataStore2);
        }
    },
    createLeftTreeCtl: function (store) {
        var tree = {
            region: 'left',
            //Ext.grid.Panel
            xtype: 'gridpanel',
            id: 'InventroyDetailGrid1',
            reserveScrollbar: true,
            //store: store,
            columnLines: true,
            flex: 1,
            //features: [{
            //    ftype: 'merge'
            //}],
            title:"仓库明细",
            columns: [{
                text: '序号'.t(),
                dataIndex: 'LineNo',
                sortable: true,
                align: 'center',
                width: 120,
            }, {
                text: '仓库'.t(),
                dataIndex: 'WareHouseCode',
                width: 180,
                sortable: true,
                align: 'center',
            }, {
                text: '现有量'.t(),
                dataIndex: 'Qty',
                width: 90,
                sortable: true,
                align: 'center',
                },{
                text: '暂收数'.t(),
                dataIndex: 'RecQty',
                width: 90,
                sortable: true,
                align: 'center',
                }
            ],
            //listeners: {
            //    afterrender: function (grid) {
            //        //var header = grid.header.items.items[0].el.dom
            //        //header.style.backgroundColor = 'white'; // 设置标题的背景色为蓝色
            //        //header.style.color = 'black'; // 设置标题的文字颜色为白色
            //        //var style = {
            //        //    'backgroundColor': 'white',
            //        //    'color': 'black',
            //        //}
            //        //grid.header.setStyle(style)
            //    }
            //}
        };
        return tree;
    },
    createRightTreeCtl: function (store) {
        var tree = {
            region: 'right',
            //Ext.grid.Panel
            xtype: 'gridpanel',
            id: 'InventroyDetailGrid2',
            reserveScrollbar: true,
            //store: store,
            columnLines: true,
            //features: [{
            //    ftype: 'merge'
            //}],
            flex: 1,
            title: "ERP子库",
            columns: [
            {
                text: '序号'.t(),
                dataIndex: 'LineNo',
                sortable: true,
                align: 'center',
                width: 120,
            }, {
                text: 'ERP子库'.t(),
                dataIndex: 'ErpWareHouseCode',
                sortable: true,
                align: 'center',
                width: 180,
            },
            {
                text: 'ERP现有量'.t(),
                dataIndex: 'ErpQty',
                width: 100,
                sortable: true,
                align: 'center',
                },

            ],
            //listeners: {
            //    afterrender: function (grid) {
            //        var header = grid.headerCt.query('gridcolumn')[0].el.dom;
            //        header.style.backgroundColor = 'white'; // 设置标题的背景色为蓝色
            //        header.style.color = 'black'; // 设置标题的文字颜色为白色
            //    }
            //}
        };
        return tree;
    }
});