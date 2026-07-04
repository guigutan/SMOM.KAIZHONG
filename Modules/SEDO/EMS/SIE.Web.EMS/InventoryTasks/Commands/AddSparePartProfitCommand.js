SIE.defineCommand('SIE.Web.EMS.InventoryTasks.Commands.AddSparePartProfitCommand', {
    meta: { text: "新增盘盈", group: "edit", iconCls: "icon-AddEntity icon-green" },
    _mainView: null,
    canExecute: function (view) {
        //主表盘点状态为【盘点中、初盘完成、复盘中】才可点击
        var parentEntity = view._parent.getCurrent();
        if (parentEntity == null) {
            return false;
        }

        //20 盘点中 30 初盘完成 40 复盘中
        if (parentEntity.data.InventoryTaskStatus !== 20
            && parentEntity.data.InventoryTaskStatus !== 30
            && parentEntity.data.InventoryTaskStatus !== 40) {
            return false;
        }

        return view.canAddItem();
    },

    execute: function (view, source) {
        this._mainView = view;
        var me = this;
        var parentEntity = view._parent.getCurrent();
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.InventoryTasks.ViewModels.AddSparePartProfitViewModel",
            module: view._parent.module,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.generateAggtControl(res);
                var entity = new detailView._view._model();
                entity.setInventoryTaskId(parentEntity.getId());
                detailView._view.setData(entity);

                var btns = ['确定并打印'.t(), '确定'.t(), '取消'.t()];

                var win = SIE.Window.show({
                    title: "新增盘盈",
                    width: '70%',
                    height: '60%',
                    items: detailView.getControl(),
                    id: "AddSparePartProfitCommand001",
                    buttons: btns,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            me.save(detailView, win, me);
                            return false;
                        }
                        else if (btn == "确定并打印".t()) {
                            var info = detailView._view.getCurrent().data;

                            //管控方式不可空，所以默认值是0
                            if (info.ControlMethod == null || info.ControlMethod == 0) {
                                SIE.Msg.showMessage("请先选择备件".t());
                                return false;
                            }

                            //物料管控的不打印，只保存
                            if (info.ControlMethod == 10) {
                                me.save(detailView, win, me);
                                return false;
                            }

                            var signdata = {
                                command: me.meta.command,
                                entityType: me.view.model,
                                parentType: me.view.getParent() ? me.view.getParent().model : ""
                            }

                            SIE.invokeDataQuery({
                                method: 'AddSparePartProfit',
                                params: [info],
                                action: 'queryer',
                                type: 'SIE.Web.EMS.InventoryTasks.InventoryTaskDataQueryer',
                                token: view.token,
                                logInfo: signdata,
                                success: function (result) {
                                    var sparePartDetailId = result.Result;

                                    //物料管控不打印
                                    if (info.ControlMethod == 10) {
                                        return false;
                                    }

                                    SIE.AutoUI.getMeta({
                                        model: "SIE.EMS.InventoryTasks.ViewModels.AddSparePartProfitPrintViewModel",
                                        ignoreCommands: false,
                                        isDetail: true,
                                        ignoreQuery: true,
                                        callback: function (resOfMeta) {
                                            var detailViewOfSelectPrintTemplate = SIE.AutoUI.createDetailView(resOfMeta);
                                            var entityOfSelectPrintTemplate = new detailViewOfSelectPrintTemplate._model();

                                            var addSparePartProfitViewModel = detailView._view.getCurrent().data;

                                            entityOfSelectPrintTemplate.setControlMethod(addSparePartProfitViewModel.ControlMethod);
                                            detailViewOfSelectPrintTemplate.setData(entityOfSelectPrintTemplate);

                                            var winSelectPrintTemplate = SIE.Window.show({
                                                title: "选择打印模板".t(),
                                                width: 480,
                                                height: 200,
                                                items: detailViewOfSelectPrintTemplate.getControl(),
                                                id: "AddSparePartProfitPrintViewModel001",
                                                callback: function (btnOk) {
                                                    if (btnOk == "确定".t()) {
                                                        var reprintInfo = detailViewOfSelectPrintTemplate.getData().data;
                                                        if (!reprintInfo.TemplateId || reprintInfo.TemplateId <= 0) {
                                                            SIE.Msg.showMessage("打印模板不能为空".t());
                                                            return false;
                                                        }

                                                        //打印
                                                        me.print(sparePartDetailId, reprintInfo, winSelectPrintTemplate)

                                                        win.close();
                                                        view._parent.reloadData();
                                                    }
                                                }
                                            });
                                        }
                                    });
                                }
                            });

                            return false;
                        }
                    }
                });
            }
        });
    },
    save: function (detailView, win, me) {

        var info = detailView._view.getCurrent().data;

        var signdata = {
            command: me.meta.command,
            entityType: me.view.model,
            parentType: me.view.getParent() ? me.view.getParent().model : ""
        }

        SIE.invokeDataQuery({
            method: 'AddSparePartProfit',
            params: [info],
            action: 'queryer',
            type: 'SIE.Web.EMS.InventoryTasks.InventoryTaskDataQueryer',
            token: this._mainView.token,
            logInfo: signdata,
            success: function (result) {
                win.close();
                view._parent.reloadData();
            }
        });
    },
    print: function (spareDetailId, reprintInfo, win) {

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.InventoryTasks.InventoryTaskDataQueryer",
            method: "PrintAddSparePartProfitViewModel",
            params: [spareDetailId, reprintInfo],
            async: false,
            token: this._mainView.token,
            success: function (resu) {
                var rstPrint = resu.Result;
                if (rstPrint.ErrMsg !== '') {
                    SIE.Msg.showError(rstPrint.ErrMsg);
                    return false;
                } else {
                    win.close();

                    var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({
                        ReportType: rstPrint.Type,
                        ReportData: {
                            path: rstPrint.Url,
                            content: rstPrint.Url
                        }
                    });

                    var cfg = printCmpt.getExtTarget();
                    if (cfg && cfg.printCallback) {
                        cfg.printCallback(printCmpt);
                    }
                    else {
                        var param = printCmpt.getPrintParams();
                        if (!printCmpt.hasError()) {
                            var printUrl = printCmpt.getPrintUrl();
                            if (!printCmpt.hasError())
                                CRT.Workbench.showPageDialog({ id: 'DeterminePrint_rpt', text: "新增备件盘盈打印".t(), method: 'POST', url: printUrl, params: param });
                        }
                    }
                }
            }
        });
    }
});