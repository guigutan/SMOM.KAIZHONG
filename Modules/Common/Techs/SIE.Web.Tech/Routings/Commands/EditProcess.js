SIE.defineCommand('SIE.Web.Tech.Routings.Commands.EditProcess', {
    extend: 'SIE.cmd.Command',
    execute: function (view) {
        this.mon(view._current, 'propertyChanged', SIE.Web.Tech.ProcessCommonFun.ProcessPropertyChanged, this);
        this.showView(view);
    },
    showView: function (processView) {
        var entity = processView._current;
        var token = processView.token;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                isDetail: true,
                ignoreQuery: true,
                ignoreCommands: false,
                isAggt: true,
                viewGroup: "DetailsView",
                token: token,
                model: "SIE.Tech.Processs.Process",
                callback: function (res) {
                    var processControl = processView.processControl;
                    var view = SIE.AutoUI.generateAggtControl(res);
                    var ui = view.getControl();
                    var mainView = view._view;
                    mainView.setData(entity);
                    if (mainView.getChildren().length < 0) return;
                    var paramView = mainView.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessParameter' });
                    if (paramView) {
                        paramView.mon(paramView._control, 'cellclick', SIE.Web.Tech.ProcessCommonFun.setStepResultEditorItems, paramView);
                    }
                    var stepView = mainView.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessCollectStep' });
                    if (stepView) {
                        stepView.mon(stepView._control, 'cellclick', SIE.Web.Tech.ProcessCommonFun.setBarcodeTypeEditorItems, stepView);
                    }
                    SIE.Web.Tech.ProcessCommonFun.initStepChild(mainView.getData(), mainView);
                    mainView._control.items.items.first(function (p) { return p.name == 'Type' }).setReadOnly(true);
                    SIE.Window.show({
                        title: "修改工序".L10N(),
                        width: 690,
                        height: 520,
                        buttons: [
                            {
                                xtype: "button", text: "确定".t(), handler: function () {
                                    var me = this;
                                    var view = mainView;
                                    if (!view.validateData()) return;;
                                    var indata = {};
                                    var opt = {};
                                    opt.model = 'SIE.Tech.Processs.Process';
                                    opt.withChildren = true;
                                    opt._changeSetData = view.serializeData(opt.withChildren);
                                    opt.token = view.token;
                                    if (!opt._changeSetData.isEmpty()) {
                                        var submitData = opt._changeSetData.getSubmitData();
                                        indata.Data = submitData;
                                    }
                                    indata.Type = opt.model;
                                    if (indata.Data == undefined) {
                                        me.up('window').close(); //没有可提交的数据就直接关闭
                                        return;
                                    }
                                    var curData = view.getData().data;
                                    var sourceContorl = processControl;
                                    indata.Data = Ext.encode(indata.Data);

                                    SIE.invokeCommand({
                                        token: opt.token,
                                        cmd: "SIE.Web.Tech.Routings.Commands.SaveProcess",
                                        async: true,
                                        data: indata,
                                        callback: function (res) {
                                            if (!res.Success) {
                                                SIE.Msg.showError(res.Message);
                                            }
                                            else {
                                                SIE.Msg.showMessage("修改成功!".t());
                                                var record = sourceContorl.getSelection()[0];
                                                if (curData.Type < 25)
                                                    record.set('text', '['+'单'.t()+']('.t() + curData.Name + ')' + curData.ReferenceTimes);
                                                else
                                                    record.set('text', '[' + '批'.t() + ']('.t() + curData.Name + ')' + curData.ReferenceTimes);

                                                record.set('type', curData.Type);
                                                me.up('window').close();
                                            }
                                        }
                                    });
                                }
                            },
                            {
                                xtype: "button", text: "取消".t(), handler: function () {
                                    this.up('window').close()
                                }
                            }
                        ],
                        items: ui,
                    });
                }
            });
        }
    },



});