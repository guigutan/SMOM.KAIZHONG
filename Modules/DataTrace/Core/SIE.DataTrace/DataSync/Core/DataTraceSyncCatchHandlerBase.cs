using System;
using System.Collections.Generic;
using SIE.DataSync.Core;
using SIE.DataSync.DataSyncTasks;

namespace SIE.DataTrace.DataSync
{
    /// <summary>
    /// 追溯数据-拉取方式基类
    /// </summary>
    public abstract class DataTraceSyncCatchHandlerBase : DataSyncCatchHandlerBase
    {
        /// <summary>
        /// 是否同步完成
        /// </summary>
        /// <param name="task"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual bool IsSyncFinished(DataSyncTask task,object result)
        {
            return true;
        }
    }
}
