Ext.define('SIE.Web.AbnormalInfo.AnomalyMonitors.AbnormalMonitorTasks.WritingAbnormalProcessViewPage',
    {
        extend: 'SIE.Page',
        /**
         * 页面初始化-绑定自定义视图生成器
         * @param {*} meta 视图元数据
         */
        onInit: function (meta) {
            meta.uiGenerator = "SIE.Web.AbnormalInfo.AnomalyMonitors.AbnormalMonitorTasks.WritingAbnormalProcessUIGenerator";
        }
    });