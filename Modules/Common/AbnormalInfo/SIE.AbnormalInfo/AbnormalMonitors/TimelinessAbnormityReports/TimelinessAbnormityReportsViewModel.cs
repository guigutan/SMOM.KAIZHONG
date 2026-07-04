using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors.TimelinessAbnormityReports
{
    /// <summary>
    /// 异常时效看板ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("异常时效看板")]
    public class TimelinessAbnormityReportsViewModel : ViewModel
    {
        #region 任务状态 TaskState
        /// <summary>
        /// 任务状态
        /// </summary>
        [Label("异常任务状态")]
        public static readonly Property<TaskStateEnum> TaskStateProperty = P<TimelinessAbnormityReportsViewModel>.Register(e => e.TaskState);

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStateEnum TaskState
        {
            get { return GetProperty(TaskStateProperty); }
            set { SetProperty(TaskStateProperty, value); }
        }
        #endregion

    }



}
