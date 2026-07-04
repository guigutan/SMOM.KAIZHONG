Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.IndicatorConditionLayout',
    {
        extend: 'SIE.autoUI.layouts.Common',
        title: "",
        module: null,
        _layoutChildren: function (regions) {
            var me = this;
            var main = regions.main;
            me.view = main.getView();
            var inView = me.getIndicatorMeta(me.view);
            return Ext.widget('container', {
                border: 0,
                layout: 'border',
                scrollable: true,
                defaults: {
                    split: false,
                    layout: 'fit',
                    border: 0
                },
                items: [
                    {
                        collapsible: false,
                        region: 'center',
                        items: me.view.getControl()
                    },
                    {
                        region: 'east',
                        width: 600,
                        dockedItems: [{
                            xtype: 'toolbar',
                            dock: 'top',
                            ui: 'footer',
                            items: [{
                                text: '查看计算字典'.t(),
                                height:25,
                                handler: me.lookCaculDic
                            }]
                        }],
                        items: inView,
                    }
                ]
            });
        },
        /**
         * 指标运算视图数据
         * @param {any} tab
         */
        getIndicatorMeta: function (view) {
            var control = null;
            SIE.AutoUI.getMeta({
                model: "SIE.AbnormalInfo.AbnormalMonitors.IndicatorRuleViewModel",
                ignoreCommands: true,
                isDetail: true,
                async: false,
                ignoreQuery: true,
                callback: function (blocks) {
                    ui = SIE.AutoUI.generateAggtControl(blocks);
                    var data = Ext.create(ui._view.model);
                    ui._view.setData(data);
                    view.indicatorOpraView = ui._view;
                    control = ui.getControl();
                },
            });
            return control;
        },
        lookCaculDic: function () {
            var win = SIE.Window.show({
                width: 300,
                height:120,
                collapsible: false,
                title: '查看计算字典'.t(),
                items: [{
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'container',
                        flex: 1,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            margin: '0 0 10 0'
                        },
                        defaultType: 'button',
                        items: [ {
                            text: 'SysDate',
                            xtype: 'label'
                        }, {
                            text: 'Total',
                            xtype: 'label'
                        }]
                    }, {
                        xtype: 'container',
                        margin: '0 0 0 20',
                        flex: 1,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            margin: '0 0 10 0'
                        },
                        defaultType: 'button',
                        items: [ {
                            text: '系统当前时间'.t(),
                            xtype: 'label'
                        }, {
                            text: '当前数据集总数'.t(),
                            xtype: 'label'
                        }]
                    }]
                }],
                buttons:[]
            });
            win.show();
        }
    });