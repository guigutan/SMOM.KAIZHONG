Ext.define('SIE.Web.MES.TaskManagement.Report.Behaviors.DispatchTaskCriteriaBehaviors', {
    /**
     * view生命周期函数--view生成前
     * @param {*} meta 实体实体元数据
     * @param {*} curEntity 当前操作实体(可空)
     */
    beforeCreate: function (meta, curEntity) {
    },

    /**
     * view生命周期函数--view生成后
     * @param {*} view 生成的view
     */
    onCreated: function (view) {
    },
    /**
     * view生命周期函数-view聚合后
     * @param {any} view
     */
    onViewReady: function (view) {
        /*view.getData().setIsShowDispatchTask(true);*/
        //SIE.invokeDataQuery({
        //    type: "SIE.Web.Common.Configs.Commands.DataQueryers.ConfigValueDataQueryer",
        //    method: "GetConfigValue",
        //    params: [view.getData().getId()],
        //    async: false,
        //    callback: function (res) {
        //        view.getData().data.ConfigValue = res.Result[0];
        //    }
        //});
    },

    /**
     * view生命周期函数--数据加载后
     * @param {any} view 逻辑视图
     */
    onDataLoaded: function (view) {
        var me = this;
        
     },
});