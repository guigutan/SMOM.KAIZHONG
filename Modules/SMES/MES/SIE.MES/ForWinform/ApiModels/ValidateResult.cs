using SIE.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{


    /// <summary>
    /// 校验结果
    /// </summary>
    [Serializable]
    public class ValidateResult
    {
        /// <summary>
        /// 工单信息
        /// </summary>
        public WorkOrderInfo WorkOrderInfo { get; set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public CollectionContext Context { get; set; }
    }
}
