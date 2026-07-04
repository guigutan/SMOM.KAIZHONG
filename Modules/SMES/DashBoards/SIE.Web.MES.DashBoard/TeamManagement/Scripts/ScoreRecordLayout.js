Ext.define('SIE.Web.MES.DashBoard.TeamManagement.ScoreRecordLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'ScoreRecordLayout',
    view: null,
    control: null,
    gridId: 'scoreRecord-id',
    chartId: 'scoreRecord-chart',
    _layoutChildren: function (regions) {
        var me = this;
        var main = regions.main;
        var mainControl = main.getControl();
        me.control = mainControl;
        me.view = main.getView();
        me.view.mon(me.control, 'closewin', function (evtArgs) {
            me._viewClose(evtArgs.tab, evtArgs.control);
        });
        var toolbar = mainControl.getDockedItems()[0];
        var girdControl = me.initGirdControl();
        var chartControl = me.initChartControl();
        me.view.GridControl = girdControl;
        me.view.ChartControl = chartControl;
        me.view.owner = me;
        return Ext.widget('container', {
            height: '100%',
            layout: {
                type: 'vbox',
                pack: 'start',
                align: 'stretch'
            },
            defaults: {
                frame: true,//底色变化
                bodyPadding: 0
            },
            requires: ['Ext.layout.container.VBox'],
            xtype: 'layout-vertical-box',
            items: [{
                baseCls: 'my-panel-no-border',
                items: toolbar  //命令栏
            },
                girdControl, chartControl
            ]
        });
    },
    //初始化表格控件
    initGirdControl: function () {
        var me = this;
        var defcolumn = me.defaultColumns('班组员工'.t(), '班组'.t());
        var x = window.innerHeight - 148;
        return {
            region: 'center',
            id: me.gridId,
            xtype: 'grid',
            width: '100%',
            height: x / 2,
            frame: true,
            header: false,
            columnLines: true,
            style: 'border-width:0',
            iconCls: 'my-panel-no-border icon-grid',
            selModel: {
                selType: 'cellmodel'
            },
            features: [{
                id: 'group',
                ftype: 'groupingsummary',
                groupHeaderTpl: '{name}',
                hideGroupedHeader: true,
                enableGroupingMenu: false
            }],
            columns: defcolumn,
            listeners: {
                //单元格点击事件更改折线图数据
                cellclick: function (g, row, col, record, tr, rowindex) {
                    var cont = Ext.getCmp(me.chartId);
                    var store = cont.getStore();
                    var series = [];
                    series.push(me.addnewSeries('emp' + record.data.empId, record.data.empName));
                    me.loadChartData(store, series, record.data.min - 5, record.data.max + 10);
                }
            },
            //查询调用，动态添加列
            addColumns: function (criter) {
                var cont = Ext.getCmp(this.id);
                var occurDate = criter.OccurDate;
                if (criter.DateType == null) { Ext.Msg.alert('提示'.t(), "请选择日期类型".t()); return; };
                var cl = cont.initColumns(occurDate.BeginValue, occurDate.EndValue, criter.DateType);
                cont.setColumns(cl);
            },
            //查询调用，动态更改store
            initStore: function (queryData) {
                var cont = Ext.getCmp(this.id);
                var taksStore = Ext.create('Ext.data.Store', {
                    data: queryData,
                    sorters: { property: 'workgroupId', direction: 'ASC' },
                    groupField: 'workgroupName'
                });
                cont.setStore(taksStore)
            },
            //初始化列
            initColumns: function (start, end, dateType) {
                var cusCmn = [];
                var detailCmn = [];
                cusCmn.push({
                    text: '班组员工'.t(),
                    width: 100,
                    locked: true,
                    sortable: true,
                    dataIndex: 'empName',
                    hideable: false,
                    summaryType: 'count',
                    summaryRenderer: function (value, summaryData, dataIndex) {
                        return ((value === 0 || value > 1) ? '(' + value + ' 人'.t() + ')' : '(1' + ' 人'.t() + ')');
                    }
                });
                cusCmn.push({
                    header: '班组'.t(),
                    width: 100,
                    locked: true,
                    align: 'center',
                    sortable: true,
                    dataIndex: 'workgroupName'
                });
                var beginDate = new Date(start);
                var endD = new Date(end);
                var endDate = new Date(Date.parse(endD.getFullYear() + "-" + (endD.getMonth() + 1) + "-" + endD.getDate()));
                if (dateType == 2) {
                    while (beginDate.getTime() <= endDate.getTime()) {
                        var month = beginDate.getMonth() + 1;
                        var day = beginDate.getDate();
                        var year = beginDate.getFullYear();
                        var curDate = new Date(beginDate);
                        var datefmt = Ext.String.format('{0}{1}{2}', year, ('0' + month).slice(-2), ('0' + day).slice(-2));
                        var header1 = day + "号".t();
                        detailCmn.push({
                            header: header1.t(),
                            width: 130,
                            align: 'center',
                            sortable: true,
                            dataIndex: 'Date' + datefmt,
                            summaryType: 'sum',
                            renderer: function (value, metaData, record, rowIdx, colIdx, store, view) {
                                return value;
                            },
                            summaryRenderer: function (value, summaryData, dataIndex) {
                                if (value == "undefined" || value == null)
                                    return 0;
                                else return value;
                            },
                            field: {
                                xtype: 'numberfield'
                            }
                        });
                        beginDate.setDate(day + 1);
                        if (beginDate.getDate() == 1 || curDate.getTime() == endDate.getTime()) {

                            detailCmn.push({
                                header: '合计'.t(),
                                width: 130,
                                sortable: true,
                                dataIndex: 'Total' + year + '' + month,
                                align: 'center',
                                summaryType: 'sum',
                                renderer: function (value, metaData, record, rowIdx, colIdx, store, view) {
                                    return value;
                                },
                                summaryRenderer: function (value, summaryData, dataIndex) {
                                    if (value == "undefined" || value == null)
                                        return 0;
                                    else return value;
                                },
                                field: {
                                    xtype: 'numberfield'
                                }
                            })
                            var header2 = month + '月'.t();
                            cusCmn.push({
                                header: header2.t(),
                                sortable: true,
                                columns: detailCmn
                            });
                            detailCmn = [];
                        }
                    };
                }
                else if (dateType == 1) {
                    var curMonth = ("0" + (beginDate.getMonth() + 1)).slice(-2);  //'01'-'12'
                    var curYear = beginDate.getFullYear();
                    var startMonth = curYear + "" + curMonth;

                    var eM = ("0" + (endDate.getMonth() + 1)).slice(-2);
                    var endYear = endDate.getFullYear();
                    var endMonth = endYear + "" + eM;

                    var intStartMonth = Number(startMonth);  //202001-202012
                    var intEndMonth = Number(endMonth);
                    var titilMonth = Number(curMonth);
                    var year = curYear;
                    while (intStartMonth <= intEndMonth) {
                        cusCmn.push({
                            header: Ext.String.format('{0}年{1}月', year, titilMonth).t(),
                            width: 130,
                            align: 'center',
                            sortable: true,
                            dataIndex: 'Date' + intStartMonth,
                            summaryType: 'sum',
                            renderer: function (value, metaData, record, rowIdx, colIdx, store, view) {
                                return value;
                            },
                            summaryRenderer: function (value, summaryData, dataIndex) {
                                if (value >= 0)
                                    return value;
                                else return null;
                            },
                            field: {
                                xtype: 'numberfield'
                            }
                        });
                        if (titilMonth == 12) {
                            titilMonth = 1;
                            year += 1;
                            intStartMonth = Number(year + '01');
                        }
                        else {
                            intStartMonth++;
                            titilMonth++;
                        }
                    }
                }
                return cusCmn;
            }
        }
    },
    //默认列，打开界面的时候显示
    defaultColumns: function (col1Text, col2Text) {
        var cusCmn = [];
        cusCmn.push({
            text: col1Text,
            width: 100,
            style: 'border-width:0;',
            locked: true,
            hideable: false,
        });
        if (col2Text != "")
            cusCmn.push({
                header: col2Text,
                width: 100,
                align: 'center',
                locked: true,
                sortable: true,
            });
        for (var i = 1; i < 13; i++) {
            var header = i + '月'.t();
            cusCmn.push({
                header: header.t(),
                width: 130,
                align: 'center',
                sortable: true,
                dataIndex: ''
            });
        }
        return cusCmn;
    },
    //初始化图形控件
    initChartControl: function () {
        return {
            xtype: 'container',
            id: this.chartId + 'Content',
            chartId: this.chartId,
            parent: this,
            width: '100%',
            html: "<div id='chartDiv' style='width:100%;height:100%;'><div>",
            listeners: {
                afterRender: function (comp) {
                    var chartStore = Ext.create('Ext.data.Store', {
                        data: null
                    });
                    this.parent.loadChartData(chartStore, [], 0, 100);
                }
            },

        }
    },
    //加载图形
    loadChartData: function (store, seriers, min, max) {
        var x = window.innerHeight - 148;
        if (!Ext.Object.isEmpty(Ext.getCmp(this.chartId))) {
            Ext.destroy(Ext.getCmp(this.chartId));
        }
        Ext.create('Ext.chart.Chart', {
            renderTo: "chartDiv",
            controller: 'DashBoardController',
            id: this.chartId,
            bodyStyle: 'border-width:0px;',
            width: '100%',
            height: x / 2,
            interactions: {
                type: 'panzoom',
                zoomOnPanGesture: true
            },
            animation: {
                duration: 200
            },
            store: store,
            innerPadding: {
                left: 40,
                right: 40
            },
            captions: {
                title: '绩效评分趋势图'.t()
            },
            legend: {
                type: 'sprite',
                docked: 'bottom'
            },
            axes: [{
                type: 'numeric',
                position: 'left',
                grid: true,
                minimum: min,
                maximum: max,
                title: '分数'.t(),
            }, {
                type: 'category',
                position: 'bottom',
                grid: true,
                label: {
                    rotate: {
                        degrees: -45
                    }
                }
            }],
            series: seriers,
        })
    },
    initSeries: function (empName, empId, chartData, min, max) {
        var me = this;
        var series = [];
        if (empName != "") {
            var empArr = empName.split(',');
            var empIdArr = empId.split(',');
            for (var i = 0; i < empArr.length; i++) {
                series.push(me.addnewSeries("emp" + empIdArr[i], empArr[i]));
            }
        }
        var cont = Ext.getCmp(me.chartId);
        var chartStore = Ext.create('Ext.data.Store', {
            data: chartData,
        });
        me.loadChartData(chartStore, series, min, max);
    },
    addnewSeries: function (yFieldText, title) {
        return {
            type: 'line',
            title: title,
            xField: 'monthDay',
            yField: yFieldText,
            label: {
                display: 'over',
                field: yFieldText,
                renderer: 'lineLabelRender'
            },
            marker: {
                animation: {
                    duration: 200,
                    easing: 'backOut'
                }
            },
            highlightCfg: {
                scaling: 2
            },
            tooltip: {
                trackMouse: true,
                renderer: 'onSeriesTooltipRender'
            }
        };
    },
});









