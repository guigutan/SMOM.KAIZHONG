using System;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 产线信息
    /// </summary>
    [Serializable]
    public class ResourceInfo
    {
        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }
    }
}
