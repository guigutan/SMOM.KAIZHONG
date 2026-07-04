SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.ExportProjectCommand', {
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    execute: function (view) {
        var me = this;
        var record = view._relations[0]._target.getCurrent();
        var criteria = record.data;

        //电子签名信息
        var signdata = {
            command: me.meta.command,
            entityType: me.view.model,
            parentType: me.view.getParent() ? me.view.getParent().model : ""
        }

        SIE.invokeDataQuery({
            method: 'ExportProject',
            params: [criteria],
            action: 'queryer',
            type: 'SIE.Web.EMS.EarlierStage.Projects.ProjectDataQueryer',
            token: view.getToken(),
            logInfo: signdata,
            success: function (res) {
                if (res.Success) {
                    var exportData = res.Result;
                    if (exportData && exportData.Tables && exportData.Tables.length === 0) {
                        me.timer = Ext.defer(function () {
                            me.timer = null;
                            Ext.MessageBox.hide();
                        }, 1000);
                        SIE.Msg.showMessage("没有可导出的数据".L10N());
                    }
                    else {
                        me.generateExcel(exportData);
                        me.timer = Ext.defer(function () {
                            me.timer = null;
                            Ext.MessageBox.hide();
                        }, 1000);
                    }
                }
            }
        });
    },
    generateExcel: function (exportData) {
        if (!exportData)
            throw new Error('导出数据不能为空'.L10N());
        if (!exportData.Tables || exportData.Tables.length === 0)
            throw new Error('没有导出数据'.L10N());
        if (!exportData.Columns || exportData.Columns.length == 0)
            throw new Error('导出栏位不能为空'.L10N());
        var tmplWorkbookXML = '<?xml version="1.0"?><?mso-application progid="Excel.Sheet"?>'
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
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
        function getSheetHead(columns) {
            var head = "<Row>";
            columns.forEach(function (column) {
                head += "<Cell ss:StyleID='headStyle'><Data ss:Type='String'>" + column + "</Data></Cell>";
            });
            head += "</Row>";
            return head;
        }
        var ctx = "";
        var workbookXML = "";
        var worksheetsXML = "";
        var rowsXML = "";
        var tables = exportData.Tables;
        for (var i = 0; i < tables.length; i++) {
            var table = tables[i];
            var colums = i <= exportData.Columns.length - 1 ? exportData.Columns[i] : exportData.Columns[0];
            rowsXML += getSheetHead(colums);
            for (var j = 0; j < table.length; j++) {
                rowsXML += '<Row>';
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
        var fileName = '项目管理'.t() + Ext.util.Format.date(new Date(), 'Ymdhis') + '.xls';
        //兼容IE,Edge
        if (navigator.msSaveOrOpenBlob) {
            workbookXML = format(tmplWorkbookXML, ctx);
            var blob = new Blob(['\ufeff', workbookXML], {
                type: "application/vnd.ms-excel" + ";charset=utf-8;"
            });
            window.navigator.msSaveBlob(blob, fileName);
        } else {
            workbookXML = format(tmplWorkbookXML, ctx);
            var blobc = new Blob([workbookXML], { type: "application/vnd.ms-excel" + ";charset=utf-8;" });
            var link = document.createElement('a');
            link.style = "visibility:hidden";
            link.href = URL.createObjectURL(blobc)
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
});