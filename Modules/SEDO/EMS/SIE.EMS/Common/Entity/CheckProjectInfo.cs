using System;
namespace SIE.EMS.Common.Entity
{
    /// <summary>
    /// 点检保养信息
    /// </summary>
    [Serializable]
    public class CheckProjectInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId { get; set; }
    }
}
