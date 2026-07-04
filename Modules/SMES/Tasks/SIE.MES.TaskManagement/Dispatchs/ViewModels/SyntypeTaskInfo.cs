using System.Collections.Generic;

namespace SIE.MES.TaskManagement.Dispatchs.ViewModels
{
    /// <summary>
    /// 关联任务单信息
    /// </summary>
    public class AssociatedDispatchTaskInfo
    {
        /// <summary>
        /// 当前主料任务单的共模辅料关联任务单所关联的所有主料任务单字典（辅料任务单Id+所有对应主料的任务单列表，共模任务单存在一个辅料对应多个主料任务单，即多个主料共模任务单存在关联相同的辅料任务单）
        /// </summary>
        public Dictionary<double, List<DispatchTask>> DicSyntypeMainDispatchTasks { get; set; } = new Dictionary<double, List<DispatchTask>>();

        /// <summary>
        /// 当前主料任务单的共模辅料关联任务单字典
        /// </summary>
        public Dictionary<double, List<DispatchTask>> DicSyntypeDispatchTasks { get; set; } = new Dictionary<double, List<DispatchTask>>();

        /// <summary>
        /// 当前主料任务单的非共模（合并/拆分）关联任务单字典
        /// </summary>
        public Dictionary<double, List<DispatchTask>> DicNotSyntypeDispatchTasks { get; set; } = new Dictionary<double, List<DispatchTask>>();

        /// <summary>
        /// 任务单Id+派工任务明细字典
        /// </summary>
        public Dictionary<double, List<DispatchTaskDetail>> DicSyntypeDispatchTaskDetails { get; set; } = new Dictionary<double, List<DispatchTaskDetail>>();
    }
}
