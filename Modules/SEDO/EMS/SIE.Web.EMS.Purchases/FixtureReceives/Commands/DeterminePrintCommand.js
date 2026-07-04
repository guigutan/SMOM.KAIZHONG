SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.DeterminePrintCommand', {
    meta: { text: "确定并打印", group: "edit", iconCls: "icon-Check icon-blue" },
    requires: ['SIE.Web.Common.Prints.Report.WebReportComponents'],
    canExecute: function (view) {
        var fromEntity = view.getCurrent();
        if (fromEntity == null)
            return false;
        if (fromEntity.getFixtureReceiveDetailId() === null)
            return false;
        if (fromEntity.getManageMode() === 10)//目前仅用ID编码管控存在序列号
            return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
        var snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveSn"; });
        if (!detailChildView  || !snChildView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var fromEntity = view.getCurrent();
        if (fromEntity.getFixtureReceiveDetailId() === null) {
            SIE.Msg.showError("请选择接收明细".t());
            return;
        }
        var verificationsn = me.verificationsn(fromEntity, detailChildView, snChildView);
        if (verificationsn === false)
            return;
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.FixtureReceives.ReceiveSnPrintViewModel",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "FixtureView",
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
                            me.snRecived(fromEntity, view, detailChildView, snChildView);
                            me.print(view, fromEntity, snChildView, reprintInfo, win);
                            return false;
                        }
                    }
                });
            }
        });
    },
    verificationsn: function (fromEntity, detailChildView, snChildView) {
        var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getFixtureReceiveDetailId() });
        if (fromEntity.getManageMode() === 5) {
            if (fromEntity.getCurrentQty() <= 0) {
                fromEntity.setMessage("请填写本次接收数量".t());
                return false;
            }
            let qty = detail.getRecivedQty();
            if (fromEntity.getReceiveType() === 50) {
                if (fromEntity.getSnCodeId() === null) {
                    fromEntity.setMessage("请选择返厂的序列号编码".t());
                    return false;
                }
                if (snChildView.getData().data.items.findIndex(function (p) { return p.data.Sn == fromEntity.getSnCodeId_Display_Display() }) !== -1) {
                    fromEntity.setMessage("序列号编码".t() + fromEntity.getSnCodeId_Display() + "已接收，请勿重复接收".t());
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
    
    snRecived: function (fromEntity, view, detailChildView, snChildView) {
        if (fromEntity.getManageMode() === 5) {
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getFixtureReceiveDetailId() });
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer",
                method: "SnDetermine",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (res) {
                    var childData = snChildView.getData();
                    childData.insert(0, res.Result);
                    var qty = detail.getRecivedQty() + res.Result.length;
                    detail.setRecivedQty(qty);
                    fromEntity.setRecivedQty(qty);
                }
            });
        }
    },
    print: function (view, fromEntity, snChildView, reprintInfo, win) {
        var snList = [];
        SIE.each(snChildView.getData().data.items, function (model) {
            model.data.CreateDate = null;
            model.data.UpdateDate = null;
            snList.push(model.data);
        });
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer",
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
                                CRT.Workbench.showPageDialog({ id: 'FixtureReceiveDeterminePrint_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                }
            }
        });
    }
});