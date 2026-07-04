Ext.define('SIE.Web.Core.Reports.RateReportLayoutBase', {
    extend: 'SIE.autoUI.layouts.Common',

    /**获取最小时间维度 */
    getMinTimeDimension: function () {
        //根据维度顺序，找出最小时间维度
        var dimensions = this.getPivotGridPassRateControl().plugins[1].getConfigPanel().getTopAxisContainer().items.items;
        var minDimension = dimensions.orderByDescending(function (t) {
            return t.field.sortIndex;
        }).first();
        if (minDimension)
            return minDimension.field.dataIndex;
        else
            return 'day';
    },

    /**获取表格控件 */
    getPivotGridPassRateControl: function () {
        this.markAbstract("getPivotGridPassRateControl");
    },

    /**
     * 根据时间维度构建图表数据
     * @param {any} sourceData 源数据
     * @param {any} dimension    时间维度
     */
    getChartData: function (sourceData, dimension) {
        var me = this;
        switch (dimension) {
            case "year":
                return me.getChartDataYear(sourceData);
            case "month":
                return me.getChartDataMonth(sourceData);
            case "day":
                return me.getChartDataDay(sourceData);
            case "week":
                return me.getChartDataWeek(sourceData);
            default:
                return me.getChartDataDay(sourceData);
        }
    },

    /**
    * 根据时间维度构建图表数据-年
    * @param {any} sourceData
    */
    getChartDataYear: function (sourceData) {
        var list = [];
        sourceData.forEach(function (item) {
            var keyTime = Ext.Date.format(new Date(item.monthDay), "Y");
            var existEntity = list.first(function (entity) { return entity.monthDay == keyTime; });
            if (existEntity) {
                //已经存在
                existEntity.totalQty += item.totalQty;
                existEntity.passQty += item.passQty;
            } else {
                list.push({
                    monthDay: keyTime,
                    totalQty: item.totalQty,
                    passQty: item.passQty
                });
            }
            list.forEach(function (entity) {
                entity.passRate = SIE.Web.Core.CommonFuns.round(entity.passQty / entity.totalQty, 4);//计算合格率，取4位小数 
            });
        });
        return list;
    },

    /**
    * 根据时间维度构建图表数据-月
    * @param {any} sourceData
    */
    getChartDataMonth: function (sourceData) {
        var list = [];
        sourceData.forEach(function (item) {
            var keyTime = Ext.Date.format(new Date(item.monthDay), "Y-m");
            var existEntity = list.first(function (entity) { return entity.monthDay == keyTime; });
            if (existEntity) {
                //已经存在
                existEntity.totalQty += item.totalQty;
                existEntity.passQty += item.passQty;
            } else {
                list.push({
                    monthDay: keyTime,
                    totalQty: item.totalQty,
                    passQty: item.passQty
                });
            }
            list.forEach(function (entity) {
                entity.passRate = SIE.Web.Core.CommonFuns.round(entity.passQty / entity.totalQty, 4);//计算合格率，取4位小数 
            });
        });
        return list;
    },

    /**
     * 根据时间维度构建图表数据-日
     * @param {any} sourceData
     */
    getChartDataDay: function (sourceData) {
        return sourceData;
    },

    /**
    * 根据时间维度构建图表数据-周
    * @param {any} sourceData
    */
    getChartDataWeek: function (sourceData) {
        var list = [];
        sourceData.forEach(function (item) {
            var keyTime = Ext.Date.format(new Date(item.monthDay), "第W周");
            var existEntity = list.first(function (entity) { return entity.monthDay == keyTime; });
            if (existEntity) {
                //已经存在
                existEntity.totalQty += item.totalQty;
                existEntity.passQty += item.passQty;
            } else {
                list.push({
                    monthDay: keyTime,
                    totalQty: item.totalQty,
                    passQty: item.passQty
                });
            }
            list.forEach(function (entity) {
                entity.passRate = SIE.Web.Core.CommonFuns.round(entity.passQty / entity.totalQty, 4);//计算合格率，取4位小数 
            });
        });
        return list;
    },
});
