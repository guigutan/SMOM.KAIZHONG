using SIE.ObjectModel;

namespace SIE.LES.MaterialPreparations.Enums
{
    /// <summary>
    /// 备料需求单状态
    /// </summary>
    public enum PrepareStatus
    {
        /// <summary>
        /// 保存
        /// </summary>
        [Label("保存")]
        Saved = 0,

        /// <summary>
        /// 已提交
        /// </summary>
        [Label("已提交")]
        Submitted = 1,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Canceled = 2,
    }
}
