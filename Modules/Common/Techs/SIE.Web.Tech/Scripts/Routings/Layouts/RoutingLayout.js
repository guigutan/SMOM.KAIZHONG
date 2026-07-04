/**
 * 工艺路线布局
 * @class SIE.Tech.layouts.RoutingLayout
 * @constructor
 */
Ext.define('SIE.Tech.layouts.RoutingLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    /**
     * 父主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 工序树控件
     * @property {SIE.Tech.ProcessTreeControl} processControl
     */
    processControl: null,

    /**
     * 工艺路线树控件
     * @property {SIE.Tech.RoutingTreeControl} routingControl
     */
    routingControl: null,

    /**
     * 工艺路线设计控件
     * @property {SIE.Tech.RoutingDesignControl} designControl
     */
    designControl: null,

    /**
     * 工序属性控件
     * @property {SIE.Tech.PropertyControl} propertyControl
     */
    propertyControl: null,

    /**
     * 子页签，不包含属性页签
     * @property
     */
    childControls: [],

    /**
     * 工艺路线布局
     * @method layout
     * @for SIE.Tech.layouts.RoutingLayout
     * @param {{}} regions 聚合块
     * @returns {container} 界面控件
     */
    layout: function (regions) {
        var me = this;
        var mainView = regions.main._view;
        me.mainView = mainView;
        mainView.layout = me;
        me.processControl = Ext.create('SIE.Tech.ProcessTreeControl', {
            mainView: me.mainView
        });
        me.routingControl = Ext.create('SIE.Tech.RoutingTreeControl', {
            mainView: me.mainView
        });
        me.designControl = Ext.create('SIE.Tech.RoutingDesignControl', {
            mainView: me.mainView
        });
        var items = SIE.Web.Tech.Common.Routings.PropertyExt.getPropertyConfigs();
        me.propertyControl = Ext.create('SIE.Tech.PropertyControl', {
            mainView: me.mainView,
            items: items
        });
        var childItems = [];   //子页签
        childItems.push({
            title: '流程属性'.L10N(),
            layout: 'fit',
            items: me.propertyControl
        });
        var configs = SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.getChildControlConfig();
        configs.forEach(function (config) {
            var control = Ext.create(config.type, {
                mainView: me.mainView
            });
            childItems.push({
                title: config.title,
                layout: 'fit',
                items: control
            });
            me.childControls.push(control);
            SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.controls.push(control);
        });

        me.registerEvents();
        var tabpanel = Ext.create('Ext.tab.Panel', {
            border: false,
            tabPosition: 'bottom',
            region: 'south',
            split: true,
            flex: 1,
            defaults: {
                scrollable: true,
                closable: false,
                border: false
            },
            items: childItems
        });
        return Ext.widget('container', {
            layout: 'border',
            that: me,
            items: [me.designControl, {
                region: 'west',
                width: 270,
                minWidth: 200,
                maxWidth: 300,
                border: 0,
                layout: 'fit',
                autoScroll: true,
                split: true,
                collapsible: true,
                title: '工艺路线'.L10N(),
                items: me.routingControl
            }, {
                region: 'east',
                width: 270,
                minWidth: 200,
                maxWidth: 350,
                border: 0,
                title: '工序信息'.L10N(),
                collapsible: true,
                split: true,
                layout: 'border',
                items: [me.processControl, tabpanel]
            }]
        });
    },

    /**
     * 事件注册
     * @method registerEvents
     * @for SIE.Tech.layouts.RoutingLayout 
     */
    registerEvents: function () {
        var me = this;
        me.routingControl.mon(me.routingControl, 'EditFlow', me.routingChanged, me);
        var canvas = me.designControl.designCanvas;
        canvas.mon(canvas, 'nodeChanged', me.nodeChanged, me);
        me.mainView.mon(me.mainView, 'beforeclosewin', me.cancelRegisterEvents, me);
    },

    /**
     * 事件取消订阅
     * @method cancelRegisterEvents
     * @for SIE.Tech.layouts.RoutingLayout
     */
    cancelRegisterEvents: function () {
        var me = this;
        me.routingControl.mun(me.routingControl, 'EditFlow', me.routingChanged, me);
        var canvas = me.designControl.designCanvas;
        canvas.mun(canvas, 'nodeChanged', me.nodeChanged, me);
        me.mainView.mun(me.mainView, 'beforeclosewin', me.cancelRegisterEvents, me);
    },

    /**
     * 工艺路线变更
     * @method routingChanged
     * @for SIE.Tech.layouts.RoutingLayout
     * @param {SIE.Tech.RoutingTreeControl} routingControl 工艺路线控件
     * @param {SIE.Tech.Routings.Routing} routing 工艺路线
     */
    routingChanged: function (routingControl, routing) {
        var layout = routingControl.mainView.layout;
        //重置属性控件
        layout.propertyControl.resetPropertyControl();
        //重置子页签 
        layout.childControls.forEach(function (childControl) {
            childControl.resetControl();
        });
        //加载工艺路线
        layout.designControl.drawRouting(routing);
    },

    /**
     * 节点变更事件
     * @method nodeChanged
     * @for SIE.Tech.layouts.RoutingLayout
     * @param {ListLogicalView} mainView 父主视图
     * @param {Object} node 节点信息
     */
    nodeChanged: function (mainView, node) {
        var layout = mainView.layout;
       
        //加载属性  
        var isDisbale = false;
        if (layout.mainView.CurRoutingVersion)
            isDisbale = layout.mainView.CurRoutingVersion.get('state') === 1;
        var isSelectedInner = false;
        if (node) {
            if (!node.designerData || (node.designerData && node.designerData.NodeType !== "RoutingGroupNode")) {
                //单个工序
                layout.childControls.forEach(function (childControl) {
                    childControl.loadData(node);
                });

                layout.propertyControl.nodeChanged(node, isDisbale);
            } else {
                //工序组需要重新取里面的Node
                var clickInnerDom = document.elementFromPoint(this.designControl.designCanvas.mouseEvent.x, this.designControl.designCanvas.mouseEvent.y);
                var blockDom = null;
                if (clickInnerDom && clickInnerDom.className === "node") {
                    blockDom = clickInnerDom;
                }
                if (clickInnerDom && clickInnerDom.className !== "node") {
                    blockDom = clickInnerDom.parentNode;
                }

                if (blockDom && blockDom.id != node.id) {
                    var groupNodes = this.designControl.designCanvas.drawControl.drawTools.getNode(node.id);//取出组的数据
                    var currentNodeData = null;
                    groupNodes.groupDesignerData.forEach(function (nodeData) {

                        if (nodeData.id == blockDom.id) {
                            currentNodeData = nodeData;
                            return;
                        }
                    });
                    if (currentNodeData) {
                        //工序组里面的工序点击
                        layout.childControls.forEach(function (childControl) {
                            childControl.loadData(currentNodeData);
                        });
                        layout.propertyControl.nodeChanged(currentNodeData, isDisbale);
                        isSelectedInner = true
                        this.designControl.designCanvas.updateNodeBoxShadow(currentNodeData, 'rgba(15, 124, 245, 1) 0px 0px 0px 1px', '45px', '118px');
                    }
                }

            }
        }
        if (!isSelectedInner) {//选中内部的时候不需要再调用
            this.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');
        }
        this.designControl.designCanvas.mouseEvent = null;
    }
});