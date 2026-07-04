/**
 * 用户协议
 * @class SIE.Tech.layouts.RoutingLayout
 * @constructor
 */
Ext.define('SIE.Core.layouts.UserAgreementsLayout', {
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
        me.designControl = Ext.create('SIE.Core.UserAgreementFileControl', {
            mainView: me.mainView
        });
        me.routingControl = Ext.create('SIE.Core.UserAgreementTreeControl', {
            mainView: me.mainView,
            designControl:me.designControl
        });
        me.mainView.routingControl = me.routingControl;
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
                title: '用户协议'.L10N(),
                items: me.routingControl
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
        layout.childControls.forEach(function (childControl) {
            childControl.loadData(node);
        });
        //加载属性  
        var isDisbale = false;
        if (layout.mainView.CurRoutingVersion)
            isDisbale = layout.mainView.CurRoutingVersion.get('state') === 1;
        layout.propertyControl.nodeChanged(node, isDisbale);
        if (node)
            this.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 0.5) 0px 0px 0px 1px', '50px', '150px');
    }
});