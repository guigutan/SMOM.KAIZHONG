SIE.defineCommand('SIE.Web.MES.TaskManagement.SchedulingInfs.Commands.SchedulingInfReportExportCommand', {
    extend: 'SIE.cmd.ExportXls',
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据

    execute: function (view, source) {
        var me = this;
        var dataCount = view.getData().data.items.length;
        //数据存在时允许导出
        if (dataCount === 0) {
            SIE.Msg.showInstantMessage('没有需要导出的数据！'.t());
            return false;
        }
        SIE.Signature.otherCheckIsNeedToSign("导出", view, function () {
            me.exportXls(view.gridConfig, view.getData().getData().items, view.label.t());
        });

    },
    exportXls: function (cfg, datas, title, mask) {
        mask = mask || SIE.ExportExcelHelper.showMask(Ext.getBody().component, "下载中...".t());
        var add_cols = function (col, cfg) {
            var a = col.addColumn({
                text: cfg.header,
                width: cfg.width || 140
            });
            cfg.columns && cfg.columns.forEach(function (c) {
                add_cols(a, c)
            })
        },
            task = new Ext.util.DelayedTask(function () {
                var columns = Ext.create("Ext.exporter.data.Column"), add_val, rows, excelData, excel;
                cfg.columns.forEach(function (col) {
                    if (col.xtype === "rownumberer")
                        return !0;
                    add_cols(columns, col)
                });
                add_val = function (row, item, cfg) {
                    cfg.columns && cfg.columns.forEach(function (col) {
                        var colIdx, arr, fmt;
                        if (add_val(row, item, col),
                            colIdx = col.dataIndex,
                            !!!colIdx)
                            return !0;
                        colIdx.indexOf("Id") > 0 && item.data[colIdx + "_Display"] !== undefined ? val = item.data[colIdx + "_Display"] : (val = item.data[colIdx],
                            (col.xtype === "comboboxcolumn" || col.editor && col.editor.xtype === "xcombobox") && (col.editor.store.data[0] instanceof Array ? (arr = col.editor.store.data.filter(function (d) {
                                return d[0] === val
                            }),
                                arr.length > 0 && (val = arr[0][1])) : (arr = col.editor.store.data.filter(function (d) {
                                    return d[col.editor.valueField] === val
                                }),
                                    val = arr.length > 0 ? arr[0][col.editor.displayField] : "")),
                            col.xtype === "comboColumn" && (arr = JSON.parse(col.editor.store.data).filter(function (d) {
                                return d.Code === val
                            }),
                                arr.length > 0 && (val = arr[0].Name)),
                            col.xtype === "checkboxcolumn" && (val = val ? "是".t() : "否".t()));
                        val instanceof Date && (fmt = "Y-m-d",
                            Ext.Date.format(val, "H:i:s") !== "00:00:00" && (fmt = "Y-m-d H:i:s"),
                            val = Ext.Date.format(val, fmt));
                        row.addCell({
                            value: val
                        })
                    })
                }
                    ;
                rows = [];
                datas.forEach(function (item) {
                    var row = Ext.create("Ext.exporter.data.Row");
                    add_val(row, item, cfg);
                    rows.push(row)
                });
                excelData = Ext.create("Ext.exporter.data.Table", {
                    columns: columns,
                    rows: rows
                });
                excel = Ext.create("SIE.Web.Core.Common.Scripts.KzXlsx"/*"Ext.exporter.excel.Xlsx"*/, {
                    fileName: title + Ext.Date.format(new Date, "YmdHis") + ".xlsx",
                    title: "",          // 标题内容（设为null/''）
                    data: excelData,
                    showTitle: false,  // 明确禁用标题行
                    startRow: -1,          // 数据起始行索引（部分版本支持）
                    startColumn: 0,       // 数据起始列索引（部分版本支持）
                    showHeader: false,     // 显示表头行
                    ignorePaging: true,   // 忽略分页（避免额外空行）
                });
                excel.saveAs();
                SIE.Msg.showInstantMessage("成功导出[{0}]笔数据！".t().format(rows.length));
                mask.hide()
            }
            );
        task.delay(50)
    },

});