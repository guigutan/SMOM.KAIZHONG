using System;

namespace SIE.EventMessages.PDCA
{
    /// <summary>
    /// 查看检验单接口参数
    /// </summary>
    [Serializable]
    public class ViewInspBillEventArgs
    {
        /// <summary>
        /// 检验类型枚举对应值，整型格式
        /// </summary>
        public int InspectionTypeInt { get; set; }

        /// <summary>
        /// 单据Id
        /// </summary>
        public double BillId { get; set; }
    }
}
