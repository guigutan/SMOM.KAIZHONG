using Elsa.Attributes;
using Elsa.Design;
using Elsa.Services.Models;
using SIE.DataTrace.Activities.Core;

namespace SIE.DataTrace.DataSyncPush.Core
{
    /// <summary>
    /// 追溯节点基类-推送方法同步数据
    /// </summary>
    public abstract class DataTracePushActivity : DataTraceActivityBase
    {
        #region 属性
        #region 同步方式是否推送方式
        /// <summary>
        /// 同步方式是否推送方式
        /// </summary>
        [ActivityInput(Label = "同步方式是否推送方式", DefaultValue = true, UIHint = ActivityInputUIHints.Checkbox, Order = -1)]
        public virtual bool isPush { get { return true; } set { } }
        #endregion
        #endregion

        /// <summary>
        /// 数据同步是否已完成
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool IsDataSyncFinished(ActivityExecutionContext context)
        {
            //推送方式，数据默认未同步完毕
            return false;
        }
    }
}
