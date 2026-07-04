using SIE.Inventory.Task.Configs;
using System;

namespace SIE.Web.Inventory.Task.Configs
{
    /// <summary>
    /// 任务管理参数 视图配置
    /// </summary>
    public class TaskParameterConfigValueViewConfig : WebViewConfig<TaskParameterConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("任务管理参数".L10N());
            using (View.OrderProperties())
            {
                View.Property(p => p.ExecuteTimeout).DefaultValue(60).Show();
                View.Property(p => p.NotGetTimeout).DefaultValue(60).Show();
                View.Property(p => p.UntreatedTimeout).DefaultValue(60).Show();
                View.Property(p => p.UrgentMaxCount).DefaultValue(5).Show();
            }
        }
    }
}
