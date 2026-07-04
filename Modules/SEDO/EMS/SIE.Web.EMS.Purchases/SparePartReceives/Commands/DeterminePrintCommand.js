SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.DeterminePrintCommand', {
    meta: { text: "确定并打印", group: "edit", iconCls: "icon-Check icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        var fromEntity = view.getCurrent();
        if (fromEntity == null)
            return false;
        if (fromEntity.getSparePartReceiveDetailId() === null)
            return false;
        if (fromEntity.getControlMethod() === 10)
            return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
        var lotChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveLot"; });
        var snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveSn"; });
        if (!detailChildView || !lotChildView || !snChildView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var fromEntity = view.getCurrent();
        if (fromEntity.getSparePartReceiveDetailId() === null) {
            SIE.Msg.showError("请选择接收明细".t());
            return;
        }
        if (fromEntity.getSparePartId() === null) {
            fromEntity.setMessage("备件编码不能为空".t());
            return;
        }
        var verificationlot = me.verificationlot(fromEntity, detailChildView);
        if (verificationlot === false)
            return;
        var verificationsn = me.verificationsn(fromEntity, detailChildView, snChildView);
        if (verificationsn === false)
            return;
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.EquipmentReceives.ReceiveSnPrintViewModel",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "SparePartView",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "打印".t(),
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
                            me.lotRecived(fromEntity, view, detailChildView, lotChildView);
                            me.snRecived(fromEntity, view, detailChildView, snChildView);
                            me.print(view, fromEntity, lotChildView, snChildView, reprintInfo, win);
                            return false;
                        }
                    }
                });
            }
        });
    },
    verificationlot: function (fromEntity, detailChildView) {
        var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
        if (fromEntity.getControlMethod() === 20) {
            if (fromEntity.getLotCount() <= 0 || fromEntity.getLotQty() <= 0) {
                fromEntity.setMessage("请填写批次个数和批次数量".t());
                return false;
            }
            let qty = detail.getRecivedQty() + (fromEntity.getLotCount() * fromEntity.getLotQty());
            if (qty > fromEntity.getQty()) {
                fromEntity.setMessage("【已接收数量】+【本次接收数量】不能大于【接收数量】".t());
                return false;
            }
        }
        return true;
    },
    verificationsn: function (fromEntity, detailChildView, snChildView) {
        var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
        if (fromEntity.getControlMethod() === 30) {
            if (fromEntity.getCurrentQty() <= 0) {
                fromEntity.setMessage("请填写本次接收数量".t());
                return false;
            }
            let qty = detail.getRecivedQty();
            if (fromEntity.getReceiveType() === 50) {
                if (fromEntity.getStoreSummaryDetailId() === null) {
                    fromEntity.setMessage("请选择返厂的序列号编码".t());
                    return false;
                }
                if (snChildView.getData().data.items.findIndex(function (p) { return p.data.Sn == fromEntity.getStoreSummaryDetailId_Display() }) !== -1) {
                    fromEntity.setMessage("序列号编码".t() + fromEntity.getStoreSummaryDetailId_Display() + "已接收，请勿重复接收".t());
                    return false;
                }
                qty++;
            } else {
                qty = qty + fromEntity.getCurrentQty();
            }
            if (qty > fromEntity.getQty()) {
                fromEntity.setMessage("【已接收数量】+【本次接收数量】不能大于【接收数量】".t());
                return false;
            }
        }
        return true;
    },
    lotRecived: function (fromEntity, view, detailChildView, lotChildView) {
        if (fromEntity.getControlMethod() === 20) {
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
            var qty = detail.getRecivedQty() + (fromEntity.getLotCount() * fromEntity.getLotQty());
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
                method: "LotDetermine",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (res) {
                    var childData = lotChildView.getData();
                    SIE.each(res.Result, function (model) {
                        model.LineNo = detail.data.LineNo;
                        model.PurchaseOrderNo = detail.data.PurchaseOrderNo;
                        model.PurchaseOrderItemLineNo = detail.data.PurchaseOrderLine;
                        model.SupplierCode = detail.data.SupplierCode;
                        model.SupplierName = detail.data.SupplierName;
                        model.PurchaseObjectType = detail.data.PurchaseObjectType;
                        model.SparePartCode = detail.data.SparePartCode;
                        model.SparePartName = detail.data.SparePartName;
                        model.ControlMethod = detail.data.ControlMethod;
                    });
                    childData.insert(0, res.Result);
                    detail.setRecivedQty(qty);
                    fromEntity.setRecivedQty(qty);
                }
            });
        }
    },
    snRecived: function (fromEntity, view, detailChildView, snChildView) {
        if (fromEntity.getControlMethod() === 30) {
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
                method: "SnDetermine",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (res) {
                    var childData = snChildView.getData();
                    SIE.each(res.Result, function (model) {
                        model.LineNo = detail.data.LineNo;
                        model.PurchaseOrderNo = detail.data.PurchaseOrderNo;
                        model.PurchaseOrderItemLineNo = detail.data.PurchaseOrderLine;
                        model.SupplierCode = detail.data.SupplierCode;
                        model.SupplierName = detail.data.SupplierName;
                        model.PurchaseObjectType = detail.data.PurchaseObjectType;
                        model.SparePartCode = detail.data.SparePartCode;
                        model.ControlMethod = detail.data.ControlMethod;
                        model.SparePartName = detail.data.SparePartName;
                    });
                    childData.insert(0, res.Result);
                    var qty = detail.getRecivedQty() + res.Result.length;
                    detail.setRecivedQty(qty);
                    fromEntity.setRecivedQty(qty);
                }
            });
        }
    },
    print: function (view, fromEntity, lotChildView, snChildView, reprintInfo, win) {
        var lotList = [];
        SIE.each(lotChildView.getData().data.items, function (model) {
            model.data.CreateDate = null;
            model.data.UpdateDate = null;
            lotList.push(model.data);
        });
        var snList = [];
        SIE.each(snChildView.getData().data.items, function (model) {
            model.data.CreateDate = null;
            model.data.UpdateDate = null;
            snList.push(model.data);
        });
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
            method: "DeterminePrint",
            params: [lotList, snList, fromEntity.data, reprintInfo],
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
                                CRT.Workbench.showPageDialog({ id: 'SparePartReceiveDeterminePrint_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                }
            }
        });
    }
});