using SIE.Domain;
using System;

namespace SIE.Kit.ReCheck.RecheckInspBills
{
    /// <summary>
    /// 电子套件-来料检验ReelID
    /// </summary>
    [Serializable]
    public class RecheckInspBillReelIDInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public double? Id { get; set; }

        /// <summary>
        /// ReelID  
        /// </summary>
        public string ReelID { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 持久化状态
        /// </summary>
        public PersistenceStatus PersistenceStatus { get; set; }
    }
}
