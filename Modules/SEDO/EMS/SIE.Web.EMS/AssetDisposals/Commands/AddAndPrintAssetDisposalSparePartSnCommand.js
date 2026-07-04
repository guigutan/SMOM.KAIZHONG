SIE.defineCommand('SIE.Web.EMS.AssetDisposals.Commands.AddAndPrintAssetDisposalSparePartSnCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "确定并打印", group: "edit", iconCls: "icon-ClipboardPaperCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;

        var formEntity = view.getData();

        if (formEntity.data.SparePartId == null || formEntity.data.WarehouseId == null
            || formEntity.data.Qty == null || formEntity.data.QualityStatus == null || formEntity.data.PrintTemplateId == null) {
            SIE.Msg.showError('备件编码、质量状态、入库仓库、打印模板均不能为空，且数量须大于0！'.t());
            return false;
        }

        SIE.invokeDataQuery({
            method: 'GetAddAssetDisposalSpareParts',
            params: [formEntity.data],
            async: false,
            action: 'queryer',
            type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
            token: view.token,
            success: function (ret) {

                console.log(assetSnAddwin.view);
                if (ret.Success) {
                    var store = assetSnAddwin.view.getData();
                    var dataArr = [];
                    for (var i = 0; i < ret.Result.data.items.length; i++) {
                        var record = ret.Result.data.items[i];

                        record.setSparePartId_Display(formEntity.data.SparePartId_Display);
                        record.setWarehouseId_Display(formEntity.data.WarehouseId_Display);
                        store.add(record);
                        dataArr.push(record.data);
                        assetSnAddwin.view.mon(record, 'propertyChanged', me.onEntityPropertyChanged, assetSnAddwin.view);
                    }

                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
                        method: "AddSnPrint",
                        params: [dataArr, formEntity.data.PrintTemplateId],
                        async: false,
                        token: view.token,
                        success: function (resu) {
                            var rstPrint = resu.Result;
                            if (rstPrint.ErrMsg !== '') {
                                SIE.Msg.showError(rstPrint.ErrMsg);
                                return false;
                            } else {
                                assetSnAddwin.close();
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
                                            CRT.Workbench.showPageDialog({ id: 'PrintAssetDisposalSparePartSn_rpt', text: "打印".t(), method: 'POST', url: printUrl, params: param });
                                    }
                                }
                            }
                        }
                    });
                }
            }
        });
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var view = this;

        if (e.property == 'SparePartId') {

            setTimeout(function () {

                if (e.entity.data.SparePartId != null) {

                    if (e.entity.data.ControlMethod == 20) {

                        SIE.invokeDataQuery({
                            type: "SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer",
                            method: "GetLotNo",
                            params: [],
                            async: false,
                            token: view.token,
                            success: function (res) {
                                if (res.Success) {
                                    e.entity.setLotNo(res.Result);
                                }
                            }
                        });
                    }
                    else {
                        e.entity.setLotNo("");
                    }
                }

                e.entity.setQty(e.entity.data.SparePartId != null && e.entity.data.ControlMethod == 30 ? 1 : null);

            }, 0);
        }
    }
});