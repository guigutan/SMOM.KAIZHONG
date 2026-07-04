/**
 * 导出Excel帮助类
 */
Ext.define('SIE.Web.Core.Common.Scripts.Helpers.DataTableExportExcelHelper', {
    statics: {
        /**
         * 导出Excel
         * @param {SIE.MES.Common.Models.ExportDataTable} exportData 导出数据
         * @param {String} title Excel文件名称
         * @param {Boolean} mask 显示遮罩
         */
        tablesToSheetExcel: function (exportData, title, mask) {
            if (!exportData)
                throw new Error('导出数据不能为空'.L10N());
            if (!exportData.Tables || exportData.Tables.length === 0)
                throw new Error('没有导出数据'.L10N());
            if (!exportData.Columns || exportData.Columns.length == 0)
                throw new Error('导出栏位不能为空'.L10N());
            mask = mask || this.showMask(Ext.getBody().component, '下载中...');
            var task = new Ext.util.DelayedTask(function () {
                var table = exportData.Tables[0];
                var tableColumns = exportData.Columns[0];
                //列名
                var columns = Ext.create('Ext.exporter.data.Column');
                tableColumns.forEach(function (col) {
                    columns.addColumn({ text: col, width: 140 });
                });
                //行列数据
                var rows = [];
                for (var j = 0; j < table.length; j++) {
                    var row = Ext.create('Ext.exporter.data.Row');
                    for (var key in table[j]) {
                        var val = table[j][key] == null ? "" : table[j][key];
                        row.addCell({ value: val });
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
                SIE.Msg.showInstantMessage('成功导出【{0}】笔数据！'.t().format(table.length));
                mask.hide();
            });
            task.delay(50);
        },
        /**
         * 显示遮罩
         * @method showMask
         * @param  target 目标控件
         * @param msg 显示信息
        */
        showMask: function (target, msg) {
            msg = msg || '读取中...';
            var mask = new Ext.LoadMask({
                target: target,
                msg: msg,
            });
            mask.show();
            return mask;
        },
    }
});