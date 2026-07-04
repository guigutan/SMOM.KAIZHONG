/**
 * 全局配置Js
 */
var GlobalConfig = (function () {

    //基础表格配置
    var baseGridConfig = {
        bodyBorder: true,
        headerBorders: true
    }

    return {

        //日期时间格式
        dateTimeFormat:'Y-m-d H:i:s',

        //默认表格配置
        defaultGridConfig: {
            minHeight: 50,
            height: '100%',
            headerBorders: baseGridConfig.headerBorders,
            bodyBorder: baseGridConfig.bodyBorder,
            columnLines: true,
            layout: 'fit',
            multiColumnSort: true,
            selModel: {
                type: 'spreadsheet',
                mode: 'MULTI'
            }
        },

        //默认树表格配置
        defaultTreeGridConfig: {
            rootVisible: false,
            minHeight: 100,
            headerBorders: baseGridConfig.headerBorders,
            bodyBorder: baseGridConfig.bodyBorder
        },

        //默认表格配置
        defaultEditFormConfig: {
            //border: 0,
            bodyBorder: true,
            autoScroll: true
        },

        //默认分页条配置
        defaultPagingBarConfig: {
            xtype: 'gridpager',
            displayInfo: true,
            dock: 'bottom',
            cls: 'pagingBarCls',
            border: 1
        },

        //默认详细表单配置
        defaultDetailFormConfig: {
            cls: "detail-form-cls",
            border: 1,
            autoScroll: true
        },

        defaultToolBarConfig: {
            xtype: 'toolbar',
            //style:'border-top-width: 0',
            //border: 0,
            enableOverflow: true,
            dock: 'top',
            overflowHandler: 'scroller'
        },

        //默认查询面板配置
        defaultConditionPanelConfig: {
            //border:0,
            //bodyBorder:false,
            region: 'west',
            width: 285,
            cls: 'custom-condition-cls', //用于样式修改
            split: true,
            scrollable: false,
            collapsible: true
            //bodyCls: 'conditionPanelBodyCls'
        },

        AjaxTimeout:1800000,

        themeConfig: [
            // {
            //     text: '科技黑',
            //     id: 'default',
            //     extThemeUrl: '/Extjs/Themes/ScientificDark/resources/scientific_dark-all.css',
            //     siteThemeUrl: '/Content/Themes/theme-science.css'
            // },
            // {
            //     text: '海洋蓝',
            //     id: 'theme1',
            //     extThemeUrl: '/Extjs/Themes/FlatBlue/resources/flat-blue-all.css',
            //     siteThemeUrl: '/Content/Themes/theme-blue.css'
            // }
            // , {
            //     text: '经典灰',
            //     id: 'theme2',
            //     extThemeUrl: '/Extjs/Themes/Gray/resources/theme-gray-all.css',
            //     siteThemeUrl: '/Content/Themes/theme-gray.css'
            // },
            {
                text: '海王星'.t(),
                id: 'theme3',
                extThemeUrl: '/Extjs/Themes/Neptune/resources/theme-neptune-all.css',
                siteThemeUrl: '/Content/Themes/theme-neptune.css'
            }
        ],
         //列表悬浮框样式设置
         defaultToolTipCfg:{
           trackMouse: true,
           style :{
              backgroundColor:'white'//背景色
          },
          ancor :'top',
         // dismissDelay : 0,//禁用自动隐藏
          listeners:{}//监听事件
         }

    }

})();
//Ext.Loader.setPath('SIE', '.');
Ext.Loader.setConfig({
    enabled: true,
    disableCaching: true,
    paths: {
        'SIE': 'siejs'
    }
});
Ext.define('SIE', {
    singleton: true,
    //internal const
    _IdPropertyName: '_id',
    _KeyPropertyName: 'Id',
    _TreePIdPropertyName: 'TreePId',
    _plugins: [],
    //internal
    _isDebugging: true,
    viewMeta: {
        //js enum mapping SIE.Web.ClientMetaModel.EditMode 
        editMode: {
            INLINE: 'Inline',
            FORM: 'Form'
        }
    },
    htmlTagsPatt: new RegExp('<[^>]+>', 'gi'),

    //--------------------------------------  异常 -------------------------------------
    error: function (name) {
        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="name" type="type"></param>
        name = name || "";
        alert(name);
        throw new Error(name);
    },

    notSupport: function (name) {
        name = name || "";
        var funcName = arguments.callee.caller.$name;
        this.error(funcName + "方法暂时不被支持：".t() + name);
    },
    notImplement: function (name) {
        name = name || "";
        var funcName = arguments.callee.caller.$name;
        this.error(funcName + "方法没有被实现!".t() + name);
    },
    markAbstract: function (name) {
        this.notImplement(name);
    },
    emptyArgument: function (name) {
        var funcName = arguments.callee.caller.$name;
        this.error(Ext.String.format("{0}方法中的参数：{1}不能为空。".L10N(),funcName,name));
    },

    //--------------------------------------  Ajax -------------------------------------
    /**
     * 封装ajax，还是调用Ext.Ajax，增加session过期拦截
     * @param {Object} op -sample:{url:{url},data:{data},success:fn,failure:fn} 
     * @returns {} 
     */
    Ajax: function (op) {
        var me = this;
        try {
            if (!SIE.App.checkCurUser()) {
                return;
            }
            op.timeout = op.timeout || GlobalConfig.AjaxTimeout;
            var oldFialure = op.failure;
            var failure = function (res, opts) {
                if (res.status === 401 || res.status === 302) {
                    me.ajaxHandler(res.status, JSON.parse(res.responseText).Message || '账号异常')
                }
                else {
                    if (oldFialure && typeof oldFialure == 'function') {
                        oldFialure(res, opts);
                    }
                }
            }
            op.failure = failure;
            Ext.Ajax.request(op);
        } catch (e) {
            SIE.Msg.showError(e.message);
        }
    },

    /**
     * ajaxHandler
     * @param {*} status 
     * @param {*} msg 
     */
    ajaxHandler: function (status, msg) {
        if (status === 401) {
            SIE.App.popupLogin(function () {

            });
        } else if (status === 302) {
            var title = '提示'.t(), okText = '确定'.t();
            Ext.MessageBox.show({
                title: title,
                msg: msg,
                width: 300,
                buttons: Ext.MessageBox.OK,
                buttonText: { ok: okText },
                scope: this,
                fn: function () {
                    location.replace('/');
                }
            });
        }
    },

    //--------------------------------------  数组扩展方法 -------------------------------------
    each: function (array, fn) {
        /// <summary>
        /// 遍历数组的每一个元素，应用指定的方法
        /// </summary>
        /// <param name="array">需要遍历的数组、或者 Store、TreeStore、MixedCollection 对象</param>
        /// <param name="fn">
        /// 一个回调函数，参数是每一个元素，如果该方法返回 false，则终止整个循环。
        /// </param>
        /// <returns>如果在某个元素时终止了整个循环，则返回这个元素，否则返回 null。</returns>
        if (array) {
            //树型数据集，使用深度递归遍历
            if (array instanceof Ext.data.TreeStore) {
                //跳过 root，直接遍历第一层子结点。
                var root = array.getRootNode();
                for (var i = 0; i < root.childNodes.length; i++) {
                    var stopped = this._eachInTreeNode(root.childNodes[i], fn);
                    if (stopped) return stopped;
                }
            } else if (array.isStore || array instanceof Ext.util.AbstractMixedCollection) {
                var c = array.getCount();
                for (var i = 0; i < c; i++) {
                    var item = array.getAt(i);
                    var res = fn(item);
                    if (res === false) return item;
                }
            } else {
                for (var i = 0; i < array.length; i++) {
                    var item = array[i];
                    var res = fn(item);
                    if (res === false) return item;
                }
            }
        }
        return null;
    },
    _eachInTreeNode: function (node, fn) {
        /// <summary>
        /// 深度递归遍历树型数据集
        /// </summary>
        /// <param name="store"></param>
        /// <param name="fn"></param>

        var res = fn(node);
        if (res === false) return node;

        //递归遍历结点的子结点。
        if (!node.isLeaf()) {
            for (var j = 0; j < node.childNodes.length; j++) {
                var stopped = this._eachInTreeNode(node.childNodes[j], fn);
                if (stopped) return stopped;
            }
        }

        return null;
    },
    findFirst: function (array, filter) {
        /// <summary>
        /// 遍历数组的每一个元素，找到指定的项
        /// </summary>
        /// <param name="array">array,Store,MixedCollection</param>
        /// <param name="filter">bool function(item)，返回真表示找到想要的结果。</param>
        /// <returns></returns>
        return this.each(array, function (i) {
            if (filter(i)) {
                return false;
            }
        });
    },
    first: function (array, filter) {
        /// <summary>
        /// 遍历数组的每一个元素，找到指定的项。
        /// 如果没有找到，则抛出异常。
        /// </summary>
        /// <param name="array">array,Store,MixedCollection</param>
        /// <param name="filter">bool function(item)</param>
        /// <returns></returns>
        var r = this.findFirst(array, filter);
        if (r == null) throw new Error("没有找到对应的元素。".t());
        return r;
    },
    select: function (array, selector) {
        /// <summary>
        /// 遍历数组的每一个元素，应用指定的方法来生成新的数组
        /// </summary>
        /// <param name="array">需要遍历的数组、或者 Store、MixedCollection 对象</param>
        /// <param name="selector">
        /// 一个回调函数，参数是每一个元素，返回选择的结果。
        /// </param>
        /// <returns>新的数组</returns>

        var res = [];

        this.each(array, function (item) {
            res.push(selector(item));
        });

        return res;
    },
    sum: function (array, numSelector) {
        /// <summary>
        /// 遍历数组的每一个元素，应用指定的方法来生成新的数组
        /// </summary>
        /// <param name="array">需要遍历的数组、或者 Store、MixedCollection 对象</param>
        /// <param name="selector">
        /// 一个回调函数，参数是每一个元素，返回 number 类型的数据。
        /// </param>
        /// <returns>最终的和</returns>
        var sum = Ext.Array.sum(SIE.select(array, numSelector));
        return sum;
    },

    isEmptyObject: function (o) {
        /// <summary>
        /// 是否不带任何属性的空对象 {}
        /// </summary>
        /// <param name="e" type="object"></param>
        /// <returns type=""></returns>
        var t;
        for (t in o)
            return !1;
        return !0
    },

    //-------------------------------------  插件 -------------------------------------
    definePlugin: function (name, data) {
        /// <summary>
        /// 定义一个插件类
        /// </summary>
        /// <param name="name">插件类型的名称</param>
        /// <param name="data">类型相关定义</param>
        var plugin = Ext.define(name, data);
        this._plugins.push(plugin);
    },
    _getPlugins: function () {
        /// <summary>
        /// internal
        /// 获取所有的插件的集合。
        /// </summary>
        /// <returns type=""></returns>
        return this._plugins;
    },

    isDebugging: function () {
        return this._isDebugging;
    },

    //-------------------------------------  实体定义 -------------------------------------
    _dm: function (model, entityConfig) {
        /// <summary>
        /// internal
        /// define model
        /// 定义一个实体类型。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entityConfig"></param>
        Ext.apply(entityConfig, {
            extend: "SIE.data.Entity"
        });

        //为所有字段生成 get、set 方法。
        this.each(entityConfig.fields, function (f) {
            var name = f.name;
            entityConfig['set' + name] = function (value) {
                this.set(name, value);
            };
            entityConfig['get' + name] = function () {
                return this.get(name);
            }
        });
        var isCreated = Ext.ClassManager.isCreated(model);
        if (!isCreated) { //加入检测，防错
            Ext.define(model, entityConfig, function () {
                //所有的实体类型上，都添加一个 isTree 属性，用于判断实体类是否为树型实体。
                this.isTree = !!entityConfig.isTree;
            });
        }
    },
    getModel: function (model) {
        /// <summary>
        /// 获取指定的实体类型。
        /// </summary>
        /// <param name="model">实体类型名称或者实体类型本身。</param>
        /// <returns type=""></returns>
        //return Ext.ModelMgr.getModel(model); //5.0之前使用
        if (model instanceof Function) {
            return model;
        } else {
            var isCreated = Ext.ClassManager.isCreated(model);
            if (!isCreated)
                SIE.tryLoadClass(model);
            return Ext.ClassManager.get(model);
        }
    },
    getModelName: function (model) {
        /// <summary>
        /// 获取指定实体模型的类型名称。
        /// </summary>
        /// <param name="model"></param>
        /// <returns type=""></returns>
        if (Ext.isString(model)) {
            return model;
        }
        return Ext.getClassName(model);
    },

    //-------------------------------------  命令定义 -------------------------------------
    _commands: [],
    defineCommand: function (cmdName, members) {
        /// <summary>
        /// 定义一个命令类型。
        /// </summary>
        /// <param name="cmdName">命令的名称。</param>
        /// <param name="members">
        /// meta: 按钮的元数据。目前主要用于配置界面上的按钮。
        /// </param>

        var o = SIE.cmd.CommandManager;
        return o.defineCommand.apply(o, arguments);
    },
    invokeCommand: function (op) {
        /// <summary>
        /// 调用指定命令
        /// </summary>
        /// <param name="op">
        /// cmd: 必选，字符串表示的命令名称。
        /// cmdInput: 可选，输入参数对应的 json 对象。
        /// callback: 可选，回调。
        /// async: true。
        /// </param>
        var o = SIE.cmd.CommandController;
        return o.excute.apply(o, arguments);
    },
    invokeDataQuery: function (op) {
        /// <summary>
        /// 调用通用查询
        /// </summary>
        /// <param name="op" type="type">
        /// type: 必选，查询器名称。
        /// method: 必选，查询方法名称。
        /// params: 可选, 参数数组。
        /// token: 可选，令牌。
        /// callback: 可选，回调。
        /// async: true。
        /// </param>                
        var o = SIE.data.DataQueryer;
        return o.query.apply(o, arguments);
    },

    //------------------------------------ viewModel --------------------------------------
    getVmData: function (vm) {
        var result = null;
        if (vm) {
            result = vm.get('p');
        }
        return result;
    },
    setVmData: function (vm, data) {
        vm.set('p', data);
    },

    //------------------------------------ 通用 --------------------------------------
    hasAnyProperty: function (obj) {
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop)) {
                return true;
            }
        }
        return false;
    },
    /**
     * 如果传递的值为空，则返回true，否则返回false。 如果是，则该值被视为空
     *
     * - `null`
     * - `undefined`
     * - a zero-length array
     * - a zero-length string (除非`allowEmptyString`参数设置为`true`)
     *
     * @param {Object} value-要测试的值
     * @param {Boolean} [allowEmptyString=false] `true` to allow empty strings.
     * @return {Boolean}
     */
    isEmpty: function (value, allowEmptyString) {
        return Ext.isEmpty(value, allowEmptyString);
    },
    /**
     * 如果传递的值是JavaScript数组则返回“true”，否则返回“false”。
     *
     * @param {Object} target The target to test.
     * @return {Boolean}
     * @method
     */
    isArray: function (value) {
        return Ext.isArray(value);
    },
    /**
     * 如果传递的值是JavaScript Date对象，则返回“true”，否则返回“false”。
     * @param {Object} obj The object to test.
     * @return {Boolean}
     */
    isDate: function (obj) {
        return Ext.isDate(obj);
    },
    /**
      * 如果传递的值是与MS Date JSON匹配的String，则返回'true'
      * encoding format.
      * @param {String} value The string to test.
      * @return {Boolean}
      */
    isMSDate: function (value) {
        return Ext.isMSDate(value);
    },
    /**
      * 如果传递的值是JavaScript对象，则返回“true”，否则返回“false”。
      * @param {Object} value The value to test.
      * @return {Boolean}
      * @method
      */
    isObject: function (value) {
        // check ownerDocument here as well to exclude DOM nodes 
        return Ext.isObject(value);
    },
    /**
     * 如果传递的值是JavaScript函数则返回“true”，否则返回“false”。
     * @param {Object} value The value to test.
     * @return {Boolean}
     * @method
     */
    isFunction: function (value) {
        return Ext.isFunction(value);
    },
    /**
      * 如果传递的值是数字，则返回“true”。 对于非有限数，返回`false`。
      * @param {Object} value The value to test.
      * @return {Boolean}
      */
    isNumber: function (value) {
        return Ext.isNumber(value) || value instanceof Number;
    },
    /**
     * 验证值是否为数字。
     * @param {Object} value Examples: 1, '1', '2.34'
     * @return {Boolean} True if numeric, false otherwise
     */
    isNumeric: function (value) {
        return Ext.isNumeric(value);
    },
    /**
     * 如果传递的值是字符串，则返回“true”。
     * @param {Object} value The value to test.
     * @return {Boolean}
     */
    isString: function (value) {
        return Ext.isString(value);
    },
    /**
     * 如果传递的值是布尔值，则返回“true”。
     *
     * @param {Object} value The value to test.
     * @return {Boolean}
     */
    isBoolean: function (value) {
        return Ext.isBoolean(value);
    },

    /**
     * 不知道对象结构时取值时,一般会采用 obj&&obj[0]&&obj.name的方法,等价于下面的方法
     *  f(obj,'[0].name') === f(obj,['0','name'])
     * @param {取值的对象} obj 
     * @param {用于取值的字符串或者数组} path 
        var testData = { a: [{ c: { b: [233] } }] };
        safeGet(testData,'a[0].c.b[0]') => 233
        safeGet(testData,['a','0','c','b','0']) => 233
        safeGet({a:'a',b:'b',c:'c'}, 'c.toString()','error')
     */
    safeGet: function (obj, path, undefinedReplace) {
        var me = this;
        if (Array.isArray(path)) {
            return path.reduce(function (ob, k) {
                if (typeof k == "string" && k.substr(k.length - 2) == '()') {
                    var fn = k.substr(0, k.length - 2); //仅支持不带参数的成员函数
                    return ob && ob[fn] ? ob[fn]() : undefinedReplace;
                };
                return ob && ob[k] ? ob[k] : undefinedReplace;
            }, obj);
        } else if (typeof path == "string") {
            var arrKeys = path.split("."), keys = [], m;
            arrKeys.forEach(function (k) {
                if (m = k.match(/([^\[\]]+)|(\[\d+\])/g)) {
                    m = m.map(function (v) {
                        return v.replace(/\[(\d+)\]/, "$1");
                    });
                    [].push.apply(keys, m);
                }
            });
            return me.safeGet(obj, keys, undefinedReplace);
        }
    },

    sleep: function (value) {
        for (var t = Date.now(); Date.now() - t <= value;);
    },
    tryCount: 3,
    tryLoadClass: function (className) {
        if (className) {
            //需求： model 为空时,去请求这个模型
            // 可以补充重试机制trycount 3，日志等
            Ext.syncRequire(className);
        }
    },

    /**
     * 是否图片类型-根据文件扩展名
     * @param {string} ext
     */
    isImageExt: function (ext) {
        return ['png', 'jpg', 'jpeg', 'bmp', 'gif', 'webp', 'psd', 'svg', 'tiff'].indexOf(ext.toLowerCase()) !== -1;
    }
});



Ext.define('SIE.Window', {
    singleton: true,
    show: function (opt) {
        
        /// <summary>
        /// 弹出一个窗体。
        /// </summary>
        /// <param name="opt">
        /// buttons: ['确定'.t(),'取消'.t()]，这里定义的按钮列表，也是回调中传出来的按钮名称。
        /// callback: 回调。参数：
        ///     btn：被点击的按钮名称。
        /// 其它：支持所有 window 的配置。
        /// </param>
        /// <returns></returns>
        if (opt.editEntity) {
            opt.listeners =
            {
                beforeclose: function() {
                    var me = this;
                    if (me.buttons != "确定".t()) {
                        if (me.editEntity.modified != null && me.editEntity.previousValues != null) {
                            var updateModified = [];
                            for (var modifiedProperty in me.editEntity.modified) {
                                updateModified.push(modifiedProperty.concat("_Display"));
                            }
                            for (var i = 0; i < updateModified.length; i++) {
                                if (me.editEntity.previousValues[updateModified[i]]) {
                                    me.editEntity.set(updateModified[i], me.editEntity.previousValues[updateModified[i]]);
                                }
                            }
                        }
                        me.editEntity.reject();
                        return true;
                    }
                }
            };
        }

        var me = this;
        Ext.applyIf(opt, {
            bodyPadding: 5,
            layout: 'fit',
            autoScroll: true,
            modal: true
        });

        //转换按钮的配置。
        me._convertButtons(opt);
        opt.constrain = true; //保证整个窗口不会越过浏览器的边界;
        var win = Ext.create('Ext.window.Window', opt).show();
        win.center();
        return win;
    },

    /**
* 弹窗自适应大小
* @param {win} win-win弹窗
* @param {defaultWidth} win-defaultWidth弹窗默认宽
* @param {defaultheight} win-defaultheight弹窗默认高
* @param {isfullscreen} win-isfullscreen弹窗是否最大化
* 用法：SIE.Window.winAutoSize(win,1000,700,false);
*/
    winAutoSize: function (win, defaultWidth, defaultHeight, isfullscreen) {
        var me = this;
        var winId = win.getId();
        //弹窗默认值
        defaultWidth = defaultWidth ? defaultWidth : win.getWidth();
        defaultHeight = defaultHeight ? defaultHeight : win.getHeight();
        me._getWinSize(win, defaultWidth, defaultHeight);
        Ext.EventManager.onWindowResize(function (a, b) {
            var win = Ext.getCmp(winId);
            if (win == undefined) {
                return;
            }
            me._getWinSize(win, defaultWidth, defaultHeight);
        });
    },

    _getWinSize: function (win, defaultWidth, defaultHeight, isfullscreen) {
        if (isfullscreen) {
            win.setPosition(0, 0);

            win.fitContainer(); // 填充满浏览器 
        }
        else {
            var docheght = document.body.clientHeight;
            var docwidth = document.body.clientWidth;
            if (docheght < 650 || docwidth < 1000) {
                win.setWidth(docwidth * 0.7)
                win.setHeight(docheght * 0.88)
                //页面宽高<800/600，铺满全屏
                //win.setPosition(0, 0);
                //win.fitContainer(); // 填充满浏览器   
            }
            else if (docheght >= 650 && docwidth >= 1000) {
                //弹窗默认值
                defaultWidth <= docwidth ? win.setWidth(defaultWidth) : win.setWidth(docwidth * 0.7);
                defaultHeight <= docheght ? win.setHeight(defaultHeight) : win.setHeight(docheght * 0.88);
            }
            win.center();
        }
    },

    _convertButtons: function (opt) {
        var me = this;

        if (!opt.buttons) {
            opt.buttons = ['确定'.t(), '取消'.t()];
        }

        if (Ext.isString(opt.buttons)) {
            opt.buttons = opt.buttons.split(',');
        }

        var btnCfg = [];
        SIE.each(opt.buttons, function (btn) {
            //只转换字符串；对象则不进行转换。
            if (Ext.isString(btn)) {
                var btnText = btn.t();
                btnCfg.push({
                    text: btnText,
                    handler: me._createButtonHandler(btnText, opt.callback)
                });
            }
            else {
                btnCfg.push(btn);
            }
        });

        opt.buttons = btnCfg;
    },
    _createButtonHandler: function (btn, callback) {
        return function () {
            var win;
            if (callback) {
                var handled = callback(btn);
                win = this.up('window');
                if(win) win.buttons=btn;
                if (handled === false) { return; }
            }
            if(win) win.close();
        };
    },

    //对象转url参数
    urlParam: function (param, key, encode) {
        me = this;
        if (param == null) return '';
        var paramStr = '';
        var t = typeof (param);
        if (t == 'string' || t == 'number' || t == 'boolean') {
            paramStr += '&' + key + '=' + ((encode == null || encode) ? encodeURIComponent(param) : param);
        }
        else {
            for (var i in param) {
                //var k = key == null ? i : key + (param instanceof Array ? '[' + i + ']' : '.' + i);
                var k = key == null ? i : key + '.' + i;
                if (param[i] instanceof Array)
                    paramStr += "&" + k + "=" + JSON.stringify(param[i]);
                else
                    paramStr += me.urlParam(param[i], k, encode);
            }
        }
        return paramStr;
    }

});


Ext.define('SIE.meta.MetaService', {
    singleton: true,
    getMeta: function (op) {
        /// <summary>
        /// 获取指定的元数据
        /// </summary>
        /// <param name="op">
        /// module 和 model 必须指定一个。
        ///     module: '', 如果是获取某个模块的元数据，则指定此参数为模块名。
        ///     model: '', 如果获取某个实体的元数据，则这个参数表示实体类名。在实体类模式下，可以选填以下两种方式
        ///         templateType: ''，此参数只在 isAggt 为 true 时有用，表示自定义的聚合块模板类型名称。
        ///         viewName: ''，如果 isAggt 为 true，表示使用的定义的扩展聚合块名称。否则表示扩展视图名称。
        /// isAggt: false
        /// isReadonly: false
        /// ignoreComands: false
        /// isDetail: false
        /// isLookup: false
        /// async: true
        /// callback: 回调，参数如下：
        ///     SIE.Web.ClientMetaModel.ClientAggtMeta
        /// </param>
        var url = this._url(op);
        var isAsync;
        if(typeof(op.async) == "undefined"){
            isAsync =true;
        }else{
            isAsync = op.async;
        }
        var aOp = {
            url: url,
            async: isAsync,
            success: function (response, opts) {
                var res = response.responseJson;
                if (!res && response.responseText)
                    res = Ext.decode(response.responseText);

                if (res.Success) {
                    var meta = res.Result;
                    //meta.model = op.model;
                    if (meta.warnMessage) console.warn(meta.warnMessage);
                    op.callback(meta);
                }
                else {
                    SIE.Msg.showError(res.Message);
                }
            },
            failure: function (response, opts) {}
        };
        SIE.Ajax(aOp);
    },
    _url: function (op) {
        var res = Ext.String.format("/api/MetaModelPortal/GetMeta?type={0}", op.model);
        if (op.module) { res += "&module=" + encodeURIComponent(op.module); }
        if (op.viewName) { res += "&viewName=" + encodeURIComponent(op.viewName); }
        if (op.templateType) { res += "&templateType=" + encodeURIComponent(op.templateType); }
        if (op.viewGroup) { res += "&viewGroup=" + encodeURIComponent(op.viewGroup); }
        if (op.isAggt) { res += "&isAggt=true"; }
        if (op.isReadonly) { res += "&isReadonly=true"; }
        if (op.ignoreCommands) { res += "&ignoreCommands=true"; }
        if (op.ignoreQuery) { res += "&ignoreQuery=true"; }
        if (op.isDetail) { res += "&isDetail=true"; }
        if (op.ignoreChild) { res += "&ignoreChild=true"; }
        if (op.isLookup) { res += "&isLookup=true"; }
        return res;
    }
});
Ext.define('SIE.data.proxy.WebApiProxy',
    {
        extend: 'Ext.data.proxy.Ajax',
        alias: 'proxy.webapi',
        extractResponseData: function(response) {
            var responseObj = response.responseJson;
            if (!responseObj && response.responseText)
                responseObj = Ext.decode(response.responseText);
            if (responseObj) {
                if (responseObj.Success) {
                    return responseObj.Result;
                } else {
                    SIE.Msg.showError(responseObj.Message);
                }
            }
            return response;
        },
        listeners: {
            exception: function(scope, response, operation, eOpts) {
                if (response.status === 401||response.status ===302) {
                    SIE.ajaxHandler(response.status, response.responseJson.Message)
                    return;
                }
                if (response.status === -1) return;
                SIE.Msg.showError("status:" +
                    response.status +
                    ", statusText:" +
                    response.statusText +
                    ',<br/>responseText:' +
                    response.responseText);
            }
        }
    });

/// <reference path="../_reference.js" />
Ext.define('SIE.data.Entity', function () {
    return {
        extend: 'Ext.data.Model',
        schema: {
            namespace: 'SIE.model'
        },
        mixins: [
            'Ext.mixin.Observable'
        ],
        idProperty: SIE._IdPropertyName,
        belongsView: null,
        relations: null,
        propertyChangedEvents:[],
        constructor: function () {
            this.relations = [], //元素结构：{key:relationName,value:relationValue}
            this.callParent(arguments);
            this.mixins.observable.constructor.call(this, arguments);
        },
        proxy: {
            type: 'webapi',
            url: '/api/DataPortal/Query',
            actionMethods: {
                create: 'POST',
                read: 'POST', // by default GET
                update: 'POST',
                destroy: 'POST'
            },
            timeout:GlobalConfig.AjaxTimeout,
            reader: {
                type: 'json',
                rootProperty: 'entities',
                totalProperty: 'totalCount'
            }
        },
        /**
         * 创建实体的ID(发起请求至后台)
         */
        generateId: function () {
            var entityName = this.entityName;
            var me = this;
            SIE.invokeCommand({
                async: false,
                cmd: "SIE.cmd.GenerateIdCommand",
                data: { type: entityName },
                callback: function (res) {
                    if (res.Success)
                        me.setId(res.Result);
                    if (!res.Success) {
                        SIE.Msg.showError(res.Message);
                    }
                }
            });
        },

        privates: {
            /**
             * override 数据是否脏(支持复合实体)
             * @returns {Boolean} true时为脏 
             */
            isDirty: function () {
                return this.isEntityChildrenDirty();
            },
            /**
             * 实体本身是否为脏（不包含复合对象)-推荐使用重写的 isDirty()判定，兼容cs写法
             * @returns {type} true时为脏 
             */
            isSelfDirty: function () {
                /// <summary>
                /// 判断当前对象的某个属性是否已经被修改。
                /// </summary>
                return this.dirty || this.phantom;
            },
            /**
             * 是否新增状态
             * @returns {Boolen} true未提交后台的 
             */
            isNew: function () {
                /// <summary>
                /// 判断整个组合对象是否是刚构造出来的对象。（还没有提交到服务端。）
                /// </summary>
                /// <returns type=""></returns>
                return this.phantom;
            },
            /**
             * 判断整个组合对象中某个部分是否已经被修改。
             * @returns {Boolean} true时为脏 
             */
            isChildrenDirty: function () {
                if (this.isSelfDirty()) return true;

                //如果加载的组合子集合中任意一个被修改，则整个组合对象也是脏的。
                var loadedChildren = this.getLoadedChildren();
                for (var i = 0, len = loadedChildren.length; i < len; i++) {
                    var childrenStore = loadedChildren.getAt(i);
                    if (childrenStore.isDirty()) return true;
                }
                return false;
            },
            /**
             * 获取当前界面的子集合数据
             * @returns {Ext.util.MixedCollection} 数据集合
             */
            getLoadedChildren: function () {
                var me = this;
                var collection = Ext.create('Ext.util.MixedCollection');
                if (me.belongsView) {
                    var childrenArray = me.belongsView.getChildren();
                    if (childrenArray) {
                        for (var i = 0, len = childrenArray.length; i < len; i++) {
                            var child = childrenArray[i];
                            var childrenStore = child.getData();
                            if (childrenStore) {
                                childrenStore.associateView = child;
                                collection.add(child.getAssociateKey(), childrenStore);
                            }
                        }
                    }
                }
                return collection;
            },
            /**
             * 判断实体对应的子列表数据是否为脏的-------是否有问题待验证
             * @returns {Boolean} true为脏
             */
            isEntityChildrenDirty: function () {
                var entity = this;
                if (this.isSelfDirty()) return true;
                var loadedChildren = this.getEntityChildren();
                for (var i = 0, len = loadedChildren.length; i < len; i++) {
                    var childrenStore = loadedChildren.getAt(i);
                    if (childrenStore.isDirty()) return true;
                }
                return false;
            },
            /**
             * 获取实体对应的子列表数据-------是否有问题待验证
             * @returns {Ext.util.MixedCollection} 
             */
            getEntityChildren: function () {
                var entity = this;
                var collection = Ext.create('Ext.util.MixedCollection');
                //根据view配置获取的相关联的
                if (this.belongsView && this.belongsView.getChildren().length > 0) {
                    var childrenArray = this.belongsView.getChildren();
                    for (var i = 0, len = childrenArray.length; i < len; i++) {
                        var childrenStore = null;
                        var child = childrenArray[i];
                        if (!child._childProperty) {
                            var pName = child._associatedProperty + "_" + child._id;
                            if (child.formConfig) {
                                if (entity[pName]) {
                                    childrenStore = entity[pName].getData().items[0];
                                }
                            } else {
                                childrenStore = entity[pName];
                            }
                        } else {
                            var pName = child._childProperty;
                            childrenStore = entity[pName]();
                        }
                        if (childrenStore) {
                            childrenStore.associateView = child;
                            collection.add(child.getAssociateKey(), childrenStore);
                        }
                    }
                }
                //没有view的关联
                if (this.relations && this.relations.length > 0) {
                    for (var i = 0, length = this.relations.length; i < length; i++) {
                        var relation = this.relations[i];
                        if (relation) {
                            if (relation.value.isModel || relation.value.isStore) { //只支持Model和Store
                                collection.add(relation.key, relation.value);
                            }
                        }
                    }
                }

                return collection;
            },
            /**
             * 递归将整个组合对象树都标记为未变更状态。
             */
            markSaved: function () {
                var entity = this;
                if (entity) {
                    if (entity.isModel && entity.isDirty()) {
                        entity.commit();
                        var loadedChildrens = entity.getEntityChildren();
                        for (var i = 0, len = loadedChildrens.length; i < len; i++) {
                            var children = loadedChildrens.getAt(i);
                            if (children.isStore) {
                                children.commitChanges();
                                for (var j = 0, lenj = children.getCount() ; j < lenj; j++) {
                                    var record = children.getAt(j);
                                    if (record) {
                                        if (record.isStore)
                                            record.commitChanges();
                                        record.markSaved();
                                    }
                                }
                            }
                            else if (children.isModel) {
                                children.markSaved();
                            }
                        }
                    }
                }
            },
            /**
             * 数据不保存时，撤销在表单的修改方法重写，extjs的reject只能撤销主表的，从表不能撤销
             * @returns {} 
             */
            rejects:function() {
                var data = this;
                if (data) {
                    if (data.isModel) {
                        for (var previousProperty in data.previousValues) {
                            if (data.modified != null && !data.modified[previousProperty]) {
                                data.data[previousProperty] = data.previousValues[previousProperty];
                            }
                        }
                        data.reject();
                        var loadedChildrens = data.getEntityChildren();
                        for (var i = 0, len = loadedChildrens.length; i < len; i++) {
                            var children = loadedChildrens.getAt(i);
                            if (children.isStore) {
                                var childrenRecords = children.getRejectRecords();
                                for (j = 0; j < childrenRecords.length; j++) {
                                    var childrenData = childrenRecords[j].data;
                                    var childrenModified = childrenRecords[j].modified;
                                    var childrenPrevious = childrenRecords[j].previousValues;
                                    for (var childrenPreviousProperty in childrenPrevious) {
                                        if (childrenModified != null && !childrenModified[childrenPreviousProperty]) {
                                            childrenData[childrenPreviousProperty] =
                                                childrenPrevious[childrenPreviousProperty];
                                        }
                                    }
                                }
                                children.rejectChanges();
                                for (var m = 0; m < children.data.length; m++) {
                                    var grandChildren = children.data.getAt(m).getEntityChildren();
                                    if (grandChildren.items[0] && grandChildren.items[0].isStore) {
                                        grandChildren.items[0].rejectChanges();
                                    } else if (grandChildren.items[0] && grandChildren.items[0].isModel) {
                                        grandChildren.items[0].reject();
                                    }
                                }
                            } else if (children.isModel) {
                                children.rejects();
                            }
                        }
                    }
                }
            }
        },
        addPropertyChanged:function(fn){
              	this.propertyChangedEvents.push(fn);
                this.monPropertyChanged(fn);
         },
        getPropertyChanged:function(){
	        return this.propertyChangedEvents;
        },
        monPropertyChanged:function(fn){
	        this.mon(this,'propertyChanged', fn,this);
        },
        /**
        *  设置某个属性的值
         * (set值跟原值相等时不进行设置)
         * @override es5 属性设置
         * @param {String} property-属性名
         * @param {Object} value-值
         * @returns {type} 
         */
        set: function (property, value) {
                var oldvalue=this.getData()[property];
                var result = this.callParent(arguments);
                // bind entity propertyChanged
                if (arguments.length > 1 || !Ext.isObject(property)) {
                    this._onPropertyChanged({ property: property, value: value,oldvalue: oldvalue, entity: this });
                }
                return result;
        },
        /**
         * @override （未发现引用，所以不知道作用，怕有工程外的地方调用，标记无用）
         * @param {type} fieldName
         * @returns {type} 
         */
        isModified: function (fieldName) {
            if (Ext.String.endsWith(fieldName, "_Display"))
                fieldName = fieldName.replace("_Display", "");
            return this.callParent(arguments);
        },
        /**
         * 属性变更事件
         * @param {Object} e- 修改的实体
         */
        _onPropertyChanged: function (e) {
            if (this && this.store){
                if (!(e.entity.getData()[e.property] === e.oldvalue))
                    this.store.fireEvent('propertyChanged', e);
                this.store.fireEvent('onpropertySet', e);
            }
            if (e) {
                //if (e.entity[e.property] != e.value) { //20190610 导致e.value不为null才执行，没懂这样写的意义，先注释，估计如果是判断实体属性值的比较，实体的属性值在e.entity.data,e.entity只是recred。
                if (!(e.entity.getData()[e.property] === e.oldvalue))                
                    this.fireEvent('propertyChanged', e);
                this.fireEvent('onpropertySet', e);
                //}
            }
        }
    };
});
/// <reference path="../_reference.js" />
/**
* 数据工具类-单例
*/
Ext.define('SIE.data.Utils', function () {
    return {
        singleton: true,
        /**
         * 获取实体可持久化的字段列表
         * @param {SIE.data.Entity} model-实体模型
         * @returns {Ext.data.Field[]} - 字段数组
         */
        getPersistFields: function (model) {
            var res = [];

            var fields;
            //注意：fields 是私有变量
            if (model.isModel) { fields = model.fields; }
            else { fields = SIE.getModel(model).prototype.fields; }

            //属性需要可以保存，并且是小写。
            Ext.Array.each(fields, function (field) {
                if (field.persist) {
                    var name = field.name;
                    var fc = name[0];
                    if (fc >= 'A' && fc <= 'Z') {
                        res.push(field);
                    }
                }
            });

            return res;
        },
        /**
         * 为sotre执行条件过滤根据自定义查询实体
         * @param {data.store} store-数据容器 data.store
         * @param {SIE.data.Entity} criteria- 示例：criteria = Ext.create('SIE.Common.Users.UserCriteria'); 
         */
        filterByCriteria: function (store, criteria) {
            var f = store.getFilters();
            f.clear();
            if (criteria.data && !criteria.NoIgnoreId) {
                delete criteria.data[SIE._IdPropertyName];
            }
            var filters = criteria.data;
            for (pro in filters) {
                //排除_Display字段
                if (Ext.String.endsWith(pro, '_Display'))
                    delete filters[pro];
            }
            f.add([new Ext.util.Filter({
                property: '_useCriteria', value: Ext.getClassName(criteria) || criteria.ClassName
            }), new Ext.util.Filter({
                property: '_criteria', value: criteria.data
            })]);
        },
        /**
         * 为store 设置过滤方法条件。
         * @param {data.store} store-数据容器 data.store
         * @param {String} method- 实体对应服务端EntityRepository的查询方法名
         * @param {Array} params-指定对应方法的参数列表。参数的顺序必须与服务端定义的参数一致。
         */
        filterByMethod: function (store, method, params) {
            if (!params) params = [];
            var f = store.getFilters();
            f.clear();
            //delete criteria.data[SIE._IdPropertyName]; 这个有什么用
            f.add([new Ext.util.Filter({
                property: '_useMethod', value: method
            }), new Ext.util.Filter({
                property: '_params', value: params
            })]);
        },
        /**
         * 根据参数创建一个数据仓库
         * @param {Object} opt-配置的参数
         * model：实体模型类型或名称
         * storeConfig：配置参数
         * @returns {data.store} 
         */
        createStore: function (opt) {
            var model = SIE.getModel(opt.model);
            var storeConfig;
            var remoteSort=opt.remoteSort;
            if (opt.storeConfig && opt.storeConfig.data && Array.isArray(opt.storeConfig.data)) {
                storeConfig = {
                    "fields": opt.storeConfig.fields,
                    "data": opt.storeConfig.data,
                    "pageSize": opt.storeConfig.pageSize,
                    "remoteSort": opt.remoteSort
                };
            } else {
                storeConfig = Ext.apply({
                    model: SIE.getModelName(model),
                   // remoteSort: remoteSort
                }, opt.storeConfig);
                if (model.isTree) {
                    if (!storeConfig.root) {
                        storeConfig.root = {
                            loaded: true,
                            Id: 0.0,
                            leaf: false,
                            text: "root",
                            expanded: true
                        }; //如果没有 root，则会自动发起一次查询。
                    }
                    storeConfig.parentIdProperty = SIE._TreePIdPropertyName;
                    storeConfig.listeners = {
                        load: function (tStore, records, successful, operation, node, eOpts) {
                            //node.expand(true);
                        },
                        nodemove: function (node, oldParent, newParent, index, eOpts) {

                        }
                    };
                }   
                storeConfig.remoteFilter = true;
                //内存排序7
                storeConfig.remoteSort = opt.remoteSort;
                this.gridStoreSort(storeConfig, storeConfig.remoteSort);

                storeConfig.remoteFilter = true;
            }
            var storeName = model.isTree ? 'Ext.data.TreeStore' : 'Ext.data.Store';
            if (model.isTree) {
                storeConfig.defaultRootText = '';
            }
            var store = Ext.create(storeName, storeConfig);

            return store;
        },

        gridStoreSort: function (store, remoteSort) {
            store.sort = function (field, direction, mode) {
                var me = this;
                if (me.data.length == 0) {
                    return;
                }
                var isDirty = me.isDirty();
                if (isDirty == true) {
                    Ext.MessageBox.confirm("提示".t(), "数据还未保存，是否排序？".t(), function (btnId) {
                        if (btnId == "yes") {
                            if (arguments.length === 0) {
                                if (me.getRemoteSort()) {
                                    me.load();
                                } else {
                                    me.forceLocalSort();
                                }
                            } else {
                                me.getSorters().addSort(field, direction, mode);
                            }
                        }
                    });
                } else {
                    if (arguments.length === 0) {
                        if (me.getRemoteSort()) {
                            me.load();
                        } else {
                            me.forceLocalSort();
                        }
                    } else {
                        //如果是引用属性，排序使用展示的值排序(适用于前端排序)
                        if (!remoteSort) {
                            var objFiled = me.data.items[0].data;
                            if (objFiled[field + "_Display"]) {
                                field += "_Display";
                            }
                        }
                        me.getSorters().addSort(field, direction, mode);
                    }
                }
            }
        },
        /**
         * 序列化请求数据并返回json字符串
         * @param {Object} input-请求的参数对象
         * @returns {String} json字符串 
         */
        seriaizeRequest: function (input) {
            for (var property in input) {
                var value = input[property];
                if (value) {
                    //如果是实体或者实体集合，则需要序列化为变更集。
                    if (value.isModel || value.isStore) {
                        //都使用组合实体序列化方式，否则需要传入的实体本身没有加载任何子。
                        var changeSet = SIE.data.Serializer.serialize(value, true);
                        input[property] = changeSet.getSubmitData();
                    }
                }
            }
            return Ext.encode(input);
        },
        /**
         * 反序列化响应内容-ajax请求
         * @param {Object} res-响应内容
         * @returns {Object} 返回并已处理的内容
         */
        deserializeResponse: function (res) {
            for (var property in res) {
                var value = res[property];
                //如果定义了 model 属性，则表示这个属性是一个实体或者实体的集合，这时需要自动转换为 Model、Store。
                if (value && value.model) {
                    if (value._isEntity) {
                        var entity = Ext.create(value.model, value);
                        res[property] = entity;
                    }
                    else {
                        var store = SIE.data.Utils.createStore({
                            model: value.model
                        });
                        if (store.isTreeStore && value.entities) {  //该value为一个树形实体的集合时，store.loadRawData有问题，暂时先这样赋值，待修改..
                            for (var i = 0; i < value.entities.length; i++) {
                                store.add(value.entities[i]);
                            }
                        } else {
                            store.loadRawData(value);
                        }

                        res[property] = store;
                    }
                }
            }
            return res;
        }
    }
});

Ext.define('SIE.data.DataQueryer', {
    singleton: true,
    query: function (op) {
        /// <summary>
        /// 调用指定的服务
        /// </summary>
        /// <param name="op">
        /// type: 必选，查询器名称。
        /// token: 可选，令牌。
        /// method: 必选，查询方法名称。
        /// params: 可选, 参数数组。
        /// callback: 可选，回调。
        /// async: true。
        /// </param>
        op = Ext.apply({ async: true }, op);
        var me = this;
        var action = 'queryer';
        if (op.action)
            action = op.action;
        //var url = Ext.String.format("/api/DataPortal/Query?action={1}&type={0}", op.type, action);
        var filter = { Method: op.method, Parameters: op.params || [] };
        SIE.Ajax({
            url: '/api/DataPortal/Query',
            timeout: op.timeout,
            async: op.async,
            method: 'POST',
            params: {
                action: action,
                type: op.type,
                filter: SIE.data.Utils.seriaizeRequest(filter),
                token: op.token
            },
            //params: { filter: SIE.data.Utils.seriaizeRequest(filter), token: op.token },
            success: function (response, opts) {
                var res = response.responseJson;
                if (!res && response.responseText)
                    res = Ext.decode(response.responseText);
                SIE.data.Utils.deserializeResponse(res);
                if (res.Success) {
                    if (op.success)
                        op.success(res);
                }
                else {
                    if (op.error)
                        op.error(res);
                    SIE.Msg.showError(res.Message);
                }
                if (op.callback) {
                    op.callback(res);
                }
            },

            failure: function (response, opts) {
                //http state
                if ('communication failure' === response.statusText) {
                    SIE.Msg.showWarning('请求时间超时'.t());
                } else if (response.statusText === '') {

                }
                else {
                    var res = response.responseJson;
                    if (!res && response.responseText) {
                        res = Ext.decode(response.responseText);
                        SIE.Msg.showError(res.Message);
                    }

                }
            }
        });
    }
});


//internal
//本类型用于封装从客户端实体中获取用于提交到服务端的变更集，所有实体都是以列表的形式来进行提交。
Ext.define('SIE.data.ListChangeSet', {
    //internal
    _data: null,

    //internal
    //根实体的类型
    _model: null,

    //-------------------------------------  Common -------------------------------------
    getModel: function () {
        /// <summary>
        /// 获取根实体的类型。
        /// </summary>
        /// <returns type="Class"></returns>
        return this._model;
    },
    isEmpty: function () {
        /// <summary>
        /// 判断数据变更集中是否有需要提交到服务端的数据。
        /// </summary>
        /// <param name="changeSetData"></param>
        /// <returns type=""></returns>

        var d = this._data;

        if (this._model.isTree) {
            //整颗树传到后端处理
            //return !(d.d && d.d.length
            //    || d.roots && d.roots.length);
            return !(d.u && d.u.length
                || d.c && d.c.length
                || d.d && d.d.length);
        }
        else {
            return !(d.u && d.u.length
                || d.c && d.c.length
                || d.d && d.d.length
                || d.uc && d.uc.length);
        }
    },
    getSubmitData: function () {
        /// <summary>
        /// 转换为可以提交到服务端的 Json 对象（非字符串）。
        /// </summary>
        /// <returns type=""></returns>

        //根实体，在数据提交到服务端前，我们需要在纯粹的数据上添加字符串属性 _model，
        //用以告诉服务端这个数据应该解析为哪个实体类。
        var d = this._data;
        if (!d._model) {
            d._model = Ext.getClassName(this._model);
        }
        return d;
    }
});
/// <reference path="../_reference.js" />

Ext.define('SIE.data.crudState', {
    statics: {
        C: 'c',
        U: 'u',
        D: 'd',
        UC: 'uc',
    }
});

//internal 本类是包类使用，外部不要使用。这是因为服务器与客户端数据传输的格式的变化比较大，需要封装在这个类中。
//实体到服务端的序列化器。
Ext.define('SIE.data.Serializer', {
    //是否要递归序列化聚合子集合。
    //默认为: false, 是否需要提取子视图中的数据。
    _withChildren: false,
    _view: null,

    //-------------------------------------  API -------------------------------------
    statics: {
        serialize: function (component, withChildren, view) {
            /// <summary>
            /// 序列化指定的实体对象或数据集。
            /// </summary>
            /// <param name="component">要序列化的实体对象或数据集。</param>
            /// <param name="withChildren">是否要递归序列化聚合子集合。</param>
            /// <returns type="Object">存放数据的对象。</returns>

            var instance = new SIE.data.Serializer();
            instance._withChildren = withChildren;
            instance._view = view;

            var changeSet = new SIE.data.ListChangeSet();

            if (component.isModel) {
                changeSet._data = instance._serializeEntity(component);
                changeSet._model = Ext.getClass(component);
            }
            else {
                changeSet._data = instance._serializeStore(component);
                changeSet._model = component.model;
            }

            return changeSet;
        }
    },

    _serializeEntity: function (entity) {
        /// <summary>
        /// 获取某个实体中需要提交到服务器上的数据。
        /// </summary>
        /// <param name="entity" type="SIE.data.Entity">要序列化的实体对象。</param>
        /// <returns type="Object">返回存放实体数据的纯 json 对象。</returns>

        var me = this;
        var crudState = SIE.data.crudState;

        //注意，单个实体的数据，依然是以 EntityList 的方式提交。
        //这样不但统一了数据的格式，而且还简单用实体列表的集合来分辨当前实体的状态（IsNew、IsDeleted）。
        //添加属性 _isEntityHost 用于分辨二者。
        var dto = { _isEntityHost: 1 };

        if (entity.isNew()) {
            dto.c = [entity];
            me._getPersistArray(dto, crudState.C);
        }
        else if (entity.isSelfDirty()) {
            dto.u = [entity];
            me._getPersistArray(dto, crudState.U);
        }
        else if (entity.isDirty()) {
            dto.uc = [entity];
            me._getPersistArray(dto, crudState.UC);
        }

        return dto;
    },
    _serializeStore: function (store) {
        /// <summary>
        /// 序列化指定的数据集。
        /// </summary>
        /// <param name="store">要序列化的数据集。</param>
        /// <returns type="Object">存放数据的对象。</returns>

        var dto = null;
        if (store instanceof Ext.data.TreeStore) {
            dto = this._serializeStore_TreeStore(store);
        }
        else {
            dto = this._serializeStore_Store(store);
        }

        return dto;
    },
    _serializeChildrenRecur: function (entity, dto) {
        /// <summary>
        /// 把某个实体 entity 中已经加载的所有关联子实体都序列化到 dto 中。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        var me = this;
        var childrenList = entity.getEntityChildren();
        childrenList.eachKey(function (key, item, index, len) {
            if (item) {
                if (item instanceof Ext.data.Model) {
                    if (Ext.getClassName(entity) === Ext.getClassName(item)) {
                        //附加子实体与父实体相同时,则处理变更过的属性值
                        var changes = item.getChanges();
                        if (SIE.hasAnyProperty(changes)) {
                            for (var prop in changes) {
                                if (changes.hasOwnProperty(prop)) {
                                    dto[prop] = changes[prop];
                                }
                            }
                        }
                    } else {
                        //不同时则作为父实体的一个关联属性
                        var listData = [];
                        listData.push(item.data);
                        if (item.getEntityChildren()) {
                            var serializeObj = me._serializeStore(item.store);
                            dto[item.associateView.getAssociateProperty()] = serializeObj;
                        }
                        else {
                            if (item.phantom == true || item.data.Id <= 0) {
                                dto[item.associateView.getAssociateProperty()] = { c: listData };
                            } else {
                                dto[item.associateView.getAssociateProperty()] = { u: listData };
                            }
                        }
                    }
                }
                else if (item instanceof Ext.data.Store || item.$className == "Ext.data.Store") {
                    if ((item.getCount() + item.getTotalCount() + item.getRemovedRecords().length) > 0) {
                        var serializeObj = me._serializeStore(item);
                        //方案1：子列表不传空数据到后台，让后台使用自身懒加载数据通过实体验证
                        if (!SIE.isEmptyObject(serializeObj)) {
                            var prop = item.associateView ? item.associateView.getAssociateProperty() : key;
                            //补充场景，同一个实体属性，拆分为多个页面去承载显示，提交时需要合并数据json，如检验单据明细属性(拆分为定性，定量显示）
                            dto[prop] = me._getPersionComplexData(dto[prop], serializeObj);
                        }
                    }
                }
            }
        });
    },

    //-------------------------------------  Common -------------------------------------
    /**
     * 获取某个实体序列化后的数据
     * @param {Object} entity-实体对象
     * @param {Boolean} deleted-是否已经被删除标识
     * @returns {Object} 普通对象
     */
    _getPersistData: function (entity, deleted) {
        var dto = {};

        //如果该对象是被删除了，则只需要传输 Id 即可。
        if (deleted) {
            dto[SIE._KeyPropertyName] = entity.getId();
            var title;
            if (this._view)
                title = this._view.title;//支持view主属性传输,为信息提示
            if (title)
                dto[title] = entity.get(title);
        }
        else {
            //属性需要可以保存，并且是大写
            var fields = SIE.data.Utils.getPersistFields(entity);
            SIE.each(fields, function (f) { dto[f.name] = entity.get(f.name); });

            if (this._withChildren) {
                this._serializeChildrenRecur(entity, dto);
            }
        }

        return dto;
    },
    /**
     * 把 dto 中指定名称 property 的一个实体集合转换为数据的集合。
     * @param {type} dto-数据传输对象
     * @param {type} property-属性名称(c,r,u,d)
     */
    _getPersistArray: function (dto, property) {
        var me = this;

        var raw = dto[property];
        if (!raw || raw.length == 0) {
            delete dto[property];
            return;
        }

        var deleted = property == 'd';

        var list = [];
        SIE.each(raw, function (item) {
            list.push(me._getPersistData(item, deleted));
        });
        dto[property] = list;
    },
    /**
     * 获取持久化的复合数据结构
     * @param {Object} source--源对象结构
     * @param {Object} dest--目标对象结构
     * @returns {Object} 合并后的对象结构
     */
    _getPersionComplexData: function (source, dest) {
        var crudState = SIE.data.crudState;
        if (SIE.isEmpty(source)) {
            source = dest;
        }
        else {
            this._getPersionComplexArray(source, dest, crudState.C);
            this._getPersionComplexArray(source, dest, crudState.U);
            this._getPersionComplexArray(source, dest, crudState.D);
            this._getPersionComplexArray(source, dest, crudState.UC);
        }

        return source;
    },
    /**
     * 合并具体的属性
     * @param {Object} source--源对象结构
     * @param {Object} dest--目标对象结构
     * @returns {Array} 合并后的对象数组结构
     */
    _getPersionComplexArray: function (source, dest, property) {
        if (source[property] && dest[property]) {
            var arr = Ext.Array.union(source[property], dest[property]);
            source[property] = arr;
        }
        else if (!source[property] && dest[property]) {
            source[property] = dest[property];
        }
    },
    //-------------------------------------  Store -------------------------------------
    _serializeStore_Store: function (store) {
        var crudState = SIE.data.crudState;
        var data = {
            c: store.getNewRecords(),
            u: store.getUpdatedRecords(),
            d: Ext.Array.filter(store.getRemovedRecords(), function (i) { return !i.isNew(); }),
            //本身未改变，组合子发生改变的实体，放到 uc 集合中提交。
            uc: store.data.filterBy(
                function (item) {
                    if (item.isModel && item.$className)
                        return !item.isSelfDirty() && item.isEntityChildrenDirty();
                    else
                        return false;
                }).items
        };

        this._getPersistArray(data, crudState.C);//toCreate
        this._getPersistArray(data, crudState.U);//toUpdate
        this._getPersistArray(data, crudState.D);//toDelete
        this._getPersistArray(data, crudState.UC);//unchanged

        return data;
    },

    //-------------------------------------  TreeStore -------------------------------------
    _serializeStore_TreeStore: function (treeStore) {
        var crudState = SIE.data.crudState;
        var data = {
            c: treeStore.getNewRecords(),
            u: treeStore.getUpdatedRecords(),
            d: Ext.Array.filter(treeStore.getRemovedRecords(), function (i) { return !i.isNew(); })
        };
        this._getPersistArray(data, crudState.C);//toCreate
        this._getPersistArray(data, crudState.U);//toUpdate
        this._getPersistArray(data, crudState.D);//toDelete
        return data;
    },
    _convertNodeRecur: function (node) {
        var item = this._getPersistData(node);

        if (node.isNew()) {
            item.isNew = 1;
        }

        if (!node.isLeaf()) {
            item.TreeChildren = [];
            for (var i = 0; i < node.childNodes.length; i++) {
                var child = this._convertNodeRecur(node.childNodes[i]);
                item.TreeChildren.push(child);
            }
        }

        return item;
    }
    //TreeStore 差异保存方案，暂留。
    //    ,_convertTreeCreateData: function (entityList, property, option) {
    //        var me = this;
    //        //树结点时，要处理树型结点之间的关系
    //        var raw = entityList[property];
    //        if (!raw || raw.length == 0) {
    //            delete entityList[property];
    //            return;
    //        }
    //        //roots 中只传输根对象
    //        var roots = [];
    //        //当前已经处理完的元素列表
    //        var added = [];
    //        //当前还没有处理的元素列表
    //        var toAdd = Ext.Array.clone(raw);
    //        SIE.each(raw, function (item) { 
    //            var isParent = item.parentNode.isRoot();
    //            if (!isParent) {
    //                var parent = SIE.findFirst(raw, function (p) { return p == item.parentNode; });
    //                isParent = parent == null;
    //            }
    //            if (isParent) {
    //                var data = me._getPersistData(item);
    //                data._rawNode = item;
    //                roots.push(data);
    //                added.push(data);
    //                Ext.Array.remove(toAdd, item);
    //            }
    //        });
    //        var i = 1;
    //        while (toAdd.length > 0) {
    //            if (i++ > 1000) { SIE.notSupport("死循环异常……"); }
    //            SIE.each(toAdd, function (item) {
    //                var data = me._getPersistData(item);
    //                data._rawNode = item;
    //                var parent = SIE.findFirst(added, function (p) { return p._rawNode == item.parentNode; });
    //                if (parent) {
    //                    if (!parent.TreeChildren) parent.TreeChildren = [];
    //                    parent.TreeChildren.push(data);
    //                    added.push(data);
    //                    Ext.Array.remove(toAdd, item);
    //                    return false;
    //                }
    //            });
    //        }
    //        SIE.each(added, function (i) { delete i._rawNode; });
    //        entityList[property] = roots;
    //    }
});

Ext.define('SIE.field.DateRange', {
     extend: 'Ext.data.field.Field',

     alias: 'data.field.daterange',
});

Ext.define('SIE.field.TextRange', {
    extend: 'Ext.data.field.Field',

    alias: 'data.field.textrange',
});

Ext.define('SIE.field.NumberRange', {
    extend: 'Ext.data.field.Field',

    alias: 'data.field.numberrange',
});
/// mapping Enum
Ext.define('SIE.Domain.State', {
    statics: {
        Enable: { value: 1, text: 'Enable', label: '可用' },
        Disable: { value: 0, text: 'Disable', label: '禁用' }
    }
});
Ext.define('SIE.MetaModel.View.FormChildSaveMode', {
    statics: {
        None: { value: 0, text: 'None', label: 'None' },
        Save: { value: 1, text: 'Save', label: 'Save' },
        NoSave: { value: 2, text: 'NoSave', label: 'NoSave' }
    }
});
Ext.define('SIE.MetaModel.View.EditMode', {
    statics: {
        Inline: { value: 0, text: 'Inline', label: '行内编辑'.t() },
        Form: { value: 1, text: 'Form', label: '表单编辑'.t() },
        Popup: { value: 2, text: 'Popup', label: '弹窗编辑'.t() }
    }
});
Ext.define('SIE.Common.Platform', {
    statics: {
        Wpf: { value: 1, text: 'Wpf', label: 'Wpf' },
        Web: { value: 2, text: 'Web', label: 'Web' },
        Mobile: { value: 4, text: 'Mobile', label: 'Mobile' }
    }
});
Ext.define('SIE.CommandGroupType', {
    statics: {
        Edit: { value: 10, text: 'edit', label: 'Edit' },
        View: { value: 20, text: 'view', label: 'View' },
        Business: { value: 30, text: 'business', label: 'Business' },
        None: { value: 40, text: 'none', label: 'None' },
        System: { value: 100, text: 'system', label: 'System' }
    }
});
Ext.define('SIE.cmd.IntervalMode', {
    statics: {
        Debounce: { value: 0, text: '防抖'.t() },
        Throttle: { value: 1, text: '节流'.t() },
        None: { value: 2, text: '' }
    }
});

Ext.define('SIE.cmd.Command', function () {
    var _executeInternal = function (view, source, scope) {
        /// <summary>
        /// 直接执行本命令。
        /// </summary>
        /// <param name="view">本命令作用的视图对象。</param>
        /// <param name="source">引发本命令的源控件。</param>
        //如果是要调试状态，则不需要截住异常并记录错误日志，否则 chrome 调试器不能定位到异常代码处。
        var me = scope || this;
        if (SIE.isDebugging()) {
            me.execute(view, source);
            me.fireEvent('executed', { view: view });
        }
        else {
            try {
                me.execute(view, source);
                me.fireEvent('executed', { view: view });
            } catch (e) {
                //如果事件处理函数设置了 cancel 为 true，则表示异常已经被处理。
                var args = { exception: e, view: view };
                me.fireEvent('executeFailed', args);
                if (!args.cancel) throw e;
            }
        }
    };
    var _executeWrape = _executeInternal;
    var _initExecuteWrape = function (scope, source) {
        _executeWrape = _executeInternal;
        if (scope.executeIntervalMode !== SIE.cmd.IntervalMode.None.value) {
            var bodyFunc = _executeInternal;
            var wrapeFunc = function (view, source, scope) {
                var me = scope || this;
                me.lastClickTime = Ext.now();
                bodyFunc(view, source, me);
            };
            var elapsed = Ext.now() - scope.lastClickTime;
            var interval = scope.getExecuteInterval();
            if (elapsed <= interval) { //速度在间隔内,则加防抖节流,间隔内只执行第一次
                /* 因为UI提交ajax请求后（比如来料检验建单功能），界面还未关闭，按钮还可以点击的，
                 * 但是超过配置的时间后，再点次保存插入会报DB主键冲突。所以将按钮禁用变灰，平台命令会请求后变可用
                */
                bodyFunc = Ext.emptyFn;
                source.disable();
                var msg = scope.getExecuteIntervalModeText();
                msg += '连击间隔:'.t() + elapsed + '毫秒,当前时间: '.t() + Ext.Date.format(new Date(), Ext.Date.patterns.ISO8601Long);
                console.log(msg);
            }
            _executeWrape = wrapeFunc;
        }
    };
    //--------------
    return {
        extend: 'Ext.util.Observable',
        config: {
            meta: {
                text: '按钮'.t(),//text 用于描述按钮的显示文本。子类必须重写，如果未重写，则显示父类名称
                group: 'view',//group 描述按钮所在的分组，一般有以下分类：edit,view,business
                title: '默认基类按钮(当你看到这个，就是忘记自己定义按钮的名称,使用了父类的)'.t(),
                tooltip: '默认基类按钮(当你看到这个，就是忘记自己定义按钮的名称,使用了父类的)'.t(),
            },
        },

        _ownerView: null,//当前正在执行的视图
        view: null,
        executeIntervalSecond: 1,// 间隔，对外友好使用单位(秒)
        getExecuteInterval: function () { return (this.executeIntervalSecond || 1) * 1000; }, // 间隔，默认1秒，单位(毫秒)，
        executeIntervalMode: SIE.cmd.IntervalMode.None.value,
        getExecuteIntervalModeText: function () {
            var text = '';
            switch (this.executeIntervalMode) {
                case SIE.cmd.IntervalMode.Throttle.value:
                    text = SIE.cmd.IntervalMode.Throttle.text;
                    break;
                case SIE.cmd.IntervalMode.Debounce.value:
                    text = SIE.cmd.IntervalMode.Debounce.text;
                    break;
            }
            if (!SIE.isEmpty(text)) {
                text += '模式,配置间隔'.t() + this.getExecuteInterval() + '毫秒,'.t();
            }
            return text;
        },
        isCommand: true,
        lastClickTime: 0, //最后点击时间
        constructor: function (config) {
            this.callParent(arguments);

            this.initConfig(config);
            this.lastClickTime = 0;
        },

        _modifyMeta: function (meta) {
            /// <summary>
            /// internal 
            /// 
            /// 把本类中定义的 meta 再次修改到参数中，接下来会用修改后的 meta 来生成界面
            /// </summary>
            /// <param name="meta"></param>

            //getMeta 是 initConfig 生成的方法。
            Ext.applyIf(meta, this.getMeta());
        },
        _setOwnerView: function (value) {
            /// <summary>
            /// internal
            /// 设置本命令对应的视图。
            /// </summary>
            /// <param name="value"></param>
            this._ownerView = value;
            this.view = value;
        },

        getOwnerView: function () {
            /// <summary>
            /// 获取本命令所在的视图
            /// </summary>
            /// <returns></returns>
            return this._ownerView;
        },

        tryExecute: function (source) {
            /// <summary>
            /// 尝试执行某个命令
            /// </summary>
            /// <param name="source">引发本命令的源控件。</param>
            var v = this._ownerView;
            if (this.canExecute(v, source)) {
                _initExecuteWrape(this, source);
                v.setSourceCmd(source);
                _executeWrape(v, source, this);
                return true;
            }

            return false;
        },

        /**
         * 命令是否可见
         * @param {SIE.view.View} view 
         * @param {} source 
         * @returns {} 
         */
        canVisible: function (view, source) {
            /// <summary>
            /// virtual
            /// 子类重写此方法来执行本命令的是否可见的逻辑。
            /// </summary>
            /// <param name="view">本命令作用的视图对象。</param>
            /// <param name="source">引发本命令的源控件。</param>
            /// <returns>返回是否可执行，默认 true。</returns>
            return true;
        },

        /**
        * @protected virtual bool canExecute
        * 命令是否可执行 
        * @param {SIE.view.View} view
        */
        canExecute: function (view, source) {
            /// <summary>
            /// virtual
            /// 子类重写此方法来执行本命令的是否可执行的逻辑。
            /// </summary>
            /// <param name="view">本命令作用的视图对象。</param>
            /// <param name="source">引发本命令的源控件。</param>
            /// <returns>返回是否可执行，默认 true。</returns>
            return true;
        },
        /**
         * @protected abstract void execute
         * 命令具体执行
         * @param {SIE.view.View} view
         * @param {SIE.cmd.Command} source
         */
        execute: function (view, source) {
            /// <summary>
            /// abstract
            /// 子类重写此方法来执行本命令的逻辑。
            /// </summary>
            /// <param name="view">本命令作用的视图对象。</param>
            /// <param name="source">引发本命令的源控件。</param>
            SIE.markAbstract();
        },

    };
});


Ext.define('SIE.cmd.CommandManager', {
    singleton: true,

    _commands: [],
    /**
     * 定义一个命令
     * @param {String} cmdName-命令名称
     * @param {Object} members-命令的元数据定义
     * @returns {SIE.cmd.Command} 
     */
    defineCommand: function (cmdName, members) {
        /// <summary>
        /// 定义一个命令类型。
        /// </summary>
        /// <param name="cmdName">命令的名称。</param>
        /// <param name="members">
        /// meta: 按钮的元数据。目前主要用于配置界面上的按钮。
        /// </param>
        if (Ext.isFunction(members)) {
            members = members();
        }
        //如果没有定义基类，那么基类是 SIE.cmd.Command。
        Ext.applyIf(members, {
            extend: 'SIE.cmd.Command'
        });

        //如果 meta 是直接定义在 members 中的，则应该把它移动到 config 中。
        var meta = members.meta;
        if (meta) {
            //delete members.meta; //先注释观察 项目需求：支持继承了父类，但可以不写text之类的情况的,
            if (!members.config) {
                members.config = {};
            }
            if (meta.iconCls) {
                if (meta.iconCls.indexOf('iconfont') === -1) {
                    meta.iconCls = Ext.String.format('{0} {1}', 'iconfont', meta.iconCls);
                }
            }
            if (meta.text) {
                meta._text = meta.text;
                meta.text = meta.text.t();
            }
            if (meta.title) {
                meta.title = meta.title.t();
            }
            if (!meta.tooltip) {
                meta.tooltip = meta.text;
            } else {
                meta.tooltip = meta.tooltip.t();
            }
            meta.tooltipType = 'title'; // tooltipType default: qtip
            members.config.meta = meta;
        }
        var userConfig = members.userConfig;
        if (userConfig) {
            Ext.apply(members.config.userConfig, userConfig);
        }
        delete members.requires;
        var cmdClass = Ext.define(cmdName, members);

        this._commands.push(cmdClass);

        return cmdClass;
    },
    /**
     * 获取所有命令类型的集合
     * @returns {Array} 
     */
    getCommandClasses: function () {
        return this._commands;
    },
    /**
     * 获取命令的集合
     * @returns {Array} 
     */
    getCommands: function () {
        var res = [];

        SIE.each(this._commands, function (c) {
            res.push(Ext.getClassName(c));
        });

        return res;
    }
});
Ext.define('SIE.cmd.CommandController', {
    singleton: true,
    excuteCommand: function (cmd, module, scope, data) {
        this.excute({
            cmd: cmd,
            module: module,
            scope: scope,
            data: data,
            callback: null
        });
    },
    excute: function (op) {
        /// <summary>
        /// 调用指定的命令
        /// </summary>
        /// <param name="op">
        /// cmd: 必选，字符串表示的命令名称。
        /// module: 命令所在模块。
        /// scope : 命令所在模块的子范围。
        /// data : 数据。
        /// callback: 可选，回调。
        /// async: true。
        /// </param>
        op = Ext.apply({ async: true }, op);

        var me = this;

        if (!op.data) { op.data = {}; }

        SIE.Ajax({
            url: op.url || '/api/Command/Excute',
            async: op.async,
            timeout: op.timeout || 7200000, //2H
            method: 'POST',
            params: { Name: op.cmd, Token: op.token, Data: SIE.data.Utils.seriaizeRequest(op.data) },
            success: function (response, opts) {
                var res = response.responseJson;
                if (!res && response.responseText)
                    res = Ext.decode(response.responseText);

                SIE.data.Utils.deserializeResponse(res);
                if (op.callback) {
                    op.callback(res);
                }
            },
            failure: function (response, opts) {
                //http state
                if ('communication failure' === response.statusText) {
                    SIE.Msg.showWarning('请求时间超时'.t());
                }
                else {
                    var res = response.responseJson;
                    if (!res && response.responseText) {
                        res = Ext.decode(response.responseText);
                        SIE.Msg.showError(res.Message);
                    }     
                }
            }
        });
    }
});
/// <reference path="../_reference.js" />

Ext.define('SIE.view.View', {
    extend: 'Ext.util.Observable',
    statics: {
        currentId: 1
    },

    //如果当前是子视图，_childProperty表示父实体的子属性
    _childProperty: null,
    //如果当前是子视图，associatedProperty表示父实体的关联子属性
    _associatedProperty: null,
    config: {
        controller: null,
    },
    isView: true,
    constructor: function (meta) {
        this.initConfig(meta);
        this.callParent(arguments);
        var me = this;
        me._id = SIE.view.View.currentId++;
        me._meta = meta;
        me._children = [];
        me._relations = []; // SIE.view.RelationView
        me._model = SIE.getModel(meta.model);
        if (!me._model)
            throw new Ext.Error(meta.model + ' not define.');
        me._isTree = me._model.isTree;
        me._commands = new Ext.util.MixedCollection();
        me._sourceCmd = null;
        me._propertyChangeHandlers = [];
        return this;
    },
    listeners: {
        //视图准备完成事件
        'isReady': function (value) {
            this.isReady = value;
            this.syncCmdState();
        }
    },

    //listeners: {
    //    //视图准备完成事件
    //    'isReady': function (value) {
    //        var me = this;
    //        me.isReady = value;
    //        me.syncCmdState();
    //        var refresh = function (recordId) {
    //            if (me.refreshData)
    //                me.refreshData(recordId);
    //        };
    //        CRT.Event.listen(me.model + "_refresh", refresh);
    //        me.mon(me, 'beforeClosewin', function () {
    //            CRT.Event.remove(me.model + "_refresh", refresh);
    //        });
    //    },
    //},

    //-------------------------------------  Common -------------------------------------
    _id: 0, //当前视图的 Id，主要是为了防止同一个实体类的多个视图上的命令 id 冲突。
    _control: null,
    _meta: null,
    _model: null,
    //internal
    _isTree: false,
    _sourceCmd: null,
    isReady: false,
    _propertyChangeHandlers: null, //属性变更处理
    addProChgHandler: function (handler) {
        this._propertyChangeHandlers.push(handler);
    },
    getProChgHandlers: function () {
        return this._propertyChangeHandlers;
    },
    getSourceCmd: function () {
        return this._sourceCmd;
    },
    setSourceCmd: function (value) {
        this._sourceCmd = value;
    },
    getToken: function () {
        return this.token;
    },
    getId: function () {
        /// <summary>
        /// 返回当前视图在客户端的运行时 Id
        /// </summary>
        /// <returns type="int"></returns>
        return this._id;
    },
    getMeta: function () {
        /// <summary>
        /// 返回本视图对应的客户端元数据。
        /// </summary>
        /// <returns type=""></returns>
        return this._meta;
    },
    getModel: function () {
        /// <summary>
        /// 获取本实体视图对应实体的类型
        /// </summary>
        /// <returns></returns>
        return this._model;
    },
    getProxyUrl: function () {
        /// <summary>
        /// 返回本视图对应的数据网关地址。
        /// </summary>
        /// <returns type=""></returns>
        return this.getModel().getProxy().url;
    },
    getControl: function () {
        /// <summary>
        /// 返回本视图对应的界面元素。
        /// </summary>
        /// <returns type=""></returns>
        return this._control;
    },
    _setControl: function (value) {
        /// <summary>
        /// internal virtual
        /// 设置本视图对应的界面元素。
        /// </summary>
        /// <param name="value"></param>
        var old = this._control;
        if (old) {
            delete old.SIEView;
        }

        this._control = value;

        //为 value 附加一个属性，表明这个控件所对应的 SIEView。
        //目前，这个值会被 ComboList 使用到。
        if (value) {
            value.SIEView = this;
        }
    },
    isTree: function () {
        /// <summary>
        /// 是否树形
        /// </summary>
        /// <returns type="bool"></returns>
        return this._isTree;
    },
    setControlReadOnly: function (readonly) {
        /// <summary>
        /// 设置控件是否可只读（抽象方法）
        /// </summary>
        SIE.markAbstract();
    },

    /**
     * 验证数据
     * 子类重写此方法实现特定的数据验证逻辑
     * @returns {} 
     */
    validateData: function () {
        return true;
    },
    //-------------------------------------  Data and Current -------------------------------------
    _current: null,
    getData: function () {
        /// <summary>
        /// abstract
        /// 返回本视图对应的所有数据。
        /// </summary>
        SIE.markAbstract();
    },
    setData: function (value) {
        /// <summary>
        /// 设置本视图对应的所有数据。
        /// </summary>
        /// <param name="value"></param>
        SIE.markAbstract();
    },
    getCurrent: function () {
        /// <summary>
        /// 获取当前的实体。
        /// </summary>
        /// <returns type="SIE.data.Entity"></returns>
        return this._current;
    },
    setCurrent: function (value, force) {
        /// <summary>
        /// 设置当前的实体对象。
        /// </summary>
        /// <param name="value"></param>
        if (force || this._current != value) {
            var oldValue = this._current;
            this._current = value;
            this._onCurrentChanged(oldValue, value);
        }
    },
    /**
     *  当数据变更时调用此方法。并发生 dataChanged 事件。
     * @param value data
     */
    _onDataChanged: function (value) {
        this.fireEvent('dataChanged', {
            value: value
        });
    },

    /**
     * 当当前实体对象变更时调用此方法。并发生 currentChanged 事件。
     * currentChanged并没有在打开表单时进行mon，作用是什么留做入口？？
     * @param oldValue 旧实体
     * @param value  新实体
     */
    _onCurrentChanged: function (oldValue, value) {
        var me = this;
        me._resetChildrenData();
        me.fireEvent('currentChanged', {
            oldValue: oldValue,
            newValue: value
        });
        me.isReady = true;
        me.syncCmdState();
    },

    /**
     * 重新加载所有子视图的所有数据
     */
    _resetChildrenData: function () {
        Ext.each(this._children, function (n, i, s) {
            n.loadChildData();
        });
    },

    /**
     * 重写此方法实现子视图数据加载
     * @param opType
     */
    loadChildData: function (opType) {

    },

    //-------------------------------------  Parent - Children -------------------------------------
    //internal
    //_parent: null,
    //_children: null,// []
    /**
     * 获取当前视图的聚合父视图
     * @returns
     */
    getParent: function () {
        return this._parent;
    },

    /**
     * 获取当前视图的所有子视图
     * @returns
     */
    getChildren: function () {
        return this._children;
    },

    /**
     *  查找某个子实体对应的视图
     * @param model
     * @param recur 是否迭归查找更下层的子
     * @returns
     */
    findChild: function (model, recur) {
        for (var i = 0; i < this._children.length; i++) {
            var c = this._children[i];
            if (c._meta.model == model) {
                return c;
            }

            if (recur) {
                c = c.findChild(model);
                if (c != null) return c;
            }
        }
        return null;
    },

    /**
     * 设置当前视图的聚合父视图
     * @param value
     */
    _setParent: function (value) {
        if (value == null) {
            if (this._parent != null) {
                this._parent._removeChild(this);
            }
        } else {
            value._addChild(this);
        }
    },

    /**
     * 添加指定的视图到本视图的子视图集合中
     * @param value
     */
    _addChild: function (value) {
        var c = this._children;
        if (!Ext.Array.contains(c, value)) {
            c.push(value);
        }
        value._parent = this;
    },

    /**
     * 从本视图的子视图集合中删除指定的视图
     * @param value
     */
    _removeChild: function (value) {
        if (value._parent == this) {
            Ext.Array.remove(this._children, value);
            value._parent = null;
        }
    },

    //-------------------------------------  Relations -------------------------------------
    //_relations: null,
    /**
     * 获取当前视图的所有关系视图
     * @returns
     */
    getRelations: function () {
        return this._relations;
    },

    /**
     * 查找对应名称的关系视图
     * @param name 视图名称
     * @returns
     */
    findRelationView: function (name) {
        var target = SIE.findFirst(this._relations, function (r) {
            return r.getName() == name;
        });
        if (target != null) return target.getTarget();
        return null;
    },

    /**
     * 为本视图添加一个关系视图
     * @param relation 
     */
    _setRelation: function (relation) {
        var exist = SIE.findFirst(this._relations, function (r) {
            return r.getName() == relation.getName();
        });
        if (exist != null) {
            Ext.Array.remove(this._relations, exist);
        }

        relation._owner = this;
        this._relations.push(relation);
    },

    //-------------------------------------  Command -------------------------------------
    //private,mixed collection, key:cmdName, value:cmd.
    _commands: null,
    getCmdControl: function (cmdName) {
        var id = this._getCmdControlId(cmdName);
        if (this._control.items !== null) {
            return this._control.queryById(id);
        } else {
            return null;
        }
    },

    /**
     * 
     * @param cmdName
     * @returns
     */
    _getCmdControlId: function (cmdName) {
        /// <summary>
        /// internal
        /// </summary>
        /// <param name="cmdName"></param>
        /// <returns type=""></returns>
        var m = this._meta;
        var name = m.model + '-' + cmdName;
        var id = name.replace(/\./g, '_') + '_' + this._id;;
        return id;
    },

    /**
     * 返回本视图上的所有命令
     * @returns key:cmdName, value:cmd
     */
    getCommands: function () {
        return this._commands;
    },

    /**
     * 通过命令类型来查找命令
     * @param cmdType 类型，可以直接通过父类型来查找子类型按钮。
     */
    findCmd: function (cmdType) {
        return SIE.findFirst(this._commands, function (c) {
            return c instanceof cmdType;
        });
        //return this._commands.getByKey(cmdType);
    },

    /**
     * 为本视图添加一个命令。
     * @param cmdName 视图名称
     * @param cmd 视图对象
     */
    _addCmd: function (cmdName, cmd) {
        this._commands.add(cmdName, cmd);
    },

    /**
     * 同步视图命令的状态
     * @param view 视图
     * @param recursion 
     */
    syncCmdState: function (view, recursion) {
        if (this.isReady) {
            view = view || this;
            recursion = Ext.isEmpty(recursion) ? true : recursion;
            var commands = view.getCommands();
            if (commands && commands.length > 0) {
                var btn = null;
                commands.eachKey(function (key, item, index, len) {
                    btn = view.getCmdControl(key);
                    if (btn != null) {
                        if (item.canVisible(view)) {
                            btn.show();
                            if (item.canExecute(view)) btn.enable();
                            else btn.disable();
                        } else {
                            btn.hide();
                        }
                    }
                }, view);
            }
            //处理关联父视图的cmd状态
            var pv = view.getParent();
            if (pv) {
                view.syncCmdState(pv, false);
            }
            //递归关联子视图
            if (recursion) {
                var children = view.getChildren();
                var len = children.length;
                if (len > 0) {
                    for (var i = 0; i < len; i++) {
                        var childView = children[i];
                        //非活动的不处理
                        var tabpanel = childView.getControl().up('tabpanel');
                        if (tabpanel) {
                            var curTabTitle = tabpanel.getActiveTab().title;
                            var isActive = curTabTitle == childView.label;
                            if (isActive) {
                                childView.isReady = true;
                                view.syncCmdState(childView);
                            }
                        }
                    }
                }
            }
        }
    },

    /**
     * 执行命令
     * @param opt
     * @param scope
     */
    execute: function (opt, scope) {
        var me = scope || this;
        opt = opt || {};
        if (Ext.isFunction(opt)) {
            opt = {
                callback: opt
            };
        }
        opt = Ext.apply({
            withChildren: false,
            model: me._meta.model
        }, opt);

        var indata = {};
        indata.Type = opt.model;

        if (opt.data) { //支持命令自己传值
            indata.Data = opt.data;
        } else {
            opt._changeSetData = this.serializeData(opt.withChildren);
            if (!opt._changeSetData.isEmpty()) {
                var submitData = opt._changeSetData.getSubmitData();
                indata.Data = submitData;
            }
        }

        if (opt.withIds) {
            indata.SelectedIds = opt.selectIds;
        }

        if (indata.Data || indata.SelectedIds) {
            var outerCallback = opt.callback;
            opt.callback = function (res) {
                //内部处理
                if (outerCallback) {
                    outerCallback(res);
                }
            };
            /* 对外支持3种方式的回调
            view.execute({
                success:function(res){
                    console.log('结果成功的回调');
                },
                error: function(res){
                    console.log('结果失败的回调');
                },
                callback: function(res){
                    console.log('自定义的回调');
                }
            });
            */
            indata.Data = Ext.encode(indata.Data);
            if (typeof (opt.async) == "undefined") opt.async = true;
            SIE.invokeCommand({
                token: opt.token || me.getToken(),
                cmd: opt.command || (me.getSourceCmd() ? me.getSourceCmd().command : null),
                async: opt.async,
                data: indata,
                timeout: opt.timeout,
                callback: function (res) {
                    if (res.Success && opt.success)
                        opt.success(res);
                    if (!res.Success) {
                        if (opt.error)
                            opt.error(res);
                        SIE.Msg.showError(res.Message);

                    }
                    opt.callback(res);
                }
            });
        } else {
            SIE.Msg.showWarning('没有可提交的数据，请检查!'.t());
        }
    },
    canFormSave: function () {
        /// <summary>
        ///子表单编辑是否直接保存
        /// </summary>
        var SaveMode = SIE.MetaModel.View.FormChildSaveMode;
        var parent = this._parent;
        if (parent != null && parent._meta.saveMode != SaveMode.None.text)
            return parent._meta.saveMode === SaveMode.Save.text;
        if (parent == null) {
            if (this._meta.saveMode === SaveMode.None.text)
                return this.defaultSaveMode();
            return this._meta.saveMode === SaveMode.Save.text;
        }
        return parent.canFormSave();
    },
    defaultSaveMode: function () {
        /// <summary>
        /// 虚方法 子类可重写
        /// </summary>
        return false;
    },
    isImmediate: function () {
        /// <summary>
        /// (web新加)是否立即触发执行后台请求
        /// </summary>
        var editMode = this.editMode;
        if (editMode === SIE.viewMeta.editMode.FORM && this.canFormSave())
            return true;
        return false;
    },
    getAssociateProperty: function () {
        /// <summary>
        /// 获取关联属性
        /// </summary>
        /// <returns type="string"></returns>
        var me = this;
        return (me._childProperty || ((me._associatedProperty || me.model)));
    },
    getAssociateKey: function () {
        /// <summary>
        /// 获取关联属性Key
        /// </summary>
        /// <returns type="string"></returns>
        var me = this;
        return me.getAssociateProperty() + '_' + me._id;
    },
    getAssociateStoreKey: function () {
        /// <summary>
        /// 获取关联StoreKey(为了兼容关联子实体必须加‘_’才能正常加载)
        /// </summary>
        /// <returns type="string"></returns>
        var me = this;
        return me._childProperty ? "_" + me._childProperty : me.getAssociateKey();
    },
    //-------------------------------------  Readonly -------------------------------------
    _isReadonly: false,
    getIsReadonly: function () {
        return this._isReadonly;
    },
    setIsReadonly: function (value) {
        if (this._isReadonly != value) {
            this._onIsReadonlyChanged(value);
            this._isReadonly = value;
        }
    },
    _onIsReadonlyChanged: function (value) {
        /// <summary>
        /// protected virtual
        /// IsReadonly 属性变化时调用此方法。
        /// </summary>
        /// <param name="value"></param>
    },

    //-------------------------------------  DTO -------------------------------------
    serializeData: function (withChildren) {
        /// <summary>
        /// 把当前视图中的数据转换为可以直接序列化并提交到服务端的 json 对象
        /// </summary>
        /// <param name="withChildren">
        /// 默认为: false, 是否需要转换聚合子视图中的数据。
        /// </param>
        /// <returns></returns>

        var data = this.getData();

        var dto = SIE.data.Serializer.serialize(data, !!withChildren, this);

        return dto;
    },
    _getPIdField: function (entity) {
        /// <summary>
        /// protected
        /// 获取树型父 Id 对应的字符。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns type="Ext.data.Field" />
        var fields = SIE.data.Utils.getPersistFields(this._meta.model);
        var pIdField = SIE.first(fields, function (f) {
            return f.name == "TreePId" || f.name == "PId";
        });
        return pIdField;
    },

    //获取lambda函数
    getFunc: function (lambda, out) {
        var exp = lambda.replace(' ', '').replace(/False/g, 'false').replace(/True/g, 'true').replace(/,Int32/g, '').replace(/,Nullable`1/g, '');
        var param = exp.split('=>')[0].replace('(', '').replace(')', '');
        //逻辑关系
        exp = exp.replace(new RegExp('AndAlsoNot', 'g'), '&& !')
            .replace(new RegExp('OrElseNot', 'g'), '|| !')
            .replace(new RegExp('\\)AndAlso\\(', 'g'), ')&&(')
            .replace(new RegExp('\\)OrElse\\(', 'g'), ')||(')
            .replace(new RegExp('Not\\(', 'g'), '!(');

        if (exp.indexOf(".PersistenceStatus") > -1) {
            exp = exp.replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)==0'.format(param), 'g'), '({0}.phantom==false && {0}.dirty==false)'.format(param))
                .replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)!=0'.format(param), 'g'), '({0}.phantom==true || ({0}.phantom==false && {0}.dirty==true))'.format(param))
                .replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)==1'.format(param), 'g'), '({0}.phantom==false)'.format(param))
                .replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)!=1'.format(param), 'g'), '({0}.phantom==true)'.format(param))
                .replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)==2'.format(param), 'g'), '({0}.phantom==true)'.format(param))
                .replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)!=2'.format(param), 'g'), '({0}.phantom==false)'.format(param))
                .replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)==3'.format(param), 'g'), '({0}.phantom==false && {0}.dirty==true)'.format(param))
                .replace(new RegExp('\\({0}\\.data.PersistenceStatus\\)!=3'.format(param), 'g'), '(true)'.format(param));
        } else {
            //返回是否带关联
            if (out)
                out.hasRel = exp.indexOf(param + '.') > 0;
        }
        //lambda表达式IE11兼容处理
        exp = '(function({0}){return {1}})'.format(param, exp.split('=>')[1]);
        return eval(exp);
    },
    setDefaultValue: function (entity) {
        if (this.gridConfig) {
            var defaultValues = this.gridConfig.columns.forEach(
                function (item, index) {
                    if (item.value) {
                        var json = Ext.JSON.decode(item.value, true);
                        if (json !== null && json.Id) {
                            entity.set(item.dataIndex + '_Display', json.DisPlay);
                            entity.set(item.dataIndex, json.Id);
                        } else {
                            entity.set(item.dataIndex, item.value);
                        }
                    }
                }
            );
        }
    },
    //--------------------------------

    /**
     * 关闭页签前的数据处理
     * 客制化页面关闭需要自己重写事件，data为具体的操作数据（如listview的data为this.getControl().getStore()，detailView的data为this.getCurrent()，this为当前操作的view）；hasData为是否为脏数据(为脏设置为true)
     * 关闭的验证data可以不处理，只处理hasData（是否为脏数据）
     */
    closeView: function () {
        var returnObj = {
            data: null,
            hasData: null,
            callback: function (view) {
                if (!view.hasListeners['dataCallback']) {
                    view.mon(view, 'dataCallback', view.dataCallback);
                }
                view.fireEvent('dataCallback', this.data);
            }
        };
        if (!this.hasListeners['beforeclosewin']) {
            this.mon(this, 'beforeClosewin', this.beforeClosewin);
        }
        this.fireEvent('beforeClosewin', returnObj);
        return returnObj;
    },
    /**
     * 关闭tab前要实现的数据验证
     * @param {} returnObj 
     * @returns {} 
     */
    beforeClosewin: function (returnObj) {
        return null;
    },

    /**
     * 刷新界面
     * @param {any} recordId
     */
    refreshData: function (recordId) {

    },

    entityCopyAfterEventName: 'entityCopyAfter',
    onEntityCopyReady: function () {
        var isChildLoad = false;
        if (this.hasListeners.entitycopyafter) {
            isChildLoad = this.isLoadChildData();
        }
        if (isChildLoad) {
            this.fireEvent(this.entityCopyAfterEventName, this);
            this.mun(this, this.entityCopyAfterEventName);
        }
    },
    isLoadChildData: function () {
        var isChildLoad = true;
        for (var i = 0; i < this._children.length; i++) {
            var view = this._children[i];
            if (view._children.length > 0)
                view.isLoadChildData();
            else {
                if (view.getData() && view.getData().isStore && !view.getData()._loaded)
                    isChildLoad = false;
                else if (view.getData() && view.getData().isEntity && view.getData().store && !view.getData().store.loaded)
                    isChildLoad = false;
                else if (!view.getData())
                    isChildLoad = false;
            }
            if (!isChildLoad)
                break;
        }
        return isChildLoad;
    },

    //--end
});
/// <reference path="../_reference.js" />
Ext.define('SIE.view.DetailView', {
    extend: 'SIE.view.View',
    isDetailView: true,
    _curPid: null,
    getData: function () {
        return this.getCurrent();
    },
    setData: function (value) {
        if (value === null && this.getParent && !this.getParent().isDetailView) 
            this.getControl().setDisabled(true);
        this.setCurrent(value);
    },
    _onCurrentChanged: function (oldValue, entity) {
        this.callParent(arguments);
        var vm = this.getControl().getViewModel();
        SIE.setVmData(vm, entity);
        this._onDataChanged(entity);
        this._RegisterEvents(entity);
        this._attachDynamicVisibility(entity);
    },

    //protected override
    _onIsReadonlyChanged: function (value) {
        var me = this;
        me.setControlReadOnly(value);
    },

    _RegisterEvents: function (value) {
        if (value) {
            var entity = value;
            if (entity) {
                this.mon(entity, 'onpropertySet', this._syncCmdOnPropertyChanged, this);
                this.mon(entity, 'onpropertySet', this._formPropertyChange, this);
            }
            if (entity.store) {
                this.mon(entity.store, 'onpropertySet', this._syncCmdOnPropertyChanged, this, {
                    scope: this,
                    single: true
                });
            }
        }
    },
    /**
     * 属性变更后更新命令状态
     * @param e
     */
    _syncCmdOnPropertyChanged: function (e) {
        if (e && e.property && e.property.length > 0) {
            if (e.property.indexOf('_Display') >= 0) {
                var data = e.entity.getData();
                if (data) {
                    if (data.hasOwnProperty(e.property.replace(/_Display/, ''))) {
                        return;
                    }
                }
            }
        }

        var me = this;
        var view = this;
        var pv = me.getParent();
        var recursion = pv != null || view.getChildren().length > 0;
        if (pv) {
            view = pv;
        }
        me.syncCmdState(view, recursion);
    },
    updateControl: function () {
        /// <summary>
        /// 使用当前使用的值来更新界面上的表单
        /// </summary>
        var c = this.getCurrent();
        if (c) {
            this.getControl().loadRecord(c);
        }
    },

    findEditor: function (property) {
        /// <summary>
        /// 根据属性名称来查找对象的 ext field 对象
        /// </summary>
        /// <param name="property">属性名称</param>

        var editors = this.getControl().items;
        for (var i = 0; i < editors.getCount(); i++) {
            var editor = editors.getAt(i);
            if (editor.name == property) {
                return editor;
            }
        }

        return null;
    },

    //------------------------------------- Dynamic Visibility -------------------------------------
    _dve: null,
    _attachDynamicVisibility: function (curEntity) {
        /// <summary>
        /// 根据当前对象来处理它的动态可见性属性。
        /// </summary>
        /// <param name="curEntity"></param>
        var me = this;
        if (curEntity != null) {
            var dve = this._getDynamicVisibleEditors();
            if (dve.length) {
                this._initDynamicVisibility();

                curEntity.on('propertyChanged', function (e) {
                    //由于没有把事件从之前的 current 对象上移除，所以这里需要对事件的发起者进行过滤。
                    if (e.entity == me.getCurrent()) {
                        for (var i = 0; i < dve.length; i++) {
                            var de = dve[i];
                            if (de.visibilityIndicator == e.property) {
                                var editor = me.findEditor(de.property);
                                editor.setVisible(e.value);

                                return false;
                            }
                        }
                    }
                });
            }
        }
    },

    /**
     * 重写此方法实现明细视图主实体刷新后执行相关逻辑
     * @param {any} data
     */
    onReloadData: function (data) {

    },

    /**
     * 刷新界面
     * @param {any} recordId
     */
    refreshData: function (recordId) {
        var me = this;
        if (me.getCurrent().getId() != recordId)
            return;
        SIE.Ajax({
            url: '/api/DataPortal/QueryEntity',
            method: 'POST',
            async: true,
            params: {
                type: me.model,
                viewGroup: me.viewGroup,
                id: recordId,
                token: me.token
            },
            success: function (res) {
                var result = JSON.parse(res.responseText);
                if (result.Success) {
                    if (result.Result) {
                        Ext.create(me.model, result.Result);
                        Ext.merge(me.getCurrent().data, result.Result);
                        me.getChildren().forEach(function (v) {
                            //to 不知道这些啥逻辑，都已经取子的界面对像作循环应该直接loadChildData 先注释
                            //if (v.reloadData)
                            //    v.reloadData();

                            //if (v.loadChildData) {
                            //    v.loadChildData()
                            //}
                            v.loadChildData(true);
                        });
                        me.getCurrent().markSaved();
                        me.syncCmdState();
                        me.onReloadData(me.getCurrent().data);
                    } else {
                        SIE.Msg.showWarning("没有找到记录id[{0}]".L10N().format(recordId));
                    }
                } else {
                    SIE.Msg.showError(result.Message);
                }
            }
        });
    },
    /**
   * detailView实现 通过当前子视图对应父视图的实体对象，加载它对应的数据。
   * @param opType
   */
    loadChildData: function (opType) {
        var me = this;
        //非当前活动页签不进行数据加载
        var tabPanel = me.getControl().up("tabpanel", 2);
        if (!opType && tabPanel) {
            if (tabPanel.getActiveTab().title != me.label) return;
        }

        var parent = me._parent;
        if (!opType) {
            if (parent.gridConfig) {
                var selectedItem = parent.getSelection()[0];
                var curSelectedId;
                if (selectedItem) {
                    curSelectedId = parent.getSelection()[0].id;
                }
                if (me._curPid && curSelectedId && me._curPid == curSelectedId) {
                    return;
                } else if (curSelectedId) {
                    me._curPid = curSelectedId;
                }
            } else if (parent.formConfig) {
                var selectedItem = parent.getData();
                var curSelectedId;
                if (selectedItem) {
                    curSelectedId = parent.getData().id;
                }
                if (me._curPid && curSelectedId && me._curPid == curSelectedId) {
                    return;
                } else if (curSelectedId) {
                    me._curPid = curSelectedId;
                }
            }
        }
        if (parent) {
            var form = me.getControl();
            var parentEntity = parent.getCurrent();
            if (parentEntity) {
                parentEntity.belongsView = parent;
                var key = me.getAssociateKey();
                var store = parentEntity[key];
                if (!store) {
                    store = SIE.data.Utils.createStore({
                        model: me.model,
                        storeConfig: {
                            proxy: Ext.clone(SIE.getModel(me.model).proxyConfig)
                        },
                        remoteSort: true
                    });
                    var newEntity = new me._model();
                    me._setDefaultValue(newEntity);
                    store.add(newEntity);
                    parentEntity[key] = store;
                }
                if (store.loaded) {
                    var record = store.getAt(0);
                    me.setData(record);
                    record.fireEvent('onpropertySet', { //me.setData(record)时不会触发表单属性就更事件_formPropertyChange
                        entity: record
                    });
                    me._parent.onEntityCopyReady();
                    return;
                }
                // else {
                //     var record = store.getAt(0);
                //     me.setData(record);
                // }

                store.mon(store, 'load', function () {
                    me.fireEvent('ondataloaded');
                });
                var proxy = store.proxy;
                proxy.setExtraParams({});
                proxy.setExtraParam("token", me.getToken());
                proxy.setExtraParam("type", me.model);
                proxy.setExtraParam("viewGroup", me.viewGroup);
                proxy.setExtraParam("action", "delegate");
                proxy.setExtraParam("parent", parent.model);
                proxy.setExtraParam("filter", Ext.encode([{
                    property: SIE._KeyPropertyName,
                    value: parent._current.data[SIE._KeyPropertyName],
                    exactMatch: true
                }]));
                //在父实体在 Id 是正数时，才表示父对象已经在服务器端有数据了，这时进行加载。
                if (parentEntity.getId() !== 0 && parentEntity.phantom === false) {
                    form.setLoading(true);
                    var record = store.getAt(0);
                    me.setData(record);//因为store.load为异步,所以要先对表单设置数据源
                    store.load({
                        callback: function (records, operation, success) {
                            store.loaded = success;
                            var record = store.getAt(0);
                            if (!record) {
                                record = new me._model();
                                me._setDefaultValue(record);
                                store.add(record);
                            }
                            if (record.data.Id === 0)//如果服务器直接返回id为0的对象，则认为是新建的
                                record.phantom = true;
                            me.setData(record);
                            me._parent.onEntityCopyReady();
                            record.fireEvent('onpropertySet', { //me.setData(record)时不会触发表单控件属性变更事件_formPropertyChange
                                entity: record
                            });
                            form.setLoading(false);
                            me.fireEvent('ondataloaded');
                        }
                    });
                } else {
                    var record = store.getAt(0);
                    me.setData(record);
                    me.fireEvent('ondataloaded');
                }

                return;
            }
        }
        //如果没有新的数据源，则需要把旧的数据源清空。
        me.setData(null);
    },
    _initDynamicVisibility: function () {
        var dve = this._getDynamicVisibleEditors();
        if (dve.length) {
            //由于没有把事件从之前的 current 对象上移除，所以这里需要对事件的发起者进行过滤。
            var entity = this.getCurrent();

            for (var i = 0; i < dve.length; i++) {
                var de = dve[i];
                var value = entity.get(de.visibilityIndicator);
                this.findEditor(de.property).setVisible(value);
            }
        }
    },
    _getDynamicVisibleEditors: function () {
        /// <summary>
        /// 获取一个动态可见性的属性列表，每一个元素有两个属性：property，visibilityIndicator，
        /// 表示某个属性应该根据另外一个属性来动态改变它的可见性。
        /// </summary>
        /// <returns></returns>
        if (!this._dve) {
            var a = [];

            var items = this.getMeta().formConfig.items;
            SIE.each(items, function (i) {
                if (i.visibilityIndicator) {
                    a.push({
                        property: i.name,
                        visibilityIndicator: i.visibilityIndicator
                    });
                }
            });

            this._dve = a;
        }

        return this._dve;
    },
    _setControl: function (value) {
        this.callParent(arguments);

        //由于要支持 DynamicVisibility，所以让所有的 checkBox 支持及时反馈数据到实体。
        if (value) {
            var me = this;
            SIE.each(value.items, function (field) {
                //!!!以下代码参考：Ext.Checkbox.onBoxClick。
                if (field.isCheckbox && !field.disabled && !field.readOnly) {
                    field.on('render', function () {
                        field.getEl().on('click', function () {
                            var e = me.getCurrent();
                            if (e) {
                                var n = field.getName();
                                var v = field.getValue();
                                e.set(n, v);
                            }
                        });
                    });
                }
            });
        }
    },
    setControlReadOnly: function (readonly) {
        ///设置控件是否允许编辑
        var items = this.getControl().items;
        if (items) {
            var filed = items.items;
            Ext.each(filed, function (field) {
                if (field.setReadOnly)
                    field.setReadOnly(readonly);
            });
        }
    },

    _setDefaultValue: function (entity) {
        //新增时设置默认值,复制新增不设置
        if (entity.phantom && !entity.isCopy) {
            if (this.formConfig.items) {
                Ext.Array.forEach(this.formConfig.items, function (item, index) {
                    if (item.value) {
                        var json = Ext.JSON.decode(item.value, true);
                        if (json !== null && json.id) {
                            entity.set(item.name + '_Display', json.DisPlay);
                            entity.set(item.name, json.Id);
                        } else {
                            entity.set(item.name, item.value);
                        }
                    }
                });
            }
        }
    },
    //表单布局特殊处理
    _specialFormLayout: function (formCfg) {
        var layout = formCfg.layout;
        if (layout) {
            switch (layout.type) {
                case "table":
                    var tableAttrs = {
                        style: {
                            width: '100%'
                        }
                    };
                    layout.tableAttrs = tableAttrs;
                    break;
            }
        }
    },

    //表单域特殊处理
    formSpecialHandle: function (formCfg, entity) {
        var me = this;
        if (formCfg.items) {
            var items = formCfg.items;
            Ext.Array.forEach(formCfg.items, function (item, index) {
                if (!item.width)
                    item.width = '100%';

                if (item.xtype == 'panel') {
                    Ext.Array.forEach(item.items, function (i, n) {
                        me._formSelfAdaption(i, true);
                        me._formfieldControl(i, entity);
                    });
                } else {
                    me._formSelfAdaption(item);
                    me._formfieldControl(item, entity);
                }
            });
        }
    },

    //表单域自适应
    _formSelfAdaption: function (item, isAdaption) {
        var colspan = 1;
        if (item.colspan) {
            colspan = item.colspan;
        }
        item.maxWidth = 425 * colspan; //计算列宽
        item.minWidth = 185;
        if (isAdaption && !item.width)
            item.width = '100%';
    },

    //表单域控制
    _formfieldControl: function (item, entity) {
        var me = this;
        if (item.readonlyLambda && item.readonlyLambda != "") {
            var out = {};
            var func = me.getFunc(item.readonlyLambda, out);
            if (!out.hasRel || entity) {
                item.readOnly = entity ? func(entity) : false;
            }
            if (out.hasRel) {
                me.addProChgHandler({
                    pro: item.name,
                    effect: 'setReadOnly',
                    lambda: func
                });
            }
        }

        if (item.visibleLambda && item.visibleLambda != "") {
            var out = {};
            var func = me.getFunc(item.visibleLambda, out);
            if (!out.hasRel || entity) {
                var isShow = entity ? func(entity) : true;
                if (!isShow)
                    delete item;
            }
            if (out.hasRel) {
                me.addProChgHandler({
                    pro: item.name,
                    effect: 'hide',
                    lambda: func
                });
            }
        }

        if (item.cascade && item.cascade.length > 0) {
            item.cascade.forEach(function (e, i, arr) {
                var func = me.getFunc(e);
                me.addProChgHandler({
                    pro: item.name,
                    effect: 'cascade',
                    lambda: func
                });
            });
        }
    },

    //属性变更事件注册
    _formPropertyChange: function (arg) {
        var me = this;
        var entity = arg.entity;
        var handlers = me.getProChgHandlers();
        var container = me.getControl();

        if (!handlers || !Array.isArray(handlers) || container.getForm().monitor == null)
            return;

        handlers.forEach(function (handler, i, arr) {
            var field;
            var search = container.query('fieldcontainer[name="' + handler.pro + '"]:first');
            if (search.length === 0)
                search = container.query('textfield[name="' + handler.pro + '"]:first');
            field = search.length > 0 ? search[0] : container.getForm().findField(handler.pro);
            if (!field) {
                //3个范围编辑器先特殊处理
                var ranges = container.query('textRange,spinRange,dateRange');
                if (ranges && ranges.length > 0) {
                    for (var i = 0; i < ranges.length; i++) {
                        var range = ranges[i];
                        if (range.name == handler.pro) {
                            field = range;
                            break;
                        }
                    }
                }
                if (!field) {
                    return;
                }
            }

            switch (handler.effect) {
                case 'setReadOnly':
                    field[handler.effect](handler.lambda(arg.entity));
                    break;
                case 'hide':
                    if (handler.lambda(entity) == true)
                        field.show();
                    else
                        field.hide();
                    break;
                case 'cascade':
                    if (handler.pro == arg.property && !(arg.entity.getData()[arg.property] == arg.oldvalue)) {
                        handler.lambda(entity);
                    }
                    break;
            }
        });
    },
    //表单修改命令  -----todo
    startEdit: function (entity, rowIdx, colIdx) {

    },

    /**
     * 验证数据
     * @returns {} 
     */
    validateData: function () {
        return this.getControl().getForm().isValid();
    },

    beforeClosewin: function (returnObj) {
        var data = this.getData();
        if (data) {
            var changeData = SIE.data.Serializer.serialize(data, true);
            if (changeData._data) {
                var hasData = false;
                for (var pro in changeData._data) {
                    if (pro == "_isEntityHost") continue;
                    hasData = true;
                    break;
                }
                returnObj.data = data;
                returnObj.hasData = hasData;
            }
        }
        return returnObj;
    },
    /**
     * 用于数据不保存时，撤销在表单的修改
     * @param {} data 
     * @returns {} 
     */
    dataCallback: function (data) {
        if (data && data.rejects) {
            data.rejects();
        }
    },

    //--end

});

/// <reference path="../_reference.js" />

Ext.define('SIE.view.ListView', {
    extend: 'SIE.view.View',
    //private
    _treeStoreInited: false,
    //internal
    _pagingBar: null,
    isListView: true,
    _curPid: null,
    /**
     * 构造函数
     * @param {Object} meta-元数据对象
     */
    constructor: function (meta) {
        this.callParent(arguments); //会将参数的(gridconfig)值apply赋给this
    },
    /**
     * 获取ListView的数据仓库
     * @returns {Ext.data.Store} 
     */
    getData: function () {
        return this.getControl().getStore();
    },
    /**
     * 设置ListView的数据仓库
     * @param {Ext.data.Store} value-数据仓库对象
     */
    setData: function (value) {
        var me = this;
        var control = me.getControl();
        var data = control.getStore();
        //内存排序1（gc）
        var remoteSort = true;
        remoteSort = data.config.remoteSort;
        if (data != value) {
            //当传入的数据集为空引用时，需要创建一个新的空数据集。
            if (value == null) {
                var model = this.getModel();
                value = SIE.data.Utils.createStore({
                    model: model,
                    remoteSort: remoteSort
                });
            } else {
                //TODO:内存排序2(gc)
                remoteSort = value.config.remoteSort;
                value.setRemoteSort(remoteSort);
            }
            var g = data.getGrouper();
            if (g) {
                value.group(g.property, g.direction);
            }

            //绑定到控件及分页控件上。
            control.reconfigure(value);
            if (me._pagingBar) {
                me._pagingBar.bindStore(value);
            }

            me._onDataChanged(value);

            me.setSelection(null);
        }
    },

    /**
     * 设置ListView的显示控件(grid)
     * @param {Ext.grid.Panel } value-grid控件
     */
    _setControl: function (value) {
        /// <summary>
        /// internal override
        /// 设置视图对应的控件。
        /// </summary>
        /// <param name="value"></param>
        this.callParent(arguments);
        this._RegisterEvents(value);
        this.setControlReadOnly();
    },
    /**
     * 设置grid控件是否允许编辑
     * @param {Bool} readonly-为true时，不可编辑
     */
    setControlReadOnly: function (readonly) {
        if (Ext.isEmpty(readonly)) {
            readonly = this.editMode === SIE.viewMeta.editMode.FORM;
        }
        var ctl = this.getControl();
        ctl.allowEdit = !readonly;
        this.gridConfig.allowEdit = ctl.allowEdit;
    },
    /**
     * 给控件注册事件
     * @param {Ext.grid.Panel} gridControl-grid控件
     */
    _RegisterEvents: function (gridControl) {
        if (gridControl !== null) {
            var selectModel = gridControl.getSelectionModel();
            if (selectModel) {
                //选中项时触发事件
                selectModel.mon(selectModel, 'selectionchange', this._onControlSelectionChanged, this);
            }
            ////编辑前事件
            gridControl.mon(gridControl, 'beforeedit', function (editor, context, eOpts) {
                if (!this.allowEdit) return false;
                var entity = context.record;
                if (entity && entity.store) {
                    entity.store.on({
                        propertyChanged: { fn: this.SIEView._onEntityPropertyChanged, scope: this, single: true },
                        onpropertySet: { fn: this.SIEView._onEntityPropertyChanged, scope: this, single: true }
                    });
                }
                ///实体为树形，entity.store未定义，不触发属性变更事件，此处增加一个treeStore的属性变更datachanged事件
                if (context.store.$className == "Ext.data.TreeStore") {
                    context.store.on({
                        datachanged: { fn: this.SIEView._onEntityPropertyChanged, scope: this, single: true }
                    });
                }
            });
            //列表单元格编辑后事件
            gridControl.mon(gridControl, 'validateedit', function (editor, e) {
                // commit the changes right after editing finished
                //此处禁用ext编辑单元格后，自动排序效果
                var store = e.grid.getStore();
                if (store.sorters && store.sorters.length > 0) store.sorters.removeAll();
                //revertInvalid只需设置一次，但在未进入编辑时getEditor()为空不能设置revertInvalid，固暂时放这里
                var column = e.column;
                //editor.grid.SIEView.getProChgHandlers().forEach(function (item) {
                //    if (item.pro === column.dataIndex&& item.effect=== "setRevertInvalid") {
                //        column.getEditor().up().revertInvalid = !item.lambda;
                //    }
                //});
            });
        }
    },

    /**
     * protected override
     * 加载当前子视图的数据,根据对应父视图的实体对象(listView实现) 
     * @param {Bool} opType-是否强制加载
     */
    loadChildData: function (opType) {
        var me = this;
        //非当前活动页签不进行数据加载
        var tabPanel = me.getControl().up("tabpanel", 2);
        if (!opType && tabPanel) {  //opType为true时，强制加载所有子视图的数据
            if (tabPanel.getActiveTab().title != me.label) {
                me._curPid = ""; //不是活动页签把_curPid清掉，不然会出现不加载数据现象
                return;
            }
        }
        var parent = me._parent;
        //如果数据已加载则不重新加载
        if (!opType) {
            if (parent.gridConfig) {
                var selectedItem = parent.getCurrent();
                var curSelectedId;
                if (selectedItem) {
                    curSelectedId = selectedItem.id;
                }
                if (me._curPid && curSelectedId && me._curPid == curSelectedId) {
                    return;
                } else if (curSelectedId) {
                    me._curPid = curSelectedId;
                } else {
                    me._curPid = "";
                }
            } else if (parent.formConfig) {
                var selectedItem = parent.getData();
                var curSelectedId;
                if (selectedItem) {
                    curSelectedId = parent.getData().id;
                }
                if (me._curPid && curSelectedId && me._curPid == curSelectedId) {
                    return;
                } else if (curSelectedId) {
                    me._curPid = curSelectedId;
                }
            }
        }
        if (parent) {
            var parentEntity = parent.getCurrent();
            if (parentEntity) {
                if (!parentEntity.belongsView)
                    parentEntity.belongsView = parent;
                var pName = me._childProperty;
                var store;
                if (pName)
                    store = parentEntity[pName]();
                else {
                    var key = me.getAssociateKey();
                    store = parentEntity[key];
                    //将父视图remoteSort赋值给子视图，让页面排序方式保持一致
                    var remoteSort= parent.config.storeConfig ? parent.config.storeConfig.remoteSort : (me.config.storeConfig ? me.config.storeConfig.remoteSort : true);
                    if (!store) {
                        store = SIE.data.Utils.createStore({
                            model: me.model,
                            remoteSort: remoteSort,
                            storeConfig: {
                                proxy: Ext.clone(SIE.getModel(me.model).proxyConfig),
                                remoteSort: remoteSort
                            }
                        });

                        parentEntity[key] = store;
                    }
                }
                //store 排序
                SIE.data.Utils.gridStoreSort(store, remoteSort);
                store.mon(store, 'load', function () {
                    me.fireEvent('ondataloaded');
                });
                var proxy = store.proxy;
                store.proxy.abort();//读取子列表前，取消当前子列表的所有请求，防止因网速慢导致子列表存在多个请求
                proxy.setExtraParams({});
                proxy.setExtraParam("token", me.getToken());
                proxy.setExtraParam("type", me.model);
                proxy.setExtraParam("viewGroup", me.viewGroup);

                //非直接关联的子属性
                if (!pName) {
                    proxy.setExtraParam("action", "delegate");
                    proxy.setExtraParam("parent", parent.model);
                    proxy.setExtraParam("filter", Ext.encode([
                        {
                            property: SIE._KeyPropertyName,
                            value: parent._current.data[SIE._KeyPropertyName],
                            exactMatch: true
                        }]));
                }
                else {
                    proxy.setExtraParam("action", "entity");
                    proxy.setExtraParam("filter", store.config.filters == null ? Ext.encode([
                        {
                            property: SIE._KeyPropertyName,
                            value: parent._current.data[SIE._KeyPropertyName],
                            exactMatch: true
                        }]) : Ext.encode(store.config.filters));
                }
                //加载子属性时请求的树形实体数据没有leaf属性，导致树形展开时找不到子节点，一直执行展开方法向后台请求数据，陷入死循环
                if (store.$className == "Ext.data.TreeStore") {
                    proxy.extractResponseData = function (response) {
                        var responseObj = response.responseJson;
                        if (!responseObj && response.responseText)
                            responseObj = Ext.decode(response.responseText);
                        if (responseObj.Success) {
                            var entities = responseObj.Result.entities;
                            Ext.each(entities, function (o) {
                                o.leaf = true;
                                Ext.each(entities, function (x) {
                                    if (o.Id == x.TreePId) {
                                        o.leaf = false;
                                        return false;
                                    }

                                });
                            });
                            return responseObj.Result;
                        }
                        return response;
                    };
                }

                //直接变更数据源。这时会发生 dataChanged 事件。
                //但是如果是第一次加载，那么此时数据源中还没有数据，需要等待异步加载完成后，数据才到达客户端。
                this.setData(store);

                //对于切换选项卡的，不重复加载数据
                if (store._loaded && !opType) {
                    return;
                }
                //在父实体在 Id 是正数时，才表示父对象已经在服务器端有数据了，这时进行加载。
                //父实体是ViewModel时，加载数据必定是走delegate
                if ((parentEntity.getId() > 0 || proxy.extraParams.action === "delegate") && parentEntity.phantom === false) {
                    if (me.getParent().getCurrent())
                        proxy.setExtraParam("parentEntity", Ext.encode(me.getParent().getCurrent().data));
                    // ajax
                    store.load({
                        scope: this,
                        callback: function (records, operation, success) {
                            store._loaded = success;
                            me._parent.onEntityCopyReady();

                        }
                    });
                }
                return;
            }
        }

        //如果没有新的数据源，则需要把旧的数据源清空。
        me.setData(null);
    },


    /**
     * 实体变更事件
     * @param {type} e
     */
    _onEntityPropertyChanged: function (e) {
        //实体属性变更事件
        this.SIEView.syncCmdState();
    },
    /**
     * 控件选择变更事件
     * @param {Ext.grid.panel} grid-控件
     * @param {Ext.grid.selection.Selection} selection-选中的集合
     * @param {Object} eOpts-事件内部对象
     */
    _onControlSelectionChanged: function (grid, selection, eOpts) {
        this._OnControlStarted = true;
        this.setSelection(selection);
        this.getChildren().forEach(function (item) {
            if (item.isDetailView && selection.length > 0)
                item.getControl().setDisabled(false);
        });
        delete this._OnControlStarted;
    },

    _onControlFocuschange: function (o, oldFocused, newFocused) {
        if (newFocused != null && newFocused != this._current) {
            this._OnControlStarted = true;
            this.setCurrent(newFocused);
            delete this._OnControlStarted;
        }
    },
    /**
     * protected override
     * 选中实体变更事件
     * @param {Ext.data.model} oldValue-旧数据
     * @param {Ext.data.mode} value-新数据
     */
    _onCurrentChanged: function (oldValue, value) {
        if (!this._OnControlStarted) {
            var sm = this.getSelectionModel();
            if (value != null) {
                sm.select(value);
            }
            else {
                sm.deselectAll();
            }
        }

        this.callParent(arguments);
    },

    //-------------------------------------  Relations -------------------------------------
    getConditionView: function () {
        return this.findRelationView(SIE.view.RelationView.condition);
    },
    getNavigationView: function () {
        return this.findRelationView(SIE.view.RelationView.navigation);
    },

    //-------------------------------------  Selection -------------------------------------
    _selection: null,
    /**
     * private 获取已选中记录集合
     * @returns {Array} 
     */
    getSelection: function () {
        return this._selection || this.getSelectionModel().getSelection();
    },
    /**
     * private 设置选中记录集合
     * @param {Array} selection-选中记录集合
     */
    setSelection: function (selection) {
        var oldSelection = this.getSelection();
        this._selection = selection;
        this._onSelectionChanged(oldSelection, selection);
    },
    /**
     * private-选中变更函数
     * @param {Array} oldValue-旧记录
     * @param {Array} value-新记录
     */
    _onSelectionChanged: function (oldValue, value) {
        var me = this;
        value = value || [];
        if (value && value.length > 0) {
            var last = value[value.length - 1]; //以选择行最后的为当前对象
            this.setCurrent(last, true);

            var isEquals = Ext.Array.equals(oldValue, value);
            if (!isEquals) { //集合变化事件扩展
                me.fireEvent('selectionChanged', {
                    oldValue: oldValue,
                    newValue: value
                });
            }
        }
        else {
            this.setCurrent(null);
        }

    },
    /**
     * 获取列表控件对应的选择模式
     * @returns {Ext.selection.Model} 
     */
    getSelectionModel: function () {
        return this.getControl().getSelectionModel();
    },
    /**
     * 获取选中记录的Id集合
     * @param {Ext.data.Model[]} items-选中的记录
     * @returns {Array}
     */
    getSelectionIds: function (items) {
        var selectedItems = items || this.getSelection();
        var selectIds = [];
        for (i = 0, len = selectedItems.length; i < len; i++) {
            var item = selectedItems[i];
            selectIds.push(item.data.Id);
        }
        return selectIds;
    },

    /**
     * public- ListEditor中是否有选择的对象
     */
    hasSelectedEntities: function () {
        return !SIE.isEmpty(this.getSelectedEntities()) && this.getSelectedEntities().length > 0;
    },
    /**
     * public- 获取选中的对象集合
     * @returns {Array} 
     */
    getSelectedEntities: function () {
        return this.getSelection();
    },
    /**
     * public-选择实体
     * @param {Ext.data.Model[]} 记录数组
     * @param {Boolean} [keepExisting=false] 如果为True，则保留现有选择, 默认为：false
     */
    selectEntities: function (entities, keepExisting) {
        if (entities) {
            keepExisting = keepExisting || false;
            var control = this.getControl();
            var sm = control.getSelectionModel();
            sm.select(entities, keepExisting);
        }
    },
    /**
     * public-不选择实体
     * @param {Ext.data.Model[]} entities 记录数组
     */
    unSelectEntities: function (entities) {
        if (entities) {
            var control = this.getControl();
            var sm = control.getSelectionModel();
            sm.deselect(entities);
        }
    },

    //-------------------------------------  Readonly -------------------------------------
    /**
     * protected override
     * 设置只读触发事件   
     * @param {type} value
     */
    _onIsReadonlyChanged: function (value) {
        var editing = this.getControl().getPlugin('editing');
        if (value) {
            editing.on('beforeedit', this._readonlyHandler, this);
        }
        else {
            editing.un('beforeedit', this._readonlyHandler, this);
        }
    },
    _readonlyHandler: function (editor, e, opt) {
        e.cancel = true;
    },

    //-------------------------------------  以下是方便 CRUD 的方法 -------------------------------------
    //最后一次使用的数据加载参数
    _lastDataArgs: null,

    _afterReloadDataEventName: 'afterReloadData',
    /**
     * 数据仓库加载数据事件
     */
    afterReloadData: function (args) {
        if (args) {
            if (args.view) {
                var view = args.view;
                var store = view.getData();
                var current = args.entity;
                if (current) {
                    var record = store.findRecord(SIE._KeyPropertyName, current.get(SIE._KeyPropertyName));
                    view.setCurrent(record);
                }
            }
        }
    },
    /**
     * 触发加载数据事件
     */
    fireReloadData: function (view, entity) {
        if (view) {
            if (this.hasListeners[this._afterReloadDataEventName.toLowerCase()]) {
                this.fireEvent(this._afterReloadDataEventName, {
                    view: view, entity: entity
                });
                this.mun(view, this._afterReloadDataEventName);
            }
        }
    },
    /**
     * 重新加载数据事件
     */
    onReloadData: function (view) {
        /** 20190409 此次调整为默认选中第一行，目前记录是根据按修改时间排序，所以没有问题
         * 因为数据分页筛选，在并发量大或者按其他列排序时查询数据库，不能保证当前修改的数据会在第一页中
         */
        if (!this.hasListeners[this._afterReloadDataEventName.toLowerCase()]) {
            this.mon(view, this._afterReloadDataEventName, this.afterReloadData, this, { single: true });
        }
    },

    ///刷新界面数据
    refreshData: function () {
        this.reloadData();
    },

    /**
     * 重载数据
     */
    reloadData: function () {
        /// <summary>
        /// 使用最后一次使用的加载参数，重新加载整个列表的数据。
        /// </summary>
        if (this.getData()) {
            this.getControl().setLoading(true);
            this.loadData(this._lastDataArgs);
            this.onReloadData(this);
            this.getControl().setLoading(false);
        }
    },

    /**
     * 为这个视图异步加载数据
     * @param {Object} args-参数对象
     * method：string； 如果定义了此参数，则使用服务端仓库中对应的方法名来进行查询。
     * async:boolean: 是否异步
     * params：[]；如果定义了 method 参数，则此参数用于指定对应方法的参数列表。参数的顺序必须与服务端定义的参数一致。
     * searchKeyWord:string 自定义数据源(lookup)为查询值.默认查询(entity)为 Or 或 And 的判断
     * criteria: 使用这个参数来进行数据查询。
     * callback: 加载完成后的回调。
     */
    loadData: function (args) {
        var me = this;
        args = args || me._lastDataArgs || {
        };
        if (Ext.isFunction(args)) {
            args = {
                callback: args
            };
        }
        this._lastDataArgs = args;
        var entity = me.getCurrent();
        var store = me.getData();
        var proxy = store.proxy;
        if (args.clearSort)
            store.sorters = null;
        if (this._relations.length > 0) {
            args.criteria = this._relations[0]._target.getData();
        }
        proxy.setExtraParams({});
        proxy.setExtraParam("token", args.token || me.getToken());
        if (!(proxy.extraParams && proxy.extraParams.action)) proxy.setExtraParam("action", args.action || proxy.action || "entity");
        if (args.action) proxy.setExtraParam("action", args.action);
        if (!(proxy.extraParams && proxy.extraParams.type)) proxy.setExtraParam("type", args.type || me.model);
        proxy.setExtraParam("viewGroup", me.viewGroup);
        proxy.setExtraParam("url", args.url || proxy.url);
        if (!(typeof args.async === 'undefined')) {
            proxy.isSynchronous = !args.async
        }
        proxy.setExtraParam("searchKeyWord", args.searchKeyWord || proxy.searchKeyWord);
        if (args.filter || proxy.filter) {
            proxy.setExtraParam("filter", args.filter || proxy.filter);
        }
        if (args.sort)
            proxy.setExtraParam("sort", args.sort);
        if (args.page)
            proxy.setExtraParam("page", args.page);
        if (args.method) {
            SIE.data.Utils.filterByMethod(store, args.method, args.params);
        }
        else if (args.criteria) {
            SIE.data.Utils.filterByCriteria(store, args.criteria);
        }

        /*
          非直接关联的子属性，则需要从代理中获取数据 mod by wuyongbo 20190222
        */
        var parent = me._parent;
        if (parent && parent._current) {
            var pName = me._childProperty;
            if (!pName) {
                proxy.setExtraParam("action", "delegate");
                proxy.setExtraParam("parent", parent.model);
                proxy.setExtraParam("filter", Ext.encode([
                    {
                        property: SIE._KeyPropertyName,
                        value: parent._current.data[SIE._KeyPropertyName],
                        exactMatch: true
                    }]));
            }
        }
        //数据加载过多或网络慢会造成超时问题，默认不开启超时机制
        proxy.timeout = args.timeout || false;
        store.mon(store, 'load', function () {
            me.fireEvent('ondataloaded');
        });
        //加载数据，并清空当前选择项。
        if (me._isTree) {
            Ext.apply(proxy, {
                extractResponseData: function (response) {
                    var responseObj = response.responseJson;
                    if (!responseObj && response.responseText)
                        responseObj = Ext.decode(response.responseText);
                    if (responseObj.Success) {
                        var entities = responseObj.Result.entities;
                        Ext.each(entities, function (o) {
                            o.leaf = true;
                            o.parentId = o.TreePId;
                            Ext.each(entities, function (x) {
                                if (o.Id == x.TreePId) {
                                    o.leaf = false;
                                    return false;
                                }
                            });

                        });
                        return responseObj.Result;
                        //response.responseText = Ext.encode(responseObj.Result);
                    } else {
                        SIE.Msg.showError(responseObj.Message);
                    }
                    return response;
                }
            });

            //由于创建时配置了 root 为已经加载来防止 treePanel 的自动加载数据，
            //所以这里在第一次查询时，需要把该值设置为 false。
            if (!me._treeStoreInited) {
                me._treeStoreInited = true;
                var root = store.getRootNode();
                root.set('loaded', false);
            }
            store.load(
                function (records, operation, success) {
                    me.setCurrent(null, true);
                    store._loaded = success;
                    if (args.callback) args.callback(arguments);
                    delete proxy.getExtraParams().page;
                }
            );
        }
        else {
            store.rejectChanges();
            store.autoLoad = true;
            store.load({
                url: args.url || proxy.url,
                callback: function (records, operation, success) {
                    if (me.getCurrent() != null)
                        me.setCurrent(null, true);
                    else
                        me.syncCmdState();//当前行为空时，需对命令进行一次状态同步
                    store._loaded = success;
                    me.fireReloadData(me, entity);
                    if (args.callback) args.callback(arguments);
                    delete proxy.getExtraParams().page;

                }
            });
        }
    },

    /**
     * 页面控件grid刷新
     */
    refresh: function () {
        var me = this;
        var grid = me.getControl();
        grid.getView().refresh();//刷新值的显示等
    },
    customLoadData: function (args) {
        var me = this;
        var flag = false;
        args = args || me._lastDataArgs || {
        };
        if (Ext.isFunction(args)) {
            args = {
                callback: args
            };
        }
        this._lastDataArgs = args;
        SIE.invokeDataQuery({
            method: args.method,
            params: [args.params],
            action: args.action,
            type: action.type,
            token: view.getToken(),
            success: function (res) {
                var mainView = _view._relations[0]._target;
                var control = mainView.getControl();
                control.setStore(null);
                control.setStore(res.Result);
                flag = true
            }
        });
        return flag;
    },
    //-------------------------------------  Tree Operations -------------------------------------
    expandSelection: function () {
        var me = this;
        if (me._isTree) {
            var s = me.getSelection();
            if (!s.length) {
                s = me._getTreeRootNodes();
            }
            SIE.each(s, function (i) { i.expand(true); });
        }
    },
    collapseSelection: function () {
        var me = this;
        if (me._isTree) {
            var s = me.getSelection();
            if (!s.length) {
                s = me._getTreeRootNodes();
            }
            SIE.each(s, function (i) { i.collapse(true); });
        }
    },
    _getTreeRoot: function () {
        return this.getData().getRootNode();
    },
    _getTreeRootNodes: function () {
        return this._getTreeRoot().childNodes;
    },

    //-------------------------------------  Edit -------------------------------------
    _getEditing: function () {
        var res = this.getControl().getPlugin('editing');
        return res;
    },

    /**
     * 行开始编辑函数
     * @param {Ext.data.record} entity-实体记录
     * @param {Number} rowIdx-行索引号
     * @param {Number} colIdx-列索引号
     */
    startEdit: function (entity, rowIdx, colIdx) {
        /// <summary>
        /// 开始编辑指定实体对应行的指定列。
        /// </summary>
        /// extjs6.0+ API调整了 
        /// 参考http://192.168.175.51/extjs/extjs-660-docs/extjs/6.6.0/classic/Ext.grid.plugin.CellEditing.html#method-startEditByPosition
        /// <param name="entity"></param>
        /// <param name="row">指定行号</param>
        /// <param name="column">指定列号</param>
        var grid = this.getControl();
        var sm = grid.getSelectionModel();
        if (Ext.isEmpty(rowIdx)) {
            var lastRowIdx = 0, lastColIdx = 1;
            var selected = sm.getSelected();
            if (selected) { //选中编辑
                if (sm instanceof Ext.selection.TreeModel || sm instanceof Ext.selection.CheckboxModel) {
                    var nm = grid.getNavigationModel();
                    var postion = nm.lastFocused;
                    lastRowIdx = postion.rowIdx;
                    lastColIdx = postion.colIdx;
                }
                else {
                    if (selected.endCell) {
                        var lastCell = selected.endCell;
                        lastRowIdx = lastCell.rowIdx;
                        lastColIdx = lastCell.colIdx;
                    }
                    else {
                        //选择的是序号列时，编辑列为列1
                        lastRowIdx = selected.getLastRowIndex();
                        lastColIdx = 1;
                    }
                }
            }
            rowIdx = lastRowIdx;
            colIdx = lastColIdx;
        }
        var editPlug = grid.getPlugin('editing');
        if (editPlug && !entity._EditGrid) {
            editPlug.startEditByPosition({ row: rowIdx, column: colIdx });
            sm.select(entity);
        }
        entity._EditGrid = false;
    },

    /**
     * 编辑后
     * 不管是否直接保存都要对store进行操作，否则无数据保存
     * @param m-当前操作实体
     * @param isImmediate-是否直接保存 true直接保存，fales:不直接保存
     * @param isCopy-没用参数(之前用于复制添加) 
     */
    afterEdit: function (m, isImmediate, isCopy) {
        var me = this;
        me.storeAddRecord(me, m);
    },

    locateDefault: function () {
        /// <summary>
        /// 定位到第一行。
        /// </summary> 
        var me = this;
        if (me._isTree) {
            alert("树型的 locateDefault 还没有实现。".t());
        }
        else {
            var list = me.getData();
            if (list.getCount() > 0) {
                var item = list.getAt(0);
                me.setCurrent(item);
            }
        }
    },
    /**
     * 添加新实体并返回
     * @returns {Ext.data.model} 
     */
    addNew: function () {
        /// <summary>
        /// 为当前列表视图添加一个新的对象，同时设置好它的关系。
        /// 在同级添加一个结点
        /// </summary>
        /// <returns type="SIE.data.Entity">返回新加的实体对象</returns>
        var newEntity = null;
        var me = this;
        if (!me._isTree) {
            newEntity = Ext.create(this.model);
            newEntity.generateId();
            this.setDefaultValue(newEntity);
            if (SIE.viewMeta.editMode.INLINE === this.editMode) {
                me.storeAddRecord(me, newEntity);
            }
            else {
                //函数未做任何操作,作用是什么？？
                me.initRefProperties(newEntity);
            }
        }
        else {
            var parent;
            var s = me.getSelection();
            if (s[0]) parent = s[0].parentNode;
            parent = parent || me._getTreeRoot();
            if (parent.isLeaf()) {
                parent.set('leaf', false);
            }
            newEntity = parent.appendChild(me._createTreeNode(parent));
            if (!parent.isExpanded()) {
                parent.expand();
            }
        }

        this.fireEvent('itemCreated', { item: newEntity });
        //newEntity.commit();
        return newEntity;
    },
    /**
     * 设置实体主从关系
     * @param view-当前视图
     * @param entity-从实体
     */
    storeAddRecord: function (view, entity) {
        var me = view;
        var isSortEnabled = me.entityMeta.isSortEnabled;
        var models;
        var rowIdx = 0;
        var store = me.getData();
        //新插入的数据清除ext自动排序排序，禁用排序后，追加到列表的最后
        if (store.sorters && store.sorters.length > 0) store.sorters.removeAll();
        if (!isSortEnabled) {
            models = store.insert(0, entity);
        } else {
            models = store.add(entity);
            rowIdx = store.getCount() - 1;
        }
        entity = models[0];
        entity.rowIdx = rowIdx;
        me.setCurrent(entity, true);
    },
    /**
     * 表单编辑复制实体数据
     * @param {type} m
     * @returns
     */
    copyFormEntity: function (m) {
        var newEntity = Ext.create(this.model);
        newEntity.generateId();
        for (var pro in m.data) {
            if (pro == SIE._KeyPropertyName)
                continue;
            var value = m.data[pro];
            newEntity.data[pro] = value;
        }
        return newEntity;
    },
    /**
     * 表格编辑复制实体数据
     * @param {type} m
     * @returns
     */
    copyEntity: function (m) {
        var newEntity = Ext.create(this.model);
        newEntity.generateId();
        for (var pro in m.data) {
            if (pro == SIE._KeyPropertyName)
                continue;
            var value = m.data[pro];
            newEntity.data[pro] = value;
        }
        // 通过 c.getLoadedChildren()复制子列表数据
        var loadedChildren = m.getLoadedChildren();
        if (loadedChildren.length > 0) {
            this._setCopyChildData(loadedChildren, newEntity);
        }
        return newEntity;
    },
    /**
     * 复制子实体的数据
     * @param {Array} loadedChildren
     * @param {Ext.data.model} newEntity
     */
    _setCopyChildData: function (loadedChildren, newEntity) {
        for (var i = 0; i < loadedChildren.length; i++) {
            var curEntity = loadedChildren.getAt(i);
            if (curEntity.isEntity) {
                var newStore = SIE.data.Utils.createStore({
                    model: curEntity.associateView._model,
                });
                var data = curEntity.associateView.getData();
                var newChildEnyity = new curEntity.associateView._model();
                newChildEnyity.data = Ext.clone(data.data);
                newStore.add(newChildEnyity);
            } else {
                var newStore = SIE.data.Utils.createStore({
                    model: curEntity.model,
                });//即使不复制，newStore也需重新赋值，否则值会为上一个循环子视图的数据
                if (curEntity.associateView.gridConfig.isCopy != false) {
                    var data = curEntity.getData();
                    for (var j = 0; j < data.length; j++) {
                        var childEntity = data.getAt(j);
                        var newChildEnyity = new curEntity.associateView._model();
                        newChildEnyity.data = Ext.clone(childEntity.getData());
                        newChildEnyity.generateId();
                        newChildEnyity.data[curEntity.foreignKeyName] = newEntity.getId();
                        if (childEntity.getLoadedChildren().length > 0) {
                            this._setCopyChildData(childEntity.getLoadedChildren(), newChildEnyity);
                        }
                        newStore.add(newChildEnyity);
                    }
                }
            }
            var storeName = curEntity.associateView.getAssociateStoreKey();
            newEntity[storeName] = newStore;
        }
    },
    //为当前选择的树型控件添加一个子结点
    insertNewChild: function () {
        /// <summary>
        /// 为当前列表视图插入一个新的对象，同时设置好它的关系。
        ///
        /// 注意，此方法只能在树型视图中被调用。
        /// </summary>
        /// <returns type="SIE.data.Entity">返回新加的实体对象</returns>
        var me = this;
        if (me._isTree) {
            var s = me.getSelection();
            var parent = s[0] || me._getTreeRoot();

            if (parent.isLeaf()) {
                parent.set('leaf', false);
            }

            var model = me._createTreeNode(parent);
            model.Label = "ChildNode";
            var model = parent.insertChild(0, model);
            model.set('leaf', true);
            if (!parent.isExpanded()) {
                parent.expand();
            }

            this.fireEvent('itemCreated', { item: model });
            //parent.dirty=false;//避免父节点出现脏数据,导致保存时把父节点POST上去
            return model;
        }
    },
    /**
     * 创建树节点
     * @param {type} parent
     * @returns
     */
    _createTreeNode: function (parent) {
        var n = {
            leaf: true
        };
        var pIdField = this._getPIdField().name;
        n[pIdField] = parent.getId();
        return n;
    },
    /**
     * 移除选中数据
     * @returns Array
     */
    removeSelection: function () {
        var me = this;
        var selection = me.getSelection();
        if (selection.length > 0) {
            if (!me._isTree) {
                me.getData().remove(selection);
            }
            else {
                //删除树结点时，在客户端展开全部再删除。
                SIE.each(selection, function (i) {
                    i.remove();
                    //                    i.expand(true, function () {
                    //                        me._removeTreeNode(i);
                    //                    });
                    //                    _removeTreeNode: function (item) {
                    //                        var me = this;
                    //                        SIE.each(item.childNodes, function (i) { me._removeTreeNode(i); });
                    //                        item.remove();
                    //                    },
                });
            }
        }
        return selection;
    },
    defaultSaveMode: function () {
        //子类可重写
        return true;
    },
    /**
     * 是否可添加
     * @returns
     */
    canAddItem: function () {
        /// <summary>
        /// 判断当前的视图是否可以添加项
        /// </summary>
        //如果有父，父当前为空则不可编辑子
        var p = this.getParent();
        if (!Ext.isEmpty(p) && p.getCurrent() == null) {
            return false;
        }
        return this.getData() != null;
    },
    /**
     * 创建新实体
     * @returns
     */
    createNewItem: function () {
        return this.addNew();
    },
    /**
     * 设置主从关系(只设置当前视图的主表)
     * @param newEntity-设置实体
     */
    initRefProperties: function (newEntity) {
        var pv = this.getParent();
        if (pv) {
            var parentEntity = pv.getCurrent();
            if (parentEntity) {
                var parentId = parentEntity.data[SIE._KeyPropertyName];
                var fields = newEntity.getFields();
                fields.forEach(function (items) {
                    if (items.reference && items.reference.type == pv.model)
                        newEntity.set(items.name, parentId);
                });
            }
        }
    },
    beforeClosewin: function (returnObj) {
        var data = this.getData();
        if (data) {
            var changeData = SIE.data.Serializer.serialize(data, true);
            if (changeData._data) {
                var hasData = false;
                for (var pro in changeData._data) {
                    hasData = true;
                    break;
                }
                returnObj.data = data;
                returnObj.hasData = hasData;
            }
        }
        return returnObj;
    },
    /**
     * 用于数据不保存时，撤销在表格的修改
     * @param {} data 
     * @returns {} 
     */
    dataCallback: function (data) {
        if (data && data.rejectChanges) {
            data.rejectChanges();
        }
    }
});


Ext.define('SIE.view.RelationView', {
    statics: {
        list: 'list',
        detail: 'detail',
        navigation: 'navigation',
        condition: 'Condition',
        result: 'result'
    },
    isRelationView: true,
    constructor: function (name, target) {
        this._owner = null;//internal
        this._name = name;
        this._target = target;
    },
    getOwner: function () { return this._owner; },
    getName: function () { return this._name; },
    getTarget: function () { return this._target; }
});
/// <reference path="../_reference.js" />


Ext.define('SIE.view.QueryView', {
    extend: 'SIE.view.DetailView',
    isQueryView: true,
    lastQueryOpt: null,
    getResultView: function () {
        return this.findRelationView(SIE.view.RelationView.result);
    },
    attachNewEntity: function () {
        var model = this.getModel();
        var entity = Ext.create(model);
        this._setDefaultValue(entity);
        this.setCurrent(entity);
    },
    tryExecuteQuery: function (opts) {
        if (!SIE.App.checkCurUser()) {
            return;
        }
        this._executeQuery(opts);
    },

    clearCondition: function () {
        var me = this;
        var control = me.getControl();

        //清除掉控件设置的默认值，比如枚举默认值（20190613gc）
        var items = control.body.component.SIEView.formConfig.items;
        for (var i = 0; i < items.length; i++) {
            //if (items[i].xtype == "xcombobox") {
            items[i].valueOf("");
        }
        var model = this.getModel();
        var entity = Ext.create(model);
        this.setCurrent(entity);

        ////重置查询条件
        // control.body.component.reset();
        //me.attachNewEntity();
        var data = me.getCurrent().data;
        if (data) {
            for (pro in data) {
                if (Ext.String.endsWith(pro, '_Display')) {
                    data[pro.replace('_Display', '')] = null;
                }
            }
        }

        Ext.each(control.query('dateRange'), function (n) {
            n.clearValue();
        });
        Ext.each(control.query('spinRange'), function (n) {
            n.clearValue();
        });
        Ext.each(control.query('textRange'), function (n) {
            n.clearValue();
        });

        Ext.each(control.query('pagingLookUp'), function (n) {
            if (n._targetSelectItems) {
                n._targetSelectItems = { items: [], keys: [] };
            }

            if (n.lastSelectionRecord) {
                n.lastSelectionRecord = { value: [], rawValue: "" };
            }
            n.setRawValue("");
        });
    },

    _executeQuery: function (opts) {
        try {
            var e = this.getCurrent();
            //this.verificationcontrol();
            this.excludeField(e);
            this.extendFieldQuery(e);
            var opts = Ext.merge(opts || {}, { criteria: e });
            this.lastQueryOpt = opts;
            var resultView = this.getResultView();
            resultView.loadData(opts);
            //递归清空子表数据
            (function clearChildView(resultView) {
                var views = resultView.getChildren();
                views.forEach(function (view) {
                    view.setData(null);
                    clearChildView(view);
                });
            })(resultView);
        } catch (ex) {
            SIE.Msg.showInstantMessage(ex, "提示".t(), 1);
        }
    },

    extendFieldQuery: function (criteria) {
        var textRanges = this.getControl().query('textRange');
        Ext.each(textRanges, function (n) {
            var value = {
                "firstText": n.firstText,
                "lastText": n.lastText
            };
            criteria.data[n.name] = value;
        });
    },

    /**
     * 验证是否存在下拉控件value值为空
     */
    verificationcontrol: function () {
        for (var i = 0; i < this.getControl().items.items.length; i++) {
            var item = this.getControl().items.items[i];
            if (item.xtype === "combolist" && !Ext.isEmpty(item.rawValue, false))
                if (Ext.isEmpty(item.value, false)) throw "控件【".t() + item.fieldLabel + "】请求未完成，请稍等。。。。".t();
        }
    },

    //排除_display、dirty字段
    excludeField: function (criteria) {
        var filters = criteria.data;
        for (pro in filters) {
            if (Ext.String.endsWith(pro, '_Display') || pro == 'dirty') delete filters[pro];
        }
    }
});

Ext.define('SIE.view.NavigationView', {
    extend: 'SIE.view.QueryView'
});

Ext.define('SIE.view.ConditionView', {
    extend: 'SIE.view.QueryView'
});
/**
 * 对指定的 regions 进行布局。
 */
Ext.define('SIE.autoUI.Layout', {
    layout: function (regions) {
    	/// <summary>
    	/// 对指定的 regions 进行布局。
    	/// </summary>
        /// <param name="regions" type="SIE.autoUI.Regions"></param>
        /// <returns type="Ext Component" />
        SIE.markAbstract();
    }
});



/// <summary>除了 main 和 children，可以为这个对象添加其它的约定属性，如 navigate、condition</summary>
Ext.define('SIE.autoUI.Regions', {
    constructor: function (main) {
        this.main = main;
        this.children = [];
        this.surrounders = []; //{type,result}

        //当前 main 是否为根对象
        this.isRoot = false;
    },
    getCondition: function () {
        return this.getRegion(SIE.view.RelationView.condition);
    },
    getNavigation: function () {
        return this.getRegion(SIE.view.RelationView.navigation);
    },
    getRegion: function (name) {
        var s = SIE.findFirst(this.surrounders, function (r) { return r.type == name; });
        if (s != null) return s.result;
        return null;
    }
});
/// <reference path="../../_reference.js" />

//
Ext.define('SIE.autoUI.layouts.Common', {
    extend: 'SIE.autoUI.Layout',
    layout: function (regions) {
        var childrenUI = this._layoutChildren(regions);
        var res = this._layoutNaviCondition(regions, childrenUI);
        return res;
    },

    isLayoutChildrenHorizonal: false,

    isLayoutChildrenGroupHorizonal: false,

    layoutSize: null,

    getLayoutSize: function () {
        var objLayoutSize;
        var parentSize;
        var childrenSize;
        var me = this;
        if (me.layoutSize) {
            objLayoutSize = JSON.parse(me.layoutSize);
            // if (objLayoutSize.Parent < 0 && objLayoutSize.Children < 0) {
            parentSize = (objLayoutSize.Parent / (objLayoutSize.Parent + objLayoutSize.Children) * 100).toFixed(0) + "%";
            childrenSize = (objLayoutSize.Children / (objLayoutSize.Parent + objLayoutSize.Children) * 100).toFixed(0) + "%";
            // }
        } else {
            return null;
        }
        return {
            parentSize: parentSize,
            childrenSize: childrenSize
        }
    },

    _layoutChildren: function (regions) {
        var me = this;
        var main = regions.main;
        this.isLayoutChildrenHorizonal = main.getView().isLayoutChildrenHorizonal;
        this.isLayoutChildrenGroupHorizonal = main.getView().isLayoutChildrenGroupHorizonal;
        this.layoutSize = main.getView().layoutSize;
        var children = regions.children;
        var cardPanels = [];

        var mainControl = main.getControl();

        if (children.length === 0) {
            return mainControl;
        }

        //Create a tab here
        var tabPanel = {
            xtype: 'tabpanel',
            cls: 'custom_tabpanel', //用于样式特殊修改
            border: 0,
            activeTab: 0,
            listeners: {
                tabchange: this._tabChange
            },
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

        //排在前面的panel
        var prePanel;

        Ext.each(children, function (child, index) {
            if (child.getView().childLayoutType === 1) {
                if (index === 0) prePanel = 'card';
                cardPanels.push({
                    title: child.getView().getMeta().label,
                    items: child.getControl()
                });
            } else {
                if (index === 0) prePanel = 'tab';
                tabPanel.items.push({
                    title: child.getView().getMeta().label,
                    items: child.getControl()
                });
            }
        });

        var view = main.getView();

        var secondPanel = {
            xtype: 'panel',
            bodyStyle: {
                border: 0
            },
            layout: {
                type: me.isLayoutChildrenGroupHorizonal ? 'hbox' : 'vbox',
                pack: 'start',
                align: 'stretch'
            },
            defaults: {
                margin: me.isLayoutChildrenGroupHorizonal ? '0 5 0 0' : '0 0 5 0',
                flex: 1,
                layout: 'fit',
                border: false
            },
            items: []
        };
        if (prePanel === 'card') {
            if (cardPanels.length > 0) {
                cardPanels.forEach(function (child, index) {
                    if (index + 1 == cardPanels.length) {
                        child.margin = 0;
                    }
                    secondPanel.items.push(child);
                });
            }
            if (tabPanel.items.length > 0) {
                if (cardPanels.length === 0) {
                    tabPanel.margin = 0;
                }
                secondPanel.items.push(tabPanel);
            }
        } else {
            if (tabPanel.items.length > 0) {
                if (cardPanels.length === 0) {
                    tabPanel.margin = 0;
                }
                secondPanel.items.push(tabPanel);
            }
            if (cardPanels.length > 0) {
                cardPanels.forEach(function (child, index) {
                    if (index + 1 == cardPanels.length) {
                        child.margin = 0;
                    }
                    secondPanel.items.push(child);
                });
            }
        }

        if (tabPanel.items.length > 0 && cardPanels.length === 0) {
            secondPanel = tabPanel;
        }
        if (view.formConfig)
            return this._layoutFormChildrenCore(mainControl, secondPanel, me.isLayoutChildrenHorizonal);
        return this.layoutChildrenCore(mainControl, secondPanel, me.isLayoutChildrenHorizonal);
    },
    _layoutNaviCondition: function (regions, childrenUI) {
        var con = regions.getCondition();
        if (con === null) {
            return childrenUI;
        }

        return Ext.widget('container', {
            border: 0,
            layout: 'border',
            scrollable: true,
            defaults: {
                //split: true,
                layout: 'fit',
                border: 0
            },
            items: [Ext.merge({
                title: '查询条件'.t(),
                items: con.getControl()
            }, GlobalConfig.defaultConditionPanelConfig), {
                region: 'center',
                items: childrenUI
            }]
        });
    },
    //protected
    _layoutFormChildrenCore: function (mainControl, secondPanel) {
        var layoutSizes = this.getLayoutSize();
        if (layoutSizes === null) {
            return Ext.widget('container', {
                layout: 'border',
                autoScroll: true,
                defaults: {
                    collapsible: false,
                    split: false,
                    layout: 'fit',
                    border: 0
                },
                items: [{
                    region: 'north',
                    minHeight: 100,
                    items: mainControl
                }, {
                    region: 'center',
                    minHeight: 100,
                    items: secondPanel
                }]
            });
        } else {
            return Ext.widget('container', {
                layout: 'border',
                autoScroll: true,
                defaults: {
                    collapsible: false,
                    split: false,
                    layout: 'fit',
                    border: 0
                },
                items: [{
                    region: 'north',
                    minHeight: 100,
                    height: layoutSizes.parentSize,
                    items: mainControl
                }, {
                    region: 'center',
                    minHeight: 100,
                    height: layoutSizes.childrenSize,
                    items: secondPanel
                }]
            });
        }

    },
    //protected
    _layoutFormChildrenCore: function (mainControl, secondPanel, isLayoutChildrenHorizonal) {
        var layoutSizes = this.getLayoutSize();
        if (isLayoutChildrenHorizonal) {
            if (layoutSizes === null) {
                layoutSizes = layoutSizes || { parentSize: '50%', childrenSize: '50%' };
                return Ext.widget('container', {
                    layout: 'border',
                    autoScroll: true,
                    defaults: {
                        collapsible: false,
                        split: true,
                        layout: 'fit',
                        border: 0
                    },
                    items: [{
                        region: 'west',
                        minHeight: 100,
                        width: layoutSizes.parentSize,
                        items: mainControl
                    }, {
                        region: 'center',
                        minHeight: 100,
                        width: layoutSizes.childrenSize,
                        items: secondPanel
                    }]
                });
            } else {
                return Ext.widget('container', {
                    layout: 'border',
                    autoScroll: true,
                    defaults: {
                        collapsible: false,
                        split: true,
                        layout: 'fit',
                        border: 0
                    },
                    items: [{
                        region: 'west',
                        minHeight: 100,
                        width: layoutSizes.parentSize,
                        items: mainControl
                    }, {
                            region: 'center',
                        minHeight: 100,
                        width: layoutSizes.childrenSize,
                        items: secondPanel
                    }]
                });
            }
        }
        else {
            if (layoutSizes === null) {
                return Ext.widget('container', {
                    layout: 'border',
                    autoScroll: true,
                    defaults: {
                        collapsible: false,
                        split: false,
                        layout: 'fit',
                        border: 0
                    },
                    items: [{
                        region: 'north',
                        minHeight: 100,
                        items: mainControl
                    }, {
                        region: 'center',
                        minHeight: 100,
                        items: secondPanel
                    }]
                });
            } else {
                return Ext.widget('container', {
                    layout: 'border',
                    autoScroll: true,
                    defaults: {
                        collapsible: false,
                        split: false,
                        layout: 'fit',
                        border: 0
                    },
                    items: [{
                        region: 'north',
                        minHeight: 100,
                        height: layoutSizes.parentSize,
                        items: mainControl
                    }, {
                        region: 'center',
                        minHeight: 100,
                        height: layoutSizes.childrenSize,
                        items: secondPanel
                    }]
                });
            }
        }
    },
    //protected
    layoutChildrenCore: function (mainControl, secondPanel, isLayoutChildrenHorizonal) {
        var items;
        var layoutSizes = this.getLayoutSize();
        layoutSizes = layoutSizes || { parentSize: '50%', childrenSize: '50%' };
        if (isLayoutChildrenHorizonal) {
            items = [{
                region: 'center',
                minHeight: 100,
                // minWidth: 500,
                width: layoutSizes.parentSize,
                items: mainControl
            }, {
                region: 'east',
                minHeight: 100,
                // minWidth: 500,
                width: layoutSizes.childrenSize,
                items: secondPanel
            }];
        } else {
            items = [{
                region: 'center',
                minHeight: 100,
                // minWidth: 500,
                height: layoutSizes.parentSize,
                items: mainControl
            }, {
                region: 'south',
                minHeight: 100,
                // minWidth: 500,
                height: layoutSizes.childrenSize,
                items: secondPanel
            }];
        }

        return Ext.widget('container', {
            border: 0,
            layout: 'border',
            autoScroll: true,
            defaults: {
                collapsible: false,
                split: true,
                layout: 'fit',
                border: 0
            },
            items: items
        });
    },

    _tabChange: function (tabPanel, newCard, oldCard, eOpts) {
        var control = newCard.down("gridpanel");
        if (control != null && control.SIEView.getChildren().length == 0)
            if (newCard.down("form"))
                control = newCard.down("form").SIEView.getChildren().length > 0 ? newCard.down("form") : control;
        if (!control)
            control = newCard.down("form");
        if (!control)
            control = newCard.down("treepanel");


        var view = control.SIEView;
        view.loadChildData();
        if (view.hasListeners['isready']) {
            view.fireEvent('isReady', true);
        }
    }
});


Ext.define('SIE.autoUI.layouts.RightChildren', {
    extend: 'SIE.autoUI.layouts.Common',
    _layoutChildrenCore: function (mainControl, childrenTab) {
        return Ext.widget('container', {
            border: 0,
            layout: 'border',
            items: [{
                region: 'west',
                width: 300,
                border: 0,
                split: true,
                layout: 'fit',
                items: mainControl
            }, {
                region: 'center',
                border: 0,
                autoScroll: true,
                items: childrenTab
            }]
        });
    }
});
/**
 * 视图工厂
 */
Ext.define('SIE.autoUI.ViewFactory', {
    extend: 'Ext.util.Observable',

    //构造器
    constructor: function () {
        this.callParent(arguments);
    },

    /*******************************************生命周期******************************************* */

    /**
     * 视图创建前
     * @param {*} block 块配置
     * @param {*} curEntity 当前操作实体(可空)
     */
    _beforeCreate: function (block, curEntity) {
        var me = this;
        if (block.behaviors && block.behaviors.length > 0) {
            block.behaviors.forEach(function (behavior) {
                if (behavior.beforeCreate) behavior.beforeCreate(block, curEntity);
            })
        }
    },

    /**
     * view生成后
     */
    _onCreated: function (view) {
        var me = this;
        if (view.behaviors && view.behaviors.length > 0) {
            view.behaviors.forEach(function (behavior) {
                if (behavior.onCreated) behavior.onCreated(view);
            })
        }
        view.mon(view, 'ondataloaded', function () {
            me._onDataLoaded(view);
        });
        view.mon(view, 'onviewready', function () {
            me._onViewReady(view);
        });
        view.mon(view, 'onshow', function () {
            me._onShow(view);
        });
    },

    /**
     * 视图关联准备好后
     * @param {*} view 
     */
    _onViewReady: function (view) {
        var me = this;
        if (view.behaviors && view.behaviors.length > 0) {
            if (view.behaviors.length > 0) {
                view.behaviors.forEach(function (behavior) {
                    if (behavior.onViewReady) behavior.onViewReady(view);
                })
            }
        }
    },

    /**
     * 数据加载后
     * @param {*} view 
     */
    _onDataLoaded: function (view) {
        var me = this;
        if (view.behaviors && view.behaviors.length > 0) {
            view.behaviors.forEach(function (behavior) {
                if (behavior.onDataLoaded) behavior.onDataLoaded(view);
            })
        }
    },

    /**
     * 页面显示
     * @param {any} view
     */
    _onShow: function (view) {
        var me = this;
        if (view.behaviors && view.behaviors.length > 0) {
            view.behaviors.forEach(function (behavior) {
                if (behavior.onShow) behavior.onShow(view);
            })
        }
    },
    /**********************************************生命周期******************************************** */

    /**
     * 创建表格视图
     * @param {*} block 块配置
     */
    createListView: function (block) {
        var view = new SIE.view.ListView(block);
        var store = this._createStore(block);
        store.associateView = view;
        var grid = null;
        if (store.model.isTree) {
            grid = this._createTreeListControl(block, store, view);
        }
        else {
            if (block.groupBy) { block.gridConfig.groupBy = block.groupBy; }
            grid = this._createListControl(block, store, view);
        }

        view._setControl(grid);
        this._onCreated(view);
        return view;
    },

    //创建store
    _createStore: function (block) {
        var storeCfg = block.storeConfig;
        //内存排序4（gc）
        var remoteSort = true;
        remoteSort = block.storeConfig.remoteSort;
        if (block.groupBy) { storeCfg.groupField = block.groupBy; }
        var model = block.model;
        var store = SIE.data.Utils.createStore({
            model: model,
            storeConfig: storeCfg,
            remoteSort: remoteSort
        });
        return store;
    },

    //创建树表格控件
    _createTreeListControl: function (block, store, view) {
        var gridConfig = block.gridConfig;
        var config = {
            store: store
        };
        config = Ext.merge(config, GlobalConfig.defaultTreeGridConfig);
        //网格列配置处理
        this._gridColumnHandler(gridConfig, view);

        config.plugins = config.plugins || [];

        var me = this;
        if (gridConfig.allowEdit) {
            config.plugins.push({
                ptype: 'cellediting',
                clicksToEdit: 2,//编辑单元格点击次数
                id: 'editing',

                listeners: {
                    beforeedit: {
                        fn: function (editor, context, eOpts) {
                            var me = this,
                                record = context.record,
                                column = context.column,
                                cell = context.cell,
                                canEdit = true;

                            var handlers = view.getProChgHandlers();
                            handlers.forEach(function (handler, i, arr) {
                                if (handler.effect == 'setReadOnly' && column.dataIndex == handler.pro) {
                                    var isReadonly = handler.lambda(record);
                                    if (isReadonly) {
                                        canEdit = false;
                                    }

                                    var extCell = Ext.get(cell);
                                    if (extCell.query('span.x-grid-checkcolumn').length > 0) {
                                        if (!canEdit) {
                                            extCell.addCls('x-item-disabled');
                                        }
                                        else {
                                            extCell.removeCls('x-item-disabled');
                                        }
                                    }
                                }
                            });

                            return canEdit;
                        }
                    },
                    edit: {
                        fn: function (editor, context, eOpts) {
                            if (context.value != context.originalValue) {
                                var column = context.column,
                                    record = context.record;

                                var handlers = view.getProChgHandlers();
                                handlers.forEach(function (handler, i, arr) {
                                    if (handler.effect == 'cascade' && column.dataIndex == handler.pro) {
                                        handler.lambda(record);
                                    }
                                });
                            }
                        },
                    }
                }
            });
            config.plugins.push({
                ptype: 'gridexporter'
            });
            if (gridConfig.draggableForTree != false) {
                Ext.apply(config,
                    {
                        viewConfig: {
                            plugins: { ptype: 'treeviewdragdrop' }
                        }
                    });
            }
        }
        Ext.merge(config, gridConfig);
        this._createCommands(config, view);
        var tempBlock = Ext.clone(block);
        tempBlock.gridConfig = config;
        me._beforeCreate(tempBlock);
        var treeGrid = Ext.create('Ext.tree.Panel', config);
        treeGrid.getSelectionModel().setSelectionMode('multi');
        treeGrid.mon(treeGrid, {
            beforeload: function (store, operation) {
                var me = this;
                if (me.getData() != null) {
                    me.scrollTo(0, 0);
                }
            },
            load: function (grid, records, successful, operation, node) {
                var me = this;
                me.expandAll();
            },
            afterlayout: function (grid, layout) {
                var me = this;
                me.scrollTo(0, 0);

            },
            itemmove: function (node, oldParent, newParent, index, eOpts) {
                var me = this;
                //node.getOwnerTree().view.refresh();

                window.setTimeout(function () {
                    var idx = index;
                    if (newParent.getId() > 0) {
                        node.setTreePId(newParent.getId());
                    } else {
                        node.setTreePId(null);
                    }
                    if (node.previousValues.index < index) {
                        //后移
                        var cnt = index - node.previousValues.index;
                        while (newParent.childNodes[idx].previousSibling &&
                            newParent.childNodes[idx].getINDEX_() <=
                            newParent.childNodes[idx].previousSibling.getINDEX_()) {
                            var curidx = newParent.childNodes[idx].getINDEX_(); //当前索引值
                            if (newParent.childNodes[idx].previousSibling) {
                                var pidx = newParent.childNodes[idx].previousSibling.getINDEX_(); //前索引

                                if (curidx == pidx) {
                                    if (newParent.childNodes[idx].nextSibling) {
                                        var nidx = newParent.childNodes[idx].nextSibling.getINDEX_();
                                        pidx = pidx + Math.floor(Math.random() * (nidx - pidx + 1));
                                    } else {
                                        pidx = pidx + 1;
                                    }
                                }
                                newParent.childNodes[idx].previousSibling.setINDEX_(curidx);
                                newParent.childNodes[idx].setINDEX_(pidx);
                            }

                            idx--;
                        }
                    } else {
                        //前移
                        while (newParent.childNodes[idx].nextSibling &&
                            newParent.childNodes[idx].getINDEX_() >=
                            newParent.childNodes[idx].nextSibling.getINDEX_()) {
                            var curidx = newParent.childNodes[idx].getINDEX_(); //当前索引值
                            if (newParent.childNodes[idx].nextSibling) {
                                var nidx = newParent.childNodes[idx].nextSibling.getINDEX_(); //后索引
                                if (curidx == nidx) {
                                    if (newParent.childNodes[idx].previousSibling) {
                                        var pidx = newParent.childNodes[idx].previousSibling.getINDEX_();
                                        nidx = pidx + Math.floor(Math.random() * (nidx - pidx + 1));
                                    } else {
                                        nidx = nidx - 1;
                                    }
                                }

                                newParent.childNodes[idx].nextSibling.setINDEX_(curidx);
                                newParent.childNodes[idx].setINDEX_(nidx);
                            }
                            idx++;
                        }
                    }
                    me.SIEView.syncCmdState();
                },
                    50);
            }
        });
        treeGrid.addListener('columnshow', this._onColumnChange, this);
        treeGrid.addListener('columnhide', this._onColumnChange, this);
        return treeGrid;
    },

    //创建表格控件
    _createListControl: function (block, store, view) {
        var gridConfig = block.gridConfig;
        var config = {
            store: store
        };
        config = Ext.merge(config, GlobalConfig.defaultGridConfig);

        //超过10000就不用分页了。
        if (store.pageSize < 10000) {
            var pagingBarItems = gridConfig.pagingBarItems || [];
            var pagingBarConfig = {
                items: pagingBarItems,
                store: store
            };
            pagingBarConfig = Ext.merge(pagingBarConfig, gridConfig.pagingBarConfig);
            pagingBarConfig = Ext.merge(pagingBarConfig, GlobalConfig.defaultPagingBarConfig);
            view._pagingBar = Ext.create(pagingBarConfig);
            config.dockedItems = [view._pagingBar];
        }
        //网格列配置处理
        this._gridColumnHandler(gridConfig, view);

        config.plugins = config.plugins || [];
        var me = this;
        if (gridConfig.allowEdit) {
            // todo 现在侵入性太高，得安排时间重构方法移出来.listeners rowdblclick 等
            config.plugins.push({
                ptype: 'cellediting',
                clicksToEdit: 2,
                id: 'editing',
                listeners: {
                    beforeedit: {
                        fn: function (editor, context, eOpts) {
                            var me = this,
                                record = context.record,
                                column = context.column,
                                cell = context.cell,
                                canEdit = true;

                            var handlers = view.getProChgHandlers();
                            handlers.forEach(function (handler, i, arr) {
                                if (handler.effect == 'setReadOnly' && column.dataIndex == handler.pro) {
                                    var isReadonly = handler.lambda(record);
                                    if (isReadonly) {
                                        canEdit = false;
                                    }
                                    //todo:未知为何加上 x-item-disabled 样式,但加上会影响列表复选框只读表达式判断
                                    //var extCell = Ext.get(cell);
                                    //if (extCell.query('span.x-grid-checkcolumn').length > 0) {
                                    //    if (!canEdit) {
                                    //        extCell.addCls('x-item-disabled');
                                    //    }
                                    //    else {
                                    //        extCell.removeCls('x-item-disabled');
                                    //    }
                                    //}
                                }
                            });

                            return canEdit;
                        }
                    },
                    edit: {
                        fn: function (editor, context, eOpts) {
                            if (context.value != context.originalValue) {
                                var column = context.column,
                                    record = context.record;

                                var handlers = view.getProChgHandlers();
                                handlers.forEach(function (handler, i, arr) {
                                    if (handler.effect == 'cascade' && column.dataIndex == handler.pro) {
                                        handler.lambda(record);
                                    }
                                });
                            }
                        },
                    }
                }
            });
        }
        //grid内容可复制
        config.viewConfig = config.viewConfig || {};
        config.viewConfig.enableTextSelection = true;

        config.plugins.push({
            ptype: 'gridexporter'
        });
        Ext.merge(config, gridConfig);
        //config.columns.splice(0, 0, {
        //    xtype: 'rownumberer',
        //    width: 40
        //});
        if (config.groupBy) {
            var groupingFeature = Ext.create('Ext.grid.feature.Grouping', {
                //                enableGroupingMenu: false,
                //                groupByText: '用该字段分组',
                //                showGroupsText: '显示分组',
                hideGroupedHeader: true,
                groupHeaderTpl: '{name}'
            });
            config.features = [groupingFeature];
        }
        this._createCommands(config, view);
        //解决列表复选框选择行，使用快捷键复制表格内容报错
        if (config.selModel.type != "checkboxmodel") {
            config.plugins.push({
                id: 'clipboardPlugin',
                ptype: 'clipboardCellData' //换成自己重写的别名clipboardCellData，默认继承clipboard ,此插件智能运用于selModel.type="spreadsheet"模式
            });
        }
        var tempBlock = Ext.clone(block);
        tempBlock.gridConfig = config;
        //调用生命周期函数
        me._beforeCreate(tempBlock);
        var grid = Ext.create('Ext.grid.Panel', config);
        grid.addListener("rowdblclick", this._EditGrid, this);
        grid.addListener('columnshow', this._onColumnChange, this);
        grid.addListener('columnhide', this._onColumnChange, this);
        grid.addListener('columnmove', this._onColumnMove, this);
        grid.addListener('afterrender', this._onGridRendered, this);
        grid.addListener('rowcontextmenu', function (g, record, tr, rowIndex, e, eOpts) {
            e.preventDefault();
            var rightClick = new Ext.menu.Menu({
                items: [{
                    handler: function () {
                        //如果存在fixcolumn情况下,在当前grid下会生成locked与normal两个grid,这两个grid中不存在当前grid中配置的插件,需要向上一级grid获取
                        var p = g.grid.getPlugin('clipboardPlugin') || g.grid.ownerGrid.getPlugin('clipboardPlugin'); if (p) p.copy();
                    }, // 点击后触发的事件
                    text: '复制文本'.t()
                }]
            });
            rightClick.showAt(e.getXY());
        }, this);
        return grid;
    },

    //当列显示或隐藏时
    _onColumnChange: function (headerCt, header) {
        var visibleFields = [];
        headerCt.getVisibleGridColumns().forEach(function (m) {
            if (m.config.dataIndex) {
                visibleFields.push(m.config.dataIndex);
            }
        });
        var token;
        if (headerCt.grid.SIEView) {
            token = headerCt.grid.SIEView.token + headerCt.grid.SIEView.viewGroup;
        } else {
            token = headerCt.grid.config.ownerGrid.SIEView.token + headerCt.grid.config.ownerGrid.SIEView.viewGroup;
        }
        var postData = {
            token: token,
            value: JSON.stringify({
                VisibleFields: visibleFields,
                LockedFields: []
            })
        };
        SIE.Ajax({
            async: true,
            url: '/api/UserSettingApi/AddUserGridSetting',
            method: 'post',
            params: postData,
            success: function (res) {

            }
        });
    },

    _onColumnMove: function (ct, column, fromIdx, toIdx, eOpts) {
        var visibleFields = [];
        ct.getVisibleGridColumns().forEach(function (m) {
            if (m.config.dataIndex) {
                visibleFields.push(m.config.dataIndex);
            }
        });
        var token;
        if (!ct.grid) {
            ct = ct.up();//多表头时ct要在上一层才能取到grid
        }
        if (ct.grid.SIEView) {
            token = ct.grid.SIEView.token + ct.grid.SIEView.viewGroup;
        } else {
            token = ct.grid.config.ownerGrid.SIEView.token + ct.grid.config.ownerGrid.SIEView.viewGroup;
        }
        var postData = {
            token: token,
            value: JSON.stringify({
                VisibleFields: visibleFields,
                LockedFields: []
            })
        };
        SIE.Ajax({
            async: true,
            url: '/api/UserSettingApi/AddUserGridSetting',
            method: 'post',
            params: postData,
            success: function (res) {
            }
        });
    },

    //表格生成后
    _onGridRendered: function (cmp) {
        var menu = cmp.headerCt.getMenu();
        var menuItem = menu.add({
            text: '重置列'.t(),
            handler: function () {
                var token;
                if (cmp.SIEView) {
                    token = cmp.SIEView.token + cmp.SIEView.viewGroup;
                } else {
                    token = cmp.config.ownerGrid.SIEView.token + cmp.config.ownerGrid.SIEView.viewGroup;
                }
                var postData = {
                    token: token
                };
                SIE.Ajax({
                    async: true,
                    url: '/api/UserSettingApi/DeleteUserGridSetting',
                    method: 'post',
                    params: postData,
                    success: function (res) {
                        var objRes = JSON.parse(res.responseText);
                        if (objRes.Success) {
                            var msg = '重置成功，请重新打开页面'.t();
                            SIE.Msg.showToast(msg);
                        }
                    }
                });
            }
        });
    },

    /**
     * 双击表行时，触发修改按钮
     * @param grid 当前grid
     * @param rowindex 行索引
     * @param e
     */
    _EditGrid: function (grid, rowindex, e) {
        var commands = [];
        if (rowindex.belongsView != null) {
            commands = rowindex.belongsView._commands;
        } else if (grid.grid.SIEView) {
            commands = grid.grid.SIEView._commands;
        }
        rowindex._EditGrid = true;
        for (var i = 0; i < commands.length; i++) {
            var editCommand = commands.items[i];
            /*ToDo：
            *问题1：如果存在多个命令继承"SIE.cmd.Edit"会执行多次修改
            *问题2：当前修改命令继承的不是"SIE.cmd.Edit",而是"SIE.cmd.Edit"的子类不会执行
            * */
            if ((editCommand.$className == "SIE.cmd.Edit" || (editCommand.superclass && editCommand.superclass.$className == "SIE.cmd.Edit")) && editCommand.meta.text == "修改") {
                if (editCommand.getEditEntity() != rowindex) {
                    editCommand.view.setCurrent(rowindex);
                }
                if (!editCommand.view._sourceCmd)
                    editCommand.view.setSourceCmd(grid.ownerGrid.SIEView.getCmdControl(editCommand.meta.command));
                if (editCommand.canExecute(editCommand.view))
                    editCommand.execute(editCommand.view);
            }
        }
    },
    /**
     * 处理表单配置方法
     * @param {Object} formCfg-元数据配置
     */
    _processFormCfg: function (formCfg) {
        if (formCfg) {
            if (formCfg.items) {
                for (var i = 0, length = formCfg.items.length; i < length; i++) {
                    var item = formCfg.items[i];
                    if (item.xtype && item.xtype === 'textfield') {
                        item.xtype = 'searchtextfield'; //Bs标准功能：查询区域：回车可触发查询的输入框
                    }
                }
            }
        }
    },
    createConditionView: function (block) {
        var view = new SIE.view.ConditionView(block);
        var formCfg = Ext.merge({ bodyPadding: 10 }, block.formConfig);
        this._processFormCfg(formCfg);
        if (!formCfg.tbar) {
            formCfg.tbar = [{ command: 'SIE.cmd.ExecuteQuery' }, { command: 'SIE.cmd.ClearCondition' }];
        }
        formCfg.bodyCls = "conditionViewBodyCls";
        view.formSpecialHandle(formCfg, null);
        block.formConfig = formCfg;
        var form = this._createEditForm(block, view);
        view._setControl(form);
        view.attachNewEntity();
        CRT.Context.PageContext.setQueryView(view);//todo 页面中有弹出框含有查询面板时是否会有问题
        this._onCreated(view);
        return view;
    },

    createNavigationView: function (block) {
        //暂时只支持 ConditionView
        return this.createConditionView(block);
    },

    /**
     * 创建明细视图
     * @param {*} block 块配置
     * @param {*} entity 当前实体
     */
    createDetailView: function (block, entity) {
        var view = new SIE.view.DetailView(block);
        var config = Ext.merge({}, GlobalConfig.defaultDetailFormConfig);
        var formCfg = Ext.merge({ bodyPadding: 10 }, block.formConfig);
        formCfg = Ext.merge(formCfg, config);
        view._specialFormLayout(formCfg);
        view.formSpecialHandle(formCfg, entity);
        var tempBlock = Ext.clone(block);
        tempBlock.formConfig = formCfg;
        var form = this._createEditForm(tempBlock, view, entity);
        view._setControl(form);
        this._onCreated(view);
        if (entity) {
            view._setDefaultValue(entity);
            view.setCurrent(entity);

        }
        return view;
    },

    /**
     * 创建form表单控件
     * @param {*} block 
     * @param {*} view 
     * @param {*} entity 
     */
    _createEditForm: function (block, view, entity) {
        var formCfg = block.formConfig;
        this._createCommands(formCfg, view);
        var config = {
            viewModel: {
                data: {
                    p: {}
                }
            }
        };
        config = Ext.merge(config, GlobalConfig.defaultEditFormConfig || []);
        formCfg = Ext.merge(formCfg, config);
        if (formCfg.layout.width)
            formCfg.width = parseInt(formCfg.layout.width);
        if (formCfg.layout.height)
            formCfg.height = parseInt(formCfg.layout.height);
        block.formConfig = formCfg;
        this._beforeCreate(block, entity);
        var formPanel = Ext.create('Ext.form.Panel', formCfg);
        return formPanel;
    },

    //网格列处理
    _gridColumnHandler: function (gridConfig, view) {
        var me = this;
        gridConfig.columns.forEach(function (item, idx, arr) {
            if (item.readonlyLambda && item.readonlyLambda != "") {
                var func = view.getFunc(item.readonlyLambda);
                view.addProChgHandler({ pro: item.dataIndex, effect: 'setReadOnly', lambda: func });
            }
            if (item.cascade && item.cascade.length > 0) {
                item.cascade.forEach(function (e, i, arr) {
                    var func = view.getFunc(e);
                    view.addProChgHandler({ pro: item.dataIndex, effect: 'cascade', lambda: func });
                });
            }
            if (item.revertInvalid) {
                view.addProChgHandler({ pro: item.dataIndex, effect: 'setRevertInvalid', lambda: item.revertInvalid });
            }
            //if (item.editor && item.editor.xtype === "datetimefield") {
            //    item.format = GlobalConfig.dateTimeFormat;
            //}
        });
    },

    createCommands: function (cmds, view) {
        var cmdList = [];
        if (cmds) {
            var me = this;
            SIE.each(cmds, function (item) {
                if (item.command) me._setHandler(item, view);
                Ext.merge(item, { margin: '0 2' });
                /*if (item.hierarchy && item.text == item.hierarchy) {
                    throw new Error(item.command + "[" + item.text +"]命令的属性text和hierarchy不能存在相同命名，继承的命令会把hierarchy属性继承过来");
                }*/

                if (item.bindingMenu) {
                    var menuGroup = cmdList.filter(function (m) {
                        return m._bm === item.bindingMenu;
                    });
                    if (menuGroup.length > 0) {
                        menuGroup[0].menu.push(item);
                        menuGroup[0].iconCls = menuGroup[0].menu[0].iconCls;
                    } else {
                        cmdList.push({
                            xtype: 'button',
                            _bm: item.bindingMenu,
                            text: item.bindingMenu.t(),
                            mergin: '0 2',
                            iconCls: item.iconCls,
                            menu: [item]
                        });
                    }
                } else if (item.splitTo) {
                    if (item.splitTo) {
                        var btn = cmdList.filter(function (m) {
                            return m._text === item.splitTo;
                        });
                        if (btn.length > 0) {
                            var b = btn[0];
                            b.xtype = 'splitbutton';
                            if (b.menu) {
                                b.menu.push(item);
                            } else {
                                b.menu = [item];
                            }
                        } else {
                            cmdList.push(item);
                        }
                    }
                } else {
                    cmdList.push(item);
                }
            });
        }
        return cmdList;
    },

    _createCommands: function (panelCfg, view) {

        var cmds = panelCfg.tbar;
        panelCfg.tbar = null;
        var cmdList = this.createCommands(cmds, view);
        if (cmdList.length > 0) {
            panelCfg.dockedItems = panelCfg.dockedItems || [];
            var toolBarConfig = { items: cmdList };
            toolBarConfig = Ext.merge(toolBarConfig, GlobalConfig.defaultToolBarConfig);
            if (panelCfg.bodyCls && panelCfg.bodyCls === "conditionViewBodyCls") {
                toolBarConfig.style = 'border-top-width: 0';
            }
            panelCfg.dockedItems.push(toolBarConfig);
        }
    },

    _setHandler: function (tbarItemCfg, view) {
        /// <summary>
        /// 为某个工具栏项生成客户端命令，并添加事件处理函数
        /// </summary>
        /// <param name="tbarItemCfg"></param>
        /// <param name="view"></param>

        tbarItemCfg.id = view._getCmdControlId(tbarItemCfg.command);
        tbarItemCfg.name = tbarItemCfg.command;

        var cmd = Ext.create(tbarItemCfg.command, { meta: tbarItemCfg });

        //在创建 cmd 的过程中，可能会修改 meta 中的数据以生成界面
        cmd._modifyMeta(tbarItemCfg);
        cmd._setOwnerView(view);

        tbarItemCfg.handler = function () {
            //this 是当前的按钮对象。
            cmd.tryExecute(this);
        };

        view._addCmd(tbarItemCfg.command, cmd);

        this.fireEvent(this.commandCreatedEventName, { command: cmd });
    },
    /**
     * commandCreatedEventName 
     */
    commandCreatedEventName: 'commandCreated',
});


Ext.define('SIE.autoUI.ControlResult', {
    constructor: function (view, control) {
        this._view = view;
        this._control = control;
    },
    getView: function () { return this._view; },
    getControl: function () { return this._control; }
});
/**
 * 生成器基类
 */
Ext.define('SIE.autoUI.UIGenerator',
    {
        _vf: null,
        constructor: function (viewFactory) {
            this._vf = viewFactory;
        },
        generateControl: function (aggtMeta, entity) {
            SIE.markAbstract();
        }
    });

/**
 * 默认生成器
 */
Ext.define('SIE.autoUI.AggtUIGeneratorDefault', {
    extend: 'SIE.autoUI.UIGenerator',

    //初始化Meta配置
    _initMetaConfig: function (config) {
        if (config.behaviors && config.behaviors.length > 0) {
            var behaviors = [];
            config.behaviors.forEach(function (behavior) {
                if (typeof behavior === 'string') {
                    behavior = Ext.create(behavior);
                }
                behaviors.push(behavior);
            })
            config.behaviors = behaviors;
        }
    },

    /**
     * 生成Control
     * @param {*} aggtMeta 聚合meta
     * @param {*} entity 当前实体
     */
    generateControl: function (aggtMeta, entity) {
        /// <summary>创建一个聚合控件</summary>
        /// <param name="aggtMeta" type="SIE.Web.ClientMetaModel.AggtMeta">服务端生成的元数据对象</param>
        /// <returns type="SIE.autoUI.ControlResult" />
        var mainView = null;
        var mk = aggtMeta.mainBlock || aggtMeta;
        this._initMetaConfig(mk);
        var vf = this._vf;
        if (mk.gridConfig) {
            mainView = vf.createListView(mk);
        }
        else {
            mainView = vf.createDetailView(mk, entity);
        }
        var aggtView = this._generateAggt(aggtMeta, mainView, true);
        if (mainView.hasListeners['isready']) {
            mainView.fireEvent('isReady', true);
        }
        if (mainView.hasListeners['onviewready']) {
            mainView.fireEvent('onviewready', true);
        }

        var getKey = function () {
            var key = mainView.model;
            if (entity) {
                key += '_' + entity.getId();
            }
            return key;
        }
        CRT.Event.fire(getKey() + '_loaded', mainView);
        //页面生成后，需要加载出当前活动的子页签数据;
        mainView._resetChildrenData();
        return aggtView;
    },

    //生成聚合块
    _generateAggt: function (aggtMeta, mainView, isRoot) {
        //regions
        var main = new SIE.autoUI.ControlResult(mainView, mainView.getControl());
        var regions = new SIE.autoUI.Regions(main);
        regions.isRoot = isRoot;

        if (aggtMeta.children) {
            this._generateChildren(aggtMeta.children, regions);
        }

        if (aggtMeta.surrounders) {
            this._generateSurrounders(aggtMeta.surrounders, regions);
        }

        var control = this._layout(aggtMeta, regions);

        return new SIE.autoUI.ControlResult(mainView, control);
    },

    //生成子块
    _generateChildren: function (childrenAggt, regions) {
        /// <returns type="SIE.autoUI.ControlResult[]" />
        var mainView = regions.main.getView();
        var vf = this._vf;
        var childView;
        var me = this;
        for (var i = 0; i < childrenAggt.length; i++) {
            var childAggt = childrenAggt[i];
            me._initMetaConfig(childAggt.mainBlock);
            if (childAggt.mainBlock.gridConfig)
                childView = vf.createListView(childAggt.mainBlock);
            else
                childView = vf.createDetailView(childAggt.mainBlock);
            childView._childProperty = childAggt.childProperty;
            childView._associatedProperty = childAggt.associatedProperty;
            if (childView.isDetailView && !mainView.isDetailView)//父为listview才禁用子表单
                childView.getControl().setDisabled(true);

            var childResult = this._generateAggt(childAggt, childView, false);
            regions.children.push(childResult);
            childView._setParent(mainView);
            if (childView.hasListeners['onviewready']) {
                childView.fireEvent('onviewready', true);
            }
        }
    },

    //生成环绕块
    _generateSurrounders: function (surroundersAggt, regions) {
        var me = this;
        var mainView = regions.main.getView();

        for (var i = 0; i < surroundersAggt.length; i++) {
            var surrounderAggt = surroundersAggt[i];

            var surrounderView = this._generateSurrounder(mainView, surrounderAggt);

            var surrounderResult = this._generateAggt(surrounderAggt, surrounderView, false);

            regions.surrounders.push({
                type: surrounderAggt.surrounderType,
                result: surrounderResult
            });
        }
    },

    //生成环绕块
    _generateSurrounder: function (mainView, surrounderAggt) {
        /// <returns type="SIE.view.View" />
        var vf = this._vf;
        var cr = SIE.view.RelationView; //common realtion

        var surrounderType = surrounderAggt.surrounderType;
        var surrounderBlock = surrounderAggt.mainBlock;

        var surrounderView = null;
        var relation = null;
        var reverseRelation = null; //相反的关系类型
        this._initMetaConfig(surrounderBlock);
        if (surrounderType === cr.condition) {
            surrounderView = vf.createConditionView(surrounderBlock);
            reverseRelation = new SIE.view.RelationView(cr.result, mainView);
        }
        else if (surrounderType === cr.navigation) {
            surrounderView = vf.createNavigationView(surrounderBlock);
            reverseRelation = new SIE.view.RelationView(cr.result, mainView);
        }
        else if (surrounderType === 'Common') {
            surrounderView = vf.createListView(surrounderBlock);
            return surrounderView;
        }
        else {
            SIE.notSupport();
        }

        relation = new SIE.view.RelationView(surrounderType, surrounderView);

        //直接使用 surrounderType 作为关系的类型，把 surrounderView 添加到 mainView 的关系。
        mainView._setRelation(relation);

        //相反的关系设置
        surrounderView._setRelation(reverseRelation);
        if (surrounderView.hasListeners['onviewready']) {
            surrounderView.fireEvent('onviewready', true);
        }
        return surrounderView;
    },

    //布局
    _layout: function (aggtMeta, regions) {
        /// <summary>
        /// 对所有区域进行布局。
        /// </summary>
        /// <param name="aggtMeta" type="SIE.Web.ClientMetaModel.ClientAggtMeta"></param>
        /// <param name="regions" type="SIE.autoUI.Regions"></param>
        /// <returns type="Ext.Component" />
        var layout = null;
        if (aggtMeta.layoutClass) {
            layout = Ext.create(aggtMeta.layoutClass);
        }
        else {
            layout = new SIE.autoUI.layouts.Common();
        }

        var res = layout.layout(regions);

        return res;
    }
});
/**
 * AutoUI类
 */
Ext.define('SIE.AutoUI', {
    singleton: true,

    /**
     * 构造器
     */
    constructor: function () {
        this.viewFactory = new SIE.autoUI.ViewFactory();
        //this.aggtUI = new SIE.autoUI.AggtUIGeneratorDefault(this.viewFactory);
    },

    //meta
    getMeta: function (op) {
        /// <summary>
        /// 获取指定的元数据
        /// </summary>
        /// <param name="op">
        /// module 和 model 必须指定一个。
        ///     module: '', 如果是获取某个模块的元数据，则指定此参数为模块名。
        ///     model: '', 如果获取某个实体的元数据，则这个参数表示实体类名。在实体类模式下，可以选填以下两种方式
        ///         templateType: ''，此参数只在 isAggt 为 true 时有用，表示自定义的聚合块模板类型名称。
        ///         viewName: ''，如果 isAggt 为 true，表示使用的定义的扩展聚合块名称。否则表示扩展视图名称。
        /// isAggt: false
        /// isReadonly: false
        /// ignoreComands: false
        /// isDetail: false
        /// isLookup: false
        /// async: true
        /// callback: 回调，参数如下：
        ///     SIE.Web.ClientMetaModel.ClientAggtMeta
        /// </param>

        var o = SIE.meta.MetaService;
        return o.getMeta.apply(o, arguments);
    },

    //ui
    createListView: function () {
        var o = this.viewFactory;
        return o.createListView.apply(o, arguments);
    },

    createDetailView: function () {
        var o = this.viewFactory;
        return o.createDetailView.apply(o, arguments);
    },

    createConditionView: function (mainView, conditionBlock) {
        var o = this.viewFactory;
        var conditionView = o.createConditionView(conditionBlock);
        var reverseRelation = new SIE.view.RelationView('result', mainView);
        relation = new SIE.view.RelationView('Condition', conditionView);
        mainView._setRelation(relation);
        conditionView._setRelation(reverseRelation);
        return conditionView;
    },

    createCommands: function (cmds, view) {
        var o = this.viewFactory;
        return o.createCommands(cmds, view);
    },

    /**
     * 生成Aggt控件
     */
    generateAggtControl: function () {
        var generator = arguments[0].uiGenerator ? arguments[0].uiGenerator : 'SIE.autoUI.AggtUIGeneratorDefault';
        var o = Ext.create(generator);
        o._vf = this.viewFactory;
        return o.generateControl.apply(o, arguments);
    }
});


Ext.define('SIE.UITemplate', {
    extend: 'Ext.util.Observable',

    _model: null,
    _serverTemplateType: null,

    constructor: function (meta) {
        this.callParent(arguments);

        //this.addEvents('blocksDefined', 'uiGenerated');
    },

    getServerTemplate: function () {
        /// <summary>
        /// 设置对应的服务端模板类
        /// </summary>
        /// <returns></returns>
        return this._serverTemplateType;
    },
    setServerTemplate: function (value) {
        /// <summary>
        /// 设置对应的服务端模板类
        /// </summary>
        /// <param name="value">
        /// value 是该类在服务端的 AssemblyQuanifyName，如："SIE.Library.Audit.AuditItem, SIE.RBAC"
        /// </param>
        this._serverTemplateType = value;
    },
    getModel: function () {
    	/// <summary>
        /// 返回本模板对应的实体类型。
    	/// </summary>
    	/// <returns type=""></returns>
        return this._model;
    },
    setModel: function (value) {
    	/// <summary>
        /// 设置本模板对应的实体类型。
    	/// </summary>
    	/// <param name="value">可以是字符串，也可以是实体类型。</param>
        this._model = value;
    },

    createUI: function (model) {
        /// <summary>
        /// 为当前的实体类型生成 ui 界面。
        /// </summary>
        /// <param name="model"></param>
        /// <returns type="SIE.autoUI.ControlResult"></returns>

        //如果指定了 model，则更新本模板当前使用的实体类型
        if (model) { this.setModel(model); }

        var blocks = this.defineBlocks();
        var ui = this._createUICore(blocks);
        this._onUIGenerated(ui);
        return ui;
    },
    defineBlocks: function () {
        /// <summary>
        /// 获取本模板定义的聚合块。
        /// </summary>
        /// <returns type="SIE.Web.ClientMetaModel.ClientAggtMeta"></returns>
        var blocks = this._defineBlocksCore();

        this._onBlockDefined(blocks);

        return blocks;
    },

    _defineBlocksCore: function () {
        /// <summary>
        /// protected virtual
        /// 子类可以重写此方法来实现本地定义聚合块。
        ///
        /// 默认使用同步的方式，获取服务器上定义的块定义。
        /// </summary>
        /// <returns></returns>

        var blocks = null;

        SIE.AutoUI.getMeta({
            async: false,
            model: SIE.getModelName(this._model),
            templateType: this._serverTemplateType,
            isAggt: true,
            callback: function (result) { blocks = result; }
        });

        return blocks;
    },
    _onBlockDefined: function (blocks) {
    	/// <summary>
        /// protected virtual
    	/// </summary>
    	/// <param name="blocks"></param>
        this.fireEvent('blocksDefined', { blocks: blocks });
    },
    _createUICore: function (blocks) {
    	/// <summary>
        /// protected virtual
    	/// </summary>
    	/// <param name="blocks"></param>
    	/// <returns type=""></returns>
        return SIE.AutoUI.generateAggtControl(blocks);
    },
    _onUIGenerated: function (ui) {
    	/// <summary>
        /// protected virtual
    	/// </summary>
    	/// <param name="ui"></param>
        this.fireEvent('uiGenerated', { ui: ui });
    }
});


//模块的运行时类型。
Ext.define('SIE.ModuleRuntime', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
        //this.addEvents('uiGenerated');
    },

    createUI: function (module) {
        /// <summary>
        /// 根据模块元数据创建UI。
        /// </summary>
        var meta = this.getMeta(module);
        var ui = this.createAggtControl(meta, true);
        return ui;
    },

    getMeta: function (module) {
        /// <summary>
        /// 返回本模板对应的模块元数据类型。
        /// </summary>
        /// <returns type=""></returns>
        var meta = null;
        SIE.AutoUI.getMeta({
            async: false,
            module: module.keyLabel,
            model: module.model,
            callback: function (res) {
                meta = res;
            }
        });
        return meta;
    },

    createAggtControl: function (meta, autoLoad) {
        /// <summary>
        /// 生成组合界面控件
        /// </summary>
        /// <param name="meta">服务端返回的组合界面元数据</param>
        /// <returns type=""></returns>
        var ui = SIE.AutoUI.generateAggtControl(meta);

        var view = ui.getView();
        //最后一次创建的根视图，用于调试。
        window.$view = view;

        //用于样式调试
        if (view.isListView) {
            var styleConfig = {
                cls: "root-listview"
            };
            Ext.merge(view._control.config, styleConfig);
        }

        //如果没有导航/查询面板，则发动一次查询。
        if (autoLoad || !meta.surrounders) {
            if (view.isListView) {
                view.loadData();
            }
        }

        return ui;
    },
});


Ext.define('SIE.ModuleCollection', {
    //private
    extend: 'Ext.util.MixedCollection',//key:keyLabel value:moduleObj
    _root: null,

    //getByLabel: function (keyLabel) {
    //    /// <summary>
    //    /// 根据模块的唯一名称来查找模块元数据
    //    /// </summary>
    //    /// <param name="keyLabel">模块的唯一名称</param>
    //    /// <returns>模块元数据</returns>
    //    return this.getByKey(keyLabel);
    //},
    getRoot: function () {
        /// <summary>
        /// 获取根模块（根模块只是一个二级模块的容器，不对应任何具体的模块。）
        /// </summary>
        /// <returns></returns>
        return this._root;
    },

    _setRoot: function (rootModule) {
        /// <summary>
        /// internal
        /// 设置模块的根对象
        /// </summary>
        /// <param name="rootModule">
        /// 模块类型是纯 json 对象，对应服务端的 ModuleJson 类型，有以下属性：
        /// kayLabel, templateType, model, url, viewName,
        /// children
        /// </param>

        this._root = rootModule;
        this._addRecur(this._root);
    },
    _addRecur: function (item) {
        if (item.keyLabel) this.add(item.keyLabel, item);

        var children = item.children;
        if (children) {
            for (var i = 0; i < children.length; i++) {
                this._addRecur(children[i]);
            }
        }
    },
    setItem: function (module) {
        /// 设置项目，应对Module调整为一层.
        var children = module.children;
        if (children) {
            for (var i = 0; i < children.length; i++) {
                var item = children[i];
                this.add(item.keyLabel, item);
            }
        }
    }
});
Ext.define('SIE.App', {
    singleton: true,
    extend: 'Ext.util.Observable',

    constructor: function () {
        this.callParent(arguments);
        this._menus = new SIE.ModuleCollection();
        this._modules = new SIE.ModuleCollection();
    },

    //-------------------------------------  模块操作 -------------------------------------
    userInfo: null, //用户信息
    _menus: null, //菜单列表
    _modules: null, //模块列表
    _workspace: null, //工作区
    _currentModule: null, //最后一次显示的模块对象。
    CurCulture: null, //当前文化
    _resourceProvider: null, //资源提供者
    orgInfo: null, //用于测试

    getUserDisplayName: function () {
        /// <summary>
        /// 获取用户友好显示
        /// </summary>
        var displayName = '';
        var curUser = CRT.Context.GlobalContext.getContext(portal.userKey);
        if (curUser) {
            displayName = Ext.String.format('{0}[{1}]', curUser.Name, curUser.Code);
        }
        return displayName;
    },

    //弹框登录
    popupLogin: null,

    //检测当前用户是否已被切换
    checkCurUser: function () {
        var result = this.validateUserResult();
        if (!result.success) {
            SIE.Msg.showMessage(result.msg, function () {
                var clientUser = CurUserStateHelper.getCookieUser();
                CurUserStateHelper.setSessionUser(clientUser);
                window.top.location.replace('/');
            });
            return false;
        }
        return true;
    },
    validateUserResult: function () {
        var result = { success: true, msg: '' };
        var sessionUser = CurUserStateHelper.getSessionUser();
        var clientUser = CurUserStateHelper.getCookieUser();
        if (SIE.isEmptyObject(sessionUser) || SIE.isEmptyObject(clientUser)) {
            result.success = false;
            result.msg = '用户会话状态已过期，页面将重新刷新'.t();
            return result;
        }
        if (sessionUser.Id != clientUser.Id) {
            result.success = false;
            result.msg = '检测到登陆账户已变化，页面将重新刷新'.t();
            return result;
        }
        if (sessionUser.CurInvOrg != clientUser.CurInvOrg) {
            result.success = false;
            result.msg = '帐户当前组织已变化，页面将重新刷新'.t();
            return result;
        }
        return result;
    },

    getMenus: function () {
        /// <summary>
        /// 获取授权的菜单。
        /// </summary>
        return this._menus;
    },
    getModules: function () {
        /// <summary>
        /// 获取所有模块的列表。
        /// </summary>
        /// <returns type="SIE.ModuleCollection"></returns>
        return this._modules;
    },
    showModule: function (opt) {
        /// <summary>
        /// 创建并显示某个模块
        /// </summary>
        /// <param name="opt">
        /// 字符串或配置对象。
        /// 
        /// module：系统中唯一的模块或名称。
        /// //container: null，类型为 Ext.Container。生成的模块应该放在这个容器中。如果未指定，则直接显示在 Body 中。
        /// </param>
        /// <returns type="">返回模块对象。</returns>

        //参数处理。找到对应的模块对象。
        if (!opt) {
            SIE.emptyArgument('opt');
        }
        if (Ext.isString(opt) || opt.keyLabel) {
            opt = {
                module: opt
            };
        }
        var module = opt.module;
        if (Ext.isString(module)) {
            module = this._modules.getByKey(module);
        }
        if (!module) {
            SIE.error("没有找到对应的模块：".t() + opt.module);
        }

        //如果该模块已经存在，则设置该模块为当前模块，并返回。
        var ws = this.getWorkspace();
        if (ws) {
            var modules = ws.getModules();
            var exists = SIE.findFirst(modules,
                function (m) {
                    return m == module;
                });
            if (exists) {
                ws.setCurrentModule(module);
                return;
            }
        }

        //为模块生成模块控件
        var ui = this._createModuleControl(module);
        var v = ui.getView();
        v.module = module.keyLabel;
        ui._control.view = v;

        //如果指定了 Workspace，则把生成的模块控件加入到 Workspace 中；否则就显示在指定的 container 中。
        if (ws) {
            ws._addModule(module.keyLabel, ui.getControl(), opt.container);
        } else {
            this._showInContainer(opt.container, ui.getControl());
        }

        //生成完毕，发生事件。
        this._onModuleCreated(ui, module.keyLabel);
        this._currentModule = module;

        return module;
    },
    getCurrentModule: function () {
        /// <summary>
        /// 返回当前显示的模块对象。
        /// </summary>
        /// <returns type=""></returns>
        return this._currentModule;
    },
    getWorkspace: function () {
        /// <summary>
        /// 返回工作区对象。如果返回 null，表示没有工作区对象，应用程序不能承载多个模块。
        /// 
        /// 工作区对象有以下方法：
        /// module[] getModules() //返回当前已经打开的所有模块的集合。
        /// setCurrentModule(module) //可设置当前模块。
        /// removeModule(module) //移除某个已经打开的模块。
        /// </summary>
        /// <returns type="Object"></returns>
        return this._workspace;
    },
    setWorkspaceProvider: function (provider) {
        /// <summary>
        /// 设置模块的提供器。
        /// 本方法只能调用一次。
        /// </summary>
        /// <param name="provider">
        /// provider 需要实现以下几个方法，所有的参数都是字符串。
        /// module[] getModules()
        /// setCurrentModule(module)
        /// addModule(module, moduleControl)
        /// removeModule(module)
        /// </param>
        var v = provider;
        if (!v.getModules || !v.setCurrentModule || !v.addModule || !v.removeModule) {
            SIE.notSupport("provider 必须支持以下方法：getModules、setCurrentModule、addModule、removeModule".t());
        }

        var app = this;

        app._workspace = {
            _provider: v,
            getModules: function () {
                var res = [];
                //获取字符串数组
                var moduleNames = this._provider.getModules();

                //转换为模块数组。
                var modules = app.getModules();
                SIE.each(moduleNames,
                    function (name) {
                        var module = modules.getByKey(name);
                        if (module) {
                            res.push(module);
                        }
                    });

                return res;
            },
            getCurrentModule: function () {
                /// <summary>
                /// 返回当前工作区中打开的模块。
                /// </summary>
                /// <returns type="Object"></returns>
                return app._currentModule;
            },
            setCurrentModule: function (value) {
                /// <summary>
                /// 设置工作区的当前模块。
                /// </summary>
                /// <param name="value">字符串或模块对象。</param>
                /// <returns type=""></returns>
                value = this._toModuleItem(value);

                app._currentModule = value;

                value = this._toModuleLabel(value);

                return this._provider.setCurrentModule(value.keyLabel);
            },
            removeModule: function (module) {
                /// <summary>
                /// 移除指定的模块
                /// </summary>
                /// <param name="module"></param>
                /// <returns type=""></returns>
                module = this._toModuleLabel(module);

                return this._provider.removeModule(module);
            },
            _addModule: function (module, control, tab) {
                return this._provider.addModule(module, control, tab);
            },
            _toModuleItem: function (module) {
                if (Ext.isString(module)) {
                    module = app.getModules().getByKey(module);
                }
                return module;
            },
            _toModuleLabel: function (module) {
                if (!Ext.isString(module)) {
                    module = module.keyLabel;
                }
                return module;
            }
        };
    },

    /**
     * 多语言
     */
    L10N: function (raw) {
        if (this._resourceProvider) {
            try {
                return this._resourceProvider._translate(raw);
            } catch (err) {
                console.warn(err.message);
                return raw;
            }
        } else {
            console.warn('can not find resourceProvider');
            return raw;
        }
    },

    /**
     * 设置资源提供者
     * @param {} provider 
     * @returns {} 
     */
    setResourceProvider: function (provider) {
        var app = this;
        app._resourceProvider = {
            _translate: function (raw) {
                return provider.translate(raw);
            },
            _initResource: function () {
                provider.initResource();
            }
        }
    },

    _createModuleControl: function (module) {
        /// <summary>
        /// 生成某个模块的控件
        /// </summary>
        /// <param name="module">客户端模块对象。</param>
        /// <returns type="">返回模块对应的控件对象。</returns>

        //根据自定义模块生成界面
        var runtime = module.moduleRuntime;
        if (runtime) {
            var mr = Ext.create(runtime);
            return mr.createUI(module);
        }

        //根据模块到服务器查找对象的界面元数据并生成。
        var meta = null;
        SIE.AutoUI.getMeta({
            async: false,
            module: module.keyLabel,
            model: module.model,
            callback: function (res) {
                meta = res;
            }
        });
        var ui = this._createAggtControl(meta, module.tryAutoLoadData); //ControlResult;
        return ui;
    },
    _onModuleCreated: function (moduleUI, moduleName) {
        this.fireEvent('moduleCreated', {
            moduleUI: moduleUI,
            moduleName: moduleName
        });
    },

    showModel: function (opt) {
        /// <summary>
        /// 显示某个模块
        /// </summary>
        /// <param name="opt">
        /// model: 当未指定 module 时，
        /// container: null(Ext.Container)。生成的模块应该放在这个容器中。如果未指定，则直接显示在 Body 中。
        /// viewName: '', 如果使用某个实体类的扩展视图，则这个参数指定扩展视图的名称。
        /// isAggt: true。是否显示聚合模型。
        /// </param>
        opt = Ext.apply({
            isAggt: true,
            viewName: ''
        },
            opt);

        var me = this;
        SIE.AutoUI.getMeta({
            model: opt.model,
            viewName: opt.viewName,
            isAggt: opt.isAggt,
            callback: function (meta) {
                if (!opt.isAggt) {
                    var listView = SIE.AutoUI.createListView(meta);
                    //最后一次创建的根视图，用于调试。
                    window.$view = listView;

                    listView.loadData();

                    me._showInContainer(opt.container, listView.getControl());
                } else {
                    var ui = me._createAggtControl(meta, true);

                    me._showInContainer(opt.container, ui.getControl());
                }
            }
        });
    },
    //此方法已修改，见方法showViewDetail（gc）
    showDetail: function (meta, entity, editmode, text, callback) {
        var me = this;
        if (!text) {
            text = "查看".L10N() + meta.mainBlock.label.L10N();
        }

        var blocks;
        var view;
        var items;
        if (meta.mainBlock) {
            blocks = meta;
            meta = blocks.mainBlock;

            var ui = SIE.AutoUI.generateAggtControl(blocks, entity);
            view = ui.getView();
            //view.formSpecialHandle(view.formConfig, entity);
            view.setCurrent(entity);
            items = ui.getControl();
        } else {
            view = SIE.AutoUI.createDetailView(meta, entity);
            items = view.getControl();
        }
        items.view = view;
        view.associateCmd = this.associateCmd;
        if (editmode == "Popup") {
            SIE.Window.show({
                title: text,
                minWidth: 320,
                //autoScroll: false,//不需要使用自动滚动，否则会出现异常的滚动条。//需要滚动条，因为需要拖动窗口大小。
                items: view.getControl(),
                callback: function (btn) {
                    if (btn == "确定".t()) {
                        if (!view.validateData()) {
                            SIE.Msg.showMessage('请先填写正确的值！'.t());
                            return false;
                        }
                        if (callback)
                            return callback(true);
                    } else {
                        var entity = view.getData();
                        entity.reject();
                    }
                }
            });
        } else {
            var entityId = entity.entityName + '-' + entity.getId();
            CRT.Workbench.addPage({
                id: entityId.replace(/\./g, ''),
                text: text,
                url: '',
                control: items
            });
        }
        return view;
    },

    showViewDetail: function (cfg) {
        if (cfg.editMode == "Popup") {
            this.showDialog(cfg);
        } else {
            var entityType;
            if (cfg.viewMeta.mainBlock) {
                entityType = cfg.viewMeta.mainBlock.model;
            } else {
                entityType = cfg.viewMeta.model;
            }
            CRT.Workbench.addPage({
                entityType: entityType,
                recordId: cfg.entity.data.Id,
                isDetail: true
            })
        }
    },

    /**
    * 获取当前页签面板
    */
    getTabPanel: function () {
        return Ext.getCmp('centerTab');
    },
    /**
    * 关闭当前页签
    */
    closeTab: function () {
        //var tabPanel = Ext.getCmp('centerTab');
        //var activeTab = tabPanel.getActiveTab();
        //tabPanel.remove(activeTab);
        CRT.Workbench.closeTab();
    },
    _addTabs: function (items, blocks) {
        if (!blocks.children)
            return;

        var tabs = [];
        var view;
        for (var i = 0; i < blocks.children.length; i++) {
            var block = blocks.children[i].mainBlock;
            if (block.gridConfig) {
                view = SIE.AutoUI.createListView(block);
            } else {
                view = SIE.AutoUI.createDetailView(block);
            }

            var control = view.getControl();
            tabs.push({
                title: block.label,
                items: control,
                layout: 'fit',
                autoScroll: true
            });
        }

        items.add({
            xtype: 'tabpanel',
            items: tabs,
            layout: 'fit',
            autoScroll: true
        });
    },

    _showInContainer: function (container, control) {
        //如果未指定容器，则直接显示在 Body 中。
        if (!container) {
            container = Ext.widget('viewport', {
                border: 0,
                layout: 'fit', //如果不添加 fit，则模块直接在页面中显示时，无法显示。例如：http://localhost:8000/default/module/?module=部门权限管理
                autoScroll: true
            });
        }

        //生成的聚合界面可能比较大，所以需要显示“加载中”
        container.setLoading(true);
        setTimeout(function () {
            var tabPanel = Ext.getCmp('centerTab');
            container.add(control);
            container.setLoading(false);
        },
            10);
    },
    _createAggtControl: function (meta, autoLoad) {
        /// <summary>
        /// 生成组合界面控件
        /// </summary>
        /// <param name="meta">服务端返回的组合界面元数据</param>
        /// <returns type=""></returns>
        var ui = SIE.AutoUI.generateAggtControl(meta);

        var view = ui.getView();
        //最后一次创建的根视图，用于调试。
        window.$view = view;

        //用于样式调试
        if (view.isListView) {
            var styleConfig = {
                cls: "root-listview"
            };
            Ext.merge(view._control.config, styleConfig);
        }

        //如果没有导航/查询面板，则发动一次查询。
        if (autoLoad || !meta.surrounders) {
            if (view.isListView) {
                view.loadData();
            }
        }

        return ui;
    },
    //-------------------------------------  启动 -------------------------------------
    startup: function (callback) {

        this._initPlugins();
        //初始化Ext的Tips
        Ext.QuickTips.init();

        this.fireEvent('loginSuccessed', callback);
        //this.fireEvent('loginFailed');
        this.fireEvent('startupCompleted');

    },
    runLogPlug: function (pluginArr) {

        if (!pluginArr)
            return;

        var me = this;
        //这里的 plugin 就不需要排序了，因为 js 输出的顺序是在服务端排序过的。
        var plugins = SIE._getPlugins();
        for (var i = 0; i < plugins.length; i++) {
            //构造实例，并替换到数组中
            var pluginType = plugins[i];
            var p = Ext.create(pluginType);
            plugins[i] = p;
            if (Ext.Array.contains(pluginArr, p.$className)) {
                if (p._logSystem) {

                    p._logSystem(me);
                }
            }
        }
    },
    _initPlugins: function () {
        var me = this;
        //这里的 plugin 就不需要排序了，因为 js 输出的顺序是在服务端排序过的。
        var plugins = SIE._getPlugins();
        for (var i = 0; i < plugins.length; i++) {
            //构造实例，并替换到数组中
            var pluginType = plugins[i];
            var p = Ext.create(pluginType);
            plugins[i] = p;
            if (p.init) {
                p.init(me);
            }
        }
    },
    //-------------------------------------  启动 -------------------------------------

    showDialog: function (cfg) {
        //先调用现有的，需要重构更加结构化
        this.associateCmd = cfg.associateCmd;
        // return this.showViewDetail(cfg);
        var meta = cfg.viewMeta;
        var callback = cfg.confirm;
        if (!cfg.title) {
            cfg.title = "查看".L10N() + meta.mainBlock.label.L10N();
        }
        var blocks;
        var view;
        var items;
        if (meta.mainBlock) {
            blocks = meta;
            meta = blocks.mainBlock;
            var ui = SIE.AutoUI.generateAggtControl(blocks, cfg.entity);
            view = ui.getView();
            view.setCurrent(cfg.entity);
            items = ui.getControl();
        } else {
            view = SIE.AutoUI.createDetailView(meta, cfg.entity);
            items = view.getControl();
        }
        items.view = view;
        view.associateCmd = this.associateCmd;
        //弹窗值设置
        var wincfg = {};
        if (cfg.dialogcfg)
            wincfg = cfg.dialogcfg;//弹窗属性
        wincfg.title = cfg.title;//设置弹窗标题
        wincfg.callback = function (btn) {
            if (btn == "确定".t()) {
                if (!view.validateData()) {
                    SIE.Msg.showMessage('请先填写正确的值！'.t());
                    return false;
                }
                if (callback)
                    return callback(true);
            } else {
                var entity = view.getData();
                var entityId = entity.getId(); //弹窗修改时，如果实体是新增的，Id未保存，点击取消会把生成的Id撤销
                entity.reject();
                entity.setId(entityId);
            }
        };
        //显示弹窗
        if (meta.children) {
            wincfg.items = items;
        }
        else {
            wincfg.items = view.getControl();
            wincfg.minWidth = 320;
        }
        if (!wincfg.width) {
            wincfg.minWidth = 320;
        }
        //是否启用弹窗自适应窗口大小
        wincfg.disableWinAutoSize = cfg.disableWinAutoSize
        var win = SIE.Window.show(wincfg);
        view._win = win;    //关联view和窗口
        return view;
    },

    showView: function (cfg) {
        this.associateCmd = cfg.associateCmd;
        return this.showViewDetail(cfg);
        // return this.showDetail(cfg.viewMeta, cfg.entity, cfg.editMode, cfg.title, cfg.confirm);
    },

    devReportPrint: function (cfg) {
        var config = Ext.Object.merge({
            method: "GET",
            url: null,
            iframeName: "DevPrintingFrame",
            params: []

        }, cfg);

        var doc = top.document || document;

        //if (!doc.getElementById(config.iframeName)) {
        //    var devFrame = doc.createElement("iframe");
        //    devFrame.style = "position:absolute; left: -10000px; top: -10000px;width:0px;height:0px;visibility:collapse";
        //    devFrame.name = config.iframeName;
        //    devFrame.id = config.iframeName;
        //    doc.body.appendChild(devFrame);

        //}

        var tempForm = doc.createElement("form");
        //制定发送请求的方式为post  
        tempForm.method = config.method || "GET";
        //此为window.open的url，通过表单的action来实现  
        tempForm.action = config.url;
        //利用表单的target属性来绑定window.open的一些参数（如设置窗体属性的参数等）  
        tempForm.target = "_blank"//config.iframeName;
        if (config.params) {
            for (var i in config.params) {
                var hideInput = doc.createElement("input");
                hideInput.type = "hidden";
                hideInput.name = i;
                hideInput.value = config.params[i];
                //将input表单放到form表单里  
                tempForm.appendChild(hideInput);
            }
        }

        //将此form表单添加到页面主体body中  
        doc.body.appendChild(tempForm);
        //手动触发，提交表单  
        tempForm.submit();
        //从body中移除form表单  
        doc.body.removeChild(tempForm);
    }
});
Ext.define('SIE.window.Msg', {
    // 工具类：统一的消息提示框 模拟WPF.IMessageService
    alternateClassName: 'SIE.Msg',
    constructor: function (timeout, config) {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="timeout" type="number">超时时长</param>
        /// <param name="config" type="object">配置值</param>
        if (timeout) {
            this.timeout = timeout;
        }
        Ext.apply(this, config);
    },
    timeout: 3, //3秒倒时计

    //------------ private
    _showMsg: function (cfg) {
        Ext.Msg.resizable = true;
        var millisecond = 1000;
        var title = cfg.title.t();
        var timeout = cfg.timeout;
        cfg = Ext.apply({ minWidth: 300 }, cfg);
        cfg.title = title;
        cfg.message = (cfg.message || cfg.msg).replace(/\r\n/g, '<br/>'); //补充为Html的换行符
        if (cfg.timeout && Ext.isNumber(cfg.timeout)) {
            cfg.title = Ext.String.format('{0}({1}){2}', title, timeout, '秒'.t());
            var setTitle = function (args) {
                timeout--;
                var display = Ext.String.format('{0}({1}){2}', title, timeout, '秒'.t());
                if (msgBox) {
                    msgBox.setTitle(display);
                    if (timeout === 0) {
                        Ext.uninterval(timerId);
                        msgBox.hide();
                    }
                }
            }

            var args = [];
            var timerId = Ext.interval(setTitle, millisecond, this, args);
        }
        var msgBox = Ext.Msg.show(cfg);
        msgBox.on("beforehide", function (panel, eOpts) {
            Ext.uninterval(timerId);
        });
        return msgBox;
    },

    //------------public
    confirm: function (msg, fn) {
        /// <summary>
        /// 显示是否确认框
        /// </summary>
        /*
        问答框示例:
        SIE.Msg.confirm('是否确认?',function () {
            console.log('点击是的回调方法');
        });
        */
        return this._showMsg({
            title: '确认'.t(),
            msg: msg,
            buttons: Ext.Msg.YESNO,
            defaultFocus: Ext.Msg.NO,
            icon: Ext.Msg.QUESTION,
            iconCls: 'iconfont icon-Notice1',
            fn: function (btnId) {
                if (btnId === 'yes') {
                    if (fn) {
                        fn.apply(this, this.arguments);
                    }
                }
            }
        });
    },
    askQuestion: function (msg, ok_fn, cancle_fn) {
        /// <summary>
        /// 显示确认取消问答框
        /// </summary>
        /*
        问答框示例:
        SIE.Msg.askQuestion('提问信息?'askQuestion,function () {
            console.log('点击ok时的回调方法');
        });
        */
        return this._showMsg({
            title: '提醒'.t(),
            msg: msg,
            buttons: Ext.Msg.OKCANCEL,
            icon: Ext.Msg.QUESTION,
            iconCls: 'iconfont icon-Notice1',
            fn: function (btnId) {
                if (btnId === 'ok') {
                    if (ok_fn) {
                        ok_fn.apply(this, this.arguments);
                    }
                } else if (btnId === 'cancel') {
                    if (cancle_fn) {
                        cancle_fn.apply(this, this.arguments);
                    }
                }
            }
        });
    },
    showWarning: function (msg) {
        /// <summary>
        /// 显示警示框
        /// </summary>
        /*
        示例:
        SIE.Msg.showWarning('警告信息');
        */
        return this._showMsg({
            title: ('警告'.t()),
            msg: msg,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.WARNING
        });
    },
    showMessage: function (msg, ok_fn) {
        /// <summary>
        /// 显示信息框
        /// </summary>
        /*
       示例:
       SIE.Msg.showMessage('提示信息');
       示例2: 提示信息,点击确认后回调
       SIE.Msg.showMessage('提示信息',function () {
            console.log('点击ok时的回调方法');
        });
       */
        return this._showMsg({
            title: ('提示'.t()),
            msg: msg,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.INFO,
            fn: function (btnId) {
                if (btnId === 'ok') {
                    if (ok_fn) {
                        ok_fn.apply(this, this.arguments);
                    }
                } 
            }
        });
    },
    showError: function (msg) {
        /// <summary>
        /// 显示信息框
        /// </summary>
        /*
       示例:
       SIE.Msg.showError('错误信息');
       */
        return this._showMsg({
            title: ('错误'.t()),
            msg: msg,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.ERROR,
            width: 500
        });
    },
    showInstantMessage: function (msg, title, timeout, ok_fn) {
        /// <summary>
        /// 显示信息框(倒计时关闭的)
        /// </summary>
        /*
       示例:
       SIE.Msg.showInstantMessage('倒计时信息');
       秒控制，两种方式
       SIE.Msg.showInstantMessage('倒计时信息','标题',3); //只控制这一个的，传参，如3秒
       SIE.Msg.timeout=5; //这个是全局设置 控制所有倒计时的
       SIE.Msg.showInstantMessage('倒计时信息'); 5秒倒计时
       */
        return this._showMsg({
            title: (title || '倒计时提示'.t()),
            msg: msg,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.INFO,
            iconCls: 'iconfont icon-Timer',
            timeout: timeout || this.timeout,
            fn: function (btnId) {
                if (btnId === 'ok') {
                    if (ok_fn) {
                        ok_fn.apply(this, this.arguments);
                    }
                }
            }
        });
    },
    wait: function (message, title, config) {
        /// <summary>
        /// 等待信息框,窗口没有关闭按钮，进度条会一直循环，直到代码关闭。
        /// </summary>
        /** 示例：
         * SIE.Msg.wait('msg');
         */
        if (Ext.isString(message)) {
            message = {
                title: title || '等待框'.t(),
                message: message,
                progressText: '',
                wait: true,
                modal: true,
                waitConfig: config
            };
        }
        return this._showMsg(message);
    },
    progress: function (title, message, progressText) {
        /// <summary>
        /// 进度条信息框,窗口有关闭按钮，需要自己更新进度条。
        /// </summary>
        /** 示例：
         * SIE.Msg.progress('title','msg','10%');
         */
        if (Ext.isString(title)) {
            title = {
                title: title,
                message: message,
                progress: true,
                progressText: progressText
            };
        }
        return this._showMsg(title);
    },
    hide: function () {
        Ext.MessageBox.hide();
    },
    close: function () {
        Ext.MessageBox.close();
    },
    showToast: function (html, title) {
        /// <summary>
        /// 显示自动消息的提示框
        /// </summary>
        /// <param name="html" type="string/object">内容说明-(支持使用Html)</param>
        /// <param name="title" type="string">标题说明</param>
        Ext.toast({
            html: html,
            title: title,
            closable: false,
            align: 't',
            slideInDuration: 800
        });
    },
    // end 
}, function (Msg) {
    Ext.onInternalReady(function () {
        SIE.MessageBox = SIE.Msg = Ext.create('SIE.Msg');
    });
});
(function () {

    String.prototype.format = function (args) {
        var result = this;
        if (arguments.length > 0) {
            if (arguments.length == 1 && typeof (args) == "object") {
                for (var key in args) {
                    if (args[key] != undefined) {
                        var reg = new RegExp("({" + key + "})", "g");
                        result = result.replace(reg, args[key]);
                    }
                }
            }
            else {
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i] != undefined) {
                        var reg = new RegExp("({)" + i + "(})", "g");
                        result = result.replace(reg, arguments[i]);
                    }
                }
            }
        }
        return result;
    };

    if (typeof String.prototype.startsWith !== 'function') {
        String.prototype.startsWith = function (str) {
            var reg = new RegExp("^" + str);
            return reg.test(this);
        };
    }

    if (typeof String.prototype.endsWith !== 'function') {
        String.prototype.endsWith = function (str) {
            var reg = new RegExp(str + "$");
            return reg.test(this);
        };
    }

    if (typeof HTMLCollection.prototype.forEach !== 'function') {
        HTMLCollection.prototype.forEach = function (fn, scope) {
            var i, ln;
            for (i = 0, ln = this.length; i < ln; i++) {
                fn.call(scope, this[i], i, array);
            }
        }
    }

    var storeExt = {
        isDirty: function () {
            if (this.getRemovedRecords().length) { return true; }
            if (this.getNewRecords().length) { return true; }
            if (this.getModifiedRecords().length) { return true; }

            //如果其它的任意项是脏的，则整个集合也是脏的。
            var dirtyItem = SIE.each(this, function (entity) {
                if (entity.isChildrenDirty()) return false;
            });
            if (dirtyItem) {
                return true;
            }

            return false;
        }
    };
    Ext.apply(Ext.data.Store.prototype, storeExt);
    Ext.apply(Ext.data.TreeStore.prototype, storeExt);

    //给 TreeStore 添加一个 loadRawData 的方法，这样，Store 和 TreeStore 都有这个方法了。
    Ext.apply(Ext.data.TreeStore.prototype, {
        loadRawData: function (serverData) {
            this.setRootNode(serverData);
        }
    });

    Ext.apply(Ext.data.field.Integer.prototype, {
        allowNull: true
    });

    Ext.override(Ext.data.Store, {
        ///扩展一个clone方法
        clone: function (newConfig) {
            newConfig = Ext.apply({
                model: this.model,
                proxy: this.proxy,
                lastOptions: this.lastOptions,
                remoteSort: this.remoteSort
            }, newConfig || {
            });  //允许外部配置覆盖

            var ds = Ext.create('Ext.data.Store', newConfig);
            if (newConfig.cloneData) {
                ds.data = this.data.clone();
                ds.modified = [].concat(this.modified);
            }
            return ds;
        }
    });

    Ext.override(Ext.tree.Panel, {
        filterByText: function (text) {
            this.filterBy(text, 'text');
        },
        /**
         * 根据字符串过滤所有的节点，将不符合条件的节点进行隐藏.
         * @param 查询字符串.
         * @param 要查询的列.
         */
        filterBy: function (text, by) {

            var treeStore = this.getStore();
            treeStore.clearFilter();
            if (text === "") {
                return;
            }


            var view = this.getView(),
                me = this,
                nodesAndParents = [];

            // 找到匹配的节点并展开.
            // 添加匹配的节点和他们的父节点到nodesAndParents数组.
            this.getRootNode().cascadeBy(function (tree, view) {
                var currNode = this;

                if (currNode && currNode.data[by] && currNode.data[by].toString().toLowerCase().indexOf(text.toLowerCase()) > -1) {
                    me.expandPath(currNode.getPath());

                    while (currNode.parentNode) {
                        nodesAndParents.push(currNode.id);
                        currNode = currNode.parentNode;
                    }
                }
            }, null, [me, view]);

            treeStore.filterBy(function (record, id) {
                //console.log(record);
                //console.log(record.data.text);

                if (Ext.Array.contains(nodesAndParents, record.getId()) || record.getId() == 0) {
                    return true;
                }

            });
            
            view.setScrollY(0);
        }
    });

    //Ext.data.Store 扩展添加field和移除field函数，移除field会同时清除data对应字段的值
    Ext.override(Ext.data.Store, {
        addField: function (field) {

            this.model.addFields(field);
            Ext.each(this.data.items, function (o) {
                if (typeof field[0].defaultValue != 'undefined') {
                    o.set(field[0].name, field[0].defaultValue);
                }

            });
        },
        removeField: function (name) {

            if (typeof name == 'string') {
                name = [name];
            }

            this.model.removeFields(name);
            Ext.each(this.data.items, function (o) {
                delete o[name[0]];
            });

        }
    });

    //Ext.grid.ColumnModel 扩展添加表格列和移除表格列函数，单独使用不会移除数据中绑定字段和数据中的值
    Ext.override(Ext.grid.ColumnModel, {
        addColumn: function (column, colIndex) {
            var grid = this.headerCt.grid;
            var gridColumns = grid.config.columns;



            var dynamicColumn = Ext.merge({ isDynamic: true }, column);

            if (typeof colIndex == 'number') {
                gridColumns.splice(colIndex, 0, dynamicColumn);
            } else {
                colIndex = gridColumns.push(dynamicColumn);
            }

            grid.reconfigure(grid.store, gridColumns);
            return colIndex;
        },
        removeColumn: function (colIndex) {
            var header = null;
            if (typeof colIndex == 'number') {
                header = this.getHeaderAtIndex(colIndex);;
            }
            else {
                header = this.getHeaderByDataIndex(colIndex);
            }


            var grid = this.headerCt.grid;
            var gridColumns = grid.config.columns;
            //处理列索引: header.fullColumnIndex - 1
            gridColumns.splice(header.fullColumnIndex - 1, 1);
            grid.reconfigure(grid.store, gridColumns);

        }
    });

    //Ext.grid.GridPanel 扩展添加表格列和移除表格列函数
    Ext.override(Ext.grid.GridPanel, {
        addColumn: function (field, column, colIndex) {


            var fields = [field];
            this.store.addField(fields);
            this.columnManager.addColumn(column, colIndex);
        },
        // 调用此方法的时候不需要处理列索引，底层方法会进行处理
        removeColumn: function (colIndex) {

            var header = null;
            if (typeof colIndex != 'number') {
                header = this.columnManager.getHeaderByDataIndex(colIndex);
            }
            else {
                header = this.columnManager.getHeaderAtIndex(colIndex);
            }
            if (header) {
                this.store.removeField(header.dataIndex);
                this.columnManager.removeColumn(header.fullColumnIndex);
            }
        },   
        //实现列表悬浮框（gc）
        afterRender: Ext.Function.createSequence(Ext.grid.GridPanel.prototype.afterRender,
            function () {
                if (!this.cellTip) {
                    return;
                }
                var view = this.getView();
                var toolTipCfg = GlobalConfig.defaultToolTipCfg;
                toolTipCfg.target = view.el;
                toolTipCfg.delegate = '.x-grid-cell-inner';
                toolTipCfg.renderTo = document.body;
                //添加监听事件
                toolTipCfg.listeners.beforeshow = function updateTipBody(tip) {

                    var record = view.getRecord(tip.triggerElement);
                    var tipIndex = view.getHeaderByCell(tip.triggerElement.parentElement) != null ? view.getHeaderByCell(tip.triggerElement.parentElement).tipIndex : null;
                    var tipText = record != null ? record.data[tipIndex] : null;
                    if (!tipText) {

                        var tipText = (tip.triggerElement.innerText || tip.triggerElement.textContent);
                    }
                    tipText = tipText.toString();
                    if (Ext.isEmpty(tipText) || Ext.isEmpty(tipText.trim())) {
                        return false;
                    }
                    if (tipText.match(SIE.htmlTagsPatt)) {
                        tipText = Ext.htmlEncode(tipText);
                    }
                    tip.update(tipText);
                };
                this.tip = new Ext.ToolTip(toolTipCfg);


            })
        //--afterRender结束--
    });

    //Ext.Function 扩展
    /**
     * 防抖
     * @param {function} fn--一个函数
     * @param {number} interval--一个数字
     * @param {boolean} immediate-true为立即执行
     * @param {object} scope--作用域
     */
    Ext.Function.createDebounce = function (fn, interval, immediate, scope) {
        var timer;
        return function () {
            var context = scope || this;
            var args = arguments;
            if (timer)
                clearTimeout(timer);
            if (immediate) {
                var callNow = !timer;
                timer = setTimeout(function () {
                    timer = null;
                }, interval);
                if (callNow)
                    fn.apply(context, args);
            } else {
                timer = setTimeout(function () {
                    fn.apply(context, args);
                }, interval);
            }
        };
    };

    /**
     * 常用日期格式化模式
     * */
    Ext.Date.patterns = {
        ISO8601Long: "Y-m-d H:i:s",
        ISO8601Short: "Y-m-d",
        ShortDate: "n/j/Y",
        LongDate: "l, F d, Y",
        FullDateTime: "l, F d, Y g:i:s A",
        MonthDay: "F d",
        ShortTime: "g:i A",
        LongTime: "g:i:s A",
        SortableDateTime: "Y-m-d\\TH:i:s",
        UniversalSortableDateTime: "Y-m-d H:i:sO",
        YearMonth: "F, Y"
    };
})();

Ext.define('Overrides.dom.UnderlayPool', {
    override: 'Ext.dom.UnderlayPool',
    /**
     * Override to check if el is destroyed
     */
    checkOut: function () {
        var el = this.cache.shift();

        // If el is destroyed shift again
        if (el && el.isDestroyed) {
            el = this.cache.shift();
        }

        if (!el) {
            el = Ext.Element.create(this.elementConfig);
            el.setVisibilityMode(2);


            el.dom.setAttribute('data-sticky', true);
        }
        return el;
    }
});

Ext.define('XH-OVERRIDE.dom.Element', {

    override: 'Ext.dom.Element',
    setStyle: function (prop, val) {
        return this.dom ? this.callParent([prop, val]) : this;
    }
});

Ext.define('ux.form.field.multiFileButton', {
    extend: 'Ext.form.field.FileButton',
    alias: 'widget.multifilebutton',
    afterRender: function () {
        var me = this;
        me.callParent(arguments);
        if (me.getConfig('multiple')) {
            me.fileInputEl.dom.setAttribute('multiple', true);
        }
    },
});
Ext.define('SIE.ExportExcelHelper', {
    singleton: true,
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);

    },
    /**
     * Excel导出
     * @method excelDownload
     * @param  title 标题
    * @param data 数据源
    * @param mask 遮罩
     */
    excelDownload: function (title, data, mask) {
        var jsonData = JSON.parse(data.rows);
        if (jsonData.length === 0) {
            SIE.Msg.showInstantMessage('没有需要导出的数据！'.t());
            return false;
        }
        var view = null;
        SIE.AutoUI.getMeta({
            async: false,
            model: data.type,
            callback: function (meta) {
                view = SIE.AutoUI.createListView(meta);
            }
        });

        view.getData().setData(jsonData);
        view.getData().commitChanges();
        this.excelDownloadCommon(view, false, title, mask);
    },
    /** 
     * Excel导出
     * @method export
     * @param  cfg gridConfig
    * @param datas 数据源列表eg. view.getSelection() or view.getData().getData().items
    * @param title 标题
    * @param mask 遮罩
     */
    exportXls: function (cfg, datas, title, mask) {
        mask = mask || this.showMask(Ext.getBody().component, '下载中...');
        var task = new Ext.util.DelayedTask(function () {
            var columns = Ext.create('Ext.exporter.data.Column');
            cfg.columns.forEach(function (col) {
                if (col.xtype === 'rownumberer') return true;
                columns.addColumn({ text: col.header, width: col.width || 140});
            });

            var rows = [];
            datas.forEach(function (item) {
                var row = Ext.create('Ext.exporter.data.Row');
                cfg.columns.forEach(function (col) {
                    var colIdx = col.dataIndex;
                    if (!!!colIdx) return true; // rownumberer没有dataIndex
                    if (colIdx.indexOf('Id') > 0 && item.data[colIdx + '_Display'] !== undefined) {
                        val = item.data[colIdx + '_Display'];
                    }
                    else {
                        val = item.data[colIdx];
                        if (col.xtype === 'comboboxcolumn') {
                            if (col.editor.store.data[0] instanceof Array) {//全局快码
                                var arr = col.editor.store.data.filter(function (d) { return d[0] === val });
                                if (arr.length > 0)
                                    val = arr[0][1];
                            } else {//枚举
                                var arr = col.editor.store.data.filter(function (d) { return d[col.editor.valueField] === val });
                                if (arr.length > 0)
                                    val = arr[0][col.editor.displayField];
                            }
                        }
                        if (col.xtype === 'comboColumn') {//快码
                            var arr = JSON.parse(col.editor.store.data).filter(function (d) { return d.Code === val });
                            if (arr.length > 0)
                                val = arr[0].Name;
                        }
                        if (col.xtype === 'checkboxcolumn') {//布尔
                            val = val ? '是' : '否';
                        }
                    }
                    if (val instanceof Date) {
                        var fmt = 'Y-m-d';
                        if (Ext.Date.format(val, 'H:i:s') !== "00:00:00")
                            fmt = 'Y-m-d H:i:s';
                        val = Ext.Date.format(val, fmt);
                    }
                    row.addCell({ value: val });
                });
                rows.push(row);
            });

            var excelData = Ext.create('Ext.exporter.data.Table', {
                columns: columns,
                rows: rows
            });
            var excel = Ext.create('Ext.exporter.excel.Xlsx', {
                fileName: title + Ext.Date.format(new Date(), 'YmdHis') + '.xlsx',
                title: title,
                author: 'SIE',
                data: excelData
            });
            excel.saveAs();
            SIE.Msg.showInstantMessage(Ext.String.format("成功导出【{0}】笔数据！".L10N(),rows.length));
            mask.hide();
        });
        task.delay(50);
    },

    /** @deprecated
     * Excel导出共用类
     * @method excelDownloadCommon
     * @param  view 视图
    * @param onlySels 是否是选中数据源
    * @param title 标题
    * @param mask 遮罩
     */
    excelDownloadCommon: function (view, onlySels, title, mask) {
        mask = mask || this.showMask(Ext.getBody().component, '下载中...');
        var task = new Ext.util.DelayedTask(function () {
            //var style = Ext.create('Ext.exporter.file.Style', {
            //    alignment: 'Automatic',//Center
            //    font: {
            //        color: '#aabbcc',
            //        family: 'Consolas, Arial'
            //    }
            //});
            //var columns = Ext.create('Ext.exporter.data.Column', { style: style });
            var columns = Ext.create('Ext.exporter.data.Column');
            view.gridConfig.columns.forEach(function (col) {
                if (col.xtype === 'rownumberer') return true;
                columns.addColumn({ text: col.header, width: col.width || 140 });
            });

            var rows = [];
            var datas = [];
            if (onlySels)
                datas = view.getSelection();
            else
                datas = view.getData().getData().items;
            datas.forEach(function (item) {
                var row = Ext.create('Ext.exporter.data.Row');
                view.gridConfig.columns.forEach(function (col) {
                    var colIdx = col.dataIndex;
                    if (!!!colIdx) return true; // rownumberer没有dataIndex
                    var val = null;
                    if (colIdx.indexOf('Id') > 0 && item.data[colIdx + '_Display'] !== undefined) {
                        val = item.data[colIdx + '_Display'];
                    }
                    else {
                        val = item.data[colIdx];
                        if (col.xtype === 'comboboxcolumn') {
                            if (col.editor.store.data[0] instanceof Array) {//全局快码
                                var arr = col.editor.store.data.filter(function (d) { return d[0] === val });
                                if (arr.length > 0)
                                    val = arr[0][1];
                            } else {//枚举
                                var arr = col.editor.store.data.filter(function (d) { return d[col.editor.valueField] === val });
                                if (arr.length > 0)
                                    val = arr[0][col.editor.displayField];
                            }
                        }
                        if (col.xtype === 'comboColumn') {//快码
                            var arr = JSON.parse(col.editor.store.data).filter(function (d) { return d.Code === val });
                            if (arr.length > 0)
                                val = arr[0].Name;
                        }
                        if (col.xtype === 'checkboxcolumn') {//布尔
                            val = val ? '是'.t() : '否'.t();
                        }
                    }
                    if (val instanceof Date) {
                        var fmt = 'Y-m-d';
                        if (Ext.Date.format(val, 'H:i:s') !== "00:00:00")
                            fmt = 'Y-m-d H:i:s';
                        val = Ext.Date.format(val, fmt);
                    }
                    row.addCell({ value: val });
                });
                rows.push(row);
            });

            var excelData = Ext.create('Ext.exporter.data.Table', {
                columns: columns,
                rows: rows
            });
            title = title || view.label;
            var excel = Ext.create('Ext.exporter.excel.Xlsx', {
                fileName: title + Ext.Date.format(new Date(), 'YmdHis') + '.xlsx',
                title: title,
                author: 'SIE',
                data: excelData
            });
            excel.saveAs();
            SIE.Msg.showInstantMessage(Ext.String.format("成功导出【{0}】笔数据！".L10N(),rows.length));
            mask.hide();
        });
        task.delay(50);
    },

    /**
     * 显示遮罩
     * @method showMask
     * @param  target 目标控件
    * @param msg 显示信息
     */
    showMask: function (target, msg) {
        msg = msg || '读取中...'.t();
        var mask = new Ext.LoadMask({
            target: target,
            msg: msg,
        });
        mask.show();
        return mask;
    }
});
Ext.define('SIE.grid.column.AceCode', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.AceCodeColumn'],

    /**
     * @cfg producesHTML
     * @inheritdoc
     */
    producesHTML: false,

    defaultRenderer: function (value) {
        var me = this;
        var editor = me.getEditor ? me.getEditor() : me.editor;
       
        return me._valueOrEmptyCellTest(value);
    },
    _valueOrEmptyCellTest: function (retureVal) {
        //////
        ///补充空值占位符&nbsp, 否则在表格TD,在树上渲染样式不完整
        //////
        var me = this;
        return retureVal || me.emptyCellText;
    },
    updater: function (cell, value) {
        Ext.fly(cell).down(this.getView().innerSelector, true).innerHTML = SIE.grid.column.AceCode.prototype.defaultRenderer.call(this, value);
    }
});
Ext.define('SIE.grid.column.ComboBox', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.comboboxcolumn'],

    /**
     * @cfg producesHTML
     * @inheritdoc
     */
    producesHTML: false,

    defaultRenderer: function (value) {
        var me = this;
        var editor = me.getEditor ? me.getEditor() : me.editor;
        if (editor) {
            var store = editor.getStore ? editor.getStore() : editor.store;
            if (store) {
                if (!store.isStore)
                    store = Ext.create('Ext.data.Store', store);
                var data = store.findRecord(editor.valueField, value);
                if (data)
                    return me._valueOrEmptyCellTest(data.get(editor.displayField));
            }
        }
        return me._valueOrEmptyCellTest(value);
    },
    _valueOrEmptyCellTest:function(retureVal){
        //////
        ///补充空值占位符&nbsp, 否则在表格TD,在树上渲染样式不完整
        //////
        var me = this;
        return retureVal || me.emptyCellText;
    },
    updater: function (cell, value) {
        Ext.fly(cell).down(this.getView().innerSelector, true).innerHTML = SIE.grid.column.ComboBox.prototype.defaultRenderer.call(this, value);
    }
});
Ext.define('SIE.grid.column.ComboList', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.combolistcolumn'],

    /**
     * @cfg producesHTML
     * @inheritdoc
     */
    producesHTML: false,
    defaultRenderer: function (value, context) {
        if (!value)
            return "";

        var me = this;
        var bindDisplayField;
        var editor = me.getEditor ? me.getEditor() : me.editor;
        if(!editor)
            return value;
        if (context) {
            bindDisplayField = editor.bindDisplayField;
            if(!bindDisplayField)
                bindDisplayField = me.dataIndex+"_Display";

            if(context.isModel)
                return me._valueOrEmptyCellTest(context.get(bindDisplayField) || context.get(me.dataIndex));
            else if(context.record)
                return me._valueOrEmptyCellTest(context.record.get(bindDisplayField) || context.record.get(me.dataIndex));
            else
                return me._valueOrEmptyCellTest(value);
        }
        else{
            var store = editor.getPicker().getStore();
            record = store.data.find(editor.valueField, value);
            if(record)
                return me._valueOrEmptyCellTest(record.get(editor.displayField) || record.get(me.dataIndex));
            else
                return me._valueOrEmptyCellTest(value);
        }
        return me._valueOrEmptyCellTest(value);
    },
    _valueOrEmptyCellTest:function(retureVal){
        //////
        ///补充空值占位符&nbsp, 否则在表格TD,在树上渲染样式不完整
        //////
        var me = this;
        return retureVal || me.emptyCellText;
    },
    updater: function (cell, value, record) {
        var editor = this.getEditor ? this.getEditor() : this.editor;
        if (editor) {
            var store = editor.getStore? editor.getStore():editor.store;
            if (store && store.isStore) {
                var record = store.findRecord(editor.valueField, value);
                if (record) {
                    var displayValue = record.get(editor.displayField);
                    record.set(editor.bindDisplayField, displayValue);
                }
            }
        }
        Ext.fly(cell).down(this.getView().innerSelector, true).innerHTML = this.defaultRenderer(value, record);
    }
});

Ext.define('SIE.grid.column.GridComboPopup', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.gridcombopopupcolumn'],
    producesHTML: false,
    defaultRenderer: function (value, context) {
        return value;
    }
});
/**
 * ComboBox扩展，修改空值下拉行高问题
 */
Ext.define('SIE.control.XComboBox', {
	extend: 'Ext.form.field.ComboBox',
	alias: 'widget.xcombobox',
	_SelectItems: [],
	defaultListConfig: {
		loadingHeight: 70,
		minWidth: 70,
		maxHeight: 300,
		shadow: 'sides',
		getInnerTpl: function(displayField) {
			return '{[values["' + displayField + '"].trim() == "" ? "&nbsp;": values["' + displayField + '"]]}';
		},
	},
	initComponent: function() {
		var me = this;
        me.callParent(arguments);
		if (me.ischeckbox) {
			me.multiSelect = true;
			me.listConfig = {
				itemTpl: Ext.create('Ext.XTemplate', '<input type=checkbox>{text}'),
				onItemSelect: function(record) {
					var me = this;
					var node = me.getNode(record);
					if (node) {
						Ext.fly(node).addCls(me.selectedItemCls);
						var checkboxs = node.getElementsByTagName("input");
						if (checkboxs != null) {
							var checkbox = checkboxs[0];
							checkbox.checked = true;
							me.up().setValue(me.up()._SelectItems);
						}
					}
				},
				listeners: {
					itemclick: function(view, record, item, index, e, eOpts) {
						var me = this;
						var isSelected = view.isSelected(item);
						var checkboxs = item.getElementsByTagName("input");
						if (checkboxs != null) {
							var checkbox = checkboxs[0];
							if (!isSelected) {
								checkbox.checked = true;
								me.up()._SelectItems.push(record.data);
							} else {
								checkbox.checked = false;
								var index = me.up()._SelectItems.indexOf(record.data);
								me.up()._SelectItems.splice(index, 1);
							}
						}
					}
				}
			}
		} 
	},
	setValue: function(value) {
		var me = this;
        me.callParent(arguments);
        if (me.ischeckbox) {
            var entity;
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record;
            }
            entity.data[me.name] = me.value;
        }
    }
});
Ext.define('SIE.control.ComboPopup', {
	extend: 'Ext.form.field.Picker',
	//extend: 'SIE.control.ComboBoxTest',
	alias: 'widget.combo_popup',
	triggerCls: "x-form-search-trigger",
	matchFieldWidth: false,
	//override default config
	editable: false,
	pageSize: 50,
	//matchFieldWidth: false,
	//config
	model: '',
	dataSourceProperty: '',
	selectOnClose: false,
	enableDoubleClick: true,
	//绑定的显示字段
	bindDisplayField: null,
	windowHeight: 500,
	windowWidth: 800,
	initComponent: function() {
		var me = this;
		me.callParent();

		if (!me.bindDisplayField) {
			me.bindDisplayField = me.getName() + "_Display";
		}
	},
	//####################################################################################
	//####################################################################################
	//override
	onInputElClick: function(e) {
		//this.onTriggerClick(this, this.getPickerTrigger(), e);
	},
	//override
	onTriggerClick: function(field, trigger, e) {
		var me = this;
		if (me._layouting === true) return;

		me._changeSelectionAfterShow = false;
		me.editingRecord = me._getContainRecord();
		if (me._layouted) {
			me._win.show();
			return;
		}

		if (!me._layouting) {
			me._layouting = true;
			me._createLayout();
		}
	},
	//override
	onFocusLeave: function(e) {
		var me = this;
		me.callParent(e);
		me._onFocusLeaveIntegrate();
	},
	_onFocusLeaveIntegrate: function() {
		var me = this;
		var rawValue = me.getRawValue();
		if (me.lastSelectionRecord) {
			var isEdited = me.lastSelectionRecord.rawValue != rawValue;
			if (isEdited) {
				me.setValue(null);
			}
		}
	},
	_getContainRecord: function() {
		var editor = this;
		var container = editor.up('container');
		while (container && !container.context) {
			container = container.up('container');
		}
		if (container) {
			return container.context.record;
		}

		return null;
	},
	/**
	 * 创建弹出窗体布局
	 */
	_createLayout: function() {
		var me = this;
		SIE.AutoUI.getMeta({
			model: me.model,
			ignoreChild: true,
			ignoreCommands: true,
			isReadonly: true,
			ignoreQuery: false,
			isAggt: true,
			callback: function(blocks) {
				me._queryBlockProcess(blocks);
				me._gridBlockProcess(blocks);
				var ui = SIE.AutoUI.generateAggtControl(blocks);
				me._popupWin(ui, me.inputEl);
				me._layouted = true;
				//me._reloadTargetViewData();
			}
		});
	},
	/**
	 * 查询块配置设置
	 * @param block 块配置
	 */
	_queryBlockProcess: function(block) {
		if (block.surrounders && block.surrounders.length) {
			var surround = block.surrounders[0];
			var items = surround.mainBlock.formConfig.items;
			for (var i = 0; i < items.length; ++i) {
				items[i].readOnly = false;
			}
		}
	},
	/**
	 * 设置grid的块配置
	 * @param block grid块配置
	 */
	_gridBlockProcess: function(block) {
		var me = this;
		var multiSelect = me.multiSelect;
		var gridConfig = block.gridConfig || block.mainBlock.gridConfig;
		gridConfig.selModel = {
			injectCheckbox: 0,
			//checkbox位于哪一列，默认值为0
			selType: 'checkboxmodel',
			//checkbox
			checkOnly: true,
			//只能通过checkbox选择
			mode: multiSelect //(multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
		};
		gridConfig.viewConfig = {
			enableTextSelection: true,
			//启用文本选中
			getRowClass: function(record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value && me.multiSelect != "Multi") {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        return 'gridRowLock';
                    }
                }
            }
        };

		gridConfig.pagingBarConfig = {
			_displayInfoOnSimple: false,
			afterPageText: '/&nbsp{0}页'.t(),
			displayMsg: '共{2}条'.t(),
			_pageSize: me.pageSize
		};

	},
	/**
	 * 弹出窗体
	 * @param ui
	 * @param source
	 */
	_popupWin: function(ui, source) {
		var me = this;
		me._targetView = ui._view;
		me._uiControl = ui.getControl();
		me._win = SIE.Window.show({
			title: ('选择' + me._targetView.label).t(),
			animateTarget: source,
			items: ui.getControl(),
			modal: false,
			closeAction: 'hide',
			height: me.windowHeight || 500,
			width: me.windowWidth || 800,
			callback: function(btn) {
				if (btn === '确定'.t()) {
                    if (me._targetSelectItems.keys.length > 0)
						me._onConfirm();
                    else
                        me.setValue(null);
					return true; //阻止窗口关闭，在save中根据返回结果处理
				} else if (btn === '取消'.t()) {
					me.isCanceling = true;
					return true;
				}
			}
		});

		me._setGridListeners();
		me._targetSelectItems = {
			items: [],
			keys: []
		};

		me._setWinListeners();
		me.grid = me._targetView.getControl();

		me.grid.store.on('load',
		function() {
			me._targetSelectItems = {
				items: [],
				keys: []
			};
		});

		delete me._layouting;
	},
	/**
	 * grid 绑定事件
	 */
	_setGridListeners: function() {
		var me = this;
		var grid = me._targetView.getControl();

		me.mon(grid.getSelectionModel(), {
			scope: me,
			select: me._onSelect,
			deselect: me._onDeselect,
			//rowdblclick: me._onRowdblClick
			//itemdblclick : me._onRowdblClick//( this, record, item, index, e, eOpts ) 
		});
        //多选不注册双击事件
		if (me.enableDoubleClick&&this.multiSelect != "Multi") {
			me.mon(grid.getView(), {
				rowdblclick: function(vthis, record, element, rowIndex, e, eOpts) {
					me._onRowdblClick(vthis, record, element, rowIndex, e, eOpts);
				}
			});
		}
	},
	_doAutoSelectOnClose: function() {
		var me = this;
		if (me.selectOnClose && me._changeSelectionAfterShow) {
			if (me._targetSelectItems.keys.length > 0) {
				if (!me.isCanceling) {
					me._onConfirm();
				}
			}
		}

	},
	/**
	 * 弹窗界面事件绑定
	 */
	_setWinListeners: function() {
		var me = this;
		me._win.on('focusleave',
		function(vthis, event, eOpts) {
			if (!event.toElement || me._win.owns(event.toElement) === false) {
				if (me._win.hasFocus === false) {
					me._win.hide();
				}
			}
		},
		me._win, {
			delay: 50
		}); //可能移光标到 主控件*/
		me.on('focusleave',
		function(vthis, event, eOpts) {
			if (!event) return;

			if (me._win && event.toElement && me._win.owns(event.toElement) === false) {
				//me._win.focus();
				me._win.hide();
			}
		},
		me, {
			delay: 50
		});
        
		me._win.on('hide',
		function() {
			//console.log('关闭中-A-' + me.isCanceling);
			me._doAutoSelectOnClose();
			delete me.isCanceling;
		});

		me._win.on('show',
		function() {
			var needQuery = false;
			if (!me.value) {
				//me.getRawValue() != me.lastSelectionRecord.rawValue
				needQuery = true;
			}

			if (needQuery) {
				//下拉列表没有选择值时，打开一个 新的放大镜查询框
				var searchCmds = me._uiControl.query('button[command=SIE.cmd.ExecuteQuery]');
				if (searchCmds && searchCmds.length) {
					searchCmds[0].handler();
				}
			}

			me.grid.getView().refresh();
		},
		me, {
			delay: 50
		});

	},
	/**
	 * 弹窗确定按钮设置值
	 */
	_onConfirm: function() {
		var me = this;
        if (this.multiSelect == "Multi"){
		    me.setMULTIValue();
		} else {
		    me.setValue(me._targetSelectItems.items[0]);
		}
		me._win.hide();
	},
	/**
	 * 设置多选值
	 */
	setMULTIValue: function() {
		var me = this;
        me.value=me._targetSelectItems.keys;
		var displayVal2 = "";
		var displayField = me.displayField;
		me._targetSelectItems.items.forEach(function(model) {
			displayVal2 += "," + model.data[displayField];
		});
		displayVal2 = displayVal2.substring(1);
        var entity;
        if(!me.up("form"))
            entity=me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
		entity.data[me.getName()] = me._targetSelectItems.keys;
		entity.data[me.bindDisplayField]=displayVal2; 
		me.setRawValue(displayVal2);
		me.lastSelectionRecord = {
			value: entity.data[me.getName()],
			rawValue: displayVal2
		};
		me.checkChange();
	},

	//override
	/**
	 * 在设置的同时，把选择项的 bindDisplayField 同步到记录上
	 * @param value 设置值
	 */
	setValue: function(value) {
		var me = this;
		var val2 = null;
		var displayVal2 = '';
        /*
          避免快码下拉列表（value="")先触发PropertyChanged后赋值
          (每次PropertyChanged后会删除监听事件)
         */
        if(value==="")
            val2 = value;
		// 设Id时，找出原来的DisplayFieldValue
		if (value && !value.isModel) {
			val2 = value;

			if (me.valueField != me.displayField) {
				if (me.column) {
					var cellEditor = me.up('container');
					while (cellEditor && !cellEditor.context) {
						cellEditor = cellEditor.up('container');
					}
					if (cellEditor) {
						displayVal2 = cellEditor.context.record.get(cellEditor.field.bindDisplayField);
					}
				} else {
					var binder = me.getBind();
					if (binder) {
						var bindRec = me._getBindRecord();
						if (bindRec) {
							displayVal2 = bindRec.get(me.bindDisplayField);
							if (me.model === "SIE.Common.Catalogs.Catalog") {
							    var data = Ext.JSON.decode(me.store.data, true) == null ? me.store.data : Ext.JSON.decode(me.store.data, true);
							    data.forEach(function (item) {
							        if (item[me.valueField] == value && !displayVal2) {
							            displayVal2 = item[me.displayField];
							        }
							    });
							}
						} else {
							//未绑定之前只是一个{}空对象(在从表,孙表)
							if (me.getRawValue() && me.lastSelectionRecord && me.lastSelectionRecord.rawValue && me.getRawValue() == me.lastSelectionRecord.rawValue) {

								displayVal2 = me.lastSelectionRecord.rawValue;
							}
						}

						//var owner = binder.value.owner;
						//displayVal2 = owner.get(me.bindDisplayField);
					}
				}
			}
		}

		var hasRecords = false;
		//设DisplayField.Value
		if (value && value.isModel) {
			val2 = value.get(me.valueField);
			displayVal2 = value.get(me.displayField);
			hasRecords = true;
		}

		me.value = val2;
		if (me.valueField == me.displayField) {
			//me.setHiddenValue(value);
			displayVal2 = val2;
		}
		me.setRawValue(displayVal2);
		me.lastSelectionRecord = {
			value: val2,
			rawValue: displayVal2
		};

		if (me.column) {
			var cellEditor = me.up('container');
			while (cellEditor && !cellEditor.context) {
				cellEditor = cellEditor.up('container');
			}
			if (cellEditor) {

				if (cellEditor.context.record.get(cellEditor.field.dataIndex) != val2) {
					cellEditor.context.record.beginEdit();

					cellEditor.context.record.set(cellEditor.field.dataIndex, val2);
					if (me.valueField != me.displayField) {
						cellEditor.context.record.set(cellEditor.field.bindDisplayField, displayVal2);
					}
					cellEditor.context.record.endEdit();

					var sieView = me._getSIEView();
					if (sieView) {
						sieView.syncCmdState();
					}
					//cellEditor.context.record.fire('change',);
					//cellEditor.grid.SIEView.refresh(); ////SIE.view.View syncCmdState
				}
			}
		} else {
			var binder = me.getBind();
			if (binder) {
				var bindRec = me._getBindRecord();
				if (bindRec) {
					if (me.valueField != me.displayField) {
						bindRec.set(me.name, val2);
						bindRec.set(me.bindDisplayField, displayVal2);
					}
				}
			}
		}

		me.checkChange();

		if (hasRecords) {
			me.fireEvent('select', me, [value]);
		}
		//this.callParent(value);
	},
	_getSIEView: function() {
		var me = this,
		cellEditor = me.up('container');

		while (cellEditor && !cellEditor.SIEView) {
			cellEditor = cellEditor.up('container');
		}

		if (!cellEditor || !cellEditor.SIEView) {
			return null;
		}

		return cellEditor.SIEView;
	},
	_getBindRecord: function() {
		var me = this;
		var binder = me.getBind();
		var ownerData = binder.value.owner.getData();
		var dataKey = Object.keys(ownerData);
		if (dataKey.length == 1) {
			//未绑定之前只是一个{}空对象(在从表,孙表)
            if (ownerData[dataKey] !== null && Ext.isFunction(ownerData[dataKey].set)) {
				return ownerData[dataKey];
			}
		} else {
			console.log('Bind内容.length应该只能等于1'.t());
			return null;
		}
	},
	_getContainerRecord: function() {
		var me = this;
		if (me.column) {
			var cellEditor = me.up('container');
			while (cellEditor && !cellEditor.context) {
				cellEditor = cellEditor.up('container');
			}
			if (cellEditor) {
				return cellEditor.context.record;
			}
		} else {
			var binder = me.getBind();
			if (binder) {
				var bindRec = me._getBindRecord();
				if (bindRec) {
					return bindRec;
				}
			}
		}

		return null;
	},
	//override
	getValue: function() {
		var me = this;
		return me.value;
	},
	//override
	doDestroy: function() {
		//去除Win 实例
		var me = this;
		if (me._win) {
			if (me._win.isVisible()) {
				me._win.hide();
			}
			me._win.destroy();
		}
		me.callParent();
	},
     /** 
	 * 复选框勾选事件
	 * @param selModel 选择模式
	 * @param record 选择的记录
	 * @param index 行索引号
	 * @param eOpts  The options object passed to Ext.util.Observable.addListener.
	 */
	_onSelect: function(selModel, record, index, eOpts) {
		var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getId(), 0);
		if (idx === -1) {
			this._targetSelectItems.keys.push(record.getId());
			this._targetSelectItems.items.push(record);
			this._changeSelectionAfterShow = true;
		}
	},
	/**
	 * 复选框取消勾选事件
	 * @param selModel 选择模式
	 * @param record 选择的记录
	 * @param index 行索引号
	 * @param eOpts The options object passed to Ext.util.Observable.addListener.
	 */
	_onDeselect: function (selModel, record, index, eOpts) {
		if (record) {
			var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getId(), 0);
			if (idx > -1) {
				//var item = this._targetSelectItems.items[idx];
				Ext.Array.removeAt(this._targetSelectItems.keys, idx);
				Ext.Array.removeAt(this._targetSelectItems.items, idx);
				this._changeSelectionAfterShow = true;
			}
		}
	},
	/**
	 * Grid行双击事件
	 * @param vthis 
	 * @param record
	 * @param element
	 * @param rowIndex 行索引
	 * @param e
	 * @param eOpts
	 */
	_onRowdblClick: function(vthis, record, element, rowIndex, e, eOpts ){
		var me = this;
		if (record) {
			me.setValue(record);
			me._win.hide();
		}
	}
});
Ext.define('SIE.control.ComboList', {
    extend: 'SIE.control.ComboPopup',
    alias: 'widget.combolist',
    triggerCls: "x-form-arrow-trigger",
    initComponent: function () {
        var me = this;
        me.enableKeyEvents = true;
        me.clearPropertiesOnDestroy = false;
        //me.focusOnToFront = false;
        me.callParent();
    },
    queryMode: 'remote',
    pageSize: 50,
    windowHeight: 290,
    windowWidth: 450,
    catalogReloadData: null, //快码类型专用 True查询调后台API 
    _SelectItems: [],
    _currentRowIndex: -1,
    _isQuerySelectItems: false,
    _delayTime: 300,//延迟加载时间(ms)
    _isdeferId: null,//当前延迟Id
    _sText: "",//上一次显示字段
    _isdeferTrue: true,//是否触发延迟
    isCtrl: false,//是否按下Ctrl按钮
    searchValue: "",//请求查询条件
    icount: 0,//Ctrl+V组合键次数
    listeners: {
        ///**
        // * 键盘按下事件
        // * (Ctrl+V必然是先按Ctrl,在按V所以要先判断Ctrl)
        // * @param {type} combo
        // * @param {type} e
        // * @param {type} eOpts
        // */
        keydown: function (combo, e, eOpts) {
            var me = this;
            if (!me.readOnly && !me.disabled) {
                console.log(e.keyCode);
                if (e.keyCode === e.CTRL) {
                    me.isCtrl = true;
                    return;
                }
                if (me.isCtrl && e.keyCode === e.V)
                    me.icount = 2;
            }
        },
        /**
         * 键盘松开事件
         * @param {type} combo
         * @param {type} e
         * @param {type} eOpts
         */
        keyup: function (combo, e, eOpts) {
            var me = this;
            if (!me.readOnly && !me.disabled) {
                console.log(e.keyCode);
                if (me.icount === 2) {
                    Ext.defer(function () {
                        me._isdeferTrue = false; me.searchValue = me.inputEl.dom.value; me.onTriggerClick();
                    }, me.delayTime);
                }
                if (me.icount > 0)
                    me.icount = me.icount - 1;
                if (e.keyCode !== e.CTRL && me.icount === 0) {//不是Ctrl组合键
                    if (me._sText !== me.inputEl.dom.value.trim()) {
                        if (me._isdeferId)
                            Ext.undefer(me._isdeferId);
                        me._sText = me.inputEl.dom.value.trim();
                        if (me.cbSearch) me.cbSearch.setRawValue(me._sText);
                        me._isdeferId = Ext.defer(function () {
                            me._isdeferTrue = false; me.searchValue = me.inputEl.dom.value; me.triggerisExpanded();
                        }, me.delayTime);
                    }
                }
            }
        },
    },

    /**
     * 下拉输入框(模糊查询)
     */
    triggerisExpanded: function () {
        var me = this;
        if (!me.isExpanded) {
            me.expand();
        }
        me._onSearchBoxTriggerClick();
        me._isdeferTrue = true;
    },

    _getViewMeta: function () {
        var me = this;
        var model = me.model;
        SIE.AutoUI.getMeta({
            async: false, //同步
            model: model, viewGroup: 'SelectionView', isLookup: true, isReadonly: true, ignoreCommands: true,
            callback: function (res) {
                if (res.mainBlock)
                    meta = res.mainBlock;
                else
                    meta = res;
            }
        });
        if (me.token)
            meta.token = me.token;

        me._isTree = SIE.getModel(model).isTree;

        Ext.applyIf(meta.gridConfig, {
            frame: false,
            //width: 450,
            columnLines: true,
            focusOnToFront: false,
            ownerCt: this.up('[floating]')
        });

        if (me.ischeckbox) {
            meta.gridConfig.selModel = {
                injectCheckbox: 0,
                //checkbox位于哪一列，默认值为0
                selType: 'checkboxmodel',
                //checkbox
                checkOnly: true,
                //只能通过checkbox选择
                mode: "MULTI" //(multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
            };

            me._targetSelectItems = {
                items: [],
                keys: []
            };
        }

        meta.gridConfig.viewConfig = {
            enableTextSelection: false,
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value) {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        if (!me.ischeckbox)
                            return 'gridRowLock';
                    }
                }
            }
        };

        if (me._isTree) {
            meta.gridConfig.useArrows = true;
        }
        meta.gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: true,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

        if (!me.catalogReloadData && me.store && me.store.data) {
            meta.storeConfig = me.store;
            if (typeof me.store.data == "string")
                meta.storeConfig.data = JSON.parse(me.store.data);

            meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
            me.pageSize = 100000;
        }

        Ext.apply(meta.storeConfig, { pageSize: me.pageSize });
        return meta;
    },
    /**
     * 分页控件事件的绑定
     * @param pagingBar 分页控件
     *  me.mon(pagingBar,{change:function (clt, newValue, oldValue, eOpts) { 
        }});
     */
    _pagingBarListeners: function (pagingBar) {
        return null;
    },
    /**
     * 创建下拉框面板
     * @returns 弹框窗体面板
     */
    createPicker: function () {
        var me = this;
        var meta = me._getViewMeta();
        me._view = SIE.AutoUI.createListView(meta);
        me.grid = me._view.getControl();
        var gridStore = me._view.getData();
        me.shadow = false;
        me._pagingBarListeners(me._view._pagingBar);
        //以防后台，无把固定Store设为 local
        me.queryMode = 'remote';
        if (gridStore && gridStore.proxy && gridStore.proxy.type == 'memory') {
            //console.log('不支持内存查询');
            me.queryMode = 'local';
        }

        me.cbSearch = Ext.create('Ext.form.field.ComboBox', {
            //padding :'2 2 2 2',
            margin: '5',
            triggerCls: "x-form-search-trigger",
            onTriggerClick: function () { me._onSearchBoxTriggerClick(); }
        });

        me.cbSearch.on('specialkey', function (field, e) {
            if (e.getKey() == e.ENTER) {
                me._onSearchBoxTriggerClick();
            }
        });

        me._win = Ext.create('Ext.window.Window', {
            title: '',
            closeAction: 'hide',
            closable: false,
            border: true,
            frame: false,
            header: false,
            focusOnToFront: false,
            layout: 'fit',
            height: me.windowHeight || 290,
            width: me.windowWidth || 450,
            tbar: me.cbSearch,
            items: [me.grid]
        });

        me.fieldKeyNav = new Ext.util.KeyNav({
            me: me,
            disabled: true,
            target: me.triggerEl
        });

        me._win.getNavigationModel = function () {
            return null;
        }
        me._setWinListeners();
        me._setGridListeners();
        me.on('beforedestroy', function (cmp) {
            if (me.cbSearch) me.cbSearch.destroy();
        });
        return me._win;
    },
    onExpand: function () {
        //console.log('onExpand');
        var me = this;
        me.callParent();
        me.cbSearch.setRawValue(me.inputEl.dom.value);
        var tableView = me.grid.getView();
        if (!me.up("form")) {
            if (me._currentRowIndex != me.up().context.rowIdx && me._currentRowIndex != -1) {
                me.cbSearch.setRawValue("");
                me._isQuerySelectItems = true;
                me._onSearchBoxTriggerClick();
            }
            me._currentRowIndex = me.up().context.rowIdx;
        }
        tableView.refresh();
    },
    onCollapse: function () {
        this.callParent();
    },
    onTriggerClick: function (comboBox, trigger, e) {
        var me = this,
            oldAutoSelect;
        if (!me.readOnly && !me.disabled) {
            if (me.isExpanded) {
                me.collapse();
            } else {
                if (me.isExpanded === false) {
                    me.expand();
                    me.cbSearch.setRawValue("");
                    if (me._isdeferTrue) {
                        if (me.dataSourceProperty) {         //使用自定义数据源的时候，希望每次下拉都重新加载数据，先这样写，待修改。
                            me._onSearchBoxTriggerClick();
                        }
                        else {
                            Ext.defer(function () {
                                //me.cbSearch.inputEl.focus(true);
                                //me.inputEl.focus(true); 
                                if (Ext.isDefined(me._lastSearchValue) == false || me._lastSearchValue != me.cbSearch.getRawValue()) {
                                    me.searchValue = "";
                                    me._onSearchBoxTriggerClick();
                                }
                            }, 300);
                        }
                    } else {
                        var criteriaData = {
                        };
                        criteriaData[me.displayField] = me.inputEl.dom.value;
                        me._AsynSearch(criteriaData,
                            function (result) {
                                //var flag = me.getBind() ? me.getBind().value : null;
                                //if (flag !== null) {
                                if (result[0] && result[0].length > 0) {
                                    me._SelectItems = [];
                                    var value = result[0][0].data[me.valueField];
                                    //bindEntity.data[fieldName] = me.value = value;
                                    //bindEntity.data[fieldName + '_Display'] = text;
                                    if (me.linkField) {
                                        me.linkField.forEach(function (key) {
                                            me._getbindEntity().set(key[0], result[0][0].data[key[1]]);
                                        });
                                    }
                                    me.setValue(result[0][0]);
                                    me._SelectItems.push(result[0][0]);
                                } else {
                                    //bindEntity.data[fieldName] = me.value = '';
                                    //bindEntity.data[fieldName + '_Display'] = inputEl.value = '';
                                    me.setValue(null);
                                    me._SelectItems = [];
                                }
                                if (!me.up("form")) {
                                    me.up('container').context.view.refresh();
                                }
                                if (me.grid) {
                                    me.grid.store.reload();
                                }
                                //}
                            });
                    }
                    me._isdeferTrue = true;
                }
            }
        }
    },

    _getbindEntity: function () {
        var me = this;
        var bindEntity;
        if (!me.bind || !me.bind.value) {
            var contet = me.up('container').context;
            var data = contet.record;
            bindEntity = data;
        } else {
            bindEntity = me.bind.value.owner.data;
            if (!(bindEntity instanceof Ext.data.Model)) {
                bindEntity = bindEntity.p;
            }
        }
        return bindEntity;
    },


    //失去焦点时获取值
    onBlur: function (e) {
        var me = this;
        if (!me.ischeckbox) {
            var inputEl = me.inputEl.dom;
            if (!me.up("form") && me.revertInvalid == false) {
                me.up('container').revertInvalid = me.revertInvalid;
            }
            me.callParent(e);

            var text = inputEl && inputEl.value;
            if (me.grid) {
                var pickerRecord = null;
                if (text && !me.value) {
                    me.grid.getStore().each(function (rec) {
                        if (rec.get(me.displayField) == text) {
                            pickerRecord = rec;
                            return false;
                        }
                    });
                }

                if (pickerRecord) {
                    me._SelectItems = [];
                    me.setValue(pickerRecord);
                    me._SelectItems.push(pickerRecord);
                    return;
                }
            }

            //var fieldName = me.name;
            //var bindEntity;
            //if (!me.bind || !me.bind.value) {
            //    var contet = me.up('container').context;
            //    var data = contet.record;
            //    bindEntity = data;
            //} else {
            //    bindEntity = me.bind.value.owner.data;
            //    if (!(bindEntity instanceof Ext.data.Model)) {
            //        bindEntity = bindEntity.p;
            //    }
            //}
            //var display = bindEntity.data[fieldName + '_Display'] != null ? bindEntity.data[fieldName + '_Display'] : me.value;
            //if (text && display != text) {
            //异步请求数据，获取value值
            //var criteriaData = {
            //};
            //criteriaData[me.displayField] = text;

            //me._AsynSearch(criteriaData,
            //    function (result) {
            //        var flag = me.getBind() ? me.getBind().value : null;
            //        if (flag !== null) {
            //            if (result[0] && result[0].length > 0) {
            //                me._SelectItems = [];
            //                var value = result[0][0].data[me.valueField];
            //                //bindEntity.data[fieldName] = me.value = value;
            //                //bindEntity.data[fieldName + '_Display'] = text;
            //                if (me.linkField) {
            //                    me.linkField.forEach(function(key) {
            //                        bindEntity.set(key[0], result[0][0].data[key[1]]);
            //                    });
            //                }
            //                me.setValue(result[0][0]);
            //                me._SelectItems.push(result[0][0]);
            //            } else {
            //                //bindEntity.data[fieldName] = me.value = '';
            //                //bindEntity.data[fieldName + '_Display'] = inputEl.value = '';
            //                me.setValue(null);
            //                me._SelectItems = [];
            //            }
            //            if (!me.up("form")) {
            //                me.up('container').context.view.refresh();
            //            }
            //            if (me.grid) {
            //                me.grid.store.reload();
            //            }
            //        }
            //    });
            //}
        }
    },
    _SetCurToFirst: function (items, store) {
        var me = this;
        var tableView = me.grid.getView();
        if (me.value && items.length > 0 && items[0].get(me.valueField) != me.value) {
            var isContains = false;
            for (var index = 0; index < items.length; index++) {
                var item = items[index];
                if (item.get(me.valueField) == me.value) {
                    items.splice(0, 0, item);
                    items.splice(index + 1, 1);
                    me._isQuerySelectItems = false;
                    isContains = true;
                    if (store) {
                        store.loadRecords(items);
                    }
                    tableView.refresh();
                    break;
                }
            }

            if (!isContains) {
                if (me._isQuerySelectItems) {
                    var criteriaData = {
                    };
                    criteriaData[me.displayField] = me.getRawValue();
                    me._isQuerySelectItems = false;
                    //异步请求
                    me._AsynSearch(criteriaData,
                        function (result) {
                            if (result[0] && result[0].length > 0) {
                                me._SelectItems = [];
                                me._SelectItems.push(result[0][0]);
                                items.splice(0, 0, me._SelectItems[0]);
                                if (store) {
                                    store.loadRecords(items);
                                }
                                tableView.refresh();
                            }
                        });
                } else {
                    if (me._SelectItems.length > 0) {
                        items.splice(0, 0, me._SelectItems[0]);
                        if (store) {
                            store.loadRecords(items);
                        }
                        tableView.refresh();
                    }
                }
            }
        }
    },
    _AsynSearch: function (criteriaData, callback) {
        var me = this;
        var view;
        if (!view) {
            var meta = me._getViewMeta();
            view = SIE.AutoUI.createListView(meta);
        }
        if (!me.dataSourceProperty) {
            var searchValues = criteriaData[me.displayField];
            me._lastSearchValues = searchValues;
            var filter = [];
            var property = {
                property: "_useLookUp",
                value: me.model,
                keyWord: searchValues
            };
            filter.push(property);
            var searchFieldList = [];
            me.searchFieldList.forEach(function (item) {
                var searchField = {
                    property: item
                };
                searchFieldList.push(searchField);
            });
            var data = {
                searchFieldList: searchFieldList
            };
            filter.push(data);
            view.loadData({
                filter: SIE.data.Utils.seriaizeRequest(filter),
                callback: function (result) {
                    if (callback && Ext.isFunction(callback)) {
                        callback(result);
                    }
                }
            });
        } else {
            var dsp = me.dataSourceProperty;
            if (!me.up("form"))
                model = me.up().context.grid.SIEView.model;
            else
                model = me.up("form").SIEView.model;
            var rec = me._getContainerRecord();
            var filter = {
                Parameters: {
                    EntityType: model,
                    Entity: rec.data,
                    DataSourceProperty: dsp
                }
            };
            var searchValues = criteriaData[me.displayField];
            view.loadData({
                action: 'lookup',
                filter: SIE.data.Utils.seriaizeRequest(filter),
                searchKeyWord: (searchValues ? searchValues : ''),
                page: 1,
                criteria: criteriaData,
                callback: function (result) {
                    if (callback && Ext.isFunction(callback)) {
                        callback(result);
                    }
                }
            });
        }
    },
    //override 
    _setWinListeners: function () {
        return null;
    },
    //override 
    _setGridListeners: function () {
        var me = this,
            grid = this.grid,
            store = grid.getStore();

        if (!me.ischeckbox) {
            me.mon(grid.getView(),
                {
                    rowdblclick: function (vthis, record, element, rowIndex, e, eOpts) {
                        me._onRowdblClick(vthis, record, element, rowIndex, e, eOpts);
                    }
                });
        } else {
            me.mon(grid.getSelectionModel(), {
                scope: me,
                select: me._onSelect,
                deselect: me._onDeselect,
            });
        }

        me.mon(store, {
            load: function (evObj, records, successful, operation, eOpts) {
                if (!me._isTree) {
                    if (me.ischeckbox) {
                        if (me._targetSelectItems && me._targetSelectItems.keys.length > 0) {
                            var selmodel = me._view.getSelectionModel();
                            for (var i = 0; i < records.length; i++) {
                                var record = records[i];
                                if (me._targetSelectItems.keys.indexOf(record.getId()) > -1) {
                                    selmodel.select(record, true, true);
                                }
                            }
                        }
                    } else {
                        me._SetCurToFirst(records, store);
                    }
                }
            }
        })
    },
    _onSearchBoxTriggerClick: function () {
        var me = this;
        var pageNum = me.grid.store ? me.grid.store.currentPage : 1;

        if (me.catalogReloadData === true) {
            me._searchByDSP(pageNum);
            return;
        }

        if (me.queryMode == 'remote') {
            var dsp = me.dataSourceProperty;
            if (dsp) {
                me._searchByDSP(pageNum);
            }
            else {
                me._searchByRawValue(pageNum);
            }
        }
        else {
            me.doLocalQuery();
        }
    },
    _searchByDSP: function (pageNum) {
        var me = this,
            dsp = this.dataSourceProperty;

        var sieView = me._getSIEView();
        //console.log(sieView);
        if (!sieView) {
            me._searchByRawValue();
            return;
            //viewGroup = view.viewGroup;
        }
        var filter = {
        };

        if (me.catalogReloadData === true) {
            filter = {
                Parameters: {
                    EntityType: me.model,
                    Entity: {
                    },
                    DataSourceProperty: dsp
                }
            };
        }
        else {
            var rec = me._getContainerRecord();

            filter = {
                Parameters: {
                    EntityType: sieView.model,
                    Entity: rec.data,
                    DataSourceProperty: dsp
                }
            };
        }
        me.searchValue = me.cbSearch.getRawValue();
        me._view.loadData({
            action: 'lookup',
            filter: SIE.data.Utils.seriaizeRequest(filter),
            searchKeyWord: (me.searchValue ? me.searchValue : ''),
            page: pageNum,
            criteria: null
        });
        me._lastSearchValue = me.searchValue;
    },
    _searchByRawValue: function (pageNum) {
        var me = this;
        me._searchText(pageNum, me.cbSearch.getRawValue());
    },

    /**
    * 请求查询(entity)[_useLookUp]
    * @param {type} pageNum
    * @param {type} text 查询条件
    */
    _searchText: function (pageNum, text) {
        var me = this;
        me._lastSearchValue = text;
        var filter = [];
        var property = {
            property: "_useLookUp",
            value: me.model,
            keyWord: text
        };
        filter.push(property);
        var searchFieldList = [];
        me.searchFieldList.forEach(function (item) {
            var searchField = {
                property: item
            };
            searchFieldList.push(searchField);
        });
        var data = {
            searchFieldList: searchFieldList
        };
        filter.push(data);
        me._view.loadData({
            filter: SIE.data.Utils.seriaizeRequest(filter),
            page: pageNum,
        });
    },

    doLocalQuery: function (queryPlan) {
        var me = this,
            store = me.grid.getStore(),
            queryString = me.cbSearch.getRawValue(),
            filter;

        store.clearFilter();
        if (queryString) {
            me.changingFilters = true;
            filter = me.queryFilter = new Ext.util.Filter({
                id: me.id + '-filter',
                anyMatch: true,
                caseSensitive: false,
                root: 'data',
                property: me.displayField,
                value: queryString
            });

            store.addFilter(filter, true);
            me.changingFilters = false;
        }
        me._lastSearchValue = queryString;
        me.grid.getView().refresh();
    },
    _getCriteriaModel: function () {
        var me = this;
        var criteria = null;
        try {
            criteria = Ext.create(me.model + 'Criteria');
        }
        catch (error) {
        }
        return criteria;
    },
    //排除_display、dirty字段  从QueryView复制
    _excludeField: function (criteria) {
        var filters = criteria.data;
        for (pro in filters) {
            if (Ext.String.endsWith(pro, '_Display') || pro == 'dirty')
                delete filters[pro];
        }
    },
    //查询 指定数据类型 的字段
    _findFieldsType: function (model, fieldType) {
        var arr = [];
        fieldType = fieldType.toLocaleLowerCase();

        Ext.each(model.fields, function (f) {
            if (f.type.toLocaleLowerCase() == fieldType) {
                arr.push(f);
            }
        });

        return arr;
    },

    //追加 范围段的 起止值
    _extendFieldQuery: function (criteria) {
        //this.getControl().query('dateRange');
        var dateRanges = this._findFieldsType(criteria, 'dateRange');
        var minValue = new Date(0, 0, 1);
        Ext.each(dateRanges, function (n) {
            var value = {
            };
            if (n.BeginValue && new Date(n.BeginValue) > minValue)
                value.BeginValue = Ext.util.Format.date(n.BeginValue, n.dateFormat);
            else
                value.BeginValue = null;
            if (n.EndValue && new Date(n.EndValue) > minValue)
                value.EndValue = Ext.util.Format.date(n.EndValue, n.dateFormat);
            else
                value.EndValue = null;

            value.dateType = n.dateType;

            criteria.data[n.name] = value;
        });

        var spinRanges = this._findFieldsType(criteria, 'spinRange');
        Ext.each(spinRanges, function (n) {
            var value = {
                "beginValue": n.beginValue,
                "endValue": n.endValue
            };
            criteria.data[n.name] = value;
        });
        var textRanges = this._findFieldsType(criteria, 'textRange');
        Ext.each(textRanges, function (n) {
            var value = {
                "firstText": n.firstText,
                "lastText": n.lastText
            };
            criteria.data[n.name] = value;
        });
    },
    //override
    _onFocusLeaveIntegrate: function () {
        var me = this;
        if (!me.ischeckbox) {
            var rawValue = me.getRawValue();

            if (!me.grid || (me.lastSelectionRecord && me.lastSelectionRecord.rawValue == rawValue)) {
                if (!rawValue && rawValue.length >= 0) {
                    me.setValue(null);
                    me._SelectItems = [];
                }
                return;
            }
            if (rawValue == "") {
                me.setValue(null);
                me._SelectItems = [];
            }
        } else {
            me.setMULTIValues();
        }
    },
    _onRowdblClick: function (vthis, record, element, rowIndex, e, eOpts) {
        var me = this;
        if (record) {
            me._SelectItems = [];
            me.setValue(record);
            me._SelectItems.push(record);
            me._win.hide();
        }
    },
    /**
     * 设置下拉列表多选值
     * 注：me.setMULTIValue();
     * 已有设置值,但未找到为什么me.setMULTIValue()不会生成[id,id,id]固暂时这样写，
     * 之后要重构掉
     */
    setMULTIValues: function () {
        var me = this;
        me.setMULTIValue();
        var entity;
        if (me.up("form")) {
            entity = me.up("form").SIEView.getData();
        } else {
            entity = me.up('container').context.record;
        }
        entity.data[me.name] = me.value;
    },
    ///**
    // * 控件的验证
    //当下拉列表所在行更改后直接返回true(不做验证)
    // */
    isValid: function (args) {
        var me = this;
        var retue = me.callParent(args);
        if (!me.up("form")) {
            if (me.up().grid.SIEView.getCurrent().getId() != me.up().context.record.getId()) {
                return true;
            }
        }
        return retue;
    }
});

Ext.define('SIE.control.ComboLink',
    {
        extend: 'SIE.control.ComboList',
        alias: 'widget.combolink',
        listeners: {
            select: function(combo, record, index) {
                var me = this;
                if (me.up("form")) {
                    entity = me.up("form").SIEView.getData();
                } else {
                    entity = me.up('container').context.record
                }
                var linkField = me.config.linkField;
                entity.set("LinkData",record[0].data);
                linkField.forEach(function(key) { 
                    entity.set(key[0], record[0].data[key[1]]);
                });
            }
        },
        _onFocusLeaveIntegrate: function() {
            var me = this;
            var rawValue = me.getRawValue();
            if (me.lastSelectionRecord) {
                var isEdited = me.lastSelectionRecord.rawValue != rawValue;
                if (isEdited) {
                    me.setValue(null);
                    if (me.up("form")) {
                        entity = me.up("form").SIEView.getData();
                    } else {
                        entity = me.up('container').context.record;
                    }
                    var linkField = me.config.linkField;
                    linkField.forEach(function(key) {
                        entity.set(key[0], '');
                    });
                }
            }
        }
    });
Ext.define('SIE.control.PagingLookUp', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.pagingLookUp',
    triggerCls: "x-form-arrow-trigger",
    model: '',
    editable: false,
    matchFieldWidth: false,//匹配当前字段宽度
    queryMode: 'remote',//是否远程拉请求数据
    reloadDataOnPopping: false,//是否每次下拉请求数据
    pageSize: 50,//下拉分页页数
    windowHeight: 290,//下拉框高度
    windowWidth: 450,//下拉框宽度
    bindDisplayField: null,//绑定的显示字段
    dataSourceProperty: '',//自定义数据源名称
    _currentRowId: -1,//当前列表行的ID
    _isQuerySelectItems: false,//查询项是否在当前页
    _SelectItems: [],//记录当前选中行
    _event: null,//当前控件
    _delayTime: 300,//延迟加载时间(ms)
    _isdeferId: null,//当前延迟Id
    _sText: "",//上一次显示字段
    _isdeferTrue: true,//是否触发延迟
    isCtrl: false,//是否按下Ctrl按钮
    searchValue: "",//请求查询条件
    icount: 0,//Ctrl+V组合键次数
    catalogReloadData: null, //快码类型专用 True查询调后台API 
    listeners: {
        ///**
        // * 键盘按下事件
        // * (Ctrl+V必然是先按Ctrl,在按V所以要先判断Ctrl)
        // * @param {type} combo
        // * @param {type} e
        // * @param {type} eOpts
        // */
        keydown: function (combo, e, eOpts) {
            var me = this;
            if (!me.readOnly && !me.disabled) {
                //console.log(e.keyCode);
                if (e.keyCode === e.CTRL) {
                    me.isCtrl = true;
                    return;
                }
                if (me.isCtrl && e.keyCode === e.V)
                    me.icount = 2;
            }
        },
        /**
         * 键盘松开事件
         * @param {type} combo
         * @param {type} e
         * @param {type} eOpts
         */
        keyup: function (combo, e, eOpts) {
            var me = this;
            if (!me.readOnly && !me.disabled) {
                if (me._sText !== me.inputEl.dom.value.trim()) {
                    if (me._isdeferId)
                        Ext.undefer(me._isdeferId);
                    me._sText = me.inputEl.dom.value.trim();
                    if (me.cbSearch) me.cbSearch.setRawValue(me._sText);
                    me._isdeferId = Ext.defer(function () {
                        me._isdeferTrue = false; me.searchValue = me.inputEl.dom.value; me.triggerisExpanded();
                    }, me.delayTime);
                }
            }
        },
    },

    /**
    * 下拉编辑器初始化
    */
    initComponent: function () {
        var me = this;
        me.callParent();
        me.enableKeyEvents = true;
        me.focusOnToFront = false;
        me.clearPropertiesOnDestroy = false;
        if (!me.bindDisplayField)
            me.bindDisplayField = me.getName() + "_Display";
        var controlClass = Ext.create(me.jsClassName);
        controlClass.initialize(me);
        me._event = controlClass;
    },

    /**
     * 下拉输入框(模糊查询)
     */
    triggerisExpanded: function () {
        var me = this;
        if (!me.isExpanded) {
            me.expand();
        }
        this._event._onSearchBoxTriggerClick();
        me._isdeferTrue = true;
    },


    /**
    * 绘制下拉列表面板
    * @returns {type} 
    */
    createPicker: function () {
        var me = this;
        var meta = me._event._getViewMeta();
        me._view = SIE.AutoUI.createListView(meta);
        me.grid = me._view.getControl();
        var gridStore = me._view.getData();
        me._event._pagingBarListeners(me._view._pagingBar);
        me.queryMode = 'remote';
        if (gridStore && gridStore.proxy && gridStore.proxy.type == 'memory') {
            me.queryMode = 'local';
        }
        me.cbSearch = Ext.create('Ext.form.field.ComboBox', {
            margin: '5',
            triggerCls: "x-form-search-trigger",
            renderTo: Ext.getBody(),
            onTriggerClick: function () {
                me._event._onSearchBoxTriggerClick();
            }
        });
        me.cbSearch.on('specialkey', function (field, e) {
            if (e.getKey() == e.ENTER) {
                me._event._onSearchBoxTriggerClick();
            }
        });
        me._win = Ext.create('Ext.window.Window', {
            title: '',
            closeAction: 'hide',
            closable: false,
            border: true,
            frame: false,
            header: false,
            layout: 'fit',
            shadow: false,
            height: me.windowHeight || 290,
            width: me.windowWidth || 450,
            tbar: me.cbSearch,
            focusOnToFront: false,
            items: [me.grid]
        });

        me.fieldKeyNav = new Ext.util.KeyNav({
            me: me,
            disabled: true,
            target: me.triggerEl
        });

        me._win.getNavigationModel = function () {
            return null;
        }
        me.on('beforedestroy', function (cmp) {
            if (me.cbSearch) me.cbSearch.destroy();
        });
        me._event._setGridListeners();
        return me._win;
    },

    /**
     * 处理单击触发（输入框）
     * @param {type} comboBox 控件
     * @param {type} trigger 
     * @param {type} e
     */
    onTriggerClick: function (comboBox, trigger, e) {
        this._event.onTriggerClick();
    },

    /**
     * 下拉展开事件
     */
    onExpand: function () {
        this.callParent();
        this._event.onExpand();
    },

    /**
     * 下拉列表失去焦点事件
     * @param {type} e
     */
    onBlur: function (e) {
        this.callParent();
        this._event.onBlur();
    },

    /**
     * 当焦点从此组件的层次结构中退出时调用
     * @param {type} e
     */
    onFocusLeave: function (e) {
        if (this.readOnly == true)
            return;

        this.callParent();
        this._event.onFocusLeave();
        if (this.up("grid") && this.isValid() === false) {///当单元格下拉编辑器验证不通过时直接退出编辑，否则dom会被回收掉
            this.up().editing = false;
        }
    },

    /**
     * 设置下拉框值
     * 注：
     *   冲掉Ext.form.field.Picker的setValue函数,不冲掉会把显示值设成ID
     *   下拉框设值函数在 SIE.control.PagingLookUpMethod的setValue里面
     * @param {type} value
     */
    setValue: function (value) {
        this._event.method.setValue(value);
    },

    /**
    * 获取下拉框值
    * @returns {type}
    */
    getValue: function () {
        return this.value;
    },

    ///**
    // * 控件的验证
    // * 当下拉列表所在行更改后直接返回true(不做验证)
    // */
    isValid: function (args) {
        var me = this;
        if (me.up("grid") && me.up()) {//表格没选中或者切换了行时认为验证通过
            var cur = me.up().grid.SIEView.getCurrent();
            if (!cur || cur.getId() !== me.up().context.record.getId())
                return true;
        }
        else if (me.rawValue && (!me.lastSelectionRecord || me.lastSelectionRecord.rawValue !== me.rawValue))//表单验证输入是否有效
            return false;
        return me.callParent(args);
    }
});
Ext.define('SIE.control.PagingLookUpBase', {
    control: null,//下拉框控件
    method: null,//下拉框方法

    initialize: function (value) {
        this.control = value;
        this.method = Ext.create(value.methodClassName);
        this.method.setControl(value);
    },

    /**
     * 处理单击触发（输入框）
     * @param {type} comboBox 控件
     * @param {type} trigger 
     * @param {type} e
     */
    onTriggerClick: function (comboBox, trigger, e) {
        var me = this;
        if (!me.control.readOnly && !me.control.disabled) {
            if (me.control.isExpanded)
                me.control.collapse();
        }
    },

    _getViewMeta: function () {
        SIE.markAbstract();
    },

    /**
     * 下拉列表放大镜查询
     */
    _onSearchBoxTriggerClick: function () {
        var me = this.control;
        var str = me.queryMode == 'remote' ? (me.dataSourceProperty ? "true" : "false") : me.queryMode;
        if (!this.method._getSIEView()) str = "false";
        var pageNum = 1;
        if (me.cbSearch) {
            if (me.cbSearch.getRawValue() == "")
                pageNum = me.grid.store ? me.grid.store.currentPage : 1;
            else
                pageNum = 1; //带关键词则重置为第1页
        }
        switch (str) {
            case "local":
                this.method.doLocalQuery();
                break;
            case "true":
                this.method._searchByDSP(pageNum);
                break;
            case "false":
                this.method._searchByRawValue(pageNum);
                break;
        }
    },

    /**
     * 下拉展开事件
     */
    onExpand: function () {
        SIE.markAbstract();
    },

    /**
     * 下拉列表失去焦点事件
     * @param {type} e
     */
    onBlur: function (e) {
        SIE.markAbstract();
    },

    /**
     * 获取焦点事件
     * @param {type} e
     */
    onFocusLeave: function (e) {
        SIE.markAbstract();
    },

    /**
     * 获取焦点处理事件
     */
    _onFocusLeaveIntegrate: function () {
        SIE.markAbstract();
    },

    /**
     * Grid行双击事件
     * @param vthis 
     * @param record
     * @param element
     * @param rowIndex 行索引
     * @param e
     * @param eOpts
     */
    _onRowdblClick: function (vthis, record, element, rowIndex, e, eOpts) {
        var me = this.control;
        if (record) {
            me._SelectItems = [];
            this.method.setValue(record);
            me._SelectItems.push(record);
            this.method._setEntityLink(record);
            me._win.hide();
        }
    },
});
Ext.define('SIE.control.PagingLookUpMethod', {
    control: null,//下拉框控件

    setControl: function (value) {
        this.control = value;
    },

    /**
     * 请求查询(entity)[_useLookUp]
     * @param {type} pageNum
     */
    _searchByRawValue: function (pageNum) {
        var me = this.control;
        this._searchText(pageNum, me.cbSearch.getRawValue());
    },

    /**
     * 请求查询(entity)[_useLookUp]
     * @param {type} pageNum
     * @param {type} text 查询条件
     */
    _searchText: function (pageNum, text) {
        var me = this.control;
        me._lastSearchValue = text;
        var filter = [];
        var property = {
            property: "_useLookUp",
            value: me.model,
            keyWord: text
        };
        filter.push(property);
        var searchFieldList = [];
        me.searchFieldList.forEach(function (item) {
            var searchField = {
                property: item
            };
            searchFieldList.push(searchField);
        });
        var data = {
            searchFieldList: searchFieldList
        };
        filter.push(data);
        if (me._view.getData().proxy) {
            me._view.getData().proxy.abort();
        }
        me._view.loadData({
            filter: SIE.data.Utils.seriaizeRequest(filter),
            page: pageNum,
        });
    },

    /**
     * 请求查询(lookup)
     * @param {type} pageNum
     */
    _searchByDSP: function (pageNum) {
        var me = this.control;
        var filter = this._searchByDSPfilter(me.dataSourceProperty);
        me.searchValue = me.cbSearch.getRawValue();
        if (me._view.getData().proxy) {
            me._view.getData().proxy.abort();
        }
        me._view.loadData({
            action: 'lookup',
            filter: SIE.data.Utils.seriaizeRequest(filter),
            searchKeyWord: (me.searchValue ? me.searchValue : ''),
            page: pageNum,
            criteria: null
        });
        me._lastSearchValue = me.searchValue;
    },

    /**
     * 生成请求参数据
     * @param {any} dsp
     */
    _searchByDSPfilter: function (dsp) {
        var me = this.control;
        var filter = {};
        if (me.catalogReloadData) {
            filter = {
                Parameters: {
                    EntityType: me.model,
                    Entity: {},
                    DataSourceProperty: dsp
                }
            };
        } else {
            filter = {
                Parameters: {
                    EntityType: this._getSIEView().model,
                    Entity: this._getContainerRecord().data,
                    DataSourceProperty: dsp
                }
            };
        }
        return filter;
    },

    /**
     *  js内存（缓存）查询
     * @param {type} queryPlan
     */
    doLocalQuery: function (queryPlan) {
        var me = this.control,
            store = me.grid.getStore(),
            queryString = queryPlan || me.cbSearch.getRawValue(),
            filter;
        store.clearFilter();
        if (queryString) {
            me.changingFilters = true;
            filter = me.queryFilter = new Ext.util.Filter({
                id: me.id + '-filter',
                anyMatch: true,
                caseSensitive: false,
                root: 'data',
                property: me.displayField,
                value: queryString
            });

            store.addFilter(filter, true);
            me.changingFilters = false;
        }
        me._lastSearchValue = queryString;
        me.grid.getView().refresh();
    },

    /**
    * 在设置的同时，把选择项的 bindDisplayField 同步到记录上
    * @param value 设置值
    */
    setValue: function (value) {
        var me = this.control;
        var sStr = me.model !== "SIE.Common.Catalogs.Catalog" ? (me.ischeckbox ? "multi" : "default") : "catalog";
        switch (sStr) {
            case "multi":
                this.multiSetValue();
                break;
            case "catalog":
                this.catalogSetValue(value);
                break;
            default:
                this.defaultSetValue(value);
                break;
        }
    },

    /**
     * (多选)设置下拉框值
     * @param {type} value
     */
    multiSetValue: function () {
        var me = this.control;
        if (me._targetSelectItems) {
            me.value = me._targetSelectItems.keys;
            var displayVal2 = "";
            var displayField = me.displayField;
            me._targetSelectItems.items.forEach(function (model) {
                displayVal2 += "," + model.data[displayField];
            });
            displayVal2 = displayVal2.substring(1);
            var entity;
            if (!me.up("form"))
                entity = me.up("container").context.record;
            else
                entity = me.up("form").SIEView.getData();
            entity.data[me.getName()] = me._targetSelectItems.keys;
            entity.data[me.bindDisplayField] = displayVal2;
            me.setRawValue(displayVal2);
            me.lastSelectionRecord = {
                value: entity.data[me.getName()],
                rawValue: displayVal2
            };
            me.checkChange();
        }
    },
    /**
     * (快码)设置下拉框值
     * @param {type} value
     */
    catalogSetValue: function (value) {
        var me = this.control;
        var val2 = null;
        var displayVal2 = '';
        if (value === "")/*避免快码下拉列表（value="")先触发PropertyChanged后赋值(每次PropertyChanged后会删除监听事件)*/
            val2 = value;
        var hasRecords = false;
        if (value && !value.isModel) {
            val2 = value;
            displayVal2 = this._getCatalogDisplayVal(value);
        }
        if (value && value.isModel) {
            val2 = value.get(me.valueField);
            displayVal2 = value.get(me.displayField);
            hasRecords = true;
        }
        me.value = val2;
        if (me.valueField == me.displayField) {
            displayVal2 = val2;
        }
        me.setRawValue(displayVal2);
        me.lastSelectionRecord = {
            value: val2,
            rawValue: displayVal2
        };
        if (me.column) {
            this._setGridcolumn(val2, displayVal2);
        } else {
            this._setformbind(val2, displayVal2);
        }
        me.checkChange();
        if (hasRecords) {
            me.fireEvent('select', me, [value]);
        }
    },
    /**
     * 默认设置下拉框值
     * @param {type} value
     */
    defaultSetValue: function (value) {
        var me = this.control;
        var val2 = null;
        var displayVal2 = '';
        var hasRecords = false;
        if (value && !value.isModel) {
            val2 = value;
            displayVal2 = this._getdisplayVal();
        }
        if (value && value.isModel) {
            val2 = value.get(me.valueField);
            displayVal2 = value.get(me.displayField);
            hasRecords = true;
        }
        me.value = val2;
        if (me.valueField == me.displayField) {
            displayVal2 = val2;
        }
        me.setRawValue(displayVal2);
        me.lastSelectionRecord = {
            value: val2,
            rawValue: displayVal2
        };
        if (me.column) {
            this._setGridcolumn(val2, displayVal2);
        } else {
            this._setformbind(val2, displayVal2);
        }
        me.checkChange();
        if (hasRecords) {
            me.fireEvent('select', me, [value]);
        }
    },

    /**
     * 设值联动字段
     */
    _setEntityLink: function (record) {
        var me = this.control;
        if (me.config.linkField && me.config.linkField.length > 0) {
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }
            var linkField = me.config.linkField;
            if (record !== null) {
                //todo：不需要把下拉选中的数据存储,因一个界面并非只有一个下拉联动，而且还会导致后台序列化多做无用功
                /* 此字段将会在下一版本去掉*/
                entity.set("LinkData", record.data);
                linkField.forEach(function (key) {
                    entity.set(key[0], record.data[key[1]]);
                });
            } else {
                linkField.forEach(function (key) {
                    entity.set(key[0], '');
                });
            }
        }
    },


    /**
     * 获取绑定属性的display(快码)
     * 注：setValue(id)时需要取viewmodel中对应display
     */
    _getCatalogDisplayVal: function (value) {
        var me = this.control;
        var displayVal2 = '';
        if (me.valueField != me.displayField) {
            if (me.column) {
                var cellEditor = this._getcontainer();
                if (cellEditor) {
                    displayVal2 = cellEditor.context.record.get(cellEditor.field.bindDisplayField);
                }
            } else {
                var bindRec = me.getBind() ? this._getBindRecord() : null;
                if (bindRec) {
                    displayVal2 = bindRec.get(me.bindDisplayField);
                    var data = Ext.JSON.decode(me.store.data, true) == null ? me.store.data : Ext.JSON.decode(me.store.data, true);
                    data.forEach(function (item) {
                        if (item[me.valueField] == value && !displayVal2) {
                            displayVal2 = item[me.displayField];
                        }
                    });
                } else {
                    var record = me.lastSelectionRecord;
                    //未绑定之前只是一个{}空对象(在从表,孙表)
                    if (me.getRawValue() && record && record.rawValue && me.getRawValue() == record.rawValue) {
                        displayVal2 = record.rawValue;
                    }
                }
            }
        }
        return displayVal2;
    },

    /**
     * 获取绑定属性的display（默认）
     * 注：setValue(id)时需要取viewmodel中对应display
     */
    _getdisplayVal: function () {
        var me = this.control;
        var displayVal2 = '';
        if (me.valueField != me.displayField) {
            if (me.column) {
                var cellEditor = this._getcontainer();
                if (cellEditor) {
                    displayVal2 = cellEditor.context.record.get(cellEditor.field.bindDisplayField);
                }
            } else {
                var bindRec = me.getBind() ? this._getBindRecord() : null;
                if (bindRec) {
                    displayVal2 = bindRec.get(me.bindDisplayField);
                } else {
                    var record = me.lastSelectionRecord;
                    //未绑定之前只是一个{}空对象(在从表,孙表)
                    if (me.getRawValue() && record && record.rawValue && me.getRawValue() == record.rawValue) {
                        displayVal2 = record.rawValue;
                    }
                }
            }
        }
        return displayVal2;
    },

    /**
     * 设置列表行数据源值
     * @param {type} val2 value值
     * @param {type} displayVal2 display值
     */
    _setGridcolumn: function (val2, displayVal2) {
        var me = this.control;
        var cellEditor = this._getcontainer();
        if (cellEditor) {
            var record = cellEditor.context.record;
            if (record.get(cellEditor.field.dataIndex) != val2) {
                record.beginEdit();
                record.set(cellEditor.field.dataIndex, val2);
                if (me.valueField != me.displayField)
                    record.set(cellEditor.field.bindDisplayField, displayVal2);
                record.endEdit();
                var sieView = this._getSIEView();
                if (sieView)
                    sieView.syncCmdState();
            }
        }
    },

    /**
     * 设置表单绑定viewModel值
     * @param {type} val2 value值
     * @param {type} displayVal2 display值
     */
    _setformbind: function (val2, displayVal2) {
        var me = this.control;
        var bindRec = me.getBind() ? this._getBindRecord() : null;
        if (bindRec && me.valueField != me.displayField) {
            bindRec.set(me.name, val2);
            bindRec.set(me.bindDisplayField, displayVal2);
        }
    },

    /**
     * 获取当前列表视图
     * @returns {type} 
     */
    _getSIEView: function () {
        var me = this.control;
        var cellEditor = me.up('container');
        while (cellEditor && !cellEditor.SIEView) {
            cellEditor = cellEditor.up('container');
        }
        if (!cellEditor || !cellEditor.SIEView)
            return null;
        return cellEditor.SIEView;
    },

    /**
     * 获取当前控件所在单元格
     * @returns {type} 
     */
    _getcontainer: function () {
        var me = this.control;
        var cellEditor = me.up('container');
        while (cellEditor && !cellEditor.context) {
            cellEditor = cellEditor.up('container');
        }
        return cellEditor;
    },

    /**
     * 
     * @returns {type} 
     */
    _getContainerRecord: function () {
        var me = this;
        if (this.control.column)
            return me._getcontainer() ? me._getcontainer().context.record : null;
        else
            return this.control.getBind() ? me._getBindRecord() : null;
    },

    /**
     * 获取bind绑定信息
     * @returns {type} 
     */
    _getBindRecord: function () {
        var me = this.control;
        var binder = me.getBind();
        var ownerData = binder.value.owner.getData();
        var dataKey = Object.keys(ownerData);
        if (dataKey.length == 1) {
            //未绑定之前只是一个{}空对象(在从表,孙表)
            if (ownerData[dataKey] !== null && Ext.isFunction(ownerData[dataKey].set)) {
                return ownerData[dataKey];
            }
        } else {
            console.log('Bind内容.length应该只能等于1');
            return null;
        }
    },

    /**
     * 验证Grid当前页是否存在当前选择行
     * @param {type} text
     * @returns {type} 
     */
    _verificaGrid: function (text) {
        var me = this.control;
        var pickerRecord = null;
        if ((text && !me.value) || (me.grid && !me.grid.selection)) {
            me.grid.getStore().each(function (rec) {
                me.searchFieldList.forEach(function (item) {//应该用所有查询字段匹配 *searchFieldList 不能为空至少存在一个字段*
                    if (rec.get(item) == text) {
                        pickerRecord = rec;
                        return false;
                    }
                });
            });
        }
        if (pickerRecord) {
            me._SelectItems = [];
            this.setValue(pickerRecord);
            me._SelectItems.push(pickerRecord);
            this._setEntityLink(pickerRecord);
            return true;
        }
        return false;
    },

    /**
     * 设置滚动条调转到最顶位置
     * 
     * */
    _viewScrollTo: function () {
        var me = this.control;
        var tableView = me.grid.getView();
        //Store没有数据无需跳转
        if (tableView.getStore().getData().length > 0) {
            var startIndex = tableView.all.startIndex;
            var endIndex = tableView.all.endIndex;
            //滚动条同步渲染数据必须设置，不设置会导致显示数据缺少
            tableView.bufferedRenderer.onRangeFetched(null, 0, endIndex - startIndex); //todo 压缩后会报错先注释
            tableView.scrollTo(0, 0);
        }
    },
});
Ext.define('SIE.control.PagingLookUpDefault', {
    extend: 'SIE.control.PagingLookUpBase',

    /**
     * 处理单击触发（输入框）
     * @param {type} comboBox 控件
     * @param {type} trigger 
     * @param {type} e
     */
    onTriggerClick: function () {
        var me = this;
        var paginglookup = me.control;
        me.callParent();
        if (!paginglookup.readOnly && !paginglookup.disabled && !paginglookup.isExpanded) {
            paginglookup.expand();
            paginglookup.cbSearch.setRawValue("");
            if (paginglookup._isdeferTrue) {
                Ext.defer(function () {
                    if (Ext.isDefined(paginglookup._lastSearchValue) == false || paginglookup._lastSearchValue != paginglookup.cbSearch.getRawValue()) {
                        me._onSearchBoxTriggerClick();
                    }
                }, 300);//下拉展开后延迟查询（300毫秒）
            } else {
                me._onBlurAsynSearch(paginglookup.inputEl.dom.value);
            }
            paginglookup._isdeferTrue = true;
        }
    },

    /**
     * Ctrl+V查询
     * @param {type} text
     */
    _onBlurAsynSearch: function (text) {
        var me = this;
        var paginglookup = me.control;
        var criteriaData = {};
        criteriaData[paginglookup.displayField] = text;
        me._AsynSearch(criteriaData, function (result) {
            if (result[0] && result[0].length > 0) {
                paginglookup._SelectItems = [];
                me.method.setValue(result[0][0]);
                paginglookup._SelectItems.push(result[0][0]);
                me.method._setEntityLink(result[0][0]);
            } else {
                me.method.setValue(null);
                me.method._setEntityLink(null);
                paginglookup._SelectItems = [];
            }
            //if (!paginglookup.up("form")) {
            //    paginglookup.up('container').context.view.refresh();
            //}
            if (paginglookup.grid) {
                paginglookup.grid.store.reload();
            }
        });
    },


    /**
     *  获取设置视图元数据（ViewMeta）
     * @returns {type} 
     */
    _getViewMeta: function () {
        var me = this.control;
        var model = me.model;
        SIE.AutoUI.getMeta({
            async: false, //同步
            model: model, viewGroup: 'SelectionView', isLookup: true, isReadonly: true, ignoreCommands: true,
            callback: function (res) {
                if (res.mainBlock)
                    meta = res.mainBlock;
                else
                    meta = res;
            }
        });
        if (me.token)
            meta.token = me.token;

        Ext.applyIf(meta.gridConfig, {
            frame: false,
            //width: 450,
            columnLines: true,
            focusOnToFront: false,
            ownerCt: me.up('[floating]')
        });

        meta.gridConfig.viewConfig = {
            enableTextSelection: false,
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value) {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        return 'gridRowLock';
                    }
                }
            }
        };

        meta.gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: true,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

        if (me.store && me.store.data) {
            meta.storeConfig = me.store;
            if (typeof me.store.data == "string")
                meta.storeConfig.data = JSON.parse(me.store.data);

            meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
            me.pageSize = 100000;
        }

        Ext.apply(meta.storeConfig, { pageSize: me.pageSize });
        return meta;
    },

    /**
     * 分页控件事件监听
     * @returns {type} 
     */
    _pagingBarListeners:function (){
        return null;
    },

    /**
     * 设置Grid列表事件监听
     */
    _setGridListeners:function(){
        var paginglookup = this.control;
        var me = this,
                   grid = paginglookup.grid,
                   store = grid.getStore();

        paginglookup.mon(grid.getView(), {
            rowdblclick: function (vthis, record, element, rowIndex, e, eOpts) {
                me._onRowdblClick(vthis, record, element, rowIndex, e, eOpts);
            }
        });

        paginglookup.mon(store, {
            load: function (evObj, records, successful, operation, eOpts) {
                me._SetCurToFirst(records, store);
            }
        })
    },

    /**
     * 设置当前选择项在第一行
     * @param {type} items
     * @param {type} store
     */
    _SetCurToFirst: function (items, store) {
        var me = this.control;
        var tableView = me.grid.getView();
        if (me.value && items && items.length > 0 && items[0].get(me.valueField) != me.value) {
            var isContains = false;
            for (var index = 0; index < items.length; index++) {
                var item = items[index];
                if (item.get(me.valueField) == me.value) {
                    items.splice(0, 0, item);
                    items.splice(index + 1, 1);
                    me._isQuerySelectItems = false;
                    isContains = true;
                    if (store) {
                        store.loadRecords(items);
                    }
                    tableView.refresh();
                    break;
                }
            }

            if (!isContains) {
                if (me._isQuerySelectItems) {
                    var criteriaData = {};
                    criteriaData[me.displayField] = me.getRawValue();
                    me._isQuerySelectItems = false;
                    //异步请求
                    this._AsynSearch(criteriaData,
                        function (result) {
                            if (result[0] && result[0].length > 0) {
                                me._SelectItems = [];
                                me._SelectItems.push(result[0][0]);
                                items.splice(0, 0, me._SelectItems[0]);
                                if (store) {
                                    store.loadRecords(items);
                                }
                                tableView.refresh();
                            }
                        });
                } else {
                    if (me._SelectItems.length > 0) {
                        items.splice(0, 0, me._SelectItems[0]);
                        if (store) {
                            store.loadRecords(items);
                        }
                        tableView.refresh();
                    }
                }
            }
        }
    },

    /**
     * 异步查询请求
     * @param {type} criteriaData 查询实体
     * @param {type} callback 回调函数
     */
    _AsynSearch: function (criteriaData, callback) {
        var me = this.control;
        var view;
        if (!view) {
            var meta = this._getViewMeta();
            view = SIE.AutoUI.createListView(meta);
        }
        var searchValue = criteriaData[me.displayField];
        me._lastSearchValue = searchValue;
        var filter = [];
        var property = {
            property: "_useLookUp",
            value: me.model,
            keyWord: searchValue
        };
        filter.push(property);
        var searchFieldList = [];
        me.searchFieldList.forEach(function (item) {
            var searchField = {
                property: item
            };
            searchFieldList.push(searchField);
        });
        var data = {
            searchFieldList: searchFieldList
        };
        filter.push(data);
        view.loadData({
            filter: SIE.data.Utils.seriaizeRequest(filter),
            callback: function (result) {
                if (callback && Ext.isFunction(callback)) {
                    callback(result);
                }
            }
        });
    },

    /**
     * 下拉展开事件
     */
    onExpand: function () {
        var me = this.control;
        var tableView = me.grid.getView();
        if (me.reloadDataOnPopping===true){
              me.cbSearch.setRawValue("");
              this._onSearchBoxTriggerClick();
         }else{
            me.cbSearch.setRawValue(me.inputEl.dom.value)
            if (me.up("grid")) {
                if (me._currentRowId != me.up().context.record.getId() && me._currentRowId != -1) {
                    me.cbSearch.setRawValue("");
                    me._isQuerySelectItems = true;
                    this._onSearchBoxTriggerClick();
                }
                me._currentRowIndex = me.up().context.record.getId();
            }
        }
        this.method._viewScrollTo();
        tableView.refresh();
     },

    /**
     * 下拉列表失去焦点事件
     * @param {type} e
     */
    onBlur: function (e) {
        var me = this.control;
        var tclass = this;
        var inputEl = me.inputEl.dom;
        if (me.up("grid") && me.revertInvalid === false) {
            me.up('container').revertInvalid = me.revertInvalid;
        }
        var text = inputEl && inputEl.value;
        this.method._verificaGrid(text);
        if (text && me.lastSelectionRecord && me.lastSelectionRecord.rawValue !== text) {
            me.markInvalid('输入['.t() + text + ']无效，未找到数据'.t());
            if (me.up("form"))
                me.value = null;
        }
    },

    /**
     * 获取控件绑定实体
     */
    _getbindEntity: function () {
        var me = this.control;
        var bindEntity;
        if (!me.bind || !me.bind.value) {
            var contet = me.up('container').context;
            var data = contet.record;
            bindEntity = data;
        } else {
            bindEntity = me.bind.value.owner.data;
            if (!(bindEntity instanceof Ext.data.Model)) {
                bindEntity = bindEntity.p;
            }
        }
        return bindEntity;
    },


    /**
     * 获取焦点事件
     * @param {type} e
     */
    onFocusLeave: function (e) {
        this._onFocusLeaveIntegrate();
    },

    /**
     * 获取焦点处理事件
     */
    _onFocusLeaveIntegrate: function () {
        var me = this.control;
        var rawValue = me.getRawValue();

        if (!me.grid || (me.lastSelectionRecord && me.lastSelectionRecord.rawValue == rawValue)) {
            if (!rawValue && rawValue.length >= 0) {
                this.method.setValue(null);
                this.method._setEntityLink(null);
                me._SelectItems = [];
            }
            return;
        }
        if (rawValue === "") {
            this.method.setValue(null);
            this.method._setEntityLink(null);
            me._SelectItems = [];
        }
    },
});
Ext.define('SIE.control.PagingLookUpMulti', {
    extend: 'SIE.control.PagingLookUpBase',

    /**
     * 处理单击触发（输入框）
     * @param {type} comboBox 控件
     * @param {type} trigger 
     * @param {type} e
     */
    onTriggerClick: function () {
        var me = this;
        var paginglookup = me.control;
        me.callParent();
        if (!paginglookup.readOnly && !paginglookup.disabled && !paginglookup.isExpanded) {
            paginglookup.expand();
            if (paginglookup.dataSourceProperty) {
                me._onSearchBoxTriggerClick();
            } else {
                Ext.defer(function () {
                    paginglookup.cbSearch.inputEl.focus(true);
                    if (Ext.isDefined(paginglookup._lastSearchValue) == false || paginglookup._lastSearchValue != paginglookup.cbSearch.getRawValue()) {
                        me._onSearchBoxTriggerClick();
                    }
                }, 300);//下拉展开后延迟查询（300毫秒）
            }
        }
    },

    /**
    *  获取设置视图元数据（ViewMeta）
    * @returns {type} 
    */
    _getViewMeta: function () {
        var me = this.control;
        var model = me.model;
        SIE.AutoUI.getMeta({
            async: false, //同步
            model: model, viewGroup: 'SelectionView', isLookup: true, isReadonly: true, ignoreCommands: true,
            callback: function (res) {
                if (res.mainBlock)
                    meta = res.mainBlock;
                else
                    meta = res;
            }
        });
        if (me.token)
            meta.token = me.token;

        Ext.applyIf(meta.gridConfig, {
            frame: false,
            //width: 450,
            columnLines: true,
            focusOnToFront: false,
            ownerCt: me.up('[floating]')
        });

        meta.gridConfig.viewConfig = {
            enableTextSelection: false,
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value) {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        return 'gridRowLock';
                    }
                }
            }
        };

        meta.gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: true,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

        meta.gridConfig.selModel = {
            injectCheckbox: 0,
            selType: 'checkboxmodel',
            checkOnly: true,
            mode: "MULTI"
        };

        me._targetSelectItems = {
            items: [],
            keys: []
        };

        if (me.store && me.store.data) {
            meta.storeConfig = me.store;
            if (typeof me.store.data == "string")
                meta.storeConfig.data = JSON.parse(me.store.data);

            meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
            me.pageSize = 100000;
        }

        Ext.apply(meta.storeConfig, { pageSize: me.pageSize });
        return meta;
    },

    /**
        * 分页控件事件监听
        * @returns {type} 
        */
    _pagingBarListeners: function (pagingBar) {
        return null;
    },

    /**
     * 设置Grid列表事件监听
     */
    _setGridListeners: function () {
        var paginglookup = this.control;
        var me = this,
            grid = paginglookup.grid;

        paginglookup.mon(grid.getSelectionModel(), {
            scope: paginglookup,
            select: me._onSelect,
            deselect: me._onDeselect,
        });

        if (paginglookup._targetSelectItems && paginglookup._targetSelectItems.keys.length > 0) {
            var selmodel = paginglookup._view.getSelectionModel();
            for (var i = 0; i < records.length; i++) {
                var record = records[i];
                if (paginglookup._targetSelectItems.keys.indexOf(record.getId()) > -1) {
                    selmodel.select(record, true, true);
                }
            }
        }
    },

    /** 
    * 复选框勾选事件
    * @param selModel 选择模式
    * @param record 选择的记录
    * @param index 行索引号
    * @param eOpts  The options object passed to Ext.util.Observable.addListener.
    */
    _onSelect: function (selModel, record, index, eOpts) {
        var me = this;
        var idx = Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0);
        if (idx === -1) {
            me._targetSelectItems.keys.push(record.data[this.valueField]);
            me._targetSelectItems.items.push(record);
        }
    },
    /**
	 * 复选框取消勾选事件
	 * @param selModel 选择模式
	 * @param record 选择的记录
	 * @param index 行索引号
	 * @param eOpts The options object passed to Ext.util.Observable.addListener.
	 */
    _onDeselect: function (selModel, record, index, eOpts) {
        var me = this;
        if (record) {
            var idx = Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0);
            if (idx > -1) {
                Ext.Array.removeAt(me._targetSelectItems.keys, idx);
                Ext.Array.removeAt(me._targetSelectItems.items, idx);
            }
        }
    },

    /**
     * 下拉展开事件
     */
    onExpand: function () {
        var me = this.control;
        var tableView = me.grid.getView();
        tableView.refresh();
    },

    /**
     * 下拉列表失去焦点事件
     * @param {type} e
     */
    onBlur: function (e) {

    },

    /**
     * 获取焦点事件
     * @param {type} e
     */
    onFocusLeave: function (e) {
        this.method.setValue();
    },

});
Ext.define('SIE.control.PagingLookUpTree', {
    extend: 'SIE.control.PagingLookUpBase',

    /**
     * 处理单击触发（输入框）
     * @param {type} comboBox 控件
     * @param {type} trigger 
     * @param {type} e
     */
    onTriggerClick: function () {
        var me = this;
        var paginglookup = me.control;
        me.callParent();
        if (!paginglookup.readOnly && !paginglookup.disabled && !paginglookup.isExpanded) {
            paginglookup.expand();
            paginglookup.cbSearch.setRawValue("");
            if (paginglookup._isdeferTrue) {
                if (paginglookup.dataSourceProperty) me._onSearchBoxTriggerClick();
                else {
                    Ext.defer(function () {
                        if (Ext.isDefined(paginglookup._lastSearchValue) == false || paginglookup._lastSearchValue != paginglookup.cbSearch.getRawValue()) {
                            me._onSearchBoxTriggerClick();
                        }
                    }, 300);
                }
            }
            else {
                me._onBlurAsynSearch(paginglookup.inputEl.dom.value);
            }
            paginglookup._isdeferTrue = true;
        }
    },

    /**
     * Ctrl+V查询
     * @param {type} text
     */
    _onBlurAsynSearch: function (text) {
        var me = this;
        var paginglookup = me.control;
        var criteriaData = {};
        criteriaData[paginglookup.displayField] = text;
        me._AsynSearch(criteriaData, function (result) {
            if (result[0] && result[0].length > 0) {
                paginglookup._SelectItems = [];
                me.method.setValue(result[0][0]);
                paginglookup._SelectItems.push(result[0][0]);
                me.method._setEntityLink(result[0][0]);
            } else {
                me.method.setValue(null);
                me.method._setEntityLink(null);
                paginglookup._SelectItems = [];
            }
            //if (!paginglookup.up("form")) {
            //    paginglookup.up('container').context.view.refresh();
            //}
            if (paginglookup.grid) {
                paginglookup.grid.store.reload();
            }
        });
    },

   /**
    *  获取设置视图元数据（ViewMeta）
    * @returns {type} 
    */
    _getViewMeta: function () {
        var me = this.control;
        var model = me.model;
        SIE.AutoUI.getMeta({
            async: false, //同步
            model: model, viewGroup: 'SelectionView', isLookup: true, isReadonly: true, ignoreCommands: true,
            callback: function (res) {
                if (res.mainBlock)
                    meta = res.mainBlock;
                else
                    meta = res;
            }
        });
        if (me.token)
            meta.token = me.token;

        me._isTree = SIE.getModel(model).isTree;

        Ext.applyIf(meta.gridConfig, {
            frame: false,
            //width: 450,
            columnLines: true,
            focusOnToFront: false,
            ownerCt: me.up('[floating]')
        });

        meta.gridConfig.viewConfig = {
            enableTextSelection: false,
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value) {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        return 'gridRowLock';
                    }
                }
            }
        };

        if (me._isTree)
            meta.gridConfig.useArrows = true;

        meta.gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: true,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

        if (me.store && me.store.data) {
            meta.storeConfig = me.store;
            if (typeof me.store.data == "string")
                meta.storeConfig.data = JSON.parse(me.store.data);

            meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
            me.pageSize = 100000;
        }

        Ext.apply(meta.storeConfig, { pageSize: me.pageSize });
        return meta;
    },

    /**
     * 分页控件事件监听
     * @returns {type} 
     */
    _pagingBarListeners: function (pagingBar) {
        return null;
    },

    /**
     * 设置Grid列表事件监听
     */
    _setGridListeners: function () {
        var paginglookup = this.control;
        var me = this,
                   grid = paginglookup.grid;

        paginglookup.mon(grid.getView(), {
            rowdblclick: function (vthis, record, element, rowIndex, e, eOpts) {
                me._onRowdblClick(vthis, record, element, rowIndex, e, eOpts);
            }
        });
    },

    /**
    * 异步查询请求
    * @param {type} criteriaData 查询实体
    * @param {type} callback 回调函数
    */
    _AsynSearch: function (criteriaData, callback) {
        var me = this.control;
        var view;
        if (!view) {
            var meta = this._getViewMeta();
            view = SIE.AutoUI.createListView(meta);
        }
        var searchValue = criteriaData[me.displayField];
        me._lastSearchValue = searchValue;
        var filter = [];
        var property = {
            property: "_useLookUp",
            value: me.model,
            keyWord: searchValue
        };
        filter.push(property);
        var searchFieldList = [];
        me.searchFieldList.forEach(function (item) {
            var searchField = {
                property: item
            };
            searchFieldList.push(searchField);
        });
        var data = {
            searchFieldList: searchFieldList
        };
        filter.push(data);
        view.loadData({
            filter: SIE.data.Utils.seriaizeRequest(filter),
            callback: function (result) {
                if (callback && Ext.isFunction(callback)) {
                    callback(result);
                }
            }
        });
    },

    /**
     * 下拉展开事件
     */
    onExpand: function () {
        var me = this.control;
        var tableView = me.grid.getView();
        if (me.reloadDataOnPopping===true){
              me.cbSearch.setRawValue("");
              this._onSearchBoxTriggerClick();
         }else{
            me.cbSearch.setRawValue(me.inputEl.dom.value)
            if (me.up("grid")) {
                if (me._currentRowIndex != me.up().context.record.getId() && me._currentRowIndex != -1) {
                    me.cbSearch.setRawValue("");
                    me._isQuerySelectItems = true;
                    this._onSearchBoxTriggerClick();
                }
                me._currentRowIndex = me.up().context.record.getId();
            }
        }
        this.method._viewScrollTo();
        tableView.refresh();
    },

    /**
     * 下拉列表失去焦点事件
     * @param {type} e
     */
    onBlur: function (e) {
        var me = this.control;
        var tclass = this;
        var inputEl = me.inputEl.dom;
        if (me.up("grid") && me.revertInvalid === false) {
            me.up('container').revertInvalid = me.revertInvalid;
        }
        var text = inputEl && inputEl.value;
        this.method._verificaGrid(text);
        if (text && me.lastSelectionRecord && me.lastSelectionRecord.rawValue !== text) {
            me.markInvalid('输入['.t() + text + ']无效，未找到数据'.t());
            if (me.up("form"))
                me.value = null;
        }
    },

    /**
     * 获取控件绑定实体
     */
    _getbindEntity: function () {
        var me = this.control;
        var bindEntity;
        if (!me.bind || !me.bind.value) {
            var contet = me.up('container').context;
            var data = contet.record;
            bindEntity = data;
        } else {
            bindEntity = me.bind.value.owner.data;
            if (!(bindEntity instanceof Ext.data.Model)) {
                bindEntity = bindEntity.p;
            }
        }
        return bindEntity;
    },

    /**
     * 获取焦点事件
     * @param {type} e
     */
    onFocusLeave: function (e) {
        this._onFocusLeaveIntegrate();
    },

    /**
     * 获取焦点处理事件
     */
    _onFocusLeaveIntegrate: function () {
        var me = this.control;
        var rawValue = me.getRawValue();

        if (!me.grid || (me.lastSelectionRecord && me.lastSelectionRecord.rawValue == rawValue)) {
            if (!rawValue && rawValue.length >= 0) {
                this.method.setValue(null);
                this.method._setEntityLink(null);
                me._SelectItems = [];
            }
            return;
        }
        if (rawValue == "") {
            this.method.setValue(null);
            this.method._setEntityLink(null);
            me._SelectItems = [];
        }
    },
});
Ext.define('SIE.control.PagingLookUpCustom', {
    extend: 'SIE.control.PagingLookUpBase',

    /**
     * 处理单击触发（输入框）
     * @param {type} comboBox 控件
     * @param {type} trigger 
     * @param {type} e
     */
    onTriggerClick: function () {
        var me = this;
        var paginglookup = me.control;
        me.callParent();
        if (!paginglookup.readOnly && !paginglookup.disabled && !paginglookup.isExpanded) {
            paginglookup.expand();
            paginglookup.cbSearch.setRawValue("");
            if (paginglookup._isdeferTrue) {
                me._onSearchBoxTriggerClick();
            } else {
                me._onBlurAsynSearch(paginglookup.inputEl.dom.value);
            }
            paginglookup._isdeferTrue = true;
        }
    },

    /**
    * Ctrl+V查询
    * @param {type} text
    */
    _onBlurAsynSearch: function (text) {
        var me = this;
        var paginglookup = me.control;
        var criteriaData = {};
        criteriaData[paginglookup.displayField] = text;
        me._AsynSearch(criteriaData, function (result) {
            if (result[0] && result[0].length > 0) {
                paginglookup._SelectItems = [];
                me.method.setValue(result[0][0]);
                paginglookup._SelectItems.push(result[0][0]);
                me.method._setEntityLink(result[0][0]);
            } else {
                me.method.setValue(null);
                me.method._setEntityLink(null);
                paginglookup._SelectItems = [];
            }
            //if (!paginglookup.up("form")) {
            //    paginglookup.up('container').context.view.refresh();
            //}
            if (paginglookup.grid) {
                paginglookup.grid.store.reload();
            }
        });
    },

    /**
    *  获取设置视图元数据（ViewMeta）
    * @returns {type} 
    */
    _getViewMeta: function () {
        var me = this.control;
        var model = me.model;
        SIE.AutoUI.getMeta({
            async: false, //同步
            model: model, viewGroup: 'SelectionView', isLookup: true, isReadonly: true, ignoreCommands: true,
            callback: function (res) {
                if (res.mainBlock)
                    meta = res.mainBlock;
                else
                    meta = res;
            }
        });
        if (me.token)
            meta.token = me.token;
        me._isTree = SIE.getModel(model).isTree;
        if (me._isTree)
            meta.gridConfig.useArrows = true;

        Ext.applyIf(meta.gridConfig, {
            frame: false,
            //width: 450,
            columnLines: true,
            focusOnToFront: false,
            ownerCt: me.up('[floating]')
        });

        meta.gridConfig.viewConfig = {
            enableTextSelection: false,
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value) {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        return 'gridRowLock';
                    }
                }
            }
        };

        meta.gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: true,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

        if (me.store && me.store.data) {
            meta.storeConfig = me.store;
            if (typeof me.store.data == "string")
                meta.storeConfig.data = JSON.parse(me.store.data);

            meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
            me.pageSize = 100000;
        }

        Ext.apply(meta.storeConfig, { pageSize: me.pageSize });
        return meta;
    },

    /**
     * 分页控件事件监听
     * @returns {type} 
     */
    _pagingBarListeners: function (pagingBar) {
        return null;
    },

    /**
     * 设置Grid列表事件监听
     */
    _setGridListeners: function () {
        var paginglookup = this.control;
        var me = this,
                   grid = paginglookup.grid,
                   store = grid.getStore();

        paginglookup.mon(grid.getView(), {
            rowdblclick: function (vthis, record, element, rowIndex, e, eOpts) {
                me._onRowdblClick(vthis, record, element, rowIndex, e, eOpts);
            }
        });

        paginglookup.mon(store, {
            load: function (evObj, records, successful, operation, eOpts) {
                me._SetCurToFirst(records, store);
            }
        })
    },

    /**
     * 设置当前选择项在第一行
     * @param {type} items
     * @param {type} store
     */
    _SetCurToFirst: function (items, store) {
        var me = this.control;
        var tableView = me.grid.getView();
        if (me.value && items && items.length > 0 && items[0].get(me.valueField) != me.value) {
            var isContains = false;
            for (var index = 0; index < items.length; index++) {
                var item = items[index];
                if (item.get(me.valueField) == me.value) {
                    items.splice(0, 0, item);
                    items.splice(index + 1, 1);
                    me._isQuerySelectItems = false;
                    isContains = true;
                    if (store) {
                        store.loadRecords(items);
                    }
                    tableView.refresh();
                    break;
                }
            }

            if (!isContains) {
                if (me._isQuerySelectItems) {
                    var criteriaData = {};
                    criteriaData[me.displayField] = me.getRawValue();
                    me._isQuerySelectItems = false;
                    //异步请求
                    this._AsynSearch(criteriaData,
                        function (result) {
                            if (result[0] && result[0].length > 0) {
                                me._SelectItems = [];
                                me._SelectItems.push(result[0][0]);
                                items.splice(0, 0, me._SelectItems[0]);
                                if (store) {
                                    store.loadRecords(items);
                                }
                                tableView.refresh();
                            }
                        });
                } else {
                    if (me._SelectItems.length > 0) {
                        items.splice(0, 0, me._SelectItems[0]);
                        if (store) {
                            store.loadRecords(items);
                        }
                        tableView.refresh();
                    }
                }
            }
        }
    },

    /**
     * 异步查询请求
     * @param {type} criteriaData 查询实体
     * @param {type} callback 回调函数
     */
    _AsynSearch: function (criteriaData, callback) {
        var me = this.control;
        var view;
        if (!view) {
            var meta = this._getViewMeta();
            view = SIE.AutoUI.createListView(meta);
        }
        var rec = this.method._getContainerRecord();
        var filter = {
            Parameters: {
                EntityType: !me.up("form") ? me.up().context.grid.SIEView.model : me.up("form").SIEView.model,
                Entity: rec.data,
                DataSourceProperty: me.dataSourceProperty
            }
        };
        var searchValue = criteriaData[me.displayField];
        view.loadData({
            action: 'lookup',
            filter: SIE.data.Utils.seriaizeRequest(filter),
            searchKeyWord: (searchValue ? searchValue : ''),
            page: 1,
            criteria: criteriaData,
            callback: function (result) {
                if (callback && Ext.isFunction(callback)) {
                    callback(result);
                }
            }
        });
    },

    /**
     * 下拉展开事件
     */
    onExpand: function () {
        var me = this.control;
        var tableView = me.grid.getView();
        if (me.reloadDataOnPopping===true){
              me.cbSearch.setRawValue("");
              this._onSearchBoxTriggerClick();
         }else{
            me.cbSearch.setRawValue(me.inputEl.dom.value)
            if (!me.up("form")) {
                if (me._currentRowId != me.up().context.record.getId() && me._currentRowId != -1) {
                    me.cbSearch.setRawValue("");
                    me._isQuerySelectItems = true;
                    me._onSearchBoxTriggerClick();
                }
                me._currentRowIndex = me.up().context.record.getId();
            }
        }
        this.method._viewScrollTo();
        tableView.refresh();
    },

    /**
    * 下拉列表失去焦点事件
    * @param {type} e
    */
    onBlur: function (e) {
        var me = this.control;
        var tclass = this;
        var inputEl = me.inputEl.dom;
        if (me.up("grid") && me.revertInvalid === false) {
            me.up('container').revertInvalid = me.revertInvalid;
        }
        var text = inputEl && inputEl.value;
        this.method._verificaGrid(text);
        if (text && me.lastSelectionRecord && me.lastSelectionRecord.rawValue !== text) {
            me.markInvalid('输入['.t() + text + ']无效，未找到数据'.t());
            if (me.up("form"))
                me.value = null;
        }
    },

    /**
     * 获取控件绑定实体
     */
    _getbindEntity: function () {
        var me = this.control;
        var bindEntity;
        if (!me.bind || !me.bind.value) {
            var contet = me.up('container').context;
            var data = contet.record;
            bindEntity = data;
        } else {
            bindEntity = me.bind.value.owner.data;
            if (!(bindEntity instanceof Ext.data.Model)) {
                bindEntity = bindEntity.p;
            }
        }
        return bindEntity;
    },

    /**
     * 获取焦点事件
     * @param {type} e
     */
    onFocusLeave: function (e) {
        this._onFocusLeaveIntegrate();
    },

    /**
     * 获取焦点处理事件
     */
    _onFocusLeaveIntegrate: function () {
        var me = this.control;
        var rawValue = me.getRawValue();

        if (!me.grid || (me.lastSelectionRecord && me.lastSelectionRecord.rawValue == rawValue)) {
            if (!rawValue && rawValue.length >= 0) {
                this.method.setValue(null);
                this.method._setEntityLink(null);
                me._SelectItems = [];
            }
            return;
        }
        if (rawValue == "") {
            this.method.setValue(null);
            this.method._setEntityLink(null);
            me._SelectItems = [];
        }
    },
});
Ext.define('SIE.control.Number', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.numberfield',
    triggerCls: "x-form-arrow-trigger",
    isReturnNum: false,
    /**
    * 下拉编辑器初始化
    */
    initComponent: function () {
        var me = this;
        me.callParent();
    },

    /**
     * 失去焦点事件
     * 当form表单数值输入错误时，提示并回退上一正确值
     * @param {type} e
     */
    onBlur: function (e) {
        this.callParent();
        var me = this;
        if (me.isReturnNum) {
            if (me.up("form") && me.getErrors().length) {
                var entity = me.up("form").SIEView.getData();
                me.setValue(entity.get(me.name));
            }
        }
    },

});
Ext.define('SIE.control.Checkbox', {
    extend: 'Ext.form.field.Checkbox',
    alias: 'widget.checkboxfield',
    triggerCls: "x-form-arrow-trigger",
    isReturnNum: false,
    /**
    * 下拉编辑器初始化
    */
    initComponent: function () {
        var me = this;
        //当列表的Checkbox设置为只读时，禁用Checkbox，单只读时未知情况会隐藏控件
        if (!me.up("form") && me.readOnly) {
            me.disabled = true;
        }
        me.callParent();
    },
});
//数值范围控件
Ext.define('SIE.control.SpinRange', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.spinRange',
    layout: 'hbox',
    border: false,
    fieldLabel: '',
    minValue: '',
    maxValue: '',
    allowDecimals: true,
    align: 'right',
    allowBlank: true,
    allowNegative: true,
    step: 1,
    decimalPrecision: 0,
    beginValue: null,
    endValue: null,
    fieldDefaults: {
        width: '100%'
    },
    config: {
    },
    beforeRender: function () {
        this.callParent();
        var me = this;
        var form = this.up('form');
        if (form) {
            var entity = form.SIEView.getData();
            entity.data[this.name] = {
                beginValue: this.beginValue,
                endValue: this.endValue,
            };
        }

    },
    items: [
        {
            xtype: 'numberfield',
            value: this.beginValue,
            //anchor: '100%',
            //fieldLabel: '从',
            labelWidth: 10,
            //labelAlign: 'right',
            listeners: {
                "change": function (field, newValue, oldValue) {
                    this.beginValue = newValue;
                    var form = this.up('form');
                    if (form) {
                        var entity = form.SIEView.getData();
                        var p = this.findParentByType('spinRange');
                        if (entity.data[p.name] != null) {
                            entity.data[p.name].beginValue = this.beginValue;
                        }
                    }

                }
            }
        },
        {
            xtype: 'numberfield',
            value: this.endValue,
            fieldLabel: '-',
            labelWidth: 10,
            fieldLabel: '到'.t(),
            //anchor: '100%',
            listeners: {
                "change": function (field, newValue, oldValue) {
                    this.endValue = newValue;
                    var form = this.up('form');
                    if (form) {
                        var entity = form.SIEView.getData();
                        var p = this.findParentByType('spinRange');
                        if (entity.data[p.name] != null) {
                            entity.data[p.name].endValue = this.endValue;
                        }
                    }

                }
            }
        }],
    initComponent: function () {
        this.initConfig({ beginValue: this.beginValue, endValue: this.endValue });
        //this.items[0].fieldLabel = this.fieldLabel;
        this.items[0].value = this.beginValue;
        this.items[0].minValue = this.minValue;
        this.items[0].allowDecimals = this.allowDecimals;
        this.items[0].allowBlank = this.allowBlank;
        this.items[0].step = this.step;
        this.items[0].decimalPrecision = this.decimalPrecision;
        if (this.allowNegative) this.items[0].minValue = 0;

        this.items[1].value = this.endValue;
        this.items[1].maxValue = this.maxValue;
        this.items[1].allowDecimals = this.allowDecimals;
        this.items[1].step = this.step;
        this.items[1].allowBlank = this.allowBlank;
        this.items[1].decimalPrecision = this.decimalPrecision;
        if (this.allowNegative) this.items[1].minValue = 0;

        this.callParent();
    },

    onResize: function (w, h, ow, oh) {
        var me = this;
        var width = me.getWidth() - me.labelWidth;
        var p = me.findParentByType('panel');
        var n1 = me.items.items[0];
        var n2 = me.items.items[1];
        var itemWidth = width - 15;//15为item的labelWidth+上\"\"的5像素
        n1.setWidth(itemWidth / 2);
        n2.setWidth(itemWidth / 2 + 15 - 5);
        this.callParent(arguments);
    },

    clearValue: function () {
        var inputs = this.el.query('input');
        inputs[0].value = this.beginValue = null;
        inputs[1].value = this.endValue = null;
        var form = this.up('form');
        if (form) {
            var entity = form.SIEView.getData();
            entity.data[this.name] = {
                beginValue: null,
                endValue: null,
            };
        }
    },

    validate: function () {
        if (!this.items.items[0].validate() || !this.items.items[1].validate())
            return false;
        else
            if (this.beginValue && this.endValue && this.beginValue > this.endValue) {
                SIE.Msg.showWarning('结束值必须大于或等于开始值'.t());
                return false;
            }
        return true;
    },
    getValue: function () {
        return Ext.JSON.encode({
            BeginValue: this.beginValue,
            EndValue: this.endValue
        });
    }
});




Ext.define('SIE.control.GridPager', {
    extend: 'Ext.toolbar.Paging',
    xtype: 'gridpager',
    _pageSize: 25,
    /**
     * Gets the standard paging items in the toolbar
     * @private
     */
    ///分页内容是否 显示 简要
    _displayInfoOnSimple: false,
    getPagingItems: function () {
        var me = this;
        items = [];
        if (!me._displayInfoOnSimple) {
            items = me._getAll();
        }
        else {
            items = me._getSimple();
        }
        return items;
    },
    _getAll: function () {
        var me = this,
            inputListeners = {
                scope: me,
                blur: me.onPagingBlur
            };

        inputListeners[Ext.supports.SpecialKeyDownRepeat ? 'keydown' : 'keypress'] = me.onPagingKeyDown;
        return [{
            xtype: 'tbtext',
            itemId: 'displayItem'
        },
            '->',
        {
            itemId: 'first',
            tooltip: me.firstText,
            overflowText: me.firstText,
            iconCls: Ext.baseCSSPrefix + 'tbar-page-first',
            disabled: true,
            handler: me.moveFirst,
            margin:'0 1 0 1',
            scope: me
        }, {
            itemId: 'prev',
            tooltip: me.prevText,
            overflowText: me.prevText,
            iconCls: Ext.baseCSSPrefix + 'tbar-page-prev',
            disabled: true,
            handler: me.movePrevious,
            margin: '0 5 0 1',
            scope: me
        },
            '-',
        me.beforePageText,
        {
            xtype: 'numberfield',
            itemId: 'inputItem',
            name: 'inputItem',
            cls: Ext.baseCSSPrefix + 'tbar-page-number',
            allowDecimals: false,
            minValue: 1,
            hideTrigger: true,
            enableKeyEvents: true,
            keyNavEnabled: false,
            selectOnFocus: true,
            submitValue: false,
            // mark it as not a field so the form will not catch it when getting fields 
            isFormField: false,
            margin: '-1 0 3 -10',
            listeners: inputListeners
        }, {
            xtype: 'tbtext',
            itemId: 'afterTextItem',
            margin: '0',
            html: Ext.String.format(me.afterPageText, 1)
        },
            '-',
        {
            itemId: 'next',
            tooltip: me.nextText,
            overflowText: me.nextText,
            iconCls: Ext.baseCSSPrefix + 'tbar-page-next',
            disabled: true,
            handler: me.moveNext,
            margin: '0 1 0 -3',
            scope: me
        }, {
            itemId: 'last',
            tooltip: me.lastText,
            overflowText: me.lastText,
            iconCls: Ext.baseCSSPrefix + 'tbar-page-last',
            disabled: true,
            handler: me.moveLast,
            margin: '0 5 0 1',
            scope: me
        },
            '-',
        {
            xtype: 'combobox',
            itemId: 'pageSizeItem',
            store: Ext.create('Ext.data.Store', {
                fields: ['value'],
                data: [
                    { "value": 5 },
                    { "value": 25 },
                    { "value": 50 },
                    { "value": 100 },
                    { "value": 1000 },
                    { "value": 5000 },
                ]
            }),
            listeners: {
                //change: { fn: me._pageSizeChange, scope: me },
                select: { fn: me._pageSizeSelect, scope: me },
                keypress: { fn: me._pageKeypress, scope: me }
            },
            value: me._pageSize,
            autoSelect: false,
            enableKeyEvents:true,
            width: 66,
            minValue: 0,
            maxValue: 5000,
            queryMode: 'local',
            displayField: 'value',
            valueField: 'value',
            margin: '0 1 0 -3'
        },
        {
            itemId: 'refresh',
            tooltip: me.refreshText,
            overflowText: me.refreshText,
            iconCls: Ext.baseCSSPrefix + 'tbar-loading',
            disabled: me.store.isLoading(),
            handler: me.doRefresh,
            margin: '0 8 0 1',
            scope: me
        }];
    },

    _getSimple: function () {
        var me = this,
            inputListeners = {
                scope: me,
                blur: me.onPagingBlur
            };

        inputListeners[Ext.supports.SpecialKeyDownRepeat ? 'keydown' : 'keypress'] = me.onPagingKeyDown;
        return [
            {
                itemId: 'prev',
                tooltip: me.prevText,
                overflowText: me.prevText,
                iconCls: Ext.baseCSSPrefix + 'tbar-page-prev',
                disabled: true,
                handler: me.movePrevious,
                scope: me
            },
            me.beforePageText,
            {
                xtype: 'numberfield',
                itemId: 'inputItem',
                name: 'inputItem',
                cls: Ext.baseCSSPrefix + 'tbar-page-number',
                allowDecimals: false,
                minValue: 1,
                hideTrigger: true,
                enableKeyEvents: true,
                keyNavEnabled: false,
                selectOnFocus: true,
                submitValue: false,
                // mark it as not a field so the form will not catch it when getting fields 
                isFormField: false,
                margin: '-1 2 3 2',
                listeners: inputListeners
            }, {
                xtype: 'tbtext',
                itemId: 'afterTextItem',
                html: Ext.String.format(me.afterPageText, 1)
            }, {
                xtype: 'tbtext',
                itemId: 'displayItem',

            },
            {
                itemId: 'next',
                tooltip: me.nextText,
                overflowText: me.nextText,
                iconCls: Ext.baseCSSPrefix + 'tbar-page-next',
                disabled: true,
                handler: me.moveNext,
                scope: me
            },
            {
                xtype: 'combobox',
                itemId: 'pageSizeItem',
                store: Ext.create('Ext.data.Store', {
                    fields: ['value'],
                    data: [
                        { "value": 5 },
                        { "value": 25 },
                        { "value": 50 },
                        { "value": 100 },
                        { "value": 1000 },
                        { "value": 5000 },
                    ]
                }),
                listeners: {
                    //change: { fn: me._pageSizeChange, scope: me }
                    select: { fn: me._pageSizeSelect, scope: me },
                    keypress: { fn: me._pageKeypress, scope: me }
                },
                value: me._pageSize,
                autoSelect: false,
                enableKeyEvents: true,
                minValue: 0,
                maxValue: 5000,
                width: 72,
                queryMode: 'local',
                displayField: 'value',
                valueField: 'value',
            },
            {
                itemId: 'refresh',
                tooltip: me.refreshText,
                overflowText: me.refreshText,
                iconCls: Ext.baseCSSPrefix + 'tbar-loading',
                disabled: me.store.isLoading(),
                handler: me.doRefresh,
                scope: me
            }];
    },

    doRefresh: function () {
        var me = this,
            store = me.store,
            current = store.currentPage;

        if (store._loaded && me.fireEvent('beforechange', me, current) !== false) {
            store.loadPage(current,
                {
                    callback: function (records, operation, success) {
                        store._loaded = success;
                    }
                });
            return true;
        }
        return false;
    },
    _pageSizeSelect: function (clt, recore, eopts) {
        var me = this,
            store = me.store;

        var value = recore.getData().value;
        var nvalue = Math.floor(value);
        me._pageSize = nvalue;
        me.store.setPageSize(nvalue);
        if (store._loaded)
            me.moveFirst();
    },
    _pageKeypress: function (clt, e, eOpts) {
        var me = this;
        store = me.store;
        if (e.keyCode == 13) {
            var value = clt.getValue();
            if (!value || !Ext.isNumeric(value) || value <= 0) {
                return;
            }
            var nvalue = Math.floor(value);

            me._pageSize = nvalue;
            me.store.setPageSize(nvalue);
            if (store._loaded)
                me.moveFirst();
        }
    }, 
    _pageSizeChange: function (clt, newValue, oldValue, eOpts) {
        var me = this,
            store = me.store;

        if (!newValue || !Ext.isNumeric(newValue) || newValue <= 0) {
            return;
        }

        var nvalue = Math.floor(newValue);

        me._pageSize = nvalue;
        me.store.setPageSize(nvalue);
        if (store._loaded)
            me.moveFirst();
    },

    bindStoreListeners: function (store) {
        store.setPageSize(this._pageSize); //设置同步页面分页选项值
        this.callParent(arguments);
    },

    initComponent: function () {
        this.displayInfo = false;
        this.callParent();
    },
});

//日期范围控件
var daterangeStore = Ext.create("Ext.data.Store", {
    fields: ["Name", "Value"],
    data: []
});

Ext.define('SIE.control.DateRange',
    {
        extend: 'Ext.form.FieldContainer',
        alias: 'widget.dateRange',
        layout: 'anchor',
        border: false,
        dateType: 2,//默认为当天
        dateFormat: 'Y-m-d H:i:s',
        allowBlank: true,
        BeginValue: '',
        defaults: {
            layout: '100%'
        },
        cls: 'ux-dateRange',
        EndValue: '',
        fieldLabel: '',
        config: {
            isSelected: true
        },

        fieldDefaults: {
            width: '100%'
        },
        beforeRender: function () {
            this.callParent();
            var me = this;
            var form = this.up('form');
            if (form) {
                var entity = form.SIEView.getData();
                entity.data[this.name] = {
                    dateType: this.dateType,
                    BeginValue: Ext.util.Format.date(Ext.Date.add(this.BeginValue, Ext.Date.Day, 0), "Y-m-d H:i:s"),
                    EndValue: Ext.util.Format.date(Ext.Date.add(this.EndValue, Ext.Date.Day, 0), "Y-m-d H:i:s")
                };
            }
        },
        afterRender: function () {
            this.callParent();
            var me = this;
            var ds = this.query('datefield');
            var dr = this.configRangeDate(this, this.dateType, ds, true);
        },
        items: [{
            xtype: 'combobox',
            store: daterangeStore,
            displayField: 'Name',
            isFormField: false,
            valueField: 'Value',
            fieldLabel: '',
            editable: false,
            emptyText: '--请选择--'.t(),
            queryMode: 'local',
            listeners: {
                select: function (combo, record, opts) {
                    var p = this.findParentByType('dateRange');
                    var form = this.up('form');
                    if (form) {
                        var entity = form.SIEView.getData();
                        if (!entity.data[p.name]) {
                            entity.data[p.name].dateType = this.value;
                        }
                    }

                    var selectValue = combo.getValue();
                    if (!selectValue) return;
                    if (selectValue !== 0) {
                        p.isSelected = true;
                    }
                    var ds = p.query('datefield');
                    if (!ds) return;
                    p.configRangeDate(p, selectValue, ds, true);
                }
            }
        }, {
            xtype: 'datefield',
            format: 'Y-m-d H:i:s',
            formatText: '',
            invalidText: '{0}不是有效日期'.t(),
            itemRole: 'fromField',
            isFormField: true,
            //value: Ext.util.Format.date(Ext.Date.add(new Date(), Ext.Date.Day, 0), "Y-m-d"),
            listeners: {
                "change": function (field, value) {
                    var p = field.findParentByType('dateRange');
                    if ((this.format === "Y/m/d" || this.format === "Y-m-d") && value !== null) {
                        field.setValue(value);
                    }
                    p.changeValue(field, value, 'BeginValue');
                    //p.setSecondMinDate(value);
                },
                "blur": function (field) {
                    var value = field.getValue();
                    var p = field.findParentByType('dateRange');
                    value = p.formatDateValue(value);
                    if (value) field.setValue(value);
                }
            }
        }, {
            xtype: 'datefield',
            format: 'Y-m-d H:i:s',
            formatText: '',
            invalidText: '{0}不是有效日期'.t(),
            itemRole: 'toField',
            isFormField: true,
            //value: Ext.util.Format.date(Ext.Date.add(new Date(), Ext.Date.Day, 0), "Y-m-d"),
            listeners: {
                "collapse": function (field) {
                    var p = field.findParentByType('dateRange');
                    var value = field.value;
                    if (value !== null) {
                        value = p.formatDateValue(value);
                        if (value !== null) {
                            value.setHours(23);
                            value.setMinutes(59);
                            value.setSeconds(59);
                            field.setValue(value);
                        }
                    }
                    p.changeValue(field, value, 'EndValue');
                },
                "change": function (field, value) {
                    var p = field.findParentByType('dateRange');
                    if ((this.format === "Y/m/d" || this.format === "Y-m-d") && value !== null) {
                        value = p.formatDateValue(value);
                        if (value !== null) {
                            value.setHours(23);
                            value.setMinutes(59);
                            value.setSeconds(59);
                            field.setValue(value);
                        }
                    }
                    p.changeValue(field, value, 'EndValue');
                },
                "blur": function (field) {
                    var value = field.getValue();
                    var p = field.findParentByType('dateRange');
                    value = p.formatDateValue(value);
                    if (value) field.setValue(value);
                }
            }
        }],

        /**
         * 初始化
         * 开始时间/结束时间等于空默认为当天 
         * *开始时间/结束时间不能设置为空，进入页面报错未知原因*
         * */
        initComponent: function () {
            this.initConfig({ startDate: this.BeginValue, endDate: this.EndValue, isSelected: true });

            var dataTypeArray = [{ Name: "全部".t(), Value: 1 },
            { Name: "当天".t(), Value: 2 },
            { Name: "本周".t(), Value: 4 },
            { Name: "本月".t(), Value: 8 },
            { Name: "最近一月".t(), Value: 16 },
            { Name: "本年".t(), Value: 32 },
            { Name: "自定义".t(), Value: 0 }];
            this.items[0].store.loadData(dataTypeArray);
            this.items[0].labelWidth = this.labelWidth;
            this.items[0].labelAlign = this.labelAlign;
            this.items[0].value = this.dateType;
            if (!this.isEmpty(this.dateFormat)) {
                this.items[1].format = this.dateFormat;
                this.items[2].format = this.dateFormat;
            }
            this.items[1].allowBlank = this.allowBlank;
            this.items[2].allowBlank = this.allowBlank;

            if (this.BeginValue == null)
                this.BeginValue = Ext.Date.parse(Ext.util.Format.date(Ext.Date.add(new Date(), Ext.Date.Day, 0, 'Y-m-d H:i:s'), this.dateFormat), this.dateFormat)
            else
                this.BeginValue = Ext.Date.parse(Ext.util.Format.date(Ext.Date.add(new Date(this.BeginValue), Ext.Date.Day, 0, 'Y-m-d H:i:s'), this.dateFormat), this.dateFormat);
            if (this.EndValue == null)
                this.EndValue = Ext.Date.parse(Ext.util.Format.date(Ext.Date.add(new Date(), Ext.Date.Day, 0, 'Y-m-d H:i:s'), this.dateFormat), this.dateFormat)
            else
                this.EndValue = Ext.Date.parse(Ext.util.Format.date(Ext.Date.add(new Date(this.EndValue), Ext.Date.Day, 0, 'Y-m-d H:i:s'), this.dateFormat), this.dateFormat);
            this.items[1].value = this.BeginValue;
            this.items[2].value = this.EndValue;
            //this.items[1].value = this.BeginValue = Ext.Date.parse(Ext.util.Format.date(Ext.Date.add(new Date(), Ext.Date.Day, 0, 'Y-m-d H:i:s'), this.dateFormat), this.dateFormat);
            //this.items[2].value = this.EndValue = Ext.Date.parse(Ext.util.Format.date(Ext.Date.add(new Date(), Ext.Date.Day, 0, 'Y-m-d H:i:s'), this.dateFormat), this.dateFormat);
            if (this.dateType === 2 && (this.dateFormat === "Y/m/d" || this.dateFormat === "Y-m-d") && this.EndValue !== null) {
                this.BeginValue.setHours(-8);
                this.EndValue.setHours(15);
                this.EndValue.setMinutes(59);
                this.EndValue.setSeconds(59);
            }
            this.callParent();
        },

        //变更值
        changeValue: function (field, value, fieldName) {
            var p = field.findParentByType('dateRange');
            var form = this.up('form');
            if (form && form.SIEView) {
                var entity = form.SIEView.getData();
                if (entity.data[p.name]) {
                    entity.data[p.name][fieldName] = value;
                }
            }
            p.resetDateType(p, p.items, 0);
            p[fieldName] = value;
        },

        //清除值
        clearValue: function () {
            var comboBox = this.down('combobox');
            var form = this.up('form');
            if (form) {
                var entity = form.SIEView.getData();
                entity.data[this.name] = {
                    dateType: 1,
                    BeginValue: null,
                    EndValue: null
                };
            }
            comboBox.select(1);
            comboBox.fireEvent('select', comboBox, daterangeStore.getAt(0));
        },

        //格式化输入的日期
        formatDateValue: function (value) {
            try {
                var formatedVal = Ext.Date.parse(Ext.util.Format.date(new Date(Date.parse(value)), this.dateFormat), this.dateFormat);
                return formatedVal ? formatedVal : null;
            } catch (e) {
                return null;
            }
        },

        isEmpty: function (obj) {
            for (var name in obj) {
                return false;
            }
            return true;
        },

        resetDateType: function (p, field, value) {
            if (p.isSelected === false) {
                var cbo = field.items[0];
                cbo.setValue(value);
            }
        },

        configRangeDate: function (scope, selectValue, ds, isUpdate) {
            var result = [];
            var now = new Date(); //当前日期
            var nowDayOfWeek = now.getDay(); //今天本周的第几天
            var nowDay = now.getDate(); //当前日
            var nowMonth = now.getMonth(); //当前月
            var nowYear = now.getFullYear();
            switch (selectValue) {
                case 0://自定义 开始日期跟结束日期相等直接清空,否则设置自定义默认值
                    if (this.dateEqual(this.BeginValue, new Date(), "Y-m-d 00:00:00")
                        && this.dateEqual(this.EndValue, new Date(), "Y-m-d 23:59:59")) {
                        ds[0].allowBlank = true;
                        ds[1].allowBlank = true;
                        ds[0].setValue(null);
                        ds[1].setValue(null);
                    } else {
                        result[0] = this.dateformat(this.BeginValue, "Y-m-d 00:00:00");
                        result[1] = this.dateformat(this.EndValue, "Y-m-d 23:59:59");
                    }
                    break;
                case 1://全部
                    ds[0].allowBlank = true;
                    ds[1].allowBlank = true;
                    ds[0].setValue(null);
                    ds[1].setValue(null);
                    break;
                case 2://当天
                    result[0] = Ext.util.Format.date(now, "Y-m-d 00:00:00");
                    result[1] = Ext.util.Format.date(now, "Y-m-d 23:59:59");
                    break;
                case 4://本周
                    result[0] = Ext.util.Format.date(new Date(nowYear, nowMonth, nowDay - nowDayOfWeek), "Y-m-d 00:00:00");
                    result[1] = Ext.util.Format.date(new Date(nowYear, nowMonth, nowDay + (6 - nowDayOfWeek)), "Y-m-d 23:59:59");
                    break;
                case 8://本月
                    result[0] = Ext.util.Format.date(Ext.Date.getFirstDateOfMonth(now), "Y-m-d 00:00:00");
                    result[1] = Ext.util.Format.date(Ext.Date.getLastDateOfMonth(now), "Y-m-d 23:59:59");
                    break;
                case 16://最近一月
                    result[0] = Ext.util.Format.date(Ext.Date.add(now, Ext.Date.MONTH, -1), "Y-m-d 00:00:00");
                    result[1] = Ext.util.Format.date(now, "Y-m-d 23:59:59");
                    break;
                case 32://本年
                    var currentYearFirstDate = new Date(nowYear, 0, 1);//本年第一天
                    var currentYearLastDate = new Date(nowYear, 11, 31);//本年最后一天
                    result[0] = Ext.util.Format.date(currentYearFirstDate, "Y-m-d 00:00:00");
                    result[1] = Ext.util.Format.date(currentYearLastDate, "Y-m-d 23:59:59");
                    break;
                default:
                    ds[0].allowBlank = true;
                    ds[1].allowBlank = true;
                    ds[0].setValue(null);
                    ds[1].setValue(null);
                    break;
            }
            if (isUpdate) {
                ds[0].setValue(Ext.Date.parse(Ext.util.Format.date(Ext.Date.parse(result[0], 'Y-m-d H:i:s'), this.dateFormat), this.dateFormat));
                //! 日期的设置值点太多，先谨慎处理，自然月尾的时间值不对应问题 pms-B0019415
                var monthLastDate = Ext.Date.getLastDateOfMonth(now);
                var endDateVal = Ext.Date.parse(result[1], 'Y-m-d H:i:s');
                if (endDateVal) { ////自定义、全部时上面代码未赋值
                    if (monthLastDate.getDate() === endDateVal.getDate()) {
                        ds[1].setValue('');
                    }
                }
                ds[1].setValue(Ext.Date.parse(Ext.util.Format.date(endDateVal, this.dateFormat), this.dateFormat));
            }
            scope.isSelected = false;
            scope.setSecondMinDate(Ext.Date.parse(result[0], "Y-m-d H:i:s"));
            return result;
        },

        //验证
        validate: function () {
            var fromFld = this.down('datefield[itemRole=fromField]');
            var toFld = this.down('datefield[itemRole=toField]');
            if (!fromFld.validate() || !toFld.validate()) return false;
            if ((fromFld.value !== null && toFld.value !== null) && fromFld.value > toFld.value) {
                SIE.Msg.showError('开始日期不能大于结束日期'.t());
                return false;
            }
            return true;
        },

        //设置toPicker最小值
        setSecondMinDate: function (value) {
            // var toField = this.down('datefield[itemRole=toField]');
            // toField.setMinValue(value);
            // toField.validate();
        },

        getValue: function () {
            return Ext.JSON.encode({
                DateRangeType: this.dateType,
                BeginValue: Ext.util.Format.date(this.BeginValue, this.dateFormat),
                EndValue: Ext.util.Format.date(this.EndValue, this.dateFormat)
            });
        },

        /**
         * 格式化时间
         * @param {any} date 日期
         * @param {any} format 格式
         */
        dateformat: function (date, format) {
            return Ext.util.Format.date(date, format);
        },

        /**
         * 对比两个时间是否相等
         * @param {any} dateone 日期1
         * @param {any} datetow 日期2
         * @param {any} format 格式
         */
        dateEqual: function (dateone, datetow, format) {
            if (this.dateformat(dateone, format) == this.dateformat(datetow, format)) {
                return true;
            }
            return false;
        },

        /**
         * 设置日期范围控件
         * (设置日期时自动更改为自定义)
         * @param {any} beginValue 开始时间
         * @param {any} endValue 结束时间
         */
        setDataRangValue: function (beginValue, endValue) {
            this.setDateType(0);
            this.setBeginValue(beginValue);
            this.setEndValue(endValue);
        },

        /**
         * 设置开始时间
         * @param {any} beginVal 开始时间
         */
        setBeginValue: function (beginVal) {
            if (this.dateType == 0)
                this.setDateType(0);
            this.items.items[1].setValue(beginVal);
            this.setEntiyValue("BeginValue", beginVal)
        },

        /**
         * 设置结束时间 
         * @param {any} endVal 结束时间
         */
        setEndValue: function (endVal) {
            if (this.dateType == 0)
                this.setDateType(0);
            this.items.items[2].setValue(endVal);
            this.setEntiyValue("EndValue", endVal)
        },

        /**
         * 设置当前日期范围
         * @param {any} datetype 日期范围
         */
        setDateType: function (datetype) {
            this.dateType = datetype;
            this.items.items[0].setValue(this.dateType);
        },

        /**
         * 维护实体日期范围同步
         * @param {any} fieldName 更新字段
         * @param {any} value 更新值
         */
        setEntiyValue: function (fieldName, value) {
            var form = this.up('form');
            if (form && form.SIEView) {
                var entity = form.SIEView.getData();
                if (entity.data[this.name]) {
                    entity.data[this.name][fieldName] = value;
                } else {
                    entity.data[this.name] = {
                        dateType: this.dateType,
                        BeginValue: Ext.util.Format.date(Ext.Date.add(this.BeginValue, Ext.Date.Day, 0), "Y-m-d H:i:s"),
                        EndValue: Ext.util.Format.date(Ext.Date.add(this.EndValue, Ext.Date.Day, 0), "Y-m-d H:i:s")
                    };
                }

            }
        }


    });


//定义下拉多选控件
Ext.define('SIE.control.DataArray', {
	extend: 'Ext.form.field.Tag',
	alias: 'widget.dataarray',
	getValue: function () {
		var e = this.callParent();
		var form = this.up("form");
		if (form && form.SIEView) {
			this.up("form").SIEView.getData().data[this.name] = this.value;
			//console.log(this.value);
		}
		//console.log(e);
		return e;
	}
});



Ext.define('SIE.control.TextField', {
    extend: 'Ext.form.field.Text',
    alias: 'widget.formtextfield',
    /**
    * 下拉编辑器初始化
    */
    initComponent: function () {
        var me = this;
        me.regex = new RegExp(me.regex);
        me.callParent();
    },
});
//文本范围控件
Ext.define('SIE.control.TextRange', {
        extend: 'Ext.container.Container',
        alias: 'widget.textRange',
        layout: 'hbox',

        fieldLabel: '',
        firstDefaultText: '',
        lastDefaultText: '',
        maxTextLength: '',
        minTextLength: '',
        regex: '',
        regexText: '',
        align: 'right',
        allowBlank: true,
        readOnly: false,
        firstText: '',
        lastText: '',
        beforeRender: function() {
                this.callParent();
                var me = this;
                var form=this.up('form');
                if (form) {
                    var entity = form.SIEView.getData();
                    entity.data[this.name] = {
                        firstText: this.firstText,
                        lastText: this.lastText
                    };
                }
        },
        items: [{
                xtype: 'textfield',
                anchor: '100%',
                isFormField: false,
                hiddenLabel: true,
                labelWidth: 85,
                //目前给定值
                labelAlign: 'right',
                minLengthText: "输入内容太短了".t(),
                maxLengthText: "输入内容太长了".t(),
                inputType: 'search',
                listeners: {
                        "change": function(field, newValue, oldValue) {
                            var param = field.findParentByType("textRange");
                            var form = this.up('form');
                            if (form) {
                                var entity = form.SIEView.getData();
                                if (entity.data[param.name] != null) {
                                    entity.data[param.name].firstText = newValue;
                                }
                            }

                            param.firstText = newValue;
                        }
                }
        },
        {
                xtype: 'textfield',
                anchor: '100%',
                isFormField: false,
                hiddenLabel: true,
                minLengthText: "输入内容太短了".t(),
                maxLengthText: "输入内容太长了".t(),
                inputType: 'search',
                listeners: {
                        "change": function(field, newValue, oldValue) {
                            var param = field.findParentByType("textRange");
                            var form = this.up('form');
                            if (form) {
                                var entity = form.SIEView.getData();
                                if (entity.data[param.name] != null) {
                                    entity.data[param.name].lastText = newValue;
                                }
                            }
                                
                            param.lastText = newValue;
                        }
                }
        }],
        initComponent: function() {
                this.initConfig({
                        firstText: this.firstText,
                        lastText: this.lastText
                });
                this.items[0].fieldLabel = this.fieldLabel;
                this.items[0].value = this.firstText;
                this.items[0].allowBlank = this.allowBlank;
                if (this.minTextLength > 0) this.items[0].maxLength = this.minTextLength;
                if (this.maxTextLength > 0) this.items[0].maxLength = this.maxTextLength;
                if (this.isEmpty(this.regex) == false) {
                        var rStr = this.regex.replace(/\//g, '');
                        var reg = new RegExp(rStr);
                        if (this.isEmpty(this.regex) == false) {
                                this.items[0].regex = reg;
                                this.items[0].regexText = this.regexText;
                        }
                }
                if (this.readOnly) this.items[0].readOnly = this.readOnly

                this.items[1].value = this.lastText;
                this.items[1].allowBlank = this.allowBlank;
                if (this.minTextLength > 0) this.items[1].maxLength = this.minTextLength;
                if (this.maxTextLength > 0) this.items[1].maxLength = this.maxTextLength;
                if (this.isEmpty(this.regex) == false) {
                        this.items[1].regex = reg;
                        this.items[1].regexText = this.regexText;
                }
                if (this.readOnly) this.items[1].readOnly = this.readOnly

                this.callParent();
        },

        onResize: function(w, h, ow, oh) {
                var me = this;
                var p = me.findParentByType('panel');
                var setWidth = w / 2;
                var n1 = me.items.items[0];
                var n2 = me.items.items[1];
                var lw = n1.labelWidth;
                var rw = n1.getWidth();
                var rw2 = n2.getWidth();
                n1.setWidth(setWidth + lw / 2);
                n2.setFieldLabel();
                n2.setWidth(setWidth - lw / 2);
                this.callParent(arguments);
        },

        clearValue: function() {
                var inputs = this.el.query('input');
                inputs[0].value = this.firstText = null;
                inputs[1].value = this.lastText = null;
        },
        isEmpty: function(obj) {
                for (var name in obj) {
                        return false;
                }
                return true;
        },

        validate:function(){
        if(!this.items.items[0].validate()||!this.items.items[1].validate())
            return false;
        else
            return true
        },
        getValue: function () {
            return Ext.JSON.encode({
                BeginValue: this.firstText,
                EndValue: this.lastText
            });
        }

});

Ext.define('SIE.control.TimePickerField', {
    extend: 'Ext.form.field.Base',
    alias: ['widget.timePickerField'],    

    requires: ['Ext.form.field.Number'],
    inputType: 'text',
    labelWidth: 40,
    style: 'padding:4px 0; margin: 0; ',
    value: null,
    spinnerCfg: {
        minWidth: 70,
        width:70
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
        me.spinners = [];
        var owner = me;
        var cfg = Ext.apply({}, me.spinnerCfg, {
            //          readOnly: me.readOnly,
            disabled: me.disabled,
            style: 'float: left',
            listeners: {
                change: {
                    fn: me.onSpinnerChange,
                    scope: me
                }
            },

        });

        me.hoursSpinner = Ext.create('Ext.form.field.Number', Ext.apply({}, cfg, {
            minNum: 0,
            maxNum: 23,
        }));
        me.minutesSpinner = Ext.create('Ext.form.field.Number', Ext.apply({}, cfg, {
            minNum: 0,
            maxNum: 59,
        }));
        me.secondsSpinner = Ext.create('Ext.form.field.Number', Ext.apply({}, cfg, {
            minNum: 0,
            maxNum: 59
        }));
        me.spinners.push(me.hoursSpinner, me.minutesSpinner, me.secondsSpinner);
    },

    onRender: function () {
        var me = this, spinnerWrapDom, spinnerWrap;
        me.callParent(arguments);
        spinnerWrap = Ext.get(Ext.DomQuery.selectNode('div', this.el.dom));
        me.callSpinnersFunction('render', spinnerWrap);
        this.el.dom.getElementsByTagName('input')[0].style.display = 'none';
        var newTimePicker = Ext.DomHelper.append(spinnerWrap, {
            tag: 'div',
            cls: 'x-form-clear-left'
        }, true);

        var cell = this.up('container');
        if (cell && cell.column) {
            var columnWidth = cell.column.width;
            var spinnerWidth = (columnWidth - columnWidth % 3 - 27) / 3;
            me.hoursSpinner.setWidth(spinnerWidth);
            me.minutesSpinner.setWidth(spinnerWidth);
            me.secondsSpinner.setWidth(spinnerWidth);
            me.hoursSpinner.setStyle('padding-left', '1px');
            me.minutesSpinner.setStyle('padding-left', '1px');
            me.secondsSpinner.setStyle('padding-left', '1px');
        }

        this.setRawValue(this.value);
    },
    _valueSplit: function (v) {
        if (Ext.isDate(v)) {
            v = Ext.Date.format(v, 'H:i:s');
        }
        if (!v || v instanceof Object)
            return v;
        var split = v.split(':');
        return {
            h: split.length > 0 ? split[0] : 0,
            m: split.length > 1 ? split[1] : 0,
            s: split.length > 2 ? split[2] : 0
        };
    },
    onSpinnerChange: function () {
        if (!this.rendered) {
            return;
        }
        //限制时间范围
        var args = arguments; //this, newValue, oldValue, eOpts
        args[0].setValue(args[1] > args[0].maxNum ? args[0].minNum : args[0].value);
        this.fireEvent('change', this, this.getValue(), this.getRawValue());
    },

    // 依次调用各输入框函数, call each spinner's function
    callSpinnersFunction: function (funName, args) {
        for (var i = 0; i < this.spinners.length; i++) {
            if (this.spinners[i][funName] != null && this.spinners[i][funName] != undefined) {
                this.spinners[i][funName](args);
            }
        }
    },

    getRawValue: function () {
        if (!this.rendered) {
            var date = this.value || new Date();
            return this._valueSplit(date);
        }
        else {
            return {
                h: this.hoursSpinner.getValue(),
                m: this.minutesSpinner.getValue(),
                s: this.secondsSpinner.getValue()
            };
        }
    },
    setRawValue: function (value) {
        if (value) {
            var v = this._valueSplit(value);
            if (this.hoursSpinner) {
                this.hoursSpinner.setValue(v.h);
                this.minutesSpinner.setValue(v.m);
                this.secondsSpinner.setValue(v.s);
            }
        }
    },

    getValue: function () {
        var v = this.getRawValue();
        return Ext.String.leftPad(v.h, 2, '0') + ':' + Ext.String.leftPad(v.m, 2, '0') + ':'
            + Ext.String.leftPad(v.s, 2, '0');
    },

    setValue: function (value) {
        this.value = Ext.isDate(value) ? Ext.Date.format(value, 'H:i:s') : value;
        if (!this.rendered) {
            return;
        }
        var reg = /^\d{1,2}:\d{1,2}:\d{1,2}$/;
        if (!this.value || !reg.test(this.value))
            this.value = '00:00:00';
        this.setRawValue(this.value);
        this.validate();
    },

    disable: function () {
        this.callParent(arguments);
        this.callSpinnersFunction('disable', arguments);
    },

    enable: function () {
        this.callParent(arguments);
        this.callSpinnersFunction('enable', arguments);
    },

    setReadOnly: function () {
        this.callParent(arguments);
        this.callSpinnersFunction('setReadOnly', arguments);
    },

    clearInvalid: function () {
        this.callParent(arguments);
        this.callSpinnersFunction('clearInvalid', arguments);
    },

    isValid: function (preventMark) {
        return this.hoursSpinner.isValid(preventMark) && this.minutesSpinner.isValid(preventMark)
            && this.secondsSpinner.isValid(preventMark);
    },

    validate: function () {
        return this.hoursSpinner.validate() && this.minutesSpinner.validate() && this.secondsSpinner.validate();
    }
});
Ext.define('SIE.control.DateTimePicker', {
    extend: 'Ext.picker.Date',
    alias: 'widget.datetimepicker',
    requires: ['SIE.control.TimePickerField', 'Ext.dom.Query'],

    todayText: '现在'.t(),
    timeLabel: '时间'.t(),
    buttonText: '确定'.t(),
    bindname: "",
    //组件初始化
    initComponent: function () {
        this.callParent();
        this.value = this.value || new Date();
        this.todayText = this.todayText.t();
    },

    onRender: function (container, position) {
        this.callParent(arguments);
        var me = this;

        //确认按键
        var btnCfg = Ext.apply({}, {}, {
            style: 'center',
            listeners: {
                click: {
                    fn: function () {
                        this.confirmDate();
                    },
                    scope: me
                }
            }
        });
        me.confirmBtn = Ext.create('Ext.Button', Ext.apply({}, btnCfg, {
            text: '确认'.t(),
        }));
        me.confirmBtn.render(this.el.child('div div.x-datepicker-footer'));


        if (!this.timefield) {
            this.timefield = Ext.create('Ext.form.field.Time',
                {
                    fieldLabel: '时间'.t(),
                    increment: 30,
                    width: 205,
                    labelWidth: 36,
                    value: Ext.Date.format(this.value, 'H:i:s'),
                    format: 'H:i:s'
                })
        }

        this.timefield.ownerCt = this;//指定范围
        this.timefield.on('change', this.timeChange, this);//

        var table = Ext.get(Ext.DomQuery.selectNode('table', this.el.dom));

        var tfEl = Ext.DomHelper.insertAfter(table, {
            tag: 'div',
            style: 'border:0px;',
            children: [{
                tag: 'div',
                cls: 'x-datepicker-footer ux-timefield'
            }]
        }, true);
        this.timefield.render(this.el.child('div div.ux-timefield'));

        var p = this.getEl().parent('div.x-layer');
        if (p) {
            p.setStyle("height", p.getHeight() + 31);
        }

        me.on('beforedestroy', function (cmp) {
            if (me.timefield) me.timefield.destroy();
        });
    },

    // listener 时间域修改, timefield change
    timeChange: function (tf, newVal, oldVal) {
        var me = this;
        me.setValue(this.fillDateTime(new Date(me.value)));
    },

    //
    fillDateTime: function (value) {
        if (this.timefield) {
            var timeHis = this.timefield.getValue();
            value.setHours(timeHis.getHours());
            value.setMinutes(timeHis.getMinutes());
            value.setSeconds(timeHis.getSeconds());
        }
        return value;
    },

    changeTimeFiledValue: function (value) {
        this.timefield.un('change', this.timeChange, this);
        this.timefield.setValue(this.value);
        this.timefield.on('change', this.timeChange, this);
    },

    //设置值
    setValue: function (value) {
        this.value = value;
        this.changeTimeFiledValue(value);
        this.update(this.value, true);
    },

    //获取值
    getValue: function () {
        return this.fillDateTime(this.value);
    },

    //日期点击事件
    handleDateClick: function (e, t) {
        var me = this,
            handler = me.handler;
        e.stopEvent();
        if (!me.disabled && t.dateValue && !Ext.fly(t.parentNode).hasCls(me.disabledCellCls)) {
            me.doCancelFocus = me.focusOnSelect === false;
            me.setValue(this.fillDateTime(new Date(t.dateValue)));
            delete me.doCancelFocus;
            //me.fireEvent('select', me, me.value);
            if (handler) {
                handler.call(me.scope || me, me, me.value);
            }
            me.onSelect();
        }
    },

    //确认按键 
    confirmDate: function () {
        this.fireEvent('select', this, this.value);
        this.onSelect();
    },

    //选择现在
    selectToday: function () {
        var me = this,
            btn = me.todayBtn,
            handler = me.handler;
        if (btn && !btn.disabled) {
            me.setValue(new Date());
            me.fireEvent('select', me, me.value);
            if (handler) {
                handler.call(me.scope || me, me, me.value);
            }
            me.onSelect();
        }
        return me;
    },

    onFocusLeave: function (e) {
        var me = this;
        me.callParent(e);
        me.onSelect();
    },

    /**
     * 隐藏picker更新值
     * */
    onHide: function () {
        var me = this;
        if (me.getBind()) {//高级查询存在未双向绑定控件，所以未绑定时直接赋值
            if (me.up("form")) {
                entity = me.up("form").SIEView.getData();
            } else {
                entity = me.up('container').context.record
            }
            var filevalue = entity.get(me.bindname);
            if (me.value != filevalue) {
                me.setValue(me.value);
                entity.set(me.bindname, me.value)
            }
        } else
            me.fireEvent('select', me, me.value);
        me.callParent();
    }
});
Ext.define('SIE.control.DateTimeField', {
    extend: 'Ext.form.field.Date',
    alias: 'widget.datetimefield',
    requires: ['SIE.control.DateTimePicker'],
    initComponent: function () {
        this.format = this.format;
        this.callParent();
    },
    format: 'Y-m-d H:i:s',
    formatText: '',
    minText: null,
    maxText: null,
    invalidText: '{0}不是有效日期'.t(),
    createPicker: function () {
        var me = this,
            format = Ext.String.format;
        this.rawDate = this.value || this.config.dateTimeValue;
        me.minValue = new Date(this.config.minValue);
        me.maxValue = new Date(this.config.maxValue);
        return Ext.create('SIE.control.DateTimePicker', {
            ownerCt: me.ownerCt,
            //                  renderTo: document.body,
            floating: true,
            //                  hidden: true,
            focusOnShow: true,
            minDate: me.minValue,
            maxDate: me.maxValue,
            bindname: me.name,
            disabledDatesRE: me.disabledDatesRE,
            disabledDatesText: me.disabledDatesText,
            disabledDays: me.disabledDays,
            disabledDaysText: me.disabledDaysText,
            format: me.format,
            showToday: me.showToday,
            startDay: me.startDay,
            minText: format(me.minText, me.formatDate(me.minValue)),
            maxText: format(me.maxText, me.formatDate(me.maxValue)),
            listeners: {
                scope: me,
                select: me.onSelect,
            },
            keyNavConfig: {
                esc: function () {
                    me.collapse();
                }
            }
        });
    },
    //展开的时候设置它的默认值
    onExpand: function () {
        var value = this.rawDate;
        value = Ext.isDate(value) ? value : (value ? new Date(value) : new Date());
        this.picker.setValue(Ext.isDate(value) ? value : this.createInitialDate());
        this.rawDate = this.value;
    },
    collapse: function () {
        var me = this;
        var srcElement = event.srcElement;
        var isSelectDate = (' ' + srcElement.className + ' ').indexOf(' ' + 'x-datepicker-date' + ' ') > -1
            || srcElement.textContent == '确认'.t()
            || (me.picker && !Ext.fly(srcElement).up('#' + me.picker.el.dom.id)) || me.getRawValue()==="";

        if (!isSelectDate) {
            me.el.dom.focus();
            return;
        }

        if (me.isExpanded && !me.destroyed && !me.destroying) {
            var openCls = me.openCls,
                picker = me.picker,
                aboveSfx = '-above';
            // hide the picker and set isExpanded flag
            picker.hide();
            me.isExpanded = false;
            // remove the openCls
            me.bodyEl.removeCls([
                openCls,
                openCls + aboveSfx
            ]);
            picker.el.removeCls(picker.baseCls + aboveSfx);
            if (!me.ariaStaticRoles[me.ariaRole]) {
                me.ariaEl.dom.setAttribute('aria-expanded', false);
            }
            // remove event listeners
            me.touchListeners.destroy();
            me.scrollListeners.destroy();
            Ext.un('resize', me.alignPicker, me);
            me.fireEvent('collapse', me);
            me.onCollapse();
        }
    },

    getErrors: function (value) {
        var errors = this.callParent(arguments);
        for (var i = 0; i < errors.length; i++) {
            if (!errors[i]) {
                errors.splice(i, 1);
                i--;
            }
        }
        return errors;
    },

});
Ext.define('SIE.control.ImageControl', {
    extend: 'Ext.form.field.FileButton',

    alias: ['widget.imageControl'],
    binder: null,
    container: null,

    afterTpl: [
        '<input id="{id}-fileInputEl" data-ref="fileInputEl" class="{childElCls} {inputCls}"  accept=".jpg,.jpeg,.png,.gif,.bmp,.pdf" ',
        'type="file" size="1" name="{inputName}" unselectable="on" hidden="hidden"',
        '<tpl if="accept != null">accept="{accept}"</tpl>',
        '<tpl if="tabIndex != null">tabindex="{tabIndex}"</tpl>',
        '>',
        '<img id="{id}-fileInputElimg" data-ref="fileInputEl"  style="height: inherit; width: inherit;" class="{childElCls} {inputCls}" ',

        '>'
    ],

    onRender: function () {
        var me = this;
        this.callParent();

        var dom = this.el.dom;
        delete dom.firstChild;

        var field = this.up(),
            binder,
            srcValue;
        binder = me.binder = field.getBind();

        if (binder) {
            var owner = binder.owner;
            while (binder && !owner) {
                binder = binder[Object.keys(binder)];
                owner = binder.owner;
            }
            if (binder && owner) {
                me.binder = binder;
                srcValue = owner.data.p.get(field.name);
            }
        }
        else {
            var container = me.container = field.up('container');
            while (container && !container.context) { container = container.up('container'); }
            if (container) {
                me.container = container;
                var record = container.context.record;
                srcValue = record.get(field.name);
            }
        }
        dom.lastChild.setAttribute('src', srcValue);
    },

    listeners: {
        click: function (button, e, eOpts) {
            button.el.dom.children[1].click();
        },

        change: function (field, newValue, oldValue) {
            var me = this;
            var file = field.fileInputEl.dom.files.item(0);
            if (!file)
                return;
            value = file;
            var fileName = file.name.substring(file.name.lastIndexOf(".") + 1).toLowerCase();
            if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "bmp" && fileName != "gif") {
                Ext.MessageBox.alert("提示".t(), "请选择图片格式文件上传(jpg,png,gif,bmp,gif等)！".t());
                field.fileInputEl.dom.value = "";
                return false;
            }

            var fileSize = file.size;
            var size = fileSize / 1024;
            if (size > 1000) {
                Ext.MessageBox.alert("提示".t(), "附件不能大于1M".t());
                field.fileInputEl.dom.value = "";
                return false;
            }

            var fileReader = new FileReader('file://' + newValue);
            fileReader.readAsDataURL(file);
            fileReader.onload = function (e) {
                var img = field.el.dom.children[2];
                img.src = e.target.result;
                field = field.up();
                field.setValue(e.target.result);
                var entity;
                if (me.binder)
                    entity = me.binder.owner.data.p;
                else
                    entity = me.container.context.record;

                if (entity) {
                    entity.set(field.name, e.target.result);
                    entity.dirty = true;
                }
            }
        }
    }

})
Ext.define('SIE.control.ImageField', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.imageField',
    layout: 'vbox',
    imgHeight: 300,
    imgWidth: 400,
    style:'width:100%; position:static; bottom:0px;',
    config: {
        value:'',
        imgData: '',
        imgUrl: '',
    },
    items: [{
        xtype: 'imageControl',
    }],

    listeners:{
        change: function (field, newValue, oldValue) {
                
                var file = field.fileInputEl.dom.files.item(0);
                
            }
    }
});


Ext.define('SIE.control.ImageComponent', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.imageComponent',
    layout: 'vbox',
    _imageDom: null,    
    items: [
        {
            xtype: 'image',
            //页面渲染前给图片控件的属性赋值
            beforeRender: function () {
                var me = this;
                if (this.up().up().SIEView && this.up().up().SIEView.viewGroup == "DetailsView") {
                    this.src = this.up().up().SIEView.getData().get(this.up().name);
                } else {
                    this.src = this.up().up().SIEView.getCurrent().get(this.up().name);
                }
                if (this.up().config.imageSrc !== "") {
                    this.src = this.up().config.imageSrc;
                };
                this.width = this.up().config.imageWidth;
                this.height = this.up().config.imageHeight;
                this.cls = this.up().config.imgCls;
            },
            //手动给图片增加点击事件
            listeners: {
                el: {
                    click: 'onClick'
                }
            },
            //调用文件按钮的弹窗上传文件
            onClick: function () {
                //this.up().items.items[1].el.dom.childNodes[1].click();
                if (this.up().items.items[1].fileInputEl.el !== null) {
                    this._imageDom = this.up().items.items[1].fileInputEl.el.dom;
                    this.up().items.items[1].fileInputEl.el.dom.click();
                    this.up().items.items[1].fireEvent('change', this.up().items.items[1]);
                } else {
                    this._imageDom.click();
                    this.up().items.items[1].fireEvent('change', this.up().items.items[1]);
                }         
                //Ext.query('[name=filebutton-1242]')[0].click();
                //Ext.getCmp('imageComponentFilebutton_Id').el.dom.childNodes[1].click();
            }
        },
        {
            xtype: 'filebutton',
            hidden: true,          //隐藏按钮
            listeners: {
                change: function (field, newValue, oldValue) {
                    var me = this;
                    if (!field.fileInputEl.dom) {
                        Ext.MessageBox.alert("提示", "请选择正确的图片格式文件上传(jpg,png,gif,bmp,gif等)！请勿重复连续点击上传图片！请退出重新修改！".t());
                        return;
                    }
                    var file = field.fileInputEl.dom.files.item(0);
                    if (!file)
                        return;
                    value = file;
                    var fileName = file.name.substring(file.name.lastIndexOf(".") + 1).toLowerCase();
                    if (fileName != "jpg" && fileName != "jpeg" && fileName != "png" && fileName != "bmp" && fileName != "gif") {
                        Ext.MessageBox.alert("提示".t(), "请选择图片格式文件上传(jpg,png,gif,bmp,gif等)！".t());
                        field.fileInputEl.dom.value = "";
                        return false;
                    }

                    var fileSize = file.size;
                    var size = fileSize / 1024;
                    if (size > 1000) {
                        Ext.MessageBox.alert("提示".t(), "附件不能大于1M".t());
                        field.fileInputEl.dom.value = "";
                        return false;
                    }

                    var fileReader = new FileReader('file://' + newValue);
                    fileReader.readAsDataURL(file);
                    fileReader.onload = function (e) {
                        var img = field.up().items.items[0];
                        field.up().up().SIEView.getData().set(field.up().name, e.target.result);
                        img.setSrc(e.target.result);
                    }
                }
            }
        }
    ],
});
Ext.define('SIE.control.TabPanel', {
    extend: 'Ext.tab.Panel',
    xtype: 'sietabpanel',
    alias: ['widget.sietabpanel'],
    listeners: {
        'tabchange': function (tp, tab) {
            if (!tp.loaded) return;
            if (tab && tab.url)
                window.top.location.href = tab.url;
            else
                window.top.location.href = window.top.location.origin + '/#';
        },
    },
    remove: function (component, autoDestroy) {
        var me = this,
            args = arguments,
            view = null,
            c = me.getComponent(component);

        // After destroying, items is nulled so we can't proceed 
        if (me.destroyed || me.destroying) {
            return;
        }

        c = me.getComponent(component);
        me.setActiveItem(component);

        //<debug> 
        if (!arguments.length) {
            Ext.log.warn(
                "Ext.container.Container: remove takes an argument of the component to remove. cmp.remove() is incorrect usage.");
        }

        if (c && (!me.hasListeners.beforeremove || me.fireEvent('beforeremove', me, c) !== false)) {
            var iframe = component.body.dom.getElementsByTagName('iframe')[0];
            //var element = iframe.contentWindow.Ext; 存在iframe未定义的情况
            try {
                if (iframe && iframe.contentWindow.Ext && iframe.contentWindow.Ext._bodyEl && iframe.contentWindow.Ext.getBody().component && iframe.contentWindow.Ext.getBody().component.view) {
                    view = iframe.contentWindow.Ext.getBody().component.view;
                } else {
                    me.doRemove(component, true);
                    return;
                }
            } catch (ex) {
                me.doRemove(component, true);
                return;
            }
            if (!view.closeView)
                view = !view.grid ? view.ownerGrid.SIEView : view.grid.SIEView;
            var returnObj = view.closeView();
            if (returnObj.hasData) {
                Ext.MessageBox.confirm("提示".t(),
                    "数据还未保存，是否继续退出？".L10N(),
                    function (btn) {
                        if (btn == "yes") {
                            returnObj.callback(view);
                            //me.closeTab(me, c, autoDestroy, view, view.getControl(), returnObj.data);
                            var data = returnObj.data;
                            if (view && view.mun) {
                                view.mun(view, 'beforeclosewin');
                                if (data) {
                                    data.propertyChangedEvents = [];
                                    view.mun(data, 'propertyChanged');
                                }
                                if (view._current && view._current.belongsView)
                                    view.getCurrent().belongsView = null;
                            }
                            me.closeTab(me, c, autoDestroy);
                        }
                    });
            } else {
                me.closeTab(me, c, autoDestroy);
            }
        }
        return c;
    },

    closeTab: function (me, c, autoDestroy) {

        if (c.getFrame) {
            var page = c.getFrame().contentWindow.Page;
            if (page && page.onClose) page.onClose();
        }

        me.doRemove(c, autoDestroy);

        if (me.hasListeners.remove) {
            me.fireEvent('remove', me, c);
        }

        if (!me.destroying && !me.destroyAfterRemoving && !c.floating) {
            me.updateLayout();
        }

        if (me.destroyAfterRemoving) {
            me.destroy();
        }
    }
});
Ext.define('SIE.control.TextButtonField', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.TextButtonField',
    triggerCls: "x-form-search-trigger",
    border: false,
    extendObj:null,
    initComponent: function () {
        this.callParent();
        if (this.config.ExtendJsObj !== undefined && this.config.ExtendJsObj !== '') {
            extendObj = Ext.create(this.config.ExtendJsObj);
            if (Ext.isFunction(extendObj.onClick))
                this.on('TriggerClick', extendObj.onClick);
        }
    },
    onTriggerClick: function (field, trigger, e) {
        var me = this;
        if(!extendObj.isOpen)
            me.fireEvent('TriggerClick', field, trigger, e);
    },

    
});
Ext.define('SIE.control.ConfigCategoryEditor', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.configcategoryeditor',
    triggerCls: "x-form-search-trigger",
    //初始化
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    //触发器事件
    onTriggerClick: function (field, trigger, e) {
        var me = this;
        me._createLayout(field);
    },

    _createLayout: function (field) {
        var me = this;
        var cell = me.up('container').context;
        var view = cell.view.up().SIEView;
        var mainview = view.getParent();
        var data = mainview.getData();
        var modelname = data.data.CategoryTypeName.split(',');
        SIE.AutoUI.getMeta({
            model: modelname[0],
            ignoreCommands: false,
            isReadonly: true,
            ignoreQuery: false,
            isLookup: true,
            callback: function (res) {
                var mainblock;
                if (res.mainBlock) mainblock = res.mainBlock;
                else mainblock = res;
                var ui = SIE.AutoUI.generateAggtControl(mainblock);
                var displayMember = mainblock.gridConfig.displayMember;
                var listview = ui.getView();
                listview.loadData();
                var items = ui.getControl();
                var win = SIE.Window.show({
                    title: "选择类型".t(),
                    items: items,
                    width: 1000,
                    height: 400,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var entity = cell.record;
                            var selection = listview._selection;
                            if (selection) {
                                var selected = selection[0];
                                field.setValue(selected.get(displayMember));
                                entity.set('Category',selected.get(displayMember));
                                entity.set('CategoryKey', selected.getId());
                                entity.phantom = true;
                                view.syncCmdState();
                            }
                        }
                    }
                });
            }
        });
    },

    //override
	/**
	 * 在设置的同时，把选择项的 bindDisplayField 同步到记录上
	 * @param value 设置值
	 
    setValue: function (value) {
        var me = this;
        me.setRawValue(value);
    }*/
});
Ext.define('SIE.control.ConfigValueEditor', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.ConfigValueEditor',
    triggerCls: 'ux-form-edit-trigger',
    /* 
    * 此编辑对 View.Property(p => p.XXX).UseTextButtonPlayEditor(...) 开放配置，
    * 注意点：添加的样式要有Ext样式规范,符合触发样式，要不划过会不见 x-form-edit-trigger
    */
    border: false,
    invalidMsg: '请先填写正确的值！'.t(),
    //初始化
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    //触发器事件
    onTriggerClick: function (field, trigger, e) {
        var me = this;
        me._createLayout(field);
    },
    _createLayout: function (field) {
        var me = this;
        var from, context, current, data, className, ConfigValue, ModuleType;
        from = me.up('form');
        if (from) {
            current = from.SIEView.getData();
            data = current.getData();
            className = data.TypeName.split(",");
            ConfigValue = data.ConfigValue;
            ModuleType = data.ModuleType;
        } else {
            context = me.up('container').context;
            current = context.view.grid.SIEView.getParent();
            data = current.getData();
            className = data.getTypeName().split(",");
            ConfigValue = context.record.data.Value ? context.record.data.Value : data.data.ConfigValue;
            ModuleType = data.data.ModuleType;
        }
        SIE.AutoUI.getMeta({
            model: ModuleType || className[0] + "Value",
            isDetail: true,
            ignoreCommands: true,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock) mainBlock = res.mainBlock;
                else mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                //todo begin 此次先兼容写法（兼容现有全局配置项)，还要重构
                if (Ext.isString(ConfigValue)) {
                    var textValue;
                    try {
                        textValue = Ext.JSON.decode(ConfigValue);
                        store.data = textValue;
                        detailView.setData(store);
                    } catch (exp) {
                        if (ConfigValue != "") {
                            SIE.Msg.showError('配置信息JSON格式错误,生成默认配置！'.t());
                        }
                        detailView.setData(entity);
                        detailView._setDefaultValue(entity);
                    }
                } else {
                    //ConfigValue未定义的情况
                    if (ConfigValue) {
                        entity.data = ConfigValue;
                        detailView.setData(entity);
                    } else {
                        detailView.setData(entity);
                        detailView._setDefaultValue(entity);
                    }
                }
                // end
                var ui = detailView.getControl();

                var configValue = Ext.JSON.encode(ConfigValue);
                var modelList = [];
                ui.items.items.forEach(function (e) {
                    if (e.xtype === "pagingLookUp" && e.model !== "SIE.Common.Catalogs.Catalog") {
                        var model = {
                            Value: e.id,
                            Name: e.model,
                            BingIdValue: e._event.method._getSIEView().getData().data[e.name],
                            BingDisPalyName: e.displayField
                        }
                        modelList.push(model);
                    }
                });

                if (modelList.length > 0) {
                    //执行异步请求
                    me._getDisPlay(modelList).then(
                        function (flag) {
                            if (flag) {
                                //这里放置延迟加载:打卡页签(由于异步请求的数据未获取到)
                                me._showWindow(ui, detailView, from, context, field)
                            }
                        }).
                        always(function () {
                            // Do something whether call succeeded or failed
                        });
                }
                else { me._showWindow(ui, detailView, from, context, field); }
                me._configInvalidMsg(ModuleType);
            }
        });

    },
    _configInvalidMsg: function (type) {
        if ('SIE.Rbac.Users.Configs.PasswordConfigValue' === type) {
            this.invalidMsg = '密码不能为空！'.t();
        }
    },
    /* 
    * 此方法异步请求：获取detailview展示值，并赋值给编辑框，
    * 注意点：方法里面使用了 Ext.Deferred对象，解决第一次请求，未获取到页面加载的值
    */
    _getDisPlay: function (modelList) {
        var me = this;
        var deferred = new Ext.Deferred(); // create the Ext.Deferred object
        var initdata = {};
        initdata = modelList;
        var entityargs = {
            Data: JSON.stringify(initdata)
        }
        if (modelList.Name !== "SIE.Common.Catalogs.Catalog") {
            var aOp = {
                url: "/api/ConfigValue/DisPlay",
                async: true,
                method: 'POST',
                params: entityargs,
                success: function (o) {
                    var displayValue = Ext.JSON.decode(o.responseText).Result;
                    var returnValue = Ext.JSON.decode(displayValue);
                    returnValue.forEach(function (item) {
                        //Ext.getCmp(item.Name).setRawValue(item.Value);
                        var commlist = Ext.getCmp(item.Name);
                        if (commlist.model !== "SIE.Common.Catalogs.Catalog") {
                            commlist.up().SIEView.getData().data[commlist.bindDisplayField] = item.Value;
                        }
                        deferred.resolve(true);
                    });
                },
                failure: function (o) { console.error(o); }
            };
            SIE.Ajax(aOp);
        } else {

        }
        return deferred.promise;  // return the Promise to the caller
    },
    /* 
     * 打开显示值配置页面，
     * ui：视图 detailView：详细页面 from：表单 
     */
    _showWindow: function (ui, detailView, from, context, field) {
        var me = this;
        var win = SIE.Window.show({
            title: '配置'.t(),
            width: 600,
            height: 300,
            items: ui,
            callback: function (btn) {
                if (btn == "确定".t()) {
                    if (!detailView.validateData()) {
                        SIE.Msg.showMessage(me.invalidMsg.t());
                        return false;
                    }
                    var items = detailView.getControl();
                    var view = items.SIEView;
                    var entity = view.getData();
                    items.items.items.forEach(function (o) {
                        var keys = o.bindDisplayField;
                        delete entity.data[keys];
                    });
                    var ConfigValue = null;
                    if (from)
                        ConfigValue = from.SIEView.getData().data.ConfigValue;
                    else
                        ConfigValue = context.record.data.Value;
                    if (!ConfigValue || !(ConfigValue !== entity.data)) {
                        var configValue = Ext.JSON.encode(entity.data);
                        var name = view.model;
                        var modelvalue = {
                            Value: configValue,
                            Name: name
                        }
                        Ajax('/api/ConfigValue/ConfigValueDisPlay', {
                            method: 'POST',
                            isAsync: 'true',
                            data: modelvalue,
                            onSuccess: function (o) {
                                var displayValue = Ext.JSON.decode(o.responseText).Result;
                                if (from) {
                                    from.SIEView.getData().dirty = true;
                                    field.setValue(displayValue);
                                    from.SIEView.getData().data.Value = configValue;
                                } else {
                                    context.record.dirty = true;
                                    context.record.phantom = true;
                                    context.record.setDisplayValue(displayValue);
                                    context.record.Value = configValue;
                                }
                            },
                            onError: function (o) {
                                console.error(o);
                            }
                        });
                    }
                }
            }
        });
    }
});

Ext.define('SIE.control.GridComboPopup', {
	extend: 'Ext.form.field.Picker',
	alias: 'widget.gridcombopopup',
	triggerCls: "x-form-search-trigger",
	matchFieldWidth: false,
	editable: false,
    pageSize: 50,
    model: '',
    //separator:'',//分隔符
	windowHeight: 500,
	windowWidth: 800,
    _sourceViewSelectItems: [], //源视图已选项值
    _targetSelectItems: [],//操作弹窗视图选择的项
    _winNum: 0,
	initComponent: function() {
		var me = this;
		me.callParent();
	},

    border: false,
    //初始化
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    //触发器事件
	onTriggerClick: function (field, trigger, e) {
        var me = this;
        if(field.readOnly)
           return;
        if (me._winNum==0) 
        {
            me._winNum=1;
            me._sourceViewSelectItems=[];
            var entity=null;
            if(me.up("form"))
                entity=me.up("form").SIEView.getData();
            else
               entity=me.up("container").context.record;
            if (me.value != "" && me.value != null) {
                me.value.split(me.separator).forEach(function (item) {
                    if (item)
                        me._sourceViewSelectItems.push(item);
                });
            } 
            me._createLayout(field);
        }
    },

    /**
     * 创建界面布局
     * @param field 
     */
    _createLayout: function (field) {
        var me = this;
		if(!me.model)
			SIE.Msg.showWarning('请设置数据关联实体'.t());
		SIE.AutoUI.getMeta({
			model: me.model,
			ignoreChild: true,
			ignoreCommands: true,
			isReadonly: true,
			ignoreQuery: false,
			isAggt: true,
			callback: function(blocks) {
				me._queryBlockProcess(blocks);
				me._gridBlockProcess(blocks);
				var ui = SIE.AutoUI.generateAggtControl(blocks);
				me._popupWin(ui, me.inputEl);
                me._reloadTargetViewData();
				me._layouted = true;
			}
		});
    },
    /**
     * 查询块设置-只读为false
     * @param block 块配置
     */
    _queryBlockProcess: function(block) {
		    if (block.surrounders && block.surrounders.length) {
			    var surround = block.surrounders[0];
			    var items = surround.mainBlock.formConfig.items;
			    for (var i = 0; i < items.length; ++i) {
				    items[i].readOnly = false;
			    }
		    }
	    },
	    _doAutoSelectOnClose: function() {
		    var me = this;
		    if (me.selectOnClose && me._changeSelectionAfterShow) {
			    if (me._targetSelectItems.keys.length > 0) {
				    if (!me.isCanceling) {
					    me._onConfirm();
				    }
			    }
		    }

	    },
    /**
     * grid 处理
     * @param block 块配置
     */
    _gridBlockProcess: function(block) {
		    var me = this;
		    var multiSelect = me.multiSelect;
		    //var multiSelect='MULTI';
		    var gridConfig = block.gridConfig || block.mainBlock.gridConfig;
		    gridConfig.selModel = {
			    injectCheckbox: 0,
			    //checkbox位于哪一列，默认值为0
			    selType: 'checkboxmodel',
			    //checkbox
			    checkOnly: true,
			    //只能通过checkbox选择
			    mode: multiSelect //(multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
		    };
		    gridConfig.viewConfig = {
			    enableTextSelection: true,
			    //启用文本选中
			    getRowClass: function(record, index, rowParams, store) {
				    if (me.lastSelectionRecord && me.lastSelectionRecord.value&&me.multiSelect!="Multi") {
					    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
						    me.grid.getSelectionModel().select(record, true);
						    return 'gridRowLock';
					    }
				    }
			    }
		    };

		    gridConfig.pagingBarConfig = {
			    _displayInfoOnSimple: false,
			    afterPageText: '/&nbsp{0}页'.t(),
			    displayMsg: '共{2}条'.t(),
			    _pageSize: me.pageSize
		    };

	    },

     /**
	 * 弹出界面窗口
	 * @param ui
	 * @param source
	 */
	_popupWin: function(ui, source) {
		var me = this;
		me._targetView = ui._view;
		me._uiControl = ui.getControl();
		//弹窗
        me._win = SIE.Window.show({
            title: ('选择' + me._targetView.label).t(),
            animateTarget: source,
            items: ui.getControl(),
            modal: false,
            closeAction: 'hide',
            height: me.windowHeight || 500,
            width: me.windowWidth || 800,
            //buttons: ['确定', '关闭'], //自定义按钮名称
            callback: function(btn) {
                me.onpopupWinbtn(btn);
            }
        });
		me._setGridListeners();
		me._targetSelectItems = {
			items: [],
			keys: []
		};

		me._setWinListeners();
		me.grid = me._targetView.getControl();

		delete me._layouting;
	},

    /**
     * 弹窗视图数据加载
     */
    _reloadTargetViewData: function () {
        var me = this;
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) { 
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },

	onLoad: function (store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上 me.displayField
        /// </summary>
        var me = this;
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.data[me.displayField]) > -1) {
                        selModel.select(record, true, true); //勾选上.
                        if (Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0) === -1) {
                            me._targetSelectItems.keys.push(record.getId());
                            me._targetSelectItems.items.push(record);
                        }
                    }
                }
            }
        }
    },
	/**
	 * 确定事件
	 * @param btn--
	 * @returns
	 */
	onpopupWinbtn:function(btn){
		var me = this;
		if (btn === '确定'.t()) {
			me.setMULTIValue();
			me._win.hide();
			return true; //阻止窗口关闭，在save中根据返回结果处理
		} else if (btn === '取消'.t()) {
			me.isCanceling = true;
			return true;
		}
	},

	/**
	 * grid 绑定事件
	 */
	_setGridListeners: function() {
		var me = this;
		var grid = me._targetView.getControl();

		me.mon(grid.getSelectionModel(), {
			scope: me,
			select: me._onSelect,
			deselect: me._onDeselect,
		});
        //多选不注册双击事件
		if (me.enableDoubleClick&&this.multiSelect != "Multi") {
			me.mon(grid.getView(), {
				rowdblclick: function(vthis, record, element, rowIndex, e, eOpts) {
					me._onRowdblClick(vthis, record, element, rowIndex, e, eOpts);
				}
			});
		}
    },
    	/**
	 * 设置值201606
	 */
    setMULTIValue: function () {
        var me = this;
        var displayVal2 = "";
        //获取关联行的键值对
        //示例{"colspan1":"code","colspan2":"name","colspan3":"email"}
        var valueDisplay;
        if(me.mutiLinkField !=""){
            valueDisplay= JSON.parse(me.mutiLinkField);
        }
        
        //获取当前选择框的展示值
        me._targetSelectItems.items.forEach(function (model) {
            displayVal2 +=me.separator + model.data[me.displayField];
		});
        displayVal2 = displayVal2.substring(me.separator.length);
        //设置多项关联列的对应值
        var list = [];
        for (var key in valueDisplay) {
            var mutiLinkfieldAndValueField = {};
            mutiLinkfieldAndValueField.linkField = key;
            mutiLinkfieldAndValueField.valueField = "";
            me._targetSelectItems.items.forEach(function (model) {
                mutiLinkfieldAndValueField.valueField += me.separator + model.data[valueDisplay[key]]
            });
            list.push(mutiLinkfieldAndValueField);
        }
        var entity;
        if(!me.up("form"))
            entity=me.up("container").context.record;
        else
            entity = me.up("form").SIEView.getData();
        //控件需要用到双向绑定不能直接设置entity，是设置viewmodel
        var binder = me.getBind();
        if (binder) {
            var bindRec = me._getBindRecord();
            if (bindRec) {
                bindRec.set(me.getName(), displayVal2);
            }
        }
        //设置关联对应列的值
        for (var i = 0; i < list.length; i++) {
            entity.set(list[i].linkField, list[i].valueField.substring(me.separator.length));
        }
		me.setRawValue(displayVal2); 
		me.checkChange();
		if(!me.up("form"))
			me.up("container").context.view.refresh();
	},
	
    _getBindRecord: function () {
        var me = this;
        var binder = me.getBind();
        var ownerData = binder.value.owner.getData();
        var dataKey = Object.keys(ownerData);
        if (dataKey.length == 1) {
            //未绑定之前只是一个{}空对象(在从表,孙表)
            if (ownerData[dataKey] !== null && Ext.isFunction(ownerData[dataKey].set)) {
                return ownerData[dataKey];
            }
        } else {
            console.log('Bind内容.length应该只能等于1'.t());
            return null;
        }
    },

	_setWinListeners: function() {
		var me = this;
		me._win.on('focusleave',
		function(vthis, event, eOpts) {
			if (!event.toElement || me._win.owns(event.toElement) === false) {
				if (me._win.hasFocus === false) {
					me._win.hide();
                    me._winNum=0;
				}
			}
		},
		me._win, {
			delay: 50
		});
         //可能移光标到 主控件
		me.on('focusleave',
		function(vthis, event, eOpts) {
			if (!event) return;

			if (me._win && event.toElement && me._win.owns(event.toElement) === false) {
				//me._win.focus();
				me._win.hide();
                me._winNum=0;
			}
		},
		me, {
			delay: 50
		});
		me._win.on('hide',
		function() {
			//console.log('关闭中-A-' + me.isCanceling);
			me._doAutoSelectOnClose();
			delete me.isCanceling;
		});

		me._win.on('show',
		function() {
			if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
			if (selModel.store.data && selModel.store.data.length > 0) {
                for (var i = 0, len = selModel.store.data.length ; i < len; i++) {
                    var record = selModel.store.data.items[i];
                    if (me._sourceViewSelectItems.indexOf(record.data[me.displayField]) > -1) {
                        selModel.select(record, true, true); //勾选上.
						if(Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0)===-1){
							me._targetSelectItems.keys.push(record.getId());
							me._targetSelectItems.items.push(record);
						}
                    }
                }
            }
        }
		},
		me, {
			delay: 50
		});
	},

	/** 
	 * 复选框勾选事件
	 * @param selModel 选择模式
	 * @param record 选择的记录
	 * @param index 行索引号
	 * @param eOpts  The options object passed to Ext.util.Observable.addListener.
	 */
	_onSelect: function(selModel, record, index, eOpts) {
		var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getId(), 0);
		if (idx === -1) {
			this._targetSelectItems.keys.push(record.getId());
			this._targetSelectItems.items.push(record);
			this._changeSelectionAfterShow = true;
		}
	},

	/**
	 * 复选框取消勾选事件
	 * @param selModel 选择模式
	 * @param record 选择的记录
	 * @param index 行索引号
	 * @param eOpts The options object passed to Ext.util.Observable.addListener.
	 */
	_onDeselect: function (selModel, record, index, eOpts) {
		if (record) {
			var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getId(), 0);
			if (idx > -1) {
				//var item = this._targetSelectItems.items[idx];
				Ext.Array.removeAt(this._targetSelectItems.keys, idx);
				Ext.Array.removeAt(this._targetSelectItems.items, idx);
				this._changeSelectionAfterShow = true;
			}
		}
	},

	/**
	 * Grid行双击事件
	 * @param vthis 
	 * @param record
	 * @param element
	 * @param rowIndex 行索引
	 * @param e
	 * @param eOpts
	 */
	_onRowdblClick: function(vthis, record, element, rowIndex, e, eOpts ){
		var me = this;
		if (record) {
			me.setValue(record);
			me._win.hide();
		}
    }
});

Ext.define('SIE.control.DateFieldPlus', {
    extend: 'Ext.form.field.Date',
    alias: 'widget.dateFieldPlus',
    initComponent: function () {
        this.callParent();
    },
    onSelect: function (m, d) {
        var me = this;
        var v = Ext.Date.parse(Ext.util.Format.date(d, "Y-m-d" + Ext.util.Format.date(me.value || new Date(), " H:i:s")), 'Y-m-d H:i:s');
        me.setValue(v);
        me.rawDate = v;
        me.fireEvent('select', me, v);

        // Focus the inputEl first and then collapse. We configure 
        // the picker not to revert focus which is a normal thing to do 
        // for floaters; in our case when the picker is focusable it will 
        // lead to unexpected results on Tab key presses. 
        // Note that this focusing might happen synchronously during Tab 
        // key handling in the picker, which is the way we want it. 
        me.onTabOut(m);
    },

});
/**
 * 重写复选框
 */
Ext.define('SIE.grid.column.CheckBox', {
    extend: 'Ext.grid.column.Check',
    alias: 'widget.checkboxcolumn',
    listeners: {
        beforecheckchange: 'onBeforeCheckChange',
    },
    onBeforeCheckChange: function (me, rowIndex, checked, record, e, eOpts) {
        var isreadonly;
        if (this.getEditor)
            isreadonly = this.getEditor().readOnly;
        else
            isreadonly = this.editor.readOnly;
        if (isreadonly && !this.readonlyLambda) {//只设置为只读但没有表达式，直接返回false
            return false;
        }
        var canEdit = true;
        var handlers = null;
        var view = null;
        if (me.up().grid) {//只会用于grid
            view = (me.up().grid.SIEView ? me.up().grid.SIEView : me.up().grid.up().SIEView);/*当grid锁定列时，需在取上一层*/
            handlers = view.getProChgHandlers();
            handlers.forEach(function (handler, i, arr) {
                if (handler.effect == 'setReadOnly' && me.dataIndex == handler.pro) {
                    var isReadonly = handler.lambda(record);
                    if (isReadonly || view.getIsReadonly()) {
                        canEdit = false;
                    }
                    //todo:未知为何加上 x-item-disabled 样式,但加上会影响列表复选框只读表达式判断
                    //var extCell = Ext.get(e.target);
                    //if (extCell.query('span.x-grid-checkcolumn').length > 0) {
                    //    if (!canEdit) {
                    //        extCell.addCls('x-item-disabled');
                    //    } else {
                    //        extCell.removeCls('x-item-disabled');
                    //    }
                    //}
                }
            });
        }
        return canEdit;
    },
});
Ext.define('SIE.control.AceEditor', {
    extend: 'Ext.Component',
    alias: 'widget.AceEditor',
    config: {
        editor: null,
    },
    afterRender: function (t, eOpts) {
        this.callParent(arguments);

        if (!window.ace) {
            this.update('No ace library loaded');
        }
        else {
            var editor = ace.edit(this.getId());
            editor.setTheme('ace/theme/xcode');
            var codeMode = 'ace/mode/javascript';
            if (this.config.codeMode)
                codeMode = this.config.codeMode;
            let jsMode = ace.require(codeMode).Mode;
            editor.session.setMode(new jsMode());

            this.setEditor(editor);
        }
    },
    onResize: function (w, h, oW, oH) {
        this.callParent(arguments);
        var editor = this.getEditor();
        if (editor) {
            editor.resize();
        }
    }
});
Ext.define('SIE.control.AceCodeField', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.AceCodeField',
    triggerCls: 'ux-form-edit-trigger',
    /* 
    * 此编辑对 View.Property(p => p.XXX).UseTextButtonPlayEditor(...) 开放配置，
    * 注意点：添加的样式要有Ext样式规范,符合触发样式，要不划过会不见 x-form-edit-trigger
    */
    border: false,
    //初始化
    initComponent: function () {
        var me = this;
        me.callParent();
    },
    //触发器事件
    onTriggerClick: function (field, trigger, e) {
        var me = this;
        me._createUI(field);
    },
    getRawValue: function () {
        var me = this,
            v = Ext.valueFrom(me.rawValue, '');
        me.rawValue = v;
        return v;
    },
    _createUI: function (field) {
        var me = this;
        var codeEditor = Ext.create("SIE.control.CodeEditorPanel", {
            xtype: 'CodeEditorPanel',
            height: 400,
            runButtonJs: me.runButtonJs,
            codeMode: me.codeMode
        });
      
        me._showWindow(codeEditor, field);
    },

    /* 
     * 打开显示值配置页面，
     * ui：视图 detailView：详细页面 from：表单 
     */
    _showWindow: function (ui, field) {
        var me = this;
        var win = SIE.Window.show({
            title: '配置'.t(),
            width: 600,
            height: 400,
            items: ui,
            callback: function (btn) {
                if (btn == "确定".t()) {
                    me.setValue(ui.getCodeValue());
                    if (me.column) {
                        me.ownerCt.context.record.set(me.column.dataIndex, ui.getCodeValue());
                    }
                }
            }
        });
        var codeValue = me.value;
        if (me.column)
            codeValue = me.ownerCt.context.record.get(me.column.dataIndex);

        ui.setCodeValue(codeValue);
    }
});
Ext.define('SIE.control.CodeEditorPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.CodeEditorPanel',
    border: true,
    config: {
        editorConfig: {},
        codeMode:null
    },
    initComponent: function () {
        this.callParent();
        this.add({
            xtype: 'AceEditor',
            height: '100%',
            codeMode: this.config.codeMode,
        });
       
    },
    tbar: [
        {
            xtype: 'button',
            text: '运行'.t(),
            handler: function () {
                var me = this;
                var codevalue = this.ownerCt.ownerCt.getCodeValue();
                var panelConfig = me.up("CodeEditorPanel").config;
                if (panelConfig.runButtonJs) {
                    var runCommonjs = Ext.create(panelConfig.runButtonJs);
                    if (runCommonjs.Run)
                        runCommonjs.Run(codevalue);
                }
            }
        }
    ],
    //items: {
    //    xtype: 'AceEditor',
    //    height: '100%',
       
    //},
    getAceEditor: function () {
        var me = this;
        var editor = this.down("AceEditor").getEditor();
        return editor;
    },
    setCodeValue: function (v) {
        var editor = this.getAceEditor();
        editor.setValue(v);
    },
    getCodeValue: function () {
        var editor = this.getAceEditor();
        return editor.getValue();
    }

});

Ext.define('Portal.ComponentBase', {
    extend: 'Ext.panel.Panel',
    xtype: 'widget.ComponentBase',
    config: {
        inputParams: [],//输入参数集合
        outputParams: [],//输出参数集合
        bindOutPutParam: new Map(),//绑定输出参数绑定输入参数键值对
        moduleName: '',
        refreshInterval: 60,
        isAutoRefresh: false,
    },
    border: false,
    bodyBorder: false,
    padding:1,
    globalOutputParams: [],
    //dockedItems: [{
    //    xtype: 'toolbar',
    //    hidden: true,
    //    border:false,
    //    dock: 'top',
    //    items: [
    //        '->', {//刷新命令
    //            xtype: 'button',
    //            iconCls: 'iconfont icon-Refresh icon-blue',
    //            handler: function (btn, event) {
    //                // refresh logic
    //                if (btn.ownerCt.ownerCt.renderData.refreshData)
    //                    btn.ownerCt.ownerCt.renderData.refreshData();
    //            }
    //        }, {//刷新放大
    //            xtype: 'button',
    //            iconCls: 'iconfont icon-ArrowExpandAll icon-blue',
    //            handler: function (btn, event) {

    //                var dashboardPanel = btn.up("dashboard-panel");
    //                // max logic
    //                var win = Ext.create('Ext.window.Window', {
    //                    id: 'portal_window',
    //                    border: false,
    //                    modal: true,
    //                    draggable: false, // 禁止移动
    //                    resizable: false,
    //                    maximizable: false, // 禁止最大化
    //                    title: dashboardPanel.title,
    //                    height: 200,
    //                    width: 400,
    //                    layout: 'fit',
    //                    items: {
    //                        xtype: btn.ownerCt.ownerCt.xtype,
    //                        dockedItems:[]
    //                    }
    //                });

    //                win.show();

    //                win.setPosition(0, 0);
    //                win.fitContainer(); // 填充满浏览器
    //            }
    //        }
    //    ]
    //}],
    initComponent: function () {
        var me = this;
        me.initParam();
        if (me.getModuleName() != '') {
            if (me.getRefOwner() && me.getRefOwner().setTitle)
                me.getRefOwner().setTitle(me.getModuleName());
        }

        if (me.getIsAutoRefresh())
            window.setInterval(function () { me.refreshData(me) }, me.getRefreshInterval()*1000);


        Ext.Array.each(me.inputParams, function (paramName) {

            var setFun = "set" + paramName
            me[paramName] = null;
            me[setFun] = function (value) {
                var me = this,
                    oldVale = me[paramName];
                this.setHeaderConfig(value, paramName, setFun, false);
            }
        });

        window.EB.on("outputPropertyChanged", function (data) {

            for (var key in data) {
                if (me.bindOutPutParam.get(key)) {
                    var fun = "set" + me.bindOutPutParam.get(key);
                    if (me[fun]) {
                        me[fun](data[key])
                    }
                }
            }

            

        });
        this.callParent();
    },
    /**
    * @zoomComponent
    * 初始化输入输出参数
    */
    initParam: function () { },
    /**
    * @zoomComponent
    * 刷新数据
     * @compt {object} compt 当前组件.
    */
    refreshData: function (compt) { },
    /**
    * @zoomComponent
    * 放大组件
    */
    zoomComponent: function () {
        var me = this;

        var itemConfig = Ext.clone(me.config);
        // max logic
        var win = Ext.create('Ext.window.Window', {
            id: 'portal_window',
            border: false,
            modal: true,
            draggable: false, // 禁止移动
            resizable: false,
            maximizable: false, // 禁止最大化
            title: me.title,
            height: 200,
            width: 400,
            layout: 'fit',
            items: itemConfig
        });

        win.show();

        win.setPosition(0, 0);
        win.fitContainer(); // 填充满浏览器
    },
    //afterRender: function () {
       
    //    this.callParent();
    //    var me = this;

    //    //鼠标接近显视tbar
    //    me.el.on("mouseover", function () {

    //        if (me.getDockedItems("toolbar").length>0)
    //            me.getDockedItems("toolbar")[0].show();
    //    });
    //     //鼠标离弄隐藏tbar
    //    me.el.on("mouseout", function () {
    //        if (me.getDockedItems("toolbar").length > 0)
    //            me.getDockedItems("toolbar")[0].hide();
    //    });
      
    //},
    listeners: {
        //itemclick: function (view, record, item, index, e, eOpts) {

        //},
        //inputselectvaluechange: function (view, value, oldValue) {
        //    alert(value.data.name);
        //}
    },
    /**
      * @addInputParam
      * 添加输入参数
      * @param {Array/string} param The array to iterate.
      */
    addInputParam: function (param) {
        var me = this;
        var inputParams = me.getInputParams();
        if (typeof (param) === 'string') {

            if (!Ext.Array.contains(inputParams, param))
                inputParams.push(param);
        }
        else if (Array.isArray(param)) {
            Ext.Array.each(param, function (paramName) {
                if (typeof (paramName) === 'string') {
                    if (!Ext.Array.contains(inputParams, paramName))
                        inputParams.push(paramName);
                }
                  
            });
        }
    },
    /**
      * @addOutputParam
      * 添加输出参数
      * @param {Array/string} param The array to iterate.
      */
    addOutputParam: function (param) {
        var me = this;
        var outputParams = me.getOutputParams();
        var g_outputParams = me.globalOutputParams;
        if (typeof (param) === 'string') {
            if (!Ext.Array.contains(outputParams, param))
                outputParams.push(param);
            if (!Ext.Array.contains(g_outputParams, param))
                g_outputParams.push(param);
        }
        else if (Array.isArray(param)) {
            Ext.Array.each(param, function (paramName) {
                if (typeof (param) === 'string') {
                    if (!Ext.Array.contains(outputParams, paramName))
                        outputParams.push(paramName);
                    if (!Ext.Array.contains(g_outputParams, paramName))
                        g_outputParams.push(paramName);
                }
                  
            });
        }
    }


});
Ext.define('SIE.view.grid.ColumnController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.column',
    comboDispaly: "_Display", //为了兼容自定义的 SIE.control.ComboPopup
    renderComboData: function (value, metaData, record, rowIndex, colIndex, store, view) {
        var columns = view.grid.getColumns(),
           column, editor, editorStore, editorFind, valueField, displayField, displayText, findRecord;
        var column = columns[colIndex];
        editor = column.getEditor(record);
        if (editor && (editor.xtype === 'combolist' || editor.xtype === 'pagingLookUp')) {
            displayField = editor.displayField;
            valueField = editor.valueField;
            if (editor.getStore) { //标准Store
                editorStore = editor.getStore();
                if (editorStore.getCount() < 1) {
                    editorStore.load();
                }
                editorFind = editorStore.findExact(valueField, value);
                if (editorFind && editorFind != -1) {
                    findRecord = editorStore.getAt(editorIndex);
                    if (findRecord) {
                        displayText = findRecord.get(displayField); //使用配置的显示属性值

                    }
                }
            }
            else {
                //兼容写法
                var items = editor.store.data;
                if (SIE.isString(items)) {
                    items = Ext.decode(editor.store.data);
                }
                var editorFind = this.findByKey(items, valueField, value);
                if (editorFind) {
                    displayText = editorFind[displayField];
                }
            }
        }

        if (SIE.isEmpty(displayText)) {
            displayText = value;
        }
        //兼容SIE.control.ComboPopup处理
        record.data[column.field.name + this.comboDispaly] = displayText;
        return displayText;
    },
    /**
     * 查找数组元素
     * @param {type} array-要操作的数组
     * @param {type} key-对象属性主键
     * @param {type} value-对象值
     * @param {Object} 一个对象
     */
    findByKey: function (array, key, value) {
        var find = SIE.findFirst(array, function (i) { return i[key] == value; });
        if (find) {
            return find;
        }
        return null;
    },
});

/*
* 实现列配置了编辑器的显示
* */
Ext.define('SIE.ux.grid.ComboColumn', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.comboColumn'],
    requires: ['SIE.view.grid.ColumnController'],
    isComboColumn: true,
    controller: 'column',
    /**
    * 渲染器是一种“拦截器”方法，可用于在渲染之前转换数据（值，外观等）
    */
    renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
        var controller = this.getColumns()[colIndex].getController();
        if (controller) {
            return controller.renderComboData.apply(controller, arguments);
        }
        else {
            return value;
        }
    },

});
/*
* 实现列配置了编辑器的显示
* */
Ext.define('SIE.ux.grid.column.Html', {
    extend: 'Ext.grid.column.Column',
    alternateClassName: ['Ext.ux.HtmlColumn', 'Ext.grid.column.HtmlColumn'],
    alias: ['widget.htmlcolumn'],
    isHtmlColumn: true,
    defaultRenderer: function (value) {
        if (value && value.toString().match(SIE.htmlTagsPatt)) {
            var text = Ext.htmlEncode(value);
            return text;
        }
        return value;
    },
});
/*
* 实现单元格不允许用快捷键复制新增
* */
Ext.define('SIE.ux.grid.clipboardCellData', function () {
    /*
     * 私有方法
     */
    var _putCellData = function (data, format, force, me) //增加force
    {
        var gridAllowEdit = me.pluginConfig.cmp.SIEView.gridConfig.allowEdit; //"当前grid是否可允许编辑,如果允许,则调用基类方法"
        var gridEditModel = me.pluginConfig.cmp.SIEView.gridConfig.editmode;
        if (gridAllowEdit || force) {
            if (gridEditModel != "Inline") {
                me.putCellData(data, format);
            }
        }
    }

    return {
        extend: 'Ext.grid.plugin.Clipboard',
        alias: ['plugin.clipboardCellData'],
        putTextData: function (data, format, force) {
            var me = this;
            _putCellData(data, format, force, me);
        },
        copy: function () {
            var me = this;
            me.onCopy();
            document.execCommand("Copy");
        },

        /**
         * grid 剪贴板去掉剪切,保留复制
         * @param {any} format
         * @param {any} erase 
         */
        getTextData: function (format, erase) {
            return this.getCellData(format, false);
        },
    }
});
//支持回车搜索的文本框
Ext.define('SIE.ux.form.SearchTextField', {
    extend: 'Ext.form.field.Text',
    alias: 'widget.searchtextfield',
    inputType: 'search',
    initComponent: function () {
        this.callParent();
        this.mon(this, 'specialkey', this.onTextfieldSpecialKey, this);
    },
    onTextfieldSpecialKey: function (field, e, eOpts) {
        var value = this.getValue();
        if (e.getKey() === e.ENTER) {
            field.setValue(eOpts.value);
            var sieView = field.up('form').SIEView;
            if (sieView) {
               field.up().SIEView._commands.items.forEach(function(cmd){
                    if(cmd instanceof SIE.cmd.ExecuteQuery||(cmd.superclass && cmd.superclass.$className == "SIE.cmd.ExecuteQuery"))
                        cmd.execute(sieView);
                });
            }
        }
    }
});
/**
 * 含第三种状态和禁用状态的选择树控件
 * @class Ext.ux.grid.TriStateTree
 * @extends Ext.tree.Panel
 */
Ext.define('Ext.ux.grid.TriStateTree', {
    extend: 'Ext.tree.Panel',
    xtype: 'triStateTree',
    rootVisible: true,
    useArrows: true,
    frame: false,
    header: false,
    disabledCls: 'x-tree-checkbox-checked-disabled',
    triStateCls: 'x-tree-checkbox-triState',

    updateNode: function (node) {
        var me = this;
        //更新子节点
        var updateChild = function (pnode) {
            pnode.childNodes.forEach(function (cnode) {
                updateChild(cnode);
            });
            if (!pnode.isLeaf()) {
                me.unsetThirdState(pnode);
                var checkedCount = 0;
                for (var i = 0; i < pnode.childNodes.length; i++) {
                    if (me.isThirdState(pnode.childNodes[i])) {
                        me.setThirdState(pnode);
                        return;
                    } else if (pnode.childNodes[i].data.checked) {
                        checkedCount++;
                    }
                }
                if (checkedCount > 0 && checkedCount < pnode.childNodes.length) {
                    me.setThirdState(pnode);
                } else if (checkedCount > 0 && checkedCount == pnode.childNodes.length) {
                    pnode.set('checked', true);
                } else if (checkedCount == 0 && pnode.childNodes.length > 0) {
                    pnode.set('checked', false);
                }
            };

        };
        updateChild(node);
        //更新父节点
        var updateParent = function (parent) {
            if (parent) {
                me.unsetThirdState(parent);
                var checkedCount = 0;
                for (var i = 0; i < parent.childNodes.length; i++) {
                    if (me.isThirdState(parent.childNodes[i])) {
                        me.setThirdState(parent);
                        updateParent(parent.parentNode);
                        return;
                    } else if (parent.childNodes[i].data.checked) {
                        checkedCount++;
                    }
                }
                if (checkedCount > 0 && checkedCount < parent.childNodes.length) {
                    me.setThirdState(parent);
                } else if (checkedCount > 0 && checkedCount == parent.childNodes.length) {
                    parent.set('checked', true);
                } else if (checkedCount == 0 && parent.childNodes.length > 0) {
                    parent.set('checked', false);
                }
                updateParent(parent.parentNode);
            }
        }
        updateParent(node.parentNode);
    },
    hasCheckChanged: false,
    listeners: {
        afterrender: function (cmp, eOpts) {
            this.updateNode(cmp.getRootNode());
        },
        itemclick: function (view, node, item, index, e, eOpts) {
            var me = this;
            //更新子节点
            if (me.hasCheckChanged) {
                this.updateNode(node);
            }
            me.hasCheckChanged = false;
        },
        checkchange: function (node, check) {
            var me = this;
            me.hasCheckChanged = true;
        }
    },

    //是否已禁用
    isDisabled: function (node) {
        if (node === undefined) return false;
        var me = this;
        var clsList = node.get('cls').split(" ");
        return clsList.indexOf(me.disabledCls) > -1;
        //return node.get('cls').indexOf(this.disabledCls) > -1;
    },

    //设置禁用
    setDisabled: function (node) {
        var clsList = node.get('cls').split(" ");
        if (clsList.indexOf(this.disabledCls) == -1) {
            clsList.push(this.disabledCls);
        }
        node.set('cls', clsList.join(' '));
    },

    //取消禁用
    unsetDisabled: function (node) {
        var clsList = node.get('cls').split(" ");
        clsList.remove(this.disabledCls);
        node.set('cls', clsList.join(' '));
    },
    //是否checked
    isChecked: function (node) {
        if (node === undefined) return false;
        return node.get('checked') === true;
    },

    //清除
    clearThirdState: function () {
        var me = this;
        me.getRootNode().cascadeBy(function () {
            if (me.isThirdState(this)) {
                me.unsetThirdState(this);
            }
        });
    },

    //是否第三种状态
    isThirdState: function (node) {
        if (node === undefined) return false;
        var me = this;
        var clsList = node.get('cls').split(" ");
        return clsList.indexOf(me.triStateCls) > -1;
    },

    //设为第三种状态
    setThirdState: function (node) {
        var clsList = node.get('cls').split(" ");
        if (clsList.indexOf(this.triStateCls) == -1) {
            clsList.push(this.triStateCls);
        }
        node.set('cls', clsList.join(' '));
        node.set('checked', true);
    },

    unsetThirdState: function (node) {
        var clsList = node.get('cls').split(" ");
        clsList.remove(this.triStateCls);
        node.set('cls', clsList.join(' '));
    }
});
//判断是否为整数
window.isInteger = window.isInteger ||
    function (obj) {
        return typeof obj === 'number' && obj % 1 === 0;
    };

String.prototype.trim = String.prototype.trim ||
    function (raw) {
        return raw.replace(/^\s+|\s+$/gm, '');
    };


function toQueryPair(key, value) {
    return encodeURIComponent(String(key)) + "=" + encodeURIComponent(String(value));
};

function toQueryString(obj) {
    var result = [];
    for (var key in obj) {
        if (obj.hasOwnProperty(key)) {
            result.push(toQueryPair(key, obj[key]));
        }
    }
    return result.join("&");
};


Array.prototype.groupBy = function (f) {
    var groups = {};
    this.forEach(function (o) {
        var group = JSON.stringify(f(o));
        groups[group] = groups[group] || [];
        groups[group].push(o);
    });
    return Object.keys(groups).map(function (group) {
        return groups[group];
    });
}

var CssHelper = (function () {
    function removeStyleSheet(stylesheet) {
        var styleEl = (typeof stylesheet === 'string') ? document.getElementById(stylesheet) : stylesheet.ownerNode;
        if (styleEl) {
            styleEl.parentNode.removeChild(styleEl);
        }
    }
    /**
     * Dynamically swaps an existing stylesheet reference for a new one
     * @param {String} id The id of an existing link tag to remove
     * @param {String} url The href of the new stylesheet to include
     */
    function swapStyleSheet(id, url) {
        var ss;
        removeStyleSheet(id);
        ss = document.createElement("link");
        ss.setAttribute("rel", "stylesheet");
        ss.setAttribute("type", "text/css");
        ss.setAttribute("id", id);
        ss.setAttribute("href", url);
        document.getElementsByTagName("head")[0].appendChild(ss);
        return ss;
    }

    return {
        removeStyleSheet: removeStyleSheet,
        swapStyleSheet: swapStyleSheet
    }
})();

/**
 * 封装日期助手
 */
var DateTimeHelper = (function () {
    /**
     * 获取上午/下午
     */
    function getCnNoon() {
        var now = new Date();
        var hours = now.getHours();
        var timeValue = "" + ((hours >= 12) ? "下午 ".t() : "上午 ".t());
        return timeValue;
    }

    /**
     * 获取时+分
     */
    function getHourAndMinute() {
        var now = new Date();
        var hours = now.getHours();
        var minutes = now.getMinutes();
        var timeValue = ""; // + ((hours >= 12) ? "下午 " : "上午 ");
        timeValue += ((hours > 12) ? hours - 12 : hours);
        timeValue += ((minutes < 10) ? ":0" : ":") + minutes;
        return timeValue;
    }

    /**
     *获取周几
     */
    function getWeekDay() {
        var now = new Date();
        var curDay = now.getDay(); //获取存储当前日期
        var weekday = ["周日".t(), "周一".t(), "周二".t(), "周三".t(), "周四".t(), "周五".t(), "周六".t()];
        return weekday[curDay];
    }

    /**
     *获取中文年份
     */
    function getCnYear() {
        var now = new Date();
        var year = now.getFullYear();
        return year + "年".t();
    }

    /**
     *获取中文月份
     */
    function getCnMonth() {
        var now = new Date();
        var month = now.getMonth() + 1;
        return month + "月".t();
    }

    /**
     *获取中文日
     */
    function getCnDay() {
        var now = new Date();
        var day = now.getDate();
        return day + "日".t();
    }

    return {
        getCnNoon: getCnNoon,
        getHourAndMinute: getHourAndMinute,
        getWeekDay: getWeekDay,
        getCnYear: getCnYear,
        getCnMonth: getCnMonth,
        getCnDay: getCnDay
    }
})();

/**
 * 封装Cookie帮助
 */
var CookieHelper = (function () {
    /**
     * 获取Cookie
     * @param {any} name
     */
    function getCookie(name) {
        if (document.cookie.length > 0) {
            var cStart = document.cookie.indexOf(name + "=");
            if (cStart !== -1) {
                cStart = cStart + name.length + 1;
                var cEnd = document.cookie.indexOf(";", cStart);
                if (cEnd === -1) cEnd = document.cookie.length;
                return unescape(document.cookie.substring(cStart, cEnd));
            }
        }
        return "";
    }

    /**
     * 删除cookie
     * @param {string} name 
     * @returns {} 
     */
    function delCookie(name) {
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = this.getCookie(name);
        if (cval != null)
            document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    }

    /**
     * 设置cookie
     * @param {any} name
     * @param {any} value
     * @param {any} expiredays
     */
    function setCookie(name, value, expiredays) {
        this.delCookie(name);
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + expiredays);
        document.cookie = name +
            "=" +
            escape(value) +
            ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString());
    }

    return {
        getCookie: getCookie,
        delCookie: delCookie,
        setCookie: setCookie
    }
})();

/**
 * 封装LocalStorage帮助类
 */
var LocalStorageHelper = (function () {
    function setJsonData(key, content) {
        window.localStorage.setItem(key, JSON.stringify(content));
    }

    function getJsonData(key) {
        var data = window.localStorage.getItem(key);
        try {
            if (data) {
                return JSON.parse(window.localStorage.getItem(key));
            } else {
                return "";
            }
        } catch (e) {
            console.warn(e.message);
            return "";
        }
    }

    function removeItem(key) {
        return window.localStorage.removeItem(key);
    }

    return {
        setJsonData: setJsonData,
        getJsonData: getJsonData,
        removeItem: removeItem
    };
})();
var SessionStorageHelper = (function () {
    function setString(key, content) {
        window.sessionStorage.setItem(key, content);
    }
    function setObject(key, content) {
        this.setString(key, JSON.stringify(content));
    }
    function getString(key) {
        return window.sessionStorage.getItem(key) || '';
    }
    function getObject(key) {
        var data = this.getString(key);
        try {
            if (data) {
                return JSON.parse(data);
            } else {
                return "";
            }
        } catch (e) {
            console.warn(e.message);
            return "";
        }
    }

    function removeItem(key) {
        return window.sessionStorage.removeItem(key);
    }

    return {
        setString: setString,
        getString: getString,
        setObject: setObject,
        getObject: getObject,
        removeItem: removeItem
    };
})();
var CurUserStateHelper = (function () {
    var identifyKey = identify.innerText;

    function setSessionUser(data) {
        SessionStorageHelper.setObject(identifyKey, data);
    }

    function getSessionUser() {
        //规则:先session,再cookie
        var data = SessionStorageHelper.getObject(identifyKey);
        if ('' === data) {
            try {
                data = this.getCookieUser();
                if (data) {
                    this.setSessionUser(data);
                }
            } catch (e) {
                console.warn(e.message);
                data = '';
            }
        }
        return data;
    }

    function getCookieUser() {
        var data = '';
        try {
            var val = CookieHelper.getCookie(identifyKey);
            if (val) {
                val = BASE64.decode(val);
                if (val) {
                    data = JSON.parse(val);
                }
            }
        } catch (e) {
            console.warn(e.message);
        }
        return data;
    }

    function getIdentifyKey() {
        return identifyKey;
    }

    return {
        setSessionUser: setSessionUser,
        getSessionUser: getSessionUser,
        getCookieUser: getCookieUser,
        getIdentifyKey: getIdentifyKey,
    };
})();
/**
 * 封装Ajax
 */
var Ajax = (function () {
    return function (uri, option) {
        var httpRequest,
            httpSuccess,
            timeout,
            isTimeout = false,
            isComplete = false;

        option = {
            method: (option.method || "GET").toUpperCase(),
            data: option.data || null,
            arguments: option.arguments || null,

            onSuccess: option.onSuccess || function () { },
            onError: option.onError || function () { },
            onComplete: option.onComplete || function () { },
            //尚未测试
            onTimeout: option.onTimeout || function () { },

            isAsync: typeof (option.isAsync) === 'undefined' ? true : option.isAsync,
            timeout: option.timeout || 30000,
            contentType: option.contentType,
            type: option.type || "xml"
        };
        if (option.data && typeof option.data === "object") {
            option.data = toQueryString(option.data);
        }

        uri = uri || "";
        timeout = option.timeout;

        httpRequest = new window.XMLHttpRequest();

        /**
         * @ignore
         */
        httpSuccess = function (r) {
            try {
                return (!r.status && location.protocol == "file:") ||
                    (r.status >= 200 && r.status < 300) ||
                    (r.status == 304) ||
                    (navigator.userAgent.indexOf("Safari") > -1 && typeof r.status == "undefined");
            } catch (e) {
                console.error("错误：[".t() + e.name + "] " + e.message + ", " + e.fileName + ", 行号:".t() + e.lineNumber + "; stack:" + typeof e.stack, 2);
            }
            return false;
        }

        /**
         * @ignore
         */
        httpRequest.onreadystatechange = function () {
            if (httpRequest.readyState == 4) {
                if (!isTimeout) {
                    var o = {};
                    o.responseText = httpRequest.responseText;
                    o.responseXML = httpRequest.responseXML;
                    o.data = option.data;
                    o.status = httpRequest.status;
                    o.uri = uri;
                    o.arguments = option.arguments;
                    if (option.type === 'json') {
                        try {
                            o.responseJSON = JSON.parse(httpRequest.responseText);
                        } catch (e) { }
                    }
                    if (httpSuccess(httpRequest)) {
                        option.onSuccess(o);
                    } else {
                        option.onError(o);
                    }
                    option.onComplete(o);
                }
                isComplete = true;
                //删除对象,防止内存溢出
                httpRequest = null;
            }
        };

        if (option.method === "GET") {
            if (option.data) {
                uri += (uri.indexOf("?") > -1 ? "&" : "?") + option.data;
                option.data = null;
            }
            httpRequest.open("GET", uri, option.isAsync);
            httpRequest.setRequestHeader("Content-Type", option.contentType || "text/plain;charset=UTF-8");
            httpRequest.send();
        } else if (option.method === "POST") {
            httpRequest.open("POST", uri, option.isAsync);
            httpRequest.setRequestHeader("Content-Type", option.contentType || "application/x-www-form-urlencoded;charset=UTF-8");
            httpRequest.send(option.data);
        } else {
            httpRequest.open(option.method, uri, option.isAsync);
            httpRequest.send();
        }

        window.setTimeout(function () {
            var o;
            if (!isComplete) {
                isTimeout = true;
                o = {};
                o.uri = uri;
                o.arguments = option.arguments;
                option.onTimeout(o);
                option.onComplete(o);
            }
        }, timeout);

        return httpRequest;
    };

})();

/**
 * 封装加载器
 */
var Loader = (function () {
    /**
     * @param {int} maxWaitTime 最长等待时间 
     * @returns {} 
     */
    function show(maxWaitTime) {
        var div = document.getElementById('custom-loader') ||
            (function () {
                var div = document.createElement('div');
                div.id = 'custom-loader';
                var style = document.createElement('style');
                style.id = 'loader-style';
                style.innerHTML =
                    '.loading{position:fixed;z-index:999;height:2em;width:2em;overflow:show;margin:auto;top:0;left:0;bottom:0;right:0}.loading:before{content:"";display:block;position:fixed;top:0;left:0;width:100%;height:100%;background-color:rgba(0,0,0,.4)}.loading:not(:required){font:0/0 a;color:transparent;text-shadow:none;background-color:transparent;border:0}.loading:not(:required):after{content:"";display:block;font-size:10px;width:1em;height:1em;margin-top:-.5em;-webkit-animation:spinner 1.5s infinite linear;-moz-animation:spinner 1.5s infinite linear;-ms-animation:spinner 1.5s infinite linear;-o-animation:spinner 1.5s infinite linear;animation:spinner 1.5s infinite linear;border-radius:.5em;-webkit-box-shadow:rgba(255,255,255,.8) 1.5em 0 0 0,rgba(255,255,255,.8) 1.1em 1.1em 0 0,rgba(255,255,255,.8) 0 1.5em 0 0,rgba(255,255,255,.8) -1.1em 1.1em 0 0,rgba(0,0,0,.5) -1.5em 0 0 0,rgba(0,0,0,.5) -1.1em -1.1em 0 0,rgba(255,255,255,.8) 0 -1.5em 0 0,rgba(255,255,255,.8) 1.1em -1.1em 0 0;box-shadow:rgba(255,255,255,.8) 1.5em 0 0 0,rgba(255,255,255,.8) 1.1em 1.1em 0 0,rgba(255,255,255,.8) 0 1.5em 0 0,rgba(255,255,255,.8) -1.1em 1.1em 0 0,rgba(255,255,255,.8) -1.5em 0 0 0,rgba(255,255,255,.8) -1.1em -1.1em 0 0,rgba(255,255,255,.8) 0 -1.5em 0 0,rgba(255,255,255,.8) 1.1em -1.1em 0 0}@-webkit-keyframes spinner{0%{-webkit-transform:rotate(0);-moz-transform:rotate(0);-ms-transform:rotate(0);-o-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(360deg);-moz-transform:rotate(360deg);-ms-transform:rotate(360deg);-o-transform:rotate(360deg);transform:rotate(360deg)}}@-moz-keyframes spinner{0%{-webkit-transform:rotate(0);-moz-transform:rotate(0);-ms-transform:rotate(0);-o-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(360deg);-moz-transform:rotate(360deg);-ms-transform:rotate(360deg);-o-transform:rotate(360deg);transform:rotate(360deg)}}@-o-keyframes spinner{0%{-webkit-transform:rotate(0);-moz-transform:rotate(0);-ms-transform:rotate(0);-o-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(360deg);-moz-transform:rotate(360deg);-ms-transform:rotate(360deg);-o-transform:rotate(360deg);transform:rotate(360deg)}}@keyframes spinner{0%{-webkit-transform:rotate(0);-moz-transform:rotate(0);-ms-transform:rotate(0);-o-transform:rotate(0);transform:rotate(0)}100%{-webkit-transform:rotate(360deg);-moz-transform:rotate(360deg);-ms-transform:rotate(360deg);-o-transform:rotate(360deg);transform:rotate(360deg)}}';
                var loaderSpin = document.createElement('div');
                loaderSpin.className = 'loading';
                loaderSpin.innerHTML = 'Loading&#8230;';
                div.appendChild(style);
                div.appendChild(loaderSpin);
                div = document.body.appendChild(div);
                return div;
            })();
        div.style.display = 'block';
        if (isInteger(maxWaitTime)) {
            setTimeout(function () {
                div.style.display = 'none';
            }, maxWaitTime);
        }
    }

    function hide() {
        var div = document.getElementById('custom-loader');
        if (div) {
            div.style.display = 'none';
        }
    }

    return {
        show: show,
        hide: hide
    }
})();

var Spinner = (function () {

    var defaults = {
        bouncewidth: '18px', //弹珠大小
        bounceColor: '#333', //弹珠颜色
        backgroundcolor: '#f5f5f5', //背景颜色
        backgroundopacity: 1, //背景透明度
        className: 'spinner-wrapper', //类名
        duration: 0 //持续时间，0则不会计时
    }

    var __assign = (this && this.__assign) || function () {
        __assign = Object.assign || function (t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s)
                    if (Object.prototype.hasOwnProperty.call(s, p))
                        t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };

    // function css(el, props) {
    //     for (var prop in props) {
    //         el.style[prop] = props[prop];
    //     }
    //     return el;
    // }

    function Spinner(opts) {
        if (opts === void 0) {
            opts = {};
        }
        this.opts = __assign({}, defaults, opts);
    }

    Spinner.prototype.show = function (target) {
        this.stop();
        this.el = document.createElement('div');
        this.el.className = this.opts.className;
        var style = '';
        style +=
            '<style>\
                    .spinner-wrapper {\
                        background-color: ' + this.opts.backgroundcolor + ';\
                        opacity:' + this.opts.backgroundopacity + ';\
                        width: 100%;\
                        height: 100%;\
                        position:absolute;\
                        z-index:1000\
                    }\
                    .spinner {\
                        position: absolute;\
                        margin: auto;\
                        text-align: center;\
                        left: 50%;\
                        top: 50%;\
                        transform: translate(-50%, -50%);\
                    }\
                    .spinner>div {\
                        width: ' + this.opts.bouncewidth + ';\
                        height: ' + this.opts.bouncewidth + ';\
                        background-color: ' + this.opts.bounceColor + ';\
                        border-radius: 100%;\
                        display: inline-block;\
                        -webkit-animation: sk-bouncedelay 1.4s infinite ease-in-out both;\
                        animation: sk-bouncedelay 1.4s infinite ease-in-out both;\
                    }\
                    .spinner .bounce1 {\
                        -webkit-animation-delay: -0.32s;\
                        animation-delay: -0.32s;\
                    }\
                    .spinner .bounce2 {\
                        -webkit-animation-delay: -0.16s;\
                        animation-delay: -0.16s;\
                    }\
                    @-webkit-keyframes sk-bouncedelay {\
                        0%,\
                        80%,\
                        100% {\
                            -webkit-transform: scale(0)\
                        }\
                        40% {\
                            -webkit-transform: scale(1.0)\
                        }\
                    }\
                    @keyframes sk-bouncedelay {\
                        0%,\
                        80%,\
                        100% {\
                            -webkit-transform: scale(0);\
                            transform: scale(0);\
                        }\
                        40% {\
                            -webkit-transform: scale(1.0);\
                            transform: scale(1.0);\
                        }\
                    }</style>';

        var html = '';
        html += '<div class="spinner">\
                                <div class="bounce bounce1"></div>\
                                <div class="bounce bounce2"></div>\
                                <div class="bounce bounce3"></div>\
                            </div>';
        this.el.innerHTML = style + html;
        if (target) {
            target.insertBefore(this.el, target.firstChild || null);
        }
        var me = this;
        if (this.opts.duration && this.opts.duration > 0) {
            setTimeout(function () {
                me.stop();
            }, me.opts.duration);
        }
        return this;
    };

    Spinner.prototype.stop = function () {
        if (this.el) {
            if (typeof requestAnimationFrame !== 'undefined') {
                cancelAnimationFrame(this.animateId);
            } else {
                clearTimeout(this.animateId);
            }
            if (this.el.parentNode) {
                this.el.parentNode.removeChild(this.el);
            }
            this.el = undefined;
        }
        return this;
    };
    return Spinner;
})();

var FileHelper = (function () {

    function removeEle(eleid) {
        if (typeof eleid === 'string') {
            var ele = document.getElementById(eleid);
            if (ele) {
                ele.parentNode.removeChild(ele);
            }
        }
    }

    function swapJsFile(id, url) {
        removeEle(id);
        var ss = document.createElement("script");
        ss.setAttribute("id", id);
        ss.setAttribute("src", url);
        document.getElementsByTagName("head")[0].appendChild(ss);
    }

    return {
        removeEle: removeEle,
        swapJsFile: swapJsFile
    }
})();

/**
*base64转换Blob类型
*
* @param {string} dataURI base64
* @returns {} blob类型对象
*/
var base64ToBlob = function (dataURI) {
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]; // mime类型
    var byteString = atob(dataURI.split(',')[1]); //base64解码
    var arrayBuffer = new ArrayBuffer(byteString.length); //创建缓冲数组
    var intArray = new Uint8Array(arrayBuffer); //创建视图
    for (var i = 0; i < byteString.length; i++) {
        intArray[i] = byteString.charCodeAt(i);
    }
    return new Blob([intArray], { type: mimeString });
};

//检测IE内核版本
function IEVersion() {
    var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
    var isIE = userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1; //判断是否IE<11浏览器
    var isEdge = userAgent.indexOf("Edge") > -1 && !isIE; //判断是否IE的Edge浏览器
    var isIE11 = userAgent.indexOf('Trident') > -1 && userAgent.indexOf("rv:11.0") > -1;
    if (isIE) {
        var reIE = new RegExp("MSIE (\\d+\\.\\d+);");
        reIE.test(userAgent);
        var fIEVersion = parseFloat(RegExp["$1"]);
        if (fIEVersion == 7) {
            return 7;
        } else if (fIEVersion == 8) {
            return 8;
        } else if (fIEVersion == 9) {
            return 9;
        } else if (fIEVersion == 10) {
            return 10;
        } else {
            return 6;//IE版本<=7
        }
    } else if (isEdge) {
        return 'edge';//edge
    } else if (isIE11) {
        return 11; //IE11
    } else {
        return -1;//不是ie浏览器
    }
}

//检测浏览器是否兼容
function checkBrowser() {
    var version = IEVersion();
    if (!isNaN(version) && version > -1 && version < 11) {
        document.write(
            '<div style="background:#FFFCCF;z-index: 99999;text-align: center;line-height: 30px">'
            + '您的浏览器版本太低，功能无法使用。请升级到IE11以上，或使用Chrome、Firefox等现代浏览器~'.t()
            + '</div>');
    }
}

checkBrowser();

(function () {
    //待选字符
    var a = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    //兼容IE9
    if (!window.btoa) {
        window.btoa = function (c) {
            var d = "";
            var m, k, h = "";
            var l, j, g, f = "";
            var e = 0;
            do {
                m = c.charCodeAt(e++);
                k = c.charCodeAt(e++);
                h = c.charCodeAt(e++);
                l = m >> 2;
                j = ((m & 3) << 4) | (k >> 4);
                g = ((k & 15) << 2) | (h >> 6);
                f = h & 63;
                if (isNaN(k)) {
                    g = f = 64
                } else {
                    if (isNaN(h)) {
                        f = 64
                    }
                }
                d = d + a.charAt(l) + a.charAt(j) + a.charAt(g) + a.charAt(f);
                m = k = h = "";
                l = j = g = f = ""
            } while (e < c.length);
            return d;
        };
        window.atob = function (c) {
            var d = "";
            var m, k, h = "";
            var l, j, g, f = "";
            var e = 0;
            do {
                l = a.indexOf(c.charAt(e++));
                if (l < 0) {
                    continue
                }
                j = a.indexOf(c.charAt(e++));
                if (j < 0) {
                    continue
                }
                g = a.indexOf(c.charAt(e++));
                if (g < 0) {
                    continue
                }
                f = a.indexOf(c.charAt(e++));
                if (f < 0) {
                    continue
                }
                m = (l << 2) | (j >> 4);
                k = ((j & 15) << 4) | (g >> 2);
                h = ((g & 3) << 6) | f;
                d += String.fromCharCode(m);
                if (g != 64) {
                    d += String.fromCharCode(k)
                }
                if (f != 64) {
                    d += String.fromCharCode(h)
                }
                m = k = h = "";
                l = j = g = f = ""
            } while (e < c.length);
            return d;
        }
    }
    //ie9+,chrome
    var b = {
        /**
         * 编码
         * @param {string} c-需要编码的字符串
         */
        encode: function (c) {
            return window.btoa(unescape(encodeURIComponent(c)));
        },
        /**
         * 解码
         * @param {string} c-需要解码的字符串
         */
        decode: function (c) {
            return decodeURIComponent(escape(window.atob(c)));
        },
        /**
         * 随机字符
         * @param {int} c-长度
         */
        randomString: function (c) {
            c = c || 32;
            var l = a.length;
            var s = '';
            for (i = 0; i < c; i++) {
                s += a.charAt(Math.floor(Math.random() * l));
            }
            return s;
        },
        /**
         * 特定业务场景的编码(比如pwd编码后第4位插入2个随机字符)
         * @param {string} c-需要编码的字符串
         */
        custom4encode: function (c) {
            return this.insertStr(this.encode(c), 4, this.randomString(2));
        },
        /**
         * 为字符串指定位置插入字符
         * @param {string} soure-为原字符串
         * @param {int} start-为将要插入字符的位置
         * @param {string} newStr-为要插入的字符
         * example: insertStr('hello word',5,' li');
         * 显示：hello li word;
         */
        insertStr: function (soure, start, newStr) {
            return soure.slice(0, start) + newStr + soure.slice(start);
        }
    };
    String.prototype.cus4encode = function () {
        var val = this.toString();
        if (val) {
            val = b.custom4encode(val);
        }
        return val;
    };
    window.BASE64 = b;

})();
/**
 * 本地化封装
 */
var localization = window.top.localization || (function (root) {

    var resource = null;

    var collectKeys = [];

    var collectedKeys = [];

    var collectKeyProvider = null;

    /**
     * 初始化资源
     */
    (function initResource() {
        loadStoregeResource();
    })();

    /**
     * 加载客户端缓存资源
     * */
    function loadStoregeResource() {
        var user = CurUserStateHelper.getSessionUser();
        var curCulture = 'zh-CN';
        if (user) {
            curCulture = user.Culture;
        }
        var resourceKey = 'sie_culture_resource_' + curCulture;
        var localResource = LocalStorageHelper.getJsonData(resourceKey);
        resource = localResource;
    }
    /**
     * 翻译
     * @param {string} raw -翻译源
     * @returns {string} -结果 
     */
    function translate(raw) {
        try {
            if (resource.Resources) {
                var resources = resource.Resources;
                if (resources.length > 0) {
                    for (var i = 0; i < resources.length; i++) {
                        if (resources[i].Key === raw) {
                            return resources[i].Value === "" ? raw : resources[i].Value;
                        }
                    }
                }
                collectKey(raw);
            }
            return raw;
        } catch (err) {
            console.warn(err.message);
            return raw;
        }
    }

    /**
     * 收集key
     * @param {any} key -key
     */
    function collectKey(key) {
        if (!(collectedKeys.indexOf(key) > -1)) {
            collectKeys.push(key);
            collectedKeys.push(key);
        }
        if (collectKeyProvider && collectKeys.length > 0) {
            submitCollectKey();
        }
    }

    /**
     * 提交收集的Key
     */
    function submitCollectKey() {
        if (collectKeys.length > 0) {
            var key = collectKeys.shift();
            collectKeyProvider(key);
            setTimeout(function () {
                submitCollectKey();
            }, 1000);
        }
    }

    /**
     * 设置翻译提供程序
     * @param {function()} fn -翻译提供程序 
     */
    function setCollectKeyProvider(fn) {
        collectKeyProvider = fn;
        var loginCollectKeys = LocalStorageHelper.getJsonData('login-collect-keys');
        if (loginCollectKeys && loginCollectKeys.length > 0) {
            collectKeys = collectKeys.concat(loginCollectKeys);
            LocalStorageHelper.removeItem('login-collect-keys');
        }
        if (collectKeys.length > 0) {
            submitCollectKey();
        }
    }

    return {
        translate: translate,
        setCollectKeyProvider: setCollectKeyProvider,
        loadStoregeResource: loadStoregeResource
    };
})(window.top);

(function () {
    String.prototype.L10N = function () {
        return localization.translate(this.toString());
    };

    String.prototype.t = function () {
        return this.L10N();
    };
})();




//参考：https://developer.mozilla.org/zh-CN/docs/Web/JavaScript/Reference

//常量 
var _MESSAGE_OF_NULL_REFERENCES = function (argName) { return argName + " is null (a) references."; };
var _MESSAGE_OF_NULL_ARGUMENTS = function (argName) { return argName + " is null (an) arguments"; };
var _MESSAGE_OF_INVALID_ARGUMENTS = function (argName, needsType) { return argName + " is (an) invalid arguments." + (!needsType ? "It's have to " + needsType : ""); };
var _MESSAGE_OF_NOT_SUPPORT_ARGUMENTS = function (argName, argObject) { return typeof argObject + " type of " + argName + " argument is not support"; };

/**
 * 仿cs的Check输入检查
 */
var Check = Check || {
    notNull: function (value, parameterName) {
        if (SIE.isEmpty(value)) {
            throw Ext.String.format('{0}参数不能为空'.t(), parameterName);
        }
    }
};

+ function () {

    // 内置默认方法
    function DefaultEqualityComparer(a, b) {
        return a === b || a.valueOf() === b.valueOf();
    };

    function DefaultSortComparer(a, b) {
        if (a === b) return 0;
        if (a == null) return -1;
        if (b == null) return 1;
        if (typeof a == "string") return a.toString().localeCompare(b.toString());
        return a.valueOf() - b.valueOf();
    };

    function DefaultPredicate() {
        return true;
    };

    function DefaultSelector(t) {
        return t;
    };

    // 选择器类

    /**
     * 将序列中的每个元素投影到新元素
     * 使用示例：
     * var arr = [ { "key": "sie",    "value": "http://www.sie.com" },
                { "key": "siesmom",       "value": "http://siesmom.com" },
                { "key": "sieNest",      "value": "http://sieNest.com" }];
        var selected = arr.select(function(o) {
            return { name: o.key, website: o.value };
        });
        // results var selected
        [ { "name": "sie",      "website": "http://www.sie.com" },
          { "name": "siesmom",  "website": "http://siesmom.com" },
          { "name": "sieNest",  "website": "http://sieNest.com" }]
     * @param {function} predicate-函数判断条件
     * @returns {element} 
     */
    Array.prototype.select = Array.prototype.map || function (selector, context) {
        context = context || window;
        var arr = [];
        var l = this.length;
        for (var i = 0; i < l; i++)
            arr.push(selector.call(context, this[i], i, this));
        return arr;
    };

    /**
     * SelectMany操作符可以将多个组合起来，将每个对象的结果合并成一个序列。
     * 示例：var arr = [{Name:"A", Values:[1, 2, 3, 4]}, {Name:"B", Values:[5, 6, 7, 8]}];  
             var res1 = arr.selectMany(function(t){ return t.Values });  //使用默认的结果选择器
             var res2 = arr.selectMany(function(t){ return t.Values }, function(t, u){ return {Name:t.Name, Val:u}});  // 使用自定义的结果选择器
     * @param {type} selector
     * @param {type} resSelector
     */
    Array.prototype.selectMany = function (selector, resSelector) {
        resSelector = resSelector || function (i, res) { return res; };
        return this.aggregate(function (a, b) {
            return a.concat(selector(b).select(function (res) { return resSelector(b, res) }));
        }, []);
    };

    /**
     *  从序列的开头返回指定的数量的连续元素
     *  var arr = [1,2,3,4,5,6,7,8,9,10];
        var take =  arr.take(5).toString();
        // results var take
        take = 1,2,3,4,5
     * @param {Number} number-要返回的元素数量
     * @returns {Array}  其中包含指定从输入序列的起始位置的元素数
     */
    Array.prototype.take = function (number) {
        if (arguments.length === 0) throw 'take方法需要一个数字参数'.t();
        return this.slice(0, number);
    };

    /**
     * 跳过指定的数量的序列中的元素，然后返回剩余元素
     * @param {Number} number-要返回的元素数量
     * @returns {Array} 其中包含指定从输入序列的起始位置的元素数
     */
    Array.prototype.skip = Array.prototype.slice;

    /**
     * 返回一个序列的第一个元素。
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var t1 = arr.first(); // 1 
        var t2 = arr.first(function(t){ return t > 2 });  // using comparer: 3 
        var t3 = arr.first(function(t){ return t > 10 }, 10);  // using comparer and default value: 10 
     * @param {function} predicate-条件
     * @param {Number||String||Objcet} def-默认值
     * @returns {element} 
     */
    Array.prototype.first = function (predicate, def) {
        var l = this.length;
        if (!predicate) return l ? this[0] : def == null ? null : def;
        for (var i = 0; i < l; i++)
            if (predicate(this[i], i, this))
                return this[i];

        return def == null ? null : def;
    };

    /**
     * 返回一个序列的最后一个元素。
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var t1 = arr.last(); // 5 
        var t2 = arr.last(function(t){ return t > 2 });  // using comparer: 5 
        var t3 = arr.last(function(t){ return t > 10 }, 10);  // using comparer and default value: 10 
     * @param {type} predicate
     * @param {type} def
     * @returns {type} 
     */
    Array.prototype.last = function (predicate, def) {
        var l = this.length;
        if (!predicate) return l ? this[l - 1] : def == null ? null : def;
        while (l-- > 0)
            if (predicate(this[l], l, this))
                return this[l];

        return def == null ? null : def;
    };

    /**
     * 使用默认的相等比较器生成两个序列的集合并集。
     * 示例：
        var arr1 = [1, 2, 3, 4, 5]; 
        var arr2 = [5, 6, 7, 8, 9];
        var res = arr1.union(arr2);  // [1, 2, 3, 4, 5, 6, 7, 8, 9]  
     * @param {Array} arr-一个数组
     * @returns {Array} 新的数组 
     */
    Array.prototype.union = function (arr) {
        return this.concat(arr).distinct();
    };

    /**
     * 生成两个序列的集合交集
     * 示例:
        var arr1 = [1, 2, 3, 4, 5]; 
        var arr2 = [1, 2, 3]; 
        var res = arr1.intersect(arr2);  // [1, 2, 3]  
     * @param {Array} arr-一个数组
     * @param {Function} comparer--比较器
     * @returns {Array} 新的数组 
     */
    Array.prototype.intersect = function (arr, comparer) {
        comparer = comparer || DefaultEqualityComparer;
        return this.distinct(comparer).where(function (t) {
            return arr.contains(t, comparer);
        });
    };

    /**
     * 生成两个序列的差集。
     * 示例：
        var arr1 = [1, 2, 3, 4, 5]; 
        var arr2 = [2, 3, 4];
        var res = arr1.except(arr2);  // [1, 5] 
     * @param {Array} arr-一个数组
     * @param {Function} comparer--比较器
     * @returns {Array} 新的数组 
     */
    Array.prototype.except = function (arr, comparer) {
        if (!(arr instanceof Array)) arr = [arr];
        comparer = comparer || DefaultEqualityComparer;
        var l = this.length;
        var res = [];
        for (var i = 0; i < l; i++) {
            var k = arr.length;
            var t = false;
            while (k-- > 0) {
                if (comparer(this[i], arr[k]) === true) {
                    t = true;
                    break;
                }
            }
            if (!t) res.push(this[i]);
        }
        return res;
    };

    /**
     * 通过使用默认的相等比较器比较值，从序列中返回不同的元素。
     * 示例
        var arr1 = [1, 2, 2, 3, 3, 4, 5, 5];   
        var res1 = arr.distinct();  // [1, 2, 3, 4, 5]

        var arr2 = [{Name:"A", Val:1}, {Name:"B", Val:1}];
        var res2 = arr2.distinct(function(a, b){ return a.Val == b.Val });  // [{Name:"A", Val:1}] 
     * @param {Function} comparer--比较器
     * @returns {Array} 新的数组 
     */
    Array.prototype.distinct = function (comparer) {
        var arr = [];
        var l = this.length;
        for (var i = 0; i < l; i++) {
            if (!arr.contains(this[i], comparer))
                arr.push(this[i]);
        }
        return arr;
    };

    /**
     * 将指定的函数应用于两个序列的相应元素，这将生成结果的序列
     * 示例：
         var arr1 = [1, 2, 3, 4]; 
         var arr2 = ["A", "B", "C", "D"];
         var res = arr1.zip(arr2, function(a, b){ return {Num:a, Letter:b} });   
         结果// [{Num:1, Letter: "A"},{Num:2, Letter: "B"}, {Num:3, Letter: "C"}, {Num:4, Letter: "D"}]  
     * @param {Array} arr-一个数组
     * @param {Function} selector-选择器
     * @returns {Array} 新的数组 
     */
    Array.prototype.zip = function (arr, selector) {
        return this
            .take(Math.min(this.length, arr.length))
            .select(function (t, i) {
                return selector(t, arr[i]);
            });
    };

    /**
     * 返回一维数组或数组某一部分中值的第一个匹配项的索引。
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var index = arr.indexOf(2);  // 1 
     */
    Array.prototype.indexOf = Array.prototype.indexOf || function (o, index) {
        var l = this.length;
        for (var i = Math.max(Math.min(index, l), 0) || 0; i < l; i++)
            if (this[i] === o) return i;
        return -1;
    };

    /**
     * 返回一维数组或数组的一部分中最后一次出现的值的索引。
     * 示例:
         var arr = [1, 2, 3, 4, 5, 3, 4, 5];
         var index = arr.lastIndexOf(3);  // 5 
     */
    Array.prototype.lastIndexOf = Array.prototype.lastIndexOf || function (o, index) {
        var l = Math.max(Math.min(index || this.length, this.length), 0);
        while (l-- > 0)
            if (this[l] === o) return l;
        return -1;
    };

    /**
     * 从Array中删除第一次出现的特定对象。
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        arr.remove(2);   // [1, 3, 4, 5]
     */
    Array.prototype.remove = function (item) {
        var i = this.indexOf(item);
        if (i != -1)
            this.splice(i, 1);
    };

    /**
     * 删除与指定谓词定义的条件匹配的所有元素。
     * 示例:
         var arr = [1, 2, 3, 4, 5];
         arr.removeAll(function(t){ return t % 2 == 0 });  // [1, 3, 5]  
     */
    Array.prototype.removeAll = function (predicate) {
        var item;
        var i = 0;
        while ((item = this.first(predicate)) != null) {
            i++;
            this.remove(item);
        }

        return i;
    };

    /**
     * 根据键按升序对序列的元素进行排序
     * 示例：
        var arr = [ 23, 8, 43 ,81, 4, 32, 64 ];
        arr = arr.orderBy(); // results [4,8,23,32,43,64,81]

        var arr = [{Name:"A", Val:1}, {Name:"a", Val:2}, {Name:"B", Val:1}, {Name:"C", Val:2}];
        var res1 = arr.orderBy(function(t){ return t.Name });
        var res2 = arr.orderBy(function(t){ return t.Val })
          .thenBy(function(t){ return t.Name });   
        var res3 = arr.orderBy(function(t){ return t.Val })
          .thenByDescending(function(t){ return t.Name }); 
     * @param {Function} selector-选择类
     * @param {Function} comparer-比较器
     * @returns {Array} 
     */
    Array.prototype.orderBy = function (selector, comparer) {
        comparer = comparer || DefaultSortComparer;
        selector = selector || DefaultSelector;
        var arr = this.slice(0);
        var fn = function (a, b) {
            return comparer(selector(a), selector(b));
        };

        arr.thenBy = function (selector, comparer) {
            comparer = comparer || DefaultSortComparer;
            selector = selector || DefaultSelector;
            return arr.orderBy(DefaultSelector, function (a, b) {
                var res = fn(a, b);
                return res === 0 ? comparer(selector(a), selector(b)) : res;
            });
        };

        arr.thenByDescending = function (selector, comparer) {
            comparer = comparer || DefaultSortComparer;
            selector = selector || DefaultSelector;
            return arr.orderBy(DefaultSelector, function (a, b) {
                var res = fn(a, b);
                return res === 0 ? -comparer(selector(a), selector(b)) : res;
            });
        };

        return arr.sort(fn);
    };

    /**
     * 按降序对序列的元素进行排序。
     * 示例：
        var arr = [ 23, 8, 43 ,81, 4, 32, 64 ];
        arr = arr.orderBy(); // results [4,8,23,32,43,64,81]

        var arr = [{Name:"A", Val:1}, {Name:"a", Val:2}, {Name:"B", Val:1}, {Name:"C", Val:2}];
        var res1 = arr.orderByDescending(function(t){ return t.Name });
        var res2 = arr.orderByDescending(function(t){ return t.Val })
          .thenBy(function(t){ return t.Name });   
        var res3 = arr.orderByDescending(function(t){ return t.Val })
          .thenBy(function(t){ return t.Name }); 
     * @param {Function} selector-选择类
     * @param {Function} comparer-比较器
     * @returns {Array} 
     */
    Array.prototype.orderByDescending = function (selector, comparer) {
        comparer = comparer || DefaultSortComparer;
        selector = selector || DefaultSelector;
        return this.orderBy(selector, function (a, b) { return -comparer(a, b) });
    };

    /**
     * 内关联两个序列的元素。
     * 示例
        var arr1 = [{Name:"A", Val:1}, {Name:"B", Val:2}, {Name:"C", Val:3}];
        var arr2 = [{Code:"A"}, {Code:"B"}, {Name:"C", Code:"C"}]; 

        var res1 = arr1.innerJoin(arr2,
            function (t) { return t.Name },                                      // arr1 selector
            function (u) { return u.Code },                                      // arr2 selector
            function (t, u) { return { Name: t.Name, Val: t.Val, Code: u.Code } });  // result selector

        // 使用自定义比较器
        var res2 = arr1.innerJoin(arr2,
            function (t) { return t.Name },                                    // arr1 selector
            function (u) { return u.Code },                                    // arr2 selector
            function (t, u) { return { Name: t.Name, Val: t.Val, Code: u.Code } },  // result selector
            function (a, b) { return a.toUpperCase() == b.toUpperCase() });         // comparer     
     * @param {Array} arr-一个数组
     * @param {Function} outer -关联键
     * @param {Function} inner - 关联键
     * @param {Function} result -结果
     * @param {Function} comparer-比较器
     * @returns {Array} 
     */
    Array.prototype.innerJoin = function (arr, outer, inner, result, comparer) {
        comparer = comparer || DefaultEqualityComparer;
        var res = [];

        this.forEach(function (t) {
            arr.where(function (u) {
                return comparer(outer(t), inner(u));
            })
                .forEach(function (u) {
                    res.push(result(t, u));
                });
        });

        return res;
    };

    /**
     * 分组关联两个序列的元素。
     * 示例
        var arr1 = [{Name:"A", Val:1}, {Name:"B", Val:2}, {Name:"C", Val:3}];
        var arr2 = [{Code:"A"}, {Code:"A"}, {Code:"B"}, {Code:"B"}, {Code:"C"}];  

        var res1 = arr1.groupJoin(arr2, 
            function(t){ return t.Name },                     // arr1 selector
            function(u){ return u.Code },                     // arr2 selector
            function(t, u){ return {Item:t, Group:u} }) ;         // result selector  
  
        // 使用自定义比较器
        var res2 = arr1.groupJoin(arr2, 
            function(t){ return t.Name },                             // arr1 selector
            function(u){ return u.Code },                             // arr2 selector
            function(t, u){ return {Item:t, Group:u} },                 // result selector 
            function(a, b){ return a.toUpperCase() == b.toUpperCase() });     // comparer  
     * @param {Array} arr-一个数组
     * @param {Function} outer -关联键
     * @param {Function} inner - 关联键
     * @param {Function} result -结果
     * @param {Function} comparer-比较器
     * @returns {Array} 
     */
    Array.prototype.groupJoin = function (arr, outer, inner, result, comparer) {
        comparer = comparer || DefaultEqualityComparer;
        return this
            .select(function (t) {
                var key = outer(t);
                return {
                    outer: t,
                    inner: arr.where(function (u) { return comparer(key, inner(u)); }),
                    key: key
                };
            })
            .select(function (t) {
                t.inner.key = t.key;
                return result(t.outer, t.inner);
            });
    };

    /**
     * 根据指定的键选择器对序列的元素进行分组。
     * 示例：
        var arr = [{Name:"A", Val:1}, {Name:"B", Val:1}, {Name:"C", Val:2}, {Name:"D", Val:2}]; 
        var res = arr.groupBy(function(t){ return t.Val }); 
        res.forEach(function(t){ 
            console.log("Key: " + t.key, "Length: " + t.length); 
        });  
     * @param {Function} selector-选择器
     * @param {Function} comparer-比较器
     * @returns {Array} 
     */
    Array.prototype.groupBy = function (selector, comparer) {
        var grp = [];
        var l = this.length;
        comparer = comparer || DefaultEqualityComparer;
        selector = selector || DefaultSelector;

        for (var i = 0; i < l; i++) {
            var k = selector(this[i]);
            var g = grp.first(function (u) { return comparer(u.key, k); });

            if (!g) {
                g = [];
                g.key = k;
                grp.push(g);
            }

            g.push(this[i]);
        }
        return grp;
    };

    /**
     * 根据指定的键选择器函数从数组创建对象
     * 示例：
        var arr = [1, 2, 3, 4, 5]; 
        var dic = arr.toDictionary(function(t){ return "Num" + t }, function(u){ return u });   
        // dic = {Num5: 5, Num4: 4, Num3: 3, Num2: 2, Num1: 1} 
     * @param {Function} keySelector
     * @param {Function} valueSelector
     * @returns {Object} 
     */
    Array.prototype.toDictionary = function (keySelector, valueSelector) {
        var o = {};
        var l = this.length;
        while (l-- > 0) {
            var key = keySelector(this[l]);
            if (key == null || key == "") continue;
            o[key] = valueSelector(this[l]);
        }
        return o;
    };


    // 统计类

    /**
     * 在序列上应用累加器
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var sum = arr.aggregate(function(a, b){ return a + b }, 0);  // 15 
     * @param {Function} func-执行函数
     * @param {Number} seed-种子
     * @returns {Number} 
     */
    Array.prototype.aggregate = Array.prototype.reduce || function (func, seed) {
        var arr = this.slice(0);
        var l = this.length;
        if (seed == null) seed = arr.shift();

        for (var i = 0; i < l; i++)
            seed = func(seed, arr[i], i, this);

        return seed;
    };

    /**
     * 返回值序列中的最小值
     * 示例：
        var arr1 = [1, 2, 3, 4, 5, 6, 7, 8];
        var min1 = arr.min();  // 1 

        var arr2 = [{Name:"A", Val:1}, {Name:"B", Val:2}];
        var min2 = arr2.min(function(t){ return t.Val });   // 1 
     * @param {function} s-选择器
     * @returns {Number} 
     */
    Array.prototype.min = function (s) {
        s = s || DefaultSelector;
        var l = this.length;
        var min = s(this[0]);
        while (l-- > 0)
            if (s(this[l]) < min) min = s(this[l]);
        return min;
    };

    /**
     * 返回值序列中的最大值。
     * 示例：
        var arr1 = [1, 2, 3, 4, 5, 6, 7, 8];
        var max1 = arr.max();  // 8 

        var arr2 = [{Name:"A", Val:1}, {Name:"B", Val:2}];
        var max2 = arr2.max(function(t){ return t.Val });   // 2 
     * @param {function} s-选择器
     * @returns {Number} 
     */
    Array.prototype.max = function (s) {
        s = s || DefaultSelector;
        var l = this.length;
        var max = s(this[0]);
        while (l-- > 0)
            if (s(this[l]) > max) max = s(this[l]);
        return max;
    };

    /**
     * 计算系列数值的总和。
     * 示例：
        var arr1 = [1, 2, 3, 4, 5, 6, 7, 8];
        var sum1 = arr.sum();  // 36 

        var arr2 = [{Name:"A", Val:1}, {Name:"B", Val:2}];
        var sum2 = arr2.sum(function(t){ return t.Val });   // 3 
     * @param {function} s-选择器
     * @returns {Number}  
     */
    Array.prototype.sum = function (s) {
        s = s || DefaultSelector;
        var l = this.length;
        var sum = 0;
        while (l-- > 0) sum += s(this[l]);
        return sum;
    };

    // 筛选类

    /**
     * 根据条件过滤系列值。
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var res = arr.where(function(t){ return t > 2 }) ;  // [3, 4, 5] 

        var arr = [ { "key": "sie",    "value": "http://www.sie.com" },
            { "key": "siesmom",       "value": "http://siesmom.com" },
            { "key": "sieNest",      "value": "http://sieNest.com.cn" }];
        var selected = arr.where(function(o) {
            return o.value.lastIndexOf(".cn") > 0
        });
        // results var selected
        [ "key": "sieNest",      "value": "http://sieNest.com.cn" }]
     * @param {Function} predicate-条件
     * @param {scope} context-上下文
     * @returns {Array} 
     */
    Array.prototype.where = Array.prototype.filter || function (predicate, context) {
        context = context || window;
        var arr = [];
        var l = this.length;
        for (var i = 0; i < l; i++)
            if (predicate.call(context, this[i], i, this) === true) arr.push(this[i]);
        return arr;
    };

    /**
     * 确定序列的任何元素是否存在或满足条件
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var res1 = arr.any();  // true
        var res2 = arr.any(function(t){ return t > 5 });  // false 
     * @param {Function} predicate-条件
     * @param {scope} context-上下文
     * @returns {Boolean} 
     */
    Array.prototype.any = function (predicate, context) {
        context = context || window;
        var f = this.some || function (p, c) {
            var l = this.length;
            if (!p) return l > 0;
            while (l-- > 0)
                if (p.call(c, this[l], l, this) === true) return true;
            return false;
        };
        return f.apply(this, [predicate, context]);
    };

    /**
     * 确定序列的所有元素是否满足条件
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var res = arr.all(function(t){ return t < 6 });  // true 
     * @param {Function} predicate-条件
     * @param {scope} context-上下文
     * @returns {Boolean} 
     */
    Array.prototype.all = function (predicate, context) {
        context = context || window;
        predicate = predicate || DefaultPredicate;
        var f = this.every || function (p, c) {
            return this.length == this.where(p, c).length;
        };
        return f.apply(this, [predicate, context]);
    };

    /**
     * 只要指定的条件为true，就返回序列中的元素，然后跳过其余元素。
     * 示例：
        var arr = [1, 2, 3, 4, 5, 6, 7, 8];
        var res = arr.takeWhile(function(t){ return t % 4 != 0 });  // [1, 2, 3] 
     * @param {Function} predicate-条件
     * @returns {Array} 
     */
    Array.prototype.takeWhile = function (predicate) {
        predicate = predicate || DefaultPredicate;
        var l = this.length;
        var arr = [];
        for (var i = 0; i < l && predicate(this[i], i) === true; i++)
            arr.push(this[i]);

        return arr;
    };

    /**
     * 只要指定的条件为true，就会跳过序列中的元素，然后返回剩余的元素。
     * 示例：
        var arr = [1, 2, 3, 4, 5, 6, 7, 8];
        var res = arr.skipWhile(function(t){ return t & 4 != 0 }) ;   // [ 4, 5, 6, 7, 8] 
     * @param {Function} predicate-条件
     * @returns {Array} 
     */
    Array.prototype.skipWhile = function (predicate) {
        predicate = predicate || DefaultPredicate;
        var l = this.length;
        var i = 0;
        for (i = 0; i < l; i++)
            if (predicate(this[i], i) === false) break;

        return this.skip(i);
    };

    /**
     * 确定序列是否包含指定的元素。
     * 示例：
        var arr1 = [1, 2, 3, 4, 5]; 
        var res1 = arr.contains(2);  // true 

        var arr2 = [{Name:"A", Val:1}, {Name:"B", Val:1}]; 
        var res2 = arr2.contains({Name:"C", Val:1}, function(a, b){ return a.Val == b.Val }) ;  // true
     * @param {Object} o-一个值
     * @param {Function} comparer-比较器
     * @returns {Boolean} 
     */
    Array.prototype.contains = function (o, comparer) {
        comparer = comparer || DefaultEqualityComparer;
        var l = this.length;
        while (l-- > 0)
            if (comparer(this[l], o) === true) return true;
        return false;
    };

    // 迭代类

    /**
     * 循环的变种写法支持
     */
    Array.prototype.forEach = Array.prototype.forEach || function (callback, context) {
        context = context || window;
        var l = this.length;
        for (var i = 0; i < l; i++)
            callback.call(context, this[i], i, this);
    };

    /**
     * 如果序列为空，则返回指定序列的元素或单例集合中的指定值
     * 示例：
        var arr = [1, 2, 3, 4, 5];
        var res = arr.where(function(t){ return t > 5 }).defaultIfEmpty(5);  // [5]  
     * @param {Function} val-值
     * @returns {Array} 
     */
    Array.prototype.defaultIfEmpty = function (val) {
        return this.length == 0 ? [val == null ? null : val] : this;
    };

    /**
    * range生成指定范围内的整数序列
    * 示例：
         var range = [].range(0,10);         // 0,1,2,3,4,5,6,7,8,9
         var arr = Array.range(10);          // 0,1,2,3,4,5,6,7,8,9
         var arr = Array.range(10, 20);      // 10,11,12,13,14,15,16,17,18,19
     * @param {Number} start-序列中第一个整数的值
     * @param {Number} count-要生成的顺序整数的数目
     * @returns {Array} 
     */
    Array.range = function (start, count) {
        var arr = [];
        while (count-- > 0) {
            arr.push(start++);
        }
        return arr;
    };

}();

if (typeof (SIE) == "undefined") { SIE = { __namespace: true }; }

if (typeof (SIE.Util) == "undefined") { SIE.Util = { __namespace: true }; }

//工具类封装
SIE.Util.Url = {

    /**
     * 获取最外层Frame的url参数的值
     * @param {string} key 
     * @example SIE.Util.Url.getQueryVariable
     */
    getQueryValue: function (key) {
        if (window.frameElement) {
            var query = window.frameElement.src.split('?')[1];
            if (query) {
                var vars = query.split("&");
                for (var i = 0; i < vars.length; i++) {
                    var pair = vars[i].split("=");
                    if (pair[0] == key) { return decodeURIComponent(pair[1]); }
                }
            }
        }
        return (false);
    },

    /**
     * 获取最外层Frame的url参数的值
     * @example SIE.Util.Url.getQueryVariable
     */
    getQueryKeyValues: function () {
        if (window.frameElement) {
            var query = window.frameElement.src.split('?')[1];
            if (query) {
                var result = {};
                var vars = query.split("&");
                for (var i = 0; i < vars.length; i++) {
                    var pair = vars[i].split("=");
                    result[pair[0]] = decodeURIComponent(pair[1]);
                }
                return result;
            }
        }
        return (false);
    },

    /**
     * 获取最外层Frame的url参数的值
     * @example SIE.Util.Url.getQueryVariable
     */
    getUrlKeyValues: function () {
        var query = window.top.location.href.split('#')[1];
        if (query) {
            var result = {};
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                result[pair[0]] = decodeURIComponent(pair[1]);
            }
            return result;
        }
        return (false);
    },

    getRootPathSuffix: function () {
        return window.top.location.href.substring(window.top.location.href.indexOf('#') + 1);
    },

    /**
     * 拼接URL参数串
     * @param {*} params 
     */
    getUrlParamsString: function (params) {
        var str = '';
        for (var param in params) {
            if (params.hasOwnProperty(param)) {
                if (str.length > 0) str += '&';
                str += param + '=' + params[param];
            }
        }
        return str;
    },

    /**
     * 设置浏览器url
     * @param {any} path
     */
    setRootPath: function (path) {
        window.top.location.href = path;
    },

    /**
     * 
     * @param {*} opt 
     */
    getPageUriSuffix: function (opt) {
        var params = {};
        if (opt.url) { uri = opt.url; }
        for (var key in opt) {
            if (typeof opt[key] == 'object') {
                params[key] = encodeURIComponent(JSON.stringify(opt[key]));
            } else {
                if (opt[key]) params[key] = encodeURIComponent(opt[key]);
            }
        }
        var suffix = this.getUrlParamsString(params);
        return suffix;
    },

    /**
  * 转换url
  * @param {string} url-url地址
  */
    urlConvert: function (url) {
        var convertUrl = url;
        var documentUrlTag = 'http://#domainName#';
        if (convertUrl.indexOf(documentUrlTag) >= 0) {
            var domainName = Ext.String.format('{0}//{1}', window.location.protocol, window.location.host);
            convertUrl = convertUrl.replace(new RegExp(documentUrlTag, 'g'), domainName);
        }
        return convertUrl;
    }
};


SIE.Util.File = {

    /**
        * 加载脚本
        * @param {Object} cfg 参数
        * @param {Array} cfg.urls 脚本路径数组。['/lib/a.js','/lib/b.js','/lib/c.js']
        * @param {int} cfg.mode 脚本加载方式，默认为串行加载LOADMODE_SEQU
        * @param {Function} cfg.onLoad 单个脚本加载成功事件,可选   回调参数[src]
        * @param {Function} cfg.onEnd 全部脚本加载完成事件,可选
        * @param {Function} cfg.onError 单个脚本加载失败事件,可选
        * @return this
    */
    loadScripts: function (cfg) {
        //var LOADMODE_SEQU = 2;
        var loaded = cfg.urls.length;
        var onLoad = cfg.onLoad;
        var onEnd = cfg.onEnd;
        var onError = cfg.onError;
        var d = document;
        var b = document.body;
        //var mode = cfg.mode || LOADMODE_SEQU;
        //if (mode === LOADMODE_SEQU)
        //    loadScript(cfg.urls.concat(), loaded, onLoad, onError, onEnd);
        //else {
        for (var i = cfg.urls.length; i--;) {
            var s = d.createElement("script");//指定src时，类型必须是javascript或者空，无法加载文本资源
            if (!s.async) s.defer = true;
            s.onload = function () {
                this.onerror = null;
                this.onload = null;
                document.body.removeChild(this);
                if (onLoad && onLoad.call) {
                    onLoad(this.src);
                }
                loaded--;
                if (!loaded && onEnd && onEnd.call) {
                    onEnd();
                }
            }
            s.onerror = function () {
                this.onerror = null;
                this.onload = null;
                document.body.removeChild(this);
                if (onError && onError.call) {
                    onError(this.src);
                }
                loaded--;
                if (!loaded && onEnd && onEnd.call) {
                    onEnd();
                }
            }
            s.src = cfg.urls[i];
            b.appendChild(s);
        }
        //}
        //return this;
    }
};

if (typeof (SIE.Control) == "undefined") { SIE.Control = { __namespace: true }; }

SIE.Control.FullScreen = function (opts) {
    opts = opts || {};
    opts.width = opts.width || '36px';
    opts.height = opts.height || '36px';
    opts.background = opts.background || '#bbbbbb';
    opts.opacity = opts.opacity || 0.5;
    opts.top = opts.top;
    opts.left = opts.left;
    opts.right = opts.right;
    opts.bottom = opts.bottom;
    opts.position = 'fixed';
    opts.cursor = 'pointer';
    opts.borderRadius = '50%';
    opts.textAlign = 'center';
    opts.zIndex = opts.zIndex || 999;
    opts.element = opts.element || document.documentElement;

    var fn = (function () {
        var val;

        var fnMap = [
            [
                'requestFullscreen',
                'exitFullscreen',
                'fullscreenElement',
                'fullscreenEnabled',
                'fullscreenchange',
                'fullscreenerror'
            ],
            // New WebKit
            [
                'webkitRequestFullscreen',
                'webkitExitFullscreen',
                'webkitFullscreenElement',
                'webkitFullscreenEnabled',
                'webkitfullscreenchange',
                'webkitfullscreenerror'

            ],
            // Old WebKit
            [
                'webkitRequestFullScreen',
                'webkitCancelFullScreen',
                'webkitCurrentFullScreenElement',
                'webkitCancelFullScreen',
                'webkitfullscreenchange',
                'webkitfullscreenerror'

            ],
            [
                'mozRequestFullScreen',
                'mozCancelFullScreen',
                'mozFullScreenElement',
                'mozFullScreenEnabled',
                'mozfullscreenchange',
                'mozfullscreenerror'
            ],
            [
                'msRequestFullscreen',
                'msExitFullscreen',
                'msFullscreenElement',
                'msFullscreenEnabled',
                'MSFullscreenChange',
                'MSFullscreenError'
            ]
        ];

        var i = 0;
        var l = fnMap.length;
        var ret = {};

        for (; i < l; i++) {
            val = fnMap[i];
            if (val && val[1] in document) {
                for (i = 0; i < val.length; i++) {
                    ret[fnMap[0][i]] = val[i];
                }
                return ret;
            }
        }

        return false;
    })();

    this.show = function () {
        var btnFullScreen = document.createElement('div');
        btnFullScreen.style.width = opts.width;
        btnFullScreen.style.height = opts.height;
        btnFullScreen.style.background = opts.background;;
        btnFullScreen.style.opacity = opts.opacity;
        btnFullScreen.style.top = opts.top;
        btnFullScreen.style.left = opts.left;
        btnFullScreen.style.right = opts.right;
        btnFullScreen.style.bottom = opts.bottom;
        btnFullScreen.style.cursor = opts.cursor;
        btnFullScreen.style.borderRadius = opts.borderRadius;
        btnFullScreen.style.textAlign = opts.textAlign;
        btnFullScreen.style.position = opts.position;
        btnFullScreen.style.zIndex = opts.zIndex;
        bindEvent(btnFullScreen);
        var icon = document.createElement('i');
        icon.style.lineHeight = opts.height;
        icon.style.width = opts.width;
        icon.style.height = opts.height;
        icon.className = 'iconfont icon-FullScreen';
        btnFullScreen.appendChild(icon);
        document.body.appendChild(btnFullScreen);
    }

    function bindEvent(btn) {
        btn.onclick = toggleFullScreen;
        btn.draggable = true;
        btn.ondragend = function (e) {
            btn.style.right = 'auto';
            btn.style.bottom = 'auto';
            btn.style.left = (e.x - (parseInt(btn.style.width) / 2)) + 'px';
            btn.style.top = (e.y - (parseInt(btn.style.height) / 2)) + 'px';
        }
    }

    function toggleFullScreen() {
        if (!isFullScreen()) {
            opts.element[fn.requestFullscreen]();
            this.firstElementChild.className = 'iconfont icon-ExitFullScreen';
        } else {
            document[fn.exitFullscreen]();
            this.firstElementChild.className = 'iconfont icon-FullScreen';
        }
    }

    function isFullScreen() {
        return document[fn.fullscreenElement];
    }
}

if (typeof (CRT) === "undefined") { CRT = { __namespace: true }; }
function initEnvironment(opts, meta, record) {
    var userInfo = CurUserStateHelper.getSessionUser();
    var jsPath;
    if (userInfo.Culture) {
        SIE.App.CurCulture = userInfo.Culture;
        jsPath = '/ExtJs/locale/locale-' + userInfo.Culture + '.js';
    } else {
        jsPath = '/ExtJs/locale/locale-zh-CN.js';
    }
    SIE.Util.File.loadScripts({
        urls: [jsPath],
        onEnd: function () { _initPage(opts, meta, record); },
        onError: function () { _initPage(opts, meta, record); console.warn('无法加载Ext语言包，请检查') }
    });

    var btnFullScreen = new SIE.Control.FullScreen({
        opacity: 0.6,
        top: '8px',
        right: '8px',
        width: '32px',
        height: '32px'
    });
    btnFullScreen.show();
}

function _initPage(opts, meta, record) {
    var pageClass = opts.pageClass;
    window.Page = Ext.create(pageClass || 'SIE.Page');
    var isDetail = opts.isDetail;
    Page.beforeLoad(meta);
    if (Page.isCustomize) {
        _initCustomizePage(meta);
    } else if (isDetail) {
        _initFormPage(opts, meta, record);
    } else {
        _initModulePage(opts, meta);
    }
}

function _initModulePage(opts, meta) {
    Page.onInit(meta);
    var entityType = opts.entityType;
    var ui = SIE.AutoUI.generateAggtControl(meta);
    var mainview = ui.getView();
    mainview.module = SIE.Util.Url.getQueryValue('module');
    //挂载刷新事件
    var refresh = function (recordId) {
        if (mainview.refreshData)
            mainview.refreshData(recordId);
    };
    CRT.Event.listen(entityType.split(',')[0] + "_refresh", refresh);
    CRT.Context.PageContext.setLogicalView(mainview);
    var viewport = _getViewPort(mainview, ui.getControl());
    CRT.Context.PageContext.setCurEntityType(entityType);
    Page.setControl(ui.getControl());
    var tabId = SIE.Util.Url.getQueryValue('tabId') || ('tab_' + entityType).replace(/[.|,]/g, '');
    var tab = window.top.Ext.getCmp(tabId);
    if (tab) {
        tab.setTitle(tab.title || meta.mainBlock.label);
    }
    Page.onLoad();
}

function _initFormPage(opts, meta, record) {
    var entityType = opts.entityType;
    var entity;
    if (meta.model || (meta.mainBlock && meta.mainBlock.model))
        entity = Ext.create(meta.model || meta.mainBlock.model);
    if (record !== null) {
        var store = SIE.data.Utils.createStore({ model: entityType });
        var phantom = false;
        if (record.PersistenceStatus == 2) {
            phantom = true
            record.CreateDate = null;
            record.UpdateDate = null;
        }
        store.add(record);
        entity = store.data.items[0];
        entity.phantom = phantom;
        CRT.Context.PageContext.setCurrentRecord(entity);
    }
    Page.onInit(meta);
    var ui = SIE.AutoUI.generateAggtControl(meta, entity);
    var mainview = ui.getView();
    mainview.module = SIE.Util.Url.getQueryValue('module');
    mainview.validateData();

    //挂载刷新事件
    var refresh = function (recordId) {
        if (mainview.refreshData)
            mainview.refreshData(recordId);
    };
    CRT.Event.listen(entityType.split(',')[0] + '_' + entity.getId() + "_refresh", refresh);
    CRT.Context.PageContext.setLogicalView(mainview);
    var viewport = _getViewPort(mainview, ui.getControl());
    CRT.Context.PageContext.setCurEntityType(entityType);
    mainview.fireEvent('ondataloaded');
    Page.setControl(ui.getControl());
    var tabId = SIE.Util.Url.getQueryValue('tabId') || ('tab_' + entityType + '-' + entity.data.Id).replace(/[.|,]/g, '');
    var tab = window.top.Ext.getCmp(tabId);
    if (tab) {
        //简单表单块可能不存在mainblock
        if (meta.mainBlock) {
            tab.setTitle(tab.title || ((CRT.Context.PageContext.isNew() ? '新增'.t() : '编辑'.t()) + '-' + meta.mainBlock.label));
        } else {
            tab.setTitle(tab.title || ((CRT.Context.PageContext.isNew() ? '新增'.t() : '编辑'.t()) + '-' + meta.label));
        }
    }
    Page.onLoad();
}

function _getViewPort(view, control) {
    return Ext.create('Ext.container.Viewport', {
        layout: {
            type: 'border'
        },
        border: 0,
        defaults: {
            layout: 'fit'
        },
        items: {
            region: 'center',
            items: control
        },
        view: view,
        renderTo: Ext.getBody(),
        listeners: {
            afterlayout: function () {
                view.fireEvent('onshow', view);
            }
        }
    });
}

function _initCustomizePage(meta) {
    if (Page && Page.onLoad) {
        Page.onLoad(meta);
    }
}
if (typeof (CRT) == "undefined") { CRT = { __namespace: true }; }

//事件组件
CRT.Event = window.top.CRT.Event || (function (root) {

    root.clientList = root.clientList || {};

    var clientList = root.clientList,
        listen,
        fire,
        remove;

    /**
     * 订阅
     * @param {string} key 关键字
     * @param {function} fn 函数
     * @returns {} 
     */
    listen = function (key, fn) {
        if (!clientList[key]) {
            clientList[key] = [];
        }
        clientList[key].push(fn);
    };

    /**
     * 触发
     * @param {any} key 关键字 
     * @returns {} 
     */
    fire = function () {
        var key = Array.prototype.shift.call(arguments),
            fns = clientList[key];
        if (!fns || fns.length === 0) {
            return false;
        }
        for (var i = 0; fn = fns[i]; i++) {
            fn.apply(this, arguments);
        }
    };

    /**
     * 取消订阅的事件
     * @param {string} key 
     * @param {function} fn 
     * @returns {} 
     */
    remove = function (key, fn) {
        var fns = clientList[key];
        if (!fns) {
            return false;
        }
        if (!fn) {
            fns && (fns.length = 0);
        } else {
            for (var l = fns.length - 1; l >= 0; l--) {
                var _fn = fns[l];
                if (_fn === fn) {
                    fns.splice(l, 1);
                }
            }
        }
        if (clientList[key] && clientList[key].length === 0) delete clientList[key];
    };

    //公开的方法
    return {
        listen: listen,
        fire: fire,
        remove: remove
    }
})(window.top);




if (typeof (CRT) == "undefined") { CRT = { __namespace: true }; }
if (typeof (CRT.Context) == "undefined") { CRT.Context = { __namespace: true }; }
/**
 * 全局上下文
 */
CRT.Context.GlobalContext = window.top.CRT.Context.GlobalContext || (function (root) {

    if (typeof root._globalContext === "undefined") root._globalContext = {};

    /**
     * 设置全局上下文
     * @param {any} key 键值
     * @param {any} value 值
     * @example CRT.Context.GlobalContext.setContext(key,value)
     */
    function setContext(key, value) {
        root._globalContext[key] = value;
    }

    /**
     * 获取全局上下文
     * @param {any} key 键值
     * @returns {any} 对应的上下文
     * @example CRT.Context.GlobalContext.getContext(key)
     */
    function getContext(key) {
        return root._globalContext[key];
    }

    //公开的方法
    return {
        setContext: setContext,
        getContext: getContext
    };
})(window.top);


if (typeof (CRT) == "undefined") { CRT = { __namespace: true }; }
if (typeof (CRT.Context) == "undefined") { CRT.Context = { __namespace: true }; }
/**
 * 页面上下文
 */
CRT.Context.PageContext = (function () {

    var _context = {};

    /**
     * 设置当前列表选中的记录
     * @param {*} record 
     * @example CRT.Context.PageContext.setCurrentRecord(record)
     */
    function setCurrentRecord(record) {
        setContext('currentRecord', record);
    }

    /**
     * 获取列表当前选中的记录
     * @example CRT.Context.PageContext.getCurrentRecord()
     */
    function getCurrentRecord() {
        return getContext('currentRecord');
    }

    /**
     * 设置页面的logicalView
     * @param {any} view
     */
    function setLogicalView(view) {
        setContext('logicalView', view);
    }

    function setQueryView(view) {
        setContext('queryView', view);
    }

    function getQueryView() {
        return getContext('queryView');
    }

    /**
     * */
    function getLogicalView() {
        return getContext('logicalView')
    }

    function getPageUrl() {
        return location.href;
    }

    /**
     * 设置页面上下文
     * @param {any} key 键值
     * @param {any} value 值
     * @example CRT.Context.PageContext.setContext(key,value)
     */
    function setContext(key, value) {
        _context[key] = value;
    }

    /**
     * 获取页面上下文
     * @param {any} key 键值
     */
    function getContext(key) {
        return _context[key];
    }

    /**
     * 获取URL中的附加参数params
     * */
    function getParams() {
        var pStr = SIE.Util.Url.getQueryValue('params');
        return JSON.parse(pStr);
    }

    /**
     * 
     * */
    function isNew() {
        return SIE.Util.Url.getQueryValue('isNew');
    }

    function setCurEntityType(entityType) {
        setContext('curEntityType', entityType);
    }

    function getCurEntityType() {
        return getContext('curEntityType');
    }

    //公开的方法
    return {
        setContext: setContext,
        getContext: getContext,
        setCurrentRecord: setCurrentRecord,
        getCurrentRecord: getCurrentRecord,
        getParams: getParams,
        isNew: isNew,
        setLogicalView: setLogicalView,
        getLogicalView: getLogicalView,
        setCurEntityType: setCurEntityType,
        getCurEntityType: getCurEntityType,
        setQueryView: setQueryView,
        getQueryView: getQueryView
    };
})();


Ext.define('SIE.Page', {
    isDirty: false,
    isCustomize: false,
    _control: null,
    beforeLoad: function (meta) {

    },
    onInit: function (meta) {

    },
    onLoad: function () {

    },
    onShow: function (args) {

    },
    beforeClose: function () {
        //var view = CRT.Context.PageContext.getLogicalView();
        //if (view && view.getData) {
        //    var isDirty = view.getData().isDirty;
        //    if (isDirty) {
        //        var promise = new Ext.Promise(function (resolve, reject) {
        //            Ext.MessageBox.confirm("提示",
        //                "数据还未保存，是否继续退出？",
        //                function (btn) {
        //                    if (btn == "yes") {
        //                        resolve(true);
        //                    } else {
        //                        reject();
        //                    }
        //                })
        //        });
        //        var result = await promise.resolve(true);
        //        return result;
        //    } else {
        //        return true;
        //    }
        //}
    },
    onClose: function (args) {
        //////////移除刷新事件//////////
        var entityType = CRT.Context.PageContext.getCurEntityType();
        var recordId;
        var record = CRT.Context.PageContext.getCurrentRecord();
        if (record) recordId = record.getId();
        if (entityType && recordId) {
            CRT.Event.remove(entityType.split(',')[0] + '_' + recordId + "_refresh");
        } else if (entityType) {
            CRT.Event.remove(entityType.split(',')[0] + "_refresh");
        }
        //////////移除刷新事件//////////
    },
    setControl: function (control) {
        this._control = control;
    },
    getControl: function () {
        return this._control;
    }
});
if (typeof (CRT) == "undefined") { CRT = { __namespace: true }; }

//工作台
CRT.Workbench = window.top.CRT.Workbench || (function (root) {

    root._pages = root._pages || {};

    var _pages = root._pages;

    //最外层的tabPanel  //todo tabpanel取消对Ext的依赖
    function _getTabPanel() {
        return root.Ext.getCmp('centerTab');
    }

    /**
     * 添加页签
     * @param {*} tab 
     * @example  
     */
    function addTab(tab) {
        var tabPanel = _getTabPanel();
        tab = tabPanel.add(tab);
        tabPanel.setActiveTab(tab);
        return tab;
    }

    /**
     * 创建iframe
     * @param {string} opt.id 
     * @param {string} opt.src
     * @param {object} opt 
     */
    function _createFrame(opt) {
        var iframe = document.createElement('iframe');
        if (opt.src) iframe.src = opt.src;
        iframe.id = opt.id;
        iframe.name = opt.id;
        iframe.scrolling = 'auto';
        iframe.width = '100%';
        iframe.height = '100%';
        iframe.frameBorder = 0;
        return iframe;
    }

    /**
     * 添加标签页
     * @param {object} opt 选项
     * @param {string} opt.entityType 实体类型
     * @param {string} opt.title 标题
     * @param {string} opt.module 模块
     * @param {string} opt.viewGroup 视图组
     * @param {boolean} opt.isDetail 是否明细
     * @param {object} opt.tabId tabId
     * @param {object} opt.recordId 实体id
     * @param {string} opt.url url
     * @param {string} opt.isNew 是否新增
     * @param {object} opt.params 附加参数
     * @param {boolean} opt.ignoreQuery 是否忽略查询块
     * @param {string} opt.pageClass 页面脚本类，若指定了则代表全客制化页面，不走框架页面生成逻辑 
     * @example CRT.Workbench.addPage(opt)
     */
    function addPage(opt) {
        var tabPanel = _getTabPanel();
        var tabId = getTabId(opt);
        if (root.Ext.getCmp(tabId)) { tabPanel.setActiveTab(root.Ext.getCmp(tabId)); return; }
        var url = '/#';
        var rootPath = '/#';
        var suffix = SIE.Util.Url.getPageUriSuffix(opt);
        if (opt.url) {
            //对于url访问的页签菜单
            url = SIE.Util.Url.urlConvert(opt.url);
        } else if (suffix.length > 0) {
            url = '/page' + '?' + suffix;
            rootPath = root.location.protocol + '//' + root.location.host + '#' + suffix;
            SIE.Util.Url.setRootPath(rootPath);
        }
        var iframe = _createFrame({
            id: tabId + '_iframeEl',
            src: url
        });

        var tab = {
            id: tabId,
            border: 0,
            title: opt.title,
            layout: 'fit',
            margin: 3,
            closable: true,
            getFrame: function () {
                return iframe;
            },
            url: rootPath
        }
        tab.contentEl = iframe;
        tab = addTab(tab);
        opt.notload ? true : tab.setLoading(true);
        iframe.onload = function (e) {
            tab.setLoading(false);
            SIE.App.fireEvent('moduleCreated', {
                moduleOpt: opt,
                moduleName: opt.title
            });
        }
    }

    /**
     * 弹窗访问html页面，带post请求
     * @param {string} url-url地址
     */
    function showPageDialog(opt) {
        var rawId = opt.mid || opt.id;
        var isLoadedClose = opt.isLoadedClose || false;
        var winId = ('win_' + rawId).replace(/[.|,]/g, '');
        var win = Ext.getCmp(winId);
        if (!win) {

            var url = opt.url || opt.Url;
            if (url) {
                url = SIE.Util.Url.urlConvert(url);
                var iframeId = winId + '_iframeEl';
                //win.html = Ext.String.format('<iframe  id="{0}" name="{0}" data-ref="iframeEl" width="100%" height="99%" scrolling="auto" frameborder="0"  ></iframe>', iframeId);
                var iframe = _createFrame({
                    id: iframeId,
                    src: opt.method == "POST" ? "" : url
                });
                win = new Ext.Window({
                    modal: true,
                    title: opt.text,
                    id: winId,
                    maximizable: true,
                    monitorResize: true,
                    draggable: false, // 禁止移动   
                    resizable: false,
                    maximizable: true, // 禁止最大化   
                    layout: 'fit',
                    plain: true,
                    buttonAlign: 'right',
                    listeners: {
                        close: function (w) {
                            w.restore(); // 关闭窗口前先还原,滚动条才不会消失   
                        },
                        beforeclose: function () {
                            if (iframe.contentWindow.beforeClose) {
                                return iframe.contentWindow.beforeClose();
                            }
                            return true;
                        },
                        maximize: function (w) {
                            //最大化后需要将窗口重新定位，否则窗口会从最顶端开始最大化   
                            w.setPosition(document.body.scrollLeft, document.body.scrollTop);
                        }
                    }
                });
                win.view = opt.view;
                win.contentEl = iframe;
                win = win.show();
                if (opt.method == "POST") {
                    var tempForm = document.createElement("form");
                    //制定发送请求的方式为post  
                    tempForm.method = opt.method || "GET";
                    //此为window.open的url，通过表单的action来实现  
                    tempForm.action = url;
                    //利用表单的target属性来绑定window.open的一些参数（如设置窗体属性的参数等）  
                    tempForm.target = iframeId;
                    if (opt.params) {
                        for (var i in opt.params) {
                            var hideInput = document.createElement("input");
                            hideInput.type = "hidden";
                            hideInput.name = i;
                            hideInput.value = opt.params[i];
                            //将input表单放到form表单里  
                            tempForm.appendChild(hideInput);
                        }
                    }

                    //将此form表单添加到页面主体body中  
                    document.body.appendChild(tempForm);
                    //手动触发，提交表单  
                    tempForm.submit();
                    //从body中移除form表单  
                    document.body.removeChild(tempForm);
                }
                win.setLoading(true);
                document.getElementById(iframeId).onload = function () {
                    win.setLoading(false);
                    if (isLoadedClose) {
                        win.close();
                    }
                }
            }
            winAutoSize(win);
        }
    }

    /**
  * 弹窗自适应大小
  * @param {win} win-win弹窗
  * @param {winId} winId-winId弹窗标识
  */
    function winAutoSize(win) {
        var winId = win.getId();
        win.setPosition(20, 20);
        win.fitContainer(); // 填充满浏览器   
        Ext.on('resize', function (a, b) {
            var win = Ext.getCmp(winId);
            if (win == undefined) {
                return;
            }
            win.setPosition(20, 20);
            win.fitContainer();
        });
    }

    function getTabId(opt) {
        if (opt.tabId) { return opt.tabId }
        var rawId, tabId;
        if (opt.entityType && opt.recordId) {
            rawId = opt.entityType + '-' + opt.recordId;
        } else if (opt.entityType) {
            rawId = opt.entityType;
        }
        tabId = ('tab_' + rawId).replace(/[.|,]/g, '');
        return tabId;
    }

    //通过url加载页面
    function loadPageFromUrl() {
        var kvs = SIE.Util.Url.getUrlKeyValues();
        if (kvs) addPage(kvs);
    }

    /**
     * 获取当前激活的tab 
     */
    function getActiveTab() {
        var tabPanel = _getTabPanel();
        return tabPanel.getActiveTab();
    }

    /**
     * 关闭tab //todo待实现
     * @param {object} tab 
     */
    function closeTab(tab) {
        var tabPanel = _getTabPanel();
        if (tab) {
            tabPanel.remove(tab);
        }
    }

    function cloaseTabById(tabId) {
        var tab = root.Ext.getCmp(tabId);
        tab.closeTab();
    }

    /**
     * 激活tab //TODO 待实现
     * @param {*} tab 
     */
    function activateTab(tab) {

    }

    /**
     * 关闭当前页签 //todo 待实现
     */
    function closeCurrentTab() {
        var tabPanel = _getTabPanel();
        var activeTab = tabPanel.getActiveTab();
        tabPanel.remove(activeTab);
    }

    /**
     * 通过tabId获取tab
     * @param {any} tabId
     */
    function getTabById(tabId) {
        return root.Ext.getCmp(tabId);
    }

    function getTabPanel() {
        return _getTabPanel();
    }

    /**
     * 激活tab标签
     * @param {*} tab String / Number / Ext.Component
     */
    function activeTab(tab) {
        var tabPanel = _getTabPanel();
        tabPanel.setActiveTab(tab);
    }

    //公开的方法
    return {
        addTab: addTab,
        addPage: addPage,
        showPageDialog: showPageDialog,
        loadPageFromUrl: loadPageFromUrl,
        closeTab: closeTab,
        getTabPanel: getTabPanel,
        cloaseTabById: cloaseTabById,
        getTabById: getTabById,
        activeTab: activeTab,
        getTabId: getTabId,
        closeCurrentTab: closeCurrentTab
    };
})(window.top);



