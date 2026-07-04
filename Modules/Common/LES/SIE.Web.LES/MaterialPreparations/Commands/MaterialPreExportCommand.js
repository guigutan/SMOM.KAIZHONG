SIE.defineCommand('SIE.Web.LES.MaterialPreparations.Commands.MaterialPreExportCommand', {
    meta: { text: "导出", group: "edit", iconCls: "icon-ExportData icon-blue" },
    selOption: null,
    execute: function (view) {
        var me = this;
        var criter = view._relations[0]._target.getCurrent();
        delete criter.data['CriteriaModuleKey'];
        delete criter.data['CriteriaType'];
        delete criter.data["CriteriaString"];
        var token = view.getToken();
        var store = me.initStore();
        var win = Ext.create("Ext.window.Window", {
            title: "导出选项".t(), //标题            
            draggable: false,
            bodyStyle: 'padding:10px 30px 10px 30px',
            height: 200, //高度
            width: 300, //宽度
            modal: true, //是否模态窗口，默认为false
            resizable: false,
            labelWidth: 40,
            closeAction: 'close',
            autoDestroy: true,
            items: [
                {
                    xtype: 'combobox',
                    name: 'rangeCb',
                    fieldLabel: '数据选项'.t(),
                    labelStyle: 'width:80px;',
                    valueField: "key",
                    displayField: "value",
                    store: store,
                    listeners: {
                        afterRender: function () {
                            me.selOption = Ext.ComponentQuery.query('combobox[name=rangeCb]');
                            me.selOption = me.selOption[me.selOption.length - 1];
                            document.getElementById(me.selOption.id).children[0].children[0].style.width = 'auto';
                        }
                    }
                }
            ],
            buttons: [{
                text: '保存'.t(),
                handler: function () {
                    var rangeOption = me.selOption.getValue();
                    var seleneity = view.getSelection();
                    var modellist = new Array();//导出选中行
                    var pagesize = view._pagingBar.store.pageSize;
                    var currentpage = view._pagingBar.store.currentPage;
                    if (rangeOption == "1") {
                        if (seleneity.length == 0) { SIE.Msg.showError("请选中至少一行再导出".t()); return; }
                        for (var i = 0; i < seleneity.length; i++) {
                            modellist.push(seleneity[i].data);
                        }
                    }
                    var signdata = {
                        command: me.meta.command,
                        entityType: me.view.model,
                        parentType: me.view.getParent() ? me.view.getParent().model : ""
                    }
                    Ext.MessageBox.show({
                        msg: '正在导出数据'.t(),
                        progressText: '...',
                        width: 300,
                        wait: {
                            interval: 200
                        }
                    });
                    SIE.invokeDataQuery({
                        method: 'GetExportData',
                        params: [rangeOption, modellist, pagesize, currentpage, criter.data],
                        action: 'queryer',
                        type: 'SIE.Web.LES.MaterialPreparations.MaterialPreparationDataqueryer',
                        token: token,
                        logInfo: signdata,
                        success: function (res) {
                            var exportData = res.Result[0]['exportData'];
                            var div = document.createElement("DIV");
                            document.body.appendChild(div);
                            div.innerHTML = exportData;
                            div.style.display = "none";
                            var l = div.children.length;
                            var catearr = [];
                            catearr.push("sheet1");
                            var datestr = Ext.util.Format.date(new Date(), 'Ymdhis');
                            me.table2Excel(div, catearr, "备料需求单".t() + datestr + ".xls", "Excel");
                            document.body.removeChild(div);

                            me.timer = Ext.defer(function () {
                                me.timer = null;
                                Ext.MessageBox.hide();
                                win.close();
                                Ext.toast({
                                    html: "导出成功".t(),
                                    closable: false,
                                    align: 't',
                                    slideInDuration: 400
                                });
                            }, 2000);
                        }
                    });
                }
            }, {
                text: '取消'.t(),
                handler: function () {
                    win.close();
                }
            }],
            autoScroll: true,
            listeners: {
                afterrender: function () {

                },
                beforeclose: function () {

                }
            }
        });
        win.show();
    },
    initStore: function () {
        var optionDataStore = new Ext.data.SimpleStore({
            fields: [
                { name: 'key', mapping: 'key' },
                { name: 'value', mapping: 'value' }
            ],
            data: [{ 'key': '0', 'value': '当前页' },
            { 'key': '1', 'value': '选中行' },
            { 'key': '2', 'value': '查询结果' }]
        });
        return optionDataStore;
    },
    tablesToExcel: function (tables, title) {
        var task = new Ext.util.DelayedTask(function () {
            var columns = Ext.create('Ext.exporter.data.Column');
            for (var i = 0; i < tables[0].rows[0].cells.length; i++) {
                var col = tables[0].rows[0].cells[i];
                columns.addColumn({ text: col.innerHTML, width: 140 });
            }

            var rows = [];
            for (var i = 1; i < tables[0].rows.length; i++) {
                var row = Ext.create('Ext.exporter.data.Row');
                for (var k = 0; k < tables[0].rows[i].cells.length; k++) {
                    var dataValue = tables[0].rows[i].cells[k].getAttribute("data-value");
                    dataValue = (dataValue) ? dataValue : tables[0].rows[i].cells[k].innerHTML;

                    row.addCell({ value: dataValue });
                }
                rows.push(row);
            }
            var excelData = Ext.create('Ext.exporter.data.Table', {
                columns: columns,
                rows: rows
            });
            var excel = Ext.create('Ext.exporter.excel.Xlsx', {
                fileName: title + Ext.Date.format(new Date(), 'YmdHis') + '.xlsx',
                title: title,
                author: 'SIE',
                data: excelData
            });
            excel.saveAs();
            SIE.Msg.showInstantMessage('导出成功！'.t());

        });
        task.delay(50);
    },

    table2Excel: function (tableid, wsnames, wbname, appname) {
        var workbookXML = "";
        var base64 = function (s) {
            return window.btoa(unescape(encodeURIComponent(s)));
        };
        var excelFormat = function (s, c) {
            return s.replace(/{(\w+)}/g,
                function (m, p) {
                    return c[p];
                });
        };
        var uri = 'data:application/vnd.ms-excel;base64,';
        var template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"' +
            'xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>'
            + '<x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>'
            + '</x:ExcelWorkbook></xml><![endif]-->' +
            ' <style type="text/css">' +
            'table td {' +
            'border: 1px solid #000000;' +
            'width: 200px;' +
            'height: 30px;' +
            ' text-align: center;' +
            ' }' +
            '</style>' +
            '</head><body ><table class="excelTable">{table}</table></body></html>';
        if (!tableid.nodeType) tableid = document.getElementById(tableid);
        var ctx = { worksheet: 'Worksheet', table: tableid.innerHTML };
        workbookXML = excelFormat(template, ctx);
        if (navigator.msSaveOrOpenBlob) {
            uri = 'data:application/vnd.ms-excel;charset=utf-8,';
            var data = uri + base64(workbookXML);
            var blob = new Blob(['\ufeff', data], {
                type: 'application/vnd.ms-excel'
            });
            navigator.msSaveOrOpenBlob(blob, wbname || 'Workbook.xls');
        }
        else {
            var link = document.createElement("A");
            link.href = uri + base64(workbookXML);
            link.download = wbname || 'Workbook.xls';
            link.target = '_blank';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
});




