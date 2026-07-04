SIE.defineCommand('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Commands.ExportCommandBase', {
    meta: { text: "导出Excel", group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        myview = view;
        var me = this;
        var dataCount = view.getData().data.items.length;
        if (dataCount == 0) //数据存在时允许导出
            return false;
        //重置导出的数据
        me.fieldNames = [];
        var columns = myview.gridConfig.columns;
        me.traversalColumns(columns);

        //获取表格数据
        var recordData = [];
        var grid = myview.getControl();
        Ext.each(grid.getStore().getRange(), function (record) {
            recordData.push(record.data);
        });

        //获取导出数据
        var exportJsonData = [];
        exportJsonData = me.gridDataProcessing(recordData, exportJsonData);

        //获取Excel表头
        var exportJsonHeaders = [];
        me.fieldNames.forEach(function (value) {
            exportJsonHeaders.push(value.header);
        });
        //表头转化为html   
        var gridHead = me.setExportGridHead(exportJsonHeaders);

        //导出Excel
        JSONToExcelConvertor(exportJsonData, myview.label + Ext.util.Format.date(new Date(), 'Ymdhis'), gridHead);

        /**
         * Json转Excel
         * @param {*} JSONData Json数据
         * @param {*} FileName 导出的文件名称
         * @param {*} worksheet 表头名
         */
        function JSONToExcelConvertor(JSONData, FileName, ShowLabel, worksheetName) {
            //先转化json
            var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
            //组装表格内容
            var tableContent = '';
            //设置表头
            tableContent += ShowLabel;
            //设置数据
            for (var i = 0; i < arrData.length; i++) {
                var row = "<tr>";
                for (var key in arrData[i]) {
                    var value = arrData[i][key] == null ? "" : arrData[i][key];

                    row += '<td style=\'mso-number-format:\"\@\"\'>' + value + '</td>';
                }
                tableContent += row + "</tr>";
            }

            //转换网页内容为data协议
            var dataType = 'application/vnd.ms-excel';
            var uri = 'data:' + dataType + ';base64,',
                template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40">'
                    + '<head><meta charset=\'UTF-8\'><!--[if gte mso 9]>'
                    + '<xml>'
                    + '<x:ExcelWorkbook>'
                    + '<x:ExcelWorksheets><x:ExcelWorksheet>'
                    + '<x:Name>{worksheet}'
                    + '</x:Name>'
                    + '<x:WorksheetOptions>'
                    + '<x:DisplayGridlines/>'
                    + '</x:WorksheetOptions>'
                    + '</x:ExcelWorksheet>'
                    + '</x:ExcelWorksheets>'
                    + '</x:ExcelWorkbook>'
                    + '</xml><![endif]-->'
                    + '</head>'
                    + '<body><table>{table}</table></body></html>',
                base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) },
                format = function (s, c) {
                    return s.replace(/{(\w+)}/g,
                        function (m, p) {
                            return c[p];
                        })
                }
            //模板的占位替换
            var ctx = {
                worksheet: worksheetName || 'Worksheet', table: tableContent
            };
            FileName = FileName ? FileName + '.xls' : 'excel_data.xls';
            //兼容IE,Edge
            if (navigator.msSaveOrOpenBlob) {
                uri = 'data:' + dataType + ';charset=utf-8, ';
                var data = uri + format(template, ctx);
                var blob = new Blob(['\ufeff', data], {
                    type: dataType
                });
                navigator.msSaveOrOpenBlob(blob, FileName);
            } else {
                var data = uri + base64(format(template, ctx));
                var link = document.createElement("a");
                link.style = "visibility:hidden";
                link.href = data;
                link.download = FileName;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        }
    },

    /**
      * 此方法可重写，处理导出的数据
      * 列表导出数据处理
      * @param {*} record 列表数据集
      * @param {*} exportJsonData 返回的数据字符串
      */
    gridDataProcessing: function (record, exportJsonData) {
        var me = this;
        record.forEach(function (row) {
            var fieldData = '';
            me.fieldNames.forEach(function (fieldName) {
                var exportValue = row[fieldName.key];
                //处理列表单元格数据
                exportValue = me._proGridCellValue(row, fieldName, exportValue);
                fieldData += '\"' + fieldName.key + '\":\"' + (exportValue ? exportValue : '') + '\",';
            });
            //解决导出文本换行报错问题：换行符要转义
            var fieldDataStr = '{' + fieldData.substr(0, fieldData.length - 1) + '}';
            exportJsonData.push(JSON.parse(fieldDataStr.replace(/\n/g, "\\n").replace(/\r/g, "\\r")));
        });
        return exportJsonData;
    },
    /**
       * 列表单元格数据处理
       * @param {*} fieldName 
       * @param {*} exportValue 处理后的数据
       */
    _proGridCellValue: function (row, fieldName, exportValue) {
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
            default:
                {
                    if (fieldName.value)
                        exportValue = Ext.util.Format.date(exportValue, fieldName.value);
                }
                break;
        }
        exportValue = exportValue ? exportValue.toString().replace(/\"/g, "'") : exportValue;//替换字符串中存在的双引号
        return exportValue;
    },

    /**
    * 多级表头导出：目前只支持二级表头的生成
    * @param {headData} 表头数据
    * @returns {Html}   表头html<tr><td></td><tr>
    * 可重写此方法，自定义表头，或者用html拼写表头（解决复杂表头导出问题） 
    */
    setExportGridHead: function (columns) {
        //设置表头
        //设置表头
        var row = "<tr>";
        for (var i = 0, l = columns.length; i < l; i++)
            row += "<td bgcolor='#00868B'><font size='3' color='white'><b>" + columns[i] + '</b></font></td>';
        //换行
        return row + "</tr>";
    },
    /**
     * 客户端导出前：处理表格数据
     * @param {xtype} 编辑器类型
     * 此方法可重写，转换列表有自定义编辑器类型【xtype】=》统一类型，否则数据解析会出错
     * 注明：统一转换：xtype=》下拉框【comboColumn】，枚举下拉【comboboxcolumn】，下拉列表【combolistcolumn】，选择框【checkboxcolumn】
     */
    changeCustomEditorXType: function (xtype) {
        /**示例
        *switch (xtype) {
        *          case 'customecomboboxcolumn1'://自定义枚举类型1
        *          case 'customecomboboxcolumn2':自定义枚举类型2
        *          case 'comboboxcolumn':
        *             {
        *                xtype="comboboxcolumn";
        *             }
        *            break;
        *      }
        */
        return xtype;
    },
    /**
    * 递归遍历所有表头数据
    * @param {columns} 列表数据
    * 可重写此方法，自定义表头，或者用html拼写表头（解决复杂表头导出问题） 
    */
    traversalColumns: function (columns) {
        var me = this;
        //递归遍历所有的
        columns.forEach(function (value) {
            if (value.columns && value.columns.length > 0) {
                me.traversalColumns(value.columns);
            }
            else {
                var fieldName = {};
                fieldName.key = value.dataIndex;
                fieldName.header = value.header ? value.header : value.text;//动态列问题
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
                xtype = fieldName.xtype;
                fieldName.xtype = me.changeCustomEditorXType(xtype);
                switch (fieldName.xtype) {
                    case 'comboColumn':
                    case 'comboboxcolumn':
                        {
                            fieldName.value = value.editor.store.data;
                        }
                        break;
                    case 'combolistcolumn':
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
                            fieldName.value = value.formatter.replace('date(\"', '').replace('\")', '');
                        }
                        break;
                }

                me.fieldNames.push(fieldName);
            }
        });
        //循环结束--end
    }
});