using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 工序采集步骤
    /// </summary>
    public class EdgeCollectStep
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 步骤
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool NeedUnbound { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public int BarcodeType { get; set; }

        /// <summary>
        /// 条码类型描述
        /// </summary>
        public string BarcodeTypeDesc { get; set; }

        /// <summary>
        /// 出入站类型
        /// </summary>
        public int? PlugType { get; set; }

        /// <summary>
        /// 是否生成批次
        /// </summary>
        public bool IsGenerateBatch { get; set; }

    }
}
