Ext.define('SIE.Web.EMS.Report.WorkOrderExcuteReports.Scripts.WorkOrderExcuteReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'WorkOrderExcuteReportLayout',
    _isRunning: false,
    _token: null,
    _criteria: null,

    /**
     * @param regions 聚合块
     * 初始化界面布局
     * @returns 布局配置
     */
    _layoutChildren: function (regions) {
        var me = this;
        regions.main._view._relations[0]._target.mainLayout = me;
        var toolbar = null;
        var dockItems = regions.main._control.getDockedItems();
        dockItems.forEach(function (dockItem) {
            if (dockItem.xtype === 'toolbar')
                toolbar = dockItem;
        });
        var lineChartControl = me.createLineChart();
        var listView = me.createListView();

        this._listView = listView;
        return Ext.widget('container', {
            layout: 'border',
            bodyBorder: false,
            items: [{
                region: 'north',
                items: toolbar,
                border: false,
            }, {
                region: 'center',
                layout: 'hbox',
                xtype: 'panel',
                border: false,
                items: [{
                    title: '当期工单执行情况汇总'.t(),
                    layout: {
                        type: 'table',
                        columns: 2,
                        tableAttrs: {
                            style: {
                                width: '100%',
                                height: '85%'
                            }
                        }
                    },
                    defaults: {
                        width: '100%',
                        margin: '3%',
                        border: 1,
                        labelAlign: 'center',

                    },
                    width: "35%",
                    height: "100%",
                    items: [
                        {
                            xtype: 'displayfield',
                            id: 'WorkCountTextId',
                            name: 'WorkCountTextId',
                            value: '工单总数'.t(),
                            fieldLabel: '',
                            fieldStyle: 'font-size:30px;vertical-align:middle;text-align:center;',
                        }, {
                            xtype: 'displayfield',
                            id: 'CompleteCountTextId',
                            name: 'CompleteCountTextId',
                            value: '已完成数'.t(),
                            fieldLabel: '',
                            fieldStyle: 'font-size:30px;vertical-align:middle;text-align:center;',

                        },
                        {
                            xtype: 'displayfield',
                            id: 'WorkCountId',
                            name: 'WorkCountId',
                            heght:"10%",
                            fieldLabel: '',
                            value: '0',
                            fieldStyle: 'font-size:60px;color:#D3D3D3;vertical-align:middle;text-align:center;',
                        }, {
                            xtype: 'displayfield',
                            id: 'CompleteCountId',
                            name: 'CompleteCountId',
                            fieldLabel: '',
                            value: '0',
                            fieldStyle: 'font-size:60px;color:#4169E1;vertical-align:middle;text-align:center;',

                        },
                        {
                            xtype: 'displayfield',
                            id: 'WaitCompleteCountTextId',
                            name: 'WaitCompleteCountTextId',
                            value: '待完成数'.t(),
                            fieldLabel: '',
                            flex: 7,
                            fieldStyle: 'font-size:30px;vertical-align:middle;text-align:center;',
                        }, {
                            xtype: 'displayfield',
                            id: 'CompleteRateTextId',
                            name: 'CompleteRateTextId',
                            value: '工单完成率'.t(),
                            fieldLabel: '',
                            flex: 7,
                            fieldStyle: 'font-size:30px;vertical-align:middle;text-align:center;',
                        }
                        ,
                        {
                            xtype: 'displayfield',
                            id: 'WaitCompleteCountId',
                            name: 'WaitCompleteCountId',
                            value: '0',
                            fieldLabel: '',
                            flex: 7,
                            fieldStyle: 'font-size:60px;color:#FF4500;vertical-align:middle;text-align:center;',
                        }, {
                            xtype: 'displayfield',
                            id: 'CompleteRateId',
                            name: 'CompleteRateId',
                            value: '0%',
                            fieldLabel: '',
                            flex: 7,
                            fieldStyle: 'font-size:60px;color:#32CD32;vertical-align:middle;text-align:center;',

                        }
                        
                    ]


                }, {
                    title: '图表展示'.t(),
                    xtype: "panel",
                    height: "100%",
                    layout: 'fit',
                    width: "65%",
                    items: [lineChartControl]
                }]
            }, {
                region: 'south',
                layout: 'vbox',
                xtype: 'panel',
                id: "WorkOrderExcuteReportLayoutMainPanel",
                _ngListView: listView,
                border: false,
                items: [{
                    title: '列表展示'.t(),
                    xtype: "panel",
                    height: 250,
                    region: "center",
                    layout: 'fit',
                    width: "100%",
                    flex: 1,
                    items: [listView.getControl()]
                }]
            }]
        });
    },

    /**
    * 创建折线图控件
    * @method createLineChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createLineChart: function () {
        return Ext.create('SIE.Web.EMS.Report.WorkOrderExcuteReports.Scripts.WorkOrderExcuteReportLineChart', {
            id: 'WorkOrderExcuteReportLineChartId',
            region: 'north',
            layout: 'fit',
            width: "100%",
            flex: 1,
            border: false,
        });
    },

    /**创建列表明细视图 */
    createListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Report.WorkOrderExcuteReports.WorkOrderExcuteReportViewModel',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            async: false,
            callback: function (res) {
                meta = res;
            }
        });
        meta.gridConfig.manageHeight = false;
        var listView = SIE.AutoUI.createListView(meta);
        return listView;
    },

    /**
     * 查询(iqcreport)
     * @param {any} criteria
     * @param {any} token
     */
    loadReportData: function (criteria, token) {
        var me = this;
        if (Ext.isEmpty(token))
            return;
        this._token = token;
        this._criteria = criteria;
        try {
            if (me._isRunning)
                return;
            me._isRunning = true;
            SIE.invokeDataQuery({
                method: 'GetWorkOrderExcuteReportData',
                params: [criteria],
                action: 'queryer',
                async: true,
                type: 'SIE.Web.EMS.Report.WorkOrderExcuteReports.DataQueryers.WorkOrderExcuteReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var Resultdata = res.Result;
                        if (Resultdata.IsSuccess) {
                            me.bindReportInfos(Resultdata);
                            me._isRunning = false;
                            Ext.getBody().unmask();
                        } else {
                            SIE.Msg.showInstantMessage(Resultdata.err);
                            Ext.getBody().unmask();
                        }
                    }

                },
                error: function () {
                    Ext.getBody().unmask();
                }
            });
        } catch (e) {
        } finally {
            me._isRunning = false;
        }
    },

    /**
     * 绑定数据报表
     * @param {any} passRateInfos
     */
    bindReportInfos: function (passRateInfos) {
        var clounNameList = passRateInfos.TableInfo.ClounNameList;
        this.setViewColumns(clounNameList, passRateInfos.Datas);

        var turnoverRateList = passRateInfos.ChartInfo;
        var lineChart = Ext.getCmp('WorkOrderExcuteReportLineChartId');
        if (lineChart) {
            var chartStore = lineChart.items.items[0].getStore();
            if (Ext.isEmpty(turnoverRateList))
                chartStore.removeAll();
            else
                chartStore.setData(turnoverRateList);
            lineChart.items.items[0].setStore(chartStore);
        }

        ////明细
        this._listView.getData().setData(passRateInfos.TableInfo.Datas);
        var mainPanel = Ext.getCmp("WorkOrderExcuteReportLayoutMainPanel");
        if (mainPanel) {
            mainPanel._dataLoaded = true; //数据已加载
        }

        //工单总数
        var WorkCountId = Ext.getCmp("WorkCountId");
        WorkCountId.setValue(passRateInfos.CounInfo.WorkCount);
        //已完成工单数
        var CompleteCountId = Ext.getCmp("CompleteCountId");
        CompleteCountId.setValue(passRateInfos.CounInfo.CompleteCount);
        //待完成工单数
        var WaitCompleteCountId = Ext.getCmp("WaitCompleteCountId");
        WaitCompleteCountId.setValue(passRateInfos.CounInfo.WaitCompleteCount);
        //工单完成率
        var equipmentConutId = Ext.getCmp("CompleteRateId");
        equipmentConutId.setValue(passRateInfos.CounInfo.CompleteRate + "%");

    },

    setViewColumns: function (clounNameList, values) {
        var view = this._listView;
        var me = this;
        //日期部份删除
        var gridPanel = view.getControl();
        var grid = gridPanel.actionables[0].grid;

        var gridColumns = grid.config.columns;
        gridColumns.splice(0);
        var dataIndex = 0;
        for (const dataColumn of clounNameList) {
            var colName = dataColumn;
            var colWidth = 104;
            var column = {
                dataIndex: dataIndex++,
                text: colName,
                header: colName,
                width: colWidth,
                align: 'center',
                sortable: false,
                renderer: function (value, meta, record, rowIndex, colIndex, store, view) {
                    var dataIndex = gridColumns[colIndex - 1].dataIndex;
                    var value = record[0][dataIndex];
                    return value;
                },
            };
            gridColumns.push(column);

            grid.reconfigure(grid.store, gridColumns);
        }
    }
});