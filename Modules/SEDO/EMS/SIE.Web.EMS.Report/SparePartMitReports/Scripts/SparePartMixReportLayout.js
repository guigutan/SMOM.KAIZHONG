Ext.define('SIE.Web.EMS.Report.SparePartMitReports.Scripts.SparePartMixReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'SparePartMixReportLayout',
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
        var lineChartControl = me.createLineChart(me);
        var ngPersonChartControl = me.createNgPersonChart();
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
                layout: 'vbox',
                xtype: 'panel',
                id: "SparePartMixReportLayoutMainPanel",
                _ngListView: listView,
                border: false,
                items: [{
                    xtype: "panel",
                    height: 240,
                    //region: "south",
                    region: "north",
                    layout: 'fit',
                    width: "100%",
                    flex: 1,
                    items: [listView.getControl()]
                }
                    ,
                    lineChartControl,
                    ngPersonChartControl
                ]
            }]
        });
    },

    /**
    * 创建折线图控件
    * @method createLineChart
    * @param {Object} me 当前视图对象
    * @return {Ext.create} 折线图控件
    */
    createLineChart: function (me) {
        return Ext.create('SIE.Web.EMS.Report.SparePartMitReports.Scripts.SparePartMixReportLineChart', {
            id: 'SparePartMixReportLineChartId',
            region: 'north',
            layout: 'fit',
            width: "100%",
            flex: 1,
            border: false,
        });
    },

    /**创建条形图控件 */
    createNgPersonChart: function () {
        return Ext.create('SIE.Web.EMS.Report.SparePartMitReports.Scripts.SparePartMixReportExWarehouseChart', {
            id: 'SparePartMixReportExWarehouseChartId',
            region: 'center',
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
            model: 'SIE.EMS.Report.SparePartMitReports.SparePartMixtReportViewModel',
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
                method: 'GetSparePartMixReportData',
                params: [criteria],
                action: 'queryer',
                async: true,
                type: 'SIE.Web.EMS.Report.SparePartMitReports.DataQueryers.SparePartMixReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        debugger;
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

        var clounNameList = passRateInfos.ClounNameList;
        this.setViewColumns(clounNameList, passRateInfos.Datas);

        var turnoverRateList = passRateInfos.TurnoverRateList;
        var lineChart = Ext.getCmp('SparePartMixReportLineChartId');
        if (lineChart) {
            var chartStore = lineChart.items.items[0].getStore();
            if (Ext.isEmpty(turnoverRateList))
                chartStore.removeAll();
            else
                chartStore.setData(turnoverRateList);
            lineChart.items.items[0].setStore(chartStore);
        }
        ////出库数报表
        var exWarehouseList = passRateInfos.ExWarehouseList;
        var  exWarehouseChart = Ext.getCmp('SparePartMixReportExWarehouseChartId');
        if (exWarehouseChart) {
            var exWarehouseChartStore = exWarehouseChart.items.items[0].getStore();
            if (Ext.isEmpty(exWarehouseList))
                exWarehouseChartStore.removeAll();
            else
                exWarehouseChartStore.setData(exWarehouseList);
            exWarehouseChart.items.items[0].setStore(exWarehouseChartStore);
        }

        ////明细
        this._listView.getData().setData(passRateInfos.Datas);
        var mainPanel = Ext.getCmp("SparePartMixReportLayoutMainPanel");
        if (mainPanel)
            mainPanel._dataLoaded = true; //数据已加载
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