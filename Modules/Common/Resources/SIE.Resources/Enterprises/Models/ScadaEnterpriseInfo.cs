using SIE.Core.ApiModels;
using System;

namespace SIE.Resources.Enterprises.Models
{
    /// <summary>
    /// SCADA企业模型
    /// </summary>
    [Serializable]
    public class ScadaEnterpriseInfo : BaseDataInfo
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId { get; set; }
    }
}