Ext.define('SIE.Web.Fixtures.Demands.Scripts.DemandBehavior',
    {
        /**
        * onViewReady 视图加载完成
        * @param {*} view 当前视图
        */
        onViewReady: function (view) {
            var me = this;
            me.mainView = view;
            view.mon(view, 'beforeClosewin', me.beforeClosewin, me);
        },

        /**
         * @override 重写关闭前方法
         * @param {} returnObj 输出参数，如果有事件需要解除绑定，例如propertyChanged，则赋值data;如果需要提示是否未保存，则赋值hasData
         * @returns {} 
         */
        beforeClosewin: function (returnObj) {
            var me = this;
            var view = me.mainView;
            me.unloadView = view.findChild('SIE.Fixtures.FixtureDemands.ViewModels.FixtureUnloadViewModel');

            if (me.isDirtyData(me.unloadView) === true)
                view.beforeClosewin(returnObj);
        },

        /**
         * @isDirtyData 是否存在新增数据
         * @param {} unloadView 出库明细视图
         * @returns {}
         */
        isDirtyData: function (unloadView) {
            var items = unloadView.getData().data.items;
            if (items != null && items.length > 0) {
                for (var i = 0; i < items.length; i++) {
                    if (items[i].data.IsOld == 0) {
                        return true;
                    }
                }               
            }

            return false;
        },
    });