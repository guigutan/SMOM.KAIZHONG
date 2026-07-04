using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 任务单状态
    /// </summary>
    public enum DispatchTaskStatus
    {
        /// <summary>
        /// 待派工
        /// </summary>
        [Label("待派工")]
        ToDispatch = 0,

        /// <summary>
        /// 派工中
        /// </summary>
        [Label("派工中")]
        Dispatching = 10,

        /// <summary>
        /// 已派工
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("已派工")]
        Dispatched = 20,

        /// <summary>
        /// 执行中
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("执行中")]
        Executing = 30,

        /// <summary>
        /// 暂停
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("暂停")]
        Pause = 40,

        /// <summary>
        /// 已关闭
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("已关闭")]
        Closed = 50,

        /// <summary>
        /// 已完成
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("已完成")]
        Finished = 60,
    }

    /// <summary>
    /// 任务单合并状态
    /// 取消合并使用
    /// </summary>
    public enum MergedStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal = 0,

        /// <summary>
        /// 已合并
        /// </summary>
        [Label("已合并")]
        Merged = 1,

        /// <summary>
        /// 合并行
        /// </summary>
        [Label("合并行")]
        MergeRows = 2
    }

    /// <summary>
    /// IOT状态
    /// </summary>
    public enum IotStatus
    {

        /// <summary>
        /// 待开工
        /// </summary>
        [Label("待开工")]
        ToDispatch = 0,

        /// <summary>
        /// 执行中
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("执行中")]
        Executing = 30,

        /// <summary>
        /// 暂停
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("暂停")]
        Pause = 40,

        /// <summary>
        /// 已完成
        /// </summary>
        [Category("CriteriaEntity")]
        [Label("已完成")]
        Finished = 60,
    }
}