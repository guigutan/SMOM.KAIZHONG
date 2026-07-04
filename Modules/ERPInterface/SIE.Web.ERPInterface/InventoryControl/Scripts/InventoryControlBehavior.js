Ext.define('SIE.Web.ERPInterface.InventoryControl.Scripts.InventoryControlBehavior', 
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
            var params = CRT.Context.PageContext.getParams();
            //view.mon(entity, 'propertyChanged', SIE.Web.WMS.Receipt.AsnAction.onEntityPropertyChanged, view);
        },

        onViewReady: function (view) {
            var entity = CRT.Context.PageContext.getCurrentRecord();
            var params = CRT.Context.PageContext.getParams();
            var SelectData = params.SelectData
            var queryView = view.getRelations()[0].getTarget();
            queryData = queryView.getData();
            queryData.setIsShowDifferent(true);
            queryData.setIsShowZero(true);
            
            
        },

        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            
        },
    });