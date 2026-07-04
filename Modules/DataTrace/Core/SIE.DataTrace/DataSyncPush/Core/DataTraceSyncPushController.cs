using SIE.DataTrace.TraceMainDatas;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.DataSyncPush
{
    /// <summary>
    /// 追溯数据-推送方式流程控制器
    /// </summary>
    public class DataTraceSyncPushController : DomainController
    {
        /// <summary>
        /// 触发工作流程里挂起中的推送追溯节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mainData"></param>
        public virtual void TriggerDataTracePushActivity<T>(TraceMainData mainData) where T : Entity
        {

        }
    }
}
