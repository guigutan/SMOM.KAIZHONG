SIE.defineCommand('SIE.Web.Inventory.Transactions.Commands.InitFunctionCommand', {
    meta: { text: "功能初始化", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    execute: function (view, source) {
        var me = this;
        me.view = view;
        me._getMeta(view, "SIE.Web.Inventory.Transactions.ViewModels.FunctionModel");
    },

    _queryBlockProcess: function (block) {
        /// <summary>
        /// 查询块处理-只读为false
        /// </summary>
        /// <param name="block" type="type"></param>
        if (block.surrounders) {
            var surround = block.surrounders["0"];
            if (surround) {
                var items = surround.mainBlock.formConfig.items;
                for (var i = 0, len = items.length; i < len; i++) {
                    var item = items[i];
                    item.readOnly = false;
                }
            }
        }
    },
    /*
    * 获取元数据 
    * @param listView 列表视图
    * @param type 查询所在类名
    * @param filter 查询参数及方法名
    */
    _getMeta: function (view, model) {
        var me = view;
        var thisMe = this;
        SIE.AutoUI.getMeta({
            model: "SIE.Web.Inventory.Transactions.ViewModels.FunctionModel",
            module: "SIE.Inventory.Transactions.Function,SIE.Inventory",
            ignoreCommands: false,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                thisMe._queryBlockProcess(blocks);
                thisMe._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                var listView = ui.getView();
                var items = ui.getControl();
                var win = SIE.Window.show({
                    title: '初始化单据大类'.t(),
                    items: items,
                    height: document.body.clientHeight * 0.8,
                    width: document.body.clientWidth * 0.5,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            if (listView.getSelection().length > 0) {
                                var indata = {};
                                var selections = listView.getSelection();
                                if (selections && selections.length > 0) {
                                    var operationDatas = [];
                                    SIE.each(selections, function (item) {
                                        var funcData = { Code: item.getCode(), Name: item.getName() };
                                        operationDatas.push(funcData);

                                    });
                                    indata = operationDatas;
                                    me.execute({
                                        data: indata,
                                        success: function (res) {
                                            win.close();  //关闭模态窗口
                                            me.reloadData();
                                        }
                                    }, me._ownerView);
                                }

                            } else {
                                SIE.Msg.showWarning('没有可提交的数据'.t());
                                return false;
                            }
                        }
                    }
                });
                thisMe.beforeLoadData(listView);
            }
        });
    },

    /**
     * 设置grid的块配置
     * @param block grid块配置
     */
    _gridBlockProcess: function (block) {
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
            mode: 'MULTI' //(multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
        };

        gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: false,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };
    },

    /*
     * 加载数据前，处理参数 
     * @param listView 列表视图
     * 此方法可重写
     */
    beforeLoadData: function (listView) {
        var me = this;
        var Parameters = [];
        Parameters.push("");
        Parameters.push("");
        var filter = {
            Method: 'GetInitFunctions',
            Parameters: Parameters
        };
        var type = 'SIE.Web.Inventory.Transactions.DataQueryer.FunctionDataQuery';
        me._tryLoadData(listView, type, filter);
    },
    /*
    * 加载列表数据 
    * @param listView 列表视图
    * @param type 查询所在类名
    * @param filter 查询参数及方法名
    */
    _tryLoadData: function (listView, type, filter) {
        var me = this;
        filter = Ext.encode(filter);
        listView.loadData({
            filter: filter,
            action: 'queryer',
            type: type,
            token: me.view.token
        });
    },
});