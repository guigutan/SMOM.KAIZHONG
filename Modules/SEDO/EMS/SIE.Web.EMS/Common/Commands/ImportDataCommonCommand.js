SIE.defineCommand('SIE.Web.EMS.Common.Commands.ImportDataCommonCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "数据导入", group: "business", iconCls: "icon-Download icon-blue", model: "" },

    _pageSize: 25, //  默认值

    /**
   * @override 初始化下载模板类型
   * BehaviorName=‘Download’：下载默认类型模板，BehaviorName=“DownloadCustom”下载自定义列模板
   * @returns  grid
   */
    _downloadTemplateType: function () {
        this.BehaviorName = "DownloadTemplate";
    },

    _gridHeaderJsonText: null,

    /**
    *@override 创建导入面板--打开窗体
    * @param {*} myview
    * @returns ，
    */
    creatImportWindow: function (myview) {
        var me = this;
        //创建一个表单面板
        var form = me.creatImportFormPanel(myview);
        //创建一个网格列表面板
        var grid = me.creatImportGridPanel();

        me._progressBar = new Ext.ProgressBar({
            renderTo: Ext.getBody(),
            width: 988,
        });

        //点击弹出框
        var win = Ext.create("Ext.window.Window", {
            title: "导入Excel".t(), //标题
            draggable: false,
            height: 510, //高度
            width: 1000, //宽度
            maxWidth: 1000, //宽度
            constrain: true,//设置窗体活动区域不能超过浏览器内容区域
            modal: true, //是否模态窗口，默认为false
            resizable: false,
            items: [form, grid, me._progressBar],
            autoScroll: true,
        });
        me._progressBar.hide();
        return win;
    },

    /**
    * @override 创建导入面板--表单
    * @param {*} myview
    * @returns ，form
    * 子类可以根据具体情况覆写，创建不同的面板
    */
    creatImportFormPanel: function (myview) {
        var me = this;
        var form = new Ext.form.FormPanel({
            bodyStyle: 'padding:5px 5px 0',
            frame: true,
            maxWidth: 980,
            border: true,
            items: [{
                xtype: 'button',
                id: 'templatebutton',
                iconCls: 'iconfont icon-Download',
                text: '下载模板'.t(),
                handler: function () {
                    myview.execute({
                        data: {
                            BehaviorName: me.BehaviorName,
                            Type: myview.model
                        },
                        success: function (res) {
                            me.onDownloadTemplateSuccess(res);
                        }
                    })
                }
            }, {
                xtype: 'filefield',
                id: 'filefield',
                name: 'fileUpload',
                fieldLabel: '请选择导入文件'.t(),
                reference: 'basicFile',
                anchor: '100%',
                buttonText: '...',
                listeners: {
                    change: function (field, newValue) {
                        var fileExt = newValue.substring(newValue.lastIndexOf(".")).toLowerCase();
                        if (fileExt != '.xlsx' && fileExt != '.xls') {
                            Ext.Msg.alert('提示'.t(), '非xls,xlsx格式文件不支持导入'.t());
                            return;
                        }
                        //获取文件对象
                        var file = field.fileInputEl.dom.files.item(0);
                        var fileReader = new FileReader('file://' + newValue);
                        fileReader.readAsDataURL(file);
                        fileReader.onload = function (e) {
                            //SIE.Msg.wait('数据正在导入中，请稍候...');
                            me._progressBar.show();
                            me._progressBar.wait({
                                interval: 100,
                                duration: 36000000,
                                text: '数据正在导入中，请稍候...'.t(),
                                increment: 10,
                                scope: this,
                                fn: function () {

                                }
                            });
                            var parent = myview.getParent() != null && myview.getParent().getCurrent() != null ? myview.getParent().getCurrent().data : null;
                            myview.execute({
                                data: {
                                    BehaviorName: 'ImportData',
                                    Type: myview.model,
                                    SelectedParent: parent != null ? Ext.encode(parent) : null,
                                    SelectedParentId: parent != null ? parent.Id : 0,
                                    Data: e.target.result,
                                    ViewGroup: myview.viewGroup
                                },
                                success: function (res) {
                                    //导入模板成功后处理数据
                                    me._importExcelCallback(res, myview);
                                    //SIE.Msg.hide();
                                    me._progressBar.hide();
                                    SIE.Msg.showMessage(res.Result.ImportMsg);
                                }
                            });
                        }
                    },
                    render: function () {
                        Ext.fly(this.el).on('click', function (e, t) {
                            //重置防止选择同一个文件
                            t.value = '';
                        });
                    }
                }
            }, {
                xtype: 'textfield',
                id: 'msgtextfield',
                fieldLabel: '导入处理的消息'.t(),
                readOnly: true,
                anchor: '100%',
            }]
        });
        return form;
    },

    /**
     *   请求下载模板成功后处理
     * @param {any} res
     */
    onDownloadTemplateSuccess: function (res) {
        var filePath = res.Result.FilePath;
        var url = window.location.origin + "/" + filePath;
        window.open(url);
    },

    /**
     * 创建导入面板--Grid
     * @returns  grid
     */
    creatImportGridPanel: function () {
        var me = this;
        if (!this.meta.model) {
            Ext.Msg.alert('提示'.t(), '未指定当前模型。'.t());
            return;
        }
        SIE.AutoUI.getMeta({
            model: this.meta.model,
            viewGroup: this.view.viewGroup,
            async: false,
            title: '导入失败数据'.t(),
            callback: function (res) {
                me.viewMeta = res;
            }
        });
        //动态Jsonstore格式
        var FailedLineNumber = "失败行号".t();
        var columnModleText = Ext.String.format("{\"text\":\"{0}\",\"dataIndex\":\"index\"}", FailedLineNumber);
        var fieldNamesText = '{\"name\":\"index\"}';
        this.viewMeta.gridConfig.columns.forEach(function (column) {
            columnModleText += Ext.String.format(",{\"text\":\"{0}\",\"dataIndex\":\"{1}\"}", column.header, column.header);
            fieldNamesText += Ext.String.format(",{\"name\":\"{0}\"}", column.header);
        });
        this._gridHeaderJsonText = Ext.String.format("{\"total\":\"0\",\"data\":[{\"index\":\"\"}],\"columnModle\":[{0}],\"fieldsNames\":[{1}]}", columnModleText, fieldNamesText);
        var json = Ext.util.JSON.decode(this._gridHeaderJsonText);
        //创建strore对象
        var store = new Ext.data.Store({
            proxy: new Ext.data.MemoryProxy(null),
            fields: json.fieldsNames,
            data: json.data,
            totalProperty: json.total,
            pageSize: me._pageSize
        });

        //创建动态JsonStore表格
        var importColumns = json.columnModle;
        var bbar = new SIE.control.GridPager({
            id: 'failedtoolbar',
            xtype: 'gridpager',
            store: store,//数据
            displayInfo: true,//是否显示数据信息
            displayMsg: '显示{0}-{1}条记录,共{2}条',//只有displayInfo:true时才有效，用来显示有数据的提示信息。
            emptyMsg: "没有记录".t(),//没有数据显示的信息,
            items: [
                {
                    xtype: 'combobox',
                    itemId: 'pageSizeItem',
                    store: Ext.create('Ext.data.Store', {
                        fields: ['value'],
                        data: [
                            { "value": 25 },
                            { "value": 50 },
                            { "value": 100 },
                            { "value": 1000 },
                            { "value": 5000 },
                        ]
                    }),
                    listeners: {
                        change: function (clt, newValue, oldValue, eOpts) {
                            var arrtydata = [];
                            var toolbar = Ext.getCmp('failedtoolbar');
                            toolbar.store.setPageSize(newValue);
                            var pageData = toolbar.getPageData();
                            for (var i = pageData.fromRecord - 1; i <= pageData.toRecord - 1; i++) {
                                arrtydata.push(toolbar.store.data.items[i]);
                            }
                            Ext.getCmp('failedGrid').store.setData(arrtydata);
                        }
                    },
                    value: me._pageSize,
                    width: 72,
                    minValue: 0,
                    maxValue: 5000,
                    queryMode: 'local',
                    displayField: 'value',
                    valueField: 'value',
                }
            ],
            listeners: {
                change: {
                    fn: function (clt, newValue, oldValue, eOpts) {
                        var arrtydata = [];
                        for (var i = this.getPageData().fromRecord - 1; i < this.getPageData().toRecord - 1; i++) {
                            arrtydata.push(this.store.data.items[i]);
                        }
                        Ext.getCmp('failedGrid').store.setData(arrtydata);
                    }
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
                me.moveFirst();
            },
        });

        var grid = Ext.create("Ext.grid.Panel", {
            id: 'failedGrid',
            name: 'failedGrid',
            title: '导入失败数据'.t(),
            xtype: 'grid-filtering',
            height: 350, //高度 
            maxWidth: 988,
            columns: importColumns,
            bodyStyle: 'overflow-x:hidden; overflow-y:hidden',
            store: store,
            tbar: [{
                xtype: 'button',
                id: 'Importbutton',
                text: '导出Excel'.t(),
                handler:
                    function () {
                        var grid = Ext.getCmp('failedGrid');
                        if (grid.store.config.totalProperty === '0') {
                            SIE.Msg.showMessage('没有出错数据！'.t());
                            return;
                        }
                        //var me =this;
                        var fieldNames = [];
                        grid.getStore().config.fields.forEach(
                            function (item) {
                                var fieldName = {};
                                fieldName.key = item.name;
                                fieldName.header = item.name === '_Index' ? '失败行号'.t() : item.name;
                                fieldNames.push(fieldName);
                            });
                        var recordData = [];

                        Ext.each(grid.getStore().getRange(), function (record) {
                            recordData.push(record.data);
                        });
                        var exportJsonData = [];
                        recordData.forEach(function (row) {
                            var fieldData = '';
                            fieldNames.forEach(function (fieldName) {
                                var exportValue = row[fieldName.key];
                                fieldData += '\"' + fieldName.key + '\":\"' + (exportValue === null ? '' : exportValue) + '\",';
                            });
                            var fieldDataStr = '{' + fieldData.substr(0, fieldData.length - 1) + '}';
                            exportJsonData.push(JSON.parse(fieldDataStr.replace(/\n/g, "\\n").replace(/\r/g, "\\r")));
                        });

                        var exportJsonHeaders = [];
                        fieldNames.forEach(function (value) {
                            exportJsonHeaders.push(value.header === '_MessageTip' ? '失败原因'.t() : value.header)
                        });
                        me.jSONToExcelConvertor(exportJsonData, myview.label + Ext.util.Format.date(new Date(), 'Ymdhis'), exportJsonHeaders);
                    },
            }],
            bbar: bbar,
        });

        return grid;
    },

    /**
  * 导入模板后回调函数--importExcelCallback
  * @returns  grid
  * 默认设置导入结果，如果导入数据成功，刷新视图，否则展示未导入成功数据
  * 子类可以根据具体情况覆写，自行处理导入后的结果处理
  */
    _importExcelCallback: function (res, view) {
        var me = this;
        //设置导入结果
        Ext.getCmp('msgtextfield').setValue(res.Result.ImportMsg);
        if (res.Result.FailedJson.length > 0) {
            var failedJson = Ext.util.JSON.decode(res.Result.FailedJson);
            var arrtydata = [];
            var num = failedJson.data.length > this.getPageSize() ? this.getPageSize() : failedJson.data.length;
            for (var i = 0; i < num; i++) {
                arrtydata.push(failedJson.data[i]);
            }
            var failedStore = new Ext.data.Store({
                proxy: new Ext.data.MemoryProxy(failedJson.data),
                fields: failedJson.fieldsNames,
                totalProperty: "total",
                data: failedJson.data,
                pageSize: this._pageSize
            });
            var store = new Ext.data.Store({
                proxy: new Ext.data.MemoryProxy(arrtydata),
                fields: failedJson.fieldsNames,
                totalProperty: "total",
                data: arrtydata
            });
            Ext.getCmp('failedtoolbar').setStore(failedStore);
            Ext.getCmp('failedGrid').reconfigure(store, failedJson.columnModle);
        }
        else {
            var json = Ext.util.JSON.decode(this._gridHeaderJsonText);
            //创建strore对象
            var store = new Ext.data.Store({
                proxy: new Ext.data.MemoryProxy(null),
                fields: json.fieldsNames,
                data: json.data,
                totalProperty: json.total,
                pageSize: me._pageSize
            });
            Ext.getCmp('failedtoolbar').setStore(store);
            Ext.getCmp('failedGrid').reconfigure(store, json.columnModle);
        }
        if (res.Result.ImportSuccessNum && res.Result.ImportSuccessNum > 0) {
            me._importSuccess(view);
        }
    },

    //数据页大小
    getPageSize: function () {
        var pagingBar = Ext.getCmp('failedtoolbar');
        if (pagingBar && !Ext.isEmpty(pagingBar._pageSize)) {
            return pagingBar._pageSize;
        }
        else {
            return this._pageSize;
        }
    }
});