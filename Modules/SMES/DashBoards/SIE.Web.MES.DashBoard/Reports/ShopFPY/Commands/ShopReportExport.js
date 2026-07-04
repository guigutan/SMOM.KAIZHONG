SIE.defineCommand('SIE.Web.MES.DashBoard.Reports.ShopFPY.Commands.ShopRrportExport', {
    meta: { text: "导出Excel", group: "business", iconCls: "icon-ExportData icon-blue" },
    _view: null,
    execute: function (view) {
        _view = view;
        var me = this;
        var record = view._relations[0]._target.getCurrent();
        delete record.data['CriteriaModuleKey'];
        delete record.data['CriteriaType'];
        delete record.data["CriteriaString"];
        var istrue = true;      
        SIE.invokeDataQuery({
            method: 'ExportShopReportRecords',
            params: [record.data],
            action: 'queryer',
            type: 'SIE.Web.MES.DashBoard.Reports.ShopReportDataQueryer',
            token: view.getToken(),
            success: function (res) {
                var exportData = res.Result;
                if (exportData == "") {
                    SIE.Msg.showMessage("没有可导出的数据".t());
                }
                else {
                    SIE.Signature.otherCheckIsNeedToSign("导出Excel", view, function () {
                        var div = document.createElement("DIV");
                        document.body.appendChild(div);
                        div.innerHTML = exportData;
                        div.style.display = "none";
                        var l = div.children.length;
                        SIE.Web.MES.CommonFuns.tablesToExcel(div.children, ['车间直通率报表'], "车间直通率.xls".t(), "Excel");
                        document.body.removeChild(div)
                        Ext.MessageBox.show({
                            msg: '正在导出数据'.t(),
                            progressText: '...',
                            width: 300,
                            wait: {
                                interval: 200
                            }
                        });
                        me.timer = Ext.defer(function () {
                            me.timer = null;
                            Ext.MessageBox.hide();
                            Ext.toast({
                                html: "导出成功".t(),
                                closable: false,
                                align: 't',
                                slideInDuration: 400
                            });
                        }, 2000);
                    });
                }
            }
        });
    },
});

