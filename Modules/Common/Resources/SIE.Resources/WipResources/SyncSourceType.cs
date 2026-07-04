using SIE.ObjectModel;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public enum SyncSourceType
    {
        ///// <summary>
        ///// 自定义
        ///// </summary>
        //[Label("自定义")]
        //Custom,

        /// <summary>
        /// 企业模型
        /// </summary>
        [Label("企业模型")]
        Enterprise,

        /// <summary>
        /// 设备台账
        /// </summary>
        [Label("设备台账")]
        Equipment,

        /// <summary>
        /// 工作中心
        /// </summary>
        [Label("工作中心")]
        WorkCenter,
        /// <summary>
        /// 产线与安灯区域
        /// </summary>
        [Label("产线与安灯区域")]
        LineAndon,
    }
}