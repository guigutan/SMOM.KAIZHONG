Ext.define('SIE.Web.MES.TaskManagement.Reports.ReportGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

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
            layout = new SIE.Web.MES.TaskManagement.Reports.ReportLayout();
        }

        var res = layout.layout(regions);

        return res;
    }
});

Ext.define('SIE.Web.MES.TaskManagement.Reports.ReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'ReportLayout',
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
        var me = this;
        var view = main.getView();
        Ext.each(children, function (child, index) {
            if (child.getView().childLayoutType === 1) {
                if (index === 0) prePanel = 'card';
                cardPanels.push({
                    title: child.getView().getMeta().label,
                    items: child.getControl()
                });
            } else {
                if (index === 0) prePanel = 'tab';
                if (child.getView().getMeta().label == "报工".t()) {
                    var leftCtl = child.getControl();
                    panelControl = me.createPanel(leftCtl);
                    view.mon(view, 'currentChanged', me._currentChanged, child.getView());
                    child.getView().dicConfig = {};
                    tabPanel.items.push({
                        title: child.getView().getMeta().label,
                        items: panelControl
                    });
                } else {
                    tabPanel.items.push({
                        title: child.getView().getMeta().label,
                        items: child.getControl()
                    });
                }
            }
        });

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
            return this._layoutFormChildrenCore(mainControl, secondPanel);
        return this.layoutChildrenCore(mainControl, secondPanel, me.isLayoutChildrenHorizonal);
    },

    createPanel: function (leftCtl) {
        return {
            xtype: 'panel',
            bodyStyle: {
                border: 0
            },
            layout: {
                type: 'hbox',
                pack: 'start',
                align: 'stretch'
            },
            items: [{
                flex: 2,
                layout: 'fit',
                border: false,
                bodyStyle: 'border:none;',
                items: leftCtl,
                listeners: {
                    afterrender: function (comb) {
                        comb.setMaxWidth(1000);
                    }
                }
            },
                {
                flex: 1,
                border: false,
                    style: 'margin-left:50px;width:auto;border-left:0px solid #C2C2C2;background-color:#FFFFFF',
                bodyStyle: 'overflow:auto;margin-left:10px;',
                layout: 'vbox',
                id: 'reportTaskRightControl',
                listeners: {
                    afterrender: function (comb) {
                        document.getElementById(this.id + '-innerCt').style.overflowY = 'scroll';
                    }
                }
            }]
        };
    },
    _currentChanged: function (parm) {
        var me = this;//父列表
        if (me._current != null) {
            var tabPanel = me.getControl().up('tabpanel');
            var routTab = tabPanel.tabBar.items.items.where(function (p) { return p.title == '工序BOM'.t(); });
            var curData = me._parent._current.getData();
            if (routTab && routTab.length > 0) {
                tabPanel.setActiveItem(me.getControl().up());
                if (curData.ProcessId > 0) {
                    routTab[0].show();
                }
                else {
                    routTab[0].hide();
                }
            }
        }

        var id = 'reportTaskRightControl';
        var ctl = Ext.getCmp(id);
        ctl.removeAll();
        document.getElementById(id).style.borderLeftWidth = "0px";
        if (parm.newValue == null) {
            var tab = this.getControl().up('tabpanel');
            tab.mon(tab, 'tabchange', function () {
                if (this.activeTab.title == "报工") {
                    var ctl = document.getElementById('reportTaskRightControl-innerCt');
                    if (ctl)
                        ctl.style.removeProperty('width');
                }
            });
            return;
        }
        //任务单已开工（执行中）才能报工，测试阶段先放开
        if (parm.newValue.data.TaskStatus === 30) {
            SIE.Web.MES.TaskManagement.Reports.ReportLayout.getMainDefectIds(me);
            if (parm.newValue.data.IsSyntypeReport === true)  //共模比报工
                SIE.Web.MES.TaskManagement.Reports.ReportCommon.getCommonModeInfo(me);
        }
    },
    statics: {
        initRightItem: function (view, entity, fir) {
            var me = this;
            var first = "";
            if (fir)
                first = "border-top-width:0px;"
            return {
                layout: 'vbox',
                bodyStyle: first + 'border-bottom-width:0px;border-left-width:0;border-right-width:0; margin-bottom:5px;',
                defaults: {
                    layout: 'form',
                    xtype: 'container',
                    defaultType: 'displayfield',
                    style: 'width:50%'
                },
                name: 'formContent',
                items: [{
                    layout: 'hbox',
                    xtype: 'container',
                    items: [{
                        fieldLabel: '任务单号'.t(),
                        style: 'width:250px;',
                        name: 'taskNo',
                        value: entity.TaskNo,
                        taskId: entity.TaskId,
                        recordId: entity.RecordId,
                        workOrderId: entity.WorkOrderId,
                        processId: entity.ProcessId,
                    }, {
                        fieldLabel: '任务数量'.t(),
                        style: 'width:250px;margin-left:20px;',
                        labelAlign: 'right',
                        labelWidth: 120,
                        name: 'taskQty',
                        value: entity.TaskQty
                    }],
                },
                {
                    layout: 'hbox',
                    xtype: 'container',
                    items: [{
                        fieldLabel: '累计合格数'.t(),
                        style: 'width:250px;',
                        name: 'okQty',
                        value: entity.OkQty
                    }, {
                        fieldLabel: '累计不合格数'.t(),
                        labelAlign: 'right',
                        labelWidth: 120,
                        style: 'width:250px;margin-left:20px;',
                        name: 'ngQty',
                        value: entity.NgQty
                    }]
                },
                {
                    layout: 'hbox',
                    items: [{
                        xtype: 'numberfield',
                        fieldLabel: '报工数量(合格)'.t(),
                        style: 'width:250px;',
                        name: 'reportOkQty',
                        minValue: 0,
                        allowBlank: false,
                        value: entity.ReportOkQty,
                        proportion: entity.Proportion,
                        readOnly: entity.IsSyntype
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: '报工数量(不合格)'.t(),
                        style: 'width:250px;margin-left:20px;',
                        labelWidth: 120,
                        labelAlign: 'right',
                        labelInnerStyle: 'width:120px;',
                        name: 'reportNgQty',
                        minValue: 0,
                        maxNum: null,
                        allowBlank: false,
                        value: entity.ReportNgQty,
                        listeners: {
                            "change": function (filed, newValue, oldValue) {
                                var isSynt = Ext.getCmp('IsSyntypeField');
                                var okFiled = this.up().query('[name=reportOkQty]')[0];
                                if (isSynt && isSynt.getValue() == true) {
                                    if (this.maxNum == null) {
                                        var oKQty = Ext.getCmp('mainOKQtyField');
                                        var nGQty = Ext.getCmp('mainNgQtyField');
                                        var mainAllValue = oKQty.getValue() + nGQty.getValue();
                                        var isSynt = Ext.getCmp('IsSyntypeField');
                                        if (isSynt && isSynt.getValue() == true) {
                                            var mainField = Ext.getCmp('MainProportionField');
                                            if (mainField) {
                                                var mainPro = mainField.getValue();
                                                if (mainPro > 0) {
                                                    var assCtl = Ext.getCmp('reportTaskRightControl');
                                                    assCtl.items.items.where(function (p) { return p.name == 'formContent'; }).forEach(function (p) {
                                                        var reportOkQtyField = p.query('[name=reportOkQty]')[0];
                                                        var assPro = reportOkQtyField.proportion;
                                                        var val = Math.floor((mainAllValue * assPro) * 1.0 / mainPro);
                                                        var reportNgQtyField = p.query('[name=reportNgQty]')[0];
                                                        reportNgQtyField.maxValue = val;
                                                        reportNgQtyField.maxNum = val;
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    var mainOk = Ext.getCmp('mainOKQtyField');
                                    var val = mainOk.getValue();
                                    if (val == 0 && newValue == 0) return;
                                    if (val == 0) this.setValue(0);
                                    if (newValue > this.maxValue) {
                                        this.setValue(this.maxValue);
                                    }
                                    else {
                                        okFiled.setValue(this.maxValue - newValue);
                                    }
                                }
                            },
                            "focus": function () {

                            }
                        }
                    }]
                },
                {
                    layout: 'hbox',
                    items: [{
                        xtype: 'textfield',
                        fieldLabel: '批次号'.t(),
                        style: 'width:250px;margin-top:5px;margin-bottom:10px;',
                        name: 'batchNo',
                        value: entity.BatchNo
                    }, {
                        fieldLabel: '缺陷录入'.t(),
                        labelAlign: 'right',
                        xtype: 'textfield',
                        labelWidth: 120,
                        style: 'width:250px;margin-left:20px;margin-top:5px;margin-bottom:10px;',
                        readOnly: true,
                        name: 'quexianValue',
                        value: entity.DefectNames,
                        valueIds: entity.DefectIds
                    }, {
                        xtype: 'button',
                        style: 'width:35px;border:0;background:none;margin-top:5px;margin-bottom:10px;',
                        iconCls: "iconfont icon-TextQuality icon-blue",
                        name: 'quexianBtn',
                        listeners: {
                            click: {
                                fn: function () {
                                    var me = this;
                                    view.currentBtnId = this.id;
                                    var cmd = Ext.create(
                                        'SIE.Web.MES.TaskManagement.Reports.DefectSelectCommand', {});
                                    cmd._setOwnerView(view);
                                    cmd.command = Ext.getClassName(cmd);
                                    cmd.tryExecute(cmd);
                                }
                            }
                        }
                    }]
                }
                ],
            }
        },
        /**
         * 设置主料单缺陷Id
         * @param {any} view
         */
        getMainDefectIds: function (view) {
            SIE.invokeDataQuery({
                method: 'GetMainDefectIds',
                params: [view._parent.getCurrent().data.Id],
                async: false,
                action: 'queryer',
                sync: true,
                type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
                token: view.token,
                success: function (res) {
                    var data = view._parent.getCurrent().data;
                    if (!view.dicConfig[data.Id])
                        view.dicConfig[data.Id] = res.Result;
                }
            });
        },
    }
});