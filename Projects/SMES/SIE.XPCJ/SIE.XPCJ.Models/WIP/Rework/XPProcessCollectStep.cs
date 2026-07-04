using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 采集步骤
    /// </summary>
    [Serializable]
    public class XPProcessCollectStep
    {
        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool IsUnbound { get; set; }

        /// <summary>
        /// 步骤
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType { get; set; }

        /// <summary>
        /// 出入站类型
        /// </summary>
        public PlugType? PlugType { get; set; }

        /// <summary>
        /// 是否生成批次
        /// </summary>
        public bool IsGenerateBatch { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public XPProcessCollectStep()
        { 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processCollectStep"></param>
        public XPProcessCollectStep(ProcessCollectStep processCollectStep)
        {
            this.IsUnbound = processCollectStep.IsUnbound;
            this.Step = processCollectStep.Step;
            this.BarcodeType = processCollectStep.BarcodeType;
            this.PlugType = processCollectStep.PlugType;
            this.IsGenerateBatch = processCollectStep.IsGenerateBatch;
            this.ProcessId = processCollectStep.ProcessId;
    }
    }
}
