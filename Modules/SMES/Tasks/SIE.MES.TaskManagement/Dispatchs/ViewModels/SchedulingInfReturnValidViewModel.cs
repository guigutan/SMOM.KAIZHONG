using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.ViewModels
{
    [RootEntity, Serializable]
    public class SchedulingInfReturnValidViewModel : ViewModel
    {
        #region 退回原因 ReturnReason
        /// <summary>
        /// 退回原因
        /// </summary>
        [Label("退回原因")]
        public static readonly Property<string> ReturnReasonProperty = P<SchedulingInfReturnValidViewModel>.Register(e => e.ReturnReason);

        /// <summary>
        /// 退回原因
        /// </summary>
        public string ReturnReason
        {
            get { return this.GetProperty(ReturnReasonProperty); }
            set { this.SetProperty(ReturnReasonProperty, value); }
        }
        #endregion

        #region 任务单Id TaskId
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单Id")]
        public static readonly Property<string> TaskIdProperty = P<SchedulingInfReturnValidViewModel>.Register(e => e.TaskId);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public string TaskId
        {
            get { return this.GetProperty(TaskIdProperty); }
            set { this.SetProperty(TaskIdProperty, value); }
        }
        #endregion

    }
}
