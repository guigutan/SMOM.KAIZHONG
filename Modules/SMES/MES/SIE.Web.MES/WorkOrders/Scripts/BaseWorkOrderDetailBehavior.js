Ext.define('SIE.Web.MES.WorkOrders.BaseWorkOrderDetailBehavior', {
    /**
     * 工单主视图
     */
    mainView: null,
    /**
     * 子页签面板
     */
    tabPanel: null,
    /**
     * 流程属性控件
     */
    propertyControl: null,

    /**
     * view生命周期函数--view准备完成
     * @param {DetailView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        me.mainView = view;
        me.attachTechTabItem(view);
        view.mon(view, 'beforeClosewin', me.beforeClosewin, me);
    },

    /**
    * 主视图关闭事件
    * @method beforeClosewin
    * @param {returnObj} returnObj 参数
    */
    beforeClosewin: function (returnObj) {
        //取消消息订阅
        var me = this;
        var view = me.mainView;
        view.mun(view, 'beforeClosewin');
        var workOrder = CRT.Context.PageContext.getCurrentRecord();
        view.mun(workOrder, 'propertyChanged');
        var params = CRT.Context.PageContext.getParams();
        if (params.action !== 3)  //查看界面取消脏数据提示
            view.beforeClosewin(returnObj);
    },

    /**
     * 页面显示
     * @param {any} view 视图
     */
    onShow: function (view) {
        var me = this;
        
        var workOrder = CRT.Context.PageContext.getCurrentRecord();
        if (!workOrder)
            throw new Error("未找到工单".L10N());
        var params = CRT.Context.PageContext.getParams();
        view.token = params.token;
        me.getTabPanel(view);
        
        me.initWorkOrderData(view, workOrder);
    },

    /**
     * 初始化工单数据
     * @param {any} view
     */
    initWorkOrderData: function (view, workOrder) {
        SIE.markAbstract();
    },

    /**
     * 获取子视图
     * @param {any} model 实体
     */
    getChildView: function (model) {
        return this.mainView._children.first(function (p) { return p.model === model; });
    },

    /**
     * 获取子页签面板
     * @param {any} view
     */
    getTabPanel: function (view) {
        var me = this;
        if (view._children.length > 0)
            me.tabPanel = view._children[0].getControl().ownerCt.ownerCt;
        else
            throw new Error("未找到任何子页签".L10N());
    },

    /**
     * 设置活动面板
     * @param {any} model
     * @param {any} activeFirstTabItem
     */
    activeTabItem: function (model, activeFirstTabItem) {
        var me = this;
        var childView = me.getChildView(model);
        if (!childView)
            return;
        var control = childView.getControl().ownerCt;
        
        me.tabPanel.setActiveItem(control);
        if (activeFirstTabItem && activeFirstTabItem === true) {
            var firstTab = me.tabPanel.items.items[0];
            me.tabPanel.setActiveItem(firstTab);
        }
    },

    /**
     * 初始化ProcessBom的属性值数据
     * @param {any} row 行数据
     * @param {any} selected  选择行
     * @param {any} eOpts 参数
     */
    initTabDataBySelect: function (sender, value, oldValue, eOpts) {
        if (sender.SIEView._parent._parent) {
            var propertylist = sender.SIEView._parent._parent.processBomPropertyList;
            if (oldValue.removed.length > 0) {
                //清空对应物料的属性
                var itemid = oldValue.removed[0].data.ItemId;
                SIE.each(propertylist, function (item) {
                    if (item.data.ItemId == itemid) {
                        Ext.Array.remove(propertylist, item);
                    }
                })
            }
            if (oldValue.data.length > 0) {
                //清空对应物料属性，重新添加对应物料的属性
                var itemid = oldValue.data.items[0].getItemId();
                SIE.each(propertylist, function (item) {
                    if (item.data.ItemId == itemid) {
                        Ext.Array.remove(propertylist, item);
                    }
                })
                SIE.each(oldValue.data.items, function (item) {
                    Ext.Array.push(propertylist, item);
                })
            }
            sender.SIEView._parent._parent.processBomPropertyList = propertylist;
        }
    },

    /**
     * 初始化Bom的属性值数据
     * @param {any} row  行数据
     * @param {any} selected 选择行
     * @param {any} eOpts 参数
     */
    initBomTabDataBySelect: function (sender, value, oldValue, eOpts) {
        if (sender.SIEView._parent._parent) {
            var propertylist = sender.SIEView._parent._parent.bomPropertyList;
            if (oldValue.removed.length > 0) {
                //清空对应物料的属性
                var itemid = oldValue.removed[0].data.ItemId;
                SIE.each(propertylist, function (item) {
                    if ((item.data && item.data.ItemId == itemid) || item.ItemId == itemid) {
                        Ext.Array.remove(propertylist, item);
                    }
                })
            }
            if (oldValue.data.length > 0) {
                //清空对应物料属性，重新添加对应物料的属性
                var itemid = oldValue.data.items[0].getItemId();
                SIE.each(propertylist, function (item) {
                    if ((item.data && item.data.ItemId == itemid) || item.ItemId == itemid) {
                        Ext.Array.remove(propertylist, item);
                    }
                })
                SIE.each(oldValue.data.items, function (item) {
                    Ext.Array.push(propertylist, item);
                })
            }
            sender.SIEView._parent._parent.bomPropertyList = propertylist;
        }
    },

    //*************************************工艺路线相关**********************************************
    /**
     * 附加工艺路线页签
     * @param {DetailLogicView} view 明细视图
     */
    attachTechTabItem: function (view) {
        var me = this;
        view.mon(view._current, 'propertyChanged', me.workOrderPropertyChanged, me);
        var canvasId = Ext.String.format('WorkOrderRoutingCanvasId:{0}', view._current.data.Id);
        var tabPanel = view._children[0].getControl().up('tabpanel'); 
        var items = SIE.Web.Tech.Common.Routings.PropertyExt.getPropertyConfigs();
        me.propertyControl = Ext.create('SIE.Tech.PropertyControl', {
            mainView: me.mainView,
            items: items
        });
        var tabItem = Ext.create('Ext.panel.Panel', {
            title: '工艺路线'.L10N(),
            border: 0,
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            canvasId: canvasId,
            context: me,
            items: [{
                title: null,
                autoScroll: true,
                flex: 1,
                bodyStyle: {
                    backgroundImage: "url('/images/drawtools/dot_bg.jpg')",
                    backgroundRepeat: 'repeat'
                },
                html: Ext.String.format('<div style="position:absolute; width:100%; height:100%" id = "{0}"></div>', canvasId),
            }, {
                width: 250,
                minWidth: 200,
                maxWidth: 300,
                border: 0,
                collapsible: true,
                split: true,
                layout: 'fit',
                title: '流程属性'.L10N(),
                items: me.propertyControl
            }],
            listeners: {
                render: function (scop, eOpts) {
                    var me = this;
                    view.designCanvas = new DesignCanvas(view, me.canvasId, null);
                    view.designCanvas.mon(view.designCanvas, 'nodeChanged', me.context.nodeChanged, me.context);
                    view.designCanvas.InitDrawViewControl();

                    //为了构建获取工序信息的方法me.mainView.layout.designControl.designCanvas.getPreProcessInfo(node);
                    view.layout = me.context;
                    view.layout.designControl = Ext.create('SIE.Web.MES.WorkOrders.UpdateRoutingControl', {
                        mainView: view
                    });
                    view.layout.designControl.designCanvas = view.designCanvas;
                    me.context.resetRouting(view._current, false);
                }
            }
        });
        if (tabPanel) {
            tabPanel.clearListeners(); //清除框架的tabchange事件，因为第一个标签页为自定义控件，不需要框架加载数据
            tabPanel.mon(tabPanel, 'tabchange', me.tabchange, this);
            tabPanel.insert(tabPanel.items.length, tabItem);
        }
        var routTab = tabPanel.tabBar.items.items.where(function (p) { return p.title === '工序清单' });
        if (routTab && routTab.length > 0)
            routTab[0].hide();
    },

    /**
    * 节点变更事件
    * @method nodeChanged
    * @for SIE.Tech.layouts.RoutingLayout
    * @param {ListLogicalView} mainView 父主视图
    * @param {Object} node 节点信息
    */
    nodeChanged: function (mainView, node) {
        var me = this;
        //加载属性
        //me.propertyControl.nodeChanged(node, true);
        //if (node)
        //    this.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');

        var isSelectedInner = false;
        if (node) {
            if (!node.designerData || (node.designerData && node.designerData.NodeType !== "RoutingGroupNode")) {
                me.propertyControl.nodeChanged(node, true);
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
                        me.propertyControl.nodeChanged(currentNodeData, true);
                        isSelectedInner = true
                        this.designControl.designCanvas.updateNodeBoxShadow(currentNodeData, 'rgba(15, 124, 245, 1) 0px 0px 0px 1px', '45px', '118px');
                    }
                }

            }
        }
        if (!isSelectedInner) {//选中内部的时候不需要再调用
            this.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');
        }

    },

    /**
     * 工单属性变更
     * @param {Object} arg 参数
     */
    workOrderPropertyChanged: function (arg) {
        var me = this;
        if (arg.property === 'VersionId') {
            me.resetRouting(arg.entity, true);
        }
    },

    /**
     * 设置工单工艺路线
     * @param {workOrder} workOrder 工单
     * @param {Boolean} fromVersion 布局来自版本变更
     */
    resetRouting: function (workOrder, fromVersion) {
        var me = this;
        if (!workOrder || !me.mainView.designCanvas)
            return;
        me.mainView.designCanvas.clearDrawControl();
        var versionId = workOrder.get('VersionId');
        var layoutId = workOrder.get('LayoutId');
        if (!versionId && !layoutId)
            return;
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.WorkOrders.WorkOrderDataQueryer",
            method: "GetWorkOrderRoutingLayout",
            token: me.mainView.token,
            params: [versionId, workOrder.getId(), fromVersion],
            success: function (res) {
                if (!res.Success)
                    return;
                //画图之前重新清除画布，避免快速切换工艺路线导致多个工艺路线同时显示在一个画布上
                me.mainView.designCanvas.clearDrawControl();
                me.mainView.designCanvas.drawRouting(null);
                me.mainView.designCanvas.drawRouting(res.Result);
            }
        });
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
    }
});