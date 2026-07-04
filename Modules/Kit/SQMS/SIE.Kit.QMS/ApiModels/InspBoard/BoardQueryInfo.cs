using System;

namespace SIE.Kit.QMS.ApiModels.InspBoard
{
    /// <summary>
    /// 看板查询信息
    /// </summary>
    [Serializable]
    public class BoardQueryInfo
    {
        /// <summary>
        /// 检验组Id
        /// </summary>
        public double? InspGroupId { get; set; }
    }
}
