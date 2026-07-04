SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.FinishStockCountsCommand', {
    meta: { text: "完工", group: "edit", iconCls: "icon-CalendarCheck icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length == 0 || view.getSelection().length > 1) {
            return false;
        }

        var sel = view.getSelection();
        return sel.all(function (p) { return p.getState() === 40; });
        return true;
    },

    execute: function (view, source) {
        var me = this;
        var curBill = me.view.getCurrent();
        SIE.invokeDataQuery({
            async: false,
            type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
            method: 'GetFinishCountDetail',
            token: me.view.token,
            params: [curBill.getId()],
            callback: function (res) {
                if (res.Success) {
                    if (res.Result == null) {
                        view.execute({
                            data: view.getSelectionIds(),
                            success: function (res) { //回调
                                view.reloadData();
                            }
                        });
                    }
                    else {
                        me.finishDiffCount(curBill, res.Result);
                    }
                }
                if (!res.Success) {
                    SIE.Msg.showError(res.Message);
                }
            }
        });
    },
    reloadView: function () {
        var me = this;
        me.view.reloadData();
    },
    finishDiffCount: function (curBill, isDiffAjust) {
        var me = this;
        var signdata = {
            command: me.meta.command,
            entityType: me.view.model,
            parentType: me.view.getParent() ? me.view.getParent().model : ""
        }

        var win = SIE.Window.show({
            title: "差异处理方式".t(),
            width: 300,
            height: 200,
            items: [
                {
                    xtype: 'radiogroup',
                    fieldLabel: '',
                    width: 200,
                    height: 30,
                    style: 'margin:0px 0 0 30px;height:30px;',
                    hideLabel: true,
                    columns: 1,
                    name: 'rb-col',
                    items: [
                        { boxLabel: '差异复核'.t(), inputValue: 10, },
                        { boxLabel: '差异调账'.t(), inputValue: 20, hidden: !isDiffAjust },
                        { boxLabel: '不处理'.t(), inputValue: 30, checked: true },
                        {
                            xtype: 'displayfield',
                            name: 'stockCountFinishDisplay',
                            hideLabel: true,
                        }
                    ]
                },
            ],
            buttons: [
                {
                    xtype: "button", text: "确定".t(), handler: function () {
                        var rb = win.query('[name=rb-col]');
                        var ckVal = rb[0].getChecked()[0].inputValue
                        SIE.invokeDataQuery({
                            async: false,
                            type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
                            method: 'StockCountFinishDiff',
                            token: me.view.token,
                            logInfo: signdata,
                            hideErrorMsg: true,
                            params: [curBill.getId(), ckVal],
                            callback: function (res) {
                                if (res.Success) {
                                    var datas = res.Result;
                                    if (datas.length > 0) {
                                        me.showAdjustView(me.view, datas);
                                    }
                                    else {
                                        SIE.Msg.showInstantMessage('操作完成'.t());
                                        me.reloadView();
                                    }
                                    win.close();

                                }
                                if (!res.Success) {
                                    var tips = win.query('[name=stockCountFinishDisplay]');
                                    tips[0].setValue("<span style='color: red'>" + res.Message + "</span>");
                                }
                            }
                        });
                    }
                },
            ],
        });
    },

    showAdjustView: function (view, datas, win) {
        var me = this;
        var curBill = me.view.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.LES.LesStockCounts.ViewModels.DiffAdjustViewModel",
            module: "SIE.LES.LesStockCounts.LesStockCount,SIE.LES",
            ignoreCommands: false,
            isReadonly: false,
            ignoreQuery: true,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                var listView = ui.getView();
                listView.getData().add(datas);
                var items = ui.getControl();
                listView._onCurrentChanged = function () { me.currentChange(listView); };
                var winson = SIE.Window.show({
                    title: '差异调账'.t(),
                    items: items,
                    height: window.innerHeight * 0.7,
                    width: window.innerWidth * 0.7,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var flag = true;
                            if (listView.childDatas) {
                                listView.getData().data.items.forEach(function (p) {
                                    if (flag) {
                                        var qty = p.data.Qty;
                                        var diffQty = listView.childDatas.where(function (f) { return f.DtlId == p.data.DtlId }).sum(function (f) { return f.DiffQty; });
                                        if (qty != diffQty) {
                                            {
                                                SIE.Msg.showError(Ext.String.format('标签号{0}物料投入工单的数量不等于差异数量'.t(), p.data.LabelNo));
                                                flag = false;
                                                return;
                                            }
                                        }
                                        var woIds = listView.childDatas.where(f => f.DtlId == p.data.DtlId).select(function (f) { return f.WorkOrderId });
                                        woIds.forEach(function (f) {
                                            if (woIds.where(function (a) { return a == f }).length > 1) {
                                                SIE.Msg.showError('工单数据重复'.t());
                                                flag = false;
                                                return;
                                            }
                                        });
                                    }
                                });
                                if (flag) {
                                    {
                                        SIE.Msg.wait("正在更新库存...".t());
                                        SIE.invokeDataQuery({
                                            async: false,
                                            type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
                                            method: 'SaveDiffAdjust',
                                            token: listView.token,
                                            params: [curBill.data.Id, listView.childDatas],
                                            callback: function (res) {
                                                if (res.Success) {
                                                    SIE.Msg.showInstantMessage('操作完成'.t());
                                                    me.reloadView();
                                                    winson.close();
                                                }
                                            }
                                        });
                                    }
                                    return false
                                }
                                else
                                    return false;
                            }
                            else {
                                SIE.Msg.showError('请维护投入工单数据'.t());
                                return false;
                            }
                        }
                        if (btn == "取消".t()) {
                            winson.close();
                        }
                    }
                });

            }
        });
    },

    currentChange: function (listView) {
        listView._children[0].syncCmdState();
        listView._children[0].getData().data.removeAll();
        var cur = listView.getCurrent().data;
        var flag = true;
        if (listView.childDatas && listView.childDatas.length > 0) {
            var datas = listView.childDatas.where(function (f) { return f.DtlId == cur.DtlId });
            if (datas.length > 0) {
                listView._children[0].getData().add(datas);
                flag = false;
            }
        }
        else {
            listView.childDatas = [];
        }
        if (flag)
            SIE.invokeDataQuery({
                async: false,
                type: "SIE.Web.LES.LesStockCounts.DataQueryer.LesStockCountDataQueryer",
                method: 'GetAdjustWorkOrderViewModels',
                token: listView.token,
                params: [cur.DtlId],
                callback: function (res) {
                    if (res.Result && res.Result.length > 0) {
                        listView._children[0].getData().data.removeAll();

                        res.Result.forEach(function (f) {
                            f.WorkOrderId_Display = f.WorkOrderNo;
                            listView.childDatas.push(f);
                        });
                        listView._children[0].getData().add(res.Result);
                    }
                }
            });
    },
});