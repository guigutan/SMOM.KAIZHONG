Ext.define('SIE.Web.LES.LesStockCounts.Scripts.LesStockCountDetailBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体实体元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {
            //code here
        },

        /**
         * view生命周期函数--view生成后
         * @param {*} view 生成的view
         */
        onCreated: function (view) {
            //code here
        },

        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var orderDetailDatas = view.getData().data.items;
            var ids = orderDetailDatas.select(function (p) { return p.data.Id; });
            if (ids.length == 0) return;
            orderDetailDatas.forEach(function (p) {
                view.mon(p, 'propertyChanged', SIE.Web.LES.LesStockCounts.Scripts.LesStockCountDtlAction.onEntityPropertyChanged, view);
            });        
        }
    });