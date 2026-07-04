"use strict";

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

//基础扩展
var drawBase = drawBase || {};

(function () {
    //根据类查找元素
    drawBase.getByClass = function (oParent, sClass) {
        var aResult = [];
        var tagList = ['DIV', 'SPAN'];

        for (var index = 0; index < tagList.length; index++) {
            var tag = tagList[index];
            var aEle = oParent.getElementsByTagName(tag);

            for (var i = 0; i < aEle.length; i++) {
                var className = aEle[i].className;

                if (className && className.indexOf(sClass) > -1) {
                    aResult.push(aEle[i]);
                }
            }
        }

        return aResult;
    }; //合并配置信息


    drawBase.apply = function (object, config, defaults) {
        if (object) {
            if (defaults) {
                this.apply(object, defaults);
            }

            if (config && _typeof(config) === 'object') {
                var i, j, k;

                for (i in config) {
                    if (config.hasOwnProperty(i)) {
                        object[i] = config[i];
                    }
                }
            }
        }

        return object;
    };
    /*
    * 声明自定义类
    * className:必填，字符串格式类名；
    * extendClass:必填，扩展类(目前只提供设计节点及设计节点数据)
    */


    drawBase.define = function (className, extendClass, config) {
        if (!className && typeof className != "string") throw new Error("类名不能为空".t());
        if (!extendClass && typeof extendClass != "string") throw new Error("扩展类不能为空".t());
        if (!this.ClassManager) this.ClassManager = {};
        var classManager = this.ClassManager;

        if (!classManager.hasOwnProperty(className)) {
            if (config && _typeof(config) == "object") {
                config = JSON.stringify(config);
            }

            if (typeof extendClass == "string" && typeof eval(extendClass) == "function") {
                extendClass = eval(extendClass);
            } else if (typeof extendClass != "function") {
                extendClass = null;
            }

            if (extendClass) {
                classManager[className] = function () {
                    extendClass.call(this, config);
                }; //classManager[className].prototype = new extendClass(config);
                //classManager[className].base = extendClass;

            }
        }
    };
    /*
    * 创建类对象
    * className:必填，字符串格式类名；
    * config:配置信息
    */


    drawBase.create = function (className, config) {
        if (!className && typeof className != "string") throw new Error("类名不能为空".t());
        if (!this.ClassManager) this.ClassManager = {};
        var classManager = this.ClassManager;

        if (classManager.hasOwnProperty(className)) {
            var obj = new classManager[className](); //obj = Ext.clone(obj);

            this.apply(obj, config);
            return obj;
        }
    };
})();