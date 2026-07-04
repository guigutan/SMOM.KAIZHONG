using System;
namespace SIE.EMS.Common.Entity
{
    /// <summary>
    /// 设备与人员权限信息
    /// </summary>
    [Serializable]
    public class DevicePurInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 设备与人员权限Id
        /// </summary>
        public double DevicePurId { get; set; }
    }
}
