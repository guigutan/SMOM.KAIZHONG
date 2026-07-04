function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

/**
 * 工艺路线设计画布
 * @class DesignCanvas
 * @constructor
 */
Ext.define('DesignCanvas', {
    mixins: ['Ext.mixin.Observable'],

    /**
     * 父主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 画布div id，所有设计功能需要单独指定，且不能重复
     * @property {string} canvas
     */
    canvas: null,

    //工序组键
    groupNodeKey: "RoutingGroupNode",

    /**
     * 画图工具
     * @property {画图工具} drawControl
     */
    drawControl: null,

    /**
     * 画布配置
     * @property {string} config
     */
    config: null,

    /**
     * 工序索引
     * @property {number} indexStep
     */
    indexStep: 10,

    /**
     * 规则线样式
     * @property {字典} defaultSourceEndpointStyle
     */
    defaultSourceEndpointStyle: {},

    /**
     * 工序节点背景色字典，key工序状态value颜色
     * @property {字典} dicNodeColor
     */
    dicNodeColor: {},

    /**
     * 选中节点
     * @property {object} selectNode
     */
    selectNode: null,
    /**
     * 选中节点数组,支持多选
     * @property [object] selectNode
     */
    selectNodes: [],

    /*
     *是否按住Ctrl键
     */
    isCtrl: false,

    /**
     *是否按下Shift键 缩放是需要
     * */
    isShift: false,

    /**
     * 工序节点背景颜色字典
     * @property {字典} dicProcessImgBackground
     */
    dicProcessImgBackground: {},

    /**
     * 连接线虚线样式
     * @property {字典} dashstyle
     */
    dashstyle: "1 3",

    mouseEvent: null,

    /*
     *布局命令 上下居中 左右居中 等
     */
    routingDesignControrl: null,
    /**
     * 事件 
     */
    listeners: [{
        /**
         * 选中节点变更事件，外层必须订阅该事件处理节点变更逻辑
         * @method nodeChanged
         * @for DesignCanvas
         * @param {ListLogicalView} mainView 父主视图
         * @param {object} node 节点信息
         */
        nodeChanged: function (mainView, node) { },

    }],


    /**
     * 构造函数
     * @method constructor
     * @for DesignCanvas
     * @param {ListLogicalView} mainView 父主视图
     * @param {string} canvas 画布div id
     * @param {string} config 画布配置
     */
    constructor: function constructor(mainView, canvas, config) {
        var me = this;
        me.callParent(arguments);

        if (canvas === null || canvas === undefined) {
            throw '画布id不能为空'.t();
        }

        if (mainView === null || mainView === undefined) {
            throw '主视图不能为空'.t();
        }

        me.canvas = canvas;
        me.mainView = mainView;
        me.config = config;
        me.initSourceEndpoint();
        me.initNode();
        me.mixins.observable.constructor.call(me, arguments);
        me.mainView.mon(me.mainView, 'beforeclosewin', me.beforeclosewin, me);
    },

    /**
     * 主视图关闭事件
     * @method beforeclosewin
     * @for DesignCanvas
     */
    beforeclosewin: function () {
        var me = this;
        me.mainView.mun(me.mainView, 'beforeclosewin', me.beforeclosewin, me);
    },

    /**
     * 初始化画图工具
     * @method InitDrawViewControl
     * @for DesignCanvas
     */
    InitDrawViewControl: function () {
        var me = this;
        me.initProcessImgBackground(); //初始化图形设计器


        document.onkeydown = function (event) {
            event = event || window.event
            if (event.keyCode === 17) {
                me.isCtrl = true;
            }
            if (event.keyCode === 16) {
                me.isShift = true;
            }
            //if (event.keyCode === 46 && me.selectNodes != null && me.selectNodes.length > 0) {
            //    if (me.drawControl.drawTools.getLock() == false) {//未发布的可以删除,工序组不支持
            //        me.selectNodes.forEach(m => {
            //            if (m.designerData.GroupId == undefined) {
            //                me.drawControl.drawTools.removeNode(m.id);
            //                me.drawControl.drawTools.deleteNodeListById(m.id);
            //            } else {
            //                var sunNode = document.getElementById(m.id);
            //                me.drawControl.drawTools.deleteNode(sunNode);
            //            }
            //        })
            //    }

            //}

        };
        document.onkeyup = function (event) {
            event = event || window.event
            if (event.keyCode === 17) {
                me.isCtrl = false;
            }
            if (event.keyCode === 16) {
                me.isShift = false;
            }

        };

        var propertyHandler = function (evt) {
            var element = evt.srcElement;
            var drawTools = Ext.getCmp(me.mainView.routingDrawControlId).drawTools;
            var node = drawTools.getNode(element.id);
            me.selectNode = node;

            me.fireEvent('nodeChanged', me.mainView, node);
        };


        me.drawControl = Ext.create('SIE.control.DrawViewControl', {
            isXmlData: true,
            canvas: me.canvas,
            nodeTag: '.x-tree-icon-leaf~.x-tree-node-text',
            designConfig: {
                canDrag: true,

                /**
                 * 节点绘制之前
                 * @method beforeDrawNode
                 * @for SIE.control.DrawViewControl
                 * @param {Array} nodeList 节点集合
                 * @param {Object} design 设计数据
                 */
                beforeDrawNode: function beforeDrawNode(nodeList, design) {
                    //绘制活动节点前处理函数
                    if (!(nodeList && Array.isArray(nodeList))) return;
                    if (design) {
                        //加载旧工序节点：1全部线已经连接的工序根据线添加锚点；2部分线未连接，获取工序参数重新设置锚点
                        var instance = me.drawControl.drawTools.getCurInstance();
                        var lineList = design.lines;
                        nodeList.forEach(function (node) {
                            if (!node.designerData.NewProcess || node.designerData.NewProcess === false) {
                                //旧工序节点，根据规则数设置锚点数
                                var lines = lineList.where(function (p) {
                                    return p.sourceNode === node.id;// instance.getReverseMappingId(p.sourceNode) === node.id;
                                });
                                if (node.designerData.NodeCount === lines.length && lines.length > 0) {  //节点数跟已连接线数量一致，根据线添加锚点
                                    var params = [];
                                    lines.forEach(function (line) {
                                        var model = SIE.getModel('SIE.Tech.Processs.ProcessParameter');
                                        var param = new model();
                                        param.setId(line.linedata.ParameterId);
                                        param.setType(me.getParameterType(line.linedata.ParamResultType));
                                        param.setDescription(line.linedata.Text);
                                        params.push(param);
                                    });
                                    me.setAnchor(node, params.select(function (p) {
                                        return p.data;
                                    }));

                                    node.designerData.ProcessParameter = params.select(function (p) {
                                        return p.data;
                                    });
                                }
                                else {
                                    if (!node.groupDesignerData || node.groupDesignerData.length <= 0) {
                                        me.getProcessInfo([node.designerData.ProcessId], function (params) {
                                            me.setAnchor(node, params);
                                            node.designerData.ProcessParameter = params;
                                        });
                                    }
                                    ///工序组
                                    if (node.designerData.NodeType == me.groupNodeKey && node.groupDesignerData && node.groupDesignerData.length > 0) {
                                        var processParameters = [];
                                        Ext.Array.forEach(node.groupDesignerData, function (nodeDesignerData) {
                                            if (nodeDesignerData.designerData.ProcessParameter && nodeDesignerData.designerData.ProcessParameter.length > 0) {
                                                nodeDesignerData.designerData.ProcessParameter.forEach(function (processParameter) {
                                                    var isExsited = processParameters.find(item => item.Type == processParameter.Type);
                                                    if (!isExsited) {
                                                        processParameters.push(processParameter);
                                                    }
                                                });
                                            }
                                        });
                                        var isExsitedAny = processParameters.findIndex(m => m.Type == 3) >= 0;//存在任意、通过
                                        var isExsitedPass = processParameters.findIndex(m => m.Type == 1) >= 0;//存在通过
                                        /*var isExsitedFail = processParameters.findIndex(m => m.Type == 2) >= 0;//存在失败*/
                                        if (isExsitedAny && isExsitedPass)//任意或者通过 、通过、失败
                                        {
                                            for (var i = 0; i < processParameters.length; i++) {
                                                if (processParameters[i].Type == 3) {
                                                    processParameters.splice(i, 1);
                                                }
                                            }
                                        }
                                        node.ProcessParameter = processParameters;
                                        node.designerData.NodeCount = processParameters.length;
                                        me.setAnchor(node, processParameters);

                                    }
                                }
                            }
                        });
                    }
                    else {
                        //新增工序，获取工序参数列表设置锚点数
                        var processIds = [];
                        Ext.Array.forEach(nodeList, function (node) {
                            if (node.designerData && node.designerData.ProcessId) {
                                processIds.push(node.designerData.ProcessId);
                            }
                        });
                        if (processIds.length > 0) {
                            me.getProcessInfo(processIds, function (processParameter) {
                                Ext.Array.forEach(nodeList, function (node) {
                                    if (node.designerData) {
                                        var params = Ext.Array.filter(processParameter, function (item) {
                                            if (item.ProcessId === node.designerData.ProcessId) //不能===两个类型不一致
                                                return true;
                                            else
                                                return false;
                                        });
                                        me.setAnchor(node, params);
                                    }
                                });
                            });
                        }
                        if (processIds.length == 0 && nodeList[0].designerData.NodeType == me.groupNodeKey)//工序组数据无需从后台重新获取
                        {
                            var groupData = me.drawControl.drawTools.getNode(nodeList[0].id);
                            if (groupData && groupData.groupDesignerData) {
                                nodeList[0].sourceAnchors = null;
                                var processParameters = [];
                                Ext.Array.forEach(groupData.groupDesignerData, function (nodeDesignerData) {
                                    if (nodeDesignerData.designerData.ProcessParameter && nodeDesignerData.designerData.ProcessParameter.length > 0) {
                                        nodeDesignerData.designerData.ProcessParameter.forEach(function (processParameter) {
                                            var isExsited = processParameters.find(item => item.Type == processParameter.Type);
                                            if (!isExsited) {
                                                processParameters.push(processParameter);
                                            }
                                        });
                                    }
                                });
                                var isExsitedAny = processParameters.findIndex(m => m.Type == 3) >= 0;//存在任意、通过
                                var isExsitedPass = processParameters.findIndex(m => m.Type == 1) >= 0;//存在通过
                                var isExsitedFail = processParameters.findIndex(m => m.Type == 2) >= 0;//存在失败
                                if (isExsitedAny && isExsitedPass)//任意或者通过 、通过、失败
                                {
                                    for (var i = 0; i < processParameters.length; i++) {
                                        if (processParameters[i].Type == 3) {
                                            processParameters.splice(i, 1);
                                        }
                                    }
                                }
                                nodeList[0].ProcessParameter = processParameters;
                                nodeList[0].designerData.NodeCount = processParameters.length;
                                me.setAnchor(nodeList[0], processParameters);
                                //nodeList[0].designerData.ProcessParameter = processParameters;
                            }
                        }

                    }
                },

                /**
                 * 规则连接
                 * @method connection
                 * @for SIE.control.DrawViewControl
                 * @param {Object} connInfo 规则信息
                 */
                connection: function connection(connInfo) {
                    //连线时添加WPF需要的属性
                    var conn = connInfo.connection;
                    var source = connInfo.source;
                    var sourceEndpoint = connInfo.sourceEndpoint;
                    var targetEndpoint = connInfo.targetEndpoint;
                    var sourceActivityId, state, isSelected, text, zIndex, parameterId, paramResultType, expression, startPoint, point1, point2, endPoint, type, beginLeft, beginTop, endLeft, endTop, textLeft, textTop, left1, left2, top1, top2, color;
                    sourceActivityId = conn.sourceId;
                    state = 'New';
                    isSelected = false;
                    zIndex = 1;

                    if (source.dataset.nodetype === 'BeginNode') {
                        parameterId = 0;
                        paramResultType = 0;
                        expression = '';
                    } else {
                        var nodeParameter = JSON.parse(sourceEndpoint.canvas.dataset.parameter);

                        switch (nodeParameter.Type) {
                            case 1:
                                paramResultType = 'Pass';
                                break;

                            case 2:
                                paramResultType = 'Fail';
                                break;

                            case 3:
                                paramResultType = 'Any';
                                break;

                            case 4:
                                paramResultType = 'Custom';
                                break;
                            case 8:
                                paramResultType = 'Optional';
                                break;
                            default:
                                paramResultType = '0';
                        }

                        parameterId = nodeParameter.Id;
                        expression = nodeParameter.Script;
                    }

                    switch (conn.connector.type) {
                        case 'StateMachine':
                        case 'Straight':
                            type = 'Line';
                            break;

                        case 'Flowchart':
                        case 'Bezier':
                            type = 'Curve';
                            break;
                    }

                    startPoint = sourceEndpoint.canvas.offsetLeft + ',' + sourceEndpoint.canvas.offsetTop;
                    point1 = startPoint;
                    point2 = startPoint;
                    endPoint = targetEndpoint.canvas.offsetLeft + ',' + targetEndpoint.canvas.offsetTop;
                    beginLeft = sourceEndpoint.canvas.offsetLeft;
                    beginTop = sourceEndpoint.canvas.offsetTop;
                    endLeft = targetEndpoint.canvas.offsetLeft;
                    endTop = targetEndpoint.canvas.offsetTop;
                    left1 = beginLeft;
                    left2 = beginLeft;
                    top1 = beginTop;
                    top2 = beginTop;
                    var labelOverlay = conn.getOverlay('label');

                    if (labelOverlay) {
                        text = sourceEndpoint.canvas.dataset.LineLabel;
                        textLeft = labelOverlay.canvas.offsetLeft;
                        textTop = labelOverlay.canvas.offsetTop;
                    } else {
                        text = '';
                        textLeft = 0;
                        textTop = 0;
                    }

                    if (sourceEndpoint.connectorStyle) color = sourceEndpoint.connectorStyle.stroke; else color = '#3B444D';
                    var linedata = {
                        SourceActivityId: conn.sourceId,
                        State: state,
                        IsSelected: isSelected,
                        Text: text,
                        ZIndex: zIndex,
                        ParameterId: parameterId,
                        ParamResultType: paramResultType,
                        Expression: expression,
                        StartPoint: startPoint,
                        Point1: point1,
                        Point2: point2,
                        EndPoint: endPoint,
                        Type: type,
                        BeginLeft: beginLeft,
                        BeginTop: beginTop,
                        EndLeft: endLeft,
                        EndTop: endTop,
                        TextLeft: textLeft,
                        TextTop: textTop,
                        Left1: left1,
                        Left2: left2,
                        Top1: top1,
                        Top2: top2,
                        Color: color
                    };
                    conn.linedata = JSON.stringify(linedata);
                    conn.canvas.setAttribute('data-linedata', conn.linedata);

                    me.updateNodeParent(connInfo);
                },

                /**
                 * 规则连接前处理
                 * @method connInfo
                 * @for SIE.control.DrawViewControl
                 * @param {Object} connInfo 规则信息
                 */
                beforeDrop: function beforeDrop(connInfo) {
                    var sourceEndpoint = connInfo.connection.endpoints.first(function (p) { return p.elementId === connInfo.sourceId; });
                    if (sourceEndpoint && sourceEndpoint.canvas.dataset.AnchorType !== 'Optional') {
                        var drawTools = Ext.getCmp(me.mainView.routingDrawControlId).drawTools;
                        var sourceNode = drawTools.getNode(connInfo.sourceId);
                        var targetNode = drawTools.getNode(connInfo.targetId);
                        if (!sourceNode || !targetNode) {
                            return false;
                        }
                        if (sourceNode.nodeType === 'BeginNode' || targetNode.nodeType === 'EndNode')
                            return true;
                        if (sourceNode.ParentNodeId) {
                            if (targetNode.ParentNodeId) {
                                if (sourceNode.ParentNodeId !== targetNode.ParentNodeId) {
                                    return false;
                                }
                                //else if (me.getDesignData().lines.any(function (p) { return p.targetNode === targetNode.id })) {
                                //    return false;
                                //}
                            } else {
                                if (sourceNode.ParentNodeId !== targetNode.id && me.getDesignData().lines.any(function (p) { return p.sourceNode === targetNode.id })) {
                                    return false;
                                }
                            }
                        } else {
                            if (targetNode.ParentNodeId) {
                                return false;
                            } else {
                                return true;
                            }
                        }
                    }
                    return true;
                }
            },
            listeners: {
                /**
                 * 节点点击事件
                 * @method nodeClick
                 * @for SIE.control.DrawViewControl
                 */
                nodeClick: function nodeClick() {
                    propertyHandler(event);

                },

                /**
                 * 规则点击事件
                 * @method lineClick
                 * @for SIE.control.DrawViewControl
                 */
                lineClick: function lineClick() {
                    propertyHandler(event);
                },

                /**
                 * 规则双击事件
                 * @method lineDbClick
                 * @for SIE.control.DrawViewControl
                 * @param {Object} conninfo 规则信息
                 * @returns {Window} 脚本编辑窗口
                 */
                lineDbClick: function (conninfo) {
                    return function () {
                        me.showLineWindow(conninfo);
                    };
                },

                /**
                 * 画布点击事件
                 * @method canvasClick
                 * @for SIE.control.DrawViewControl
                 */
                canvasClick: function canvasClick() {
                    propertyHandler(event);
                },
            },

            /**
             * 转换设计图，子类重写
             * @method changeDesign
             * @for SIE.control.DrawViewControl
             * @param {Object} design 工艺路线设计信息
             * @returns {Object} 工艺路线信息
             */
            changeDesign: function changeDesign(design) {
                if (!design || _typeof(design) !== "object") return design;
                var pre = this.attributePrefix; //流程

                var container = design.Container;
                var flowdata = {
                    Height: container[pre + "Height"],
                    Id: container[pre + "Id"],
                    IsSelected: container[pre + "IsSelected"],
                    RoutingId: container[pre + "RoutingId"],
                    RoutingVersionId: container[pre + "RoutingVersionId"],
                    ShowGridLine: container[pre + "ShowGridLine"],
                    State: container[pre + "State"],
                    Width: container[pre + "Width"],
                    ZoomDeep: container[pre + "ZoomDeep"]
                }; //节点处理

                var nodes = [];

                if (design.Container.Activitys) {
                    if (!design.Container.Activitys["Activity"].length) {
                        design.Container.Activitys["Activity"] = [design.Container.Activitys["Activity"]];
                    }

                    var activitys = design.Container.Activitys.Activity;

                    for (var i = 0; i < activitys.length; i++) {
                        var activity = activitys[i];
                        var node;
                        var type = activity[pre + 'Type'];
                        var processType = activity[pre + 'ProcessType'];
                        var nodeData = {
                            ProcessId: parseFloat(activity[pre + "ProcessId"]),
                            Type: activity[pre + "Type"],
                            ProcessType: activity[pre + "ProcessType"],
                            State: activity[pre + "State"],
                            ZIndex: activity[pre + "ZIndex"],
                            ContainerHeight: activity[pre + "ContainerHeight"],
                            Index: parseInt(activity[pre + "Index"]),
                            ProcessState: activity[pre + "ProcessState"],
                            ShowImg: '',
                            NodeCount: parseInt(activity[pre + "NodeCount"]),   //工序节点（分支）数量
                            GroupId: activity[pre + "GroupId"],
                            GroupName: activity[pre + "GroupName"],
                            IsGroup: activity[pre + "IsGroup"] == "true" ? true : false,
                            Index: activity[pre + "Index"],
                            Boms: [],
                            ProcessParameter: activity[pre + "ProcessParameter"] ? JSON.parse(activity[pre + "ProcessParameter"]) : [],
                        };
                        if (nodeData.Type == "Initial") {
                            nodeData.SourceAnchorCount = activity[pre + "SourceAnchorCount"] == 0 ? 1 : activity[pre + "SourceAnchorCount"];//默认是一个
                        }

                        //序列化属性
                        SIE.Web.Tech.Common.Routings.PropertyExt.properties.forEach(function (property) {
                            property.serialize(nodeData, activity, pre);
                        });

                        //序列号子页签
                        SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.controls.forEach(function (ctl) {
                            ctl.serialize(nodeData, activity, pre);
                        });
                        switch (type) {
                            case "Initial":
                                node = drawBase.create('BeginNode');
                                break;

                            case "Completion":
                                node = drawBase.create('EndNode');
                                break;
                            case me.groupNodeKey:
                                node = drawBase.create('RoutingGroupNode');
                                break;
                            default:
                                node = drawBase.create('RoutingNode');
                                nodeData.ShowImg = '/images/drawtools/Assembled.svg';
                                nodeData.ShowRepeatImg = '/images/drawtools/Repeat.svg';
                                nodeData.ShowCheckImg = '/images/drawtools/Check.svg';
                                me.processType(nodeData, processType);
                                break;
                        }

                        drawBase.apply(node.designerData, nodeData);
                        node.id = activity[pre + "Id"];
                        node.left = activity[pre + "Left"];
                        node.top = activity[pre + "Top"];
                        node.ParentNodeId = activity[pre + "ParentNodeId"];
                        nodes.push(node);
                    }
                }

                var sourceCount = {};
                var lines = []; //关系处理

                if (design.Container.Rules && design.Container.Rules.Rule !== '') {
                    if (!design.Container.Rules["Rule"].length) {
                        design.Container.Rules["Rule"] = [design.Container.Rules["Rule"]];
                    }

                    var rules = design.Container.Rules.Rule;
                    rules = rules.sort(me.compareArrary(pre + "ParameterId"));
                    for (var j = 0; j < rules.length; j++) {
                        var rule = rules[j];
                        var line = {
                            id: rule[pre + "Id"],
                            sourceNode: rule[pre + "BeginActivityId"],
                            sourceAnchor: rule[pre + "SourceAnchor"],
                            targetNode: rule[pre + "EndActivityId"],
                            targetAnchor: rule[pre + "EndAnchor"]
                        };
                        var linedata = {
                            SourceActivityId: rule[pre + "SourceActivityId"],
                            State: rule[pre + "State"],
                            IsSelected: rule[pre + "IsSelected"],
                            Text: rule[pre + "Text"],
                            ZIndex: rule[pre + "ZIndex"],
                            ParameterId: rule[pre + "ParameterId"],
                            ParamResultType: rule[pre + "ParamResultType"] || '0',
                            Expression: rule[pre + "Expression"] || '',
                            StartPoint: rule[pre + "StartPoint"] || 0,
                            Point1: rule[pre + "Point1"] || 0,
                            Point2: rule[pre + "Point2"] || 0,
                            EndPoint: rule[pre + "EndPoint"] || 0,
                            Type: rule[pre + "Type"] || '',
                            BeginLeft: rule[pre + "BeginLeft"] || 0,
                            BeginTop: rule[pre + "BeginTop"] || 0,
                            EndLeft: rule[pre + "EndLeft"] || 0,
                            EndTop: rule[pre + "EndTop"] || 0,
                            TextLeft: rule[pre + "TextLeft"] || 0,
                            TextTop: rule[pre + "TextTop"] || 0,
                            Left1: rule[pre + "Left1"] || 0,
                            Left2: rule[pre + "Left2"] || 0,
                            Top1: rule[pre + "Top1"] || 0,
                            Top2: rule[pre + "Top2"] || 0,
                            Color: rule[pre + "Color"] || '#00191A'
                        };
                        line.linedata = linedata;

                        if (!sourceCount.hasOwnProperty(linedata.SourceActivityId)) {
                            sourceCount[linedata.SourceActivityId] = 0;
                        } else {
                            sourceCount[linedata.SourceActivityId] = sourceCount[linedata.SourceActivityId] + 1;
                        }

                        //line.sourceAnchor = linedata.ParamResultType !== "0" ? linedata.ParamResultType + sourceCount[linedata.SourceActivityId] : this.designConfig.SourceAnchor + sourceCount[linedata.SourceActivityId];
                        line.sourceAnchor = linedata.ParamResultType !== "0" ? linedata.ParamResultType + linedata.ParameterId : this.designConfig.SourceAnchor + sourceCount[linedata.SourceActivityId];
                        line.targetAnchor = line.targetAnchor || this.designConfig.TargetAnchor + '0';
                        lines.push(line);
                    }
                }

                var json = {};
                json.flowdata = flowdata;
                json.nodes = nodes;
                json.lines = lines;
                return json;
            },

            /**
             * 工艺路线设计信息转xml之前
             * @method beforeGetXml
             * @for SIE.control.DrawViewControl
             * @param {Object} design 工艺路线xml
             * @returns {string} 工艺路线xml
             */
            beforeGetXml: function beforeGetXml(design) {
                if (!design || _typeof(design) !== 'object') return design;
                me.updateIndex(design);
                var json = {
                    Activitys: {},
                    Rules: {}
                };
                var pre = this.attributePrefix;

                if (design.nodes && Array.isArray(design.nodes)) {
                    json.Activitys.Activity = [];
                    design.nodes.forEach(function (node) {
                        if (node.designerData.NodeType === "RoutingGroupNode") {//工序组
                            var nodeDatas = node.groupDesignerData;

                            var groupActivity = {};
                            groupActivity[pre + "Text"] = node.designerData.Text;
                            groupActivity[pre + "Id"] = node.id;
                            groupActivity[pre + "Width"] = 100;
                            groupActivity[pre + "Height"] = 60;
                            groupActivity[pre + "Left"] = node.left;
                            groupActivity[pre + "Top"] = node.top;
                            groupActivity[pre + "IsGroup"] = true;
                            groupActivity[pre + "ProcessId"] = 0;
                            groupActivity[pre + "State"] = "New";
                            groupActivity[pre + "ProcessState"] = node.designerData.ProcessState != undefined ? node.designerData.ProcessState : "Not";
                            groupActivity[pre + "ProcessType"] = node.designerData.ProcessType != undefined ? node.designerData.ProcessType : "Pqc";
                            groupActivity[pre + "Type"] = node.designerData.Type != undefined ? node.designerData.Type : "Interaction";
                            groupActivity[pre + "ZIndex"] = 999;
                            groupActivity[pre + "ContainerHeight"] = 110;
                            groupActivity[pre + "GroupId"] = node.id; //工序组Id
                            groupActivity[pre + "Index"] = node.designerData.Index;
                            if (typeof (node.designerData.ProcessParameter) != "undefined") {
                                groupActivity[pre + "NodeCount"] = node.designerData.ProcessParameter.length;
                                groupActivity[pre + "ProcessParameter"] = JSON.stringify(node.designerData.ProcessParameter);
                            }

                            groupActivity.ProcessBoms = {};
                            groupActivity.ProcessBoms.ProcessBom = [];

                            json.Activitys.Activity.push(groupActivity);//增加工序组

                            nodeDatas.forEach(function (nodeDataOuter) {
                                var activity = {};
                                var nodeData = nodeDataOuter.designerData;
                                activity[pre + "Id"] = nodeDataOuter.id;
                                activity[pre + "Width"] = 100;
                                activity[pre + "Height"] = 60;
                                activity[pre + "Left"] = node.left;
                                activity[pre + "Top"] = node.top;
                                activity[pre + "IsGroup"] = false;

                                activity[pre + "GroupId"] = node.id; //工序组Id
                                activity[pre + "GroupName"] = node.designerData.Text;//工序组名称 后面赋值

                                activity[pre + "ProcessId"] = nodeData.ProcessId;
                                activity[pre + "State"] = nodeData.State;
                                activity[pre + "ProcessState"] = nodeData.ProcessState;
                                activity[pre + "ProcessType"] = nodeData.ProcessType;
                                activity[pre + "Type"] = nodeData.Type;
                                activity[pre + "ZIndex"] = nodeData.ZIndex;
                                activity[pre + "ContainerHeight"] = nodeData.ContainerHeight;
                                activity[pre + "Index"] = nodeData.Index;
                                if (typeof (nodeData.ProcessParameter) != "undefined") {
                                    activity[pre + "NodeCount"] = nodeData.length;
                                    //将工序参数已JSON方式存放
                                    activity[pre + "ProcessParameter"] = JSON.stringify(nodeData.ProcessParameter);
                                }
                                if (typeof (node.ParentNodeId) != "undefined") {
                                    activity[pre + "ParentNodeId"] = node.ParentNodeId;
                                }

                                //反序列化属性
                                SIE.Web.Tech.Common.Routings.PropertyExt.properties.forEach(function (property) {
                                    property.deserialize(nodeData, activity, pre);
                                });
                                //反序列化子页签
                                SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.controls.forEach(function (ctl) {
                                    ctl.deserialize(nodeData, activity, pre);
                                });

                                //工序BOM
                                if (nodeData.ProcessBoms != null && nodeData.ProcessBoms != undefined) {
                                    activity.ProcessBoms = {};
                                    activity.ProcessBoms.ProcessBom = [];

                                    nodeData.ProcessBoms.forEach(function (bom) {
                                        var processBom = {};
                                        processBom[pre + "ItemId"] = bom.ItemId;
                                        processBom[pre + "Qty"] = bom.Qty;
                                        processBom[pre + "IsBuckleMaterial"] = bom.IsBuckleMaterial;

                                        if (bom.Point != null && bom.Point != undefined) {
                                            processBom[pre + "Point"] = bom.Point;
                                        }

                                        if (bom.WorkStepId != null && bom.WorkStepId != undefined && bom.WorkStepId != 0) {
                                            processBom[pre + "WorkStepId"] = bom.WorkStepId;
                                        }

                                        if (bom.IsAttachment != null && bom.IsAttachment != undefined) {
                                            processBom[pre + "IsAttachment"] = bom.IsAttachment;
                                        }

                                        if (bom.IsExternal != null && bom.IsExternal != undefined) {
                                            processBom[pre + "IsExternal"] = bom.IsExternal;
                                        }

                                        if (bom.IsSingleLabel != null && bom.IsSingleLabel != undefined) {
                                            processBom[pre + "IsSingleLabel"] = bom.IsSingleLabel;
                                        }


                                        if (bom.HasBarcodeRule != null && bom.HasBarcodeRule != undefined) {
                                            processBom[pre + "HasBarcodeRule"] = bom.HasBarcodeRule;
                                        }

                                        if (bom.MainMaterialId != null && bom.MainMaterialId != undefined && bom.MainMaterialId != 0) {
                                            processBom[pre + "MainMaterialId"] = bom.MainMaterialId;
                                        }

                                        activity.ProcessBoms.ProcessBom.push(processBom);
                                    });
                                }
                                json.Activitys.Activity.push(activity);
                            });

                        }
                        else//单工序
                        {
                            var activity = {};
                            var nodeData = node.designerData;
                            activity[pre + "Id"] = node.id;
                            activity[pre + "Width"] = 100;
                            activity[pre + "Height"] = 60;
                            activity[pre + "Left"] = node.left;
                            activity[pre + "Top"] = node.top;
                            activity[pre + "ProcessId"] = nodeData.ProcessId;
                            activity[pre + "State"] = nodeData.State;
                            activity[pre + "ProcessState"] = nodeData.ProcessState;
                            activity[pre + "ProcessType"] = nodeData.ProcessType;
                            activity[pre + "Type"] = nodeData.Type;
                            activity[pre + "ZIndex"] = nodeData.ZIndex;
                            activity[pre + "ContainerHeight"] = nodeData.ContainerHeight;
                            activity[pre + "Index"] = nodeData.Index;
                            activity[pre + "IsGroup"] = false;
                            if (typeof (nodeData.ProcessParameter) != "undefined") {
                                activity[pre + "NodeCount"] = nodeData.ProcessParameter.length;
                                //将工序参数已JSON方式存放
                                activity[pre + "ProcessParameter"] = JSON.stringify(nodeData.ProcessParameter);
                            }

                            if (typeof (node.ParentNodeId) != "undefined") {
                                activity[pre + "ParentNodeId"] = node.ParentNodeId;
                            }
                            //支持多分支
                            if (node.designerData.Type == "Initial") {

                                var endNodes = window.jsp.getEndpoints(node.id);
                                activity[pre + "SourceAnchorCount"] = endNodes ? endNodes.length : 1;
                            }

                            //反序列化属性
                            SIE.Web.Tech.Common.Routings.PropertyExt.properties.forEach(function (property) {
                                property.deserialize(nodeData, activity, pre);
                            });
                            //反序列化子页签
                            SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.controls.forEach(function (ctl) {
                                ctl.deserialize(nodeData, activity, pre);
                            });

                            //工序BOM
                            if (nodeData.ProcessBoms != null && nodeData.ProcessBoms != undefined) {
                                activity.ProcessBoms = {};
                                activity.ProcessBoms.ProcessBom = [];

                                nodeData.ProcessBoms.forEach(function (bom) {
                                    var processBom = {};
                                    processBom[pre + "ItemId"] = bom.ItemId;
                                    processBom[pre + "Qty"] = bom.Qty;
                                    processBom[pre + "IsBuckleMaterial"] = bom.IsBuckleMaterial;

                                    if (bom.Point != null && bom.Point != undefined) {
                                        processBom[pre + "Point"] = bom.Point;
                                    }

                                    if (bom.WorkStepId != null && bom.WorkStepId != undefined && bom.WorkStepId != 0) {
                                        processBom[pre + "WorkStepId"] = bom.WorkStepId;
                                    }

                                    if (bom.IsAttachment != null && bom.IsAttachment != undefined) {
                                        processBom[pre + "IsAttachment"] = bom.IsAttachment;
                                    }

                                    if (bom.IsExternal != null && bom.IsExternal != undefined) {
                                        processBom[pre + "IsExternal"] = bom.IsExternal;
                                    }

                                    if (bom.IsSingleLabel != null && bom.IsSingleLabel != undefined) {
                                        processBom[pre + "IsSingleLabel"] = bom.IsSingleLabel;
                                    }

                                    if (bom.HasBarcodeRule != null && bom.HasBarcodeRule != undefined) {
                                        processBom[pre + "HasBarcodeRule"] = bom.HasBarcodeRule;
                                    }

                                    if (bom.MainMaterialId != null && bom.MainMaterialId != undefined && bom.MainMaterialId != 0) {
                                        processBom[pre + "MainMaterialId"] = bom.MainMaterialId;
                                    }

                                    activity.ProcessBoms.ProcessBom.push(processBom);
                                });
                            }

                            json.Activitys.Activity.push(activity);
                        }
                    });
                }

                if (design.lines && Array.isArray(design.lines)) {
                    json.Rules.Rule = [];
                    design.lines.forEach(function (line) {
                        var rule = {};
                        rule[pre + "Id"] = line.id;
                        rule[pre + "BeginActivityId"] = line.sourceNode;
                        rule[pre + "EndActivityId"] = line.targetNode; //扩展属性
                        //rule[pre + "SourceAnchor"] = line.sourceAnchor;
                        //rule[pre + "EndAnchor"] = line.targetAnchor;

                        var linedata = line.linedata;

                        if (linedata) {
                            var pros = Object.getOwnPropertyNames(linedata);

                            for (var i = 0; i < pros.length; i++) {
                                var pro = pros[i];
                                rule[pre + pro] = linedata[pro];
                            }
                        }

                        json.Rules.Rule.push(rule);
                    });
                }

                return json;
            },

            /**
             * 工艺路线设计信息转xml之后
             * @method afterGetXml
             * @for SIE.control.DrawViewControl
             * @param {string} xml 工艺路线xml
             * @param {Object} json 工艺路线信息
             * @returns {string} 工艺路线xml
             */
            afterGetXml: function afterGetXml(xml, json) {
                var result = '<Container';

                if (json.flowdata) {
                    var flowdata = json.flowdata;
                    var pros = Object.getOwnPropertyNames(flowdata);

                    for (var i = 0; i < pros.length; i++) {
                        var pro = pros[i];
                        if (pro == "Height") {
                            var height = document.getElementById(this.canvas.id).style.height;
                            result += ' ' + pro + '="' + height.replace("px", "") + '"';
                        }
                        else if (pro == "Width") {
                            var width = document.getElementById(this.canvas.id).style.width;
                            result += ' ' + pro + '="' + width.replace("%", "") + '"';
                        }
                        else { result += ' ' + pro + '="' + flowdata[pro] + '"'; }
                    }
                }

                result += '>';
                result += xml;
                result += '</Container>';
                return result;
            }
        });
        me.mainView.routingDrawControlId = me.drawControl.id;
        me.drawControl.drawTools.setLock(true);

        document.addEventListener("mousemove", function (e) {

            var rigthWidths = document.getElementsByClassName("x-panel x-border-item x-box-item x-panel-default");
            var rigthHeights = document.getElementsByClassName("x-autocontainer-outerCt");
            var scrollTop = 0;
            var cavasDom = document.getElementById("techCanvas");
            if (cavasDom && cavasDom.parentNode) {
                if (me.selectNodes.length == 1) {
                    window.jsp.clearDragSelection();
                }
                scrollTop = cavasDom.parentNode.parentNode.parentNode.scrollTop;
            }
            if (cavasDom && cavasDom.parentElement) {
                if (me.selectNodes.length == 1) {
                    window.jsp.clearDragSelection();
                }
                scrollTop = cavasDom.parentElement.parentElement.parentElement.scrollTop;
            }
            const x = e.pageX - e.offsetX - rigthWidths[2].clientWidth;
            const y = e.pageY - e.offsetY - rigthHeights[0].clientHeight + scrollTop;
            me.drawControl.drawTools.setOffset(x, y);
        });

        document.addEventListener("click", function (e) {
            me.mouseEvent = e;//记录当前点击的鼠标参数

        });

        //框选
        var canvasDom = document.getElementById(me.canvas);
        if (canvasDom) {

            canvasDom.onmousedown = function (event) {
                event = event || window.event
                var posx = event.offsetX;
                var posy = event.offsetY;
                var div = document.createElement("div");
                div.id = "selectDiv";
                div.style.position = "absolute";

                var startX = event.x || event.clientX;
                var startY = event.y || event.clientY;
                div.style.left = startX + "px";
                div.style.top = startY + "px";
                div.style.background = "rgba(255, 255, 255,0.1)";
                div.style.border = "1px solid rgba(255, 69, 0,1)";
                div.style.zIndex = "9999";
                canvasDom.appendChild(div);

                canvasDom.onmousemove = function (ev) {
                    if (div) {
                        console.log(this.id);
                        if (ev.srcElement.id != "techCanvas" && ev.srcElement.id != "selectDiv" && ev.srcElement.id != this.id) {
                            //鼠标不允许进入工序块以及工序组中，否则会出现闪烁
                        } else {
                            if (ev.srcElement.id != "selectDiv") {
                                div.style.left = Math.min(ev.offsetX, posx) + "px";
                                div.style.top = Math.min(ev.offsetY, posy) + "px";
                                div.style.width = Math.abs(posx - ev.offsetX) + "px";
                                div.style.height = Math.abs(posy - ev.offsetY) + "px";
                            }
                        }
                    }
                };
                canvasDom.onmouseup = function (event) {
                    var selDiv = document.getElementById("selectDiv");
                    var fileDivs = document.getElementsByClassName("jtk-node");
                    var selectedEls = [];
                    var selDivLeft = selDiv.offsetLeft; // 减去容器位置
                    var selDivTop = selDiv.offsetTop; // 减去容器位置
                    var selDivWidth = selDiv.offsetWidth;
                    var selDivHeight = selDiv.offsetHeight;
                    for (var i = 0; i < fileDivs.length; i++) {
                        //所有节点
                        var selectedLeft = fileDivs[i].offsetLeft;
                        var selectedTop = fileDivs[i].offsetTop;
                        if (selectedLeft > selDivLeft && selectedTop > selDivTop && selectedLeft < selDivLeft + selDivWidth && selectedTop < selDivTop + selDivHeight) {
                            // 区域内选中节点
                            selectedEls.push(fileDivs[i].id);

                            var node = me.drawControl.drawTools.getNode(fileDivs[i].id)
                            if (node) {
                                me.isCtrl = true;
                                me.updateNodeBoxShadow(node, 'rgba(15, 124, 245, 1) 0px 0px 0px 2px', '45px', '118px');
                            }
                        }
                    }
                    if (div) {
                        div.parentNode.removeChild(div);
                        canvasDom.onmousemove = null;
                        canvasDom.onmouseup = null;

                        me.isCtrl = false;
                        event.stopPropagation();
                    }
                    //再次清除
                    var delSelectDiv = document.getElementById("selectDiv");
                    if (delSelectDiv) {
                        delSelectDiv.parentNode.removeChild(delSelectDiv);

                    }
                };

            };
        }
    },

    /**
     * 更新节点的父节点信息
     * @param {any} connInfo 连接线
     */
    updateNodeParent: function (connInfo) {
        var me = this;
        var drawTools = Ext.getCmp(me.mainView.routingDrawControlId).drawTools;
        var sourceNode = drawTools.getNode(connInfo.sourceId);
        var targetNode = drawTools.getNode(connInfo.targetId);
        if (!(sourceNode.nodeType === 'BeginNode' || targetNode.nodeType === 'EndNode')) {
            if (connInfo.sourceEndpoint.canvas.dataset.AnchorType === 'Optional') {
                targetNode.ParentNodeId = connInfo.sourceId;
                targetNode.designerData.SourceAnchor.forEach(function (p) {
                    p.connectorStyle.dashstyle = me.dashstyle;
                });
            }
            if (sourceNode.ParentNodeId) {
                //var parentNodeId = me.drawControl.drawTools.getCurInstance().getReverseMappingId(sourceNode.ParentNodeId);
                if (sourceNode.ParentNodeId !== connInfo.targetId) {
                    targetNode.ParentNodeId = sourceNode.ParentNodeId;
                    targetNode.designerData.SourceAnchor.forEach(function (p) {
                        p.connectorStyle.dashstyle = me.dashstyle;
                    });
                }
            }
        }
    },

    getProcessInfo: function (processIds, parameterCallback) {
        var me = this;
        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "GetProcessInfo",
            token: me.mainView.token,
            params: [processIds],
            callback: function callback(res) {
                if (res.Success) {
                    parameterCallback(res.Result.ProcessParameter);
                }
            },
            async: false
        });
    },

    /**
     * 设置锚点
     * @param {any} node 节点
     * @param {any} params 工序参数
     */
    setAnchor: function (node, params) {
        var me = this;
        if (params)
            params.sort(me.compareArrary("Id"));

        node.designerData.SourceAnchor = []; //创建来源节点配置参数
        node.designerData.ProcessParameter = params;
        Ext.Array.each(params, function (p) {
            var anchor = Ext.clone(me.defaultSourceEndpointStyle["oranger"]);

            switch (p.Type) {
                case 1:
                    anchor = Ext.clone(me.defaultSourceEndpointStyle["green"]);
                    anchor.AnchorType = 'Pass';
                    break;

                case 2:
                    anchor = Ext.clone(me.defaultSourceEndpointStyle["red"]);
                    anchor.AnchorType = 'Fail';
                    break;

                case 3:
                    anchor = Ext.clone(me.defaultSourceEndpointStyle["oranger"]);
                    anchor.AnchorType = 'Any';
                    break;

                case 4:
                    anchor = Ext.clone(me.defaultSourceEndpointStyle["lightBlue"]);
                    anchor.AnchorType = 'Custom';
                    break;
                case 8:
                    anchor = Ext.clone(me.defaultSourceEndpointStyle["gray"]);
                    anchor.AnchorType = 'Optional';
                    break;
            }
            anchor.paramId = p.Id;
            anchor.LineLabel = p.Description;
            if (node.ParentNodeId) {
                anchor.connectorStyle.dashstyle = me.dashstyle;
            }
            node.designerData.SourceAnchor.push(anchor);
        });
    },

    /**
     * 获取参数类型
     * @param {any} strType 参数类型
     */
    getParameterType: function (strType) {
        var type = 0;

        switch (strType) {
            case 'Pass':
                type = 1;
                break;

            case 'Fail':
                type = 2;
                break;

            case 'Any':
                type = 3;
                break;

            case 'Custom':
                type = 4;
                break;
            case 'Optional':
                type = 8;
                break;
        }

        return type;
    },

    /**
     * 初始化工序节点图标背景色
     * @method initProcessImgBackground
     * @for DesignCanvas
     */
    initProcessImgBackground: function () {
        var me = this;
        me.dicProcessImgBackground['Assembly'] = me.dicProcessImgBackground['BatchAssembly'] = '#0D7CF5';
        me.dicProcessImgBackground['Fix'] = me.dicProcessImgBackground['BatchFix'] = '#F43B5A';
        me.dicProcessImgBackground['Pqc'] = me.dicProcessImgBackground['BatchPqc'] = '#23D89E';
        me.dicProcessImgBackground['Packing'] = me.dicProcessImgBackground['BatchPacking'] = '#21D4DB';
        me.dicProcessImgBackground['Fqc'] = '#6D56E2';
    },

    /**
     * 设置工序类型图标和颜色
     * @method processType
     * @for DesignCanvas
     * @param {Object} nodeData 工序节点信息
     * @param {string} processType 工序类型
     */
    processType: function (nodeData, _processType) {
        var me = this;
        var imgBackground = '#7C44FF';
        if (me.dicProcessImgBackground[_processType]) imgBackground = me.dicProcessImgBackground[_processType];
        nodeData.ImgBackground = imgBackground;
        nodeData.ShowRepeatImg = '/images/drawtools/Repeat.svg';
        nodeData.ShowCheckImg = '/images/drawtools/Check.svg';
        switch (_processType) {
            case "Assembly":
                nodeData.ShowImg = '/images/drawtools/Assembly.svg';
                break;

            case "Fix":
                nodeData.ShowImg = '/images/drawtools/Fix.svg';
                break;

            case "Pqc":
                nodeData.ShowImg = '/images/drawtools/Pqc.svg';
                break;

            case "Packing":
                nodeData.ShowImg = '/images/drawtools/Packing.svg';
                break;

            case "Fqc":
                nodeData.ShowImg = '/images/drawtools/Fqc.svg';
                break;

            case "BatchAssembly":
                nodeData.ShowImg = '/images/drawtools/Assembly.svg';
                break;

            case "BatchFix":
                nodeData.ShowImg = '/images/drawtools/Fix.svg';
                break;

            case "BatchPqc":
                nodeData.ShowImg = '/images/drawtools/Pqc.svg';
                break;

            case "BatchPacking":
                nodeData.ShowImg = '/images/drawtools/Packing.svg';
                break;

            default:
                nodeData.ShowImg = '/images/drawtools/Default.svg';
                break;
        }
    },

    /**
     * 更新活动节点的索引
     * @method updateIndex
     * @for DesignCanvas
     * @param {Object} design 工序路线信息
     */
    updateIndex: function (design) {
        var me = this;
        var nodes = design.nodes;
        var lines = design.lines;
        var beginNode, firstLine;
        var nodeIdList = []; //先把所有节点的索引置为-1

        Ext.Array.forEach(nodes, function (node) {
            node.designerData.Index = -1;
        }); //获取开始节点和第一条连线

        Ext.Array.each(nodes, function (node) {
            if (node.nodeType === 'BeginNode') {
                beginNode = node;
                Ext.Array.each(lines, function (line) {
                    if (line.sourceNode === beginNode.id) {
                        firstLine = line;
                        return false;
                    }
                });
                return false;
            }
        });
        beginNode.designerData.Index = 0;

        if (firstLine) {
            me.UpdateTrunkIndex(nodeIdList, firstLine.targetNode, nodes, lines, me.indexStep);
        } //更新NG工序的索引


        var maxIndexNode = Ext.Array.max(nodes, function (max, item) {
            if (Number(item.designerData.Index) === Number(max.designerData.Index)) {
                return 0;
            } else if (Number(item.designerData.Index) < Number(max.designerData.Index)) {
                return 1;
            } else {
                return -1;
            }
        });
        var maxIndex = maxIndexNode.designerData.Index;
        var ngNodes = Ext.Array.filter(nodes, function (node) {
            if (node.designerData.Index === -1) return true; else return false;
        });
        Ext.Array.forEach(ngNodes, function (node) {
            maxIndex = maxIndex + me.indexStep;
            node.designerData.Index = maxIndex;
        });
    },

    /**
     * 更新活动节点的索引
     * @method UpdateTrunkIndex
     * @for DesignCanvas
     * @param {number} targetNodeId 模板节点ID
     * @param {Array} nodes 节点集合
     * @param {Array} lines 规则集合
     * @param {number} index 索引
     */
    UpdateTrunkIndex: function (nodeIdList, targetNodeId, nodes, lines, index) {
        var me = this;
        var nextNode, passLine, anyLine; //先处理PASS的工序

        Ext.Array.each(nodes, function (node) {
            if (node.id === targetNodeId) {
                nextNode = node;
                return false;
            }
        });
        if (me.IsOrderNode(nodeIdList, targetNodeId) == false) nodeIdList.push(targetNodeId); else return;
        Ext.Array.each(lines, function (line) {
            if (line.sourceNode === nextNode.id && line.linedata.ParamResultType === 'Pass') {
                passLine = line;
                return false;
            }
        });
        nextNode.designerData.Index = index;

        if (passLine) {
            me.UpdateTrunkIndex(nodeIdList, passLine.targetNode, nodes, lines, index + me.indexStep);
            return;
        } //如果找不到PASS的就找任意的


        Ext.Array.each(lines, function (line) {
            if (line.sourceNode === nextNode.id && line.linedata.ParamResultType === 'Any') {
                anyLine = line;
                return false;
            }
        });

        if (anyLine) {
            me.UpdateTrunkIndex(nodeIdList, anyLine.targetNode, nodes, lines, index + me.indexStep);
        }
    },

    /**
     * 是否已遍历过节点（不判断会死循环）
     * @method IsOrderNode
     * @for DesignCanvas
     * @param {Array} nodeIdList 节点ID集合
     * @param {number} nodeId 模板节点ID
     */
    IsOrderNode: function (nodeIdList, nodeId) {
        var me = this;
        var len = nodeIdList.length;

        for (var i = 0; i < len; i++) {
            if (nodeIdList[i] == nodeId) return true;
        }

        return false;
    },

    /**
     * 初始化节点中的锚样式
     * @method initSourceEndpoint
     * @for DesignCanvas
     */
    initSourceEndpoint: function () {
        var me = this; //绿色锚
        // 基本连接线样式
        var connectorPaintStyle = {
            strokeWidth: 2,
            stroke: "#00FF00",
            joinstyle: "round",
            outlineStroke: "white",
            outlineWidth: 2
        },
            //鼠标悬浮在连线上的样式
            connectorHoverStyle = {
                strokeWidth: 2,
                stroke: "#000000",
                outlineWidth: 2,
                outlineStroke: "white"
            },
            endpointHoverStyle = {
                stroke: "#216477"
            },
            // the definition of source endpoints (the small blue ones)
            greenEndpoint = {
                endpoint: "Dot",
                paintStyle: {
                    stroke: "#00FF00",
                    //fill: "transparent",
                    radius: 7,
                    zIndex: 20,
                    strokeWidth: 0
                },
                isSource: true,
                // connector: [ "Flowchart", { stub: [40, 60], gap: 10, cornerRadius: 5, alwaysRespectStubs: true } ],
                connector: ["StateMachine"],
                connectorStyle: connectorPaintStyle,
                hoverPaintStyle: endpointHoverStyle,
                connectorHoverStyle: connectorHoverStyle,
                dragOptions: {//禁止拖动
                    //disableDrag: false,
                },
                maxConnections: 1,
                overlays: [["Label", {
                    location: [0.5, 1.5],
                    label: "",
                    cssClass: "endpointSourceLabel",
                    visible: false
                }]]
            }; //#D52BB3 紫色   

        //#FF0000 红色 失败
        var redEndpoint = Ext.clone(greenEndpoint);
        redEndpoint.paintStyle.stroke = '#FF0000';
        redEndpoint.connectorStyle.stroke = '#FF0000';

        //#FF8433 橙色 任意
        var orangerEndpoint = Ext.clone(greenEndpoint);
        orangerEndpoint.paintStyle.stroke = '#FF8433';
        orangerEndpoint.connectorStyle.stroke = '#FF8433'; //#00E5E6

        //#00E5E6 蓝色 自定义
        var lightBlueEndpoint = Ext.clone(greenEndpoint);
        lightBlueEndpoint.paintStyle.stroke = '#00E5E6';
        lightBlueEndpoint.connectorStyle.stroke = '#00E5E6';

        //#808080 灰色 可选
        var grayEndpoint = Ext.clone(greenEndpoint);
        grayEndpoint.paintStyle.stroke = '#808080'; //灰色
        grayEndpoint.connectorStyle.stroke = '#808080';
        grayEndpoint.connectorStyle.dashstyle = me.dashstyle;
        grayEndpoint.maxConnections = -1;

        me.defaultSourceEndpointStyle = {
            green: greenEndpoint,
            red: redEndpoint,
            oranger: orangerEndpoint,
            lightBlue: lightBlueEndpoint,
            gray: grayEndpoint,
        };
    },

    /**
     * 定义节点格式
     * @method initNode
     * @for DesignCanvas
     */
    initNode: function () {
        drawBase.define('RoutingNode', 'DesignerNode', {
            tpl: '<div class="node" data-nodeType="RoutingNode" style="background-color:#FFFFFF; width:120px; height:46px; border-radius:4px;box-shadow:0px 3px 5px 0px rgba(175,193,215,0.4); padding-left:5px; padding-top:5px;">'
                + '<div style="float:left;width:38px;text-align:center;height:38px;color:#FFFFFF; background:{ImgBackground};border-radius:30px;padding-top:5px"><img draggable="false" src="{ShowImg}" style="width:28px; height:28px;"></div>'
                + '<div style="float:left;width:70px; padding-left:5px; height:40px; font-size:12px; color:#3B444D;align-items:center; display: -webkit-flex;word-break:break-word;overflow: hidden;cursor: default;">{Text}'
                + '</div><div style="float: right;position: relative;width:1px;right: 15px;" ><div style="height:15px;width:15px;"><img  style="height:15px;width:15px;display:none;" name="isCheck" draggable="false" src="{ShowCheckImg}"></img></div>'
                + '<div style="height:15px;width:15px;position: absolute;bottom: -22px;"><img  style="height:15px;width:15px;display:none;" name="isRepeat" draggable="false" src="{ShowRepeatImg}"></img></div></div>'
                + '</div>',
            nodeType: 'RoutingNode'
        });
        //定义工序组模板
        drawBase.define('RoutingGroupNode', 'DesignerNode', {
            tpl: '<div class="node" data-nodeType="RoutingGroupNode" style=" z-index:99 !important; background-color:rgba(255,255,255,0.5);display:table-cell;vertical-align: middle;text-align: center; width:140px; height:200px; border-radius:4px;box-shadow:0px 3px 5px 0px rgba(175,193,215,0.4);">'
                + '<div style="height:32px;background:#ADD2ED;vertical-align: middle;display: flex;border-radius:4px 4px 0px 0px;"><span style="margin-left:5px;font-size:14px;vertical-align:middle;font-weight:bold;align-self:center;cursor: default;">{Text}</span></div>'
                + '<div style="font-size:12px; color:#3B444D;align-items:center; display: inline-block;word-break:break-word;overflow: hidden;" >{ProcessHTML}</div></div > ',
            nodeType: 'RoutingNode'
        });
    },

    /**
     * 显示Line编辑窗口
     * @method showLineLabelWindow
     * @for DesignCanvas
     * @param {object} conninfo 规则信息
     */
    showLineWindow: function (conninfo) {
        var me = this;

        var linedata = {};

        if (!conninfo.linedata) {
            return;
        }

        linedata = JSON.parse(conninfo.linedata);
        if (linedata.ParamResultType === 'Custom') {
            me.showScriptWindow(conninfo);
        }
        else if (linedata.ParamResultType === 'Optional') {
            me.showLineLabelWindow(conninfo);
        }
    },

    /**
     * 显示Line标签编辑窗口
     * @method showLineLabelWindow
     * @for DesignCanvas
     * @param {object} conninfo 规则信息
     */
    showLineLabelWindow: function (conninfo) {
        var me = this;
        var drawTools = Ext.getCmp(me.mainView.routingDrawControlId).drawTools;
        var node = drawTools.getNode(conninfo.sourceId);

        var linedata = {};
        linedata = JSON.parse(conninfo.linedata);
        var scriptPanel = Ext.create('Ext.panel.Panel', {
            viewModel: {},
            //layout: 'fit',
            border: false,
            items: [{
                fieldLabel: '线标签'.L10N(),
                label: '连线标签'.L10N(),
                xtype: 'textfield',
                name: 'LabelText',
                bind: '{l.Text}',
                value: linedata.Text,
                allowBlank: false,
                renderer: function renderer() { }
            }]
        });
        scriptPanel.getViewModel().setData({
            l: linedata
        }); //弹窗

        var win = SIE.Window.show({
            title: '连线标签编辑'.L10N(),
            items: scriptPanel,
            width: 350,
            height: 150,
            modal: true,
            callback: function callback(btn) {
                if (btn.t() === '确定'.L10N()) {
                    conninfo.linedata = JSON.stringify(linedata);
                    conninfo.canvas.setAttribute('data-linedata', conninfo.linedata);
                    conninfo.getOverlay("label").setLabel(linedata.Text);

                    var sourceAnchor = node.designerData.SourceAnchor.first(function (p) {
                        return p.paramId == linedata.ParameterId;
                    });
                    if (sourceAnchor && sourceAnchor.LineLabel !== linedata.Text) {
                        sourceAnchor.LineLabel = linedata.Text;
                    }
                    win.close();
                    return false;
                }
            }
        });
    },

    /**
     * 显示脚本编辑窗口
     * @method showScriptWindow
     * @for DesignCanvas
     * @param {object} conninfo 规则信息
     */
    showScriptWindow: function (conninfo) {
        var linedata = {};
        linedata = JSON.parse(conninfo.linedata);
        var versionRecord = me.mainView.CurRoutingVersion;
        var isPublish = versionRecord.get('state') === 1;
        var scriptPanel = Ext.create('Ext.panel.Panel', {
            viewModel: {},
            layout: 'fit',
            border: false,
            items: [{
                xtype: 'textareafield',
                name: 'ScriptText',
                bind: '{l.Expression}',
                fieldStyle: 'word-wrap:normal',
                readOnly: isPublish,
                renderer: function renderer() { }
            }]
        });
        scriptPanel.getViewModel().setData({
            l: linedata
        }); //弹窗

        var win = SIE.Window.show({
            title: '脚本编辑'.L10N(),
            items: scriptPanel,
            width: 600,
            height: 300,
            modal: true,
            callback: function callback(btn) {
                if (btn.t() === '确定'.L10N()) {
                    conninfo.linedata = JSON.stringify(linedata);
                    conninfo.canvas.setAttribute('data-linedata', conninfo.linedata);
                    win.close();
                    return false;
                }
            }
        });
    },

    /**
     * 清除工艺工艺路线
     * @method clearDrawControl
     * @for DesignCanvas
     */
    clearDrawControl: function () {
        var me = this;
        if (me.drawControl) {
            me.drawControl.clear();
            me.drawControl.drawTools.resetGroupIndex();
        }
    },

    /**
     * 画工艺路线
     * @method drawRouting
     * @for DesignCanvas
     * @param {xml} layout 工艺路线布局
     */
    drawRouting: function (layout) {
        var me = this;
        me.drawControl.draw(layout, true);
    },

    /**
     * 画节点
     * @method drawNode
     * @for DesignCanvas
     * @param {Object} node 节点信息
     */
    drawNode: function (node) {
        var me = this;
        me.drawControl.dragNode(node);
    },

    /**
     * 设置画布锁，禁止编辑
     * @method setLock
     * @for DesignCanvas
     * @param {Boolean} lock 是否锁定
     */
    setLock: function (lock) {
        var me = this;
        me.drawControl.drawTools.setLock(lock);
    },

    /**
     * 获取工艺路线设计信息
     * @method getDesignData
     * @for DesignCanvas
     * @returns {object} 工艺路线设计信息（节点，规则等等）
     */
    getDesignData: function getDesignData() {
        var me = this;
        return me.drawControl.generalDesign();
    },

    /**
     * 更新节点背景色
     * @method updateNodesColor
     * @for DesignCanvas
     */
    updateNodesColor: function updateNodesColor() {
        var me = this;

        if (Object.keys(me.dicNodeColor).length === 0) {
            me.dicNodeColor['Current'] = '#B0C4DE';
            me.dicNodeColor['Has'] = '#CED0D2';
            me.dicNodeColor['Not'] = '#FFFFFF';
        }

        var design = me.getDesignData();
        design.nodes.forEach(function (node) {
            switch (node.designerData.ProcessState) {
                case 'Current':
                    me.updateNodeColor(node, 'Current', me.dicNodeColor['Current']);
                    break;
                case 'Has':
                    me.updateNodeColor(node, 'Has', me.dicNodeColor['Has']);
                    break;
                case 'Not':
                    me.updateNodeColor(node, 'Not', me.dicNodeColor['Not']);
                    break;
            }
        });
    },

    /**
     * 更新节点背景色
     * @method updateNodeColor
     * @for DesignCanvas
     * @param {ListLogicalView} node 节点信息
     * @param {string} status 状态
     * @param {string} color 背景色
     */
    updateNodeColor: function updateNodeColor(node, status, color) {
        var nodeElement = document.getElementById(node.id);
        if (!nodeElement || !color) return;
        var me = this;
        nodeElement.style.backgroundColor = color;
        var imageBackgroud = me.dicProcessImgBackground[node.designerData.ProcessType];

        if (status !== 'Not') {
            imageBackgroud = color;
        }
        if (nodeElement.childNodes[0].style)
            nodeElement.childNodes[0].style.background = imageBackgroud === undefined || imageBackgroud === null ? '#7C44FF' : imageBackgroud;
    },
    /**
     * 取消选中
     * @param {any} design
     */

    cancelSelected: function (design) {
        var me = this;
        design.nodes.forEach(function (item) {
            var itemElement = document.getElementById(item.id);
            if (itemElement.nodeBoxShadowInfo && itemElement.nodeBoxShadowInfo.isUpdateBoxShadow) {
                itemElement.style.boxShadow = itemElement.nodeBoxShadowInfo.oldBoxShadow;
                if (item.designerData.NodeType != me.groupNodeKey) {//工序组不更新回旧尺寸
                    itemElement.style.height = itemElement.nodeBoxShadowInfo.oldHeight;
                    itemElement.style.width = itemElement.nodeBoxShadowInfo.oldWidth;
                }
                itemElement.nodeBoxShadowInfo.isUpdateBoxShadow = false;
            }
        });
        var innerNodes = document.getElementsByClassName("node");
        for (var i = 0; i < innerNodes.length; i++) {
            var itemElement = innerNodes[i];
            if (itemElement.nodeBoxShadowInfo && itemElement.nodeBoxShadowInfo.isUpdateBoxShadow) {
                itemElement.style.boxShadow = itemElement.nodeBoxShadowInfo.oldBoxShadow;
                itemElement.style.height = itemElement.nodeBoxShadowInfo.oldHeight;
                itemElement.style.width = itemElement.nodeBoxShadowInfo.oldWidth;
                itemElement.nodeBoxShadowInfo.isUpdateBoxShadow = false;
            }
        }
        this.selectNodes = [];
        this.setLayoutCommandsDisabled(this.selectNodes.length);
        CRT.Context.PageContext.setContext("selectNodes", this.selectNodes);//将选中信息覆盖
        window.jsp.clearDragSelection();
    },


    /**
     * 更新选择节点边框和高宽
     * @param {any} node 节点信息
     * @param {any} boxShadow 边框样式
     * @param {any} height 高度
     * @param {any} width 宽度
     */
    updateNodeBoxShadow: function updateNodeBoxShadow(node, boxShadow, height, width) {
        var me = this;
        var design = me.getDesignData();
        if (!node) {
            //window.jsp.clearDragSelection();
            me.cancelSelected(design);
            return;
        }
        var nodeElement = document.getElementById(node.id);
        if (!nodeElement || !boxShadow || !height || !width) return;
        if (!me.isCtrl)//ctrl键  不取消原来块的选中
        {
            //window.jsp.removeFromDragSelection(nodeElement);
            me.cancelSelected(design);
        }
        if (nodeElement.nodeBoxShadowInfo) {
            nodeElement.nodeBoxShadowInfo.isUpdateBoxShadow = true;
        } else {
            nodeElement.nodeBoxShadowInfo = {
                isUpdateBoxShadow: true,
                oldBoxShadow: nodeElement.style.boxShadow,
                oldHeight: nodeElement.style.height,
                oldWidth: nodeElement.style.width
            };
        }
        if (node.nodeType === 'RoutingNode') {
            nodeElement.style.boxShadow = boxShadow;//工序组只增加边框
            this.selectNodes.push(node);
            CRT.Context.PageContext.setContext("selectNodes", this.selectNodes);

            this.setLayoutCommandsDisabled(this.selectNodes.length);
            if (node.designerData.NodeType != me.groupNodeKey) {
                nodeElement.style.height = height;
                nodeElement.style.width = width;
            }
            window.jsp.clearDragSelection();
            this.selectNodes.forEach(moveNode => {
                window.jsp.addToDragSelection(document.getElementById(moveNode.id));//增加多个拖动
            });
        }
    },
    /**
     缩小
     */
    zoomIn: function () {
        var me = this;
        me.drawControl.drawTools.zoomIn();
    },
    /*
     *放大
     */
    zoomOut: function () {
        var me = this;
        me.drawControl.drawTools.zoomOut();
    },
    /**
     * 缩放回原比例
     * */
    zoomOriginal: function () {
        var me = this;
        me.drawControl.drawTools.resetZoom();
    },


    /**
     * 左右居中对齐
     * */
    leftRightAlignment: function () {
        if (this.selectNodes.length >= 2) {
            var leftPositionArr = [];
            var doms = [];
            for (var i = 0; i < this.selectNodes.length; i++) {
                var curNode = this.selectNodes[i];
                var curNodeDom = document.getElementById(curNode.id);
                if (curNodeDom) {
                    leftPositionArr.push(parseInt(curNodeDom.style.left.replace("px", "")));
                    doms.push(curNodeDom);
                }
            }
            var maxLeft = Math.max.apply(null, leftPositionArr);
            var minLeft = Math.min.apply(null, leftPositionArr);
            var newLeft = (maxLeft + minLeft) / 2;
            for (var i = 0; i < doms.length; i++) {
                doms[i].style.left = newLeft + "px";
            }
            window.jsp.repaintEverything();
        }

    },
    /**
     * 上下居中
     * @param {any} _routingDesignControrl
     */
    verticalAlignment: function () {
        if (this.selectNodes.length >= 2) {
            var topPositionArr = [];
            var doms = [];
            for (var i = 0; i < this.selectNodes.length; i++) {
                var curNode = this.selectNodes[i];
                var curNodeDom = document.getElementById(curNode.id);
                if (curNodeDom) {
                    topPositionArr.push(parseInt(curNodeDom.style.top.replace("px", "")));
                    doms.push(curNodeDom);
                }
            }
            var maxTop = Math.max.apply(null, topPositionArr);
            var minTop = Math.min.apply(null, topPositionArr);
            var newTop = (maxTop + minTop) / 2;
            for (var i = 0; i < doms.length; i++) {
                doms[i].style.top = newTop + "px";
            }
            window.jsp.repaintEverything();
        }
    },
    /**
     *横向分布
     * */
    horizontalDistribution: function () {
        if (this.selectNodes.length >= 3) {
            var leftPositionArr = [];
            var doms = [];
            for (var i = 0; i < this.selectNodes.length; i++) {
                var curNode = this.selectNodes[i];
                var curNodeDom = document.getElementById(curNode.id);
                if (curNodeDom) {
                    leftPositionArr.push(parseInt(curNodeDom.style.left.replace("px", "")));
                    doms.push(curNodeDom);
                }
            }
            var positionLeft = Math.min.apply(null, leftPositionArr);
            var positionRight = Math.max.apply(null, leftPositionArr);
            //计算间距应该要多少
            var spacing = (positionRight - positionLeft) / (this.selectNodes.length - 1);

            //从左到右边排序
            doms.sort((a, b) => { return parseInt(a.style.left.replace("px", "")) - parseInt(b.style.left.replace("px", "")) });
            for (var i = 0; i < doms.length; i++) {
                doms[i].style.left = (positionLeft + spacing * i) + "px";
            }
            window.jsp.repaintEverything();
        }


    },

    /**
     * 纵向分布
     * */
    verticalDistribution: function () {
        if (this.selectNodes.length >= 3) {
            var topPositionArr = [];
            var doms = [];
            for (var i = 0; i < this.selectNodes.length; i++) {
                var curNode = this.selectNodes[i];
                var curNodeDom = document.getElementById(curNode.id);
                if (curNodeDom) {
                    topPositionArr.push(parseInt(curNodeDom.style.top.replace("px", "")));
                    doms.push(curNodeDom);
                }
            }
            var positionButtom = Math.min.apply(null, topPositionArr);
            var positionTop = Math.max.apply(null, topPositionArr);
            //计算间距应该要多少
            var spacing = (positionTop - positionButtom) / (this.selectNodes.length - 1);

            //从左到右边排序
            doms.sort((a, b) => { return parseInt(a.style.top.replace("px", "")) - parseInt(b.style.top.replace("px", "")) });
            for (var i = 0; i < doms.length; i++) {
                doms[i].style.top = (positionButtom + spacing * i) + "px";
            }
            window.jsp.repaintEverything();
        }
    },


    /**
     * 将上级的命令传入过来
     * */
    setRoutingDesignControrl: function (_routingDesignControrl) {
        this.routingDesignControrl = _routingDesignControrl;
    },
    /**
     * 设置布均按钮状态
     * */
    setLayoutCommandsDisabled: function (selectedLength) {

        if (this.routingDesignControrl) {
            this.routingDesignControrl.setMainBlockCommandDisabled('LeftRightCommand', selectedLength < 2);
            this.routingDesignControrl.setMainBlockCommandDisabled('UpDownCommand', selectedLength < 2);
            this.routingDesignControrl.setMainBlockCommandDisabled('VerticalDistributionCommand', selectedLength < 3);
            this.routingDesignControrl.setMainBlockCommandDisabled('HorizontalDistributionCommand', selectedLength < 3);
        }
    },

    /******************************以下工艺路线验证**********************************/

    /**
     * 验证工艺路线设计
     * @method validateDesignData
     * @for DesignCanvas
     * @returns {Boolean} 验证通过返回true，不通过返回false，内部直接弹框提示错误信息
     */
    validateDesignData: function validateDesignData() {
        //验证成功返回true，失败返回false
        try {
            var me = this;
            var design = me.getDesignData();
            me.validateNodes(design);
            return true;
        } catch (e) {
            SIE.Msg.showError(e.message);
            return false;
        }
    },

    /**
     * 验证工艺路线
     * @method validateNodes
     * @for DesignCanvas
     * @param {Object} design 设计数据对象
     */
    validateNodes: function validateNodes(design) {
        var me = this;
        var nodes = design.nodes;
        var lines = design.lines;
        //验证工艺路线
        me.validateRouting(nodes);
        //验证通用
        me.validateProcess(nodes);
        //验证扩展属性
        SIE.Web.Tech.Common.Routings.PropertyExt.properties.forEach(function (property) {
            property.validate(me, nodes, lines);
        });
        //验证扩展子页签 
        SIE.Web.Tech.Common.Routings.RoutingProcessChildExtension.controls.forEach(function (ctl) {
            ctl.validate(me, nodes, lines);
        });
        nodes.forEach(function (node) {
            //验证工序规则
            me.validateRule(lines, node);
        });
    },

    /**
     * 验证工艺路线
     * @method validateRouting
     * @for DesignCanvas
     * @param {Array} nodes 节点信息
     */
    validateRouting: function validateRouting(nodes) {
        if (!nodes.any(function (p) {
            return p.nodeType === 'BeginNode';
        })) throw new Error('未找到开始工序'.L10N());
        if (!nodes.any(function (p) {
            return p.nodeType === 'EndNode';
        })) throw new Error('未找到结束工序'.L10N());
        if (!nodes.any(function (p) {
            return p.nodeType === 'RoutingNode';
        })) throw new Error('至少要有一个活动工序'.L10N()); //判断是否重复工序 
        //var groupNodes = nodes.where(function (p) {
        //    return p.nodeType === 'RoutingNode';
        //}).groupBy(function (p) {
        //    return p.designerData.ProcessId;
        //});
        //groupNodes.forEach(function (groupNode) {
        //    if (groupNode.length > 1) {
        //        var processName = groupNode[0].designerData.Text;
        //        throw new Error(Ext.String.format('存在{0}个工序[{1}]'.L10N(), groupNode.length, processName));
        //    }
        //});
    },

    /**
     * 验证工序信息
     * @method validateProcess
     * @for DesignCanvas
     * @param {Array} nodes 节点信息
     */
    validateProcess: function validateProcess(nodes) {
        var me = this;
        var routingNodes = nodes.where(function (p) {
            return p.nodeType === 'RoutingNode';
        });
        if (routingNodes.length === 0) return;
        var isBatch = false;

        for (var i = 0; i < routingNodes.length; i++) {
            var node = routingNodes[i];
            if (node.designerData.NodeType == "RoutingGroupNode") {

                for (var j = 0; j < node.groupDesignerData.length; j++) {
                    var designerDataNode = node.groupDesignerData[j].designerData;
                    if (j === 0 && isBatch == false) {
                        isBatch = me.isBatch(designerDataNode.ProcessType);
                    } else {
                        if (me.isBatch(designerDataNode.ProcessType) !== isBatch) throw new Error('批次工序和非批次工序不能混合'.L10N());
                    }
                }
            } else {
                //单个工序
                if (i === 0 && isBatch == false) {
                    isBatch = me.isBatch(node.designerData.ProcessType);
                }
                else {
                    if (me.isBatch(node.designerData.ProcessType) !== isBatch) throw new Error('批次工序和非批次工序不能混合'.L10N());
                }
            }
        }
    },

    /**
     * 判断工序是否批次工序
     * @method isBatch
     * @for DesignCanvas
     * @param {ProcessType} processType 工序类型
     * @returns {Boolean} 批次工序返回true，非批次返回false
     */
    isBatch: function isBatch(processType) {
        return processType === 'BatchAssembly' || processType === 'BatchPqc' || processType === 'BatchFix' || processType === 'BatchPacking';
    },

    /**
     * 验证工序参数
     * @method validateRule
     * @for DesignCanvas
     * @param {Array} lines 背景色
     * @param {Object} node 节点信息
     */
    validateRule: function validateRule(lines, node) {
        //验证时，排除可选连线
        var nodeLinesLength = node.designerData.SourceAnchor.where(function (p) { return p.AnchorType !== 'Optional'; }).length;
        var connectLines = lines.where(function (p) {
            return p.sourceNode === node.id && p.linedata.ParamResultType !== 'Optional';
        });
        var connectedLength = connectLines.length;

        if (node.nodeType === 'BeginNode') {
            if (connectedLength < 1)
                throw new Error('开始节点未连线'.L10N());
        } else if (node.nodeType === 'EndNode') {
            if (lines.where(function (p) {
                return p.targetNode === node.id;
            }).length === 0) throw new Error('结束节点未连线'.L10N());
        } else {
            if (nodeLinesLength !== connectedLength && nodeLinesLength !== 0)
                throw new Error(Ext.String.format('工序[{0}]存在{1}个参数未进行连线'.L10N(), node.designerData.Text, nodeLinesLength - connectedLength));
            if (lines.where(function (p) {
                return p.targetNode === node.id;
            }).length === 0) throw new Error(Ext.String.format('工序[{0}]没有工序连接'.L10N(), node.designerData.Text)); //验证是否存在多个分支指向相同下一工序

            if (!node.ParentNodeId) {
                var groupLine = connectLines.groupBy(function (p) {
                    return p.targetNode;
                });
                if (groupLine.length !== connectedLength)
                    throw new Error(Ext.String.format('工序[{0}]不能存在多个分支指向下一工序'.L10N(), node.designerData.Text));
            }
        }
    },

    /**
     * 比较数组对象属性
     * @method compareArrary
     * @for DesignCanvas
     * @param {Param} prop 参数
     */
    compareArrary: function (prop) {
        return function (obj1, obj2) {
            var val1 = obj1[prop];
            var val2 = obj2[prop];
            if (!isNaN(Number(val1)) && !isNaN(Number(val2))) {
                val1 = Number(val1);
                val2 = Number(val2);
            }
            if (val1 < val2) {
                return -1;
            } else if (val1 > val2) {
                return 1;
            } else {
                return 0;
            }
        }
    },

    /**
     * 获取当前工序节点的前面工序节点信息
     * @method compareArrary
     * @for DesignCanvas
     * @param {Param} node 当前工序节点
     */
    getPreProcessInfo: function (node) {
        var me = this;
        var designerData = me.getDesignData();
        var nodes = designerData.nodes.where(function (p) { return p.nodeType === 'RoutingNode' && p.designerData.Index <= node.designerData.Index && p.designerData.ProcessId !== node.designerData.ProcessId });
        var datas = nodes.select(function (item) { return { Id: item.designerData.ProcessId, Name: item.designerData.Text } });
        return datas;
    }
});