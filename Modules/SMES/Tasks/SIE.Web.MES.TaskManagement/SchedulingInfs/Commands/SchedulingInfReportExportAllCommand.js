SIE.defineCommand('SIE.Web.MES.TaskManagement.SchedulingInfs.Commands.SchedulingInfReportExportAllCommand', {
    extend: 'SIE.cmd.ExportXlsAll',
    meta: { text: "导出全部", group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据
    pageSize: 500000,

    execute: function (view, source) {

        var mask = SIE.ExportExcelHelper.showMask(Ext.getBody().component, '数据准备中...');

        myview = view;
        var me = this;

        var store = SIE.AutoUI.viewFactory._createStore(myview.getMeta());
        Ext.Object.merge(store.getFilters(), myview.getControl().store.getFilters());
        store.setPageSize(me.pageSize);
        var proxy = store.proxy;
        proxy.setExtraParams({});
        proxy.setExtraParam("token", myview.getToken());
        if (!(proxy.extraParams && proxy.extraParams.action)) proxy.setExtraParam("action", proxy.action || "entity");
        if (!(proxy.extraParams && proxy.extraParams.type)) proxy.setExtraParam("type", myview.model);
        proxy.setExtraParam("viewGroup", myview.viewGroup);
        proxy.setExtraParam("url", proxy.url);
        var parent = myview._parent;

        if (parent && parent._current) {
            var pName = myview._childProperty;
            if (!pName) {
                proxy.setExtraParam("action", "delegate");
                proxy.setExtraParam("parent", parent.model);
                proxy.setExtraParam("filter", Ext.encode([
                    {
                        property: SIE._KeyPropertyName,
                        value: parent._current.data[SIE._KeyPropertyName],
                        exactMatch: true
                    }]));
            }
        }
        store.load({
            scope: myview,
            callback: function (records, operation, success) {
                mask.hide();
                store._loaded = success;
                if (records.length === 0) {
                    SIE.Msg.showInstantMessage('没有需要导出的数据！'.t());
                    return false;
                }
                SIE.Signature.otherCheckIsNeedToSign("导出全部", view, function () {
                    me.exportXls(myview.gridConfig, records, myview.label.t());
                });
            }
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
                excel = Ext.create("SIE.Web.Core.Common.Scripts.KzXlsx", {
                    fileName: title + Ext.Date.format(new Date, "YmdHis") + ".xlsx",
                    //title: title,
                    data: excelData
                });
                excel.saveAs();
                SIE.Msg.showInstantMessage("成功导出[{0}]笔数据！".t().format(rows.length));
                mask.hide()
            }
            );
        task.delay(50)
    },

});