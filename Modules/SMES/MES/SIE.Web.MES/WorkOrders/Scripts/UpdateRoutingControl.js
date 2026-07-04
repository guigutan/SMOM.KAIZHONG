/**
 * 工单工艺路线保编辑控件
 * @class SIE.Web.MES.WorkOrders.UpdateRoutingControl
 * @constructor
 */
Ext.define('SIE.Web.MES.WorkOrders.UpdateRoutingControl', {
    extend: 'Ext.panel.Panel',
    bodyPadding: 0,
    border: 0,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    items: [{
        title: null,
        autoScroll: true,
        flex: 1,
        bodyStyle: {
            backgroundImage: "url('/images/drawtools/dot_bg.jpg')",
            backgroundRepeat: 'repeat'
        },
        html: '<div style="position:absolute; width:100%; height:100%" id = "workOrderRoutingCanvas"></div>'
    }],
    listeners: {
        /**
         * 节点变更事件
         * @method nodeChanged
         * @for SIE.Web.MES.WorkOrders.UpdateRoutingControl
         * @param {designControl} designControl 控件
         * @param {Object} node 节点信息
         */
        nodeChanged: function (designControl, node) {


        },

        /**
         * Window渲染后事件，加载工单工艺路线
         * @method afterrender
         * @for SIE.Web.MES.WorkOrders.UpdateRoutingControl
         * @param {Ext.window.Window} scrop Window
         * @param {Object} eOpts 参数
         */
        render: function (scrop, eOpts) {
            var me = this;
            me.createDesignCanvas();  //必须在控件初始化后执行，不然找不到画布 
        }
    },
    /**
     * 主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 设计界面
     * @property {DesignCanvas} designCanvas
     */
    designCanvas: null,

    /**
     * 工艺路线布局
     * @property {String} routingLayout
     */
    routingLayout: null,  

    /**
     * 初始化工单工艺路线编辑控件
     * @method initComponent
     * @for SIE.Web.MES.WorkOrders.UpdateRoutingControl 
     */
    initComponent: function () {
        var me = this;
        me.designCanvas = new DesignCanvas(me.mainView, 'workOrderRoutingCanvas', null);  //先初始化，这样外部就可以挂nodeChanged事件
        me.callParent();
    },

    /**
     * 创建画图工具
     * @method createDesignCanvas
     * @for SIE.Web.MES.WorkOrders.UpdateRoutingControl 
     */
    createDesignCanvas: function () {
        var me = this;
        me.designCanvas.InitDrawViewControl();
        me.designCanvas.setLock(false);
    },

    /**
     * 画工艺路线
     * @method drawRouting
     * @for SIE.Web.MES.WorkOrders.UpdateRoutingControl
     * @param {String} layout 布局xml
     */
    drawRouting: function (layout) {
        var me = this;
        me.routingLayout = layout;
        me.resetMainBlock();
        me.designCanvas.drawRouting(layout);
    },

    /**
     * 重置画布
     * @method resetMainBlock
     * @for SIE.Web.MES.WorkOrders.UpdateRoutingControl 
     */
    resetMainBlock: function () {
        var me = this;
        //清除画布内容
        me.designCanvas.clearDrawControl();
    },

    /**
     * 获取工单工艺路线布局结果
     * @method getRoutingXml
     * @for SIE.Web.MES.WorkOrders.UpdateRoutingControl 
     * @returns {String} 布局结果
     */
    getRoutingXml: function () {
        var me = this;
        return Ext.getCmp(me.mainView.routingDrawControlId).getXml();
    }
});