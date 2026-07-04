//移除关闭事件
Ext.define('SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailCloseBehavior',
    {
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            //移除关闭事件
            view.mon(view, 'beforeClosewin', this.beforeClosewin, view);
        },
        beforeClosewin: function (returnObj) {
            this.mun(this, 'beforeClosewin');
        }
    });