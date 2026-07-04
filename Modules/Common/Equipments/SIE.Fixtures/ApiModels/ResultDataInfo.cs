using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 结果信息
    /// </summary>
    [Serializable]
    public class ResultDataInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否闲置
        /// </summary>
        public bool IsRelax { get; set; }
    }
}
