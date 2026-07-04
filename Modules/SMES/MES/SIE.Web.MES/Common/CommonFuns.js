Ext.define('SIE.Web.MES.CommonFuns', {
    statics: {  
        /**
        * Listview界面重新加载，用于带查询块的ListView
        * @param {} view 
        * @returns {} 
        */
        mainReloadData: function (view) {
            if (view.isListView) {
                //当ReloadData之前未查询过，则执行查询块的查询方法。以免不使用查询实体来查询
                if (!view._lastDataArgs) {
                    var conditionView = view.getConditionView();
                    if (conditionView) {
                        conditionView._commands.items.first(function (p) { return p.meta.command === "SIE.cmd.ExecuteQuery"; }).execute(conditionView);
                        return;
                    }
                }
                view.reloadData();
            }
        },
        /**
      * 时间转换
      * @param {} js date 
      * @returns {} 年月日 时分秒 
      */
        dateTypeChange: function (date) {
            date = new Date(date);
            return date.toLocaleDateString() + ' ' + date.toTimeString().substring(0, 8);
        },
        /**
 * Html的table转成Excel导出
 * @param {tables} tables 
 * @param {worksheet} wsnames 
 * @param {导出的excel名称} wbname 
 * @param {导出的文件类型(Excel)} appname 
 * @returns {void} 
 */
        tablesToExcel: function (tables, wsnames, wbname, appname) {
            var uri = 'data:application/vnd.ms-excel;base64,'
        , tmplWorkbookXML = '<?xml version="1.0"?><?mso-application progid="Excel.Sheet"?><Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">'
        + '<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office"><Author>Axel Richter</Author><Created>{created}</Created></DocumentProperties>'
        + '<Styles>'
        + '<Style ss:ID="Currency"><NumberFormat ss:Format="Currency"></NumberFormat></Style>'
        + '<Style ss:ID="Date"><NumberFormat ss:Format="Medium Date"></NumberFormat></Style>'
        + '</Styles>'
        + '{worksheets}</Workbook>'
        , tmplWorksheetXML = '<Worksheet ss:Name="{nameWS}"><Table>{rows}</Table></Worksheet>'
        , tmplCellXML = '<Cell{attributeStyleID}{attributeFormula}><Data ss:Type="{nameType}">{data}</Data></Cell>'
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };

            var ctx = "";
            var workbookXML = "";
            var worksheetsXML = "";
            var rowsXML = "";

            for (var i = 0; i < tables.length; i++) {
                if (!tables[i].nodeType) tables[i] = document.getElementById(tables[i]);

                //           控制要导出的行数
                for (var j = 0; j < tables[i].rows.length; j++) {
                    rowsXML += '<Row>';

                    for (var k = 0; k < tables[i].rows[j].cells.length; k++) {
                        var dataType = tables[i].rows[j].cells[k].getAttribute("data-type");
                        var dataStyle = tables[i].rows[j].cells[k].getAttribute("data-style");
                        var dataValue = tables[i].rows[j].cells[k].getAttribute("data-value");
                        dataValue = (dataValue) ? dataValue : tables[i].rows[j].cells[k].innerHTML;
                        var dataFormula = tables[i].rows[j].cells[k].getAttribute("data-formula");
                        dataFormula = (dataFormula) ? dataFormula : (appname == 'Calc' && dataType == 'DateTime') ? dataValue : null;
                        ctx = {
                            attributeStyleID: (dataStyle == 'Currency' || dataStyle == 'Date') ? ' ss:StyleID="' + dataStyle + '"' : ''
            , nameType: (dataType == 'Number' || dataType == 'DateTime' || dataType == 'Boolean' || dataType == 'Error') ? dataType : 'String'
            , data: (dataFormula) ? '' : dataValue
            , attributeFormula: (dataFormula) ? ' ss:Formula="' + dataFormula + '"' : ''
                        };
                        rowsXML += format(tmplCellXML, ctx);
                    }
                    rowsXML += '</Row>';
                }
                ctx = { rows: rowsXML, nameWS: wsnames[i] || 'Sheet' + i };
                worksheetsXML += format(tmplWorksheetXML, ctx);
                rowsXML = "";
            }

            ctx = { created: (new Date()).getTime(), worksheets: worksheetsXML };
            workbookXML = format(tmplWorkbookXML, ctx);

            //       查看后台的打印输出
            //console.log(workbookXML);
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
    }
});