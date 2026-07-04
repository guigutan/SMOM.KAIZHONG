/**
 * 产品族生命周期，添加报工规则设置页签
 * @class SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior
 * @constructor
 */
Ext.define('SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior', {

    /**
     * 产品族主视图
     * @param {DetailView} mainView
     */
    mainView: null,

    /**
     * 规则配置控件
     * @param {Ext.form.Panel} configPanel
     */
    configPanel: null,

    /**
     * 产品族
     * @param {productFamily} productFamily
     */
    productFamily: null,

    /**
     * 规则配置字典：key产品族，value规则配置
     * @param {字典} dicConfig
     */
    dicConfig: {},

    /**
     * 规则配置字典：key产品族，value任务单生成规则配置
     * @param {字典} dicTaskConfig
     */
    dicTaskConfig: {},

    /**
     * 数据是否已经加载
     * @param {Boolean} isLoaded
     */
    isLoaded: true,

    /**
    * 数据是否已经加载
    * @param {Boolean} isLoaded
    */
    isValidate: true,

    /**
     * 视图创建后方法
     * @method onCreated
     * @param {ListLogicView} view 产品族视图
     */
    onCreated: function (view) {
        var me = this;
        me.mainView = view;
        me.interceptSaveCmd(view);
        view.mon(view, 'currentChanged', me.productFamilyPropertyChanged, me);
    },

    /**
     * 视图关联后方法
     * @method onViewReady
     * @param {ListLogicView} view 产品族视图
     */
    onViewReady: function (view) {
        var me = this;
        var printControl = null;
        if (view._children.length <= 0)
            return;
        var tabPanel = view._children[0].getControl().up('tabpanel');
        var printLayout = Ext.Array.findBy(view._children, function (item) {
            if (item.model == 'SIE.MES.TaskManagement.Reports.ReportPrintConfig') { return true; }
        });

        if (!printLayout)
            SIE.Msg.showMessage("请配置产品族报工打印设置权限!".t());
        else
            printControl = printLayout.getControl();

        me.taskConfigPanel = me.createTaskConfigControl(me);

        me.configPanel = me.createRuleConfigControl(printControl);
        me.configPanel.lifeCycle = me;
        me.configPanel.mainView = me.mainView;
        me.taskConfigPanel.lifeCycle = me;
        me.taskConfigPanel.mainView = me.mainView;

        //删除打印设置页签
        var printTabPanel = Ext.Array.findBy(tabPanel.items.items, function (item) {
            if (item.title == '报工打印设置'.t()) { return true; }
        });
        tabPanel.remove(printTabPanel);

        if (tabPanel) {
            tabPanel.clearListeners(); //清除框架的tabchange事件，因为第一个标签页为自定义控件，不需要框架加载数据
            tabPanel.mon(tabPanel, 'tabchange', me.tabchange, this);
            tabPanel.insert(1, me.taskConfigPanel);
            tabPanel.insert(tabPanel.items.length, me.configPanel);
        }
    },

    /**
     * 创建报工规则配置控件
     * @method createRuleConfigControl
     * @returns {Ext.form.Panel}  报工规则配置控件
     */
    createRuleConfigControl: function (printControl) {
        return Ext.create('Ext.form.Panel', {
            title: '报工参数配置'.t(),
            id: 'ruleConfigCtl',
            border: 0,
            margin: 10,
            autoScroll: true,
            viewModel: {
                type: 'ruleConfig'
            },
            items: [{
                xtype: 'fieldset',
                title: '报工方式'.t(),
                layout: 'anchor',
                defaults: {
                    anchor: '100%',
                    componentCls: ""
                },
                width: 500,
                items: [{
                    xtype: 'radiogroup',
                    bind: {
                        value: '{ReportMode}',
                        disabled: '{reportDisabled}'
                    },
                    simpleValue: true,
                    defaults: {
                        name: 'reportMode',
                        margin: '0 15 0 0'
                    },
                    items: [{
                        boxLabel: '自动'.t(),
                        bind: {
                            disabled: '{autoDisabled}'
                        },
                        inputValue: 0
                    }, {
                        boxLabel: '手动'.t(),
                        bind: {
                            disabled: '{handDisabled}'
                        },
                        inputValue: 1
                    }],
                    onChange: function (newVal) {
                        var me = Ext.getCmp('ruleConfigCtl').lifeCycle;
                        if (newVal === 0) {
                            //选择自动，报工数量按默认值方式
                            Ext.getCmp('reportQtyCtl').setValue(0);
                            //修改绑定数据无效？
                            //var model = me.configPanel.getViewModel().data;
                            //model.ModReport = 0;
                            //me.configPanel.getViewModel().setData(model);
                        }
                        me.setSaveCmdEnable();
                    }
                }]
            },
                {
                xtype: 'fieldset',
                title: '报工数量'.t(),
                layout: 'anchor',
                width: 500,
                defaults: {
                    anchor: '100%',
                    componentCls: ""
                },
                items: [{
                    xtype: 'panel',
                    border: false,
                    layout: {
                        type: 'hbox',
                        align: 'stretch',
                        pack: 'start'
                    },
                    items: [{
                        xtype: 'radiogroup',
                        width: 300,
                        height: 35,
                        simpleValue: true,  // set simpleValue to true to enable value 
                        bind: {
                            value: '{ModReport}',
                            disabled: '{modDisabled}'
                        },
                        id: 'reportQtyCtl',
                        items: [{
                            boxLabel: '按默认值'.t(),
                            bind: {
                                disabled: '{defaultDisabled}'
                            },
                            inputValue: 0
                        },
                        {
                            xtype: 'numberfield',
                            id: 'reportQtyId',
                            name: 'basic',
                            value: 1,
                            minValue: 1,
                            maxHeight: 10,
                            maxWidth: 80,
                            bind: {
                                value: '{ReportQty}',
                                disabled: '{reportQtyDiasble}'
                            },
                            listeners: [{
                                change: function (control, newValue, oldValue, eOpts) {
                                    Ext.getCmp('ruleConfigCtl').lifeCycle.setSaveCmdEnable();
                                    if (newValue <= 0) {
                                        Ext.getCmp('reportQtyId').setValue(1);
                                    }
                                }
                            }]
                        }, {

                            boxLabel: '按剩余可报数'.t(),
                            bind: {
                                disabled: '{modRportDisabled}'
                            },
                            inputValue: 1,
                        }
                        ],
                        onChange: function (newVal) {
                            Ext.getCmp('ruleConfigCtl').lifeCycle.setSaveCmdEnable();
                        }
                    },]
                }]
                },
                {
                    xtype: 'fieldset',
                    title: '物料倒扣计算设置'.t(),
                    layout: 'anchor',
                    width: 500,
                    defaults: {
                        anchor: '100%',
                        componentCls: ""
                    },
                    items: [{
                        xtype: 'checkboxfield',
                        bind: {
                            value: '{IsExpendItem}',
                            disabled: '{isExpendItemDiasble}'
                        },
                        name: 'isExpendItem',
                        boxLabel: '不合格报工数量是否耗用物料'.t(),
                        listeners: [{
                            change: function (control, newValue, oldValue, eOpts) {
                                Ext.getCmp('ruleConfigCtl').lifeCycle.setSaveCmdEnable();
                            }
                        }]
                    }]
                },
                {
                    xtype: 'fieldset',
                    title: '共模报工'.t(),
                    layout: 'anchor',
                    width: 500,
                    defaults: {
                        anchor: '100%',
                        componentCls: ""
                    },
                    items: [{
                        xtype: 'checkboxfield',
                        bind: {
                            value: '{IsSyntype}',
                            disabled: '{syntypeDiasble}'
                        },
                        name: 'syntype',
                        boxLabel: '共模任务必须按共模比报工'.t(),
                        listeners: [{
                            change: function (control, newValue, oldValue, eOpts) {
                                Ext.getCmp('ruleConfigCtl').lifeCycle.setSaveCmdEnable();
                            }
                        }]
                    }]
                },
                {
                    xtype: 'panel',
                    //title: '共模报工'.L10N(),
                    border: "0",
                    layout: 'anchor',
                    width: 500,
                    defaults: {
                        anchor: '100%',
                        componentCls: ""
                    },
                    items: printControl,
                }
                ]
            });
    },

    /**
     * 创建任务单生成配置控件
     * @method createTaskConfigControl
     * @returns {Ext.form.Panel}  任务单生成配置控件
     */
    createTaskConfigControl: function (me) {
        return Ext.create('Ext.form.Panel', {
            title: '任务单生成配置项'.t(),
            border: 0,
            margin: 10,
            bodyPadding: 10,
            layout: 'form',
            id: 'taskConfigId',
            viewModel: {
                type: 'taskConfig'
            },
            items: [{
                xtype: 'fieldcontainer',
                combineErrors: false,
                layout: 'hbox',
                defaults: {
                    hideLabel: true,
                    margin: '0 0 0 0'
                },
                items: [{
                    xtype: 'checkboxfield',
                    name: 'byProcess',
                    bind: {
                        value: '{ByProcess}',
                        disabled: '{byProcessDisabled}'
                    },
                    fieldLabel: 'Checkbox',
                    boxLabel: '按照工序生成任务单'.t(),
                    minValue: 0,
                    width: 150,
                    margin: '20 150 20 20',
                    allowBlank: false,
                    listeners: [{
                        change: function (control, newValue, oldValue, eOpts) {
                            me.isValidate = true;
                            Ext.getCmp('taskConfigId').lifeCycle.setSaveCmdEnable();
                        }
                    }]
                }, {
                    xtype: 'checkboxfield',
                    name: 'bySpecification',
                    bind: {
                        value: '{BySpecification}',
                        disabled: '{bySpecificationDisabled}'
                    },
                    fieldLabel: 'Checkbox',
                    boxLabel: '按照规格件生成任务单'.t(),
                    minValue: 0,
                    width: 150,
                    margin: '20 150 20 0',
                    allowBlank: false,
                    listeners: [{
                        change: function (control, newValue, oldValue, eOpts) {
                            me.isValidate = true;
                            Ext.getCmp('taskConfigId').lifeCycle.setSaveCmdEnable();
                        }
                    }]
                }, {
                    xtype: 'checkboxfield',
                    name: 'byQty',
                    bind: {
                        value: '{ByQty}',
                        disabled: '{byQtyDisabled}'
                    },
                    fieldLabel: 'Checkbox',
                    boxLabel: '按照固定数量生成任务单'.t(),
                    minValue: 0,
                    width: 170,
                    margin: '20 0 20 0',
                    allowBlank: false,
                    listeners: [{
                        change: function (control, newValue, oldValue, eOpts) {
                            me.isValidate = true;
                            Ext.getCmp('taskConfigId').lifeCycle.setSaveCmdEnable();
                        }
                    }]
                }, {
                    name: 'qty',
                    bind: {
                        value: '{Qty}',
                        disabled: '{qtyDisabled}'
                    },
                    xtype: 'numberfield',
                    minValue: 0,
                    width: 95,
                    margin: '20 0 20 0',
                    allowBlank: false,
                    listeners: [{
                        change: function (control, newValue, oldValue, eOpts) {
                            me.isValidate = true;
                            Ext.getCmp('taskConfigId').lifeCycle.setSaveCmdEnable();
                        }
                    }]
                }]
            }, {
                xtype: 'fieldcontainer',
                combineErrors: false,
                layout: 'hbox',
                defaults: {
                    hideLabel: true,
                    margin: '0 0 0 0'
                },
                items: [{
                    xtype: 'checkboxfield',
                    name: 'byVirtualPart',
                    bind: {
                        value: '{ByVirtualPart}',
                        disabled: '{byVirtualPartDisabled}',
                    },
                    fieldLabel: 'Checkbox',
                    boxLabel: '是否生成虚拟件任务单'.t(),
                    minValue: 0,
                    width: 150,
                    margin: '0 0 0 20',
                    allowBlank: false,
                    listeners: [{
                        change: function (control, newValue, oldValue, eOpts) {
                            me.isValidate = true;
                            Ext.getCmp('taskConfigId').lifeCycle.setSaveCmdEnable();
                        }
                    }]
                }]
            }]
        });
    },

    /**
     * 拦截产品族保存命令
     * @method interceptSaveCmd
     * @param {ListLogicView} view 产品族视图
     */
    interceptSaveCmd: function (view) {
        var me = this;
        if (view._control.dockedItems.items.first(function (p) { return p.xtype === "toolbar"; }) == undefined) {
            return;
        }
        var btnSave = view._control.dockedItems.items[2].items.items.first(function (p) { return p.command === "SIE.cmd.Save"; });
        var saveCmd = view._commands.items.first(function (p) { return p.meta.command === "SIE.cmd.Save"; });
        if (!btnSave || !saveCmd)
            return;
        btnSave.handler = function () {
            me.isValidate = true;
            me.saveTempTaskConfig(me.productFamily);
            var configs = me.initTaskConfigs();
            var result = Ext.JSON.encode(configs);
            SIE.invokeDataQuery({
                type: "SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer",
                method: "ValidateTaskConfigs",
                token: me.mainView.token,
                params: [result],
                success: function (res) {
                    var errMsg = res.Result;
                    if (errMsg.length > 0) {
                        me.isValidate = false;
                        me.dicTaskConfig = {};
                        SIE.Msg.showMessage(errMsg);
                        return;
                    }

                    saveCmd.tryExecute(btnSave);
                    me.saveReportRuleConfig();
                    me.saveTaskConfig();

                    if (!me.productFamily)
                        return;
                    if (me.dicTaskConfig[me.productFamily.Id]) {
                        me.setTaskRuleConfig(me.dicTaskConfig[me.productFamily.Id]);
                    }
                    if (me.dicConfig[me.productFamily.Id]) {
                        me.setReportRuleConfig(me.dicConfig[me.productFamily.Id]);
                    }
                }
            });
        };
    },

    /**
     * 产品族属性变更事件
     * @method productFamilyPropertyChanged
     * @param {object} arg 属性变更参数 
     */
    productFamilyPropertyChanged: function (arg) {
        var me = this;
        me.productFamily = arg.newValue;
        if (arg.newValue && arg.oldValue && arg.newValue.getId() === arg.oldValue.getId())
            return;
        me.saveTempConfig(arg.oldValue);
        if (me.isValidate == true)
            me.saveTempTaskConfig(arg.oldValue);

        me.clearReportRuleConfig(arg.newValue, me.configPanel);
        me.clearTaskRuleConfig(arg.newValue, me.taskConfigPanel);
        if (arg.newValue) {
            var familyId = arg.newValue.getId();
            if (me.dicConfig[familyId]) {
                me.setReportRuleConfig(me.dicConfig[familyId]);
            }
            else {
                SIE.invokeDataQuery({
                    type: "SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer",
                    method: "GetReportRuleConfig",
                    token: me.mainView.token,
                    params: [familyId],
                    success: function (res) {
                        if (res.Success) {
                            if (!me.dicConfig[familyId]) {
                                me.dicConfig[familyId] = res.Result;
                                me.setReportRuleConfig(res.Result);
                            }
                        }
                    }
                });
            }

            if (me.dicTaskConfig[familyId]) {
                me.setTaskRuleConfig(me.dicTaskConfig[familyId]);
            }
            else {
                SIE.invokeDataQuery({
                    type: "SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer",
                    method: "GetTaskConfig",
                    token: me.mainView.token,
                    params: [familyId],
                    success: function (res) {
                        if (res.Success) {
                            if (!me.dicTaskConfig[familyId]) {
                                me.dicTaskConfig[familyId] = res.Result;
                                me.setTaskRuleConfig(res.Result);
                            }
                        }
                    }
                });
            }
        }
    },

    /**
     * 保存报工规则配置临时数据
     * @method saveTempConfig
     * @param {ProductFamily} oldFamily 属性变更参数
     */
    saveTempConfig: function (oldFamily) {
        if (!oldFamily || !oldFamily.getId())
            return;
        var me = this;
        var model = JSON.parse(JSON.stringify(me.configPanel.getViewModel().data));
        me.dicConfig[oldFamily.getId()] = model;
    },

    /**
     * 保存任务单生成配置临时数据
     * @method saveTempTaskConfig
     * @param {ProductFamily} oldFamily 属性变更参数
     */
    saveTempTaskConfig: function (oldFamily) {
        if (!oldFamily || !oldFamily.getId())
            return;
        var me = this;
        var model = JSON.parse(JSON.stringify(me.taskConfigPanel.getViewModel().data));
        me.dicTaskConfig[oldFamily.getId()] = model;
    },

    /**
     * 保存报工规则配置
     * @method saveReportRuleConfig
     */
    saveReportRuleConfig: function () {
        var me = this;
        me.saveTempConfig(me.productFamily);
        var configs = me.initRuleConfigs();
        var result = Ext.JSON.encode(configs);
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer",
            method: "SaveReportRuleConfigs",
            token: me.mainView.token,
            params: [result],
            success: function (res) {
                if (!res.Success) {
                    me.dicConfig[familyId] = res.Result;
                }
            }
        });
    },

    /**
     * 保存任务单生成规则配置
     * @method saveReportRuleConfig
     */
    saveTaskConfig: function () {
        var me = this;
        me.saveTempTaskConfig(me.productFamily);
        var configs = me.initTaskConfigs();
        var result = Ext.JSON.encode(configs);
        SIE.invokeDataQuery({
            type: "SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer",
            method: "SaveTaskConfigs",
            token: me.mainView.token,
            params: [result],
            success: function (res) {
                if (!res.Success) {
                    me.dicConfig[familyId] = res.Result;
                }
            }
        });
    },

    /**
     * 报工规则配置数据处理
     * @method initRuleConfigs
     * @returns {Array} 规则配置列表
     */
    initRuleConfigs: function () {
        var me = this;
        var configs = [];
        for (var familyId in me.dicConfig) {
            var config = me.dicConfig[familyId];
            configs.push({ familyId: familyId, config: config });
        }
        return configs;
    },

    /**
     * 任务单生成配置数据处理
     * @method initTaskConfigs
     * @returns {Array} 规则配置列表
     */
    initTaskConfigs: function () {
        var me = this;
        var configs = [];
        for (var familyId in me.dicTaskConfig) {
            var config = me.dicTaskConfig[familyId];
            configs.push({ familyId: familyId, config: config });
        }
        return configs;
    },

    /**
     * 加载任务单生成规则配置
     * @method setTaskRuleConfig
     * @param {Object} config 报工规则配置
     */
    setTaskRuleConfig: function (config) {
        var me = this;
        try {
            me.isLoaded = false;
            config.Family = 0;
            me.taskConfigPanel.getViewModel().setData(config);
        } catch (e) {
            SIE.Msg.showError(e.message);
        } finally {
            setTimeout(function () {
                me.isLoaded = true;
            }, 1000);
        }
    },

    /**
     * 加载报工规则配置
     * @method setReportRuleConfig
     * @param {Object} config 报工规则配置
     */
    setReportRuleConfig: function (config) {
        var me = this;
        try {
            me.isLoaded = false;
            config.Family = 0;
            me.configPanel.getViewModel().setData(config);
        } catch (e) {
            SIE.Msg.showError(e.message);
        } finally {
            setTimeout(function () {
                me.isLoaded = true;
            }, 1000);
        }
    },

    /**
     * 清除任务单生成规则配置
     * @method clearTaskRuleConfig
     * @param {ProductFamily} newFamily 属性变更参数
     * @param {Ext.form.Panel} panel 属性变更参数
     */
    clearTaskRuleConfig: function (newFamily, panel) {
        if (newFamily)
            return;
        var me = this;
        try {
            me.isLoaded = false;
            panel.getViewModel().setData({
                ByProcess: false,
                BySpecification: false,
                ByQty: false,
                ByVirtualPart: false,
                Qty: 0,
                Family: -1,
            });
        } catch (e) {
            SIE.Msg.showError(e.message);
        } finally {
            setTimeout(function () {
                me.isLoaded = true;
            }, 1000);
        }
    },

    /**
     * 清除报工规则配置
     * @method clearReportRuleConfig
     * @param {ProductFamily} newFamily 属性变更参数
     * @param {Ext.form.Panel} panel 属性变更参数
     */
    clearReportRuleConfig: function (newFamily, panel) {
        if (newFamily)
            return;
        var me = this;
        try {
            me.isLoaded = false;
            panel.getViewModel().setData({
                IsSyntype: false,
                IsExpendItem: true,
                ModReport: -1,
                ReportMode: -1,
                ReportQty: 1,
                Family: -1,
            });
        } catch (e) {
            SIE.Msg.showError(e.message);
        } finally {
            setTimeout(function () {
                me.isLoaded = true;
            }, 1000);
        }
    },

    /**
     * 设置产品族保存命令状态
     * @method setSaveCmdEnable
     */
    setSaveCmdEnable: function () {
        var me = this;
        var dates = me.mainView.getSelection();
        if (dates.length < 1 || me.isLoaded === false)
            return;
        dates[0].dirty = true;
        me.mainView.syncCmdState(me.mainView, true);
    },

    /**
     * 子列表标签页切换事件
     * @method tabchange
     * @param {Ext.tab.Panel} tabPanel 标签控件
     * @param {newCard} newCard 新激活子页签
     * @param {oldCard} oldCard 旧子页签
     * @param {eOpts} eOpts 参数
     */
    tabchange: function (tabPanel, newCard, oldCard, eOpts) {
        if (newCard.title === '报工规则配置' || '任务单生成配置项')
            return;
        var control = newCard.down("gridpanel");
        if (control !== null && control.SIEView.getChildren().length === 0)
            if (newCard.down("form"))
                control = newCard.down("form").SIEView.getChildren().length > 0 ? newCard.down("form") : control;
        if (!control)
            control = newCard.down("form");
        if (!control)
            control = newCard.down("treepanel");
        var view = control.SIEView;
        view.inactive = false;
        view.loadChildData();
        if (view.hasListeners['isready']) {
            view.fireEvent('isReady', true);
        }
    }
});

/**
 * 报工规则设置页签数据模型
 * @class SIE.Web.MES.TaskManagement.RuleConfig
 * @constructor
 */
Ext.define('SIE.Web.MES.TaskManagement.RuleConfig', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.ruleConfig',
    data: {
        IsExpendItem: true,
        IsSyntype: false,
        ReportMode: -1,   //报工方式：0自动  1手动
        ModReport: -1,    //0按默认值报工  1剩余可报工数报工
        ReportQty: 1,
        Family: -1,
    },
    formulas: {
        modRportDisabled: {
            get: function (get) {
                var fn = get('ReportMode');
                var family = get('Family');
                return fn === 0 || family === -1;
            }
        },
        reportQtyDiasble: {
            get: function (get) {
                var fn = get('ModReport');
                var family = get('Family');
                return fn === 1 || family === -1;
            }
        },
        defaultDisabled: {
            get: function (get) {
                var family = get('Family');
                return family === -1;
            }
        },
        modDisabled: {
            get: function (get) {
                var family = get('Family');
                return family === -1;
            }
        },
        reportDisabled: {
            get: function (get) {
                var family = get('Family');
                return family === -1;
            }
        },
        autoDisabled: {
            get: function (get) {
                var family = get('Family');
                return family === -1;
            }
        },
        handDisabled: {
            get: function (get) {
                var family = get('Family');
                return family === -1;
            }
        },
        syntypeDiasble: {
            get: function (get) {
                var family = get('Family');
                return family === -1;
            }
        },
        isExpendItemDiasble: {
            get: function (get) {
                var family = get('Family');
                return family === -1;
            }
        },
        
    }
});

Ext.define('SIE.Web.MES.TaskManagement.TaskConfig', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.taskConfig',
    data: {
        ByProcess: false,
        BySpecification: false,
        ByQty: false,
        ByVirtualPart: false,
        Qty: 0,
        Family: -1
    },
    formulas: {
        qtyDisabled: {
            get: function (get) {
                var fn = get('ByQty');
                var family = get('Family');
                return fn === false || family === -1;
            }
        },
        byQtyDisabled: {
            get: function (get) {
                var fn = get('Family');
                return fn === -1;
            }
        },
        byProcessDisabled: {
            get: function (get) {
                var fn = get('Family');
                return fn === -1;
            }
        },
        bySpecificationDisabled: {
            get: function (get) {
                var fn = get('Family');
                return fn === -1;
            }
        },
        byVirtualPartDisabled: {
            get: function (get) {
                var fn = get('Family');
                return fn === -1;
            }
        },
    }
});

