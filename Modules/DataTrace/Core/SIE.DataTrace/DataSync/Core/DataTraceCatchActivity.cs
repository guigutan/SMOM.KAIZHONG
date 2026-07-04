using Elsa.Services.Models;
using SIE.DataTrace.Activities.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.DataSync.Core
{
    /// <summary>
    /// 追溯节点基类-拉取方法同步数据
    /// </summary>
    public abstract class DataTraceCatchActivity: DataTraceActivityBase
    {
        /// <summary>
        /// 节点到达时可执行生成同步任务
        /// </summary>
        /// <param name="context"></param>
        public override void OnArriveActivityExecute(ActivityExecutionContext context)
        {
            base.OnArriveActivityExecute(context);
            if (!IsDataSyncFinished(context))
            {
                OnGenerateDataSyncTask(context);
            }
        }

        /// <summary>
        /// 生成数据同步任务
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnGenerateDataSyncTask(ActivityExecutionContext context)
        {
        }
    }
}
