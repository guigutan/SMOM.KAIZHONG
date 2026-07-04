Ext.define('SIE.Web.LES.LesStockCounts.Scripts.LesStockCountBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
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
            var entity = CRT.Context.PageContext.getCurrentRecord();
            if (entity) {
                //view.mon(entity, 'propertyChanged', SIE.Web.WMS.INV.StockCountAction.onEntityPropertyChanged, view);
            }
            var data = CRT.Context.PageContext.getParams();
            if (entity.isNew()) {
                if (data) {
                    entity.setNo(data.No);
                    entity.setSourceType(data.SourceType);
                    entity.setOrderType(SIE.Inventory.Commom.OrderType.StandardCount.value);
                    entity.setState(0);

                }
            } else {
                entity.markSaved();
            }
        },

        onViewReady: function (view) {

        },

        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            //code here
            //view._children.first(function (p) { return p.model == "SIE.WMS.INV.Count.StockCountRange"; }).getData().setCountDimension(0);
        }
    });
