using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.AbnormalInfo.AbnormalMonitors.Commands;
using System;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常监控任务视图配置
    /// </summary>
    internal class AbnormalMonitorTaskLogViewConfig : WebViewConfig<AbnormalMonitorTaskLog>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            //View.Property(p => p.HandleNode);
            View.Property(p => p.TaskHandleAction);
            View.Property(p => p.Content).ShowInList(width: 500);
            View.Property(p => p.HandlerId);
        }
    }
}