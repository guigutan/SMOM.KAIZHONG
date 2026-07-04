"use strict";

/**
 * 设计器元素装饰
 */
DesignerDecorater = function DesignerDecorater(nodeType, initConfig) {
    var me = this;

    var init = function init() {
        if (!nodeType || typeof nodeType != 'string') {
            return;
        }

        var defaultConfig = {};

        if (me.NodeType && me.NodeType[nodeType]) {
            defaultConfig = me.NodeType[nodeType];
        }

        drawBase.apply(me, initConfig, defaultConfig);
    };

    init();
};
/**
 * 设计器元素装饰元素类型(可分别定义装饰设计器画布、节点、锚点、线条)
 * 供具体应用扩展
 */


DesignerDecorater.prototype.PartType = {};
/**
 * 流程装饰
 */

DesignerDecorater.prototype.PartType.Flow = {};
/**
 * 画布装饰
 */

DesignerDecorater.prototype.PartType.Canvas = {};
/**
 * 节点装饰
 */

DesignerDecorater.prototype.PartType.Node = {};
/**
 * 锚点装饰
 */

DesignerDecorater.prototype.PartType.Anchor = {};
/**
 * 线条装饰
 */

DesignerDecorater.prototype.PartType.Line = {};