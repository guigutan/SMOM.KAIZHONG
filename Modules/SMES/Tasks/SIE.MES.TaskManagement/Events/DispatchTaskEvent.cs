using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;

namespace SIE.MES.TaskManagement.Events
{
    /// <summary>
    /// 任务单事件
    /// </summary>
    public class DispatchTaskEvent
    {
        /// <summary>
        /// 
        /// </summary>
        protected DispatchTaskEvent()
        {
            //
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTask"></param>
        public DispatchTaskEvent(DispatchTask dispatchTask)
        {
            DispatchTasks.Add(dispatchTask);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTasks"></param>
        public DispatchTaskEvent(EntityList<DispatchTask> dispatchTasks)
        {
            DispatchTasks.AddRange(dispatchTasks);
        }
        /// <summary>
        /// 
        /// </summary>
        public EntityList<DispatchTask> DispatchTasks { get; } = new EntityList<DispatchTask>();
    }

    /// <summary>
    /// 任务单派工
    /// </summary>
    public class DispatchTaskDispatched : DispatchTaskEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTask"></param>
        public DispatchTaskDispatched(DispatchTask dispatchTask) : base(dispatchTask)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTasks"></param>
        public DispatchTaskDispatched(EntityList<DispatchTask> dispatchTasks) : base(dispatchTasks)
        {
        }
    }

    /// <summary>
    /// 任务单开工
    /// </summary>
    public class DispatchTaskStartUp : DispatchTaskEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTask"></param>
        public DispatchTaskStartUp(DispatchTask dispatchTask) : base(dispatchTask)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTasks"></param>
        public DispatchTaskStartUp(EntityList<DispatchTask> dispatchTasks) : base(dispatchTasks)
        {
        }
    }

    /// <summary>
    /// 任务单完成
    /// </summary>
    public class DispatchTaskFinish : DispatchTaskEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTask"></param>
        public DispatchTaskFinish(DispatchTask dispatchTask) : base(dispatchTask)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTasks"></param>
        public DispatchTaskFinish(EntityList<DispatchTask> dispatchTasks) : base(dispatchTasks)
        {
        }
    }

    /// <summary>
    /// 任务单关闭
    /// </summary>
    public class DispatchTaskClose : DispatchTaskEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTask"></param>
        public DispatchTaskClose(DispatchTask dispatchTask) : base(dispatchTask)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTasks"></param>
        public DispatchTaskClose(EntityList<DispatchTask> dispatchTasks) : base(dispatchTasks)
        {
        }
    }
}