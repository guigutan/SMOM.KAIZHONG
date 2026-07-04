SIE.defineCommand('SIE.Web.MES.TaskManagement.DailyOutputReports.Commands.DailyOutputReportExportAllCommand', {
    meta: { text: "全部导出Excel", hierarchy: "导出".t(), group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据
    pageSize: 500000,
    canExecute: function (view) {
        //var flag = true;
        //var dataCount = view.getData().data.length;
        //if (dataCount == 0) //数据存在时允许导出
        //    flag = false;
        return true;
    },
    execute: function (view, source) {
        if (view.getData().isDirty()) {
            SIE.Msg.showInstantMessage('请保存后，再导出列表数据!');
            return;
        }
        var me = this;
        me.myview = view;
        var exportView = null;
        var conditionView = me.myview.getConditionView();
        var queryOpt = null;
        var sourceStore = me.myview.getControl().store;
        SIE.AutoUI.getMeta({
            async: false,
            model: view.model,
            callback: function (meta) {
                exportView = SIE.AutoUI.createListView(meta);
            }
        });
        //exportView = SIE.AutoUI.createListView(me.myview.getMeta());
        Ext.Object.merge(exportView.getControl().store.getFilters(), sourceStore.getFilters());
        // exportView.getControl().store.setFilters(sourceStore.getFilters());
        exportView.getControl().store.setPageSize(me.pageSize);
        var parentView = me.myview.getParent();
        if (parentView) {
            exportView._setParent(parentView);
        }

        if (me.myview._childProperty) {
            exportView._childProperty = me.myview._childProperty;
        }

        var store = exportView.getControl().store;
        var grid = me.onBeforeDocumentSave(me.myview, store, exportView);

        //var fileName = view.label + Ext.util.Format.date(new Date(), 'Ymdhis');
        //me.ExportToExcel(grid, fileName);
        ////列表的值已经改变，不刷新列表，保存会出错
        //view.getData().reload();

    },
    onBeforeDocumentSave: function (myview, store, exportView) {
        var mask = SIE.ExportExcelHelper.showMask(Ext.getBody().component, '数据准备中...');
        var me = this;
        var columns = myview.gridConfig.columns;
        //获取所有的表头
        me._traversalColumns(columns);
        var grid = myview.getControl();
        store.load({
            scope: exportView,
            callback: function (records, operation, success) {
                mask.hide();
                grid.store = store;
                //导出前，数据处理 grid.getStore().getRange()
                Ext.each(grid.getStore().getRange(), function (record) {
                    var data = record.data;
                    record.data = me._processExportData(data);
                });
                //设置checkbox表头可导出
                for (j = 0, len = grid.columns.length; j < len; j++) {
                    if (grid.columns[j].xtype == "checkboxcolumn" && grid.columns[j].ignoreExport) {
                        grid.columns[j].ignoreExport = false;
                    }
                }
                //return grid;
                var fileName = myview.label + Ext.util.Format.date(new Date(), 'Ymdhis');
                me.ExportToExcel(grid, fileName);
                //列表的值已经改变，不刷新列表，保存会出错
                //myview.getData().reload();
            }
        });


    },
    ExportToExcel: function (view, fileName) {
        var me = this;
        var cfg = {
            type: 'excel07',//这里设置导出的文件类型【excel07，html，csv，xml】
            title: me.view.label,
            fileName: fileName + '.xlsx'
        };
        view.saveDocumentAs(cfg);
    },
    /**
   * 默认导出数据处理函数
   * @param {row} 列表的行数据
   * 可重写此方法，自定义表头，或者用html拼写表头（解决复杂表头导出问题） 
   */
    _processExportData: function (row) {
        var me = this;
        //数据处理
        me.fieldNames.forEach(function (fieldName) {
            var exportValue = row[fieldName.key];

            // 先判断是否为数字类型（避免后续处理转为字符串）
            var isNumber = !isNaN(exportValue) && typeof exportValue === 'number';
            // 仅对非数字类型进行特殊处理（如下拉框、日期等）
            if (!isNumber) {

                switch (fieldName.xtype) {
                    case 'comboColumn':
                        {
                            var catalogValues = JSON.parse(fieldName.value);
                            catalogValues.forEach(function (items) {
                                if (items.Code == exportValue)
                                    exportValue = items.Name;
                            });
                        }
                        break;
                    case 'comboboxcolumn':
                        {
                            fieldName.value.forEach(function (items) {
                                if (items.value === exportValue)
                                    exportValue = items.text;
                            });
                        }
                        break;
                    case 'combolistcolumn':
                        {
                            exportValue = row[fieldName.value];
                        }
                        break;
                    case 'checkboxcolumn':
                        {
                            //导出列表checkbox值的转换
                            exportValue = exportValue == true ? "是" : "否";
                        }
                        break;
                    case 'datecolumn':
                        {
                            //导出列表checkbox值的转换
                            exportValue = me.dateConversion(exportValue);
                        }
                        break;
                    //default:
                    //    {
                    //        if (fieldName.value)
                    //            exportValue = Ext.util.Format.date(exportValue, fieldName.value);_processExportData
                    //    }
                    //    break;
                }
                exportValue = exportValue ? exportValue.toString().replace(/\"/g, "'") : "";//替换字符串中存在的双引号
            }
            else {
                // 数字类型保留原始值（不转为字符串）
                exportValue = exportValue; // 可根据需要做精度处理，如：Number(exportValue.toFixed(2))
            }
            row[fieldName.key] = exportValue;
        });
        return row;
    },
    /**
     * 客户端导出前：处理表格数据
     * @param {xtype} 编辑器类型
     * 此方法可重写，转换列表有自定义编辑器类型【xtype】=》统一类型，否则数据解析会出错
     * 注明：统一转换：xtype=》下拉框【comboColumn】，枚举下拉【comboboxcolumn】，下拉列表【combolistcolumn】，选择框【checkboxcolumn】
     */
    changeCustomEditorXType: function (xtype) {
        return xtype;
    },
    /**
    * 递归遍历所有表头数据
    * @param {columns} 列表数据
    * （解决复杂表头导出问题） 
    */
    _traversalColumns: function (columns) {
        var me = this;
        //递归遍历所有的
        columns.forEach(function (value) {
            if (value.columns && value.columns.length > 0) {
                me._traversalColumns(value.columns);
            }
            else {
                var fieldName = {};
                fieldName.key = value.dataIndex;
                fieldName.header = value.header;
                if (!value.xtype && !value.editor)
                    value.xtype = 'textfield';
                var xtype = value.xtype ? value.xtype : value.editor.xtype;
                fieldName.xtype = xtype;
                //判断并获取所有的xtype是chckbox类型的编辑器
                if (xtype.toLowerCase().indexOf("checkbox") != -1) {
                    fieldName.xtype = "checkboxcolumn";
                }
                //判断并获取所有的xtype是comboboxcolumn类型的自定义枚举编辑器
                if (xtype.toLowerCase().indexOf("comboboxcolumn") != -1) {
                    fieldName.xtype = "comboboxcolumn";
                }
                //转换自定义编辑器xtype值
                //转换自定义编辑器xtype值
                xtype = fieldName.xtype;
                fieldName.xtype = me.changeCustomEditorXType(xtype);
                switch (fieldName.xtype) {
                    case 'comboColumn'://下拉框
                    case 'comboboxcolumn'://枚举下拉框
                        {
                            fieldName.value = value.editor.store.data;
                        }
                        break;
                    case 'combolistcolumn'://下拉列表
                        {
                            fieldName.value = value.dataIndex + '_Display';
                        }
                        break;
                    default:
                        {
                            if (value.formatter == null) {
                                fieldName.value = null;
                                break;
                            }
                            //时间格式
                            // fieldName.value = value.formatter.replace('date(\"', '').replace('\")', '');
                        }
                        break;
                }
                me.fieldNames.push(fieldName);
            }
        });
        //循环结束--end
    },

    dateConversion: function (value) {
        var d = new Date(value);
        var date = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
        return date;
    }

});