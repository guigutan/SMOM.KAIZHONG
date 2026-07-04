using System;
using System.Collections.Generic;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 执行对象信息
    /// </summary>
    [Serializable]
    public class TaskPerformerInfo
    {
        /// <summary>
        /// 班组员工组员工列表
        /// </summary>
        public List<ShiftEmployeeInfo> ShiftEmployeeInfos { get; } = new List<ShiftEmployeeInfo>();

        /// <summary>
        /// 对象信息列表
        /// </summary>
        public List<AdoInfo> AdoInfos { get; } = new List<AdoInfo>();

        /// <summary>
        /// 选中对象信息列表
        /// </summary>
        public List<AdoInfo> SelectedAdoInfos { get; } = new List<AdoInfo>();

        /// <summary>
        /// 选择两笔或者两笔以上任务单，判断是否已选对象
        /// </summary>
        public bool IsSelectedTaskPerformer { get; set; }
    }

    /// <summary>
    /// 班组员工类
    /// </summary>
    [Serializable]
    public class ShiftEmployeeInfo
    {
        /// <summary>
        /// 对象值
        /// </summary>
        public string AdoValue { get; set; }

        /// <summary>
        /// 对象名称
        /// </summary>
        public string AdoName { get; set; }
    }

    /// <summary>
    /// 对象信息
    /// </summary>
    [Serializable]
    public class AdoInfo
    {
        /// <summary>
        /// 所有勾选的任务单Id列表
        /// </summary>
        public List<double> SelectedTaskIds { get; set; } = new List<double>();

        /// <summary>
        /// 当前选中派工任务Id
        /// </summary>
        public double DispatchTaskId { get; set; }

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 对象Id
        /// </summary>
        public double AdoId { get; set; }

        /// <summary>
        /// 对象名称
        /// </summary>
        public string AdoName { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public string AdoType { get; set; }

        /// <summary>
        /// 员工类型(只针对员工对象，区分员工是否属于员工组或班组)
        /// </summary>
        public string AdoGroup { get; set; }

        /// <summary>
        /// 已派任务单数
        /// </summary>
        public double SendQty { get; set; }

        /// <summary>
        /// 工序技能匹配度
        /// </summary>
        public decimal MatchDegree { get; set; }

        /// <summary>
        /// 指定设备
        /// </summary>
        public string DispatchEquipment { get; set; }

        /// <summary>
        /// 是否新增或删除
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Disable { get; set; }
    }

    /// <summary>
    /// 班组员工组员工信息
    /// </summary>
    [Serializable]
    public class WorkEmployeeGroup
    {
        /// <summary>
        /// 班组员工组员工id
        /// </summary>
        public double WorkEmployeeId { get; set; }

        /// <summary>
        /// 已派任务单数
        /// </summary>
        public double SendQty { get; set; }
    }
}
