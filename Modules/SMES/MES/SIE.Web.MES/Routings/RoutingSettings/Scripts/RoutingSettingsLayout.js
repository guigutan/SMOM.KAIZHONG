/**
 * 产品工艺路线布局
 * @class SIE.Web.MES.RoutingSettings.RoutingSettingsLayout
 * @constructs
 */
Ext.define('SIE.Web.MES.RoutingSettings.RoutingSettingsLayout', {
    extend: 'SIE.autoUI.layouts.Common',

    /**
     * 主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 产品工艺路线设置
     * @property {SIE.MES.RoutingSettings.ProductRouting} current
     */
    current: null,

    /**
     * 界面布局
     * @method layout
     * @param {regions} regions 聚合块
     * @return {Ext.Container} 布局
     */
    layout: function (regions) {
        var me = this;
        me.mainView = regions.main._view;
        me.mainView.layout = me;
        me.registerEvent();
        var childrenUI = this._layoutChildren(regions);
        me.addRoutingCanvasTabItem(childrenUI);
        var res = this._layoutNaviCondition(regions, childrenUI);
        return res;
    },

    /**
     * 事件注册
     * @method registerEvent
     */
    registerEvent: function () {
        var me = this;
        var view = me.mainView;
        view.mon(view, 'beforeClosewin', me.beforeClosewin, me);
        view.mon(view, 'currentChanged', me.currentChanged, me);
    },

    /**
     * 添加默认工艺路线子页签
     * @method addRoutingCanvasTabItem
     * @param {Ext.Container} childrenUI 除查询块外的所有控件集合
     */
    addRoutingCanvasTabItem: function (childrenUI) {
        var me = this;
        var tabItem = Ext.widget('container', {
            layout: 'border',
            title: '工艺路线'.L10N(),
            items: [{
                region: 'center',
                xtype: 'panel',
                border: 0,
                autoScroll: true,
                bodyStyle: {
                    backgroundImage: "url('/images/drawtools/dot_bg.jpg')",
                    backgroundRepeat: 'repeat'
                },
                html: '<div style="position:absolute; width:100%; height:100%" id = "productTechSettingCanvas"></div>'
            }],
            listeners: {
                render: function (scop, eOpts) {
                    var me = this;
                    var layout = me.context;
                    layout.designCanvas = new DesignCanvas(layout.mainView, 'productTechSettingCanvas', null);
                    layout.designCanvas.InitDrawViewControl();
                    layout.mainView.fireEvent('currentChanged', {
                        oldValue: null,
                        newValue: layout.current
                    });
                }
            }
        });
        tabItem.context = me;
        me.tabPanel = childrenUI.down('tabpanel');
        if (me.tabPanel) {
            me.tabPanel.clearListeners(); //清除框架的tabchange事件，因为第一个标签页为自定义控件，不需要框架加载数据
            me.tabPanel.mon(me.tabPanel, 'tabchange', me.tabchange, me);
            me.tabPanel.insert(1, tabItem);
            me.tabPanel.setActiveTab(0);
        }
    },

    /**
     * 主视图关闭事件
     * @method beforeClosewin
     * @param {returnObj} returnObj 参数
     * @returns {returnObj} 参数
     */
    beforeClosewin: function (returnObj) {
        //取消消息订阅
        var me = this;
        var view = me.mainView;
        view.mun(view, 'beforeClosewin');
        view.mun(view, 'currentChanged');
        me.tabPanel.mun(me.tabPanel, 'tabchange'); 
        return me.isDataSaved(returnObj);
    },

    /**
     * 数据是否已保存
     * @method isDataSaved
     * @param {returnObj} returnObj 参数
     * @returns {returnObj} 参数
     */
    isDataSaved: function (returnObj) {
        var me = this;
        var data = me.mainView.getData();
        if (data) {
            var changeData = SIE.data.Serializer.serialize(data, true);
            if (changeData._data) {
                var hasData = false;
                for (var pro in changeData._data) {
                    hasData = true;
                    break;
                }
                returnObj.data = data;
                returnObj.hasData = hasData;
            }
        }
        return returnObj;
    },

    /**
     * 子列表标签页切换事件
     * @method tabchange
     * @param {Ext.tab.Panel} tabPanel 标签控件
     * @param {newCard} newCard 新激活子页签
     * @param {oldCard} oldCard 旧子页签
     * @param {eOpts} eOpts 参数
     */
    tabchange: function (tabPanel, newCard, oldCard, eOpts) {
        var me = this;
        if (newCard.title === '工艺路线')
            return;
        var control = newCard.down("gridpanel");
        if (control !== null && control.SIEView.getChildren().length === 0)
            if (newCard.down("form"))
                control = newCard.down("form").SIEView.getChildren().length > 0 ? newCard.down("form") : control;
        if (!control)
            control = newCard.down("form");
        if (!control)
            control = newCard.down("treepanel");
        var view = control.SIEView;
        view.inactive = false;
        view.loadChildData();
        if (view.hasListeners['isready']) {
            view.fireEvent('isReady', true);
        }
    },
    
    /**
     * 主视图产品工艺路线设置变更事件
     * @method currentChanged
     * @param {SIE.MES.RoutingSettings.ProductRouting} entity 产品工艺路线设置
     */
    currentChanged: function (entity) {
        var me = this;
        me.mainView.layout.current = entity.newValue;
        if ((!entity.newValue && !entity.oldValue) || (entity.oldValue && entity.newValue && entity.oldValue.data.Id === entity.newValue.data.Id))
            return;
        if (!me.mainView.layout.designCanvas)
            return;
        me.mainView.layout.designCanvas.clearDrawControl();
        if (!entity.newValue)
            return;
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.RoutingSettings.RoutingSettingDataQuery",
            method: "GetDefaultVersionLayout",
            token: token,
            params: [entity.newValue.data.RoutingId, entity.newValue.data.RoutingVersionId],
            success: function (res) {
                //画图之前重新清除画布，避免快速切换工艺路线导致多个工艺路线同时显示在一个画布上
                me.mainView.layout.designCanvas.clearDrawControl();
                me.mainView.layout.designCanvas.drawRouting(null);
                me.mainView.layout.designCanvas.drawRouting(res.Result);
            }
        });
    }
});