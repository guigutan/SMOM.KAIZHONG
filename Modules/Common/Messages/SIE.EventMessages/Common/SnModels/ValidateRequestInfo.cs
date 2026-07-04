using System;

namespace SIE.EventMessages.Common.SnModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ValidateRequestInfo
    {
        /// <summary>
        /// 报检单号Id
        /// </summary>
        public double BillId {  get; set; }


        /// <summary>
        /// Sn
        /// </summary>
        public string Sn { get; set; }
    }
}
