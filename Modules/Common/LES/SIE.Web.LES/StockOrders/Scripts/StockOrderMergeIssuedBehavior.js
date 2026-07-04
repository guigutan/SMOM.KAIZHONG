Ext.define('SIE.Web.LES.StockOrders.Scripts.StockOrderMergeIssuedBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var datas = view.getData().data.items;
            datas.forEach(function (p) {              
                p.setLinesideWarehouseId_Display(p.getWarehouseName());
                p.markSaved();
            });
        },
    });