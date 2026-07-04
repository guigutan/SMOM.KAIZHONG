/*
 * 批次产品工艺路线布局JS
 * @class SIE.Web.MES.ProductRoutings.BatchRoutingLayout
 */
Ext.define('SIE.Web.MES.ProductRoutings.BatchRoutingLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    mainView: null,
    barcode: null,  //当前选中产品条码
    version: null,  //产品在制版本
    batchRelation: null,
    dicRelationView: {},
    designControl: null,
    isWorkOrderLayout: null,
    runtimeProduct: null,
    processBomDic: {},   //工序BOM字典：key工序id，value工序BOM集合
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
        me.designControl = Ext.create('SIE.Web.MES.BatchRoutingDesignControl', {
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
                layout: 'fit',
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
                    layout: 'fit',
                    border: 0,
                    height: 150,
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
    initTabPanel: function (tabPanel) {
        var me = this;
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductProcessKeyItem']);
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.RoutingSettings.ProductBomViewModel']);
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductDefect']);
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductRepaire']);
        me.addTabItem(tabPanel, me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductRoutingEvent']);
    },
    addTabItem: function (tabPanel, view) {
        tabPanel.items.push({
            title: view.getMeta().label.L10N(),
            items: view.getControl()
        });
    },
    registerEvent: function () {
        var me = this;
        var canvas = me.designControl.designCanvas;
        canvas.mon(canvas, 'nodeChanged', me.nodeChanged, me);
        //注册事件
        me.mainView.mon(me.mainView, 'currentChanged', me.barcodeChanged, me);
        me.mainView.mon(me.mainView, 'isReady', me.isReady, me);
        me.mainView.mon(me.mainView, 'beforeclosewin', me.beforeclosewin, me);
    },
    beforeclosewin: function () {
        var me = this;
        var canvas = me.designControl.designCanvas;
        canvas.mon(canvas, 'nodeChanged', me.nodeChanged, me);
        //注册事件
        var barcodeView = me.dicRelationView['SIE.MES.BatchWIP.WipBatchExt'];
        if (barcodeView) {
            barcodeView.mon(barcodeView, 'currentChanged', me.barcodeChanged, me);
        }
        me.mainView.mon(me.mainView, 'isReady', me.isReady, me);
        me.mainView.mon(me.mainView, 'beforeclosewin', me.beforeclosewin, me);
    },

    isReady: function (value) {
        var me = this;
        me.updateBomCommandState();
    },
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
    refreshWipProductInfo: function (barcode) {
        var me = this;
        me.barcode = barcode;
        me.version = null;
        me.processBomDic = {};
        me.isWorkOrderLayout = null;
        me.clearData();
        if (me.barcode) {
            me.loadWipProductInfo(barcode);
        }
        else {
            me.designControl.drawRouting(null);
            me.designControl.updateCommandsStatus();
            me.updateBomCommandState();
        }
    },

    //节点变更事件
    nodeChanged: function (mainView, node) {


        var me = mainView.layout;
        //工序节点变更时需要加载：产品缺陷记录、产品维修记录、产品生产关键件、产品测试结果、工序bom
        var designerData = {};
        me.designControl.updateSetCurrentStatus();
        if (node && me.version && node.nodeType === 'RoutingNode') {
            designerData = node.designerData;
            var productStatus = 2;   //在制
            if (!me.version) {
                productStatus = 0;   //未上线
            }
            else if (me.version.IsFinish)
                productStatus = 1;   //已完工
            me.loadWipProcessInfo(me.version.Id, designerData.ProcessId, productStatus);
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
        //    this.designControl.designCanvas.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');

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
        this.updateBomCommandState();


    },
    setContextMenu: function () {
        //TODO 右键菜单未实现
    },
    updateProcessNodeInfo: function (product) {
        var me = this;
        if (me.isWorkOrderLayout && product !== null) {
            me.updateProcessBoms(product);
        }

        var design = me.getDesignData();
        if (me.batchRelation && me.batchRelation.IsFinish) {
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

    updateProcessBoms: function (product) {
        var me = this;
        if (product === null || product === undefined)
            return;
        var design = me.getDesignData();
        var nodes = design.nodes;
        Ext.each(nodes, function (node) {
            if (node.designerData.ProcessId != undefined && node.designerData.ProcessId != 0) {
                var process = product.Routing.Processes.where(function (p) { return p.ProcessId === node.designerData.ProcessId && p.Index == node.designerData.Index }).first();
                if (process) {
                    if (node.designerData.ProcessBoms == undefined) {
                        node.designerData.ProcessBoms = [];
                    }
                    process.Boms.forEach(function (bom) {
                        var item = { ItemId: bom.ItemId, Qty: bom.Qty, IsBuckleMaterial: bom.IsBuckleMaterial };
                        node.designerData.ProcessBoms.push(item);
                    });
                }
            } else//工序组
            {
                for (var i = 0; i < node.groupDesignerData.length; i++) {
                    var nodeDesignerData = node.groupDesignerData[i].designerData;
                    var processGroupDesigner = product.Routing.Processes.where(function (p) { return p.ProcessId === nodeDesignerData.ProcessId && p.Index == nodeDesignerData.Index }).first();
                    if (processGroupDesigner) {
                        processGroupDesigner.Boms.forEach(function (bom) {
                            var item = { ItemId: bom.ItemId, Qty: bom.Qty, IsBuckleMaterial: bom.IsBuckleMaterial };
                            nodeDesignerData.ProcessBoms.push(item);
                        });
                    }
                }

            }
        });
    },
    getDesignData: function () {
        var me = this;
        return me.designControl.designCanvas.getDesignData();
    },
    //**************************************数据加载*************************************************

    loadWipProductInfo: function (subWipBatch) {
        var me = this;
        me.invokeDataQuery('LoadWipProductData', [subWipBatch], function (result) {
            me.designControl.designCanvas.selectNode = null;
            me.version = result.WipProductVersion;
            me.batchRelation = result.BatchRelation;
            me.designControl.drawRouting(result.Layout);
            me.loadRoutingEvents(result.RoutingEventList);
            me.isWorkOrderLayout = result.IsWorkOrderLayout;
            //TODO设置工序节点状态
            me.setContextMenu();
            me.runtimeProduct = result.Product;
            me.updateProcessNodeInfo(me.runtimeProduct);
            me.designControl.updateCommandsStatus();
        });
    },
    loadWipProcessInfo: function (versionId, processId, productStatus) {
        var me = this;
        me.invokeDataQuery('LoadWipProcessData', [me.barcode.Id, versionId, processId, productStatus, productStatus === 2 ? me.runtimeProduct : null], function (result) {
            me.loadDefects(result.DefectList);
            me.loadKeyItems(result.KeyItemList);
            me.loadRepaires(result.RepaireList);
            me.loadBoms(result.BomList);
        });
    },
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
    loadDefects: function (data) {
        var me = this;
        var eventView = me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductDefect'];
        if (!eventView)
            return;
        eventView.setData(null);
        eventView.getData().loadData(data);
    },
    loadKeyItems: function (data) {
        var me = this;
        var eventView = me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductProcessKeyItem'];
        if (!eventView)
            return;
        eventView.setData(null);
        eventView.getData().loadData(data);
    },
    loadRepaires: function (data) {
        var me = this;
        var eventView = me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductRepaire'];
        if (!eventView)
            return;
        eventView.setData(null);
        eventView.getData().loadData(data);
    },
    loadRoutingEvents: function (data) {
        var me = this;
        var eventView = me.dicRelationView['SIE.MES.BatchWIP.Products.BatchWipProductRoutingEvent'];
        if (!eventView)
            return;
        eventView.setData(null);
        eventView.getData().loadData(data);
    },
    loadBoms: function (data) {
        var me = this;
        var eventView = me.dicRelationView['SIE.MES.RoutingSettings.ProductBomViewModel'];
        if (!eventView)
            return;
        var boms = me.initBoms(data);
        eventView.setData(null);
        eventView.getData().loadData(boms);
    },
    invokeDataQuery: function (method, params, action) {
        var me = this;
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.BatchProductRoutings.BatchProductRoutingDataQueryer",
            method: method,
            token: token,
            params: params,
            async: true,
            success: function (res) {
                if (!res.Success)
                    return;
                action(res.Result);
            }
        });
    },
    //清除数据：
    clearData: function () {
        var me = this;
        for (var item in me.dicRelationView) {
            if (item === 'SIE.MES.BatchWIP.WipBatchExt' || item === 'SIE.MES.BatchWIP.BatchCriteria')
                continue;
            var view = me.dicRelationView[item];
            view.getControl().setStore(null);
        }
    },
    getSelectNode: function () {
        var me = this;
        return me.designControl.designCanvas.selectNode;
    },
    updateBomCommandState: function () {
        var me = this;
        var bomView = me.dicRelationView['SIE.MES.RoutingSettings.ProductBomViewModel'];
        if (bomView) {
            bomView.isReady = true;
            bomView.syncCmdState(bomView, true);
        }
    },
    initBoms: function (datas) {
        var me = this;
        var selectNode = me.getSelectNode();
        if (selectNode == undefined) {
            return [];
        }
        var processId = selectNode.id;
        if (me.processBomDic[processId]) {
            var bomList = me.processBomDic[processId];
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
                    bomList.push(data);
                }
            });
        }
        else {

            datas.forEach(function (data) {
                if (selectNode.designerData.ProcessBoms) {
                    var nodeProcessBom = selectNode.designerData.ProcessBoms.where(function (p) { return p.ItemId === data.ItemId.toString(); }).first();
                    if (nodeProcessBom) {
                        data.IsBuckleMaterial = nodeProcessBom.IsBuckleMaterial;
                        data.Qty = nodeProcessBom.Qty;
                    }
                }
                data.ItemId_Display = data.Code;
                data.ItemName = data.Name;
            });
            me.processBomDic[processId] = datas;
        }
        return me.processBomDic[processId];
    }
});