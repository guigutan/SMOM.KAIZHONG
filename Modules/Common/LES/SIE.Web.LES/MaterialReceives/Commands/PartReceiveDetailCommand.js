SIE.defineCommand("SIE.Web.LES.MaterialReceives.Commands.PartReceiveDetailCommand", {
    meta: { text: "部分接收", group: "edit", iconCls: "icon-Checkmark icon-blue" },
    _operationView: null,//操作弹窗视图
    canExecute: function (view) {
        var sel = view.getSelection();
        if (sel == null || sel.length == 0) {
            return false;
        }
        return true;
    },
    execute: function (view) {
        var me = this;
        me.showPartReceiveLabel(view);
    },
    //弹出框 - 部分接收
    showPartReceiveLabel: function (view) {
        var me = this;
        var bill = me.view.getParent().getCurrent();
        var billDtl = me.view.getCurrent();

        SIE.AutoUI.getMeta({
            model: 'SIE.LES.MaterialReceives.MaterialReceiveLabel',
            viewGroup: 'PartReceiveView',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var listView = SIE.AutoUI.createListView(mainBlock);

                me._operationView = listView;
                var filter = {
                    Method: 'GetMaterialReceiveLabels',
                    Parameters: [billDtl.getMaterialReceiveId(), [billDtl.getId()]]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.LES.MaterialReceives.DataQueryer.MaterialReceiveQueryer',
                });
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "标签部分接收".t(),
                    width: 1000,
                    height: 500,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            if (listView.getSelection() == 0) {
                                SIE.Msg.showError("请选择要接收的标签号".t());
                                return false;
                            }
                            var isValidate = true;
                            var indata = listView.getSelection().map(p => p.data);
                            var isPartLables = listView.getData().data.length > indata.length;
                            var receivedQty = 0;
                            indata.forEach(p => {
                                receivedQty = receivedQty + p.ReceivedQty;
                                if (p.ReceivedQty > p.IssuedQty) {
                                    SIE.Msg.showError("接收数不能大于发料数".t());
                                    isValidate = false;
                                    return false;
                                }
                            });
                            billDtl.setReceivedQty(receivedQty);

                            if (!isValidate)
                                return false;

                            if (isPartLables) {
                                SIE.Msg.askQuestion('未勾选的数据将进行拒收处理,确认要提交吗?'.t(), function () {
                                    me.confirm(view, billDtl.data, indata, win);
                                });
                            } else {
                                me.confirm(view, billDtl.data, indata, win);
                            }


                            return false;
                        }
                    }
                });
            },
        });
    },
    //确认提交
    confirm(view, dtlData, labelData, win) {
        var me = this;
        var msg = me.validationOverReceive(view, dtlData);
        if (msg.length > 0) {
            SIE.Msg.askQuestion(msg + '; \r\n确认要继教接收吗?'.t(), function () {
                me.partReceiveLabels(view, dtlData, labelData, win);
            });
        } else {
            me.partReceiveLabels(view, dtlData, labelData, win);
        }
    },
    //序列号部分接收
    partReceiveLabels(view, dtlData, labelData, win) {

        SIE.Msg.wait("正在执行......".t());
        SIE.invokeDataQuery({
            type: "SIE.Web.LES.MaterialReceives.DataQueryer.MaterialReceiveQueryer",
            method: "ReceiveLabels",
            params: [dtlData, labelData],
            //async: false,
            token: view.token,
            callback: function (res) {
                if (res.Success) {
                    win.close();
                    SIE.Msg.showInstantMessage("接收成功".t());
                    view.reloadData();
                }
            }
        });
    },

    //校验备料单超收
    validationOverReceive(view, dtlData) {
        var msg = "";
        var indata = [dtlData];
        SIE.invokeDataQuery({
            type: "SIE.Web.LES.MaterialReceives.DataQueryer.MaterialReceiveQueryer",
            method: "ValidationOverReceiveDetail",
            params: [indata],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Success) {
                    msg = res.Result;
                }
            }
        });
        return msg;
    }
})