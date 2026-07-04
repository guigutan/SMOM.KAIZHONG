using SIE.Domain;
using System;

namespace SIE.Core.Common
{
    /// <summary>
    /// 实体Id/Code对象，用于简单查询
    /// </summary>
    [Serializable]
    public class EntityBaseData
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public State State { get; set; }
    }
}
