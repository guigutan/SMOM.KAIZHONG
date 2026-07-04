SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.DeterminePrintCommand', {
    meta: { text: "接收并打印", group: "edit", iconCls: "icon-Check icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var childView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.EquipmentReceives.ReceiveScanSnViewModel"; });
        var fromEntity = view.getCurrent();
        var verification = me.verification(childView, fromEntity);
        if (verification === false)
            return;
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.EquipmentReceives.ReceiveSnPrintViewModel",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "序列号打印".t(),
                    width: 480,
                    height: 200,
                    items: detailView.getControl(),
                    id: "DeterminePrintCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var reprintInfo = detailView.getData().data;
                            if (!reprintInfo.TemplateId || reprintInfo.TemplateId <= 0) {
                                SIE.Msg.showMessage("打印模板不能为空".L10N());
                                return false;
                            }
                            me.determine(view, childView, fromEntity);
                            me.print(view, childView, reprintInfo, win);
                            return false;
                        }
                    }
                });
            }
        });
    },
    verification: function (childView, fromEntity) {
        if (!childView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return false;
        }
        if (fromEntity.getEquipmentReceiveDetailId() === null) {
            SIE.Msg.showError("请选择接收明细".t());
            return false;
        }
        if (fromEntity.getEquipModelId() === null) {
            SIE.Msg.showError("设备型号不能为空".t());
            return false;
        }
        if (fromEntity.getCurrentQty() === null) {
            SIE.Msg.showError("请输入接收数量".t());
            return false;
        }
        if (fromEntity.data.RecivedQty + fromEntity.data.CurrentQty > fromEntity.data.Qty) {
            SIE.Msg.showError("已接收数量+本次接收数量不能大于接收数量".t());
            return false;
        }
        if (fromEntity.getReceiveType() === 50) {
            if (fromEntity.getEquipAccountId() === null) {
                SIE.Msg.showError("请选择返厂的设备编码".t());
                return false;
            }
            if (childView.getData().data.items.findIndex(function (p) { return p.data.EquipmentCode == fromEntity.getEquipAccountId_Display() }) !== -1) {
                SIE.Msg.showError("设备编码".t() + fromEntity.getEquipAccountId_Display() + "已接收，请勿重复接收".t());
                return false;
            }
        }
        return true;
    },
    determine: function (view, childView, fromEntity) {
        if (fromEntity.getReceiveType() === 50) {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer",
                method: "Determine",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (resa) {
                    var childData = childView.getData();
                    childData.add(resa.Result.SnInfo);
                    fromEntity.setRecivedQty(fromEntity.data.RecivedQty + 1);
                }
            });
        } else {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer",
                method: "DetermineOnQty",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (resb) {
                    var childData = childView.getData();
                    childData.add(resb.Result);
                    fromEntity.setRecivedQty(fromEntity.data.RecivedQty + fromEntity.data.CurrentQty);
                }
            });
        }
    },
    print: function (view, childView, reprintInfo, win) {
        var snList = [];
        SIE.each(childView.getData().data.items, function (model) {
            snList.push(model.data);
        });
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer",
            method: "DeterminePrint",
            params: [snList, reprintInfo],
            async: false,
            token: view.token,
            success: function (resu) {
                var rstPrint = resu.Result;
                if (rstPrint.ErrMsg !== '') {
                    SIE.Msg.showError(rstPrint.ErrMsg);
                    return false;
                } else {
                    win.close();
                    var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({ ReportType: rstPrint.Type, ReportData: { path: rstPrint.Url, content: rstPrint.Url } });
                    var cfg = printCmpt.getExtTarget();
                    if (cfg && cfg.printCallback) {
                        cfg.printCallback(printCmpt);
                    }
                    else {
                        var param = printCmpt.getPrintParams();
                        if (!printCmpt.hasError()) {
                            var printUrl = printCmpt.getPrintUrl();
                            if (!printCmpt.hasError())
                                CRT.Workbench.showPageDialog({ id: 'DeterminePrint_rpt', text: "序列号打印".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                }
            }
        });
    }
});