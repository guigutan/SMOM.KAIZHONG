using System;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具类型
    /// </summary>
    [Serializable]
    public class FixtureTypeData
    {
        /// <summary>
        /// 工治具类型ID
        /// </summary>
        public double TypeId { get; set; }

        /// <summary>
        /// 类型编号
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
    }
}