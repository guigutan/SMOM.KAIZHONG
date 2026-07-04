SIE.defineCommand('SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Commands.ExportCommand', {
    extend: 'SIE.Web.EMS.Report.EquipmentIntegrateStatistics.Commands.ExportCommandBase',
    meta: { text: "导出", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    /**
     * @override 是否可执行
     * @param {any} view
     */
    canExecute: function (view) {
        var mainPanel = Ext.getCmp('esdReportMainPanel');
        if (mainPanel._dataLoaded)
            return true;
        return false;
    },

    /**
     * @override 执行
     * @param {any} view
     */
    execute: function (view) {
        var me = this;
        var chart = this.getExportInfo(view);
        var indata = {};
        indata.Data = Ext.encode(chart);
        //调用后台计算
        view.execute({
            data: indata,
            success: function (res) {
                if (res.Success) {
                    var result = res.Result;
                    if (result) {
                        var fileName = result.FileName;
                        var dataURI = result.FileContent;

                        //获取blob文件数据
                        var blob = base64ToBlob(dataURI);
                        if (window.navigator.msSaveOrOpenBlob) {
                            navigator.msSaveBlob(blob, fileName);
                        } else {
                            var link = document.createElement('a');
                            var body = document.querySelector('body');
                            link.href = window.URL.createObjectURL(blob);
                            link.download = fileName;
                            //兼容火狐
                            link.style.display = 'none';
                            body.appendChild(link);
                            link.click();
                            body.removeChild(link);
                            window.URL.revokeObjectURL(link.href);
                        }
                    }
                }
            },
            error: function (res) {
                Ext.Msg.alert('提示'.t(), res.Message);
            }
        });
    },
    /**
     * 获取导出的内容
     * @param {any} view
     */
    getExportInfo: function (view) {
        var me = this;
        var esdMainPanel = Ext.getCmp("esdReportMainPanel");
        var rateChart = Ext.getCmp("edoRateChartId");
        var ngListView = esdMainPanel._ngListView;
        var gridData = me.getGridData(ngListView);//不通过数据列表
        var pictures = [];
        rateChart.items.items.forEach(
            function (p) {
                pictures.push(p.getImage());
            });  //图表截图
        var exportInfo = {
            Images: pictures,
            GridData: gridData
        };
        return exportInfo;
    },
    /**
     * 获取抽样列表数据
     * @param {any} view
     */
    getGridData: function (view) {
        var myview = view;
        var me = this;
        var dataCount = view.getData().data.items.length;
        if (dataCount == 0) //数据存在时允许导出
            return false;
        //重置导出的数据
        me.fieldNames = [];
        var columns = myview.gridConfig.columns;
        me.traversalColumns(columns);

        columns.forEach(function (value) {
            if (value.hidden) {
                me.fieldNames.remove(me.fieldNames.first(function (p) { return p.key === value.dataIndex }));
            }
        });

        //获取Excel表头
        var exportJsonHeaders = [];
        me.fieldNames.forEach(function (value) {
            exportJsonHeaders.push(value.header)
        });

        //获取表格数据
        var recordData = [];
        var grid = myview.getControl();
        Ext.each(grid.getStore().getRange(), function (record) {
            recordData.push(record.data);
        });

        //获取导出数据
        var exportJsonData = [];
        exportJsonData = me.gridDataProcessing(recordData, exportJsonData);


        return {
            Datas: exportJsonData,
            Headers: exportJsonHeaders
        };
    },

    /**
    * 列表单元格数据处理，检测时间格式化
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
        if (fieldName.key == "InspTime") //检测时间格式化
            exportValue = exportValue ? Ext.Date.toString(exportValue) : exportValue;
        else
            exportValue = exportValue ? exportValue.toString().replace(/\"/g, "'") : exportValue;//替换字符串中存在的双引号
        return exportValue;
    },

});