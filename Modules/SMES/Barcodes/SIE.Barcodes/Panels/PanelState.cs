using SIE.ObjectModel;

namespace SIE.Barcodes.Panels
{
    /// <summary>
    /// 拼板码状态
    /// </summary>
    public enum PanelState
    {
        /// <summary>
        /// 未打印
        /// </summary>
        [Label("未打印")]
        ToPrint,

        /// <summary>
        /// 已打印
        /// </summary>
        [Label("已打印")]
        Printed,
    }
}