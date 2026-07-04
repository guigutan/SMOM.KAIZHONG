Ext.define('SIE.Web.LES.StockOrders.Scripts.StockOrderMergeTimesBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = view;
            if (me.getData()) {
                me.getData().sort('Start', 'ASC');
            }
            var datas = view.getData().data.items;
            datas.forEach(function (p) {
                var startTime = p.getStartTime().getHours() + ":"
                    + (p.getStartTime().getMinutes() < 10 ? "0" + p.getStartTime().getMinutes() : p.getStartTime().getMinutes()) + (":"
                    + (p.getStartTime().getSeconds() < 10 ? "0" + p.getStartTime().getSeconds() : p.getStartTime().getSeconds()))
                var endTime = p.getEndTime().getHours() + ":"
                    + (p.getEndTime().getMinutes() < 10 ? "0" + p.getEndTime().getMinutes() : p.getEndTime().getMinutes()) + (":"
                    + (p.getEndTime().getSeconds() < 10 ? "0" + p.getEndTime().getSeconds() : p.getEndTime().getSeconds()))               
                p.setStartTimeText(startTime);
                p.setEndTimeText(endTime);
                p.markSaved();
            });
        },
    });