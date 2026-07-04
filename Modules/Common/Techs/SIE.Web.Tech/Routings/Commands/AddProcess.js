SIE.defineCommand('SIE.Web.Tech.Routings.Commands.AddProcess', {
    extend: 'SIE.cmd.Command',
    meta: { text: "添加", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    execute: function (listView, processTreeCtl) {
        var source = processTreeCtl.getSelection()[0];
        source.token = listView.token;
        var model = this.getEditEntity(source);
        this.showView(model, listView.token, processTreeCtl);
    },
    getEditEntity: function (source) {
        var model = SIE.getModel('SIE.Tech.Processs.Process');
        var newModel = new model();
        var ProductFamilyId = source.get('Id');
        newModel.generateId();
        newModel.setProductFamilyId(ProductFamilyId); 
        newModel.setCategoryCode(source.data.Code);
        newModel.setCategoryName(source.data.Name); 
        this.mon(newModel, 'propertyChanged', SIE.Web.Tech.ProcessCommonFun.ProcessPropertyChanged, this);
        return newModel;
    },
    showView: function (entity, token, processTreeCtl) {

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
                    entity.setType(0);
                    var processTree = processTreeCtl;
                    SIE.Window.show({
                        title: "新增工序".L10N(),
                        width: 690,
                        height: 520,
                        buttons: [
                            {
                                xtype: "button", text: "确定".t(), handler: function () {
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
                                    indata.Data = Ext.encode(indata.Data);
                                    var process = entity.data;
                                    //客制化界面命令注册签名（注册在context，没有后台请求）
                                    SIE.Signature.createCmdContext({ command: "SIE.Web.Tech.Routings.Commands.SaveProcess", commandName: "添加", Type: opt.model });
                                    var me = this;
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
                                                var record = processTreeCtl.getSelection()[0];
                                                var newNode = {
                                                    Id: process.Id,
                                                    text: Ext.String.format('[{0}]({1})0'.t(), process.Type < 25 ? '单'.t() : '批'.t(), process.Name),
                                                    processName: process.Name,
                                                    leaf: true,
                                                    nodetype: 'RoutingNode',
                                                    Type: process.Type,
                                                    parameterList: process.ParameterList
                                                }
                                                record.appendChild(newNode);
                                                record.expand();
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