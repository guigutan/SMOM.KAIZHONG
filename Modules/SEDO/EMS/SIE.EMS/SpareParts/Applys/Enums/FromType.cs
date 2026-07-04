using SIE.ObjectModel;

namespace SIE.EMS.SpareParts.Applys.Enums
{
    /// <summary>
    /// 来源类型
    /// </summary>
    [Label("来源类型")]
    public enum FromType
    {
        /// <summary>
        /// 手工创建
        /// </summary>
        [Label("手工创建")]
        Hand = 0,
        /// <summary>
        /// 点检
        /// </summary>
        [Label("点检")]
        SpotCheck = 1,
        /// <summary>
        /// 维修
        /// </summary>
        [Label("维修")]
        Upkeep = 2,
        /// <summary>
        /// 保养
        /// </summary>
        [Label("保养")]
        Maintain = 3,
        /// <summary>
        /// 安装调试
        /// </summary>
        [Label("安装调试")]
        Setup = 4,
        /// <summary>
        /// 润滑
        /// </summary>
        [Label("润滑")]
        Lubrication = 5,
    }
}
