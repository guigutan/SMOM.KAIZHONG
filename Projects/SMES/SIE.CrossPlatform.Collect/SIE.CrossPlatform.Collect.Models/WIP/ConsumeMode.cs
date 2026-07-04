using SIE.CrossPlatform.Collect.Models.Attributes;
using System.ComponentModel;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    public enum ConsumeMode
    {
        /// <summary>
        /// 拉式物料
        /// </summary>
        [Label("拉式物料")]
        [Category("PullPush")]
        Pull = 0,

        /// <summary>
        /// 推式物料
        /// </summary>
        [Label("推式物料")]
        [Category("PullPush")]
        Push = 1,

        /// <summary>
        /// 储备物料
        /// </summary>
        [Label("储备物料")]
        Reserve = 2
    }
}
