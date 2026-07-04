using System;

namespace SIE.Core.ApiModels
{
    /// <summary>
    /// 设备查询信息
    /// </summary>
    [Serializable]
    public class EquipQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId { get; set; }
    }
}