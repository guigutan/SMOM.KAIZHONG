SIE.defineCommand('SIE.Web.EMS.Equipments.AlarmStates.Commands.ViewChartCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看曲线", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    token: null,
    /*
    * @override 是否可执行
    * @param {} view
    * @returns {}
    */
    canExecute: function (view) {
        var parentEntity = view.getParent().getCurrent();

        if (parentEntity == null) {
            return false;
        }

        return true;
    },
    /**
     * @override 
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        this.token = view.token;
        me.initData(view);
    },

    initData: function (view) {
        var tags = [];
        view.getData().data.items.forEach(function (tag) {
            if (tag.getIsShowInChart()) {
                tags.push(tag.getFullTagName());
            }            
        })

        var dpBeginDateTime = view.getControl().dockedItems.items.first(function (p) {
            return p.xtype == "toolbar" && p.id == "toolbarDateTime"
        }).items.items.first(function (p) {
            return p.name == "dpBeginDateTime"
        });

        var dpEndDateTime = view.getControl().dockedItems.items.first(function (p) {
            return p.xtype == "toolbar" && p.id == "toolbarDateTime"
        }).items.items.first(function (p) {
            return p.name == "dpEndDateTime"
        });

        this.id = id;
        var div = document.getElementById("mainEchart");
        var myChart = echarts.init(div);

        //myChart.showLoading({
        //    text: "图表数据正在努力加载..."
        //});

        this.myChart = myChart;

        // 初始化数据

        var option;

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Equipments.AlarmStates.DataQuery.AlarmStateDataQueryer",
            method: "GetHistoryTagValue",
            params: [tags, dpBeginDateTime.getValue(), dpEndDateTime.getValue()],
            token: this.token,
            success: function (res) {
                var data = res.Result;

                var series = [];
                var legends = [];
                data.ChartSeries.forEach(function (chartSerie) {
                    series.push({
                        name: chartSerie.Name,
                        type: 'line',
                        data: chartSerie.DataValues
                    });

                    legends.push(chartSerie.Name);
                });

                myChart.setOption(
                    (option = {
                        title: {
                            //text: 'Beijing AQI',
                            //left: '1%'
                        },
                        tooltip: {
                            trigger: 'axis'
                        },
                        legend: {
                            data: legends
                        },
                        grid: {
                            left: '5%',
                            right: '15%',
                            bottom: '10%'
                        },
                        xAxis: {
                            type: 'category',
                            data: data.XaxisList
                        },
                        yAxis: {
                            min: 'dataMin',
                            max: 'dataMax'
                        },
                        toolbox: {
                            right: 10,
                            feature: {
                                dataZoom: {
                                    yAxisIndex: 'none'
                                },
                                restore: {},
                                saveAsImage: {}
                            }
                        },
                        dataZoom: [
                            {
                                //startValue: '2014-06-01'
                            },
                            {
                                type: 'inside'
                            }
                        ],
                        series: series
                    })
                );
            }
        });

        option && myChart.setOption(option);
    }
});