/**
 * 工艺路线显示编辑器
 * @class SIE.Web.Tech.RoutingDisplayEditor
 * @constructor
 */
Ext.define('SIE.Web.Tech.RoutingDisplayEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.routingDisplayEditor',
    layout: 'fit',
    style: 'margin: -10px; overflow-x: auto; overflow-y: hidden;',
    mainView: null,
    items: [{
        title: null,
        autoScroll: true,
        layout: 'fit',
        flex: 1,
        bodyStyle: {
            backgroundImage: "url('/images/drawtools/dot_bg.jpg')",
            backgroundRepeat: 'repeat'
        }
    }],
    listeners: {
        /**
         * 编辑器渲染事件
         * @method render
         * @for SIE.Web.Tech.RoutingDisplayEditor
         * @param {Object} scop 作用域
         * @param {Object} eOpts 参数
         */
        render: function (scop, eOpts) {
            var me = this;
            var form = this.up('form');
            if (form) {
                me.mainView = form.SIEView;
                this.mon(me.mainView, 'dataChanged', me.currentChanged, me);
                me.designCanvas = new DesignCanvas(me.mainView, me.config.canvas, null);
                me.InitDrawViewControl();
                me.designCanvas.setLock(true);
            }
        }
    },
    /**
     * 控件初始化化
     * @method initComponent
     * @for SIE.Web.Tech.RoutingDisplayEditor
     */
    initComponent: function () {
        var me = this;
        if (me.config === null || me.config.canvas === null)
            throw '画布id不能为空'.t();
        if (me.config.property === null || me.config.property === null)
            throw '属性不能为空'.t();
        me.items[0].html = Ext.String.format('<div id="{0}";style="position:absolute; width:100%; height:100%" ></div>', me.config.canvas);
        this.callParent();
    },

    /**
     * 编辑器渲染后事件
     * @method afterRender
     * @for SIE.Web.Tech.RoutingDisplayEditor
     */
    afterRender: function () {
        this.callParent();
        var me = this;
        document.getElementById(me.id).style.width = '100%';
    },

    /**
     * 数据变更事件
     * @method currentChanged
     * @for SIE.Web.Tech.RoutingDisplayEditor
     * @param {Object} context 上下文（实体对象）
     */
    currentChanged: function (context) {
        var me = this;
        me.drawRouting(context);
    },

    /**
     * 初始化画图工具
     * @method InitDrawViewControl
     * @for SIE.Web.Tech.RoutingDisplayEditor
     */
    InitDrawViewControl: function () {
        var me = this;
        me.designCanvas.InitDrawViewControl();
    },

    /**
     * 画工艺路线
     * @method drawRouting
     * @for SIE.Web.Tech.RoutingDisplayEditor
     * @param {Object} context 上下文（实体对象）
     */
    drawRouting: function (context) {
        var me = this;
        me.designCanvas.clearDrawControl();
        me.setHtml('');
        if (!context.value) {
            return;
        }
        var versionId = context.value.data[me.config.property] ? context.value.data[me.config.property] : 0;
        if (versionId === 0)
            return;
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "GetRoutingLayout",
            token: token,
            params: [versionId],
            success: function (res) {
                if (!res.Success)
                    return;
                me.setHtml('');
                me.designCanvas.drawRouting(res.Result);
            }
        });
    }
});