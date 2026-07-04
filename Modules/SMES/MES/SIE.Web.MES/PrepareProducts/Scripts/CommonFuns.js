Ext.define('SIE.Web.MES.PrepareProducts.Scripts.CommonFuns', {
    statics: {

        /**
         * 导出Excel
         * @param {any} tables
         * @param {any} title
         */
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
        }


    }
});