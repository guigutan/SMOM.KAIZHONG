using SIE.Domain;
using System;

namespace SIE.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 任务控制器
    /// </summary>
    public class TaskController : DomainController
    {
        /// <summary>
        /// 获取任务类型
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>任务类型</returns>
        public virtual TaskType GetTaskType(string name)
        {
            return Query<TaskType>().Where(p => p.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// 获取任务类型列表
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>任务类型列表</returns>
        public virtual EntityList<TaskType> GetTaskTypes(string name)
        {
            return Query<TaskType>().Where(p => p.Name.Contains(name)).ToList();
        }

        /// <summary>
        /// 获取任务类型列表
        /// </summary>
        /// <param name="moduleCategory">模块分类</param>
        /// <returns>任务类型列表</returns>
        public virtual EntityList<TaskType> GetTaskTypes(ModuleCategory moduleCategory)
        {
            return Query<TaskType>().Where(p => p.ModuleCategory == moduleCategory).ToList();
        }

        /// <summary>
        /// 最近15天创建的任务
        /// </summary>
        /// <returns>最近15天我创建的任务</returns>
        public virtual EntityList<TaskInfo> GetMyLatestCreated()
        {
            return Query<TaskInfo>().Where(p => p.CreateBy == RT.IdentityId && p.CreateDate >= DateTime.Today.AddDays(-15)).OrderByDescending(p => p.Importance).OrderBy(p => p.Status).OrderByDescending(p => p.CreateDate).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 待办任务
        /// </summary>
        /// <returns>最近15天我待办的任务</returns>
        public virtual EntityList<TaskInfo> GetMyPadding()
        {

            return Query<TaskInfo>().Where(p => p.AssignToId == RT.IdentityId && p.CreateDate >= DateTime.Today.AddDays(-15) && (p.Status == TaskStatus.Padding || p.Status == TaskStatus.Finish)).OrderByDescending(p => p.Importance).OrderBy(p => p.Status).OrderByDescending(p => p.CreateDate).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 未完成任务
        /// </summary>
        /// <returns>最近15天我未完成的任务</returns>
        public virtual EntityList<TaskInfo> GetMyDelayed()
        {
            return Query<TaskInfo>().Where(p => p.AssignToId == RT.IdentityId && p.CreateDate >= DateTime.Today.AddDays(-15) && ((p.Status == TaskStatus.Padding && p.PlanEnd < DateTime.Today) || (p.Status == TaskStatus.Finish && p.ActualEnd > p.PlanEnd))).OrderByDescending(p => p.Importance).OrderBy(p => p.Status).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 最近15日任务
        /// </summary>
        /// <returns>最近15日的任务</returns>
        public virtual EntityList<TaskInfo> GetMyLatest()
        {
            return Query<TaskInfo>().Where(p => p.AssignToId == RT.IdentityId && p.CreateDate >= DateTime.Today.AddDays(-15)).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 最近15天抄送给我的任务
        /// </summary>
        /// <returns>最近15天抄送给我的任务</returns>
        public virtual EntityList<TaskInfo> GetCopyto()
        {
            return Query<TaskInfo>().Where(p => p.CopyToId == RT.IdentityId && p.CreateDate >= DateTime.Today.AddDays(-15)).OrderByDescending(p => p.Importance).OrderBy(p => p.Status).OrderByDescending(p => p.CreateDate).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
           

        }


    }
}
