SIE.defineCommand('SIE.Web.Core.Common.Commands.PrintExportCommand', {
    extend: "SIE.Web.Common.Prints.Commands.PrintPreviewCommand",
    meta: { text: "导出报告", group: "view", iconCls: "icon-PrintData icon-blue" },

    entityType: null, //打印实体类型
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current === null) { return false; }
        if (view.getSelection().length !== 1) { return false; }
        return true;
    },
    execute: function (view, source) {
        var mefun = this;
        var me = view;
        var indata = {};
        indata.Type = this.entityType || me.model; //支持自定义ViewModel类型来打印

        SIE.invokeCommand({
            cmd: "SIE.Web.Common.Prints.Commands.GetTemplateCommand",
            data: indata,
            token: this.view.token,
            callback: function (res) {
                if (res.Success) {
                    if (res.Result.length == 1) {
                        var indata = res.Result[0];
                        me.execute({
                            data: indata,
                            withIds: true,
                            selectIds: me.getSelectionIds(),
                            success: function (res) {

                                var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({ ReportType: indata.Type, ReportData: { path: res.Result, content: res.Result } });
                                var cfg = printCmpt.getExtTarget();
                                if (cfg && cfg.printCallback) {
                                    cfg.printCallback(printCmpt);
                                }
                                else {
                                    var param = printCmpt.getPrintParams();
                                    if (!printCmpt.hasError()) {
                                        printUrl = '/SimpleList/Reports/ExportReport';
                                        var winId = me.getSelectionIds() + '_rpt';
                                        mefun.showPageDialog({ id: winId, text: "单据打印".t(), method: 'POST', url: printUrl, params: param });
                                    }
                                }

                            }
                        });
                    }
                    else {
                        var templates = res.Result;
                        mefun.showSelectView(me, templates);
                    }
                }
            }
        });
    },
    showSelectView: function (view, datas) {
        var _this = this;
        var me = view;
        SIE.AutoUI.getMeta({
            model: "SIE.Common.Prints.PrintTemplate",
            ignoreCommands: true,
            isReadonly: true,

            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var listView = SIE.AutoUI.createListView(mainBlock);
                listView.getData().add(datas);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: '选择模板'.t(),
                    items: ui,
                    width: 800,
                    height: 500,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var selection = listView.getSelection();
                            if (selection.length > 1 || selection.length <= 0) {
                                Ext.Msg.show({
                                    title: '错误'.t(),
                                    message: '请选择选择一行'.t()
                                });
                                return false;
                            }

                            var indata = listView.getCurrent().data;

                            me.execute({
                                data: indata,
                                withIds: true,
                                selectIds: me.getSelectionIds(),
                                success: function (res) {
                                    var printCmpt = new SIE.Web.Common.Prints.Report.WebReportComponents({ ReportType: indata.Type, ReportData: { path: res.Result, content: res.Result } });
                                    var cfg = printCmpt.getExtTarget();
                                    if (cfg && cfg.printCallback) {
                                        cfg.printCallback(printCmpt);
                                    }
                                    else {
                                        var param = printCmpt.getPrintParams();
                                        if (!printCmpt.hasError()) {
                                            var printUrl = '/SimpleList/Reports/ExportReport';
                                            var winId = me.getSelectionIds() + '_rpt';
                                            _this.showPageDialog({ id: winId, text: "单据打印".t(), method: 'POST', url: printUrl, params: param });
                                        }
                                    }
                                }
                            });
                        }
                    },
                });
                //

            },
        });
    },
    showPageDialog: function (opt) {
        var rawId = opt.mid || opt.id;
        var isLoadedClose = opt.isLoadedClose || false;
        var winId = ('win_' + rawId).replace(/[.|,]/g, '');
        var win = Ext.getCmp(winId);

        var url = opt.url || opt.Url;
        if (url) {
            url = SIE.Util.Url.urlConvert(url);
            var iframeId = winId + '_iframeEl';
            if (!win) {
                win = new Ext.Window({
                    modal: true,
                    title: opt.text,
                    id: winId,
                    maximizable: true,
                    monitorResize: true,
                    draggable: false, // 禁止移动   
                    resizable: false,
                    layout: 'fit',
                    plain: true,
                    buttonAlign: 'right',
                    //items: { id: 'win_panel', contentEl: iframe },
                    listeners: {
                        close: function (w) {
                            w.restore(); // 关闭窗口前先还原,滚动条才不会消失   
                        },
                        beforeclose: function () {
                            if (iframe.contentWindow.beforeClose) {
                                return iframe.contentWindow.beforeClose();
                            }
                            return true;
                        },
                        maximize: function (w) {
                            //最大化后需要将窗口重新定位，否则窗口会从最顶端开始最大化   
                            w.setPosition(document.body.scrollLeft, document.body.scrollTop);
                        }
                    }
                });
                win.html = Ext.String.format('<iframe  id="{0}" name="{0}" data-ref="iframeEl" width="100%" height="99%" scrolling="auto" frameborder="0"  ></iframe>', iframeId);
                win.view = opt.view;
            }
            //win = win.show();
            if (opt.method === "POST") {
                var tempForm = document.createElement("form");
                //制定发送请求的方式为post  
                tempForm.method = opt.method || "GET";
                //此为window.open的url，通过表单的action来实现  
                tempForm.action = url;
                //利用表单的target属性来绑定window.open的一些参数（如设置窗体属性的参数等）  
                tempForm.target = iframeId;
                if (opt.params) {
                    for (var i in opt.params) {
                        var hideInput = document.createElement("input");
                        hideInput.type = "hidden";
                        hideInput.name = i;
                        hideInput.value = opt.params[i];
                        //将input表单放到form表单里  
                        tempForm.appendChild(hideInput);
                    }
                }

                //将此form表单添加到页面主体body中  
                document.body.appendChild(tempForm);
                //手动触发，提交表单  
                tempForm.submit();
                //从body中移除form表单  
                document.body.removeChild(tempForm);
            }
            /*       var panel = Ext.getCmp('win_panel');
                   panel.setLoading(true);
                   Ext.get(iframe).on('load', function () {
                       panel.setLoading(false);
                       if (isLoadedClose) {
                           win.close();
                       }
                   });*/
        }
    }

});