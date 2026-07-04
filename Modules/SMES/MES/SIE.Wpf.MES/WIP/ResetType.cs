using SIE.ObjectModel;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResetType
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Label("初始化")]
        Init = 0,

        /// <summary>
        /// 重新开始
        /// </summary>
        [Label("重新开始")]
        CollectRestart=1,

        /// <summary>
        /// 切换工作单元
        /// </summary>
        [Label("切换工作单元")]
        ChangeWorkStation = 2,

        /// <summary>
        /// 成功
        /// </summary>
        [Label("成功")]
        Success = 3,

        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        Error = 4,

        /// <summary>
        /// 空白
        /// </summary>
        [Label("空白")]
        None = 5,
    }
}
