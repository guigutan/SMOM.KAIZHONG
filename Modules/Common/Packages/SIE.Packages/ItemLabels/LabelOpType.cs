using SIE.ObjectModel;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 标签操作类型
    /// </summary>
    public enum LabelOpType
    {
        /// <summary>
        /// 收货
        /// </summary>
        [Label("收货")]
        Collect,

        /// <summary>
        /// 拣货
        /// </summary>
        [Label("拣货")]
        Pick,

        /// <summary>
        /// 发货
        /// </summary>
        [Label("发货")]
        Delivery,

        /// <summary>
        /// 取消
        /// </summary>
        [Label("取消")]
        Cancel,
    }
}
