using System;
using System.Collections.Generic;

namespace SIE.Equipments.SMDC.Equipments.Infos
{
    /// <summary>
    /// Tag历史值信息
    /// </summary>
    [Serializable]
    public class TagHistoryValueInfo
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<TagHistoryValue> TagRecord_INT { get; set; }
    }

    /// <summary>
    /// Tag历史值
    /// </summary>
    [Serializable]
    public class TagHistoryValue
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级路径
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// QualityStamp
        /// </summary>
        public int QualityStamp { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public decimal Value { get; set; }
    }
}
