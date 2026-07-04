using SIE.AbnormalInfo.AbnormalEvent;
using SIE.EventMessages.AbnormalInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.Events
{
    /// <summary>
    /// 生成异常任务监听处理
    /// </summary>
    public class AbnormalMonitorTaskEventListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static AbnormalMonitorTaskEventListener Instance { get; set; } = new AbnormalMonitorTaskEventListener();

        /// <summary>
        /// 订阅不合格审核流程完成信息
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<PubTaskEvent>(this, e =>
            {
                RT.Service.Resolve<AbnormalEventController>().CreateAbnormalTask(e);
            });
        }
    }
}
