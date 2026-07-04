SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.AddOutsourcingRequestCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit",iconCls: "iconfont icon-AddEntity icon-green" },
    _model: "SIE.MES.Outsourcing.Model.AddModel",

    
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: "SIE.MES.Outsourcing.Model.AddModel",
            module: 'SIE.MES.Outsourcing.OutsourcingRequest,SIE.MES',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                me.detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = Ext.create("SIE.MES.Outsourcing.Model.AddModel");

                me.detailView.setData(entity);
                var ui = me.detailView.getControl();
                var win = SIE.Window.show({
                    title: "新增".t(),
                    width: '35%',
                    height: '35%',
                    layout: 'fit',
                    plain: true,
                    buttonAlign: 'right',
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var detailData = me.detailView.getCurrent().getData();
                            var isError = false;
                            SIE.invokeDataQuery({
                                async: false,
                                method: 'Add',
                                params: [detailData],
                                action: 'queryer',
                                type: 'SIE.Web.MES.Outsourcing.OutsoucingDataQueryer',
                                token: view.getToken(),
                                callback: function (res) {
                                    if (!res.Success) {
                                        SIE.Msg.showError(res.Message);
                                        isError = true;
                                    } else {
                                        SIE.Msg.showInstantMessage('保存成功！'.t(), "提示".t(), 2, function () { win.close(); });
                                        CRT.Event.fire("SIE.MES.Outsourcing.OutsourcingRequest_refresh");
                                        win.close();
                                    }

                                }
                            });
                            return !isError;
                        }
                        if (btn == "取消".t()) {
                            win.close();
                        }
                    }
                });
            }
        });

    }
});