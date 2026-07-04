Ext.define('SIE.Web.Andon.Andons.Scripts.AndonTypeMessageTemplateEditor', {
    extend: 'Ext.util.Observable',
    constructor: function () {
        this.callParent(arguments);
    },
    onClick: function (field, trigger, e) {
        var me = this;
        var cell = field.up();
        var row_data = cell.context.record;
        var msgTempText = row_data.getMessageTemplate();
        var pushPlugId = row_data.getPushPlugId();
        var codeValue = "";
        if (!pushPlugId) {
            SIE.Msg.showWarning('请先选择推送模块!'.t());
            return;
        }
        SIE.invokeDataQuery({
            type: "SIE.Web.Andon.Andons.DataQuery.AndonTypeMessageTemplateDataQuery",
            method: "GetMessageTemplate",
            params: [pushPlugId],
            callback: function (res) {
                var templateInfo = res.Result;
                if (templateInfo == null) {
                    SIE.Msg.showWarning('该推送方式未实现，无法打开信息模板!'.t());
                    return;
                }
                SIE.AutoUI.getMeta({
                    model: templateInfo.TemplateModel,
                    ignoreCommands: true,
                    isDetail: true,
                    ignoreQuery: true,
                    callback: function (res) {
                        var mainBlock;
                        if (res.mainBlock)
                            mainBlock = res.mainBlock;
                        else
                            mainBlock = res;
                        var detailView = SIE.AutoUI.createDetailView(mainBlock);
                        var store = new detailView._model();
                        var textValue;
                        debugger;
                        if (msgTempText == "") {
                            try {
                                msgTempText = Ext.JSON.decode(templateInfo.TemplateJson);
                            } catch (error) {
                                try {
                                    msgTempText = eval('(' + templateInfo.TemplateJson + ')');
                                }
                                catch (error) {
                                    SIE.Msg.showError('配置信息JSON格式错误！'.t());
                                    return;
                                }
                            }
                            store.data = msgTempText;
                            detailView.setData(store);
                        } else {
                            try {
                                msgTempText = Ext.JSON.decode(msgTempText);
                            } catch (error) {
                                try {
                                    msgTempText = eval('(' + msgTempText + ')');
                                }
                                catch (error) {
                                    SIE.Msg.showError('配置信息JSON格式错误！'.t());
                                    return;
                                }
                            }
                            store.data = msgTempText;
                            if (msgTempText.Message)
                                codeValue = msgTempText.Message;
                            detailView.setData(store);
                        }
                        var ui = detailView.getControl();
                        var uicontainer = Ext.create('Ext.container.Container', {
                            layout: {
                                type: 'vbox',
                                align: 'left'
                            },
                            items: [{
                                xtype: 'toolbar',
                                width: '100%',
                                items: [
                                    {
                                        xtype: 'button',
                                        text: '代码'.t(),
                                        listeners: {
                                            'click': function (btn) {
                                                var field = null;
                                                if (Ext.ComponentQuery.query('textfield:last', ui).length > 0) {
                                                    field = Ext.ComponentQuery.query('textfield:last', ui)[0];
                                                }
                                                if (field) {
                                                    field.setValue(codeValue);
                                                }
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        text: '模板'.t(),
                                        listeners: {
                                            'click': function (btn) {
                                                SIE.invokeDataQuery({
                                                    type: "SIE.Web.Andon.Andons.DataQuery.AndonTypeMessageTemplateDataQuery",
                                                    method: "GetRazorTemplate",
                                                    params: [],
                                                    callback: function (res) {
                                                        var field = null;
                                                        if (Ext.ComponentQuery.query('textfield:last', ui).length > 0) {
                                                            field = Ext.ComponentQuery.query('textfield:last', ui)[0];
                                                        }
                                                        if (!res.Success) {
                                                            SIE.Msg.showMessage(res.Message);
                                                        }
                                                        if (field) {
                                                            //codeValue = field.getValue();
                                                            field.setValue(res.Result);
                                                        }
                                                    }
                                                });
                                            }
                                        }
                                    }
                                ]
                            }, ui]
                        });
                        var win = SIE.Window.show({
                            title: "配置".t(),
                            items: uicontainer,
                            height: document.body.clientHeight * 0.8,
                            width: document.body.clientWidth * 0.3,
                            callback: function (btn) {
                                if (btn == '确定'.t()) {
                                    debugger;
                                    var items = detailView.getControl();
                                    var view = items.SIEView;
                                    var data = view.getData();
                                    var templateValue = Ext.JSON.encode(data.data);
                                    var currentData = row_data;
                                    currentData.setMessageTemplate(templateValue);
                                }
                            }
                        });
                    }
                });
            }
        });
    }
});