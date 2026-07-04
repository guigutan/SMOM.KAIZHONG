Ext.define('SIE.Web.EMS.Report.EquipCostAnalyses.EquipCostAnalysesLayout', {
    extend: 'SIE.autoUI.layouts.Common',
    xtype: 'EquipCostAnalysesLayout',
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
        var monthlyCostInfoList = me.createMonthlyCostInfoListView(me);
        var listView = me.createEquipCostInfoListView();
        this._listView = listView;
        this._monthlyCostInfoList = monthlyCostInfoList;

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
                id: "esdReportMainPanel",
                items: [
                    {
                        xtype: "tabpanel",
                        id:"tabpanelCenter",
                        height: 240,
                        region: "north",
                        layout: 'fit',
                        width: "100%",
                        flex: 1,
                        items: [listView.getControl(), monthlyCostInfoList.getControl()]
                    },
                    {
                        width: "100%",
                        layout: {
                            type: 'hbox',
                            pack: 'end',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'displayfield',
                                id: 'displayEquipmentConutId',
                                name: 'displayEquipmentConut',
                                value: '本次统计设备数:0台'.t(),
                                fieldLabel: '',
                                width: "10%"
                            }
                        ]
                    },
                ]
            }]
        });
    },

    /**创建设备成本分析 */
    createEquipCostInfoListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Report.EquipCostAnalyses.EquipCostInfo',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            async: false,
            callback: function (res) {
                meta = res;
            }
        });
        meta.gridConfig.manageHeight = true;
        meta.gridConfig.title = "设备成本分析".t();
        meta.gridConfig.features = [{
            ftype: 'summary',
            dock: 'bottom'
        }];
        var listView = SIE.AutoUI.createListView(meta);
        var grid = listView.getControl();
        var gridColumns = grid.config.columns;
        for (var i = 0; i < gridColumns.length; i++) {
           
            var column = gridColumns[i];
            if (column.dataIndex != "EquipCode" && column.dataIndex != "EquipName") {
                column.summaryType = 'sum';
                column.summaryRenderer=function (value, summaryData, dataIndex) {
                    return value.toFixed(2) //保留2位
                }
            }
            if (column.dataIndex == "EquipName") {
                column.summaryRenderer = function (value, summaryData, dataIndex) {
                    return "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;合计：";
                }
            }
        }
        grid.reconfigure(grid.store, gridColumns);
        return listView;
    },
    /**创建月度成本分析 */
    createMonthlyCostInfoListView: function () {
        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Report.EquipCostAnalyses.MonthlyCostInfo',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            async: false,
            callback: function (res) {
                meta = res;
            }
        });
        meta.gridConfig.manageHeight = true;
        meta.gridConfig.title = "月度成本分析".t();
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
                method: 'GetReportData',
                params: [criteria],
                action: 'queryer',
                async: true,
                type: 'SIE.Web.EMS.Report.EquipCostAnalyses.EquipCostAnalysesDataQueryer',
                token: token,
                success: function (res) {
                    if (res.Success) {
                        var resultdata = res.Result;
                        me.bindReportInfos(resultdata);
                        Ext.getBody().unmask();
                    }
                    Ext.getBody().unmask();
                },
                error: function() {
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
        this._monthlyCostInfoList.getData().loadData(passRateInfos.MonthlyCostInfoList);
        this._listView.getData().loadData(passRateInfos.EquipCostInfoInfo);
        
        var displayEquipmentConutId = Ext.getCmp("displayEquipmentConutId");
        displayEquipmentConutId.setData("本次统计设备数:".t() + passRateInfos.EquipmentCount + "台".t());
        var mainPanel = Ext.getCmp("esdReportMainPanel");
        if (mainPanel)
            mainPanel._dataLoaded = true; //数据已加载
    },
});