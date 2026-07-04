using SapNwRfc;
using System;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// 物料下载参数
    /// </summary>
    public class ItemParameter
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        [SapName("I_MATNR")]
        public string ItemCode { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        [SapName("I_WERKS")]
        public int InvOrgId { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [SapName("I_LAEDA")]
        public DateTime? LastUpdateTime { get; set; }
    }
}
