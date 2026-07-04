Ext.define('SIE.Web.Inventory.LocationOnhandBehavior',
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
            if (view.getRelations().length > 0) {
                var criter = view.getRelations().first(function (p) { return p._target.isQueryView; });
                if (criter && !criter._target.getData().data.IsWindow) {
                    var storeData = view.getControl().getStore().data;
                    var flag = storeData.items.any(function (p) { return p.data.Id == 0; });
                    if (storeData.items.length > 0 && !flag) {
                        var model = SIE.getModel(view.model);
                        var entity = new model();
                        entity.setItemCode("合计".t());
                        entity.setId(0);



                        var qty = storeData.items.sum(function (p) { return p.data.Qty; });
                        entity.setAvailableQty(Math.floor(qty * 1000) / 1000);
                        var availableQty = storeData.items.sum(function (p) { return p.data.AvailableQty; });
                         entity.setAvailableQty(Math.floor(availableQty * 1000) / 1000);                        
                        var allottedQty = storeData.items.sum(function (p) { return p.data.AllottedQty; });
                        entity.setAllottedQty(Math.floor(allottedQty * 1000) / 1000);
                        var freezingQty = storeData.items.sum(function (p) { return p.data.FreezingQty; });
                        entity.setFreezingQty(Math.floor(freezingQty * 1000) / 1000);

                        storeData.add(entity);
                        entity.markSaved();
                    }
                }
            }
        }
    });