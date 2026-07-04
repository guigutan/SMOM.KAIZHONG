Ext.define('SIE.Web.Kit.MES.CallMaterials.Scripts.CallMaterialWorkOrderLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'CallMaterialWorkOrderLayout',
    /**
     * 生产资源id
     * @property {double} oldResourceId
     */
    oldResourceId: null,

    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
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

        Ext.each(children, function (child) {
            if (child.getView().childLayoutType === 1) {
                cardPanels.push({
                    title: child.getView().getMeta().label,
                    items: child.getControl()
                });
            } else {
                if (child.getView().getMeta().label == "工单匹配") {
                    panelControl = me.createPanel(child.getControl());
                    tabPanel.items.push({
                        title: child.getView().getMeta().label,
                        items: panelControl
                    });
                }
                else {
                    tabPanel.items.push({
                        title: child.getView().getMeta().label,
                        items: child.getControl()
                    });
                }
            }
        });

        var view = main.getView();
        if (view.label == "叫料工单") {
            me.oldResourceId = 0;
            view.mon(view, 'currentChanged', this.onCurrentChanged, this);
            view.mon(view.getData(), 'load', this.onLoad, this);

            var matchWoLayout = Ext.Array.findBy(view._children, function (item) {
                if (item.model == 'SIE.Kit.MES.CallMaterials.CallMatchWorkOrder') { return true; }
            });
            if (matchWoLayout) {
                var childControl = matchWoLayout.getControl();
                childControl.mon(childControl, 'selectionchange', this.onControlSelectionChanged, childControl);
                var checkBoxId = childControl.columns[1].id;
                var checkBoxControl = Ext.getCmp(checkBoxId);
                checkBoxControl.mon(checkBoxControl, 'checkchange', this.checkchange, checkBoxControl);
            }

            this.setAutoVisible(view);
            var criteriaView = CRT.Context.PageContext.getQueryView();//view.getControl().SIEView._relations[0]._target;
            if (criteriaView) {
                var comboId = criteriaView.getControl().items.items[4].id;//todo 框架提供更便捷的API获取控件
                var conditionControl = Ext.getCmp(comboId);
                conditionControl.pageScope = me;
                conditionControl.mon(conditionControl, 'select', this.select, conditionControl);
                conditionControl.mon(conditionControl, 'change', this.change, conditionControl);
                var token = view.getControl().SIEView.token;
                SIE.invokeDataQuery({
                    type: "SIE.Web.Kit.MES.CallMaterials.DataQuery.CallMaterialWODataQueryer",
                    method: "GetDefaultResource",
                    params: [],
                    async: false,
                    token: token,
                    callback: function (res) {
                        if (res.Success) {
                            if (res.Result) {
                                var resource = res.Result.data.items[0].data;
                                if (resource) {
                                    criteriaView.getCurrent().setResourceName(resource.Name);
                                    criteriaView.getCurrent().setResourceId(resource.Id);
                                }
                            }
                        }
                    }
                });
            }
        }

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
        if (tabPanel.items.length > 0 && cardPanels.length === 0) {
            secondPanel = tabPanel;
        }
        if (view.formConfig)
            return this._layoutFormChildrenCore(mainControl, secondPanel);
        return this.layoutChildrenCore(mainControl, secondPanel, me.isLayoutChildrenHorizonal);
    },

    /**
     * 创建面板控件
     * @method createPanel
     * @param {childControl} childControl 子控件
     * @return {Ext.panel.Panel} 面板控件
     */
    createPanel: function (childControl) {
        return {
            xtype: 'panel',
            id: 'matchWoPanel',
            bodyStyle: {
                border: 0
            },
            layout: {
                type: 'hbox',
                pack: 'start',
                align: 'stretch'
            },
            items: [{
                flex: 2,
                layout: 'fit',
                items: childControl
            }, {
                xtype: 'splitter'   // A splitter between the two child items
            }, {
                flex: 1,
                layout: 'fit',
                title: '物料占比:0%',
                ui: 'light',
                font: '20px Arial',
                items: [{
                    layout: 'fit',
                    xtype: 'pie-donut',
                    id: 'itemPie',
                }]
            }]
        };
    },

    /**
     * 当前变更事件
     * @method onCurrentChanged
     * @param {oldValue} oldValue 旧值
     * @param {entity} entity 实体对象
     */
    onCurrentChanged: function (oldValue, entity) {
        var me = this;
        var length = 0;
        if (oldValue.newValue != null) {
            var matchWoPanelChart = Ext.getCmp('matchWoPanel');
            if (matchWoPanelChart)
                matchWoPanelChart.items.items[2].setTitle("物料占比:0%");
            var itemPieChart = Ext.getCmp('itemPie');
            if (itemPieChart) {
                itemPieChart.items.items[0].store.data.items[1].data.data1 = 100;
                itemPieChart.items.items[0].store.data.items[1].data.Content = 0;
                itemPieChart.fireEvent('onColorSpreadChange', 0.4);
            }

            var matchWoLayout = Ext.Array.findBy(oldValue.newValue.belongsView._children, function (item) {
                if (item.model == 'SIE.Kit.MES.CallMaterials.CallMatchWorkOrder') { return true; }
            });
            if (matchWoLayout) {
                length = matchWoLayout.getData().data.length;
                if (length > 0) {
                    var matchRate = matchWoLayout.getData().data.items[0].data.MatchRate;
                    var msg = "物料占比:" + 100 * matchRate + "%";
                    if (matchWoPanelChart)
                        matchWoPanelChart.items.items[2].setTitle(msg);
                    if (itemPieChart) {
                        itemPieChart.items.items[0].store.data.items[1].data.data1 = 100;
                        itemPieChart.items.items[0].store.data.items[1].data.Content = 100 * matchRate;
                        itemPieChart.fireEvent('onColorSpreadChange', 1);
                    }
                }
            }

            if (length <= 0) {
                var callWorkOrder = oldValue.newValue.data;
                var autoTaken = oldValue.newValue.belongsView.token;
                SIE.invokeDataQuery({
                    type: "SIE.Web.Kit.MES.CallMaterials.DataQuery.CallMaterialWODataQueryer",
                    method: "GetMatchRate",
                    params: [callWorkOrder],
                    async: false,
                    token: autoTaken,
                    callback: function (res) {
                        if (res.Success) {
                            if (res.Result && res.Result != null) {
                                var msg = "物料占比:" + 100 * res.Result + "%";
                                if (matchWoPanelChart)
                                    matchWoPanelChart.items.items[2].setTitle(msg);
                                if (itemPieChart) {
                                    itemPieChart.items.items[0].store.data.items[1].data.data1 = 100;
                                    itemPieChart.items.items[0].store.data.items[1].data.Content = 100 * res.Result;
                                    itemPieChart.fireEvent('onColorSpreadChange', 1);
                                }
                            }
                        }
                    }
                });
            }
        }
    },

    /**
     * 加载数据事件
     * @method onLoad
     * @param {store} store
     * @param {records} records
     * @param {successful} successful
     * @param {operation} operation
     * @param {eOpts} eOpts
     */
    onLoad: function (store, records, successful, operation, eOpts) {
        var itemPieChart = Ext.getCmp('itemPie');
        if (itemPieChart)
            itemPieChart.fireEvent('onColorSpreadChange', 0.4);
    },

    /**
     * 当前选中变更事件
     * @method onControlSelectionChanged
     * @param {grid} grid grid
     * @param {selection} selection 选中对象
     * @param {eOpts} eOpts 对象
     */
    onControlSelectionChanged: function (grid, selection, eOpts) {
        if (!selection.startCell)
            return;
        var data = selection.view.lastFocused.record.getData();
        var itemPieChart = Ext.getCmp('itemPie');
        var matchWoPanelChart = Ext.getCmp('matchWoPanel');
        if (data === null) {
            if (matchWoPanelChart)
                matchWoPanelChart.items.items[2].setTitle("物料占比:0%");
            if (itemPieChart) {
                itemPieChart.items.items[0].store.data.items[1].data.data1 = 100;
                itemPieChart.items.items[0].store.data.items[1].data.Content = 0;
                itemPieChart.fireEvent('onColorSpreadChange', 0.4);
            }
        }
        else {
            var msg = "物料占比:" + 100 * data.MatchRate + "%";
            if (matchWoPanelChart)
                matchWoPanelChart.items.items[2].setTitle(msg);
            if (itemPieChart) {
                itemPieChart.items.items[0].store.data.items[1].data.data1 = 100;
                itemPieChart.items.items[0].store.data.items[1].data.Content = 100 * data.MatchRate;
                itemPieChart.fireEvent('onColorSpreadChange', 1);
            }
        }
    },

    /**
     * 选中变更事件
     * @method select
     * @param {combo} combo 下拉框对象
     * @param {record} record 记录数据
     * @param {index} index index
     */
    select: function (combo, record, index) {
        var me = this;
        var pageScope = combo.pageScope;
        //var resData = me._getSIEView().getCurrent();
        var view = me.up('container').SIEView;
        var resData = view.getCurrent();
        if (!record)
            return;

        var newResourceId = record[0].data.Id;
        var newResourceName = record[0].data.Name;
        if (newResourceId === pageScope.oldResourceId)
            return;

        var saveResourceSetting = Ext.create('SIE.Web.Kit.MES.CallMaterials.Commands.SaveResourceSetting');
        saveResourceSetting.execute(me._view, {
            resourceId: record[0].data.Id
        });

        resData.setResourceId(newResourceId);
        resData.setResourceName(newResourceName);

        var autoCallMaterialCM = view._relations[0]._target.getCmdControl("SIE.Web.Kit.MES.CallMaterials.Commands.AutoCallMaterialCommand");
        var resourceId = view._current.data.ResourceId;
        var autoTaken = view._relations[0]._target.token;
        if (!autoCallMaterialCM || resourceId == null)
            return;

        pageScope.oldResourceId = resourceId;
        if (resourceId === 0)
            return;
        SIE.invokeDataQuery({
            type: "SIE.Web.Kit.MES.CallMaterials.DataQuery.CallMaterialWODataQueryer",
            method: "GetCallMaterialSetting",
            params: [resourceId],
            async: false,
            token: autoTaken,
            callback: function (res) {
                if (res.Success) {
                    if (res.Result) {
                        var callMaterialSet = res.Result.data.items[0].data;
                        if (callMaterialSet) {
                            if (callMaterialSet.IsAuto == 1)
                                autoCallMaterialCM.iconCls = "iconfont icon-checkbox-checked icon-blue";
                            else
                                autoCallMaterialCM.iconCls = "iconfont icon-checkbox-unchecked icon-blue";
                        }
                    }
                }
            }
        });
    },
    change: function (combo, newValue, oldValue, eOpts) {
        var me = this;
        //var resData = me._getSIEView().getCurrent();
        var resData = me.up('container').SIEView.getCurrent();
        if (Ext.isEmpty(newValue)) {
            SIE.Msg.showMessage("当前资源不能为空".L10N());
            resData.setResourceName(oldValue);
        }
    },

    /**
     * 设置变更图标
     * @method setAutoVisible
     * @param {view} view 视图对象
     */
    setAutoVisible: function (view) {
        var me = this;
        var autoCallMaterialCM = view.getControl().SIEView.getCmdControl("SIE.Web.Kit.MES.CallMaterials.Commands.AutoCallMaterialCommand");
        var resourceId = view.getControl().SIEView._relations[0]._target.getData().data.ResourceId;
        var autoTaken = view.getControl().SIEView._relations[0]._target.token;
        if (!autoCallMaterialCM || resourceId == null)
            return;

        me.oldResourceId = resourceId;
        if (resourceId === 0)
            return;
        SIE.invokeDataQuery({
            type: "SIE.Web.Kit.MES.CallMaterials.DataQuery.CallMaterialWODataQueryer",
            method: "GetCallMaterialSetting",
            params: [resourceId],
            async: false,
            token: autoTaken,
            callback: function (res) {
                if (res.Success) {
                    if (res.Result) {
                        var callMaterialSet = res.Result.data;
                        if (callMaterialSet) {
                            if (callMaterialSet.IsAuto == 1)
                                autoCallMaterialCM.iconCls = "iconfont icon-checkbox-checked icon-blue";
                            else
                                autoCallMaterialCM.iconCls = "iconfont icon-checkbox-unchecked icon-blue";
                        }
                    }
                }
            }
        });
    },

    /**
     * 选中变更事件
     * @method checkchange
     * @param {checkbox} checkbox checkbox对象
     * @param {rowIndex} rowIndex 行Index
     * @param {checked} checked 是否选中
     * @param {record} record 记录数据
     * @param {e} e 事件参数
     * @param {eOpts} eOpts eOpts
     */
    checkchange: function (checkbox, rowIndex, checked, record, e, eOpts) {
        var me = this;
        var token = me.getView().grid.SIEView.getParent().token;
        var indata = record.data;

        SIE.invokeDataQuery({
            type: "SIE.Web.Kit.MES.CallMaterials.DataQuery.CallMaterialWODataQueryer",
            method: "GetCallMatchItem",
            params: [indata],
            async: false,
            token: token,
            callback: function (res) {
                if (res.Success) {
                    if (res.Result == null)
                        indata.IsChange = 1;
                }
            }
        });
    }
});

/**
 * 饼形图Store的定义
 * @class SIE.Web.Kit.MES.CallMaterials.Scripts.PieChartStore
 * @constructs
 */
Ext.define('SIE.Web.Kit.MES.CallMaterials.Scripts.PieChartStore', {
    extend: 'Ext.data.Store',
    alias: 'store.PieChartStore',

    fields: ['os', 'data1', 'Content'],
    data: [
        { os: '', data1: 0, Content: 0 },
        { os: '物料占比', data1: 100, Content: 0 },
    ]
});

/**
 * 饼形图控件定义
 * @class SIE.Web.Kit.MES.CallMaterials.Scripts.Donut
 * @constructs
 */
Ext.define('SIE.Web.Kit.MES.CallMaterials.Scripts.Donut', {
    extend: 'Ext.Panel',
    xtype: 'pie-donut',
    controller: 'PieChartController',
    width: 400,
    listeners: {
        onColorSpreadChange: 'onColorSpreadChange',
    },
    items: [{
        xtype: 'polar',
        reference: 'chart',
        width: '100%',
        height: 350,
        innerPadding: 5,
        opacity: 0.4,
        store: {
            type: 'PieChartStore'
        },
        series: [{
            type: 'pie',
            angleField: 'data1',
            //style: {
            //    colors: ["'#2D5976", "#CADCE2","#FF0000"]
            //},         
            donut: 70,
            tooltip: {
                trackMouse: true,
                renderer: 'onSeriesTooltipRender'
            }
        }]
    }]
});

/**
 * 饼形图控制器定义
 * @class SIE.Web.Kit.MES.CallMaterials.Scripts.PieChartController
 * @constructs
 */
Ext.define('SIE.Web.Kit.MES.CallMaterials.Scripts.PieChartController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.PieChartController',
    /**
     * 数据显示事件
     * @method onDataRender
     * @param {v} v 值
     * @param {string} 界面显示的值
     */
    onDataRender: function (v) {
        return v + '%';
    },

    /**
     * 控件变更事件
     * @method onColorSpreadChange
     * @param {double} value 按钮控件
     */
    onColorSpreadChange: function (value) {
        this.setPieStyle({
            opacity: value
        });
    },

    /**
     * 设置饼图样式
     * @method setPieStyle
     * @param {style} style 样式
     */
    setPieStyle: function (style) {
        var chart = this.lookup('chart'),
            series = chart.getSeries()[0];
        series.setStyle(style);
        chart.redraw();
    },

    /**
     * 饼图提示语
     * @method onSeriesTooltipRender
     * @param {tooltip} tooltip 提示语
     * @param {record} record 记录数据
     * @param {item} item 对象
     */
    onSeriesTooltipRender: function (tooltip, record, item) {
        tooltip.setHtml(record.get('os') + ': ' + record.get('Content') + '%');
    }
});



