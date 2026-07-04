using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 拣货处理类型
    /// </summary>
    public enum PickProcessType
    {
        /// <summary>
        /// 混合拣货
        /// </summary>
        [Label("混合拣货")]
        MixedPick,

        /// <summary>
        /// 混合拣货
        /// </summary>
        [Label("按件拣货")]
        PiecePick,

        /// <summary>
        /// 按箱拣货
        /// </summary>
        [Label("按箱拣货")]
        BoxPick,

        /// <summary>
        /// 按托拣货
        /// </summary>
        [Label("按托拣货")]
        TrayPick,
    }
}
