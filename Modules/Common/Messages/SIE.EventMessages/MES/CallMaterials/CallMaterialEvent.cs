using System;

namespace SIE.EventMessages.CallMaterials
{
    /// <summary>
    /// 叫料信息
    /// </summary>
    [Serializable]
    public class CallMaterialEvent
    {
        /// <summary>
        /// 叫料单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 操作
        /// 0取消叫料;10设置优先级;
        /// </summary>
        public int Operation { get; set; }

        /// <summary>
        /// 优先级
        /// 0普通;1紧急
        /// </summary>
        public int Priority { get; set; }
    }
}