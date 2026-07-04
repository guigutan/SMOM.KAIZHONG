
Ext.define('Portal.ModuleBase', {
    extend: 'Ext.panel.Panel',
    xtype: 'ModuleBase',
    config: {
        inputParams: [],//输入参数集合
        outputParams: [],//输出参数集合
        bindOutPutParam: new Map(),//绑定输出参数绑定输入参数键值对
        moduleName:''
    },
    globalOutputParams:[],
    initComponent: function () {
        var me = this;
        me.initParam();
        if(me.getModuleName()!='')
            me.getRefOwner().setTitle(me.getModuleName());

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
    initParam: function () { },
    refreshData: function () { },
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