using System;

namespace SIE.MES.TaskManagement.Models
{
    /// <summary>
    /// 报工任务查询信息
    /// </summary>
    [Serializable]
    public class TaskQueryInfo
    {
        /// <summary>
        /// 任务类型  0待处理任务、1处理中任务、2已完成任务
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 优先级 0普通 1紧急
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// 日期 0未来三天 1最近一周 2本月
        /// </summary>
        public int? QueryDate { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// 每页记录数 框架默认
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 关键字查询   工单编号  物料编码  任务单编号
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 筛选的工序ID
        /// </summary>
        public string ProcessArray { get; set; }

        /// <summary>
        /// 是否显示派工中和未派工
        /// </summary>
        public bool Visiable { get; set; }
    }
}