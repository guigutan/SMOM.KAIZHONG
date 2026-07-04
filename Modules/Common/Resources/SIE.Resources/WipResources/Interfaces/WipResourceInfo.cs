using System;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 生产资源信息
    /// </summary>
    [Serializable]
    public class WipResourceInfo
    {
        /// <summary>
        /// 生产资源ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }
    }
}