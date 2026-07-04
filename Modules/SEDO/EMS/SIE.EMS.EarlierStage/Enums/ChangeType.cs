using SIE.ObjectModel;

namespace SIE.EMS.EarlierStage.Enums
{
    /// <summary>
    /// 变更内容
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// 项目预算
        /// </summary>
        [Label("项目预算")]
        Amount = 10,
        /// <summary>
        /// 关键事项
        /// </summary>
        [Label("关键事项")]
        KeyItem = 20,
        /// <summary>
        /// 项目成员
        /// </summary>
        [Label("项目成员")]
        Member = 30,
        /// <summary>
        /// 项目计划
        /// </summary>
        [Label("项目计划")]
        WorkItem = 40,
        /// <summary>
        /// 项目状态
        /// </summary>
        [Label("项目状态")]
        ProjectStatus = 50
    }
}
