/**
 * SPC查看预览-自定义布局
 * 仅在ReadonlyView（查看预览）时接管渲染，其他ViewGroup走默认布局。
 * 布局结构：父表字段面板 + 明细表Grid + 均值折线图 + 极差折线图
 */
Ext.define('SIE.Web.MES.SpcFromMess.SpcFromMesReadonlyLayout', {
    extend: 'SIE.autoUI.layouts.Common',

    layout: function (regions) {
        var me = this;
        var main = regions.main;
        var view = main.getView();
        var viewGroup = view.viewGroup || view._viewGroup || '';

        console.log('[SPC Layout] layout called, viewGroup=', viewGroup, ', children count=', regions.children ? regions.children.length : 0);

        if (viewGroup !== 'ReadonlyView') {
            return me.callParent(arguments);
        }

        var mainControl = main.getControl();
        var children = regions.children;
        var childControl = null;

        if (children && children.length > 0) {
            childControl = children[0].getControl();
        }

        var avgDivId = 'spc-avg-div-' + Ext.id();
        var rangeDivId = 'spc-range-div-' + Ext.id();

        var headerPanel = Ext.widget('panel', {
            border: false,
            padding: '10 10 0 10',
            html: ''
        });

        var avgChartPanel = Ext.widget('panel', {
            title: 'X̄ 均值控制图',
            border: true,
            margin: '5 10',
            height: 280,
            html: '<div id="' + avgDivId + '" style="width:100%;height:250px;"></div>'
        });

        var rangeChartPanel = Ext.widget('panel', {
            title: 'R 极差控制图',
            border: true,
            margin: '5 10',
            height: 280,
            html: '<div id="' + rangeDivId + '" style="width:100%;height:250px;"></div>'
        });

        var items = [headerPanel];

        if (childControl) {
            items.push(Ext.widget('container', {
                layout: 'fit',
                height: 250,
                margin: '5 10',
                items: [childControl]
            }));
        }

        items.push(avgChartPanel);
        items.push(rangeChartPanel);

        var result = Ext.widget('container', {
            layout: { type: 'vbox', pack: 'start', align: 'stretch' },
            scrollable: 'y',
            items: items
        });

        view.mon(view, 'ondataloaded', function () {
            var data = view.getData();
            console.log('[SPC Layout] ondataloaded fired, data=', data);
            if (!data) return;

            headerPanel.update(me._renderHeaderHtml(data));

            Ext.defer(function () {
                console.log('[SPC Layout] rendering charts, avgDivId=', avgDivId, ', el=', document.getElementById(avgDivId));
                me._renderChart(view, avgDivId, 'avg');
                me._renderChart(view, rangeDivId, 'range');
            }, 500);
        });

        return result;
    },

    _renderHeaderHtml: function (data) {
        var g = function (field) {
            var v = data.get(field);
            return (v !== null && v !== undefined) ? v : '';
        };
        var html = '<table cellpadding="4" cellspacing="0" style="border-collapse:collapse;width:100%;font-size:13px;border:1px solid #ccc;">';
        html += '<tr style="background:#f0f0f0;font-weight:bold;text-align:center;"><td colspan="10" style="border:1px solid #ccc;font-size:15px;padding:6px;">SPC Xbar-R 控制图</td></tr>';
        html += '<tr>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;width:80px;">产品名称</td><td style="border:1px solid #ccc;">' + g('ItemName') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;width:80px;">管制项目</td><td style="border:1px solid #ccc;">' + g('Project') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;width:80px;">测量单位</td><td style="border:1px solid #ccc;">' + g('UnitCode') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;width:80px;">子组大小</td><td style="border:1px solid #ccc;">' + g('GroupCount') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;width:80px;">样本组数</td><td style="border:1px solid #ccc;">' + g('Qty') + '</td>';
        html += '</tr><tr>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;">标准上限USL</td><td style="border:1px solid #ccc;">' + g('StandardUsl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;">标准中心SL</td><td style="border:1px solid #ccc;">' + g('StandardSl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;">标准下限LSL</td><td style="border:1px solid #ccc;">' + g('StandardLsl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;">总组数</td><td style="border:1px solid #ccc;">' + g('TotalQty') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;"></td><td style="border:1px solid #ccc;"></td>';
        html += '</tr><tr>';
        html += '<td style="border:1px solid #ccc;background:#e8f4fd;">均值UCL</td><td style="border:1px solid #ccc;">' + g('AverageUsl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8f4fd;">均值CL</td><td style="border:1px solid #ccc;">' + g('AverageSl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8f4fd;">均值LCL</td><td style="border:1px solid #ccc;">' + g('AverageLsl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;">生产部门</td><td style="border:1px solid #ccc;">' + g('ProductionDept') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;">机台</td><td style="border:1px solid #ccc;">' + g('ResourceName') + '</td>';
        html += '</tr><tr>';
        html += '<td style="border:1px solid #ccc;background:#fff3e0;">极差UCL</td><td style="border:1px solid #ccc;">' + g('RangeUsl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fff3e0;">极差CL</td><td style="border:1px solid #ccc;">' + g('RangeSl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fff3e0;">极差LCL</td><td style="border:1px solid #ccc;">' + g('RangeLsl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;">测量人员</td><td style="border:1px solid #ccc;">' + g('InspectorName') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#fafafa;"></td><td style="border:1px solid #ccc;"></td>';
        html += '</tr><tr style="background:#f9f9ff;">';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">∑X(总)</td><td style="border:1px solid #ccc;">' + g('TotalSumX') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">∑R(总)</td><td style="border:1px solid #ccc;">' + g('TotalSumR') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">X̄(总)</td><td style="border:1px solid #ccc;">' + g('TotalAvgX') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">R̄(总)</td><td style="border:1px solid #ccc;">' + g('TotalAvgR') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">σ</td><td style="border:1px solid #ccc;">' + g('Sigma') + '</td>';
        html += '</tr><tr style="background:#f9f9ff;">';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">CPU</td><td style="border:1px solid #ccc;">' + g('Cpu') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">CPL</td><td style="border:1px solid #ccc;">' + g('Cpl') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">STDEV</td><td style="border:1px solid #ccc;">' + g('Stdev') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">PPK</td><td style="border:1px solid #ccc;">' + g('Ppk') + '</td>';
        html += '<td style="border:1px solid #ccc;background:#e8e8ff;">CPK</td><td style="border:1px solid #ccc;">' + g('Cpk') + '</td>';
        html += '</tr></table>';
        return html;
    },

    _renderChart: function (view, divId, chartType) {
        var data = view.getData();
        if (!data) return;

        var childView = null;
        if (view._children && view._children.length > 0) {
            childView = view._children[0];
        } else if (view.getChildren && view.getChildren().length > 0) {
            childView = view.getChildren()[0];
        }
        if (!childView) {
            console.log('[SPC Layout] no childView found for chart');
            return;
        }

        var store = childView.getStore ? childView.getStore() : null;
        if (!store) {
            console.log('[SPC Layout] no store found for chart');
            return;
        }

        var doRender = function () {
            var records = store.getRange();
            if (!records || records.length === 0) return;

            var chartData = [];
            for (var i = 0; i < records.length; i++) {
                var rec = records[i];
                var val = chartType === 'avg' ? rec.get('AvgX') : rec.get('RangeX');
                chartData.push({ seq: (i + 1).toString(), value: parseFloat(val) || 0 });
            }

            var ucl, cl, lcl;
            if (chartType === 'avg') {
                ucl = parseFloat(data.get('AverageUsl')) || 0;
                cl = parseFloat(data.get('AverageSl')) || 0;
                lcl = parseFloat(data.get('AverageLsl')) || 0;
            } else {
                ucl = parseFloat(data.get('RangeUsl')) || 0;
                cl = parseFloat(data.get('RangeSl')) || 0;
                lcl = parseFloat(data.get('RangeLsl')) || 0;
            }

            for (var j = 0; j < chartData.length; j++) {
                chartData[j].ucl = ucl;
                chartData[j].cl = cl;
                chartData[j].lcl = lcl;
            }

            var el = document.getElementById(divId);
            if (!el) return;

            Ext.create('Ext.chart.CartesianChart', {
                renderTo: divId,
                width: el.offsetWidth || 800,
                height: 240,
                store: Ext.create('Ext.data.Store', { data: chartData }),
                animation: { duration: 200 },
                innerPadding: { left: 20, right: 20 },
                legend: { type: 'sprite', docked: 'bottom' },
                axes: [{
                    type: 'numeric', position: 'left', grid: true,
                    fields: ['value', 'ucl', 'cl', 'lcl'],
                    title: chartType === 'avg' ? 'X̄' : 'R'
                }, {
                    type: 'category', position: 'bottom',
                    fields: ['seq'], title: '样本组序号'
                }],
                series: [{
                    type: 'line', title: chartType === 'avg' ? 'X̄' : 'R',
                    xField: 'seq', yField: 'value', smooth: false,
                    style: { lineWidth: 2, strokeStyle: '#2196F3' },
                    marker: { type: 'circle', radius: 3, fillStyle: '#2196F3' },
                    tooltip: { trackMouse: true, renderer: function (t, r) { t.setHtml(r.get('seq') + ': ' + r.get('value')); } }
                }, {
                    type: 'line', title: 'UCL', xField: 'seq', yField: 'ucl',
                    smooth: false, showMarkers: false,
                    style: { lineWidth: 1.5, strokeStyle: '#F44336', lineDash: [6, 3] }
                }, {
                    type: 'line', title: 'CL', xField: 'seq', yField: 'cl',
                    smooth: false, showMarkers: false,
                    style: { lineWidth: 1.5, strokeStyle: '#4CAF50', lineDash: [6, 3] }
                }, {
                    type: 'line', title: 'LCL', xField: 'seq', yField: 'lcl',
                    smooth: false, showMarkers: false,
                    style: { lineWidth: 1.5, strokeStyle: '#F44336', lineDash: [6, 3] }
                }]
            });
        };

        if (store.isLoaded && store.isLoaded()) {
            doRender();
        } else {
            store.on('load', doRender, this, { single: true });
        }
    }
});
