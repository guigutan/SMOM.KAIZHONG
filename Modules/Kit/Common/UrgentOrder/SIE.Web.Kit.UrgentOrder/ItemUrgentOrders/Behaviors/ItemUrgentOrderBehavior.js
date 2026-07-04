Ext.define('SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Behaviors.ItemUrgentOrderBehavior',
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
            //code here
            var entity = view.getData();
            if (entity) {
                entity.on('propertyChanged', function (e) {
                    if (e.entity.isNew()) {
                        return;
                    }
                    //收料
                    if (e.property == 'IsReceive') {

                        var isReceive = entity.getIsReceive();
                        if (isReceive) {
                            var ReceiveQty = entity.getReceiveQty();
                            if (ReceiveQty == null || ReceiveQty==0) {
                                entity.setReceiveQty(entity.getQty());
                            }
                        } else {
                            entity.setReceiveQty(null);
                        }
                    }
                    //入库
                    if (e.property == 'IsInstorage') {
                        var isInstorage = entity.getIsInstorage();
                        if (isInstorage) {
                            var InstorageQty = entity.getInstorageQty();
                            if (InstorageQty == null || InstorageQty == 0) {
                                entity.setInstorageQty(entity.getQty());
                            }

                        } else {
                            entity.setInstorageQty(null);
                        }
                    }
                    //IQC检验
                    if (e.property == 'IsInspectIqc') {

                        var isInspectIqc = entity.getIsInspectIqc();
                        if (isInspectIqc) {
                            var InspectIqcQty = entity.getInspectIqcQty();
                            if (InspectIqcQty == null || InspectIqcQty == 0) {
                                entity.setInspectIqcQty(entity.getQty());
                            }
                        } else {
                            entity.setInspectIqcQty(null);
                        }
                    }
                    //备料
                    if (e.property == 'IsStockUp') {

                        var isStockUp = entity.getIsStockUp();
                        if (isStockUp) {
                            var StockUpQty = entity.getStockUpQty();
                            if (StockUpQty == null || StockUpQty == 0) {
                                entity.setStockUpQty(entity.getQty());
                            }
                        } else {
                            entity.setStockUpQty(null);
                        }
                    }
                });
            }
        }
    });