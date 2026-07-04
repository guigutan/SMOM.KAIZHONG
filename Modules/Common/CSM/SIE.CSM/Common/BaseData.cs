using System;

namespace SIE.CSM.Common
{
    /// <summary>
    /// 基础数据结构
    /// </summary>
    public class BaseData
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 类型对象，一般用于枚举返回到PDA
    /// </summary>
    public class BaseType
    {
        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 显示值
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 简单免检清单类
    /// </summary>
    public class SimpleNoIqcItem
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 是否免检
        /// </summary>
        public bool InspectionFree { get; set; }

        /// <summary>
        /// 免检开始时间
        /// </summary>
        public DateTime? EffectiveStartTime { get; set; }

        /// <summary>
        /// 免检结束时间
        /// </summary>
        public DateTime? EffectiveEndTime { get; set; }
    }
}
