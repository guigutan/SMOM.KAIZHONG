using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP.Entity;

namespace SIE.XPCJ.Models.WIP
{
    public class ProcessCollectStep
    {
        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool IsUnbound
        {
            get;
            set;
        }

        /// <summary>
        /// 步骤
        /// </summary>
        public int Step
        {
            get;
            set;
        }


        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get;
            set;
        }

        /// <summary>
        /// 出入站类型
        /// </summary>
        public PlugType? PlugType
        {
            get;
            set;
        }

        /// <summary>
        /// 是否生成批次
        /// </summary>
        public bool IsGenerateBatch
        {
            get;
            set;
        }
        
        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get;
            set;
        }
        public Process Process
        {
            get;
            set;
        }
    }
}
