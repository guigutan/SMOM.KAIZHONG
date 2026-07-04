Ext.define('SIE.Web.EMS.Report.MttrAndMtbfReports.Scripts.MttrAndMtbfReportLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'MttrAndMtbfReportLayout',
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
        var listView = me.createNgListView();
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
                id: "MttrAndMtbfReportMainPanel",
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
                }, {
                    width: "100%",
                    height: "5%",
                    layout: {
                        type: 'hbox',
                        pack: 'start',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'displayfield',
                            id: 'displayMttrAndMtbfId',
                            name: 'displayMttrAndMtbfId',
                            value: '注:平均无故障工作时间MTBF=设备运行时长(h)/故障次数，故障平均修复时间MTTR=设备故障总时间(h)/故障次数'.t(),
                            fieldLabel: '',
                            flex: 7
                        },
                        {
                            xtype: 'displayfield',
                            id: 'equipmentConutId',
                            name: 'equipmentConutId',
                            value: '本次统计设备数:0台'.t(),
                            fieldLabel: '',
                            flex: 1
                        }
                    ]
                },
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
        return Ext.create('SIE.Web.EMS.Report.MttrAndMtbfReports.Scripts.MttrAndMtbfReportLineChart', {
            id: 'MttrAndMtbfReportLineChartId',
            region: 'north',
            layout: 'fit',
            width: "100%",
            flex: 1,
            border: false,
        });
    },

    /**创建条形图控件 */
    createNgPersonChart: function () {
        return Ext.create('SIE.Web.EMS.Report.MttrAndMtbfReports.Scripts.MttrAndMtbfReportBarChart', {
            id: 'MttrAndMtbfReportBarChartId',
            region: 'center',
            layout: 'fit',
            width: "100%",
            flex: 1,
            border: false,
        });
    },
    /**创建列表明细视图 */
    createNgListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Report.MttrAndMtbfReports.MttrAndMtbfReportViewModel',
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
                method: 'GetMttrAndMtbfReportData',
                params: [criteria],
                action: 'queryer',
                async: true,
                type: 'SIE.Web.EMS.Report.MttrAndMtbfReports.DataQueryers.MttrAndMtbfReportDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var Resultdata = res.Result;
                        me.bindReportInfos(Resultdata);
                        me._isRunning = false;
                        Ext.getBody().unmask();
                    }
                    Ext.getBody().unmask();
                },
                error: function () {
                    Ext.getBody().unmask();
                }
            });
        } catch (e) {
            throw e;
        } finally {
            me._isRunning = false;
        }
    },

    /**
     * 绑定数据报表
     * @param {any} passRateInfos
     */
    bindReportInfos: function (passRateInfos) {
        //设备台账数量
        var equipmentConutId = Ext.getCmp("equipmentConutId");
        equipmentConutId.setData("本次统计设备数:".t() + passRateInfos.EquipmentCount + " 台".t());

        //MTBF/MTTR统计报表
        var passRateData = passRateInfos.MttrAndMtbfBarReport.MttrAndMtbfBarDatas;
        var lineChart = Ext.getCmp('MttrAndMtbfReportLineChartId');
        if (lineChart) {
            var chartStore = lineChart.items.items[0].getStore();
            if (Ext.isEmpty(passRateData))
                chartStore.removeAll();
            else
                chartStore.setData(passRateData);
            lineChart.items.items[0].setStore(chartStore);
        }
        //维修时长和故障次数报表
        var ngPersonData = passRateInfos.MttrAndMtbfBarReport.MttrAndMtbfBarDatas;
        var barChart = Ext.getCmp('MttrAndMtbfReportBarChartId');
        if (barChart) {
            var chartStore = barChart.items.items[0].getStore();
            if (Ext.isEmpty(ngPersonData))
                chartStore.removeAll();
            else
                chartStore.setData(ngPersonData);
            barChart.items.items[0].setStore(chartStore);

        }

        //明细
        this._listView.getData().loadData(passRateInfos.MttrAndMtbfList);
        var datas = this._listView.getData().getData().items;
        datas.forEach(function (data) {
            if (data) data.markSaved();
        });


        var mainPanel = Ext.getCmp("MttrAndMtbfReportMainPanel");
        if (mainPanel)
            mainPanel._dataLoaded = true; //数据已加载
    },
});