Ext.define('SIE.Web.MES.DashBoard.Common.ReportBaseController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.reportBaseController',
    requires: [
        'Ext.exporter.text.CSV',
        'Ext.exporter.text.TSV',
        'Ext.exporter.text.Html',
        'Ext.exporter.excel.Xml',
        'Ext.exporter.excel.Xlsx',
        'Ext.exporter.excel.PivotXlsx'
    ],
    events: ['beforedocumentsave', 'documentsave', 'dataready'],

    /**
     * 切换Dock事件
     * @method changeDock
     * @param {Ext.button.Button} button 按钮控件
     * @param {bool} checked 是否选中
     */
    changeDock: function (button, checked) {
        if (checked) {
            this.getView().getPlugin('configurator').setDock(button.text.toLowerCase());
        }
    },

    /**
     * 获取菜单
     * @method getCustomMenus
     * @param {Ext.panel.Panel} panel 面板
     * @param {options} options 选择
     */
    getCustomMenus: function (panel, options) {

        //此处未发现有其他的处理逻辑，仅仅只是多了弹窗，出现的英文和对应的数据也无法正确翻译成中文，影响界面语言的一致，故先注释

        //options.menu.add({
        //    text: 'Custom menu item',
        //    handler: function () {
        //        Ext.Msg.alert('Custom menu item', Ext.String.format('Do something for "{0}"', options.field.getHeader()));
        //    }
        //});
    },

    /**
     * 获取年标签事件
     * @method yearLabelRenderer
     * @param {object} value y轴值
     * @return {string} 年标签
     */
    yearLabelRenderer: function (value) {
        return value + '年 '.t();
    },

    /**
    * 获取月标签事件
    * @method monthLabelRenderer
    * @param {object} value y轴值
    * @return {string} 月标签
    */
    monthLabelRenderer: function (value) {
        //中文
        if("月".t() == "月")
            return value + '月 '.t();
        //英文
        if ("月".t() == "month") {
            switch (value) {
                case 1:
                    return "January";
                    break;
                case 2:
                    return "February";
                    break;
                case 3:
                    return "March";
                    break;
                case 4:
                    return "April";
                    break;
                case 5:
                    return "May";
                    break;
                case 6:
                    return "June";
                    break;
                case 7:
                    return "July";
                    break;
                case 8:
                    return "August";
                    break;
                case 9:
                    return "September";
                    break;
                case 10:
                    return "October";
                    break;
                case 11:
                    return "November";
                    break;
                case 12:
                    return "December";
                    break;
            }
        }
    },

    /**
     * 获取日标签事件
     * @method dayLabelRenderer
     * @param {object} value y轴值
     * @return {string} 日标签
     */
    dayLabelRenderer: function (value) {
        //中文
        if ("月".t() == "月")
            return value + '号'.t();
        //英文
        if ("月".t() == "month") {
            switch (value) {
                case 1:
                case 21:
                case 31:
                    return value + 'st';
                case 2:
                case 22:
                    return value + 'nd';
                case 3:
                case 23:
                    return value + 'rd';
                default:
                    return value + 'th';
            }
        }
    },

    /**
     * 获取周标签事件
     * @method weekLabelRenderer
     * @param {object} value y轴值
     * @return {string} 周标签
     */
    weekLabelRenderer: function (value) {
        return Ext.String.format('第{0}周'.t(), value);
    },

    /**
     * 导出Xlsx
     * @method exportToPivotXlsx
     */
    exportToPivotXlsx: function () {
        this.doExport({
            type: 'pivotxlsx',
            matrix: this.getView().getMatrix(),
            title: 'Pivot grid export demo',
            fileName: 'ExportPivot.xlsx'
        });
    },

    /**
     * 导出
     * @method exportTo
     * @param {object} btn btn
     */
    exportTo: function (btn) {
        var cfg = Ext.merge({
            title: 'Pivot grid export demo',
            fileName: 'PivotGridExport' + (btn.cfg.onlyExpandedNodes ? 'Visible' : '') + '.' + (btn.cfg.ext || btn.cfg.type)
        }, btn.cfg);

        this.doExport(cfg)
    },

    /**
     * 导出
     * @method exportTo
     * @param {object} config config
     */
    doExport: function (config) {
        this.getView().saveDocumentAs(config).then(null, this.onError);
    },

    /**
     * 错误
     * @method onError
     * @param {object} error error
     */
    onError: function (error) {
        Ext.Msg.alert('Error', typeof error === 'string' ? error : 'Unknown error');
    },

    /**
     * 文件保存之前
     * @method onBeforeDocumentSave
     * @param {object} view view
     */
    onBeforeDocumentSave: function (view) {
        view.mask('Document is prepared for export. Please wait ...');
    },

    /**
     * 文件保存
     * @method onDocumentSave
     * @param {object} view view
     */
    onDocumentSave: function (view) {
        view.unmask();
    },

    /**
     * 获取折线图x标签
     * @method onLineChartAxisLabel
     * @param {axis} axis 轴对象
     * @param {label} label 标签
     * @param {layoutContext} layoutContext 内容
     * @return {string} x标签
     */
    onLineChartAxisLabel: function (axis, label, layoutContext) {
        return layoutContext.renderer(label) + '%';
    },

    /**
     * 获取折线图序列提示语
     * @method onLineChartSeriesTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 折线图序列提示语
     */
    onLineChartSeriesTooltip: function (tooltip, record, item) {
        tooltip.setHtml(record.get('XDate') + ': ' + record.get('YData') + '%');
    },

    /**
     * 获取折线图目标提示语
     * @method onLineChartDesiredTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 折线图目标提示语
     */
    onLineChartDesiredTooltip: function (tooltip, record, item) {
        tooltip.setHtml('目标值'.t() + ': ' + record.get('YDesired') + '%');
    },

    /**
     * 获取折线图警示提示语
     * @method onLineChartAlarmTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 折线图警示提示语
     */
    onLineChartAlarmTooltip: function (tooltip, record, item) {
        tooltip.setHtml('预警值'.t() + ': ' + record.get('YAlarm') + '%');
    },

    /**
     * 折线图高亮事件
     * @method onLineChartItemHighlight
     * @param {chart} chart 图形控件
     * @param {newHighlightItem} newHighlightItem 新高亮对象
     * @param {oldHighlightItem} oldHighlightItem 旧高亮对象
     */
    onLineChartItemHighlight: function (chart, newHighlightItem, oldHighlightItem) {
        this.setLineChartSeriesLineWidth(newHighlightItem, 4);
        this.setLineChartSeriesLineWidth(oldHighlightItem, 2);
    },

    /**
     * 设置折线图对象高亮
     * @method setLineChartSeriesLineWidth
     * @param {item} item 高亮对象
     * @param {lineWidth} lineWidth 线宽
     */
    setLineChartSeriesLineWidth: function (item, lineWidth) {
        if (item) {
            item.series.setStyle({
                lineWidth: lineWidth
            });
        }
    },

    /**
     * 获取工序直通率x标签
     * @method onProcessDirectRateAxisLabel
     * @param {axis} axis 轴对象
     * @param {label} label 标签
     * @param {layoutContext} layoutContext 内容
     * @return {string} x标签
     */
    onProcessDirectRateAxisLabel: function (axis, label, layoutContext) {
        return layoutContext.renderer(label) + '%';
    },

    /**
     * 获取工序直通率序列标签
     * @method onProcessDirectRateSeriesLabel
     * @param {v} v 值
     * @return {string} 序列标签
     */
    onProcessDirectRateSeriesLabel: function (v) {
        return v + '%';
    },

    /**
     * 获取工序直通率对象提示语
     * @method onProcessDirectRateItemTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {item} item 对象
     * @param {target} target 目标对象
     * @param {e} e 事件对象
     * @return {string} 工序直通率对象提示语
     */
    onProcessDirectRateItemTooltip: function (tooltip, item, target, e) {
        record = item.record;
        tooltip.setHtml(record.get('ProcessName') + ': ' +
            record.get(item.field) + '%');
    },

    /**
     * 获取工序直通率序列提示语
     * @method onProcessDirectRateSeriesTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 工序直通率序列提示语
     */
    onProcessDirectRateSeriesTooltip: function (tooltip, record, item) {

        tooltip.setHtml(record.get('ProcessName') + ': ' +
            record.get(item.field) + '%');
    },

    /**
    * 获取列标签
    * @method onColumnRender
    * @param {v} v 值
    * @return {string} 列标签
    */
    onColumnRender: function (v) {
        return Ext.util.Format.usMoney(v * 1000);
    },

    yearTotal: {},

    /**
     * 获取累计总数
     * @method getYearTotal
     * @param {record} record 记录数据
     * @return {int} 累计总数
     */
    getYearTotal: function (record) {
        var map = this.yearTotal,
            year = record.get('ProcessName'),
            total = map[year];

        if (!total) {
            map[year] = total =
                record.get('PassQty') +
                record.get('FailedQty');
        }

        return total;
    },

    /**
     * 获取工序一次良品/不良品的提示语
     * @method onProcessStatisticsBarTip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 工序一次良品/不良品的提示语
     */
    onProcessStatisticsBarTip: function (tooltip, record, item) {
        var fieldIndex = Ext.Array.indexOf(item.series.getYField(), item.field),
            manufacturer = item.series.getTitle()[fieldIndex],
            percent = record.get(item.field) / this.getYearTotal(record) * 100;

        tooltip.setHtml(manufacturer + '  ' + record.get('ProcessName') + ': ' +
            percent.toFixed(1) + '%');
    },

    /**
     * 获取工序一次良品/不良品的x标签
     * @method onProcessStatisticsAxisLabel
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     * @return {string} 工序一次良品/不良品的x标签
     */
    onProcessStatisticsAxisLabel: function (axis, label, layoutContext) {
        return layoutContext.renderer(label) + '%';
    },

    /**
    * 获取工序一次良品/不良品的序列标签
    * @method onProcessStatisticsSeriesLabel
    * @param {v} v 值
    * @return {string} 序列标签
    */
    onProcessStatisticsSeriesLabel: function (v) {
        return v;
    },
    onProcessStatisticsShopBarTip: function (tooltip, record, item) {
        var fieldIndex = Ext.Array.indexOf(item.series.getYField(), item.field),
            manufacturer = item.series.getTitle()[fieldIndex];
        var str = record.data.ProcessName;
        if (fieldIndex == 0) {
            str += " " + manufacturer + "：" + record.data.PassQty;
        }
        else {
            str += " " + manufacturer + "：" + record.data.FailedQty;
        }
        tooltip.setHtml(str);
    },

    /**
     * 目标/预警值变更事件
     * @method onDesiredAlarmChange
     * @param {object} lineChartSettingInfo
     */
    onDesiredAlarmChange: function (lineChartSettingInfo) {
        var chart = this.lookup('chart');
        if (lineChartSettingInfo !== null) {
            var alarmLine = {
                value: lineChartSettingInfo.Alarm,
                line: {
                    strokeStyle: 'red',
                    lineWidth: 2,
                    lineDash: [6, 3],
                    title: {
                        text: '预警值'.t() + ':' + lineChartSettingInfo.Alarm + ' %',
                        fontWeight: 'bold',
                        fillStyle: 'red',
                        fontSize: 14
                    }
                }
            };

            var desiredLine = {
                value: lineChartSettingInfo.Desired,
                line: {
                    strokeStyle: 'green',
                    lineWidth: 2,
                    lineDash: [6, 3],
                    title: {
                        text: '目标值'.t() + ':' + lineChartSettingInfo.Desired + ' %',
                        fontWeight: 'bold',
                        fillStyle: 'green',
                        fontSize: 14
                    }
                }
            };

            chart.getAxes()[0].setLimits([
                alarmLine,
                desiredLine
            ]);
        }
        else {
            var alarmLine = {
                value: 0,
                line: {
                    strokeStyle: 'red',
                    lineWidth: 2,
                    lineDash: [6, 3],
                    title: {
                        text: '',
                        fontWeight: 'bold',
                        fillStyle: 'red',
                        fontSize: 14
                    }
                }
            };

            var desiredLine = {
                value: 0,
                line: {
                    strokeStyle: 'green',
                    lineWidth: 2,
                    lineDash: [6, 3],
                    title: {
                        text: '',
                        fontWeight: 'bold',
                        fillStyle: 'green',
                        fontSize: 14
                    }
                }
            };

            chart.getAxes()[0].setLimits([
                alarmLine,
                desiredLine
            ]);
        }

        chart.redraw();
    },

    /**
     * 直通率标题变更事件
     * @method onLineChartTitleChange
     * @param {string} lineName 车间
     * @param {string} shift 班制
     */
    onLineChartTitleChange: function (lineName, shift, minValue, maxValue) {
        var chart = this.lookup('chart');
        var leftAxis = chart.getAxes()[0];
        leftAxis.setMaximum(maxValue);
        leftAxis.setMinimum(minValue);
        leftAxis.setVisibleRange([minValue, maxValue]);
        if (shift)
            chart.setCaptions({ title: shift + '直通率趋势图'.t() });
        else if (lineName)
            chart.setCaptions({ title: lineName + '直通率趋势图'.t() });
        else
            chart.setCaptions({ title: '直通率趋势图'.t() });

        chart.redraw();
    },

    /**
     * 工序直通率标题变更事件
     * @method onProcessDirectRateTitle
     * @param {string} title 标题
     */
    onProcessDirectRateTitle: function (title) {
        var chart = this.lookup('chart');
        chart.setCaptions({ title: title });
    },

    /**
     * 获取缺陷报表x轴标签
     * @method onDefectChartAxisLabel
     * @param {axis} axis 轴对象
     * @param {string} label 标题
     * @param {string} layoutContext 内容
     * @return {string} x标签
     */
    onDefectChartAxisLabel: function (axis, label, layoutContext) {
        var total = axis.getRange()[1];
        return (label / total * 100).toFixed(0) + '%';
    },

    /**
     * 设置缺陷图Bar序列提示语
     * @method onDefectChartBarSeriesTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     */
    onDefectChartBarSeriesTooltip: function (tooltip, record, item) {
        tooltip.setHtml(record.get('DefectName') + ': ' + record.get('Qty'));
    },

    /**
     * 设置缺陷图Line序列提示语
     * @method onDefectChartLineSeriesTooltip
     * @param {tooltip} tooltip 提示对象
     * @param {record} record 数据
     * @param {item} item 对象
     */
    onDefectChartLineSeriesTooltip: function (tooltip, record, item) {
        var store = record.store,
            i, complaints = [];

        for (i = 0; i <= item.index; i++) {
            complaints.push(store.getAt(i).get('DefectName'));
        }
        tooltip.setHtml('<div style="text-align: center; font-weight: bold">' +
            record.get('CumPercent') + '%</div>' + complaints.join('<br>'));
    },
    onPercentRender: function (v) {
        return v + '%';
    }
});