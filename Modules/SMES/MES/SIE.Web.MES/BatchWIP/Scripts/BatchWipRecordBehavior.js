Ext.define("SIE.Web.MES.BatchWip.BatchWipRecordBehavior", {
    CanOpenWindow: true, // 防止重复点击
    beforeCreate: function (viewmeta, curEntity) {
        var gridConfig = viewmeta.gridConfig;
        gridConfig.columns.forEach(function (columnConfig) {
            //指定列
            if (columnConfig.dataIndex === 'SplitQty') {
                //数据列配置
                var config = {
                    //绘制函数
                    renderer: function (value, cellmeta, record, rowIndex, colIndex, store, view) {//设置列样式
                        if (record.data.SplitQty > 0) {
                            //设置单元格样式
                            return "<span style= 'text-decoration: underline;'>" + value + '</span>';
                        }
                        else {
                            return value;
                        }
                    }
                };
                Ext.merge(columnConfig, config);
            }
        });
    },
    onDataLoaded:function(view) {
        var me = this;
        me.view = view;
        var gridConfig = view.gridConfig;
        var grid = view.getControl();
        if (grid) {
            grid.mon(grid, 'cellclick', this.splitClick, me);
        }
    },
    splitClick: function (grid, td, cellIndex, record, tr, rowIndex, e) {
        var me = this;
        
        if (cellIndex === 5 && record.get(grid.getGridColumns()[5].dataIndex) > 0 && me.CanOpenWindow) {
            me.CanOpenWindow = false;
            var batchNo = record.getBatchNo();
            var resourceId = record.getResourceId();
            var processId = record.getProcessId();
            var stationId = record.getStationId();
            SIE.AutoUI.getMeta({
                model: "SIE.MES.BatchWIP.Products.SplitAndMerge.BatchWipSplitViewModel",
                ignoreCommands: true,
                isDetail: false,
                ignoreQuery: true,
                viewGroup: "ListView",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var showView = SIE.AutoUI.createListView(mainBlock);
                    var showViewUI = showView.getControl();
                    showViewUI.flex = 1;
                    var filter = {
                        Method: "GetSplitSourceDatas",
                        Parameters: [batchNo, resourceId, processId, stationId],
                        IsPaging: false
                    };
                    filter = Ext.encode(filter);
                    showView.loadData({
                        filter: filter,
                        action: 'queryer',
                        type: 'SIE.Web.MES.BatchWIP.Products.BatchWipDataQueryer',
                        callback: function (records) {
                            me.leftData = records[0];
                        }
                    });
                    var panel = Ext.create({
                        xtype: 'panel',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [showViewUI]
                    });
                    SIE.Window.show({
                        title: "批次拆分明细".L10N(),
                        width: '670px',
                        height: '50%',
                        buttons: [],
                        modal: true,
                        items: panel,
                        listeners: {
                            beforeclose: function (panel) {
                                me.CanOpenWindow = true;
                            }
                        }
                    });
                },
            });
        }
        
    },
});
