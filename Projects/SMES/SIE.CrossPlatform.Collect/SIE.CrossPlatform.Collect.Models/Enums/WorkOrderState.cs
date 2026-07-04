using SIE.CrossPlatform.Collect.Models.Attributes;
using System.ComponentModel;

namespace SIE.CrossPlatform.Collect.Models.Enums
{

    /// <summary>
    /// 工单状态
    /// </summary>
    public enum WorkOrderState
    {
        /// <summary>
        /// 发放
        /// </summary>
        [Category("CallMaterial")]
        [Label("发放")]
        Release = 0,

        /// <summary>
        /// 生产中
        /// </summary>
        [Category("CallMaterial")]
        [Label("生产中")]
        Producing = 1,

        /// <summary>
        /// 完工
        /// </summary>
        [Label("完工")]
        Finish = 2,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 3,

        /// <summary>
        /// 取消发放
        /// </summary>
        [Label("取消发放")]
        CancelRelease = 4,
    }
}
