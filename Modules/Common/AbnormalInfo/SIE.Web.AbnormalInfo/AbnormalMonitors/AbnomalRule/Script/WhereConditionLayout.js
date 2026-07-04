Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.WhereConditionLayout',
    {
        extend: 'SIE.autoUI.layouts.Common',
        title: "",
        module: null,
        _layoutChildren: function (regions) {
            var me = this;
            var main = regions.main;
            me.view = main.getView();
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
                        items: [me.view.getControl(),
                 
                            {
                                xtype: 'container',
                                layout: 'anchor',
                                defaults: {
                                    width: '100%'
                                },
                                defaultType: 'textfield',
                                items: [{
                                    xtype: 'textarea',
                                    fieldStyle: {
                                        fontSize: '18px',
                                        lineHeight: '1.5em'
                                    },
                                    raw:true,
                                    fieldLabel: '',
                                    id: "whereConditionSqlFiled",
                                    emptyText: '请输入SQL语句，获取相关数据集'.t(),
                                    labelWidth: 80,
                                    maxLength: 2000,
                                    height : "85%"
                                }, {
                                        xtype: 'container',
                                        layout: {
                                            type: 'hbox',
                                            pack: 'left'
                                        },
                                        items: [{
                                            xtype: 'button',
                                            cls: 'contactBtn',
                                            scale: 'large',
                                            text: '运行测试'.t(),
                                            handler: me.runSqlScript
                                        }]
                                    }]
                            }
                        ]
                    }]
            });
        },
        runSqlScript: function () {
            var mainView = CRT.Context.PageContext.getLogicalView();
            //SQL语句赋值
            var sqlFiled = Ext.getCmp("whereConditionSqlFiled");
            var sql= sqlFiled.getValue();
            SIE.invokeDataQuery({
                type: "SIE.Web.AbnormalInfo.AbnormalMonitors.DataQuerys.AnomalyMonitorQueryer",
                method: "RunSqlScript",
                params: [sql],
                token: mainView.getToken(),
                async: false,
                callback: function callback(res) {
                    if (res.Success) {
                        SIE.Msg.showMessage("执行成功".t());
                    }
                }
            });
        }

    });