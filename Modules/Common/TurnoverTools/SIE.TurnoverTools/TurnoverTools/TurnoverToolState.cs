using SIE.ObjectModel;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 周转工具状态
    /// </summary>
    public enum TurnoverToolState
    {
        /// <summary>
        /// 闲置
        /// </summary>
        [Label("闲置")]
        Unused = 5,
        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap = 10,
        /// <summary>
        /// 维修
        /// </summary>
        [Label("维修")]
        Repair = 15,
        /// <summary>
        /// 使用中
        /// </summary>
        [Label("使用中")]
        Inuse = 20,
    }
}