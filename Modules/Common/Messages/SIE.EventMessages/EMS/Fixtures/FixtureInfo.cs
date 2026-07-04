using System;

namespace SIE.EventMessages.EMS.Fixtures
{
    /// <summary>
    /// 工治具编码信息
    /// </summary>
    [Serializable]
    public class FixtureInfo
    {
        /// <summary>
        /// 工治具编码ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string Type { get; set; }
    }
}