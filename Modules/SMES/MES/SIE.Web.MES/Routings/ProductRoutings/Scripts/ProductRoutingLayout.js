/**
 * 产品工艺路线布局
 * @class SIE.Web.MES.ProductRoutings.ProductRoutingLayout
 * @constructor
 */
Ext.define('SIE.Web.MES.ProductRoutings.ProductRoutingLayout', {
    extend: 'SIE.autoUI.layouts.Common',

    /**
     * 产品工艺路线主视图
     * @property {ListLogicView} mainView
     */
    mainView: null,

    /**
     * 当前选中产品条码
     * @property {Barcode} barcode
     */
    barcode: null,

    /**
     * 产品在制版本   IsPause 0:No 1:Yes
     * @property {WipProductVersion} version
     */
    version: null,

    /**
     * 产品工艺路线关联子视图字典  key视图model类型，value子视图view
     * @property {字典} dicRelationView
     */
    dicRelationView: {},

    /**
     * 产品工艺路线设计控件
     * @property {ProductRoutingDesignControl} designControl
     */
    designControl: null,

    /**
     * 是否工单布局
     * @property {Boolean} isWorkOrderLayout
     */
    isWorkOrderLayout: null,

    /**
     * 工序bom字典：key工序id，value工序bom集合
     * @property {字典} processBomDic
     */
    processBomDic: {},

    /**
     * 布局
     * @method layout
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {regions} regions 聚合块
     * @return {container} 结果控件
     */
    layout: function (regions) {
        var me = this;
        me.mainView = regions.main._view;
        me.mainView.layout = me;
        var tabPanel = {
            xtype: 'tabpanel',
            cls: 'custom_tabpanel', //用于样式特殊修改
            border: 0,
            activeTab: 0,
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
        me.surrounders = regions.surrounders;
        Ext.each(me.surrounders, function (surrounder) {
            var result = surrounder.result;
            var view = result.getView();
            view.parentView = me.mainView;  //子视图通过parentView关联父视图
            me.dicRelationView[view.model] = view;
        });
        me.initTabPanel(tabPanel);
        me.processControl = Ext.create('SIE.Tech.ProcessTreeControl', {
            mainView: me.mainView
        });
        me.designControl = Ext.create('SIE.Web.MES.ProductRoutingDesignControl', {
            mainView: me.mainView
        });
        var items = SIE.Web.Tech.Common.Routings.PropertyExt.getPropertyConfigs();
        me.propertyControl = Ext.create('SIE.Tech.PropertyControl', {
            mainView: me.mainView,
            items: items
        });

        me.registerEvent();
        var tabProcessPanel = Ext.create('Ext.tab.Panel', {
            border: false,
            //tabPosition: 'bottom',
            region: 'center',
            split: true,
            flex: 1,
            defaults: {
                scrollable: true,
                closable: false,
                border: false
            },
            items: [{
                title: '工序信息'.L10N(),
                layout: 'fit',
                items: me.processControl
            }, {
                title: '流程属性'.L10N(),
                layout: 'fit',
                items: me.propertyControl
            }]
        });
        var mainControl = Ext.widget('container', {
            layout: 'border',
            items: [{
                ////下面标签页 
                region: 'south',
                xtype: 'panel',
                layout: 'fit',
                height: 300,
                minHeight: 200,
                maxHeight: 500,
                split: true,
                border: 0,
                items: tabPanel
            }, {
                //右侧工序信息
                region: 'east',
                width: 250,
                minWidth: 100,
                maxWidth: 300,
                border: 0,
                layout: 'border',
                split: true,
                collapsible: true,
                items: [tabProcessPanel]
            }, {
                border: 0,
                title: '工艺流程'.L10N(),
                region: 'center',
                layout: 'fit',
                split: true,
                items: me.designControl
            }]
        });
        var con = regions.getCondition();
        return Ext.widget('container', {
            border: 0,
            layout: 'border',
            scrollable: false,
            defaults: {
                layout: 'fit',
                border: 0
            },
            items: [Ext.merge({
                title: '查询条件'.t(),
                layout: 'border',
                items: [{
                    region: 'north',
                    border: 0,
                    height: 120,
                    items: con.getControl()
                }, {
                    region: 'center',
                    layout: 'fit',
                    border: 0,
                    items: me.mainView.getControl()
                }]
            }, GlobalConfig.defaultConditionPanelConfig), {
                region: 'center',
                border: 0,
                xtype: 'panel',
                items: mainControl
            }]
        });
    },

    /**
     * 初始化子页签
     * @method initTabPanel
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {tabpanel} tabPanel 标签页控件
     */
    initTabPanel: function (tabPanel) {
        var me = this;
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.WIP.Products.WipProductProcessKeyItem']);
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.WIP.Products.WipProductTestResult']);

        var productBomViewModel = me.dicRelationView['SIE.MES.RoutingSettings.ProductBomViewModel']
        me.addTabItem(tabPanel, productBomViewModel);

        var commands = productBomViewModel.getCommands();
        if (commands && commands.length > 0) {
            var btn = null;
            commands.eachKey(function (key, item, index, len) {
                btn = productBomViewModel.getCmdControl(key);
                if (btn != null) {
                    if (item.canVisible(productBomViewModel)) {
                        btn.show();
                        if (item.canExecute(productBomViewModel)) btn.enable();
                        else btn.disable();
                    } else {
                        btn.hide();
                    }
                }
            }, productBomViewModel);
        }

        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.WIP.Products.WipProductDefect']);
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.WIP.Products.WipProductRepair']);
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.WIP.Products.WipProductRoutingEvent']);
    },

    /**
     * 添加子页签
     * @method addTabItem
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {tabpanel} tabPanel 标签页控件
     * @param {ListLogicView} view 视图
     */
    addTabItem: function (tabPanel, view) {
        tabPanel.items.push({
            title: view.getMeta().label.L10N(),
            items: view.getControl()
        });
    },

    /**
     * 注册事件
     * @method registerEvent
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout 
     */
    registerEvent: function () {
        var me = this;
        var canvas = me.designControl.designCanvas;
        canvas.mon(canvas, 'nodeChanged', me.nodeChanged, me);
        me.mainView.mon(me.mainView, 'currentChanged', me.barcodeChanged, me);
        me.mainView.mon(me.mainView, 'isReady', me.isReady, me);
        me.mainView.mon(me.mainView, 'beforeclosewin', me.beforeclosewin, me);
    },

    /**
     * 视图关闭事件，取消事件订阅
     * @method beforeclosewin
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout 
     */
    beforeclosewin: function () {
        var me = this;
        var canvas = me.designControl.designCanvas;
        canvas.mun(canvas, 'nodeChanged', me.nodeChanged, me);
        me.mainView.mun(me.mainView, 'currentChanged', me.barcodeChanged, me);
        me.mainView.mun(me.mainView, 'isReady', me.isReady, me);
        me.mainView.mun(me.mainView, 'beforeclosewin', me.beforeclosewin, me);
    },

    /**
     * 视图准备完成
     * @method isReady
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {value} value 值
     */
    isReady: function (value) {
        var me = this;
        me.updateBomCommandState();
    },

    /**
     * 条码变更后事件
     * @method barcodeChanged
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Barcode} entity 条码 
     */
    barcodeChanged: function (entity) {
        var me = this;
        if (entity.oldValue !== null && entity.newValue !== null && entity.oldValue.data.Id === entity.newValue.data.Id)
            return;
        var barcode = null;
        if (entity.newValue === null)
            barcode = null;
        else
            barcode = entity.newValue.data;
        me.refreshWipProductInfo(barcode);
    },

    /**
     * 刷新生产产品数据
     * @method refreshWipProductInfo
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Barcode} barcode 条码
     */
    refreshWipProductInfo: function (barcode) {
        var me = this;
        me.barcode = barcode;
        me.version = null;
        me.processBomDic = {};
        me.isWorkOrderLayout = null;
        me.clearData();
        if (me.barcode) {
            me.loadWipProductInfo(me.barcode.WorkOrderId, me.barcode.Sn);
        }
        else {
            me.designControl.drawRouting(null);
            me.designControl.updateCommandsStatus();
            me.updateBomCommandState();
        }
    },

    /**
     * 节点变更事件
     * @method nodeChanged
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {ListLogicView} mainView 主视图
     * @param {Object} node 节点信息 
     */
    nodeChanged: function (mainView, node) {
        var me = mainView.layout;
        mainView.currentNode = node;
        //工序节点变更时需要加载：产品缺陷记录、产品维修记录、产品生产关键件、产品测试结果、工序bom
        var designerData = {};
        me.designControl.updateSetCurrentStatus();
        if (node && node.nodeType === 'RoutingNode') {
            designerData = node.designerData;
            var productStatus = 2;   //在制
            if (!me.version) {
                productStatus = 0;   //未上线
            }
            else if (me.version.IsFinish) {
                productStatus = 1;   //已完工
            }

            var woId = null;
            if (mainView.getCurrent() && mainView.getCurrent().data) {
                woId = mainView.getCurrent().data.WorkOrderId;
            }

            var versionId = null;
            if (me.version) {
                versionId = me.version.Id;
            }

            me.loadWipProcessInfo(versionId, designerData.ProcessId, productStatus, node.id, woId);
        }
        else {
            me.clearProcessInfo();
        }

        //加载属性  
        var isDisbale = true;
        if (me.version && me.version.IsPause === 1 && me.version.IsFinish === false)
            isDisbale = false;
        //me.propertyControl.nodeChanged(node, isDisbale);
        //if (node)
        //    me.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');

        var isSelectedInner = false;
        if (node) {
            if (!node.designerData || (node.designerData && node.designerData.NodeType !== "RoutingGroupNode")) {
                me.propertyControl.nodeChanged(node, isDisbale);
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
                        me.propertyControl.nodeChanged(currentNodeData, isDisbale);
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
     * 设置节点上下文
     * @method setContextMenu
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     */
    setContextMenu: function () {
        //TODO 右键菜单未实现
    },

    /**
     * 更新工序节点信息
     * @method updateProcessNodeInfo
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {product} product 运行时产品信息
     */
    updateProcessNodeInfo: function (product) {
        var me = this;
        if (me.version === null || me.version === undefined)
            return;

        //工序BOM已经改成点击工序节点时，从数据库查询，此处不再显示工序BOM
        if (!me.isWorkOrderLayout && product !== null) {
            me.updateProcessBoms(product);
        }

        var design = me.getDesignData();
        if (me.version.IsFinish) {
            //生产版本已经完工，所有工序节点标记为已过站
            if (design) {
                design.nodes.forEach(function (node) {
                    var designerData = node.designerData;
                    if (designerData.Type === 'Interaction')
                        designerData.ProcessState = 'Has';
                });
            }
        }
        else if (product) {
            var currentProcess = product.Routing.Processes.first(function (p) { return p.Id === product.Routing.CurrentId; });
            if (currentProcess !== null) {
                var currentNode = design.nodes.first(function (p) { return p.designerData.Index === currentProcess.Index; });
                me.designControl.InitProcessStatus(currentNode, product);
            }
        }
        me.designControl.designCanvas.updateNodesColor();
    },

    /**
     * 更新工序bom
     * @method updateProcessBoms
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {product} product 运行时产品信息
     */
    updateProcessBoms: function (product) {
        var me = this;
        if (product === null || product === undefined)
            return;
        var design = me.getDesignData();
        for (var item in design.nodes) {
            var node = design.nodes[item];

            if (!node.designerData)
                continue;

            var process = product.Routing.Processes.where(function (p) { return p.ProcessId === node.designerData.ProcessId; }).first();

            if (!process)
                continue;

            var processBoms = [];

            process.Boms.forEach(function (bom) {
                //var item = { ItemId: bom.ItemId, Qty: bom.Qty, IsBuckleMaterial: bom.IsBuckleMaterial };
                //node.designerData.ProcessBoms.push(item);
                processBoms.push({
                    ItemId: bom.ItemId,
                    Qty: bom.Qty,
                    IsBuckleMaterial: bom.IsBuckleMaterial,
                    Point: bom.Point,
                    WorkStepId: bom.WorkStepId,
                    IsAttachment: bom.IsAttachment,
                    IsExternal: bom.IsExternal,
                    IsSingleLabel: bom.IsSingleLabel,
                    IsRepeat: bom.IsRepeat,
                    HasBarcodeRule: bom.HasBarcodeRule,
                    MainMaterialId: bom.MainMaterialId
                });
            });

            node.designerData.ProcessBoms = processBoms;
        }
    },

    /**
     * 获取工艺路线设计数据
     * @method getDesignData
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @return {Object} 设计数据
     */
    getDesignData: function () {
        var me = this;
        return me.designControl.designCanvas.getDesignData();
    },
    //**************************************数据加载*************************************************
    /**
     * 加载生产产品信息
     * @method loadWipProductInfo
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Number} workOrderId 工单ID
     * @param {String} sn 产品条码
     */
    loadWipProductInfo: function (workOrderId, sn) {
        var me = this;
        me.invokeDataQuery('LoadWipProductData', [workOrderId, sn], function (result) {
            me.version = result.WipProductVersion;
            me.designControl.drawRouting(result.Layout);
            me.loadRoutingEvents(result.RoutingEventList);
            me.isWorkOrderLayout = result.IsWorkOrderLayout;
            me.setContextMenu();
            me.runtimeProduct = result.Product;
            me.updateProcessNodeInfo(me.runtimeProduct);
            me.designControl.updateCommandsStatus();
            me.updateBomCommandState();
        });
    },

    /**
     * 加载生产产品工序信息
     * @method loadWipProcessInfo
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Number} versionId 版本ID
     * @param {Number} processId 工序ID
     * @param {Number} productStatus 产品状态
     * @param {String} id 节点ID
     */
    loadWipProcessInfo: function (versionId, processId, productStatus, id, woId) {
        var me = this;
        me.invokeDataQuery('LoadWipProcessData', [versionId, processId, productStatus, id, woId], function (result) {
            me.loadDefects(result.DefectList);
            me.loadKeyItems(result.KeyItemList);
            me.loadRepaires(result.RepaireList);
            me.loadTestResults(result.TestResultList);
            me.loadBoms(result.BomList);
            me.updateBomCommandState();
        });
    },

    /**
     * 清除数据
     * @method clearProcessInfo
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     */
    clearProcessInfo: function () {
        var me = this;
        var defectView = me.dicRelationView['SIE.MES.WIP.Products.WipProductDefect'];
        if (defectView)
            defectView.setData(null);
        var keyItemView = me.dicRelationView['SIE.MES.WIP.Products.WipProductProcessKeyItem'];
        if (keyItemView)
            keyItemView.setData(null);
        var repairView = me.dicRelationView['SIE.MES.WIP.Products.WipProductRepair'];
        if (repairView)
            repairView.setData(null);
        var resultView = me.dicRelationView['SIE.MES.WIP.Products.WipProductTestResult'];
        if (resultView)
            resultView.setData(null);
        var bomView = me.dicRelationView['SIE.MES.RoutingSettings.ProductBomViewModel'];
        if (bomView)
            bomView.setData(null);
    },

    /**
     * 加载生产产品缺陷信息
     * @method loadDefects
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Array} data 缺陷列表
     */
    loadDefects: function (data) {
        var me = this;
        var defectView = me.dicRelationView['SIE.MES.WIP.Products.WipProductDefect'];
        if (!defectView)
            return;
        defectView.setData(null);
        defectView.getData().loadData(data);
    },

    /**
     * 加载关键件信息
     * @method loadKeyItems
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Array} data 关键件列表
     */
    loadKeyItems: function (data) {
        var me = this;
        var keyItemView = me.dicRelationView['SIE.MES.WIP.Products.WipProductProcessKeyItem'];
        if (!keyItemView)
            return;
        keyItemView.setData(null);
        keyItemView.getData().loadData(data);
    },

    /**
     * 加载生产产品维修记录信息
     * @method loadRepaires
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Array} data 维修记录列表
     */
    loadRepaires: function (data) {
        var me = this;
        var repairView = me.dicRelationView['SIE.MES.WIP.Products.WipProductRepair'];
        if (!repairView)
            return;
        repairView.setData(null);
        repairView.getData().loadData(data);
    },

    /**
     * 加载工艺路线事件信息
     * @method loadRoutingEvents
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Array} data 产品工艺路线变更信息列表
     */
    loadRoutingEvents: function (data) {
        var me = this;
        var eventView = me.dicRelationView['SIE.MES.WIP.Products.WipProductRoutingEvent'];
        if (!eventView)
            return;
        eventView.setData(null);
        eventView.getData().loadData(data);
    },

    /**
     * 加载生产产品测试结果信息
     * @method loadTestResults
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Array} data 测试结果列表
     */
    loadTestResults: function (data) {
        var me = this;
        var resultView = me.dicRelationView['SIE.MES.WIP.Products.WipProductTestResult'];
        if (!resultView)
            return;
        resultView.setData(null);
        resultView.getData().loadData(data);
    },

    /**
     * 加载工序bom信息
     * @method loadBoms
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Array} data 工序BOM列表
     */
    loadBoms: function (data) {
        var me = this;

        var bomView = me.dicRelationView['SIE.MES.RoutingSettings.ProductBomViewModel'];
        if (!bomView)
            return;
        var boms = me.initBoms(data);
        bomView.setData(null);
        bomView.getData().loadData(boms);
    },

    /**
     * 执行DataQueryer
     * @method invokeDataQuery
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {String} method 参数说明
     * @param {Array} params 参数说明
     * @param {Function} action 回调
     */
    invokeDataQuery: function (method, params, action) {
        var me = this;
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.ProductRoutings.ProductRoutingDataQueryer",
            method: method,
            token: token,
            params: params,
            success: function (res) {
                if (!res.Success)
                    return;
                action(res.Result);
            }
        });
    },

    /**
     * 清除数据
     * @method clearData
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     */
    clearData: function () {
        var me = this;
        for (var item in me.dicRelationView) {
            if (item === 'SIE.MES.WIP.Products.WipProductBarcode'
                || item === 'SIE.MES.WIP.Products.WipProductBarcodeCriteria') {
                continue;
            }
            var view = me.dicRelationView[item];
            view.getControl().setStore(null);
        }
    },

    /**
     * 获取选择节点
     * @method getSelectNode
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout 
     * @return {Object} 选中节点
     */
    getSelectNode: function () {
        var me = this;
        return me.designControl.designCanvas.selectNode;
    },

    /**
     * 更新BOM视图命令状态
     * @method updateBomCommandState
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     */
    updateBomCommandState: function () {
        var me = this;
        var bomView = me.dicRelationView['SIE.MES.RoutingSettings.ProductBomViewModel'];
        if (bomView) {
            bomView.isReady = true;
            bomView.syncCmdState(bomView, true);
        }
    },

    /**
     * 初始化工序bom
     * @method initBoms
     * @for SIE.Web.MES.ProductRoutings.ProductRoutingLayout
     * @param {Array} datas 数据库查询BOM列表
     * @return {Array} 处理后bom列表
     */
    initBoms: function (datas) {
        var me = this;
        var selectNode = me.getSelectNode();

        if (!selectNode)
            return;

        //var processId = selectNode.designerData.ProcessId;
        var activityId = selectNode.id;

        if (me.processBomDic[activityId]) {
            var bomList = me.processBomDic[activityId];

            datas.forEach(function (data) {
                var exist = bomList.where(function (p) { return p.id === data.id; }).first();

                if (!exist) {
                    if (selectNode.designerData.ProcessBoms) {
                        var nodeProcessBom = selectNode.designerData.ProcessBoms.where(function (p) { return p.ItemId === data.ItemId.toString(); }).first();

                        if (nodeProcessBom) {
                            data.IsBuckleMaterial = nodeProcessBom.IsBuckleMaterial;
                            data.Qty = nodeProcessBom.Qty;
                        }
                    }

                    data.ItemId_Display = data.Code;
                    data.ItemName = data.Name;
                    data.WorkStepId_Display = data.WorkStepName;
                    data.MainMaterialId_Display = data.MainItemCode;
                    bomList.push(data);
                }
            });
        }
        else {
            datas.forEach(function (data) {
                if (selectNode.designerData.ProcessBoms) {
                    var nodeProcessBom = selectNode.designerData.ProcessBoms.where(function (p) {
                        return p.ItemId === data.ItemId.toString();
                    }).first();

                    if (nodeProcessBom) {
                        data.IsBuckleMaterial = nodeProcessBom.IsBuckleMaterial;
                        data.Qty = nodeProcessBom.Qty;
                    }
                }

                data.ItemId_Display = data.Code;
                data.ItemName = data.Name;
                data.WorkStepId_Display = data.WorkStepName;
                data.MainMaterialId_Display = data.MainItemCode;
            });

            me.processBomDic[activityId] = datas;
        }

        return me.processBomDic[activityId];
    }
});