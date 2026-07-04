using SIE.ObjectModel;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 优先级
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Label("普通")]
        Normal,

        /// <summary>
        /// 紧急
        /// </summary>
        [Label("紧急")]
        Urgency,
    }
}