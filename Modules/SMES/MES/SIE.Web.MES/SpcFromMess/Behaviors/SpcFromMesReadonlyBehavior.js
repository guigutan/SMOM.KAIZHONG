/**
 * SPC查看预览行为（Behavior）
 * 功能：在ReadonlyView的detail视图数据加载后，在页面底部追加均值折线图和极差折线图
 * 不修改默认的表单/子表布局，仅在下方追加图表区域
 */
Ext.define("SIE.Web.MES.SpcFromMess.Behaviors.SpcFromMesReadonlyBehavior", {

    onDataLoaded: function (view) {
        var me = this;
        var data = view.getData();
        if (!data) return;

        var control = view.getControl();
        if (!control) return;

        // 避免重复添加
        if (control._spcChartsAdded) return;
        control._spcChartsAdded = true;

        // 获取控制限值
        var averageUsl = parseFloat(data.get('AverageUsl')) || 0;
        var averageSl = parseFloat(data.get('AverageSl')) || 0;
        var averageLsl = parseFloat(data.get('AverageLsl')) || 0;
        var rangeUsl = parseFloat(data.get('RangeUsl')) || 0;
        var rangeSl = parseFloat(data.get('RangeSl')) || 0;
        var rangeLsl = parseFloat(data.get('RangeLsl')) || 0;

        // 创建图表容器
        var avgChartPanel = Ext.create('Ext.panel.Panel', {
            title: 'X̄ 均值控制图',
            itemId: 'spcAvgChart',
            border: true,
            margin: '10 0 5 0',
            height: 300,
            layout: 'fit'
        });

        var rangeChartPanel = Ext.create('Ext.panel.Panel', {
            title: 'R 极差控制图',
            itemId: 'spcRangeChart',
            border: true,
            margin: '5 0 10 0',
            height: 300,
            layout: 'fit'
        });

        // 找到外层容器并追加图表
        var container = control.ownerCt || control;
        container.add(avgChartPanel);
        container.add(rangeChartPanel);
        container.updateLayout();

        // 等待子表数据加载后渲染图表
        me._waitForChildData(view, function (records) {
            me._renderLineChart(avgChartPanel, records, 'AvgX', averageUsl, averageSl, averageLsl, 'X̄');
            me._renderLineChart(rangeChartPanel, records, 'RangeX', rangeUsl, rangeSl, rangeLsl, 'R');
        });
    },

    _waitForChildData: function (view, callback) {
        var children = view._children || (view.getChildren ? view.getChildren() : []);
        if (!children || children.length === 0) {
            Ext.defer(function () {
                var c2 = view._children || [];
                if (c2.length > 0) {
                    this._loadFromChildView(c2[0], callback);
                }
            }, 1000, this);
            return;
        }
        this._loadFromChildView(children[0], callback);
    },

    _loadFromChildView: function (childView, callback) {
        if (!childView) return;
        var store = childView.getStore ? childView.getStore() : null;
        if (!store) return;

        if (store.getCount() > 0) {
            callback(store.getRange());
        } else {
            store.on('load', function () {
                callback(store.getRange());
            }, this, { single: true });
        }
    },

    _renderLineChart: function (panel, records, field, ucl, cl, lcl, label) {
        if (!records || records.length === 0) return;

        var chartData = [];
        for (var i = 0; i < records.length; i++) {
            var rec = records[i];
            var val = rec.get ? rec.get(field) : rec[field];
            chartData.push({
                seq: (i + 1).toString(),
                value: parseFloat(val) || 0,
                ucl: ucl,
                cl: cl,
                lcl: lcl
            });
        }

        var chartStore = Ext.create('Ext.data.Store', {
            fields: ['seq', 'value', 'ucl', 'cl', 'lcl'],
            data: chartData
        });

        var chart = Ext.create('Ext.chart.CartesianChart', {
            store: chartStore,
            animation: { duration: 200 },
            innerPadding: { left: 20, right: 20 },
            legend: { type: 'sprite', docked: 'bottom' },
            axes: [{
                type: 'numeric',
                position: 'left',
                grid: true,
                fields: ['value', 'ucl', 'cl', 'lcl'],
                title: label
            }, {
                type: 'category',
                position: 'bottom',
                fields: ['seq'],
                title: '样本组序号'
            }],
            series: [{
                type: 'line',
                title: label,
                xField: 'seq',
                yField: 'value',
                smooth: false,
                style: { lineWidth: 2, strokeStyle: '#2196F3' },
                marker: { type: 'circle', radius: 3, fillStyle: '#2196F3' },
                tooltip: {
                    trackMouse: true,
                    renderer: function (t, r) { t.setHtml(r.get('seq') + ': ' + r.get('value')); }
                }
            }, {
                type: 'line',
                title: 'UCL',
                xField: 'seq',
                yField: 'ucl',
                smooth: false,
                showMarkers: false,
                style: { lineWidth: 1.5, strokeStyle: '#F44336', lineDash: [6, 3] }
            }, {
                type: 'line',
                title: 'CL',
                xField: 'seq',
                yField: 'cl',
                smooth: false,
                showMarkers: false,
                style: { lineWidth: 1.5, strokeStyle: '#4CAF50', lineDash: [6, 3] }
            }, {
                type: 'line',
                title: 'LCL',
                xField: 'seq',
                yField: 'lcl',
                smooth: false,
                showMarkers: false,
                style: { lineWidth: 1.5, strokeStyle: '#F44336', lineDash: [6, 3] }
            }]
        });

        panel.add(chart);
        panel.updateLayout();
    }
});
