using System;

namespace SIE.Fixtures
{
    /// <summary>
    /// 工治具编码
    /// </summary>
    [Serializable]
    public class FixtureEncodeData
    {
        /// <summary>
        /// 工治具编码ID
        /// </summary>
        public double FixtureId { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureCode { get; set; }

        /// <summary>
        /// 工治具名称
        /// </summary>
        public string FixtureName { get; set; }

        /// <summary>
        /// 类型Id
        /// </summary>
        public double TypeId { get; set; }
    }
}