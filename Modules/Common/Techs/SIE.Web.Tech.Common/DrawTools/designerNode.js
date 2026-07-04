"use strict";

//活动节点对像
DesignerNode = function DesignerNode(initConfig) {
    var defaultConfig = {
        width: 80,
        height: 80
    }; //初始化

    var init = function init() {
        if (initConfig && typeof initConfig == "string") {
            initConfig = JSON.parse(initConfig);
        }

        var me = this;

        if (initConfig && initConfig.nodeType) {
            me.designerData = new DesignerData(initConfig.nodeType);
        }

        drawBase.apply(me, initConfig, defaultConfig);
    };

    var bindData = function bindData(data) { };

    init.apply(this); //模板,html标签模板

    this.tpl = this.tpl || {}; //doc 元素

    this.element = this.element || this.tpl; //数据

    this.designerData = this.designerData || {}; //装饰

    this.groupDesignerData = this.groupDesignerData || []; //工序组数据

    this.designerDecorater = this.designerDecorater || {}; //事件监听

    this.listeners = this.listeners || [];
};

DesignerNode.CreateNode = function (nodeType) {
    if (nodeType && typeof nodeType == "string") {
        // if (typeof (eval(nodeType)) == "function") {
        //     eval(nodeType + "();");
        // }
        return drawBase.create(nodeType);
    }
};

DesignerNode.extend = function (nodeType) {
    if (nodeType && typeof nodeType == "string") {
        if (typeof eval(nodeType) == "function") {
            eval(nodeType + "();");
        }
    }
};