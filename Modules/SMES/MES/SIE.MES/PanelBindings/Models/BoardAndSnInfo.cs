using System;

namespace SIE.MES.PanelBindings.Models
{
    /// <summary>
    /// 板号与SN对应信息
    /// </summary>
    [Serializable]
    public class BoardAndSnInfo
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 板号
        /// </summary>
        public int BoardNo { get; set; }
    }
}