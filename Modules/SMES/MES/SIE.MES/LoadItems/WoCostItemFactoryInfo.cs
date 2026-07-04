using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 工厂资源信息
    /// </summary>
    [Serializable]
    public class FactoryInfo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string WipResourceName { get; set; }
    }
}
