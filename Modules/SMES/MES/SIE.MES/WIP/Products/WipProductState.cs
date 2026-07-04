using SIE.ObjectModel;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品生产状态
    /// </summary>
    public enum WipProductState
    {
        /// <summary>
        /// 生产中
        /// </summary>
        [Label("生产中")]
        Producing = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Finish = 2,
    }
}