"use strict";


function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }



DrawingTools = function DrawingTools(container, initConfig) {
    var _drawingInstance;

    var _addEndpoints;

    var _maxAutoNodeId = 0;
    var _autoNodePre = 'node';
    var _nodeClass = 'jtk-node';

    var offsetX = 0;
    var offsetY = 0;
    var _canDrag = true; //是否可拖动节点

    var _nodeList = {};//节点列表

    var _beforeDrawNode;

    var _afterConnection;

    /**
     * 默认缩放值
     * */
    var baseZoom = 1;

    var groupIndex = 0;

    //工序组key
    var groupNodeKey = "RoutingGroupNode";

    var me;
    var isLock = false;
    var nodeClickEvent, lineClickEvent, lineDbClickEvent, canvasClickEvent;

    /**
     * 初始化
     * container:容器，即dom的Id或dom对象,必须
     * config 配置信息
     * {
     *   
     *   ContainerDecorate:{} 容器装饰配置
     * }
     */
    var init = function init(curObj, container, config) {
        me = curObj;
        setConfig(config);
        createDrawInstance(container, config);
    };

    /*
     * 工序组里面的块的右键菜单事件
     * 
     */
    customContextMenu = function customContextMenu(event) {
        event.preventDefault ? event.preventDefault() : (event.returnValue = false);
        event.stopPropagation();
    };
    /*
     * 删除工序组里面的节点
     */
    var _deleteNode = function _deleteNode(currentBlock) {
        if (currentBlock) {
            var id = currentBlock.id;
            var parentNode = currentBlock.parentNode.parentNode;
            me.removeNode(currentBlock.id);
            var groupNodeData = me.getNode(parentNode.id);

            //移除组里面的数据
            var currentNodeData = null;
            groupNodeData.groupDesignerData.forEach(function (nodeData) {

                if (nodeData.id == id) {
                    currentNodeData = nodeData;
                    return;
                }
            }
            );
            if (currentNodeData) {
                groupNodeData.groupDesignerData.remove(currentNodeData);

                if (groupNodeData.groupDesignerData.length == 1)//只剩下单个的时候 单独拉出来并删除该工序组
                {
                    //先铲除原先的点
                    window.jsp.deleteConnectionsForElement(parentNode.id);//删除divID所有连接线
                    window.jsp.removeAllEndpoints(parentNode.id);//删除divID所有端点
                    me.removeNode(parentNode.id);
                    me.deleteNodeListById(parentNode.id);

                    //注意： 必须先删除工序组重新创建的工序才会可拖动 需将原先工序组位置赋值到新的块里面中
                    groupNodeData.groupDesignerData[0].left = parseInt(parentNode.style.left.replace("px", ""));
                    groupNodeData.groupDesignerData[0].top = parseInt(parentNode.style.top.replace("px", ""));
                    var resultNode = me.addSingleNode(groupNodeData.groupDesignerData[0]);
                    if (resultNode) {

                    }
                } else {
                    me.changeGroupNodeLine(groupNodeData);
                }
            }
        }
    };


    getCanvasDom = function getCanvasDom() {
        var canvasDom = document.getElementById("techCanvas");
        if (!canvasDom) {
            canvasDom = document.getElementById("workOrderRoutingCanvas");
        }
        //产品
        if (!canvasDom) {
            canvasDom = document.getElementById("productRoutingCanvas");
        }
        //批次
        if (!canvasDom) {
            canvasDom = document.getElementById("batchRoutingCanvas");
        }
        
        return canvasDom;
    }
    /*
     * *更改Canvas尺寸
     */
    changeCanvasSize = function changeCanvasSize(e, dragEl) {

        //canvas
        var canvasDom = getCanvasDom();

        if (dragEl && canvasDom) {
            const threshold = 0;//阈值 
            var canvasHeight = canvasDom.clientHeight;
            var canvasWidth = canvasDom.clientWidth;

            var height = dragEl.clientHeight;
            var width = dragEl.clientWidth;
            var left = dragEl.offsetLeft;
            var top = dragEl.offsetTop;

            //计算拖动元素的右下方顶点进行判断
            var rigth = left + width + threshold;
            var bottom = top + height + threshold;

            //获取canvas的滚动条元素
            var scrollDom = canvasDom.parentNode.parentNode.parentNode;
            if (!scrollDom)
                scrollDom = canvasDom.parentElement.parentElement.parentElement;


            //增加画布宽度,每次增加20%
            if (rigth >=canvasWidth) {
                canvasDom.style.width = (parseInt(canvasDom.style.width.replace("%")) + 20) + "%";
                if (scrollDom) {
                    scrollDom.scrollLeft= canvasDom.scrollWidth; //滚动条如果跟着滚 会有跳动的感觉 
                }
            }
            //增加画布高度,每次增加200px
            if (bottom >=canvasHeight) {
                canvasDom.style.height = (parseInt(canvasDom.style.height.replace("px")) + 200) + "px";
                //滚动条设置到最大
                if (scrollDom) {
                    scrollDom.scrollTop = canvasDom.scrollHeight;
                }
            }
        }
    };



    /**
     * 添加到工序组
     * */

    addToGroupDrogEnd = function addToGroupDrogEnd(e, dragEl) {
        if (isLock) {
            return;
        }
        var dom = document.elementFromPoint(e.x, e.y);
        var lastNode = 0;
        while (dom && dom.getAttribute("data-nodetype") != me.getGroupNodeKey()) {
            dom = dom.parentNode;
            lastNode++;
            if (lastNode == 4) {//最多循环5层找到父级
                break;
            }
        }

        var currentDom = dragEl;
        if (dom && currentDom) {
            var nodetype = dom.getAttribute("data-nodetype");

            if (nodetype == me.getGroupNodeKey() && currentDom.dataset && currentDom.dataset.nodetype != me.getGroupNodeKey()) {
                var groupNodeData = me.getNode(dom.id);//获取工序组的数据
                var nodeData = me.getNode(currentDom.id);//获取拖动块的数据
                if (nodeData && nodeData.designerData.ProcessType == "Fix") {
                    SIE.Msg.showMessage("维修类型工序不允许加入工序组！".t());
                    return;
                }

                var selectNodeInfo = DesignerNode.CreateNode(nodeData.nodeType);//创建节点
                if (selectNodeInfo.element) {
                    if (typeof selectNodeInfo.element === 'string') {
                        me.removeNode(currentDom.id);
                        selectNodeInfo.element = selectNodeInfo.element.replace("float:left; width:70px;height:40px;", "vertical-align: middle;margin-bottom:10px");
                        selectNodeInfo.element = selectNodeInfo.element.replace("float:left;width:70px; padding-left:5px; height:40px;", "float:left;width:70px;height:40px;");
                        selectNodeInfo.element = selectNodeInfo.element.replace("padding-left:5px; padding-top:5px;", "padding-left:5px; padding-top:5px; margin-bottom:10px;margin:5px");
                        selectNodeInfo.element = selectNodeInfo.element.replace('class="node"', 'class="node", oncontextmenu="customContextMenu(event);" id="' + currentDom.id + '"');
                        // delete _nodeList[currentDom.id];

                        //移除数据后将数据整合到工序组的集合数据中
                        me.deleteNodeListById(currentDom.id);
                        groupNodeData.groupDesignerData.push(nodeData);
                        me.deleteNodeListById(currentDom.id);

                        var element = me.dwhandleTemplate(selectNodeInfo.element, nodeData);
                        var wrapper = document.createElement('div');
                        wrapper.innerHTML = element;
                        var elementDom = wrapper.firstChild;
                        dom.children[1].appendChild(elementDom);

                        var childRightClick = Ext.create('Ext.menu.Menu', {
                            width: 50,
                            height: 50,
                            floating: true,
                            items: [{
                                text: '删除节点'.L10N(),
                                handler: function () {
                                    if (isLock) return;
                                    clearTimeout(300);
                                    me.deleteNode(elementDom);
                                }
                            }
                            ]
                        });
                        elementDom.addEventListener('contextmenu', function (e) {
                            childRightClick.showAt([e.clientX, e.clientY]);
                            event.preventDefault();
                        });

                        me.changeGroupNodeLine(groupNodeData)
                        // me.drawNode(nodeList);
                    }
                }
            }
        }
    }

    /**
     * 添加单个节点
     * */
    var _addSingleNode = function addSingleNode(nodeInfo) {

        nodeInfo.id = generateId();
        var selectNodeInfo = DesignerNode.CreateNode(nodeInfo.nodeType);//创建节点
        if (selectNodeInfo.element) {
            if (typeof selectNodeInfo.element === 'string') {
                var element = handleTemplate(selectNodeInfo.element, nodeInfo);
                dragObj = createElement(element);
                if (!dragObj) return;
                var newObj = dragObj.cloneNode(true);
                newObj.style.position = 'absolute';
                newObj.style.float = "left";

                //取外部工序组的位置作为新的位置
                newObj.style.left = nodeInfo.left + 'px';; //this.offsetX + 'px';
                newObj.style.top = nodeInfo.top + 'px';;// this.offsetY + 'px';

                newObj.style.width = newObj.style.width || dragObj.offsetWidth + 'px';
                newObj.style.height = newObj.style.height;
                if (newObj.attributes["draggable"]) {
                    newObj.attributes.removeNamedItem('draggable');
                }

                if (newObj.attributes["ondragstart"]) {
                    newObj.attributes.removeNamedItem('ondragstart');
                }
                var node = _createNode(newObj.outerHTML, nodeInfo, nodeInfo.id);
                drawBase.apply(node, nodeInfo);
                var nodeList = [];
                nodeList.push(node);
                _drawNode(null, nodeList);
                return node;
            }

        }
    };

    /**
     * 设置配置信息
     * @param {配置信息} config 
     */
    var setConfig = function setConfig(config) {
        if (!config) return;

        if (config.canDrag != undefined) {
            _canDrag = config.canDrag;
        }

        if (config.beforeDrawNode && Ext.isFunction(config.beforeDrawNode)) _beforeDrawNode = config.beforeDrawNode;
        if (config.connection && Ext.isFunction(config.connection)) handlers.connection = config.connection;
        if (config.beforeDrop && Ext.isFunction(config.beforeDrop)) handlers.beforeDrop = config.beforeDrop;
    };

    /**
     * 创建画图实例
     * @param {容器} container 
     * @param {配置信息} config 
     */
    var createDrawInstance = function createDrawInstance(container, config) {
        jsPlumb.ready(function () {
            //初始化画线对象
            _drawingInstance = window.jsp = jsPlumb.getInstance({
                DragOptions: {
                    cursor: 'pointer',
                    zIndex: 2000
                },
                ConnectionOverlays: [["Arrow", {
                    id: 'arrow',
                    location: 1,
                    visible: true,
                    width: 11,
                    length: 11,
                    events: {//click: function () { alert("线箭头") }
                    }
                }],
                ["Label", {
                    //location: 0.1,
                    id: 'label',
                    label: "",
                    //cssClass: "aLabel",
                    //events: {
                    //    tap: function () {
                    //        alert("线标签");
                    //    }
                    //}
                }]
                ] // Container: container

            }); //画布装饰

            canvasDecorate(config);
            var basicType = {
                connector: "Straight",
                paintStyle: {
                    stroke: "red",
                    strokeWidth: 4
                },
                hoverPaintStyle: {
                    stroke: "blue"
                },
                overlays: ["Arrow"]
            };

            _drawingInstance.registerConnectionType("basic", basicType); // this is the paint style for the connecting lines..


            var connectorPaintStyle = {
                strokeWidth: 2,
                stroke: "#3B444D",
                joinstyle: "round",
                outlineStroke: "white",
                outlineWidth: 2
            },
                // .. and this is the hover style.
                connectorHoverStyle = {
                    strokeWidth: 2,
                    stroke: "#000000",
                    outlineWidth: 2,
                    outlineStroke: "white"
                },
                endpointHoverStyle = {
                    //fill: "#216477",
                    stroke: "#216477"
                },
                // the definition of source endpoints (the small blue ones)
                sourceEndpoint = {
                    canvas: container,
                    scope: container,
                    endpoint: "Dot",
                    paintStyle: {
                        stroke: "#7AB02C",
                        //fill: "transparent",
                        //radius: 5
                        radius: 7,
                        zIndex: 20,
                        strokeWidth: 0
                    },
                    isSource: true,
                    // connector: [ "Flowchart", { stub: [40, 60], gap: 10, cornerRadius: 5, alwaysRespectStubs: true } ],
                    connector: ["Straight"],
                    //connector: ["Flowchart"],BillPrintable
                    connectorStyle: connectorPaintStyle,
                    hoverPaintStyle: endpointHoverStyle,
                    connectorHoverStyle: connectorHoverStyle,
                    dragOptions: {//禁止拖动
                        //disableDrag: false,
                    },
                    maxConnections: 1,
                    overlays: [["Label", {
                        location: [0.5, 1.5],
                        label: "Drag",
                        cssClass: "endpointSourceLabel",
                        visible: false
                    }]]
                },
                // the definition of target endpoints (will appear when the user drags a connection)
                targetEndpoint = {
                    canvas: container,
                    scope: container,
                    endpoint: "Dot",
                    anchor: "Continuous",
                    paintStyle: {
                        fill: "#7AB02C",
                        //fill: "transparent",
                        radius: 7,
                        zIndex: 20,
                        strokeWidth: 0
                    },
                    hoverPaintStyle: endpointHoverStyle,
                    maxConnections: -1,
                    //allowLoopback: true,
                    dropOptions: {
                        hoverClass: "hover",
                        activeClass: "active"
                    },
                    isTarget: true
                }; //注册流程线

            _drawingInstance.registerConnectionType("flowchart", {
                paintStyle: {
                    stroke: "blue"
                },
                connector: ["Flowchart"],
                overlays: [["Arrow", {
                    location: 1,
                    visible: true,
                    width: 11,
                    length: 11,
                    events: {//click: function () { alert("1") }
                    }
                }], ["Label", {
                    id: 'label',
                    label: '<button class="delete-node-btn">X</button>',
                    //cssClass: "aLabel",
                    //events: {
                    //    tap: function () {
                    //        alert("线标签");
                    //    }
                    //}
                }]]
            }); //添加节点锚点


            _addEndpoints = function _addEndpoints(toId, sourceAnchors, targetAnchors, canDrag, connector, node, sourcePortNumber) {
                sourceEndpoint.dragOptions.disableDrag = canDrag == false;
                sourceEndpoint.connector = connector || ["Straight"];
                sourceEndpoint.connectionsDetachable = !isLock;
                targetEndpoint.connectionsDetachable = !isLock;

                for (var i = 0; i < sourceAnchors.length; i++) {
                    //克隆节点配置
                    var nodeSourceEndpoint = Ext.clone(sourceEndpoint); //合并节点配置

                    if (node && node.designerData && node.designerData.SourceAnchor && node.designerData.SourceAnchor[i]) {
                        nodeSourceEndpoint = Ext.applyIf(node.designerData.SourceAnchor[i], nodeSourceEndpoint);
                    }

                    var anchorType = nodeSourceEndpoint.AnchorType + nodeSourceEndpoint.paramId || 'SourcePort' + (sourcePortNumber ? sourcePortNumber : i);

                    var sourceUUID = toId + anchorType; //添加节点

                    var sourceDiv = _drawingInstance.addEndpoint(toId, nodeSourceEndpoint, {
                        anchor: sourceAnchors[i],
                        uuid: sourceUUID
                    }); //设置画线后的标签


                    if (nodeSourceEndpoint.LineLabel) {
                        sourceDiv.canvas.dataset.LineLabel = nodeSourceEndpoint.LineLabel;
                        sourceDiv.canvas.dataset.AnchorType = nodeSourceEndpoint.AnchorType;
                    } //设置锚的工序参数


                    if (node && node.designerData && node.designerData.SourceAnchor && node.designerData.ProcessParameter) {
                        var processParameter = node.designerData.ProcessParameter[i];
                        if (processParameter) sourceDiv.canvas.dataset.parameter = JSON.stringify(processParameter);
                    }
                } //for (var j = 0; j < targetAnchors.length; j++) {
                //    var targetUUID = toId + 'TargetPort'+j;
                //    _drawingInstance.addEndpoint(toId, targetEndpoint, { anchor: targetAnchors[j], uuid: targetUUID });
                //}


                if (node.designerData.Type != 'Initial') _drawingInstance.makeTarget(toId, targetEndpoint);
            }; //jsPlumb.fire("jsPlumbDemoLoaded", _drawingInstance);

            //初始化节点、线条、画布事件
            initEvent(config);
        });
    };

    //初始化节点、线条、画布事件
    var initEvent = function initEvent(config) {
        var instance = _getCurInstance();

        nodeClickEvent = document.createEvent("HTMLEvents");
        nodeClickEvent.initEvent("nodeClick", false, true);
        lineClickEvent = document.createEvent('HTMLEvents');
        lineClickEvent.initEvent("lineClick", false, true);
        lineDbClickEvent = document.createEvent('HTMLEvents');
        lineDbClickEvent.initEvent("lineDbClick", false, true);
        canvasClickEvent = document.createEvent('HTMLEvents');
        canvasClickEvent.initEvent("canvasClick", false, true);
        drawBase.apply(me.listeners, config.listeners);
        instance.bind("connection", function (connInfo, originalEvent) {
            //设置连线id
            connInfo.connection.id = generateId(); //var sourceNode = getNode(connInfo.sourceId);

            if (connInfo.sourceId == connInfo.targetId) {
                var flowchartType = this._connectionTypes['flowchart'];

                if (flowchartType) {
                    var paintStyle = connInfo.connection.getPaintStyle();

                    if (paintStyle && paintStyle.stroke) {
                        flowchartType.paintStyle.stroke = paintStyle.stroke;
                    }

                    connInfo.connection.setType("flowchart");
                }
            }

            var lineLabel = null;
            if (connInfo.sourceEndpoint.canvas.dataset.LineLabel) lineLabel = connInfo.sourceEndpoint.canvas.dataset.LineLabel;
            if (lineLabel && !connInfo.connection.getLabel()) {
                if (originalEvent) {
                    connInfo.connection.getOverlay("label").setLabel(lineLabel);
                }
                else {
                    connInfo.connection.getOverlay("label").setLabel(connInfo.linelabel);
                }

            }
            listenEvent(connInfo.connection.canvas, 'lineClick', me.listeners.lineClick);
            listenEvent(connInfo.connection.canvas, 'lineDbClick', me.listeners.lineDbClick(connInfo.connection));
            handlers.connection(connInfo);
        }); //线条点击事件

        instance.bind("click", function (connInfo, originalEvent) {
            connInfo.canvas.dispatchEvent(lineClickEvent);
            event.preventDefault();
        }); //线条双击事件

        instance.bind("dblclick", function (connInfo, originalEvent) {
            connInfo.canvas.dispatchEvent(lineDbClickEvent);
            event.preventDefault();
        });
        instance.bind("beforeStart", function (connection) {//未知该事件用途
            //alert('dd');
        }); //线条从锚点拖动事件

        instance.bind("beforeDrop", function (connInfo) {
            var result = handlers.beforeDrop(connInfo);
            return result;
        }); //连接建立前事件

        instance.bind("connectionDrag", function (connection) {
            if (!_canDrag) {//delete connection;
            }
        }); //线条拖动结束事件

        instance.bind("connectionDragStop", function (connection) { }); //线条移动事件

        instance.bind("connectionMoved", function (connInfo, originalEvent) { }); //连接移动事件
    };

    //监听节点元素事件
    var listenEvent = function listenEvent(element, eventName, eventHandlers) {
        if (!eventHandlers) return;

        if (Array.isArray(eventHandlers)) {
            eventHandlers.forEach(function (ev) {
                element.addEventListener(eventName, ev);
            });
        } else {
            element.addEventListener(eventName, eventHandlers);
        }
    };

    //设置容器
    var _setContainer = function setContainer(container) {
        var instance = _getCurInstance();

        instance.setContainer(container);
        canvasDecorate();
        var container = instance.getContainer();
        //设置画布单击事件
        container.addEventListener('mousedown', function () {
            event.srcElement.dispatchEvent(canvasClickEvent);
            event.preventDefault();
        });
        //设置画布滚轮事件
        container.addEventListener('mousewheel', function (ent) {
            _mousewheelHandler(ent);
        });

        listenEvent(container, 'canvasClick', me.listeners.canvasClick);
        _resetZoom();

    };

    /**
     * 重置缩放
     * */
    var _resetZoom = function resetZoom() {
        if (baseZoom !== 1) {
            baseZoom = 1;
            const zoom = baseZoom;
            _zoomScale(zoom);
            window.jsp.setZoom(zoom);
        }
    };

    /**
     * 缩放是整个画布及其内容一起缩放
     * @param {any} scale 缩放参数
     */
    var _zoomScale = function zoomScale(scale) {

        var techCanvas =getCanvasDom();
        //var techCanvas = document.getElementById("techCanvas");
        if (techCanvas) {

            techCanvas.style.setProperty("-webkit-transform", 'scale(' + scale + ')');
            techCanvas.style.setProperty("-moz-transform", 'scale(' + scale + ')');
            techCanvas.style.setProperty("-ms-transform", 'scale(' + scale + ')');
            techCanvas.style.setProperty("-o-transform", 'scale(' + scale + ')');
            techCanvas.style.setProperty("transform", 'scale(' + scale + ')');
            techCanvas.style.setProperty("transform-origin", "0px 0px");
        }
    };
    /**
     * 放大
     * */
    var _zoomin = function zoomin() {
        if (baseZoom + 0.05 >= 2) {
            return;
        }
        baseZoom += 0.05;
        const zoom = baseZoom;
        _zoomScale(zoom);
        window.jsp.setZoom(zoom);
    };

    /**
     * 缩小
     * */
    var _zoomout = function zoomout() {
        if (baseZoom - 0.1 < 0.4) {
            return;
        }
        baseZoom -= 0.05;

        if (baseZoom > 0) {
            const zoom = baseZoom;
            _zoomScale(zoom);
            window.jsp.setZoom(zoom);
        } else {
            baseZoom += 0.05;
        }
    }


    /*
     *处理鼠标滚轮事件
     */
    _mousewheelHandler = function mousewheelHandler(evt) {
        if (evt.shiftKey) {

            const direction = _scrollFunc(evt);
            if (direction == 1) {
                _zoomin();
            } else {
                _zoomout();
            }
            //阻止滚动条继续执行其原来功能
            event.preventDefault();
        }
    };

    /**
     * 滚动条正负缩放判断
     * @param {any} e
     */
    var _scrollFunc = function scrollFunc(e) {
        var e = e || window.event;
        if (e.wheelDelta) {
            if (e.wheelDelta > 0) {     //当鼠标滚轮向上滚动时
                return 1;
            }
            if (e.wheelDelta < 0) {     //当鼠标滚轮向下滚动时
                return -1;
            }
        } else if (e.detail) {
            if (e.detail < 0) {   //当鼠标滚轮向上滚动时
                return 1;
            }
            if (e.detail > 0) {   //当鼠标滚轮向下滚动时
                return -1;
            }
        }
    };


    //获取画图对像
    var _getCurInstance = function getCurInstance() {
        return _drawingInstance;
    };

    /**
     * 画布装饰
     * @param {json对象} config 
     */
    var canvasDecorate = function canvasDecorate(config) {
        var container = _getCurInstance().getContainer();

        if (container) {
            //画布默认样式
            var defaults = {// backgroundColor: '#E8EFF7',
                // backgroundImage: 'linear-gradient(0deg, #DDDDDD 1.1px, transparent 0), linear-gradient(90deg, #DDDDDD 1.1px, transparent 0)',
                // backgroundSize: '30px 30px',
                //backgroundImage: "url('/content/images/drawtools/dot_bg.jpg')",
                //width: '100%',
                //height: '100%',
                //backgroundRepeat: 'repeat'
            };
            var containerDecorate = config && config.ContainerDecorate ? config.ContainerDecorate : {};
            drawBase.apply(_drawingInstance.getContainer().style, containerDecorate, defaults);
        }
    };

    /**
     * 创建节点
     * @param {any} html
     * @param {any} nodeInfo
     * @param {any} nodeId
     */
    var _createNode = function createNode(html, nodeInfo, nodeId) {
        if (!(html && (_typeof(html) === 'object' || typeof html === 'string'))) return;
        var element = createElement(html); //var node = drawBase.create('RoutingNode');

        var node = drawBase.create(nodeInfo.nodeType);
        if (!node.id) node.id = generateId();
        if (nodeId) {
            node.id = nodeId;
        }
        drawBase.apply(node.designerData, nodeInfo.designerData);
        appendTo(element.outerHTML, node);
        addNodeToList(node);
        listenEvent(element, 'nodeClick', me.listeners.nodeClick);
        return node;
    };

    var addNodeToList = function addNodeToList(node) {
        if (!_nodeList.hasOwnProperty(node.id)) {
            _nodeList[node.id] = node;
        }
    };

    var clearNodeList = function clearNodeList() {
        for (var key in _nodeList) {
            delete _nodeList[key];
        }
    };

    /**
     * 根据html字符串创建dom元素
     * @param {html字符串} html 
     */
    var createElement = function createElement(html) {
        var wrapper = document.createElement('div');
        wrapper.innerHTML = html;
        var element = wrapper.firstChild;
        return element;
    };

    //附加节点
    var appendTo = function appendTo(element, nodeInfo) {
        var node;
        var touchtime = null;

        if (typeof element == 'string') {
            element = handleTemplate(element, nodeInfo);
            node = createElement(element);
        } else {
            node = element;
        }

        node.id = nodeInfo.id;
        node.style.left = nodeInfo.left + 'px';
        node.style.top = nodeInfo.top + 'px';
        node.style.width = node.style.width || nodeInfo.width + 'px';
        node.style.height = node.style.height || nodeInfo.height + 'px';
        node.style.position = 'absolute';
        node.style.zIndex = 20;
        node.setAttribute('class', 'jtk-node');
        node.addEventListener('click', function (e) {
            clearTimeout(touchtime);
            var evt = event;
            touchtime = setTimeout(function () {
                var srcElement = node;
                node.dispatchEvent(nodeClickEvent);
                evt.preventDefault();
            }, 300);
        }, true);

        if (node.dataset.nodetype !== 'EndNode') {
            //添加右键菜单
            var rightClick = null;
            if (node.dataset.nodetype !== me.getGroupNodeKey()) { //'RoutingGroup' //单工序组

                if (nodeInfo.designerData.ProcessType === "Fix")//维修工序不允许添加工序组
                {
                    rightClick = Ext.create('Ext.menu.Menu', {
                        width: 50,
                        height: 50,
                        floating: true,
                        items: [{
                            text: '删除节点'.L10N(),
                            handler: function () {
                                if (isLock) return;
                                clearTimeout(touchtime);
                                me.getCurInstance().remove(node);
                            }
                        }]
                    });
                } else {
                    if (node.dataset.nodetype === 'BeginNode') {
                        rightClick = Ext.create('Ext.menu.Menu', {
                            width: 50,
                            height: 50,
                            floating: true,
                            items: [{
                                text: '增加分支'.L10N(),
                                handler: function () {
                                    if (isLock) return;
                                    clearTimeout(touchtime);
                                    me.addNewBranch(node);
                                }
                            }]
                        });

                    } else {
                        rightClick = Ext.create('Ext.menu.Menu', {
                            width: 50,
                            height: 50,
                            floating: true,
                            items: [{
                                text: '删除节点'.L10N(),
                                handler: function () {
                                    if (isLock) return;
                                    clearTimeout(touchtime);
                                    me.getCurInstance().remove(node);
                                }
                            },
                            {
                                text: '添加工序组'.L10N(),
                                handler: function () {
                                    if (isLock) return;
                                    clearTimeout(touchtime);
                                    me.toGroup();
                                }
                            }
                            ]
                        });
                    }
                }

            } else {
                node.style.height = "";
                node.style.zIndex = 99;
                rightClick = Ext.create('Ext.menu.Menu', {
                    width: 50,
                    height: 50,
                    floating: true,
                    items: [{
                        text: '删除工序组'.L10N(),
                        handler: function () {
                            if (isLock) return;
                            clearTimeout(touchtime);
                            me.getCurInstance().remove(node);
                        }
                    }
                    ]
                });
            }
            //发布后的不再加右键菜单
            if (rightClick != null && !isLock) {
                node.addEventListener('contextmenu', function (e) {
                    rightClick.showAt([e.clientX, e.clientY]);
                    event.preventDefault();
                });
            }
        }

        listenEvent(node, 'nodeClick', me.listeners.nodeClick);

        var container = _getCurInstance().getContainer();

        var mappingId = _getCurInstance().setMappingId(node.id);

        //node.id = nodeInfo.id = mappingId;
        container.appendChild(node);
        var nodeDom = document.getElementById(node.id);

        if (nodeDom) {
            if (node.dataset.nodetype === me.getGroupNodeKey()) {
                for (var i = 0; i < nodeDom.childNodes[1].childNodes.length; i++) {
                    var childNodeDom = nodeDom.childNodes[1].childNodes[i];


                    if (childNodeDom) {
                        var childRightClick = Ext.create('Ext.menu.Menu', {
                            width: 50,
                            height: 50,
                            floating: true,
                            items: [{
                                text: '删除节点'.L10N(),
                                value: childNodeDom.id,
                                handler: function (menuItem, envent) {
                                    if (isLock) return;
                                    clearTimeout(touchtime);
                                    var resultChildNodeDom = document.getElementById(menuItem.value);
                                    if (resultChildNodeDom) {
                                        me.deleteNode(resultChildNodeDom);
                                    }
                                }
                            }
                            ]
                        });
                        childNodeDom.addEventListener('contextmenu', function (e) {
                            childRightClick.items.items[0].value = e.currentTarget.id;
                            childRightClick.showAt([e.clientX, e.clientY]);
                            event.preventDefault();
                        });
                    }
                }
            }
        }
    };

    /*
     *添加工序组
     */
    var _toGroup = function () {
        var selectNodes = CRT.Context.PageContext.getContext('selectNodes');
        if (!selectNodes || selectNodes.length <= 0) {
            return;
        }
        _createGroup(selectNodes);
    }

    /**
     * 加载工序组
     * @param {any} design
     * @param {any} groupsNodes
     */
    var _loadGroups = function (design, groupsNodes) {

        if (groupsNodes && groupsNodes.length > 0) {
            //1.分组
            //2.将每组的生成对应的工序组
            const sorted = _groupBy(groupsNodes, function (item) {
                return [item.designerData.GroupId];//按照name进行分组
            });
            sorted.forEach(function (selectNodes) {
                _createGroup(selectNodes, design, true);
            });
        }
        
    }

    /**
     * 添加新分支
     * @param {any} node 开始节点
     */
    var _addNewBranch = function _addNewBranch(node) {
        //已存在一个
        var nodeData = _nodeList[node.id];
        console.log(nodeData);

        var endpoints = window.jsp.getEndpoints(node.id);
        if (endpoints && endpoints.length >= 1) {
            var connections = window.jsp.getConnectionsById(node.id);
            if (connections && endpoints.length > connections.length) {
                return;
            }
            nodeData.designerData.SourceAnchorCount += 1;
            _addEndpoints(node.id, nodeData.sourceAnchors, nodeData.targetAnchors, _canDrag, null, nodeData, nodeData.designerData.SourceAnchorCount - 1);

        }

    }
    /**
     * 分组函数
     * @param {any} array
     * @param {any} f
     */
    var _groupBy = function groupBy(array, f) {
        const groups = {};
        array.forEach(function (o) { //注意这里必须是forEach 大写
            const group = JSON.stringify(f(o));
            groups[group] = groups[group] || [];
            groups[group].push(o);
        });
        return Object.keys(groups).map(function (group) {
            return groups[group];
        });
    }

    /**
     * 创建工序组块
     * @param {any} selectNodes 选中的节点
     * @param {any} design 设计内容
     * @param {any} isload 是否重加载
     */
    var _createGroup = function createGroup(selectNodes, design, isload = false) {

        //校验
        if (selectNodes.length <= 1 && !isload) {
            SIE.Msg.showMessage("请至少选中两个以上工序！".t());
            return;
        }
        //校验

        for (var i = 0; i < selectNodes.length; i++) {
            if (selectNodes[i].designerData.NodeType === "RoutingGroupNode") {
                SIE.Msg.showMessage("工序组不允许再加入工序组！".t());
                return;
            }
            if (selectNodes[i].designerData.ProcessType == "Fix" && !isload) {
                SIE.Msg.showMessage("维修类型工序不允许加入工序组！".t());
                return;
            }
        }
        var groupProcessBlockNum = selectNodes.where(m => m.designerData.GroupId != undefined && m.designerData.GroupId != "");
        var processBlockNum = selectNodes.where(m => m.designerData.GroupId == undefined);
        if (!isload && groupProcessBlockNum.length >= 2 && processBlockNum.length >= 1) {
            SIE.Msg.showMessage("工序组内工序不允许重新组成工序组！".t());
            return;
        }


        var dragObj;
        var selectNodesData = [];
        var nodeInfo = DesignerNode.CreateNode("RoutingGroupNode");//创建组节点
        if (nodeInfo.element) {
            var innerHtml = "";
            var height = 0;
            for (var i = 0; i < selectNodes.length; i++) {

                if (selectNodes[i].designerData.ProcessType == "Fix" && !isload) {
                    SIE.Msg.showMessage("维修类型工序不允许加入工序组！".t());
                    return;
                }

                var selectNodeInfo = DesignerNode.CreateNode(selectNodes[i].nodeType);//创建组节点
                if (selectNodeInfo.element) {
                    if (typeof selectNodeInfo.element === 'string') {
                        if (!isload) {
                            me.removeNode(selectNodes[i].id);
                            delete _nodeList[selectNodes[i].id];//移除数据
                        }
                        selectNodeInfo.element = selectNodeInfo.element.replace("float:left; width:70px;height:40px;", "vertical-align: middle;margin-bottom:10px");
                        selectNodeInfo.element = selectNodeInfo.element.replace("float:left;width:70px; padding-left:5px; height:40px;", "float:left;width:70px;height:40px;");
                        selectNodeInfo.element = selectNodeInfo.element.replace("padding-left:5px; padding-top:5px;", "padding-left:5px; padding-top:5px; margin-bottom:10px;margin:5px");
                        selectNodeInfo.element = selectNodeInfo.element.replace('class="node"', 'class="node", oncontextmenu="customContextMenu(event);" id="' + selectNodes[i].id + '"');

                        var element = handleTemplate(selectNodeInfo.element, selectNodes[i]);
                        selectNodesData.push(selectNodes[i]);
                        innerHtml += element;
                        height += 55;
                    }
                }
            }

            var groupName = "";
            //非保存的重新加载需要重新生成工序组编号
            if (!isload) {
                groupIndex += 1;
                groupName = "工序组".t() + groupIndex;
            }
            if (design) {
                var designGroupData = design.nodes.find(m => selectNodes[0].designerData.GroupId == m.id);
                if (designGroupData) {
                    groupName = designGroupData.designerData.Text;
                    var exsitedIndex = parseInt(groupName.replace("工序组".t(), ""));
                    if (exsitedIndex > groupIndex) {
                        groupIndex = exsitedIndex;
                    }
                }
            }
            if (typeof nodeInfo.element === 'string') {
                var element = nodeInfo.tpl.replace("{ProcessHTML}", innerHtml).replace("{Text}", groupName);//替换模板
                dragObj = createElement(element);
            } else {
                dragObj = nodeInfo.element;
            }
            if (!dragObj) return;
            var newObj = dragObj.cloneNode(true);
            newObj.style.position = 'absolute';
            newObj.style.float = "left";

            if (!isload) {
                newObj.style.left = selectNodes[0].left + 'px';
                newObj.style.top = selectNodes[0].top + 'px';
            } else {
                newObj.style.left = selectNodes[0].left + 'px';
                newObj.style.top = selectNodes[0].top + 'px';
            }
            newObj.style.width = newObj.style.width || dragObj.offsetWidth + 'px';
            newObj.style.height = height + 'px';

            if (newObj.attributes["draggable"]) {
                newObj.attributes.removeNamedItem('draggable');
            }

            if (newObj.attributes["ondragstart"]) {
                newObj.attributes.removeNamedItem('ondragstart');
            }
            var node = _createNode(newObj.outerHTML, nodeInfo, (isload ? selectNodes[0].designerData.GroupId : null));

            drawBase.apply(node, nodeInfo);
            var nodeList = [];
            nodeList.push(node);
            node.designerData.NodeType = "RoutingGroupNode";
            if (!isload) {
                node.designerData.Text = groupName;
            }
            if (design) {
                var designGroupData = design.nodes.find(m => selectNodes[0].designerData.GroupId == m.id);
                if (designGroupData) {
                    node.designerData.GroupId = designGroupData.designerData.GroupId;
                    node.designerData.IsGroup = designGroupData.designerData.IsGroup;
                    node.designerData.ProcessState = designGroupData.designerData.ProcessState;
                    
                    node.designerData.NodeCount = designGroupData.designerData.NodeCount;
                    node.designerData.ProcessParameter = designGroupData.designerData.ProcessParameter;
                    node.designerData.Text = designGroupData.designerData.Text;

                    node.designerData.Type = designGroupData.designerData.Type;
                    node.designerData.Index = designGroupData.designerData.Index;

                }
            }

            node.groupDesignerData = selectNodesData;
            _drawNode(design, nodeList);
        }
    };

    var _setOffset = function setOffset(x, y) {
        this.offsetX = x;
        this.offsetY = y;
    };



    var handleTemplate = function handleTemplate(element, nodeInfo) {
        if (!nodeInfo || !element || typeof element != 'string') return;
        var reg = /{\S[^}]+}/g;

        if (reg.test(element)) {
            var matches = element.match(reg);
            if (!matches) return;

            for (var i = 0; i < matches.length; i++) {
                var match = matches[i];
                var bindField = match.substr(1, match.length - 2);
                var bindValue = '';

                if (nodeInfo.designerData) {
                    bindValue = nodeInfo.designerData[bindField] || bindValue;
                }

                element = element.replace(match, bindValue);
            }
        }

        return element;
    };

    /**
     * 工序组内部元素改变后变更线条起点
     * @param {any} node
     */
    var _changeGroupNodeLine = function _changeGroupNodeLine(node) {
        var instance = _getCurInstance();
        //先铲除原先的点
        window.jsp.deleteConnectionsForElement(node.id);//删除divID所有连接线
        window.jsp.removeAllEndpoints(node.id);//删除divID所有端点

        var nodeList = [];
        nodeList.push(node);

        if (_beforeDrawNode) _beforeDrawNode(nodeList, null); //节点锚点位置右上下左 x,y,dx,dy

        var initialAnchors = ["Perimeter", {
            shape: "Square",
            anchorCount: 150
        }],
            //结束端点位置
            completionAnchors = ["TopCenter"],
            //节点来源端点1
            nodeSourceAnchors_1 = [[1, 0.2, 1, 0], [0.5, 1, 0, 1], [0, 0.2, -1, 0]],
            //节点来源端点2
            nodeSourceAnchors_2 = [[1, 0.8, 1, 0], [0.6, 1, 0, 1], [0, 0.2, -1, 0]],
            //节点接触端点
            nodeTargetAnchors = [[0.5, 0, 0, -1], [1, 0.5, 1, 0], [0.5, 1, 0, 1], [0, 0.5, -1, 0]];
        if (!node.sourceAnchors) {

            var anchorArray = [];
            var offset = 0.2;
            if (node.designerData.SourceAnchor) {
                for (var j = 0; j < node.designerData.SourceAnchor.length; j++) {
                    var topPoint = [offset, 0, 0, -1];
                    var topPoint2 = [offset + 0.5, 0, 0, -1];
                    var rightPoint = [1, offset, 1, 0];
                    var rightPoint2 = [1, offset + 0.5, 1, 0];
                    var downPoint = [offset, 1, 0, 1];
                    var downPoint2 = [offset + 0.5, 1, 0, 1];
                    var leftPoint = [0, offset, -1, 0];
                    var leftPoint2 = [0, offset + 0.5, -1, 0];
                    offset = offset + 0.1;
                    anchorArray.push([topPoint, rightPoint, downPoint, leftPoint, topPoint2, rightPoint2, downPoint2, leftPoint2]);

                }
            }
            node.sourceAnchors = anchorArray;
            node.targetAnchors = [nodeTargetAnchors];
        }

        _addEndpoints(node.id, node.sourceAnchors, node.targetAnchors, _canDrag, null, node);

        if (_canDrag) {
            instance.draggable(node.id);
        }
    };


    //画节点
    var _drawNode = function drawNode(design, nodeList) {
        var instance = _getCurInstance();

        if (!(nodeList && Array.isArray(nodeList))) return; //画节点前处理函数

        if (_beforeDrawNode) _beforeDrawNode(nodeList, design); //节点锚点位置右上下左 x,y,dx,dy

        var initialAnchors = ["Perimeter", {
            shape: "Square",
            anchorCount: 150
        }],
            // var initialAnchors = [[0.5, 0, 0, -1], [1, 0.5, 1, 0], [0.5, 1, 0, 1], [0, 0.5, -1, 0]];

            //结束端点位置
            completionAnchors = ["TopCenter"],
            //节点来源端点1
            nodeSourceAnchors_1 = [[1, 0.2, 1, 0], [0.5, 1, 0, 1], [0, 0.2, -1, 0]],
            //节点来源端点2
            nodeSourceAnchors_2 = [[1, 0.8, 1, 0], [0.6, 1, 0, 1], [0, 0.2, -1, 0]],
            //节点接触端点
            nodeTargetAnchors = [[10, 0, 0, -20], [1, 0.5, 1, 0], [0.5, 1, 0, 1], [0, 0.5, -1, 0]]  //[[0.5, 0, 0, -1], [1, 0.5, 1, 0], [0.5, 1, 0, 1], [0, 0.5, -1, 0]];

        for (var i = nodeList.length - 1; i >= 0; i--) {
            var node = nodeList[i];
            addNodeToList(node);

            if (!node.sourceAnchors) {
                if (node.designerData.Type == 'Initial') {
                    node.sourceAnchors = [initialAnchors];
                    node.targetAnchors = [];
                } else if (node.designerData.Type == 'Completion') {
                    node.sourceAnchors = [];
                    node.targetAnchors = [completionAnchors];
                } else {
                    var anchorArray = [];
                    var offset = 0.2;
                    if (node.designerData.SourceAnchor) {
                        for (var j = 0; j < node.designerData.SourceAnchor.length; j++) {
                            var topPoint = [offset, 0, 0, -1];
                            var topPoint2 = [offset + 0.5, 0, 0, -1];
                            var rightPoint = [1, offset, 1, 0];
                            var rightPoint2 = [1, offset + 0.5, 1, 0];
                            var downPoint = [offset, 1, 0, 1];
                            var downPoint2 = [offset + 0.5, 1, 0, 1];
                            var leftPoint = [0, offset, -1, 0];
                            var leftPoint2 = [0, offset + 0.5, -1, 0];
                            offset = offset + 0.1;
                            anchorArray.push([topPoint, rightPoint, downPoint, leftPoint, topPoint2, rightPoint2, downPoint2, leftPoint2]);

                            if (design) {
                                //如果修改了线标签，则更新为修改后的
                                var sourceAnchor = node.designerData.SourceAnchor[j];
                                var curline = design.lines.first(function (p) { return p.linedata.ParameterId == sourceAnchor.paramId; });
                                if (curline && curline.linedata && sourceAnchor.LineLabel !== curline.linedata.Text) {
                                    node.designerData.SourceAnchor[j].LineLabel = curline.linedata.Text;
                                }
                            }
                        }
                    }
                    node.sourceAnchors = anchorArray;
                    node.targetAnchors = [nodeTargetAnchors];
                }
            }
            if (node.designerData.Type == 'Initial') {
                if (!node.designerData.SourceAnchorCount) {//支持旧数据
                    node.designerData.SourceAnchorCount = 1;
                }
                for (var k = 0; k < node.designerData.SourceAnchorCount; k++) {
                    _addEndpoints(node.id, node.sourceAnchors, node.targetAnchors, _canDrag, null, node, k);
                }
            } else {
                _addEndpoints(node.id, node.sourceAnchors, node.targetAnchors, _canDrag, null, node);
            }

            if (_canDrag) {
                instance.draggable(node.id);
            }

            if (node.designerData.NodeType !== me.getGroupNodeKey() && node.designerData.Type !== 'Initial' && node.designerData.Type !== 'Completion')//工序
            {
                var nodeDom = document.getElementById(node.id);
                if (nodeDom) {
                    var isRepeatImg = nodeDom.getElementsByTagName("img")[1];
                    if (isRepeatImg) {
                        isRepeatImg.style.display = (node.designerData.IsOptional == "true" || node.designerData.IsOptional == true) ? "block" : "none";
                    }
                    var isOptionalImg = nodeDom.getElementsByTagName("img")[2];
                    if (isOptionalImg) {
                        isOptionalImg.style.display = (node.designerData.IsRepeat == "true" || node.designerData.IsRepeat == true) ? "block" : "none";
                    }
                }
            }
        }
        //对可选项和可重复过站属性的图标显示隐藏
        var groupNode = nodeList[0];
        if (groupNode.designerData.Type !== 'Initial' && groupNode.designerData.Type !== 'Completion') {
            var nodeDom = document.getElementById(groupNode.id);
            if (groupNode.designerData.NodeType === me.getGroupNodeKey())//工序组
            {
                var childNodes = nodeDom.childNodes[1].childNodes;
                for (var i = 0; i < childNodes.length; i++) {
                    var childNodeDom = nodeDom.childNodes[1].childNodes[i];
                    var childNodeDomData = groupNode.groupDesignerData.find(m => m.id == childNodeDom.id);
                    if (childNodeDom && childNodeDomData) {

                        var isRepeatImg = childNodeDom.getElementsByTagName("img")[1];
                        if (isRepeatImg) {
                            isRepeatImg.style.display = (childNodeDomData.designerData.IsOptional == true || childNodeDomData.designerData.IsOptional == "true") ? "block" : "none";
                        }
                        var isOptionalImg = childNodeDom.getElementsByTagName("img")[2];
                        if (isOptionalImg) {
                            isOptionalImg.style.display = (childNodeDomData.designerData.IsRepeat == true || childNodeDomData.designerData.IsRepeat == "true") ? "block" : "none";
                        }
                    }
                }
            }
        }
    };

    //画线
    var drawLine2 = function drawLine2(nodeList) {
        var instance = _getCurInstance();

        for (var i = 0; i < nodeList.length; i++) {
            var node = nodeList[i];
            var sourceAnchors = node.sourceAnchors;
            if (!(sourceAnchors && Array.isArray(sourceAnchors))) continue;

            for (var j = 0; j < sourceAnchors.length; j++) {
                var sourceAnchor = sourceAnchors[j];
                var lineToList = sourceAnchor.lineToList;
                if (!(lineToList && Array.isArray(lineToList))) continue;

                for (var k = 0; k < lineToList.length; k++) {
                    var lineTo = lineToList[k];
                    instance.connect({
                        uuids: [node.id + sourceAnchor.position, lineTo.targetNode + lineTo.targetPosition],
                        editable: true
                    });
                }
            }
        }
    };
    /**
    * 画线
    * lineList:线条列表
    * line 线条信息
    * {
    *   id:''
    *   sourceNode:'' 源节点id
    *   sourceAnchor:'' 源锚
    *   targetNode:'' 目标节点id
    *   targetAnchor:'' 目标锚
    * }
    **/
    var _drawLine = function drawLine(lineList) {
        var instance = _getCurInstance();

        for (var i = 0; i < lineList.length; i++) {
            var line = lineList[i];
            //line.sourceNode = instance.getReverseMappingId(line.sourceNode);
            //line.targetNode = instance.getReverseMappingId(line.targetNode);
            //line.linedata.SourceActivityId = instance.getReverseMappingId(line.linedata.SourceActivityId);
            var lineText = line.linedata.Text;
            var connector = instance.connect({
                uuids: [line.sourceNode + line.sourceAnchor],
                target: line.targetNode,
                editable: true,
                linelabel: lineText,
                overlays: [
                    ["Label", { label: lineText, id: "label" }]
                ]
            });

            if (connector && connector.canvas) {
                if (!connector.canvas.dataset) connector.canvas.dataset = {};
                connector.linedata = JSON.stringify(line.linedata);
                //listenEvent(connector.canvas, 'lineClick', me.listeners.lineClick);
                //listenEvent(connector.canvas, 'lineDbClick', me.listeners.lineDbClick(connector));
            }
        }
    };

    //生成Id
    var generateId = function generateId() {
        // return _autoNodePre + ++_maxAutoNodeId;
        function guid() {
            function S4() {
                return ((1 + Math.random()) * 0x10000 | 0).toString(16).substring(1);
            }

            return S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4();
        }

        return guid();
    };

    //获取节点列表
    var getNodes = function getNodes() {
        var elements = drawBase.getByClass(_getCurInstance().getContainer(), _nodeClass);
        var nodes = new Array();

        for (var i = elements.length - 1; i >= 0; i--) {
            var element = elements[i];
            var node = _nodeList[element.id];
            node.element = element;
            getNodeInfo(node, element);
            nodes.push(node);
        }

        return nodes; // var nodes = new Array();
        // if(_nodeList){
        //     var propertys = Object.getOwnPropertyNames(_nodeList);
        //     for (let i = 0; i < propertys.length; i++) {
        //         const property = propertys[i];
        //         var node = _nodeList[property];
        //         getNodeInfo(node, node.element);
        //         nodes.push(node);
        //     }
        // }
        // return nodes;
    };

    var _getNode = function getNode(id) {
        if (!_nodeList) return null;
        return _nodeList[id];
    };

    //设置nodeData
    var _setNodeList = function setNodeList(id, nodeData) {
        if (!_nodeList) return null;
        return _nodeList[id] = nodeData;
    };

    //获取节点信息
    var getNodeInfo = function getNodeInfo(node, element) {
        if (typeof element === 'string') {
            element = createElement(element);
            node.element = element;
        }

        node.id = element.id;
        node.width = element.style.width;
        node.height = element.style.height;
        node.left = element.offsetLeft;
        node.top = element.offsetTop;
    };

    //获取连线信息
    var getLines = function getLines(nodes) {
        var lines = [];

        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i];
            if (!node.element) continue;

            var endpoints = _getCurInstance().getEndpoints(node.element);

            if (endpoints && Array.isArray(endpoints)) {
                for (var j = 0; j < endpoints.length; j++) {
                    var endpoint = endpoints[j];
                    if (!endpoint.isSource || endpoint.connections.length <= 0) continue;

                    for (var k = 0; k < endpoint.connections.length; k++) {
                        var connection = endpoint.connections[k];
                        var line = getLine(connection);
                        if (line) lines.push(line);
                    }
                }
            }
        }

        return lines;
    };

    //获取单个连线信息
    var getLine = function getLine(connection) {
        var sourceEndpoint, targetEndpoint, sourceEndpointParameter;

        for (var i = 0; i < connection.endpoints.length; i++) {
            var conPoint = connection.endpoints[i];

            if (conPoint.isTarget) {
                targetEndpoint = conPoint;
            } else if (conPoint.isSource) {
                sourceEndpoint = conPoint;
            }
        }

        if (!sourceEndpoint || !targetEndpoint) return;
        var line = {
            id: connection.id,
            sourceNode: connection.sourceId,
            sourceAnchor: sourceEndpoint.anchor.type,
            targetNode: connection.targetId,
            targetAnchor: targetEndpoint.anchor.type
        };

        if (sourceEndpoint.canvas.dataset.paramter) {
            sourceEndpointParameter = JSON.parse(sourceEndpoint.canvas.dataset.parameter);
        }

        line.linedata = JSON.parse(connection.linedata || '{}');
        return line;
    };

    //生成json
    var generalJson = function generalJson() {
        var json = {};
        json.flowdata = getFlowData();
        json.nodes = getNodes();
        json.lines = getLines(json.nodes);
        return json;
    };

    var getFlowData = function getFlowData() {
        var flowdata = _getCurInstance().getContainer().dataset.flowdata;

        if (flowdata) {
            return JSON.parse(flowdata);
        }

        return null;
    };

    //加载节点
    var loadNodes = function loadNodes(design) {
        var nodes = design.nodes; //加载节点

        if (!nodes || !Array.isArray(nodes)) return;
        clearNodeList();

        var groupNodes = [];
        var singleNodes = [];
        nodes.forEach(function (node, index) {
            if (node.designerData.IsGroup == true) {
            } else {
                if (node.element && (!node.designerData.GroupId || node.designerData.GroupId === "")) {
                    if (typeof node.element == 'string') {
                        appendTo(node.element, node);
                    } else {
                        appendTo(node.element.outerHTML, node);
                    }
                    singleNodes.push(node);
                }
                if (node.designerData.GroupId !== "" && node.designerData.GroupId) {//工序组的区分开绘制
                    groupNodes.push(node);
                }
            }
        });
        //区分工序组和工序加载
        if (singleNodes.length > 0) {
            _drawNode(design, singleNodes);
        }
        if (groupNodes.length > 0) {
            _loadGroups(design, groupNodes);
        }
    };

    //事件处理器
    var handlers = {
        connection: function connection(_connection) {// connection.getOverlay("label").setLabel(connection.sourceId.substring(15) + "-" + connection.targetId.substring(15));
        },
        beforeDrop: function beforeDrop(connInfo) {
            return true;
        }
    };

    //拖拽
    var allowDrop = function allowDrop(ev) {
        ev.preventDefault();
    };

    //活动节点拖入位置
    var drag = function drag(ev, node) {
        //if (ev.target.id) {
        //    ev.dataTransfer.setData("targetId", ev.target.id);
        //}

        var tData = {
            targetId: ev.target.id,
            content: ev.target.outerHTML,
            offsetX: ev.offsetX,
            offsetY: ev.offsetY
        };

        //ev.dataTransfer.setData("text/plain", ev.target.outerHTML);
        //ev.dataTransfer.setData("offsetX", ev.offsetX);
        ev.dataTransfer.setData("Text", JSON.stringify(tData));
    };


    //活动节点放入目标画布处理
    var drop = function drop(ev) {
        ev.preventDefault();
        if (isLock) return;
        var tData = JSON.parse(ev.dataTransfer.getData("Text"));
        var targetId = tData.targetId;
        var dragObj;
        var nodeInfo;

        if (targetId) {
            dragObj = document.getElementById(targetId);
        } else {
            var html = tData.content;
            dragObj = createElement(html);

            if (dragObj.dataset && dragObj.dataset.nodetype) {
                nodeInfo = DesignerNode.CreateNode(dragObj.dataset.nodetype);

                if (dragObj.dataset.nodedata) {
                    var nodedata = JSON.parse(dragObj.dataset.nodedata);
                    drawBase.apply(nodeInfo.designerData, nodedata);
                }

                if (nodeInfo.element) {
                    if (typeof nodeInfo.element === 'string') {
                        var element = handleTemplate(nodeInfo.element, nodeInfo);
                        dragObj = createElement(element);
                    } else {
                        dragObj = nodeInfo.element;
                    }
                }
            }
        }

        if (!dragObj) return;
        var container = ev.target;

        if (dragObj.parentElement != container) {
            var offsetX = tData.offsetX;
            var offsetY = tData.offsetY;
            var newObj = dragObj.cloneNode(true);
            newObj.style.position = 'absolute';
            newObj.style.float = "left";
            newObj.style.left = ev.offsetX - offsetX + 'px';
            newObj.style.top = ev.offsetY - offsetY + 'px';
            newObj.style.width = newObj.style.width || dragObj.offsetWidth + 'px';
            newObj.style.height = newObj.style.height || dragObj.offsetHeight + 'px';

            if (newObj.attributes["draggable"]) {
                newObj.attributes.removeNamedItem('draggable');
            }

            if (newObj.attributes["ondragstart"]) {
                newObj.attributes.removeNamedItem('ondragstart');
            }

            var node = _createNode(newObj.outerHTML, nodeInfo);

            drawBase.apply(node, nodeInfo);
            var nodeList = [];
            nodeList.push(node);

            _drawNode(null, nodeList);
            var nodeDom = document.getElementById(node.id);
            if (nodeDom) {
                addToGroupDrogEnd(ev, nodeDom);
            }
        }
    };
    var _getLock = function getLock()
    {
        return isLock;
    };
    var _addNode = function addNode(html, nodeData) {
        var dragObj;
        var nodeInfo;

        if (!nodeData) {
            dragObj = createElement(html.outerHTML);
        } else {
            dragObj = createElement(nodeData.element);
        }

        if (dragObj.dataset && dragObj.dataset.nodetype) {
            nodeInfo = DesignerNode.CreateNode(dragObj.dataset.nodetype);

            if (dragObj.dataset.nodedata) {
                var nodedata = JSON.parse(dragObj.dataset.nodedata);
                drawBase.apply(nodeInfo.designerData, nodedata);
            }

            if (nodeInfo.element) {
                if (typeof nodeInfo.element === 'string') {
                    var element = handleTemplate(nodeInfo.element, nodeInfo);
                    dragObj = createElement(element);
                } else {
                    dragObj = nodeInfo.element;
                }
            }
        }

        if (!dragObj) return;
        var newObj = dragObj.cloneNode(true);
        newObj.style.position = 'absolute';
        newObj.style.float = "left";
        newObj.style.left = '100px';
        newObj.style.top = '100px';
        newObj.style.width = newObj.style.width;
        newObj.style.height = newObj.style.height;

        if (newObj.attributes["draggable"]) {
            newObj.attributes.removeNamedItem('draggable');
        }

        if (newObj.attributes["ondragstart"]) {
            newObj.attributes.removeNamedItem('ondragstart');
        }

        var node = _createNode(newObj.outerHTML, nodeInfo);

        drawBase.apply(node, nodeInfo);
        var nodeList = [];
        nodeList.push(node);

        _drawNode(null, nodeList);
    };

    var returnObj = {

        getGroupNodeKey: function getGroupNodeKey() {
            return groupNodeKey;
        },

        getGroupNodeKey: function getGroupNodeKey() {
            return groupNodeKey;
        },

        getCurInstance: function getCurInstance() {
            return _getCurInstance();
        },
        setContainer: function setContainer(container) {
            _setContainer(container);
        },
        isReady: function isReady() {
            var instance = _getCurInstance();

            return instance ? true : false;
        },
        clear: function clear() {
            var instance = _getCurInstance(); // instance.empty();


            var nodes = getNodes();

            if (nodes) {
                nodes.forEach(function (node) {
                    instance.remove(node.element);
                });
            }

            instance.getContainer().innerHTML = '';
        },
        getNode: function getNode(element) {
            if (typeof element === 'HTMLElement') {
                element = element.id;
            }

            return _getNode(element);
        },
        removeNode: function removeNode(element) {
            if (typeof element === 'string') {
                element = document.getElementById(element);
            }

            _getCurInstance().remove(element);
        },
        createNode: function createNode(html, nodeInfo, id) {
            return _createNode(html, nodeInfo, id);
        },
        drawNode: function drawNode(nodeList) {
            _drawNode(null, nodeList);
        },
        drawLine: function drawLine(nodeList) {
            _drawLine(nodeList);
        },
        getAllNodes: function getAllNodes() {
            return getNodes();
        },
        getDesignJson: function getDesignJson() {
            return generalJson();
        },
        loadDesign: function loadDesign(design) {
            if (design) {
                if (design.flowdata) {
                    _getCurInstance().getContainer().dataset.flowdata = JSON.stringify(design.flowdata);
                }

                loadNodes(design);

                _drawLine(design.lines);
            }
        },
        draggable: function draggable(node, isDrag) {
            if (!node) return;
            if (typeof node === "string") node = document.getElementById(node);
            if (isDrag) {
                node.setAttribute('draggable', true); // node.setAttribute('ondragstart', 'drag(event)');

                node.addEventListener('dragstart', drag);
            } else {
                // node.setAttribute('ondragover', 'allowDrop(event)');
                node.addEventListener('dragover', allowDrop); // node.setAttribute('ondrop', 'drop(event)');

                node.addEventListener('drop', drop);
            }
        },
        setLock: function setLock(islock) {
            isLock = islock;
        },
        getLock:function getLock(){
            return _getLock();
        },
        addNode: function addNode(html, nodedata) {
            _addNode(html, nodedata);
        },
        toGroup: function toGroup() {
            _toGroup();
        },
        setOffset: function setOffset(x, y) {
            _setOffset(x, y);

        },
        customContextMenu: function customContextMenu(event) {
            _customContextMenu(event);
        },
        dwhandleTemplate: function dwhandleTemplate(element, nodeInfo) {
            return handleTemplate(element, nodeInfo);
        },
        setNodeList: function setNodeList(id, data) {
            _setNodeList(id, data);
        },
        changeGroupNodeLine: function changeGroupNodeLine(node) {
            _changeGroupNodeLine(node);

        },
        //根据Id删除节点数据
        deleteNodeListById: function _deleteNodeListById(id) {
            delete _nodeList[id];
        },
        addSingleNode: function addSingleNode(designdataset) {
            return _addSingleNode(designdataset);
        },
        deleteNode: function deleteNode(currentBlock) {
            _deleteNode(currentBlock);
        },
        resetGroupIndex: function () {
            groupIndex = 0;
        },
        addNewBranch: function addNewBranch(node) {
            return _addNewBranch(node);
        },
        zoomOut: function zoomOut() {
            return _zoomout();
        },
        zoomIn: function zoomIn() {
            return _zoomin();
        },
        resetZoom: function resetZoom() {
            return _resetZoom();
        },
        listeners: {}
    };
    init(returnObj, container, initConfig);
    return returnObj;
};