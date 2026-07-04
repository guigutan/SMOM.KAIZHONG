using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具履历信息
    /// </summary>
    [Serializable]
    public class FixtureRecordInfo
    {
        /// <summary>
        /// 工治具治具编码
        /// </summary>
        public string FixtureEncodeCode { get; set; }

        /// <summary>
        /// 型号编码
        /// </summary>
        public string FixtureModelCode { get; set; }

        /// <summary>
        /// 型号名称
        /// </summary>
        public string FixtureModelName { get; set; }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtrueType { get; set; }
    }
}