Ext.define('SIE.Web.SO.SaleOrders.Behaviors.DetailPropertyChangedBehavior',
    {
        /**
          * view生命周期函数--view生成后
          * @param {DetailView} view 生成的view
          */
        onCreated: function (view) {
            var me = this;
            mainView = view;
            view.mun(view, 'currentChanged');
            view.mon(view, 'currentChanged', me.onCurrentChanged, me);
        },

        /**
        * view生命周期函数--数据加载后
        * @param {any} view 逻辑视图
        */
        onDataLoaded: function (view) {
            var me = this;
            this.view = view;
            var entity = view.getData();
            view.mon(entity, "propertyChanged", me.onPropertyChanged, me);
        },

       
        /**
         * 当前变更事件
         * @method onCurrentChanged
         * @param {arg} arg 新旧值
         */
        onCurrentChanged: function (arg) {
            var me = this;
            if (arg.newValue && arg.oldValue != arg.newValue) {
                mainView.mun(arg.newValue, 'propertyChanged');
                mainView.mon(arg.newValue, "propertyChanged", me.onPropertyChanged, me);
            }
        },

        /**
         * 属性变更处理
         * @param {any} 
         */
        onPropertyChanged: function (e) {
            var me = this;
            if (e.property.length > 0) {
                var entity = e.entity;
                if (e.property === "Area" || e.property === "Qty") {
                    var area = entity.getArea();
                    var qty = entity.getQty();
                    if (area === null) { area = 0 };
                    if (qty === null) { qty = 0 };
                    var SingleArea =  Math.round((area / qty) * Math.pow(10, 3)) / Math.pow(10, 3);
                    entity.setSingleArea(SingleArea);
                }
                if (e.property === "OrderClassify") {
                    var orderClassify = entity.getOrderClassify();
                    var isNew = orderClassify === '新单';
                    entity.setIsNew(isNew);
                }
            }
        }
    });