Ext.define('SIE.Web.Andon.AndonMonthReports.Scripts.AndonMonthChart', {
    extend: 'Ext.Panel',
    xtype: 'AndonMonthChart',
    controller: 'AndonMonthChartController',
    width: 350,
    items: {
        xtype: 'cartesian',
        reference: 'chart',
        width: '100%',
        minHeight: 200,
        captions: {
            title: '  '.t(),
        },
        legend: {
            type: 'sprite',
            docked: 'right'
        },
        interactions: {
            type: 'panzoom',
            zoomOnPanGesture: true
        },
        animation: {
            duration: 200
        },
        store: {
            fields: ['GroupName', 'AndonNum', 'AndonTime', 'AndonStopNum', 'AndonStopLine','TriggerAccuracy'],

        },
        innerPadding: {
            left: 40,
            right: 40
        },
        axes: [{
            type: 'numeric',
            position: 'left',
            minimum: 0,
            fields: ['AndonNum', 'AndonTime', 'AndonStopNum', 'AndonStopLine','TriggerAccuracy'],
            renderer: 'onLineChartAxisLabelRender'
        }, {
            type: 'category',
            position: 'bottom',
            fields: ['GroupName'],
            grid: false,
        },],
        series: [{
            type: 'line',
            title: '安灯次数'.t(),
            xField: 'GroupName',
            yField: 'AndonNum',
            style: {
                lineWidth: 2,
            },
            label: {
                display: 'rotate',
                field: 'AndonNum',
                renderer: 'onBarSeriesLabelRender'
            },
            marker: {
                animation: {
                    duration: 200,
                    easing: 'backOut'
                }
            },
            highlightCfg: {
                scaling: 2,
                rotationRads: Math.PI / 4
            },
            tooltip: {
                trackMouse: true,
                renderer: 'onLineBarChartSeriesTooltipRender'
            }
        },
            {
                type: 'line',
                title: '安灯时长'.t(),
                xField: 'GroupName',
                yField: 'AndonTime',
                style: {
                    lineWidth: 2,
                },
                label: {
                    display: 'rotate',
                    field: 'AndonTime',
                    renderer: 'onBarSeriesLabelRender'
                },
                marker: {
                    animation: {
                        duration: 200,
                        easing: 'backOut'
                    }
                },
                highlightCfg: {
                    scaling: 2,
                    rotationRads: Math.PI / 4
                },
                tooltip: {
                    trackMouse: true,
                    renderer: 'onLineBarChartSeriesTooltipRender'
                }
            },
            {
                type: 'line',
                title: '停线次数'.t(),
                xField: 'GroupName',
                yField: 'AndonStopNum',
                style: {
                    lineWidth: 2,
                },
                label: {
                    display: 'rotate',
                    field: 'AndonStopNum',
                    renderer: 'onBarSeriesLabelRender'
                },
                marker: {
                    animation: {
                        duration: 200,
                        easing: 'backOut'
                    }
                },
                highlightCfg: {
                    scaling: 2,
                    rotationRads: Math.PI / 4
                },
                tooltip: {
                    trackMouse: true,
                    renderer: 'onLineBarChartSeriesTooltipRender'
                }
            },
            {
                type: 'line',
                title: '停线时长'.t(),
                xField: 'GroupName',
                yField: 'AndonStopLine',
                style: {
                    lineWidth: 2,
                },
                label: {
                    display: 'rotate',
                    field: 'AndonStopLine',
                    renderer: 'onBarSeriesLabelRender'
                },
                marker: {
                    animation: {
                        duration: 200,
                        easing: 'backOut'
                    }
                },
                highlightCfg: {
                    scaling: 2,
                    rotationRads: Math.PI / 4
                },
                tooltip: {
                    trackMouse: true,
                    renderer: 'onLineBarChartSeriesTooltipRender'
                }
            }
            //,{
            //    type: 'line',
            //    title: '安灯名称变更率'.t(),
            //    xField: 'GroupName',
            //    yField: 'TriggerAccuracy',
            //    style: {
            //        lineWidth: 2,
            //    },
            //    label: {
            //        display: 'rotate',
            //        field: 'TriggerAccuracy',
            //        renderer: 'onBarSeriesLabelRender'
            //    },
            //    marker: {
            //        animation: {
            //            duration: 200,
            //            easing: 'backOut'
            //        }
            //    },
            //    highlightCfg: {
            //        scaling: 2,
            //        rotationRads: Math.PI / 4
            //    },
            //    tooltip: {
            //        trackMouse: true,
            //        renderer: 'onLineBarChartSeriesTooltipRender'
            //    }
            //}
        ]
    }
});