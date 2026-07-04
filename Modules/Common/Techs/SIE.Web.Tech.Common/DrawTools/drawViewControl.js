"use strict";

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

/**
 * 画图视图控件（仅显示画图部分）
 */
Ext.define('SIE.control.DrawViewControl', {
    extend: 'Ext.Component',
    alias: ['widget.drawViewControl'],
    xtype: 'drawViewControl',
    //画布大小
    width: '100%',
    height: '1000px',
    //画布
    canvas: null,
    //节点标识
    nodeTag: null,
    //设计信息
    design: null,
    //画图工具
    drawTools: null,
    //节点列表
    nodeList: [],
    //画图视图控件
    drawViewControl: null,
    //xml属性前缀,默认为_
    attributePrefix: '_',
    //是否需要进行xml格式转换，默认为false
    isXmlData: false,
    //设计图相关参数配置
    designConfig: {},
    start: '开始'.t(),
    end: '结束'.t(),
    /**
     * 设计图相关默认参数配置
     */
    defautDesign: {
        //节点是否可拖动
        canDrag: true,
        //默认源锚点
        SourceAnchor: 'SourcePort',
        //默认目标锚点
        TargetAnchor: 'TargetPort'
    },
    constructor: function constructor(config) {
        var me = this;
        me.callParent(arguments);
        me.initControl(config);
        me.draw();
    },

    /**
     * 初始化
     * @param {} config 
     */
    initControl: function initControl(config) {
        var me = this;

        if (config) {
            me.setCanvas(config);
            me.nodeTag = config.nodeTag;
            me.design = config.design;
            me.isXmlData = config.isXmlData;
            drawBase.apply(me.designConfig, me.defautDesign, config.designConfig);
        } //画图工具


        var drawConfig = me.designConfig;
        drawConfig.listeners = config.listeners;
        me.drawTools = new DrawingTools(null, drawConfig);
        me.customNode(); //me.dragNode(me.nodeTag);
    },
    setCanvas: function setCanvas(config) {
        var me = this;

        if (typeof config.canvas == 'string') {
            me.canvas = document.getElementById(config.canvas);
        } else {
            me.canvas = config.canvas;
        }
    },
    //自定义节点，子类可重写
    customNode: function customNode() {
        var me = this;

        drawBase.define('BeginNode', 'DesignerNode', {
            tpl: '<div id="startnode" class="node" data-nodeType="BeginNode" style="border:solid 4px #3BBD7E; background-color:#FFFFFF; color:#3B444D; font-size:12px; width:48px; height:48px; border-radius:30px; text-align:center;justify-content:center;align-items:center; display: -webkit-flex;">' + me.start +'</div>',
            nodeType: 'BeginNode'
        });
        drawBase.define('EndNode', 'DesignerNode', {
            tpl: '<div id="endnode" class="node" data-nodeType="EndNode" style="border:solid 4px #F7484D; background-color:#FFFFFF; color:#3B444D; font-size:12px; width:48px; height:48px; border-radius:30px; text-align:center; justify-content:center;align-items:center; display: -webkit-flex;">' + me.end +'</div>',
            nodeType: 'EndNode'
        });

        var beginNode = drawBase.create('BeginNode');
        var endNode = drawBase.create('EndNode');
        this.nodeList.push(beginNode);
        this.nodeList.push(endNode);
    },

    /**
     * 设置拖拽节点
     * @param {节点列表或查询表达式} nodeList 
     */
    dragNode: function dragNode(nodeList) {
        var drawTools = this.drawTools; //查找节点

        var nodes;

        if (typeof nodeList === 'string') {
            nodes = Ext.query(nodeList);
        } else {
            nodes = nodeList;
        }

        nodes.forEach(function (node) {
            drawTools.draggable(node, true);
        });
    },
    clear: function clear() {
        if (!this.drawTools.isReady()) {
            setTimeout(function () {
                this.drawTools.clear();
            }, 1000);
        } else {
            this.drawTools.clear();
        }
    },

    /**
     * 绘制设计图
     * @param {设计图数据} designData 
     */
    draw: function draw(designData, isXmlData) {
        this.isXmlData = isXmlData || this.isXmlData;
        this.checkCanDraw(designData);
    },

    /**
     * 检测是否已加载完毕
     * @param {设计图数据} designData 
     */
    checkCanDraw: function checkCanDraw(designData) {
        var me = this;

        if (!this.drawTools.isReady()) {
            setTimeout(function () {
                me.checkCanDraw(designData);
            }, 200);
        } else {
            me.loadDesign(designData);
        }
    },

    /**
     * 生成设计图
     * @param {json对象格式的设计图数据} designData 
     */
    loadDesign: function loadDesign(designData) {
        //绘制图片前先设置下容器
        this.setContainer();
        var design = designData || this.design;

        if (design) {
            if (this.isXmlData) {
                {
                    design = this.turnToJson(design);
                    //设置画布大小
                    if (design!=null &&  design.flowdata !== undefined) {
                        //兼容旧数据 旧数据宽度1920 宽度1080 旧数据不进行设置
                        if (design.flowdata.Width != "1920" && design.flowdata.Height != "1080") {

                            if (design.flowdata.Width == "0" || design.flowdata.Height == "0") {
                                this.canvas.style.width = "1920";
                                this.canvas.style.width = "1080";
                            }
                            else {
                                this.canvas.style.width = design.flowdata.Width + "%";
                                this.canvas.style.height = design.flowdata.Height + "px";
                            }

                        }
                    }
                    else {
                        this.canvas.style.width = "1920";
                        this.canvas.style.width = "1080";
                    }
                }
            } else if (typeof design == 'string') {
                design = JSON.parse(design);
            }
            this.drawTools.loadDesign(design);
        }
    },

    /**
     * 设置容器
     */
    setContainer: function setContainer() {
        this.drawTools.setContainer(this.canvas.id);
        this.canvas.style.width = this.width;
        this.canvas.style.height = this.height;
        this.drawTools.draggable(this.canvas);
    },
    //获取设计图
    turnToJson: function turnToJson(design) {
        if (design) {
            if (this.isXmlData) {
                design = this.xml2Json(design);
            }

            design = this.changeDesign(design);
        }

        return design;
    },

    /**
     * xml转json
     * @param {xml格式字符串} xml 
     */
    xml2Json: function xml2Json(xml) {
        var x2js = new X2JS({
            attributePrefix: this.attributePrefix
        });
        return x2js.xml_str2json(xml);
    },

    /**
     * 转换设计图，子类重写
     * @param {*} design 
     */
    changeDesign: function changeDesign(design) {
        if (!design || _typeof(design) != "object") return design; //节点处理

        var nodes = [];

        for (var i = 0; i < design.nodes.length; i++) {
            var activity = design.nodes[i];
            var node = drawBase.create(activity.nodeType);
            drawBase.apply(node, activity);
            nodes.push(node);
        } //关系处理


        var json = {};
        json.nodes = nodes;
        json.lines = design.lines;
        return json;
    },
    //生成json设计数据
    generalDesign: function generalDesign() {
        var design = this.drawTools.getDesignJson();

        if (design) {
            Ext.Array.forEach(design.nodes, function (node) {
                delete node.element;
            });
        }

        return design;
    },
    //json转换xml前的处理，子类重写
    beforeGetXml: function beforeGetXml(design) {
        return design;
    },
    //获取xml格式
    getXml: function getXml() {
        var design = this.generalDesign();
        if (!design) return design;
        var changed = this.beforeGetXml(design);
        var x2js = new X2JS();
        var xml = x2js.json2xml_str(changed);
        xml = this.afterGetXml(xml, design);
        return xml;
    },

    /**
     * 获取xml后处理
     * 子类重写
     * @param {string} xml 
     */
    afterGetXml: function afterGetXml(xml, json) {
        return xml;
    }
});