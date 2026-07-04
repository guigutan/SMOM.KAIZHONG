/**
 * 导出Excel帮助类
 */
Ext.define('SIE.Web.MES.Common.Scripts.Helpers.ExportExcelHelper', {
    statics: {
        /**
         * 导出Excel（支持多页签）
         * @param {SIE.MES.Common.Models.ExportDataTable} exportData 导出数据
         * @param {String} excelName Excel文件名称
         * @param {Boolean} isSameHead 多页签是否相同列头
         */
        tablesToMultiSheetExcel: function (exportData, excelName, isSameHead) {
            var me = this;
            if (!exportData)
                throw new Error('导出数据不能为空'.L10N());
            if (!exportData.Tables || exportData.Tables.length === 0)
                throw new Error('没有导出数据'.L10N());
            if (!exportData.Columns || exportData.Columns.length == 0)
                throw new Error('导出栏位不能为空'.L10N());
            var uri = 'data:application/vnd.ms-excel;base64,'
                , tmplWorkbookXML = '<?xml version="1.0"?><?mso-application progid="Excel.Sheet"?>'
                    + '<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">'
                    + '<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office"></DocumentProperties>'
                    + '<Styles>'
                    + "<Style ss:ID='headStyle'><Font ss:FontName='宋体' ss:Size='11' ss:Color='#ffffff' ss:Bold='1'/><Interior ss:Color='#00868b' ss:Pattern='Solid'/><NumberFormat/></Style>"
                    + "<Style ss:ID='fixRowStyle'><Font ss:FontName='宋体' ss:Size='11' ss:Color='#00000' ss:Bold='1'/></Style>"
                    + '</Styles>'
                    + '{worksheets}'
                    + '</Workbook>'
                , tmplWorksheetXML = '<Worksheet ss:Name="{nameWS}"><Table>{rows}</Table></Worksheet>'
                , tmplCellXML = '<Cell><Data ss:Type="String">{data}</Data></Cell>'
                , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
            function getSheetHead(columns) {
                var head = "<Row>";
                columns.forEach(function (column) {
                    head += "<Cell ss:StyleID='headStyle'><Data ss:Type='String'>" + column + "</Data></Cell>";
                    //ctx = { data: column };
                    //head += format(tmplCellXML, ctx);
                });
                head += "</Row>";
                return head;
            }
            var ctx = "";
            var workbookXML = "";
            var worksheetsXML = "";
            var rowsXML = "";
            var tables = exportData.Tables;
            var head = '';
            if (isSameHead === true) {
                head = getSheetHead(exportData.Columns[0]);
            }
            //设置内容数据
            for (var i = 0; i < tables.length; i++) {
                var table = tables[i];
                if (isSameHead === true)
                    rowsXML += head;
                else {
                    var colums = i <= exportData.Columns.length - 1 ? exportData.Columns[i] : exportData.Columns[0];
                    rowsXML += getSheetHead(colums);
                }
                //控制要导出内容行
                for (var j = 0; j < table.length; j++) {
                    rowsXML += '<Row>';

                    //控制要导出的列数
                    for (var key in table[j]) {
                        var dataValue = table[j][key] == null ? "" : table[j][key];
                        ctx = { data: dataValue };
                        rowsXML += format(tmplCellXML, ctx);
                    }
                    rowsXML += '</Row>'
                }

                ctx = { rows: rowsXML, nameWS: exportData.SheetNames[i] || 'Sheet' + i };
                worksheetsXML += format(tmplWorksheetXML, ctx);
                rowsXML = "";
            }

            ctx = { created: (new Date()).getTime(), worksheets: worksheetsXML };

            var fileName = excelName ? excelName + '.xls' : 'excel_data.xls';
            //兼容IE,Edge
            if (navigator.msSaveOrOpenBlob) {

                workbookXML = format(tmplWorkbookXML, ctx);

                var blob = new Blob(['\ufeff', workbookXML], {
                    type: "application/vnd.ms-excel" + ";charset=utf-8;"
                });
                window.navigator.msSaveBlob(blob, fileName)

            } else {

                workbookXML = format(tmplWorkbookXML, ctx);

                //得到编码后的地址
                var blob = new Blob([workbookXML], { type: "application/vnd.ms-excel" + ";charset=utf-8;" });

                var link = document.createElement('a');
                link.style = "visibility:hidden";
                link.href = URL.createObjectURL(blob)
                link.download = fileName;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        },
    }
});