using System;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 车间信息
    /// </summary>
    [Serializable]
    public class EnterpriseInfo
    {
        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; }
    }
}
