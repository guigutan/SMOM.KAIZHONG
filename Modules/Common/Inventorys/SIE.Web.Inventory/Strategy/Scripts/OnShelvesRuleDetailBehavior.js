Ext.define('SIE.Web.Inventory.Strategy.Scripts.OnShelvesRuleDetailBehavior',
    {       
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {          
            var current = view.getParent().getCurrent();
            view.setCurrent(current);
            //this.mon(current, 'propertyChanged', SIE.Web.Inventory.Strategy.OnShelvesRuleDetailAction.onEntityPropertyChanged, this);
        },       
    });