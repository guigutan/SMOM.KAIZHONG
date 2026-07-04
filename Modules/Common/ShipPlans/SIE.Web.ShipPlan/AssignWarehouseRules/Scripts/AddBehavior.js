Ext.define('SIE.Web.ShipPlan.Scripts.AddBehavior',
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
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {

            
        },
        onEntityPropertyChanged: function (e) {
            debugger;
            if (e.property == 'OrderType') {
                if (e.value == 287) {
                    e.entity.setItemType(2);
                }
            }


        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            var me = this;
            debugger;
            var childStore = view.getData();
            view.mon(childStore, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
    });
