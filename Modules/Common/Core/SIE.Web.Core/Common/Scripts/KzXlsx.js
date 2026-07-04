Ext.define('SIE.Web.Core.Common.Scripts.KzXlsx', {
    extend: 'Ext.exporter.excel.Xlsx',
    alternateClassName: 'Ext.exporter.Excel',
    alias: [
        'exporter.excel07',
        'exporter.xlsx',
        // last version of excel supported will get this alias
        'exporter.excel'
    ],
    requires: [
        'Ext.exporter.file.ooxml.Excel'
    ],
    buildHeader: function () {
        var me = this,
            ret = {},
            data = me.getData(),
            rows = [],
            keys, row, i, j, len, lenCells, style, arr, fStyle, col, colCfg, cell;
        me.buildHeaderRows(data.getColumns(), ret);
        keys = Ext.Object.getKeys(ret);
        len = keys.length;
        for (i = 0; i < len; i++) {
            row = {
                height: me.headerRowHeight,
                styleId: me.tableHeaderStyleId,
                cells: []
            };
            arr = ret[keys[i]];
            lenCells = arr.length;
            for (j = 0; j < lenCells; j++) {
                cell = arr[j];
                cell.styleId = me.tableHeaderStyleId;
                row.cells.push(cell);
            }
            rows.push(row);
        }
        if (me.config.title == '' || me.config.title == null) {
            //清除掉第一行空白行
            rows.remove(rows[0]);
        }
        arr = data.getBottomColumns();
        lenCells = arr.length;
        me.columnStylesNormal = [];
        me.columnStylesNormalId = [];
        me.columnStylesFooter = [];
        me.columnStylesFooterId = [];
        fStyle = me.getGroupFooterStyle();
        for (j = 0; j < lenCells; j++) {
            col = arr[j];
            colCfg = {
                style: col.getStyle(),
                width: col.getWidth()
            };
            style = Ext.applyIf({
                parentId: 0
            }, fStyle);
            style = Ext.merge(style, colCfg.style);
            me.columnStylesFooter.push(style);
            me.columnStylesFooterId.push(me.excel.addCellStyle(style));
            style = Ext.applyIf({
                parentId: 0
            }, colCfg.style);
            me.columnStylesNormal.push(style);
            colCfg.styleId = me.excel.addCellStyle(style);
            me.columnStylesNormalId.push(colCfg.styleId);
            colCfg.min = colCfg.max = j + 1;
            colCfg.style = null;
            if (colCfg.width) {
                colCfg.width = colCfg.width / 10;
            }
            me.worksheet.addColumn(colCfg);
        }
        return rows;
    },
});