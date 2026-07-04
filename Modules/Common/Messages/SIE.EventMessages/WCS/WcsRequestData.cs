using System.Collections.Generic;

namespace SIE.EventMessages.WCS
{
    /// <summary>
    /// 发送搬运指令
    /// </summary>
    public class InstructData
    {
        /// <summary>
        /// 搬运指令完成的库房编码(可能为空)
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 指令列表信息
        /// </summary>
        public List<InstructDetailData> Instructs { get; set; }
    }

    /// <summary>
    /// 取消
    /// </summary>
    public class CancelInstructData
    {
        /// <summary>
        /// 搬运指令完成的库房编码(可能为空)
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 需要取消搬运的多个最外层包装条码(垛号)
        /// </summary>
        public List<string> Barcode { get; set; } = new List<string>();
    }

    /// <summary>
    /// 单个指令消息字段
    /// </summary>
    public class InstructDetailData
    {
        /// <summary>
        /// 最外层包装条码(垛号)
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 指令优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 出入库类型:0：出库、1：入库、2：垛移位
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 垛的初始地址
        /// </summary>
        public string Start_Addr { get; set; }

        /// <summary>
        /// 垛的目的地址
        /// </summary>
        public string Dest_Addr { get; set; }
    }
}
