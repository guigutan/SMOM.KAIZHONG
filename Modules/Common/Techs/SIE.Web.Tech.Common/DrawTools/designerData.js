"use strict";

/**
 * 节点数据基类
 * nodeType:节点类型
 * initConfig:配置信息
 */
DesignerData = function DesignerData(nodeType, initConfig) {
    var me = this; //原始值

    var originalValue = this.originalValue = {};

    var init = function init() {
        if (!nodeType || typeof nodeType != 'string') {
            return;
        }

        var defaultConfig = {};

        if (me.NodeType && me.NodeType[nodeType]) {
            defaultConfig = me.NodeType[nodeType];
        }

        drawBase.apply(me, initConfig, defaultConfig); //记录下原始值

        me.originalValue = drawBase.apply(defaultConfig, initConfig);
        ;
    };

    init();
};
/**
 * 节点数据类型
 * 供具体应用扩展
 */


DesignerData.prototype.NodeType = {};