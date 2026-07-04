Ext.define('SIE.Web.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports.Scripts.TimelinessAbnormityReportController', {
    alias: 'controller.TimelinessAbnormityReportController',
    /**
     * 绑定饼图-异常任务总览图Store
     * @param {any} passRateInfos
     */
    setPicChartStore: function (pieChart, PieChartStore) {
        var data = [];
        if (PieChartStore.doingCount != 0) {
            data.push({ taskState: '执行中'.t(), data: PieChartStore.doingCount })
        }
        if (PieChartStore.doneCount != 0) {
            data.push({ taskState: '已完成'.t(), data: PieChartStore.doneCount })
        }
        if (PieChartStore.todoCount != 0) {
            data.push({ taskState: '未开始'.t(), data: PieChartStore.todoCount })
        }
        if (PieChartStore.upGradeCount != 0) {
            data.push({ taskState: '预警升级'.t(), data: PieChartStore.upGradeCount })
        }
        if (PieChartStore.cancelCount != 0) {
            data.push({ taskState: '关闭'.t(), data: PieChartStore.cancelCount })
        }
        var store = Ext.create('Ext.data.Store', {
            fields: ['taskState', 'data'],
            data: data
        });
        if (data.length == 1) {
            pieChart.items.items[0].getSeries()[0].setHighlight(false)
        } else {
            pieChart.items.items[0].getSeries()[0].setHighlight(true)
        }
        pieChart.items.items[0].setStore(store)
    },

    setParetoChartStore: function (paretoChart, paretoChartStores) {
        var paretoChartStoresSort = paretoChartStores.sort((a, b) => b.count - a.count);
        var data = []
        var sumCount = 0;//总和cumpercent(用于计算百分比)
        for (const element of paretoChartStoresSort) {
            sumCount += element.count
        }
        for (var i = 0; i < paretoChartStoresSort.length; i++) {
            let cumnumber = 0; //累加数（1到X的累加）
            for (let j = 0; j <= i; j++) {
                cumnumber += paretoChartStoresSort[j].count
            }
            data.push({
                complaint: paretoChartStoresSort[i].name.t(),
                count: paretoChartStoresSort[i].count,
                cumnumber: cumnumber,
                cumpercent: parseFloat((cumnumber / sumCount * 100).toFixed(3)),
                cumpercentString: parseFloat((cumnumber / sumCount * 100).toFixed(3))+"%"
            });
        }
        let store = Ext.create('Ext.data.Store', {
            alias: 'store.pareto',
            fields: ['complaint', 'count', 'cumnumber', 'cumpercent'],
            data: data
        });
        var minCumpercent = data.reduce(function (min, current) {
            return Math.min(min, current.cumpercent);
        }, Infinity);
        var Difference = 100 - minCumpercent;
        majorTickSteps = Difference > 10 ? 10 : 5;
        var minimum = minCumpercent < 10 ? 0 : minCumpercent - 5;
        const maxCount = data.reduce((max, item) => {
            return (item.count > max) ? item.count : max;
        }, 0);
        
        paretoChart.items.items[0].getAxes()[2].setConfig({
            majorTickSteps: majorTickSteps,
            minimum: minimum,
            maximum: 100,
        });
        paretoChart.items.items[0].getAxes()[0].setConfig({
            majorTickSteps: majorTickSteps,
            minimum: 0,
            maximum: maxCount,
        });
        paretoChart.items.items[0].setStore(store);
    },

    //设置折线图Store
    setLineChartStore: function (lineChart, lineChartChartStores) {
        var data = []
        for (var i = 0; i < lineChartChartStores.length + 1; i++) {
            if (i + 1 == lineChartChartStores.length+1) {
                data.push({ date: ">" + i + "天".t(), count: lineChartChartStores[i-1] })
            } else if (i == 0) {
                data.push({ date: 0 })
            }
            else {
                data.push({ date: i +"天".t(), count: lineChartChartStores[i-1] })
            }
        }
        var store = Ext.create('Ext.data.Store', {
            alias: 'store.pareto',
            fields: ['date', 'count'],
            data: data
        });
        lineChart.items.items[0].getAxes()[0].setMaximum(Math.max(...lineChartChartStores)+2)
        lineChart.items.items[0].setStore(store)

    },
    setGridPanelStore: function (ajaxTabs, gridPanelStores) {

        var me = this;
        if (ajaxTabs.getComponent('TimelinessAbnormityReportGridPanel') != undefined) {
            ajaxTabs.remove(ajaxTabs.getComponent('TimelinessAbnormityReportGridPanel'))
        }
        ajaxTabs.add(this.createGridPanel(gridPanelStores))
    },

    setColumnChartStore: function (columnChart, gridPanelStores) {
        var data = []
        for (const element of gridPanelStores) {
            let sumCount = element.todoCount
                + element.doingCount
                + element.doneCount
                + element.cancelCount
            var details = { date: element.date };
            if (sumCount == 0) {
                continue;
                //data.push(details)
            } else {
                details.sumCount = sumCount
            }
            //if (element.todoCount != 0) {
            //    details.ToDo= element.todoCount
            //}
            //if (element.doingCount != 0) {
            //    details.Doing = element.doingCount
            //}
            //if (element.doneCount != 0) {
            //    details.Done = element.doneCount
            //}
            //if (element.cancelCount != 0) {
            //    details.Cancel = element.cancelCount
            //}
                details.ToDo= element.todoCount
                details.Doing = element.doingCount
                details.Done = element.doneCount
                details.Cancel = element.cancelCount
            
            data.push(details)
        }
        var store = Ext.create('Ext.data.Store', {
            alias: 'store.pareto',
            data: data
        });
        const maxSumCount = data.reduce((max, item) => {
            return (item.sumCount > max) ? item.sumCount : max;
        }, 0);
        columnChart.items.items[0].getAxes()[0].setConfig({
            maximum: maxSumCount+1,
        });
        columnChart.items.items[0].setStore(store)
    },

    /**
     * 表格--异常统计列表
     * */
    createGridPanel: function (gridPanelStores) {
        var store = Ext.create('Ext.data.Store', {
            storeId: 'simpsonsStore',
            data: gridPanelStores,
            fields: ['date', 'sumCount', 'todoCountRatio', 'doingCountRatio', 'doneCountRatio','cancelCountRatio', 'cancelCountRatio']

        });
        var columns = [
            { text: '时间'.t(), dataIndex: 'date' },
            { text: '任务总数'.t(), dataIndex: 'sumCount' },
            { text: '未开始'.t(), dataIndex: 'todoCountRatio' },
            { text: '进行中'.t(), dataIndex: 'doingCountRatio' },
            { text: '完成'.t(), dataIndex: 'doneCountRatio' },
            { text: '取消'.t(), dataIndex: 'cancelCountRatio' }
        ];
        return Ext.create('Ext.grid.Panel', {
            title: '异常统计列表'.t(),
            store: store,
            id: "TimelinessAbnormityReportGridPanel",
            alwaysOnTop: 1,
            columns: columns,
            height: 200,
            width: 400,

        });
    },
});